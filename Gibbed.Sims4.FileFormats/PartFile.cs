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
using System.IO;
using Gibbed.IO;
using System.Collections.Generic;

namespace Gibbed.Sims4.FileFormats
{
    public class PartFile
    {
        #region Fields
        private readonly List<Preset> _Presets;
        private string _Name;
        private float _DisplayIndex;
        private ushort _SecondaryDisplayIndex;
        private uint _PrototypeId;
        private uint _AuralMaterialHash;
        private PartFlags _Flags;
        private ulong _ExcludePartFlags;
        private uint _ExcludeModifierRegionFlags;
        private readonly List<Tag> _Tags;
        private uint _SimoleonPrice;
        private uint _PartTitleKey;
        private uint _PartDescriptionKey;
        private bool _HasUniqueTextureSpace;
        private int _BodyType;

        private uint _AgeGender;

        private readonly List<uint> _SwatchColors;
        private ResourceKey _BuffKey;
        private ResourceKey _VariantThumbnailKey;
        private ResourceKey _NakedKey;
        private ResourceKey _ParentKey;
        private int _SortLayer;
        // ReSharper disable InconsistentNaming
        private readonly List<LevelOfDetail> _LODs;
        // ReSharper restore InconsistentNaming
        private readonly List<ResourceKey> _SlotKeys;
        private ResourceKey _DiffuseKey;
        private ResourceKey _ShadowKey;
        private byte _CompositionMethod;
        private ResourceKey _RegionMapKey;
        private readonly List<Override> _Overrides;
        private ResourceKey _NormalMapKey;
        private ResourceKey _SpecularMapKey;
        #endregion

        public PartFile()
        {
            this._Presets = new List<Preset>();
            this._Tags = new List<Tag>();
            this._SwatchColors = new List<uint>();
            this._LODs = new List<LevelOfDetail>();
            this._SlotKeys = new List<ResourceKey>();
            this._Overrides = new List<Override>();
        }

        #region Properties
        public List<Preset> Presets
        {
            get { return this._Presets; }
        }

        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        public float DisplayIndex
        {
            get { return this._DisplayIndex; }
            set { this._DisplayIndex = value; }
        }

        public ushort SecondaryDisplayIndex
        {
            get { return this._SecondaryDisplayIndex; }
            set { this._SecondaryDisplayIndex = value; }
        }

        public uint PrototypeId
        {
            get { return this._PrototypeId; }
            set { this._PrototypeId = value; }
        }

        public uint AuralMaterialHash
        {
            get { return this._AuralMaterialHash; }
            set { this._AuralMaterialHash = value; }
        }

        public PartFlags Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public ulong ExcludePartFlags
        {
            get { return this._ExcludePartFlags; }
            set { this._ExcludePartFlags = value; }
        }

        public uint ExcludeModifierRegionFlags
        {
            get { return this._ExcludeModifierRegionFlags; }
            set { this._ExcludeModifierRegionFlags = value; }
        }

        public List<Tag> Tags
        {
            get { return this._Tags; }
        }

        public uint SimoleonPrice
        {
            get { return this._SimoleonPrice; }
            set { this._SimoleonPrice = value; }
        }

        public uint PartTitleKey
        {
            get { return this._PartTitleKey; }
            set { this._PartTitleKey = value; }
        }

        public uint PartDescriptionKey
        {
            get { return this._PartDescriptionKey; }
            set { this._PartDescriptionKey = value; }
        }

        public bool HasUniqueTextureSpace
        {
            get { return this._HasUniqueTextureSpace; }
            set { this._HasUniqueTextureSpace = value; }
        }

        public int BodyType
        {
            get { return this._BodyType; }
            set { this._BodyType = value; }
        }

        public uint AgeGender
        {
            get { return this._AgeGender; }
            set { this._AgeGender = value; }
        }

        public List<uint> SwatchColors
        {
            get { return this._SwatchColors; }
        }

