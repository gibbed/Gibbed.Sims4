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

namespace Gibbed.Sims4.TextureCompress
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

            var fullTransparentAlpha = new byte[] { 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var fullTransparentColor = new byte[] { 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            var fullOpaqueAlpha = new byte[] { 0x00, 0x05, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

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
                            output.Write(fullTransparentAlpha, 0, 8);
                            output.Write(fullTransparentColor, 0, 8);
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
                                output.Write(fullOpaqueAlpha, 0, 8);
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
                for (int mipIndex = 0; mipIndex < header.MipMapCount; mipIndex++)
                {
                    mipHeaders[mipIndex] = new MipHeader()
                    {
                        CommandOffset = (int)commandData.Length,
                        Offset2 = (int)block2Data.Length,
                        Offset3 = (int)block3Data.Length,
                        Offset0 = (int)block0Data.Length,
                        Offset1 = (int)block1Data.Length,
                    };

                    var mipWidth = Math.Max(4, header.Width >> mipIndex);
                    var mipHeight = Math.Max(4, header.Height >> mipIndex);
                    var mipDepth = Math.Max(1, header.Depth >> mipIndex);

                    var mipSize = Math.Max(1, (mipWidth + 3) / 4) * Math.Max(1, (mipHeight + 3) / 4) * 16;
                    var mipData = input.ReadBytes(mipSize);

                    for (int offset = 0; offset < mipSize;)
                    {
                        ushort transparentCount = 0;
                        while (transparentCount < 0x3FFF &&
                               offset < mipSize &&
                               TestAlphaAny(mipData, offset, a => a != 0) == false)
                        {
                            transparentCount++;
                            offset += 16;
                        }

                        if (transparentCount > 0)
                        {
                            transparentCount <<= 2;
                            transparentCount |= 0;
                            commandData.WriteValueU16(transparentCount, endian);
                            continue;
                        }

                        var opaqueOffset = offset;
                        ushort opaqueCount = 0;
                        while (opaqueCount < 0x3FFF &&
                               offset < mipSize &&
                               TestAlphaAll(mipData, offset, a => a == 0xFF) == true)
                        {
                            opaqueCount++;
                            offset += 16;
                        }

                        if (opaqueCount > 0)
                        {
                            for (int i = 0; i < opaqueCount; i++, opaqueOffset += 16)
                            {
                                block2Data.Write(mipData, opaqueOffset + 8, 4);
                                block3Data.Write(mipData, opaqueOffset + 12, 4);
                            }

                            opaqueCount <<= 2;
                            opaqueCount |= 2;
                            commandData.WriteValueU16(opaqueCount, endian);
                            continue;
                        }

                        var translucentOffset = offset;
                        ushort translucentCount = 0;
                        while (translucentCount < 0x3FFF &&
                               offset < mipSize &&
                               TestAlphaAny(mipData, offset, a => a != 0) == true &&
                               TestAlphaAll(mipData, offset, a => a == 0xFF) == false)
                        {
                            translucentCount++;
                            offset += 16;
                        }

                        if (translucentCount > 0)
                        {
                            for (int i = 0; i < translucentCount; i++, translucentOffset += 16)
                            {
                                block0Data.Write(mipData, translucentOffset + 0, 2);
                                block1Data.Write(mipData, translucentOffset + 2, 6);
                                block2Data.Write(mipData, translucentOffset + 8, 4);
                                block3Data.Write(mipData, translucentOffset + 12, 4);
                            }

                            translucentCount <<= 2;
                            translucentCount |= 1;
                            commandData.WriteValueU16(translucentCount, endian);
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

        // http://msdn.microsoft.com/en-us/library/windows/desktop/bb206238(v=vs.85).aspx#THREE_BIT_LINEAR_ALPHA_INTERPOLATION

        private static unsafe void UnpackAlpha(byte[] array, int offset, byte* alpha, out ulong bits)
        {
            alpha[0] = array[offset + 0];
            alpha[1] = array[offset + 1];

            if (alpha[0] > alpha[1])
            {
                alpha[2] = (byte)((6 * alpha[0] + 1 * alpha[1] + 3) / 7);
                alpha[3] = (byte)((5 * alpha[0] + 2 * alpha[1] + 3) / 7);
                alpha[4] = (byte)((4 * alpha[0] + 3 * alpha[1] + 3) / 7);
                alpha[5] = (byte)((3 * alpha[0] + 4 * alpha[1] + 3) / 7);
                alpha[6] = (byte)((2 * alpha[0] + 5 * alpha[1] + 3) / 7);
                alpha[7] = (byte)((1 * alpha[0] + 6 * alpha[1] + 3) / 7);
            }
            else
            {
                alpha[2] = (byte)((4 * alpha[0] + 1 * alpha[1] + 2) / 5);
                alpha[3] = (byte)((3 * alpha[0] + 2 * alpha[1] + 2) / 5);
                alpha[4] = (byte)((2 * alpha[0] + 3 * alpha[1] + 2) / 5);
                alpha[5] = (byte)((1 * alpha[0] + 4 * alpha[1] + 2) / 5);
                alpha[6] = 0x00;
                alpha[7] = 0xFF;
            }

            bits = 0;
            for (int i = 7; i >= 2; i--)
            {
                bits <<= 8;
                bits |= array[offset + i];
            }
        }

        private static unsafe bool TestAlphaAll(byte[] array, int offset, Func<byte, bool> test)
        {
            var alpha = stackalloc byte[16];
            ulong bits;

            UnpackAlpha(array, offset, alpha, out bits);

            for (int i = 0; i < 16; i++)
            {
                if (test(alpha[bits & 7]) == false)
                {
                    return false;
                }

                bits >>= 3;
            }

            return true;
        }

        private static unsafe bool TestAlphaAny(byte[] array, int offset, Func<byte, bool> test)
        {
            var alpha = stackalloc byte[16];
            ulong bits;

            UnpackAlpha(array, offset, alpha, out bits);

            for (int i = 0; i < 16; i++)
            {
                if (test(alpha[bits & 7]) == true)
                {
                    return true;
                }

                bits >>= 3;
            }

            return false;
        }

        private static bool TrueForAll<T>(T[] array, int offset, int count, Predicate<T> match)
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

        private static bool TrueForAny<T>(T[] array, int offset, int count, Predicate<T> match)
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
