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
using System.Runtime.InteropServices;
using Gibbed.IO;

namespace Gibbed.Sims4.FileFormats
{
    public class DatabasePackedFile
    {
        #region Properties
        public Version Version
        {
            get { return _PackageVersion; }
            set { _PackageVersion = value; }
        }

        public List<Entry> Entries
        {
            get { return _Entries; }
        }

        public long IndexOffset { get; set; }
        public long IndexSize { get; set; }
        #endregion

        #region Fields
        private Version _PackageVersion;
        private Version _UserVersion;
        private readonly List<Entry> _Entries;
        #endregion

        public DatabasePackedFile()
        {
            this._PackageVersion = new Version();
            this._Entries = new List<Entry>();
        }

        public void Read(Stream input)
        {
            const Endian endian = Endian.Little;

            long indexCount;
            long indexSize;
            long indexOffset;

            var magic = input.ReadValueU32(endian);
            if (magic != Header.Signature)
            {
                throw new FormatException("not a database packed file");
            }
            input.Seek(-4, SeekOrigin.Current);

            var header = input.ReadStructure<Header>();
            if (header.KeyType != 3)
            {
                throw new FormatException("key type is invalid");
            }

            if (header.Flags != 0 ||
                header.TimeCreated != 0 ||
                header.TimeUpdated != 0)
            {
                throw new FormatException();
            }

            if (header.UserVersionMajor != 0 ||
                header.UserVersionMinor != 0 ||
                header.IndexType != 0 ||
                header.IndexOffsetOld != 0)
            {
                throw new FormatException();
            }

            if (header.Padding.Any(p => p != 0))
            {
                throw new FormatException();
            }

            if (header.PackageVersionMajor != 2 ||
                header.PackageVersionMinor != 1)
            {
                throw new FormatException();
            }

            this._PackageVersion = new Version(header.PackageVersionMajor, header.PackageVersionMinor);
            this._UserVersion = new Version(header.UserVersionMajor, header.UserVersionMinor);

            indexCount = header.IndexEntryCount;
            indexOffset = header.IndexOffset != 0 ? header.IndexOffset : header.IndexOffsetOld;
            indexSize = header.IndexSize;

            this._Entries.Clear();
            this.IndexOffset = indexOffset;
            this.IndexSize = indexSize;

            if (indexCount > 0)
            {
                if (indexOffset == 0)
                {
                    throw new FormatException();
                }

                input.Seek(indexOffset, SeekOrigin.Begin);

                using (var data = input.ReadToMemoryStream(indexSize))
                {
                    var globalIndexValues = data.ReadValueEnum<GlobalIndexValue>();
                    if ((globalIndexValues & GlobalIndexValue.Invalid) != 0)
                    {
                        throw new InvalidDataException("don't know how to handle this index data");
                    }

                    var hasGlobalTypeId = (globalIndexValues & GlobalIndexValue.TypeId) != 0;
                    var hasGlobalGroupId = (globalIndexValues & GlobalIndexValue.GroupId) != 0;
                    var hasGlobalInstanceIdHi = (globalIndexValues & GlobalIndexValue.UpperInstanceId) != 0;

                    uint globalTypeId = hasGlobalTypeId ? data.ReadValueU32(endian) : 0xFFFFFFFF;
                    uint globalGroupId = hasGlobalGroupId ? data.ReadValueU32(endian) : 0xFFFFFFFF;
                    uint globalInstanceIdHi = hasGlobalInstanceIdHi ? data.ReadValueU32(endian) : 0xFFFFFFFF;

                    for (int i = 0; i < indexCount; i++)
                    {
                        uint typeId = hasGlobalTypeId ? globalTypeId : data.ReadValueU32(endian);
                        uint groupId = hasGlobalGroupId ? globalGroupId : data.ReadValueU32(endian);

                        ulong instanceId = 0;
                        instanceId |= hasGlobalInstanceIdHi ? globalInstanceIdHi : data.ReadValueU32(endian);
                        instanceId <<= 32;
                        instanceId |= data.ReadValueU32();

                        var offset = data.ReadValueS32(endian);
                        var compressedSize = data.ReadValueU32(endian);
                        var uncompressedSize = data.ReadValueU32(endian);

                        CompressionScheme compressionScheme;
                        ushort flags;
                        if ((compressedSize & 0x80000000) != 0)
                        {
                            compressedSize &= ~0x80000000;
                            compressionScheme = data.ReadValueEnum<CompressionScheme>(endian);
                            flags = data.ReadValueU16(endian);
                        }
                        else
                        {
                            throw new FormatException("strange index data");
                            compressionScheme = compressedSize == uncompressedSize
                                                    ? CompressionScheme.None
                                                    : CompressionScheme.RefPack;
                            flags = 0;
                        }

                        if (compressionScheme != CompressionScheme.None &&
                            compressionScheme != CompressionScheme.Zlib &&
                            compressionScheme != CompressionScheme.RefPack)
                        {
                            throw new FormatException("bad compression flags");
                        }

                        if (flags != 1)
                        {
                            throw new FormatException();
                        }

                        this._Entries.Add(new Entry
                        {
                            Key = new ResourceKey(instanceId, typeId, groupId),
                            Offset = offset,
                            CompressedSize = compressedSize,
                            UncompressedSize = uncompressedSize,
                            CompressionScheme = compressionScheme,
                            Flags = flags,
                            IsValid = true
                        });
                    }

                    if (data.Position != data.Length)
                    {
                        throw new FormatException();
                    }
                }
            }
        }