        public ResourceKey BuffKey
        {
            get { return this._BuffKey; }
            set { this._BuffKey = value; }
        }

        public ResourceKey VariantThumbnailKey
        {
            get { return this._VariantThumbnailKey; }
            set { this._VariantThumbnailKey = value; }
        }

        public ResourceKey NakedKey
        {
            get { return this._NakedKey; }
            set { this._NakedKey = value; }
        }

        public ResourceKey ParentKey
        {
            get { return this._ParentKey; }
            set { this._ParentKey = value; }
        }

        public int SortLayer
        {
            get { return this._SortLayer; }
            set { this._SortLayer = value; }
        }

        // ReSharper disable InconsistentNaming
        public List<LevelOfDetail> LODs
            // ReSharper restore InconsistentNaming
        {
            get { return this._LODs; }
        }

        public List<ResourceKey> SlotKeys
        {
            get { return this._SlotKeys; }
        }

        public ResourceKey DiffuseKey
        {
            get { return this._DiffuseKey; }
            set { this._DiffuseKey = value; }
        }

        public ResourceKey ShadowKey
        {
            get { return this._ShadowKey; }
            set { this._ShadowKey = value; }
        }

        public byte CompositionMethod
        {
            get { return this._CompositionMethod; }
            set { this._CompositionMethod = value; }
        }

        public ResourceKey RegionMapKey
        {
            get { return this._RegionMapKey; }
            set { this._RegionMapKey = value; }
        }

        public List<Override> Overrides
        {
            get { return this._Overrides; }
        }

        public ResourceKey NormalMapKey
        {
            get { return this._NormalMapKey; }
            set { this._NormalMapKey = value; }
        }

        public ResourceKey SpecularMapKey
        {
            get { return this._SpecularMapKey; }
            set { this._SpecularMapKey = value; }
        }
        #endregion

