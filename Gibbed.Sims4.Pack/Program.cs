﻿/* Copyright (c) 2014 Rick (rick 'at' gibbed 'dot' us)
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

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.XPath;
using Gibbed.IO;
using Gibbed.RefPack;
using Gibbed.Sims4.FileFormats;
using NDesk.Options;

namespace Gibbed.Sims4.Pack
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            bool showHelp = false;
            bool verbose = false;
            bool avoidDuplicates = false;

            var options = new OptionSet()
            {
                { "a|avoid-duplicates", "avoid duplicates", v => avoidDuplicates = v != null },
                { "v|verbose", "be verbose", v => verbose = v != null },
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
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_dir [output_package]", GetExecutableName());
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string filesPath = extras[0];
            string filesBasePath;

            if (Directory.Exists(filesPath) == true)
            {
                filesBasePath = filesPath;
                filesPath = Path.Combine(filesBasePath, "files.xml");
            }
            else
            {
                filesBasePath = Path.GetDirectoryName(filesPath);
                filesBasePath = filesBasePath ?? "";
            }

            string outputPath = extras.Count > 1
                                    ? extras[1]
                                    : Path.ChangeExtension(filesBasePath, ".package");

            var document = new XPathDocument(filesPath);
            var navigator = document.CreateNavigator();
            var nodes = navigator.Select("/files/file");

            var filePaths = new Dictionary<ResourceKey, KeyValuePair<string, DatabasePackedFile.CompressionScheme>>();

            if (verbose == true)
            {
                Console.WriteLine("Discovering files...");
            }

            while (nodes.MoveNext())
            {
                var groupText = nodes.Current.GetAttribute("group", "");
                var instanceText = nodes.Current.GetAttribute("instance", "");
                var typeText = nodes.Current.GetAttribute("type", "");
                var compressionText = nodes.Current.GetAttribute("compression", "");

                if (string.IsNullOrWhiteSpace(groupText) == true ||
                    string.IsNullOrWhiteSpace(instanceText) == true ||
                    string.IsNullOrWhiteSpace(typeText) == true)
                {
                    throw new InvalidDataException("file missing attributes");
                }

                uint group;
                ulong instance;
                uint type;
                if (FromHex(groupText, out group) == false ||
                    FromHex(instanceText, out instance) == false ||
                    FromHex(typeText, out type) == false)
                {
                    Console.WriteLine("Failed to parse resource key [{0}, {1}, {2}]!",
                                      groupText,
                                      instanceText,
                                      typeText);
                    continue;
                }

                var key = new ResourceKey(instance, type, group);
                var compressionScheme = DatabasePackedFile.CompressionScheme.None;
                if (string.IsNullOrWhiteSpace(compressionText) == false)
                {
                    if (Enum.TryParse(compressionText, out compressionScheme) == false)
                    {
                        Console.WriteLine("Failed to parse compression scheme [{0}] for {1}!", compressionText, key);
                    }
                }

                string inputPath;
                if (Path.IsPathRooted(nodes.Current.Value) == false)
                {
                    // relative path, it should be relative to the XML file
                    inputPath = Path.Combine(filesBasePath, nodes.Current.Value);
                }
                else
                {
                    inputPath = nodes.Current.Value;
                }

                inputPath = Path.GetFullPath(inputPath);

                if (File.Exists(inputPath) == false)
                {
                    Console.WriteLine(inputPath + " does not exist!");
                    continue;
                }

                filePaths.Add(key,
                              new KeyValuePair<string, DatabasePackedFile.CompressionScheme>(
                                  inputPath,
                                  compressionScheme));
            }

            if (verbose == true)
            {
                Console.WriteLine("Writing files...");
            }

            using (var output = File.Create(outputPath))
            {
                var dbpf = new DatabasePackedFile
                {
                    Version = new Version(2, 1),
                };

                var writtenFiles = new Dictionary<string, DatabasePackedFile.Entry>();

                dbpf.WriteHeader(output, 0, 0);
                foreach (var kv in filePaths)
                {
                    var key = kv.Key;
                    var filePath = kv.Value.Key;
                    var compressionScheme = kv.Value.Value;

                    if (verbose == true)
                    {
                        Console.WriteLine("{0}", filePath);
                    }

                    if (avoidDuplicates == true &&
                        writtenFiles.ContainsKey(filePath) == true)
                    {
                        var writtenEntry = writtenFiles[filePath];

                        dbpf.Entries.Add(new DatabasePackedFile.Entry
                        {
                            Key = key,
                            CompressedSize = writtenEntry.CompressedSize,
                            UncompressedSize = writtenEntry.UncompressedSize,
                            CompressionScheme = writtenEntry.CompressionScheme,
                            Flags = writtenEntry.Flags,
                            Offset = writtenEntry.Offset,
                        });

                        continue;
                    }

                    using (var input = File.OpenRead(filePath))
                    {
                        DatabasePackedFile.Entry entry;

                        switch (compressionScheme)
                        {
                            case DatabasePackedFile.CompressionScheme.RefPack:
                            {
                                input.Position = 0;

                                byte[] compressed;
                                var success = input.RefPackCompress((int)input.Length, out compressed);

                                if (success == false || compressed.Length >= input.Length)
                                {
                                    goto case DatabasePackedFile.CompressionScheme.None;
                                }

                                long offset = output.Position;
                                output.WriteBytes(compressed);

                                entry = new DatabasePackedFile.Entry
                                {
                                    Key = key,
                                    CompressedSize = (uint)(compressed.Length) | 0x80000000,
                                    UncompressedSize = (uint)input.Length,
                                    CompressionScheme = DatabasePackedFile.CompressionScheme.RefPack,
                                    Flags = 1,
                                    Offset = offset,
                                };

                                break;
                            }

                            case DatabasePackedFile.CompressionScheme.Zlib:
                            {
                                input.Position = 0;

                                using (var temp = new MemoryStream())
                                {
                                    var zlib = new DeflaterOutputStream(temp);
                                    zlib.WriteFromStream(input, input.Length);
                                    zlib.Finish();
                                    temp.Flush();

                                    if (temp.Length >= input.Length)
                                    {
                                        goto case DatabasePackedFile.CompressionScheme.None;
                                    }

                                    temp.Position = 0;
                                    long offset = output.Position;
                                    output.WriteFromStream(temp, temp.Length);

                                    entry = new DatabasePackedFile.Entry
                                    {
                                        Key = key,
                                        CompressedSize = (uint)(temp.Length) | 0x80000000,
                                        UncompressedSize = (uint)input.Length,
                                        CompressionScheme = DatabasePackedFile.CompressionScheme.Zlib,
                                        Flags = 1,
                                        Offset = offset,
                                    };
                                }

                                break;
                            }

                            case DatabasePackedFile.CompressionScheme.None:
                            {
                                input.Position = 0;

                                long offset = output.Position;
                                output.WriteFromStream(input, (uint)input.Length);

                                entry = new DatabasePackedFile.Entry
                                {
                                    Key = key,
                                    CompressedSize = (uint)input.Length | 0x80000000,
                                    UncompressedSize = (uint)input.Length,
                                    CompressionScheme = DatabasePackedFile.CompressionScheme.None,
                                    Flags = 1,
                                    Offset = offset,
                                };

                                break;
                            }

                            default:
                            {
                                throw new NotSupportedException();
                            }
                        }

                        dbpf.Entries.Add(entry);

                        if (avoidDuplicates == true)
                        {
                            writtenFiles.Add(filePath, entry);
                        }
                    }
                }

                var endOfData = output.Position;

                dbpf.WriteIndex(output);

                var indexSize = (uint)(output.Position - endOfData);

                output.Position = 0;
                dbpf.WriteHeader(output, endOfData, indexSize);
            }
        }

        private static bool FromHex(string text, out uint value)
        {
            return uint.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        }

        private static bool FromHex(string text, out ulong value)
        {
            return ulong.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        }

        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }
    }
}
