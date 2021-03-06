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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Gibbed.IO;
using Gibbed.Sims4.FileFormats;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using NDesk.Options;

namespace Gibbed.Sims4.Unpack
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
            bool extractUnknowns = true;
            string filterPattern = null;
            ulong? filterInstance = null;
            uint? filterType = null;
            uint? filterGroup = null;
            bool matching = true;
            bool overwriteFiles = false;
            bool verbose = false;

            var options = new OptionSet()
            {
                { "o|overwrite", "overwrite existing files", v => overwriteFiles = v != null },
                { "nu|no-unknowns", "don't extract unknown files", v => extractUnknowns = v == null },
                { "f|filter=", "only extract files using pattern", v => filterPattern = v },
                { "t|type=", "only extract files that are of type", v => filterType = ParseUInt32(v) },
                { "g|group=", "only extract files that have specified group", v => filterGroup = ParseUInt32(v) },
                { "i|instance=", "only extract files that have specified instance", v => filterInstance = ParseUInt64(v) },
                { "x|exclude", "exclude when matching rather than include", v => matching = v == null },
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
                Console.WriteLine("Usage: {0} [OPTIONS]+ input_package [output_dir]", GetExecutableName());
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            string inputPath = extras[0];
            string outputPath = extras.Count > 1 ? extras[1] : Path.ChangeExtension(inputPath, null) + "_unpack";

            inputPath = Path.GetFullPath(inputPath);
            outputPath = Path.GetFullPath(outputPath);

            Regex filter = null;
            if (string.IsNullOrEmpty(filterPattern) == false)
            {
                filter = new Regex(filterPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }

            if (verbose == true)
            {
                Console.WriteLine("Loading project...");
            }

            var manager = ProjectData.Manager.Load();
            if (manager.ActiveProject == null)
            {
                Console.WriteLine("Warning: no active project loaded.");
            }

            var typeLookup = new PackageTypeLookup();
            if (manager.ActiveProject != null &&
                string.IsNullOrEmpty(manager.ActiveProject.ListsPath) == false)
            {
                var typePath = Path.Combine(manager.ActiveProject.ListsPath, "package types.xml");
                if (File.Exists(typePath) == true)
                {
                    using (var input = File.OpenRead(typePath))
                    {
                        var serializer = new XmlSerializer(typeof(PackageTypeLookup));
                        typeLookup = (PackageTypeLookup)serializer.Deserialize(input);
                    }
                }
            }

            var groupLookup = new PackageGroupLookup();
            if (manager.ActiveProject != null &&
                string.IsNullOrEmpty(manager.ActiveProject.ListsPath) == false)
            {
                var groupPath = Path.Combine(manager.ActiveProject.ListsPath, "package groups.xml");
                if (File.Exists(groupPath) == true)
                {
                    using (var input = File.OpenRead(groupPath))
                    {
                        var serializer = new XmlSerializer(typeof(PackageGroupLookup));
                        groupLookup = (PackageGroupLookup)serializer.Deserialize(input);
                    }
                }
            }

            if (verbose == true)
            {
                Console.WriteLine("Reading package...");
            }

            var instance32Hashes = manager.LoadListsInstance32Names();
            var instance64Hashes = manager.LoadListsInstance64Names();

            using (var input = File.OpenRead(inputPath))
            {
                var dbpf = new DatabasePackedFile();
                dbpf.Read(input);

                var settings = new XmlWriterSettings()
                {
                    Indent = true,
                };

                Directory.CreateDirectory(outputPath);

                var xmlPath = Path.Combine(outputPath, "files.xml");
                using (var xml = XmlWriter.Create(xmlPath, settings))
                {
                    xml.WriteStartDocument();
                    xml.WriteStartElement("files");

                    if (dbpf.Entries.Count > 0)
                    {
                        if (verbose == true)
                        {
                            Console.WriteLine("Unpacking files...");
                        }

                        long current = 0;
                        long total = dbpf.Entries.Count;
                        var padding = total.ToString(CultureInfo.InvariantCulture).Length;

                        foreach (var entry in dbpf.Entries)
                        {
                            current++;

                            if (filterInstance != null &&
                                entry.Key.Instance != filterInstance &&
                                matching == true)
                            {
                                continue;
                            }

                            if (filterType != null &&
                                entry.Key.Type != filterType &&
                                matching == true)
                            {
                                continue;
                            }

                            if (filterGroup != null &&
                                entry.Key.Group != filterGroup &&
                                matching == true)
                            {
                                continue;
                            }

                            var typeInfo = typeLookup.GetTypeInfo(entry.Key.Type);

                            bool isUnknown = false;
                            string entryName;

                            if (instance32Hashes.Contains(entry.Key.Instance) == true)
                            {
                                entryName = instance32Hashes[entry.Key.Instance];
                            }
                            else if (instance64Hashes.Contains(entry.Key.Instance) == true)
                            {
                                entryName = instance64Hashes[entry.Key.Instance];
                            }
                            else
                            {
                                if (extractUnknowns == false)
                                {
                                    continue;
                                }

                                isUnknown = true;

                                if ((entry.Key.Instance & 0xFFFFFFFF00000000ul) == 0)
                                {
                                    entryName = "#" + ToHex((uint)entry.Key.Instance);
                                }
                                else
                                {
                                    entryName = "#" + ToHex(entry.Key.Instance);
                                }
                            }

                            if (typeInfo != null &&
                                string.IsNullOrEmpty(typeInfo.Extension) == false)
                            {
                                entryName += "." + typeInfo.Extension;
                            }
                            else
                            {
                                entryName += ".#" + ToHex(entry.Key.Type);
                            }

                            var groupInfo = groupLookup.GetGroupInfo(entry.Key.Group);
                            if (groupInfo == null ||
                                string.IsNullOrEmpty(groupInfo.Name) == true)
                            {
                                if (entry.Key.Group == 0)
                                {
                                    entryName = Path.Combine("default", entryName);
                                }
                                else
                                {
                                    entryName = Path.Combine("__UNKNOWNGROUP", "#" + ToHex(entry.Key.Group), entryName);
                                }
                            }
                            else
                            {
                                entryName = Path.Combine(groupInfo.Name, entryName);
                            }

                            if (filter != null &&
                                filter.IsMatch(entryName) != matching)
                            {
                                continue;
                            }

                            if (isUnknown == true)
                            {
                                entryName = Path.Combine("__UNKNOWNNAME", entryName);
                            }

                            var entryPath = Path.Combine(outputPath, entryName);

                            xml.WriteStartElement("file");

                            xml.WriteAttributeString("group", ToHex(entry.Key.Group));
                            xml.WriteAttributeString("instance", ToHex(entry.Key.Instance));
                            xml.WriteAttributeString("type", ToHex(entry.Key.Type));

                            if (entry.IsCompressed == true)
                            {
                                xml.WriteAttributeString("compression", entry.CompressionScheme.ToString());
                            }

                            xml.WriteValue(GetRelativePathForFiles(xmlPath, entryPath));
                            xml.WriteEndElement();

                            if (overwriteFiles == false &&
                                File.Exists(entryPath) == true)
                            {
                                continue;
                            }

                            if (verbose == true)
                            {
                                Console.WriteLine("[{0}/{1}] {2}",
                                                  current.ToString(CultureInfo.InvariantCulture).PadLeft(padding),
                                                  total,
                                                  entryName);
                            }

                            var entryParent = Path.GetDirectoryName(entryPath);
                            if (string.IsNullOrEmpty(entryParent) == false)
                            {
                                Directory.CreateDirectory(entryParent);
                            }

                            using (var output = File.Create(entryPath))
                            {
                                input.Seek(entry.Offset, SeekOrigin.Begin);
                                if (entry.IsCompressed == true)
                                {
                                    if (entry.CompressionScheme == DatabasePackedFile.CompressionScheme.RefPack)
                                    {
                                        var compressedBytes = input.ReadBytes(entry.CompressedSize);
                                        var uncompressedBytes = RefPack.Decompression.Decompress(compressedBytes);
                                        if (uncompressedBytes.Length != entry.UncompressedSize)
                                        {
                                            throw new InvalidOperationException();
                                        }
                                        output.WriteBytes(uncompressedBytes);
                                    }
                                    else if (entry.CompressionScheme == DatabasePackedFile.CompressionScheme.Zlib)
                                    {
                                        var zlib = new InflaterInputStream(input);
                                        output.WriteFromStream(zlib, entry.UncompressedSize);
                                    }
                                    else
                                    {
                                        throw new NotSupportedException();
                                    }
                                }
                                else
                                {
                                    output.WriteFromStream(input, entry.UncompressedSize);
                                }
                            }
                        }
                    }

                    xml.WriteEndElement();
                    xml.WriteEndDocument();
                }
            }
        }

        private static uint? ParseUInt32(string text)
        {
            if (text == null)
            {
                return null;
            }

            if (text.StartsWith("0x") == false)
            {
                return text.HashFNV32();
            }

            return uint.Parse(text.Substring(2), NumberStyles.AllowHexSpecifier);
        }

        private static ulong? ParseUInt64(string text)
        {
            if (text == null)
            {
                return null;
            }

            if (text.StartsWith("0x") == false)
            {
                return text.HashFNV64();
            }

            return ulong.Parse(text.Substring(2), NumberStyles.AllowHexSpecifier);
        }

        private static string ToHex(uint value)
        {
            return value.ToString("X8", CultureInfo.InvariantCulture);
        }

        private static string ToHex(ulong value)
        {
            return value.ToString("X16", CultureInfo.InvariantCulture);
        }

        private static string GetRelativePathForFiles(string fromPath, string toPath)
        {
            var toName = Path.GetFileName(toPath);
            if (string.IsNullOrEmpty(toName) == true)
            {
                throw new InvalidOperationException();
            }

            fromPath = Path.GetDirectoryName(fromPath);
            toPath = Path.GetDirectoryName(toPath);
            var relativePath = GetRelativePath(fromPath, toPath);
            return Path.Combine(relativePath, toName);
        }

        private static string GetRelativePath(string fromPath, string toPath)
        {
            if (fromPath == null)
            {
                throw new ArgumentNullException("fromPath");
            }

            if (toPath == null)
            {
                throw new ArgumentNullException("toPath");
            }

            if (Path.IsPathRooted(fromPath) == true &&
                Path.IsPathRooted(toPath) == true)
            {
                if (string.Compare(Path.GetPathRoot(fromPath),
                                   Path.GetPathRoot(toPath),
                                   StringComparison.OrdinalIgnoreCase) != 0)
                {
                    return toPath;
                }
            }

            var relativePath = new List<string>();
            var fromDirectories = fromPath.Split(Path.DirectorySeparatorChar);
            var toDirectories = toPath.Split(Path.DirectorySeparatorChar);

            int length = Math.Min(fromDirectories.Length, toDirectories.Length);
            int lastCommonRoot = -1;

            // find common root
            for (int x = 0; x < length; x++)
            {
                if (string.Compare(fromDirectories[x],
                                   toDirectories[x],
                                   StringComparison.OrdinalIgnoreCase) != 0)
                {
                    break;
                }

                lastCommonRoot = x;
            }

            if (lastCommonRoot == -1)
            {
                return toPath;
            }

            // add relative folders in from path
            for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)
            {
                if (fromDirectories[x].Length > 0)
                {
                    relativePath.Add("..");
                }
            }

            // add to folders to path
            for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)
            {
                relativePath.Add(toDirectories[x]);
            }

            // create relative path
            return string.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture),
                               relativePath.ToArray());
        }
    }
}