        public void Serialize(Stream output)
        {
            const Endian endian = Endian.Little;

            var resourceKeyTable = new ResourceKeyTable();

            output.WriteValueU32(26, endian);

            var resourceKeyTableOffsetPosition = output.Position;
            output.WriteValueU32(0, endian);

            var dataPosition = output.Position;

            output.WriteValueS32(this._Presets.Count, endian);
            foreach (var preset in this._Presets)
            {
                output.WriteValueU64(preset.ComplateId, endian);

                output.WriteValueU8((byte)preset.Parameters.Count);
                foreach (var parameter in preset.Parameters)
                {
                    output.WriteValueU32(parameter.NameId, endian);
                    output.WriteValueU8((byte)parameter.Type);

                    switch (parameter.Type)
                    {
                        case PresetParameterType.UInt32:
                        {
                            output.WriteValueU32((uint)parameter.Value, endian);
                            break;
                        }

                        case PresetParameterType.Float:
                        {
                            output.WriteValueF32((float)parameter.Value, endian);
                            break;
                        }

                        case PresetParameterType.ResourceKey:
                        {
                            output.WriteResourceKeyIGT((ResourceKey)parameter.Value, endian);
                            break;
                        }

                        case PresetParameterType.Reference:
                        {
                            output.WriteValueU32((uint)parameter.Value, endian);
                            break;
                        }

                        default:
                        {
                            throw new NotSupportedException();
                        }
                    }
                }
            }

            WriteString(output, this._Name);
            output.WriteValueF32(this._DisplayIndex, endian);
            output.WriteValueU16(this._SecondaryDisplayIndex, endian);
            output.WriteValueU32(this._PrototypeId, endian);
            output.WriteValueU32(this._AuralMaterialHash, endian);
            output.WriteValueU8((byte)this._Flags);
            output.WriteValueU64(this._ExcludePartFlags, endian);
            output.WriteValueU32(this._ExcludeModifierRegionFlags, endian);

            output.WriteValueS32(this._Tags.Count, endian);
            foreach (var tag in this._Tags)
            {
                output.WriteValueU16(tag.Category, endian);
                output.WriteValueU16(tag.Value, endian);
            }

            output.WriteValueU32(this._SimoleonPrice, endian);
            output.WriteValueU32(this._PartTitleKey, endian);
            output.WriteValueU32(this._PartDescriptionKey, endian);

            output.WriteValueB8(this._HasUniqueTextureSpace);
            output.WriteValueS32(this._BodyType, endian);
            output.Seek(4, SeekOrigin.Current);
            output.WriteValueU32(this._AgeGender, endian);

            output.WriteValueU8(1);
            output.WriteValueU8(0);

            output.WriteValueU8((byte)this._SwatchColors.Count);
            foreach (var usedColor in this._SwatchColors)
            {
                output.WriteValueU32(usedColor, endian);
            }

            resourceKeyTable.WriteKey(output, this._BuffKey);
            resourceKeyTable.WriteKey(output, this._VariantThumbnailKey);
            resourceKeyTable.WriteKey(output, this._NakedKey);
            resourceKeyTable.WriteKey(output, this._ParentKey);

            output.WriteValueS32(this._SortLayer, endian);

            output.WriteValueU8((byte)this._LODs.Count);
            foreach (var lod in this._LODs)
            {
                output.WriteValueU8(lod.Level);
                output.WriteValueU32(lod.Unknown, endian);

                output.WriteValueU8((byte)lod.Assets.Count);
                foreach (var asset in lod.Assets)
                {
                    output.WriteValueS32(asset.Sorting, endian);
                    output.WriteValueS32(asset.SpecularLevel, endian);
                    output.WriteValueS32(asset.CastShadow, endian);
                }

                output.WriteValueU8((byte)lod.LODKeys.Count);
                foreach (var lodKey in lod.LODKeys)
                {
                    resourceKeyTable.WriteKey(output, lodKey);
                }
            }

            output.WriteValueU8((byte)this._SlotKeys.Count);
            foreach (var slotKey in this._SlotKeys)
            {
                resourceKeyTable.WriteKey(output, slotKey);
            }

            resourceKeyTable.WriteKey(output, this._DiffuseKey);
            resourceKeyTable.WriteKey(output, this._ShadowKey);
            output.WriteValueU8(this._CompositionMethod);

            resourceKeyTable.WriteKey(output, this._RegionMapKey);

            output.WriteValueU8((byte)this._Overrides.Count);
            foreach (var overrid in this._Overrides)
            {
                output.WriteValueU8(overrid.Region);
                output.WriteValueF32(overrid.Layer, endian);
            }

            resourceKeyTable.WriteKey(output, this._NormalMapKey);
            resourceKeyTable.WriteKey(output, this._SpecularMapKey);

            var resourceKeyTablePosition = output.Position;
            resourceKeyTable.WriteTable(output, endian);

            var endPosition = output.Position;

            output.Position = resourceKeyTableOffsetPosition;
            output.WriteValueU32((uint)(resourceKeyTablePosition - dataPosition), endian);

            output.Position = endPosition;
        }

