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
            string outputPath = Path.ChangeExtension(inputPath, ".dds");

            using (var input = File.OpenRead(inputPath))
            {
                if (input.Length < 16)
                {
                    throw new EndOfStreamException("file not large enough to be a DDS file");
                }

                var pixelFormat = input.ReadValueU32(Endian.Little);
                if (pixelFormat != DDS.FourCC.DXT5 &&
                    pixelFormat.Swap() != DDS.FourCC.DXT5)
                {
                    throw new FormatException("file is not a RLE2 file");
                }
                var endian = pixelFormat == DDS.FourCC.DXT5 ? Endian.Little : Endian.Big;

                var version = input.ReadValueU32(endian);
                if (version != 0x32454C52) // RLE2
                {
                    throw new FormatException("unsupported version");
                }

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
                    mipHeaders[i] = MipHeader.Read(input, endian);
                }
                var dummy = new MipHeader();
                dummy.CommandOffset = mipHeaders[mipCount - 1].OffsetB;
                dummy.OffsetB = mipHeaders[mipCount - 1].OffsetC;
                dummy.OffsetC = mipHeaders[mipCount - 1].OffsetD;
                dummy.OffsetD = mipHeaders[mipCount - 1].OffsetE;
                dummy.OffsetE = (int)input.Length;
                mipHeaders[mipCount] = dummy;

                input.Position = 0;
                var temp = input.ReadBytes((int)input.Length);

                using (var output = File.Create(outputPath))
                {
                    output.WriteValueU32(DDS.Header.Signature, endian);
                    var header = new DDS.Header();
                    header.Size = DDS.Header.StructureSize;
                    header.Flags = DDS.HeaderFlags.Texture;
                    header.Width = width;
                    header.Height = height;
                    header.Depth = 1;
                    header.MipMapCount = 1; //mipCount;
                    header.PixelFormat.Size = DDS.PixelFormat.StructureSize;
                    header.PixelFormat.Flags = DDS.PixelFormatFlags.FourCC;
                    header.PixelFormat.FourCC = DDS.FourCC.DXT5;
                    header.Serialize(output, endian);

                    var emptyTexel = new byte[16];

                    for (int i = 0; i < mipCount && i < 1; i++)
                    {
                        var mipHeader = mipHeaders[i];
                        var nextMipHeader = mipHeaders[i + 1];

                        int blockOffset2, blockOffset3, blockOffset0, blockOffset1;
                        blockOffset2 = mipHeader.OffsetB;
                        blockOffset3 = mipHeader.OffsetC;
                        blockOffset0 = mipHeader.OffsetD;
                        blockOffset1 = mipHeader.OffsetE;

                        int op2s = 0;

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
                                op2s += count;

                                var localBlockOffset0 = blockOffset0 - (2 * count);
                                var localBlockOffset1 = blockOffset1 - (6 * count);

                                for (int j = 0; j < count; j++)
                                {
                                    output.WriteValueU64(0xFFFFFFFFFFFF0500ul, endian);
                                    //output.Write(temp, localBlockOffset0, 2);
                                    //output.Write(temp, localBlockOffset1, 6);
                                    output.Write(temp, blockOffset2, 4);
                                    output.Write(temp, blockOffset3, 4);

                                    blockOffset2 += 4;
                                    blockOffset3 += 4;
                                    localBlockOffset0 += 2;
                                    localBlockOffset1 += 6;
                                }
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }
                        }

                        if (blockOffset0 != nextMipHeader.OffsetD ||
                            blockOffset1 != nextMipHeader.OffsetE ||
                            blockOffset2 != nextMipHeader.OffsetB ||
                            blockOffset3 != nextMipHeader.OffsetC)
                        {
                            throw new InvalidOperationException();
                        }
                    }
                }
            }
        }
    }
}
