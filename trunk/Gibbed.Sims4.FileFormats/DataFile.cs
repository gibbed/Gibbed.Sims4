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
using System.Text;
using Gibbed.IO;

namespace Gibbed.Sims4.FileFormats
{
    public class DataFile
    {
        public const uint Signature = 0x41544144; // 'DATA'

        private readonly List<StructureDefinition> _StructureDefinitions;

        public DataFile()
        {
            this._StructureDefinitions = new List<StructureDefinition>();
        }

        public List<StructureDefinition> StructureDefinitions
        {
            get { return this._StructureDefinitions; }
        }

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            var endian = Endian.Little;

            var magic = input.ReadValueU32(endian);
            if (magic != Signature)
            {
                throw new FormatException();
            }

            var maybeVersion = input.ReadValueU32(endian);
            if (maybeVersion != 0x100)
            {
                throw new FormatException();
            }

            var dataTableOffset = ReadOffset(input, endian);
            var dataTableCount = input.ReadValueU32(endian);
            var structureTableOffset = ReadOffset(input, endian);
            var structureTableCount = input.ReadValueU32(endian);

            if (dataTableOffset == null)
            {
                throw new FormatException();
            }

            this._StructureDefinitions.Clear();
            if (structureTableCount > 0)
            {
                if (structureTableOffset == null)
                {
                    throw new FormatException();
                }

                SeekOffset(input, structureTableOffset);

                var rawStructureOffsets = new long[structureTableCount];
                var rawStructureDefs = new RawStructureDefinition[structureTableCount];
                var rawStructureFieldDefs = new RawFieldDefinition[structureTableCount][];

                for (uint i = 0; i < structureTableCount; i++)
                {
                    rawStructureOffsets[i] = input.Position;
                    rawStructureDefs[i] = RawStructureDefinition.Read(input, endian);
                    rawStructureFieldDefs[i] = new RawFieldDefinition[rawStructureDefs[i].FieldTableCount];
                }

                for (uint i = 0; i < structureTableCount; i++)
                {
                    if (rawStructureDefs[i].FieldTableCount > 0)
                    {
                        SeekOffset(input, rawStructureDefs[i].FieldTableOffset);
                        for (uint j = 0; j < rawStructureDefs[i].FieldTableCount; j++)
                        {
                            rawStructureFieldDefs[i][j] = RawFieldDefinition.Read(input, endian);
                        }
                    }
                }

                var structureDefs = new List<StructureDefinition>();
                for (uint i = 0; i < structureTableCount; i++)
                {
                    var structureDef = new StructureDefinition();
                    structureDef.OriginalOffset = rawStructureOffsets[i];

                    var rawStructureDef = rawStructureDefs[i];
                    var rawFieldDefs = rawStructureFieldDefs[i];

                    if (rawStructureDef.NameOffset == null)
                    {
                        structureDef.Name = null;
                    }
                    else
                    {
                        SeekOffset(input, rawStructureDef.NameOffset);
                        structureDef.Name = input.ReadStringZ(Encoding.UTF8);
                    }

                    structureDef.DataSize = rawStructureDef.DataSize;

                    for (uint j = 0; j < rawStructureDef.FieldTableCount; j++)
                    {
                        var fieldDef = new FieldDefinition();
                        var rawFieldDef = rawFieldDefs[j];

                        if (rawFieldDef.NameOffset == null)
                        {
                            fieldDef.Name = null;
                        }
                        else
                        {
                            SeekOffset(input, rawFieldDef.NameOffset);
                            fieldDef.Name = input.ReadStringZ(Encoding.UTF8);
                        }

                        fieldDef.Type = rawFieldDef.Type;
                        fieldDef.DataOffset = rawFieldDef.DataOffset;
                        structureDef.FieldDefinitions.Add(fieldDef);

                        if (rawFieldDef.Unknown10Offset != null)
                        {
                            throw new NotSupportedException();
                        }
                    }

                    structureDefs.Add(structureDef);
                }

                this._StructureDefinitions.AddRange(structureDefs);
            }
        }

        public enum FieldType : uint
        {
        }

        public class StructureDefinition
        {
            private long _OriginalOffset;
            private string _Name;
            private uint _DataSize;
            private readonly List<FieldDefinition> _FieldDefinitions;

            public StructureDefinition()
            {
                this._FieldDefinitions = new List<FieldDefinition>();
            }

            public long OriginalOffset
            {
                get { return this._OriginalOffset; }
                set { this._OriginalOffset = value; }
            }

            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }

            public uint DataSize
            {
                get { return _DataSize; }
                set { _DataSize = value; }
            }

            public List<FieldDefinition> FieldDefinitions
            {
                get { return _FieldDefinitions; }
            }
        }

        public class FieldDefinition
        {
            private string _Name;
            private FieldType _Type;
            private uint _DataOffset;

            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }

            public FieldType Type
            {
                get { return _Type; }
                set { _Type = value; }
            }

            public uint DataOffset
            {
                get { return _DataOffset; }
                set { _DataOffset = value; }
            }
        }

        private struct RawStructureDefinition
        {
            public long? NameOffset;
            public uint NameHash;
            public uint Unknown08;
            public uint DataSize;
            public long? FieldTableOffset;
            public uint FieldTableCount;

            public static RawStructureDefinition Read(Stream input, Endian endian)
            {
                var nameOffset = ReadOffset(input, endian);
                var nameHash = input.ReadValueU32(endian);
                var unknown08 = input.ReadValueU32(endian);
                var dataSize = input.ReadValueU32(endian);
                var fieldTableOffset = ReadOffset(input, endian);
                var fieldTableCount = input.ReadValueU32(endian);

                return new RawStructureDefinition()
                {
                    NameOffset = nameOffset,
                    NameHash = nameHash,
                    Unknown08 = unknown08,
                    DataSize = dataSize,
                    FieldTableOffset = fieldTableOffset,
                    FieldTableCount = fieldTableCount,
                };
            }

            public void Write(Stream output, Endian endian)
            {
                throw new NotImplementedException();
            }
        }

        private struct RawFieldDefinition
        {
            public long? NameOffset;
            public uint NameHash;
            public FieldType Type;
            public uint DataOffset;
            public long? Unknown10Offset;

            public static RawFieldDefinition Read(Stream input, Endian endian)
            {
                var nameOffset = ReadOffset(input, endian);
                var nameHash = input.ReadValueU32(endian);
                var type = input.ReadValueEnum<FieldType>(endian);
                var dataOffset = input.ReadValueU32(endian);
                var unknown10Offset = ReadOffset(input, endian);

                return new RawFieldDefinition()
                {
                    NameOffset = nameOffset,
                    NameHash = nameHash,
                    Type = type,
                    DataOffset = dataOffset,
                    Unknown10Offset = unknown10Offset,
                };
            }

            public void Write(Stream output, Endian endian)
            {
                throw new NotImplementedException();
            }
        }

        private static void SeekOffset(Stream stream, long? offset)
        {
            if (offset == null)
            {
                throw new InvalidOperationException();
            }

            stream.Position = offset.Value;
        }

        private static long? ReadOffset(Stream input, Endian endian)
        {
            var position = input.Position;
            var offset = input.ReadValueU32(endian);
            if (offset == 0x80000000)
            {
                return null;
            }
            return position + offset;
        }
    }
}
