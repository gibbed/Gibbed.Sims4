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

namespace Gibbed.Sims4.TextureShuffle
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
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_fat [output_dir]", GetExecutableName());
                Console.WriteLine();
                Console.WriteLine("Unpack files from a Big File (FAT/DAT pair).");
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string inputPath = extras[0];

            using (var input = File.OpenRead(inputPath))
            {
                if (input.Length < 128)
                {
                    throw new EndOfStreamException("file not large enough to be a DDS file");
                }

                var magic = input.ReadValueU32(Endian.Little);
                if (magic != DDS.Header.Signature &&
                    magic.Swap() != DDS.Header.Signature)
                {
                    throw new FormatException("file is not a DDS file");
                }
                var endian = magic == DDS.Header.Signature ? Endian.Little : Endian.Big;

                var header = new DDS.Header();
                header.Deserialize(input, endian);

                if (header.Size != DDS.Header.StructureSize)
                {
                    throw new FormatException("bad header size");
                }

                if (header.PixelFormat.Size != DDS.PixelFormat.StructureSize)
                {
                    throw new FormatException("bad pixel format size");
                }

                if ((header.Flags & DDS.HeaderFlags.Texture) != DDS.HeaderFlags.Texture)
                {
                    throw new FormatException("header missing required texture flags");
                }

                if (header.PixelFormat.FourCC == DDS.FourCC.DXT1 ||
                    header.PixelFormat.FourCC == DDS.FourCC.DXT3 ||
                    header.PixelFormat.FourCC == DDS.FourCC.DXT5)
                {
                    var outputPath = extras.Count > 1
                                         ? extras[1]
                                         : Path.ChangeExtension(inputPath, null) + "_shuffled.dds";

                    using (var output = File.Create(outputPath))
                    {
                        Shuffle(header, input, output, endian);
                    }
                }
                else if (header.PixelFormat.FourCC == DDS.FourCC.DST1 ||
                         header.PixelFormat.FourCC == DDS.FourCC.DST3 ||
                         header.PixelFormat.FourCC == DDS.FourCC.DST5)
                {
                    var outputPath = extras.Count > 1
                                         ? extras[1]
                                         : Path.ChangeExtension(inputPath, null) + "_unshuffled.dds";

                    using (var output = File.Create(outputPath))
                    {
                        Unshuffle(header, input, output, endian);
                    }
                }
                else
                {
                    Console.WriteLine("Texture does not need to be shuffled.");
                }
            }
        }

        private static void Shuffle(DDS.Header header, FileStream input, FileStream output, Endian endian)
        {
            throw new NotImplementedException();
        }

        private static void Unshuffle(DDS.Header header, FileStream input, FileStream output, Endian endian)
        {
            const int dataOffset = 128;
            var dataSize = (int)(input.Length - dataOffset);

            input.Position = dataOffset;
            var temp = input.ReadBytes(dataSize);

            if (header.PixelFormat.FourCC == DDS.FourCC.DST1)
            {
                output.WriteValueU32(DDS.Header.Signature, endian);
                header.PixelFormat.FourCC = DDS.FourCC.DXT1;
                header.Serialize(output, endian);

                var blockOffset2 = 0;
                var blockOffset3 = blockOffset2 + (dataSize >> 1);

                // probably a better way to do this
                var count = (blockOffset3 - blockOffset2) / 4;

                for (int i = 0; i < count; i++)
                {
                    output.Write(temp, blockOffset2, 4);
                    output.Write(temp, blockOffset3, 4);
                    blockOffset2 += 4;
                    blockOffset3 += 4;
                }
            }
            else if (header.PixelFormat.FourCC == 0x33545344) // DST3
            {
                output.WriteValueU32(DDS.Header.Signature, endian);
                header.PixelFormat.FourCC = DDS.FourCC.DXT3;
                header.Serialize(output, endian);

                var blockOffset0 = 0;
                var blockOffset2 = blockOffset0 + (dataSize >> 1);
                var blockOffset3 = blockOffset2 + (dataSize >> 2);

                throw new NotImplementedException("no samples");
            }
            else if (header.PixelFormat.FourCC == 0x35545344) // DST5
            {
                output.WriteValueU32(DDS.Header.Signature, endian);
                header.PixelFormat.FourCC = DDS.FourCC.DXT5;
                header.Serialize(output, endian);

                var blockOffset0 = 0;
                var blockOffset2 = blockOffset0 + (dataSize >> 3);
                var blockOffset1 = blockOffset2 + (dataSize >> 2);
                var blockOffset3 = blockOffset1 + (6 * dataSize >> 4);

                // probably a better way to do this
                var count = (blockOffset2 - blockOffset0) / 2;

                for (int i = 0; i < count; i++)
                {
                    output.Write(temp, blockOffset0, 2);
                    output.Write(temp, blockOffset1, 6);
                    output.Write(temp, blockOffset2, 4);
                    output.Write(temp, blockOffset3, 4);

                    blockOffset0 += 2;
                    blockOffset1 += 6;
                    blockOffset2 += 4;
                    blockOffset3 += 4;
                }
            }
        }
    }
}