        public void Deserialize(Stream input)
        {
            const Endian endian = Endian.Little;

            var version = input.ReadValueU32(endian);
            if (version > 26)
            {
                throw new FormatException();
            }

            this._Presets.Clear();
            this._Tags.Clear();
            this._SwatchColors.Clear();
            this._LODs.Clear();
            this._SlotKeys.Clear();
            this._Overrides.Clear();

            var resourceKeyTable = new ResourceKeyTable();
            var keyTableOffset = input.ReadValueU32(endian);
            var dataPosition = input.Position;

            input.Position += keyTableOffset;
            var keyTablePosition = input.Position;

            resourceKeyTable.ReadTable(input, endian);

            var endPosition = input.Position;
            input.Position = dataPosition;

            var presetCount = input.ReadValueU32(endian);
            for (uint i = 0; i < presetCount; i++)
            {
                var preset = new Preset();

                preset.ComplateId = input.ReadValueU64(endian);

                var parameterCount = input.ReadValueU8();
                for (int j = 0; j < parameterCount; j++)
                {
                    var nameId = input.ReadValueU32(endian);
                    var type = (PresetParameterType)input.ReadValueU8();
                    object value;

                    switch (type)
                    {
                        case PresetParameterType.UInt32:
                        {
                            value = input.ReadValueU32(endian);
                            break;
                        }

                        case PresetParameterType.Float:
                        {
                            value = input.ReadValueF32(endian);
                            break;
                        }

                        case PresetParameterType.ResourceKey:
                        {
                            value = input.ReadResourceKeyIGT(endian);
                            break;
                        }

                        case PresetParameterType.Reference:
                        {
                            value = input.ReadValueU32(endian);
                            break;
                        }

                        default:
                        {
                            throw new NotSupportedException();
                        }
                    }

                    preset.Parameters.Add(new PresetParameter(nameId, type, value));
                }

                this._Presets.Add(preset);
            }

            this._Name = ReadString(input);
            this._DisplayIndex = input.ReadValueF32(endian);

            if (version >= 13 && version <= 22)
            {
                input.Seek(1, SeekOrigin.Current); // input.ReadValueU8();
            }

            if (version >= 13)
            {
                this._SecondaryDisplayIndex = input.ReadValueU16(endian);
                this._PrototypeId = input.ReadValueU32(endian);
            }

            if (version >= 15)
            {
                this._AuralMaterialHash = input.ReadValueU32(endian);
                this._Flags = (PartFlags)input.ReadValueU8();
            }

            if (version >= 24)
            {
                this._ExcludePartFlags = input.ReadValueU64(endian);
            }

            if (version >= 25)
            {
                this._ExcludeModifierRegionFlags = input.ReadValueU32(endian);
            }

            if (version >= 16)
            {
                var tagCount = input.ReadValueU32(endian);
                for (uint i = 0; i < tagCount; i++)
                {
                    var tagCategory = input.ReadValueU16(endian);
                    var tagValue = input.ReadValueU16(endian);
                    this._Tags.Add(new Tag(tagCategory, tagValue));
                }
            }

            if (version >= 17)
            {
                this._SimoleonPrice = input.ReadValueU32(endian);
            }

            if (version >= 18)
            {
                this._PartTitleKey = input.ReadValueU32(endian);
                this._PartDescriptionKey = input.ReadValueU32(endian);
            }

            this._HasUniqueTextureSpace = input.ReadValueB8();
            this._BodyType = input.ReadValueS32(endian);
            input.Seek(4, SeekOrigin.Current); // input.ReadValueS32(endian);
            this._AgeGender = input.ReadValueU32(endian);

            if (version >= 14)
            {
                var unused2 = input.ReadValueU8();
                if (unused2 != 0)
                {
                    input.Seek(1, SeekOrigin.Current); // input.ReadValueU8();
                }
            }

            if (version >= 20)
            {
                var usedColorCount = input.ReadValueU8();
                for (int i = 0; i < usedColorCount; i++)
                {
                    var rawData = input.ReadValueU32(endian);
                    this._SwatchColors.Add(rawData);
                }
            }

            if (version >= 21)
            {
                this._BuffKey = resourceKeyTable.ReadKey(input);
            }

            if (version >= 26)
            {
                this._VariantThumbnailKey = resourceKeyTable.ReadKey(input);
            }

            if (version <= 22)
            {
                input.Seek(4, SeekOrigin.Current); // input.ReadValueU32(endian);
            }

            this._NakedKey = resourceKeyTable.ReadKey(input);
            this._ParentKey = resourceKeyTable.ReadKey(input);
            this._SortLayer = input.ReadValueS32(endian);

            if (version <= 2)
            {
                this._SortLayer *= 1000;
            }

            var lodCount = input.ReadValueU8();
            for (int i = 0; i < lodCount; i++)
            {
                var lod = new LevelOfDetail();
                lod.Level = input.ReadValueU8();
                lod.Unknown = input.ReadValueU32(endian);

                var assetCount = input.ReadValueU8();
                for (int j = 0; j < assetCount; j++)
                {
                    var sorting = input.ReadValueS32(endian);
                    var specularLevel = input.ReadValueS32(endian);
                    var castShadow = input.ReadValueS32(endian);
                    lod.Assets.Add(new LevelOfDetailAsset(sorting, specularLevel, castShadow));
                }

                if (version >= 7)
                {
                    byte lodKeyCount = version >= 10 ? input.ReadValueU8() : (byte)1;
                    for (int j = 0; j < lodKeyCount; j++)
                    {
                        var lodKey = resourceKeyTable.ReadKey(input);
                        lod.LODKeys.Add(lodKey);
                    }
                }

                this._LODs.Add(lod);
            }

            var slotKeyCount = input.ReadValueU8();
            for (int i = 0; i < slotKeyCount; i++)
            {
                var slotKey = resourceKeyTable.ReadKey(input);
                this._SlotKeys.Add(slotKey);
            }

            if (version >= 2 && version <= 6)
            {
                /*
                for (int i = 0; i < lodCount; i++)
                {
                    resourceKeyTable.ReadKey(input);
                }
                */

                throw new NotImplementedException();
            }

            if (version >= 4)
            {
                this._DiffuseKey = resourceKeyTable.ReadKey(input);
            }

            if (version >= 6)
            {
                this._ShadowKey = resourceKeyTable.ReadKey(input);
            }

            if (version >= 5 && version <= 14)
            {
                this._AuralMaterialHash = input.ReadValueU32(endian);
            }

            if (version >= 8)
            {
                this._CompositionMethod = input.ReadValueU8();
            }

            if (version >= 11)
            {
                this._RegionMapKey = resourceKeyTable.ReadKey(input);
            }

            if (version >= 12)
            {
                var overrideCount = input.ReadValueU8();
                for (int i = 0; i < overrideCount; i++)
                {
                    var region = input.ReadValueU8();
                    var layer = input.ReadValueF32(endian);
                    this._Overrides.Add(new Override(region, layer));
                }
            }

            if (version == 19)
            {
                var usedColorCount = input.ReadValueU8();
                for (int i = 0; i < usedColorCount; i++)
                {
                    this._SwatchColors.Add(input.ReadValueU32(endian));
                }
            }

            if (version >= 22)
            {
                this._NormalMapKey = resourceKeyTable.ReadKey(input);
                this._SpecularMapKey = resourceKeyTable.ReadKey(input);
            }

            if (input.Position != keyTablePosition)
            {
                throw new FormatException();
            }

            input.Position = endPosition;
        }