        public void WriteHeader(Stream output, long indexOffset, uint indexSize)
        {
            var header = new Header
            {
                Magic = Header.Signature,
                PackageVersionMajor = this._PackageVersion.Major,
                PackageVersionMinor = this._PackageVersion.Minor,
                KeyType = 3,
                IndexEntryCount = (uint)this._Entries.Count,
                IndexOffset = indexOffset,
                IndexSize = indexSize
            };
            output.WriteStructure(header);
        }

        public void WriteIndex(Stream output)
        {
            if (this._Entries.Count == 0)
            {
                output.WriteValueEnum<GlobalIndexValue>(GlobalIndexValue.None);
            }
            else
            {
                bool hasGlobalTypeId = true;
                bool hasGlobalGroupId = true;
                bool hasGlobalUpperInstanceId = true;

                var globalTypeId = this._Entries[0].Key.Type;
                var globalGroupId = this._Entries[0].Key.Group;
                var globalUpperInstanceId = (uint)(this._Entries[0].Key.Instance >> 32);

                for (int i = 1; i < this._Entries.Count; i++)
                {
                    hasGlobalTypeId = hasGlobalTypeId && globalTypeId == this._Entries[i].Key.Type;
                    hasGlobalGroupId = hasGlobalGroupId && globalGroupId == this._Entries[i].Key.Group;
                    hasGlobalUpperInstanceId = hasGlobalUpperInstanceId &&
                                               globalUpperInstanceId == (uint)(this._Entries[i].Key.Instance >> 32);

                    if (hasGlobalTypeId == false &&
                        hasGlobalGroupId == false &&
                        hasGlobalUpperInstanceId == false)
                    {
                        break;
                    }
                }

                var globalIndexValues = GlobalIndexValue.None;

                if (hasGlobalTypeId == true)
                {
                    globalIndexValues |= GlobalIndexValue.TypeId;
                }

                if (hasGlobalGroupId == true)
                {
                    globalIndexValues |= GlobalIndexValue.GroupId;
                }

                if (hasGlobalUpperInstanceId == true)
                {
                    globalIndexValues |= GlobalIndexValue.UpperInstanceId;
                }

                output.WriteValueEnum<GlobalIndexValue>(globalIndexValues);

                if (hasGlobalTypeId == true)
                {
                    output.WriteValueU32(globalTypeId);
                }

                if (hasGlobalGroupId == true)
                {
                    output.WriteValueU32(globalGroupId);
                }

                if (hasGlobalUpperInstanceId == true)
                {
                    output.WriteValueU32(globalUpperInstanceId);
                }

                foreach (var entry in this._Entries)
                {
                    if (hasGlobalTypeId == false)
                    {
                        output.WriteValueU32(entry.Key.Type);
                    }

                    if (hasGlobalGroupId == false)
                    {
                        output.WriteValueU32(entry.Key.Group);
                    }

                    if (hasGlobalUpperInstanceId == false)
                    {
                        output.WriteValueU32((uint)(entry.Key.Instance >> 32));
                    }

                    output.WriteValueU32((uint)(entry.Key.Instance & 0xFFFFFFFF));
                    output.WriteValueS32((int)entry.Offset);
                    output.WriteValueU32(entry.CompressedSize);
                    output.WriteValueU32(entry.UncompressedSize);
                    output.WriteValueEnum<CompressionScheme>(entry.CompressionScheme);
                    output.WriteValueU16(entry.Flags);
                }
            }
        }

        [Flags]
        public enum GlobalIndexValue : uint
        {
            None = 0,
            TypeId = 1 << 0,
            GroupId = 1 << 1,
            UpperInstanceId = 1 << 2,
            Invalid = ~(TypeId | GroupId | UpperInstanceId),
        }

        public struct Entry
        {
            public ResourceKey Key { get; set; }
            public long Offset { get; set; }
            public uint CompressedSize { get; set; }
            public uint UncompressedSize { get; set; }
            public CompressionScheme CompressionScheme { get; set; }
            public ushort Flags { get; set; }

            public bool IsValid { get; set; }

            public bool IsCompressed
            {
                get { return this.CompressionScheme != CompressionScheme.None; }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct Header
        {
            public const uint Signature = 0x46504244; // 'DBPF'

            public uint Magic;
            public int PackageVersionMajor;
            public int PackageVersionMinor;
            public int UserVersionMajor;
            public int UserVersionMinor;
            public uint Flags;
            public uint TimeCreated;
            public uint TimeUpdated;
            public uint IndexType;
            public uint IndexEntryCount;
            public uint IndexOffsetOld;
            public uint IndexSize;
            public uint HoleEntryCount;
            public uint HoleOffsetOld;
            public uint HoleSize;
            public uint KeyType;
            public long IndexOffset;
            public long HoleOffset;
            public ushort Unknown50;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            public readonly byte[] Padding;
        }

        public enum CompressionScheme : ushort
        {
            None = 0x0000,
            Zlib = 0x5A42,
            RefPack = 0xFFFF,
        }
    }
}
