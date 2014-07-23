/* Copyright (c) 2014 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.IO;
using NDesk.Options;
using DDS = Gibbed.Sims4.TextureFormats.DDS;

namespace Gibbed.Sims4.TextureConvert
{
    internal class Program
    {
        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public static void Main(string[] args)
        {
            bool showHelp = false;

            var options = new OptionSet()
            {
                { "h|help", "show this message and exit", v => showHelp = v != null },
            };

            List<string> extras;

            try
            {
                extras = options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("{0}: ", GetExecutableName());
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `{0} --help' for more information.", GetExecutableName());
                return;
            }

            if (extras.Count < 1 || extras.Count > 2 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+ input [output]", GetExecutableName());
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string inputPath = extras[0];

            using (var input = File.OpenRead(inputPath))
            {
                if (input.Length < 16)
                {
                    throw new EndOfStreamException("file not large enough to be a RLE or DDS file");
                }

                var magic = input.ReadValueU32(Endian.Little);
                if (magic == DDS.FourCC.DXT5 ||
                    magic.Swap() == DDS.FourCC.DXT5)
                {
                    var outputPath = extras.Count > 1 ? extras[1] : Path.ChangeExtension(inputPath, ".dds");
                    using (var output = File.Create(outputPath))
                    {
                        ConvertToDDS(input, output, magic == DDS.FourCC.DXT5 ? Endian.Little : Endian.Big);
                    }
                }
                else if (magic == DDS.Header.Signature ||
                         magic.Swap() == DDS.Header.Signature)
                {
                    var outputPath = extras.Count > 1 ? extras[1] : Path.ChangeExtension(inputPath, ".rle2");
                    using (var output = File.Create(outputPath))
                    {
                        ConvertToRLE2(input, output, magic == DDS.Header.Signature ? Endian.Little : Endian.Big);
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        private static void ConvertToDDS(Stream input, Stream output, Endian endian)
        {
            var version = input.ReadValueU32(endian);
            if (version != 0x32454C52 && // RLE2
                version != 0x53454C52) // RLES
            {
                throw new FormatException("unsupported version");
            }
            bool hasSpecular = version == 0x53454C52;

            var width = input.ReadValueU16(endian);
            var height = input.ReadValueU16(endian);
            var mipCount = input.ReadValueU16(endian);
            var unknown0E = input.ReadValueU16(endian);

            if (unknown0E != 0)
            {
                throw new FormatException();
            }

            var mipHeaders = new MipHeader[mipCount + 1];
            for (var i = 0; i < mipCount; i++)
            {
                mipHeaders[i] = new MipHeader
                {
                    CommandOffset = input.ReadValueS32(endian),
                    Offset2 = input.ReadValueS32(endian),
                    Offset3 = input.ReadValueS32(endian),
                    Offset0 = input.ReadValueS32(endian),
                    Offset1 = input.ReadValueS32(endian),
                    Offset4 = hasSpecular == false ? 0 : input.ReadValueS32(endian),
                };
            }
            mipHeaders[mipCount] = new MipHeader
            {
                CommandOffset = mipHeaders[0].Offset2,
                Offset2 = mipHeaders[0].Offset3,
                Offset3 = mipHeaders[0].Offset0,
                Offset0 = mipHeaders[0].Offset1,
                Offset1 = hasSpecular == false ? (int)input.Length : mipHeaders[0].Offset4,
                Offset4 = (int)input.Length,
            };

            input.Position = 0;
            var temp = input.ReadBytes((int)input.Length);

            output.WriteValueU32(DDS.Header.Signature, endian);
            var header = new DDS.Header()
            {
                Size = DDS.Header.StructureSize,
                Flags = DDS.HeaderFlags.Texture,
                Width = width,
                Height = height,
                Depth = 1,
                MipMapCount = mipCount,
                PixelFormat =
                {
                    Size = DDS.PixelFormat.StructureSize,
                    Flags = DDS.PixelFormatFlags.FourCC,
                    FourCC = DDS.FourCC.DXT5,
                },
            };
            header.Serialize(output, endian);

            for (int i = 0; i < mipCount; i++)
            {
                var mipHeader = mipHeaders[i];
                var nextMipHeader = mipHeaders[i + 1];

                int blockOffset2, blockOffset3, blockOffset0, blockOffset1;
                blockOffset2 = mipHeader.Offset2;
                blockOffset3 = mipHeader.Offset3;
                blockOffset0 = mipHeader.Offset0;
                blockOffset1 = mipHeader.Offset1;

                for (int commandOffset = mipHeader.CommandOffset;
                    commandOffset < nextMipHeader.CommandOffset;
                    commandOffset += 2)
                {
                    var command = BitConverter.ToUInt16(temp, commandOffset);

                    var op = command & 3;
                    var count = command >> 2;

                    if (op == 0)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            output.WriteValueU16(0);

                            output.WriteValueU16(0);
                            output.WriteValueU16(0);
                            output.WriteValueU16(0);

                            output.WriteValueU32(0);
                            output.WriteValueU32(0);
                        }
                    }
                    else if (op == 1)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            output.Write(temp, blockOffset0, 2);
                            output.Write(temp, blockOffset1, 6);
                            output.Write(temp, blockOffset2, 4);
                            output.Write(temp, blockOffset3, 4);
                            blockOffset2 += 4;
                            blockOffset3 += 4;
                            blockOffset0 += 2;
                            blockOffset1 += 6;
                        }
                    }
                    else if (op == 2)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            if (hasSpecular == false)
                            {
                                // TODO: fix me
                                output.WriteValueU64(0xFFFFFFFFFFFF0500ul, endian);
                                //output.WriteValueU64(0ul, endian);
                            }
                            else
                            {
                                output.Write(temp, blockOffset0, 2);
                                output.Write(temp, blockOffset1, 6);
                                blockOffset0 += 2;
                                blockOffset1 += 6;
                            }

                            output.Write(temp, blockOffset2, 4);
                            output.Write(temp, blockOffset3, 4);
                            blockOffset2 += 4;
                            blockOffset3 += 4;
                        }
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }

                if (blockOffset0 != nextMipHeader.Offset0 ||
                    blockOffset1 != nextMipHeader.Offset1 ||
                    blockOffset2 != nextMipHeader.Offset2 ||
                    blockOffset3 != nextMipHeader.Offset3)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private static void ConvertToRLE2(Stream input, Stream output, Endian endian)
        {
            var header = new DDS.Header();
            header.Deserialize(input, endian);

            if (header.Size != DDS.Header.StructureSize)
            {
                throw new FormatException("bad header size");
            }

            if ((header.Flags & DDS.HeaderFlags.Texture) != DDS.HeaderFlags.Texture)
            {
                throw new FormatException("header missing required texture flags");
            }

            if (header.PixelFormat.Size != DDS.PixelFormat.StructureSize)
            {
                throw new FormatException("bad pixel format size");
            }

            if (header.PixelFormat.Flags != DDS.PixelFormatFlags.FourCC)
            {
                throw new FormatException("bad pixel format flags");
            }

            if (header.PixelFormat.FourCC != DDS.FourCC.DXT5)
            {
                throw new NotSupportedException("DDS must be DXT5");
            }

            if (header.Width > ushort.MaxValue ||
                header.Height > ushort.MaxValue)
            {
                throw new NotSupportedException("texture dimensions are too large");
            }

            if (header.MipMapCount > 16)
            {
                throw new NotSupportedException("texture has too many mipmaps");
            }

            if (header.Depth != 1 &&
                header.Depth != 0)
            {
                throw new NotSupportedException();
            }

            if (header.Depth == 0)
            {
                header.Depth = 1;
            }

            output.WriteValueU32(DDS.FourCC.DXT5, endian);
            output.WriteValueU32(0x32454C52, endian);
            output.WriteValueU16((ushort)header.Width, endian);
            output.WriteValueU16((ushort)header.Height, endian);
            output.WriteValueU16((ushort)header.MipMapCount, endian);
            output.WriteValueU16(0, endian);

            var headerOffset = 16;
            var dataOffset = 16 + (20 * header.MipMapCount);

            var mipHeaders = new MipHeader[header.MipMapCount];

            using (var commandData = new MemoryStream())
            using (var block2Data = new MemoryStream())
            using (var block3Data = new MemoryStream())
            using (var block0Data = new MemoryStream())
            using (var block1Data = new MemoryStream())
            {
                for (int i = 0; i < header.MipMapCount; i++)
                {
                    mipHeaders[i] = new MipHeader()
                    {
                        CommandOffset = (int)commandData.Length,
                        Offset2 = (int)block2Data.Length,
                        Offset3 = (int)block3Data.Length,
                        Offset0 = (int)block0Data.Length,
                        Offset1 = (int)block1Data.Length,
                    };

                    var mipWidth = Math.Max(4, header.Width >> i);
                    var mipHeight = Math.Max(4, header.Height >> i);
                    var mipDepth = Math.Max(1, header.Depth >> i);

                    var mipSize = Math.Max(1, (mipWidth + 3) / 4) * Math.Max(1, (mipHeight + 3) / 4) * 16;
                    var mipData = input.ReadBytes(mipSize);

                    for (int offset = 0; offset < mipSize;)
                    {
                        ushort nullCount = 0;
                        while (nullCount < 0x3FFF &&
                               offset < mipSize &&
                               TrueForAll(mipData, offset, 16, b => b == 0) == true)
                        {
                            nullCount++;
                            offset += 16;
                        }

                        if (nullCount > 0)
                        {
                            nullCount <<= 2;
                            nullCount |= 0;
                            commandData.WriteValueU16(nullCount, endian);
                            continue;
                        }

                        var startOffset = offset;
                        ushort fullCount = 0;
                        while (fullCount < 0x3FFF &&
                               offset < mipSize &&
                               TrueForAny(mipData, offset, 16, b => b != 0) == true)
                        {
                            fullCount++;
                            offset += 16;
                        }

                        if (fullCount > 0)
                        {
                            for (int j = 0; j < fullCount; j++, startOffset += 16)
                            {
                                block0Data.Write(mipData, startOffset + 0, 2);
                                block1Data.Write(mipData, startOffset + 2, 6);
                                block2Data.Write(mipData, startOffset + 8, 4);
                                block3Data.Write(mipData, startOffset + 12, 4);
                            }

                            fullCount <<= 2;
                            fullCount |= 1;
                            commandData.WriteValueU16(fullCount, endian);
                            continue;
                        }

                        throw new NotImplementedException();
                    }
                }

                output.Position = dataOffset;

                commandData.Position = 0;
                var commandOffset = (int)output.Position;
                output.WriteFromStream(commandData, commandData.Length);

                block2Data.Position = 0;
                var block2Offset = (int)output.Position;
                output.WriteFromStream(block2Data, block2Data.Length);

                block3Data.Position = 0;
                var block3Offset = (int)output.Position;
                output.WriteFromStream(block3Data, block3Data.Length);

                block0Data.Position = 0;
                var block0Offset = (int)output.Position;
                output.WriteFromStream(block0Data, block0Data.Length);

                block1Data.Position = 0;
                var block1Offset = (int)output.Position;
                output.WriteFromStream(block1Data, block1Data.Length);

                output.Position = headerOffset;
                for (int i = 0; i < header.MipMapCount; i++)
                {
                    var mipHeader = mipHeaders[i];
                    output.WriteValueS32(mipHeader.CommandOffset + commandOffset, endian);
                    output.WriteValueS32(mipHeader.Offset2 + block2Offset, endian);
                    output.WriteValueS32(mipHeader.Offset3 + block3Offset, endian);
                    output.WriteValueS32(mipHeader.Offset0 + block0Offset, endian);
                    output.WriteValueS32(mipHeader.Offset1 + block1Offset, endian);
                }
            }
        }

        public static bool TrueForAll<T>(T[] array, int offset, int count, Predicate<T> match)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            if (offset < 0 || offset > array.Length)
            {
                throw new IndexOutOfRangeException();
            }

            var end = offset + count;
            if (end < 0 || end > array.Length)
            {
                throw new IndexOutOfRangeException();
            }

            for (int index = offset; index < end; index++)
            {
                if (match(array[index]) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool TrueForAny<T>(T[] array, int offset, int count, Predicate<T> match)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            if (offset < 0 || offset > array.Length)
            {
                throw new IndexOutOfRangeException();
            }

            var end = offset + count;
            if (end < 0 || end > array.Length)
            {
                throw new IndexOutOfRangeException();
            }

            for (int index = offset; index < end; index++)
            {
                if (match(array[index]) == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
