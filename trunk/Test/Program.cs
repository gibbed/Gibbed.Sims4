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
using System.Linq;
using Gibbed.Sims4.FileFormats;

namespace Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var paths = Directory.GetFiles(@"545AC67A", "*", SearchOption.AllDirectories);

            var unknownNames = new Dictionary<DataFile.FieldType, List<string>>();
            var unknownSizes = new Dictionary<DataFile.FieldType, List<uint>>();

            foreach (var path in paths)
            {
                Console.WriteLine(path);
                var data = new DataFile();
                using (var input = File.OpenRead(path))
                {
                    input.Position = 0;
                    data.Deserialize(input);
                }

                foreach (var structDef in data.StructureDefinitions)
                {
                    var fieldDefs = structDef.FieldDefinitions
                                             .Where(f => Enum.IsDefined(typeof(DataFile.FieldType), f.Type) == false)
                                             .ToArray();

                    foreach (var fieldDef in fieldDefs)
                    {
                        if (unknownNames.ContainsKey(fieldDef.Type) == false)
                        {
                            unknownNames[fieldDef.Type] = new List<string>();
                        }

                        if (unknownSizes.ContainsKey(fieldDef.Type) == false)
                        {
                            unknownSizes[fieldDef.Type] = new List<uint>();
                        }

                        unknownNames[fieldDef.Type].Add(fieldDef.Name);

                        uint size = fieldDef.DataOffset;

                        var nextFieldDef = structDef.FieldDefinitions
                                                    .OrderBy(f => f.DataOffset)
                                                    .FirstOrDefault(f => f.DataOffset > fieldDef.DataOffset);
                        if (nextFieldDef == null)
                        {
                            size = structDef.DataSize - size;
                        }
                        else
                        {
                            size = nextFieldDef.DataOffset - size;
                        }

                        unknownSizes[fieldDef.Type].Add(size);
                    }
                }
            }

            foreach (var type in unknownNames.Keys.Concat(unknownSizes.Keys).Distinct().OrderBy(t => t))
            {
                Console.WriteLine("Unknown '{0}':", type);
                if (unknownNames.ContainsKey(type) == true)
                {
                    Console.WriteLine("  field names: {0}",
                                      string.Join(", ",
                                                  unknownNames[type].Distinct()
                                                                    .OrderBy(n => n)
                                                                    .Select(n => "\"" + n + "\"")
                                                                    .ToArray()));
                }
                if (unknownSizes.ContainsKey(type) == true)
                {
                    Console.WriteLine("  field sizes: {0}",
                                      string.Join(", ",
                                                  unknownSizes[type].Distinct()
                                                                    .OrderBy(n => n)
                                                                    .Select(n => n.ToString())
                                                                    .ToArray()));
                }
            }
        }
    }
}
