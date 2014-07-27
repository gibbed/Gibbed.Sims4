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
    public class CASPartFile
    {
        private uint _Unknown34;
        private uint _Unknown38;
        private string _Unknown3C;
        private uint _Unknown4C;
        private uint _Unknown50;
        private uint _Unknown54;
        private uint _Unknown58;
        private float _CatalogPriority;
        private ushort _Unknown60;
        private byte _Unknown62;
        private uint _Unknown64;
        private ResourceKey _Unknown68;
        private byte _Unknown78;
        private ulong _Unknown80;
        private uint _Unknown88;
        private ResourceKey _Unknown90;
        private uint _UnknownE4;
        private ResourceKey _UnknownE8;
        private ResourceKey _UnknownF8;
        private uint _Unknown108;
        private ResourceKey _Unknown110;
        private ResourceKey _Unknown120;
        private byte _Unknown10C;
        private ResourceKey _Unknown190;
        private ResourceKey _Unknown140;
        private ResourceKey _Unknown130;
        private readonly List<uint> _UnknownBC;

        #region Properties
        #endregion

        #region Fields
        #endregion

        public CASPartFile()
        {
            this._UnknownBC = new List<uint>();
        }

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            const Endian endian = Endian.Little;

            var version = input.ReadValueU32(endian);
            if (version > 26)
            {
                throw new FormatException();
            }

            this._UnknownBC.Clear();

            var keyTableOffset = input.ReadValueU32(endian);
            var dataPosition = input.Position;

            input.Position += keyTableOffset;
            var keyTablePosition = input.Position;

            var keyCount = input.ReadValueU8();
            var keys = new ResourceKey[keyCount];
            for (int i = 0; i < keyCount; i++)
            {
                var instance = input.ReadValueU64(endian);
                var group = input.ReadValueU32(endian);
                var type = input.ReadValueU32(endian);
                keys[i] = new ResourceKey(instance, type, group);
            }
            var endPosition = input.Position;
            input.Position = dataPosition;

            var count = input.ReadValueU32(endian);
            for (uint i = 0; i < count; i++)
            {
                var v26 = input.ReadValueU64(endian);
                var b = input.ReadValueU8();
                if (b != 0)
                {
                    var v22 = input.ReadValueU32(endian);
                    var type = input.ReadValueU8();
                    switch (type)
                    {
                        case 1:
                        {
                            var v23 = input.ReadValueU32(endian);
                            break;
                        }

                        case 2:
                        {
                            var v24 = input.ReadValueF32(endian);
                            break;
                        }

                        case 3:
                        {
                            var v31_instance = input.ReadValueU64(endian);
                            var v31_group = input.ReadValueU32(endian);
                            var v31_type = input.ReadValueU32(endian);
                            var v31 = new ResourceKey(v31_instance, v31_type, v31_group);
                            break;
                        }

                        case 4:
                        {
                            var v23 = input.ReadValueU32(endian);
                            break;
                        }

                        default:
                        {
                            throw new NotSupportedException();
                        }
                    }
                }
            }

            var length = this.ReadPackedUInt32(input);
            this._Unknown3C = input.ReadString(length, true, System.Text.Encoding.BigEndianUnicode);

            this._CatalogPriority = input.ReadValueF32(endian);

            if (version >= 13 && version <= 22)
            {
                this._Unknown62 = input.ReadValueU8();
            }

            if (version >= 13)
            {
                this._Unknown60 = input.ReadValueU16(endian);
                this._Unknown64 = input.ReadValueU32(endian);
            }

            if (version >= 15)
            {
                this._Unknown4C = input.ReadValueU32(endian);
                this._Unknown78 = input.ReadValueU8();
            }

            if (version >= 24)
            {
                this._Unknown80 = input.ReadValueU64(endian);
            }

            if (version >= 25)
            {
                this._Unknown88 = input.ReadValueU32(endian);
            }

            if (version >= 16)
            {
                var count2 = input.ReadValueU32(endian);
                for (uint i = 0; i < count2; i++)
                {
                    input.ReadValueU16(endian);
                    input.ReadValueU16(endian);
                }
            }

            if (version >= 17)
            {
                this._Unknown50 = input.ReadValueU32(endian);
            }

            if (version >= 18)
            {
                this._Unknown54 = input.ReadValueU32(endian);
                this._Unknown58 = input.ReadValueU32(endian);
            }

            var v45 = input.ReadValueB8();
            this._Unknown38 = input.ReadValueU32(endian);
            var v52 = input.ReadValueU32(endian);
            this._Unknown34 = input.ReadValueU32(endian);

            if (version >= 14)
            {
                var v44 = input.ReadValueU8();
                if (v44 != 0)
                {
                    var v46 = input.ReadValueU8();
                    if (v46 != 0)
                    {
                    }
                }
            }

            if (version >= 20)
            {
                var count3 = input.ReadValueU8();
                for (int i = 0; i < count3; i++)
                {
                    this._UnknownBC.Add(input.ReadValueU32(endian));
                }
            }

            if (version >= 21)
            {
                var index = input.ReadValueU8();
                this._Unknown90 = keys[index];
            }

            if (version >= 26)
            {
                var index = input.ReadValueU8();
                this._Unknown68 = keys[index];
            }

            if (version <= 22)
            {
                this._UnknownE4 = input.ReadValueU32(endian);
            }

            var index2 = input.ReadValueU8();
            this._UnknownE8 = keys[index2];

            var index3 = input.ReadValueU8();
            this._UnknownF8 = keys[index3];

            this._Unknown108 = input.ReadValueU32(endian);
            if (version <= 2)
            {
                this._Unknown108 *= 1000;
            }

            var count4 = input.ReadValueU8();
            for (int i = 0; i < count4; i++)
            {
                var a1 = input.ReadValueU8();
                var a2 = input.ReadValueU32(endian);
                var count5 = input.ReadValueU8();
                for (int j = 0; j < count5; j++)
                {
                    var a3 = input.ReadValueU32(endian);
                    var a4 = input.ReadValueU32(endian);
                    var a5 = input.ReadValueU32(endian);
                }

                if (version >= 7)
                {
                    byte count6 = version >= 10 ? input.ReadValueU8() : (byte)1;
                    for (int j = 0; j < count6; j++)
                    {
                        var v97 = keys[input.ReadValueU8()];
                        if (v97.Type != 0x015A1849)
                        {
                        }
                    }
                }
            }

            var v162 = input.ReadValueU8();
            for (int i = 0; i < v162; i++)
            {
                var v170 = keys[input.ReadValueU8()];
                if (v170 != ResourceKey.Invalid)
                {
                }
            }

            if (version >= 2 && version <= 6)
            {
                for (int i = 0; i < count4; i++)
                {
                    var v112 = keys[input.ReadValueU8()];
                }
            }

            if (version >= 4)
            {
                var index = input.ReadValueU8();
                this._Unknown110 = keys[index];
            }

            if (version >= 6)
            {
                var index = input.ReadValueU8();
                this._Unknown120 = keys[index];
            }

            if (version >= 5 && version <= 14)
            {
                this._Unknown4C = input.ReadValueU32(endian);
            }

            if (version >= 8)
            {
                this._Unknown10C = input.ReadValueU8();
            }

            if (version >= 11)
            {
                var index = input.ReadValueU8();
                this._Unknown190 = keys[index];
            }

            if (version >= 12)
            {
                var count6 = input.ReadValueU8();
                for (int i = 0; i < count6; i++)
                {
                    var u1 = input.ReadValueU8();
                    var v148 = input.ReadValueF32(endian);
                }
            }

            if (version == 19)
            {
                var count3 = input.ReadValueU8();
                for (int i = 0; i < count3; i++)
                {
                    this._UnknownBC.Add(input.ReadValueU32(endian));
                }
            }

            if (version >= 22)
            {
                var index4 = input.ReadValueU8();
                this._Unknown140 = keys[index4];
                var index5 = input.ReadValueU8();
                this._Unknown130 = keys[index5];
            }

            if (input.Position != keyTablePosition)
            {
                throw new FormatException();
            }

            input.Position = endPosition;
        }

        private uint ReadPackedUInt32(Stream input)
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
    }
}