        public class Preset
        {
            private ulong _ComplateId;
            private readonly List<PresetParameter> _Parameters;

            public Preset()
            {
                this._Parameters = new List<PresetParameter>();
            }

            public ulong ComplateId
            {
                get { return this._ComplateId; }
                set { this._ComplateId = value; }
            }

            public List<PresetParameter> Parameters
            {
                get { return this._Parameters; }
            }
        }

        public struct PresetParameter
        {
            public readonly uint NameId;
            public readonly PresetParameterType Type;
            public readonly object Value;

            public PresetParameter(uint nameId, PresetParameterType type, object value)
            {
                this.NameId = nameId;
                this.Type = type;
                this.Value = value;
            }
        }

        public enum PresetParameterType : byte
        {
            UInt32 = 1,
            Float = 2,
            ResourceKey = 3,
            Reference = 4,
        }

        public struct Tag
        {
            public readonly ushort Category;
            public readonly ushort Value;

            public Tag(ushort category, ushort value)
            {
                this.Category = category;
                this.Value = value;
            }
        }

        public class LevelOfDetail
        {
            private byte _Level;
            private uint _Unknown;
            private readonly List<LevelOfDetailAsset> _Assets;
            private readonly List<ResourceKey> _LODKeys;

            public LevelOfDetail()
            {
                this._Assets = new List<LevelOfDetailAsset>();
                this._LODKeys = new List<ResourceKey>();
            }

