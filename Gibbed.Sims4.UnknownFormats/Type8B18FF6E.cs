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
using ResourceKey = Gibbed.Sims4.FileFormats.ResourceKey;

namespace Gibbed.Sims4.UnknownFormats
{
    // ReSharper disable InconsistentNaming
    public class Type8B18FF6E
        // ReSharper restore InconsistentNaming
    {
        #region Properties
        public uint Version
        {
            get { return this._Version; }
            set { this._Version = value; }
        }

        public uint Unknown30
        {
            get { return this._Unknown30; }
            set { this._Unknown30 = value; }
        }

        public byte Unknown34
        {
            get { return this._Unknown34; }
            set { this._Unknown34 = value; }
        }

        public byte Unknown38
        {
            get { return this._Unknown38; }
            set { this._Unknown38 = value; }
        }

        public byte Unknown3C
        {
            get { return this._Unknown3C; }
            set { this._Unknown3C = value; }
        }

        public uint Unknown40
        {
            get { return this._Unknown40; }
            set { this._Unknown40 = value; }
        }

        public ResourceKey Unknown48
        {
            get { return this._Unknown48; }
            set { this._Unknown48 = value; }
        }

        public uint Unknown58
        {
            get { return this._Unknown58; }
            set { this._Unknown58 = value; }
        }

        public byte Unknown44
        {
            get { return this._Unknown44; }
            set { this._Unknown44 = value; }
        }

        public Sub1[] Unknown5C
        {
            get { return this._Unknown5C; }
        }
        #endregion

        #region Fields
        private uint _Version;
        private uint _Unknown30;
        private byte _Unknown34;
        private byte _Unknown38;
        private byte _Unknown3C;
        private uint _Unknown40;
        private ResourceKey _Unknown48;
        private uint _Unknown58;
        private byte _Unknown44;
        private readonly Sub1[] _Unknown5C;
        #endregion

        public Type8B18FF6E()
        {
            this._Unknown5C = new Sub1[6];
        }

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            const Endian endian = Endian.Little;

            var version = input.ReadValueU32(endian);
            if (version > 11)
            {
                throw new FormatException();
            }

            this._Version = version;
            this._Unknown30 = version >= 7 ? input.ReadValueU32(endian) : 12415u;
            this._Unknown34 = input.ReadValueU8();
            this._Unknown38 = input.ReadValueU8();
            this._Unknown3C = input.ReadValueU8();
            this._Unknown40 = version >= 10 ? input.ReadValueU32(endian) : UpdateUnknown40(input.ReadValueU32(endian));

            if (version >= 2 && version <= 4)
            {
                // input.ReadValueU64(endian);
                // input.ReadValueU64(endian);
                input.Seek(16, SeekOrigin.Current);
            }

            if (version >= 3)
            {
                var instance = input.ReadValueU64(endian);
                this._Unknown48 = instance != 0 ? new ResourceKey(instance, 0x00B2D882, 0) : ResourceKey.Invalid;
            }

            if (version >= 6)
            {
                this._Unknown58 = input.ReadValueU32(endian);
            }

            if (version == 4)
            {
                this._Unknown44 = input.ReadValueU8();
            }

            var setFlags = 0u;
            if (version >= 5)
            {
                var count = input.ReadValueU8();
                for (int i = 0; i < count; i++)
                {
                    var flags = input.ReadValueU8();
                    this._Unknown44 |= flags;

                    Sub1 sub = new Sub1();

                    if (version >= 9)
                    {
                        sub.Unknown40 = input.ReadValueB8();
                    }

                    if (version >= 11)
                    {
                        sub.Unknown44 = input.ReadValueF32(endian);
                        sub.Unknown48 = input.ReadValueF32(endian);
                    }

                    if (version >= 8)
                    {
                        var instance1 = input.ReadValueU64(endian);
                        var instance2 = input.ReadValueU64(endian);
                        var instance3 = input.ReadValueU64(endian);
                        var instance4 = input.ReadValueU64(endian);

                        sub.Unknown00 = instance1 != 0
                                            ? new ResourceKey(instance1, 0xC5F6763E, 0)
                                            : ResourceKey.Invalid;
                        sub.Unknown10 = instance2 != 0
                                            ? new ResourceKey(instance2, 0xC5F6763E, 0)
                                            : ResourceKey.Invalid;
                        sub.Unknown30 = instance3 != 0
                                            ? new ResourceKey(instance3, 0xC5F6763E, 0)
                                            : ResourceKey.Invalid;
                        sub.Unknown20 = instance4 != 0
                                            ? new ResourceKey(instance4, 0xC5F6763E, 0)
                                            : ResourceKey.Invalid;

                        for (int j = 0; j < 6; j++)
                        {
                            if ((flags & (1 << j)) != 0)
                            {
                                if ((setFlags & (1u << j)) != 0)
                                {
                                    //throw new InvalidOperationException();
                                }
                                setFlags |= 1u << j;

                                this._Unknown5C[j] = sub;
                            }
                        }
                    }
                    else
                    {
                        var instance1 = input.ReadValueU64(endian);
                        var instance2 = input.ReadValueU64(endian);

                        var resourceKey1 = instance1 != 0
                                               ? new ResourceKey(instance1, 0x69B9B5FC, 0)
                                               : ResourceKey.Invalid;
                        var resourceKey2 = instance2 != 0
                                               ? new ResourceKey(instance2, 0x69B9B5FC, 0)
                                               : ResourceKey.Invalid;

                        throw new NotImplementedException();
                    }
                }
            }
        }

        public struct Sub1
        {
            public ResourceKey Unknown00 { get; set; }
            public ResourceKey Unknown10 { get; set; }
            public ResourceKey Unknown20 { get; set; }
            public ResourceKey Unknown30 { get; set; }
            public bool Unknown40 { get; set; }
            public float Unknown44 { get; set; }
            public float Unknown48 { get; set; }
        }

        private uint UpdateUnknown40(uint value)
        {
            switch (value)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 28:
                case 29:
                case 30:
                case 31:
                {
                    return value;
                }

                case 10:
                {
                    return 8;
                }

                case 11:
                {
                    return 9;
                }

                case 12:
                {
                    return 10;
                }

                case 18:
                {
                    return 12;
                }

                case 22:
                {
                    return 14;
                }

                case 23:
                {
                    return 21;
                }

                case 24:
                {
                    return 22;
                }

                case 25:
                {
                    return 23;
                }

                case 26:
                {
                    return 24;
                }

                case 27:
                {
                    return 25;
                }
            }

            return 32;
        }
    }
}
