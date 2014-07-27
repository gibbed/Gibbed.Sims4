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

namespace Gibbed.Sims4.UnknownFormats
{
    // ReSharper disable InconsistentNaming
    public class TypeDB43E069
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

        public uint Unknown40
        {
            get { return this._Unknown40; }
            set { this._Unknown40 = value; }
        }

        public uint Unknown50
        {
            get { return this._Unknown50; }
            set { this._Unknown50 = value; }
        }

        public byte Unknown54
        {
            get { return this._Unknown54; }
            set { this._Unknown54 = value; }
        }

        public byte Unknown55
        {
            get { return this._Unknown55; }
            set { this._Unknown55 = value; }
        }

        public uint Unknown5C
        {
            get { return this._Unknown5C; }
            set { this._Unknown5C = value; }
        }

        public uint Unknown64
        {
            get { return this._Unknown64; }
            set { this._Unknown64 = value; }
        }

        public uint Unknown68
        {
            get { return this._Unknown68; }
            set { this._Unknown68 = value; }
        }

        public uint Unknown6C
        {
            get { return this._Unknown6C; }
            set { this._Unknown6C = value; }
        }

        public byte Unknown58
        {
            get { return this._Unknown58; }
            set { this._Unknown58 = value; }
        }

        public byte[] Unknown48
        {
            get { return this._Unknown48; }
            set { this._Unknown48 = value; }
        }
        #endregion

        #region Fields
        private uint _Version;
        private uint _Unknown30;
        private uint _Unknown40;
        private uint _Unknown50;
        private byte _Unknown54;
        private byte _Unknown55;
        private uint _Unknown5C;
        private uint _Unknown64;
        private uint _Unknown68;
        private uint _Unknown6C;
        private byte _Unknown58;
        private byte[] _Unknown48;
        #endregion

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            const Endian endian = Endian.Little;

            var version = input.ReadValueU32(endian);
            if (version < 1 || version > 5)
            {
                throw new FormatException();
            }

            this._Version = version;
            this._Unknown30 = input.ReadValueU32(endian);
            this._Unknown40 = input.ReadValueU32(endian);

            if (version >= 3)
            {
                this._Unknown50 = input.ReadValueU32(endian);
            }
            else
            {
                var temp1 = input.ReadValueU8();
                var temp2 = input.ReadValueU8();

                this._Unknown50 = 1u << temp1;
                this._Unknown50 |= temp2 != 0xFF ? 0x1000u << temp2 : 0x3000u;
            }

            this._Unknown54 = input.ReadValueU8();
            this._Unknown55 = input.ReadValueU8();

            if (version >= 2)
            {
                this._Unknown5C = input.ReadValueU32(endian);
                this._Unknown64 = input.ReadValueU32(endian);
                this._Unknown68 = input.ReadValueU32(endian);
                this._Unknown6C = input.ReadValueU32(endian);
            }

            if (version >= 4)
            {
                this._Unknown58 = input.ReadValueU8();
            }

            var foo = this._Unknown64 - this._Unknown5C;
            var bar = this._Unknown6C - this._Unknown68 + 1;
            var bufferLength = 6 * (foo + 1) * bar;

            if (version >= 5)
            {
                bufferLength = input.ReadValueU32(endian);
            }
            else
            {
                throw new NotImplementedException();
            }

            this._Unknown48 = input.ReadBytes(bufferLength);
        }
    }
}