            public byte Level
            {
                get { return this._Level; }
                set { this._Level = value; }
            }

            public uint Unknown
            {
                get { return this._Unknown; }
                set { this._Unknown = value; }
            }

            public List<LevelOfDetailAsset> Assets
            {
                get { return this._Assets; }
            }

            public List<ResourceKey> LODKeys
            {
                get { return this._LODKeys; }
            }
        }

        public struct LevelOfDetailAsset
        {
            public readonly int Sorting;
            public readonly int SpecularLevel;
            public readonly int CastShadow;

            public LevelOfDetailAsset(int sorting, int specularLevel, int castShadow)
            {
                this.Sorting = sorting;
                this.SpecularLevel = specularLevel;
                this.CastShadow = castShadow;
            }
        }

        public struct Override
        {
            public readonly byte Region;
            public readonly float Layer;

            public Override(byte region, float layer)
            {
                this.Region = region;
                this.Layer = layer;
            }
        }

        private class ResourceKeyTable
        {
            private readonly List<ResourceKey> _ResourceKeys;

            public ResourceKeyTable()
            {
                this._ResourceKeys = new List<ResourceKey>();
            }

            public void ReadTable(Stream input, Endian endian)
            {
                this._ResourceKeys.Clear();
                var count = input.ReadValueU8();
                for (int i = 0; i < count; i++)
                {
                    var instance = input.ReadValueU64(endian);
                    var group = input.ReadValueU32(endian);
                    var type = input.ReadValueU32(endian);
                    this._ResourceKeys.Add(new ResourceKey(instance, type, group));
                }
            }

            public void WriteTable(Stream output, Endian endian)
            {
                var count = this._ResourceKeys.Count;
                if (count >= 256)
                {
                    throw new InvalidOperationException();
                }

                output.WriteValueU8((byte)count);
                for (int i = 0; i < count; i++)
                {
                    output.WriteValueU64(this._ResourceKeys[i].Instance, endian);
                    output.WriteValueU32(this._ResourceKeys[i].Group, endian);
                    output.WriteValueU32(this._ResourceKeys[i].Type, endian);
                }
            }

            public ResourceKey ReadKey(Stream input)
            {
                var index = input.ReadValueU8();
                if (index < 0 || index > this._ResourceKeys.Count)
                {
                    throw new InvalidOperationException();
                }
                return this._ResourceKeys[index];
            }

            public void WriteKey(Stream output, ResourceKey key)
            {
                int index = this._ResourceKeys.IndexOf(key);
                if (index < 0)
                {
                    index = this._ResourceKeys.Count;
                    this._ResourceKeys.Add(key);
                }

                if (index >= 256)
                {
                    throw new InvalidOperationException();
                }

                output.WriteValueU8((byte)index);
            }
        }

        private static uint ReadPackedUInt32(Stream input)
        {
            uint value = 0;
            int shift = 0;
            byte b;
            do
            {
                b = input.ReadValueU8();
                value |= (b & 0x7Fu) << shift;
                shift += 7;
            }
            while ((b & 0x80) != 0);
            return value;
        }

        private static string ReadString(Stream input)
        {
            var length = ReadPackedUInt32(input);
            return input.ReadString(length, true, System.Text.Encoding.BigEndianUnicode);
        }

        private static void WritePackedUInt32(Stream output, uint value)
        {
            do
            {
                var b = (byte)(value & 0x7F);
                value >>= 7;

                if (value != 0)
                {
                    b |= 0x80;
                }

                output.WriteValueU8(b);
            }
            while (value != 0);
        }

        private static void WriteString(Stream output, string text)
        {
            var bytes = System.Text.Encoding.BigEndianUnicode.GetBytes(text);
            var length = bytes.Length;

            WritePackedUInt32(output, (uint)length);
            output.Write(bytes, 0, length);
        }
    }
}
