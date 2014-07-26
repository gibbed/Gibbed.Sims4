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

namespace Gibbed.Sims4.FileFormats
{
    public class SimInfoFile
    {
        #region Properties
        public ResourceKey Unknown30
        {
            get { return this._Unknown30; }
            set { this._Unknown30 = value; }
        }

        public uint Unknown40
        {
            get { return this._Unknown40; }
            set { this._Unknown40 = value; }
        }

        public uint Unknown44
        {
            get { return this._Unknown44; }
            set { this._Unknown44 = value; }
        }

        public float Unknown48
        {
            get { return this._Unknown48; }
            set { this._Unknown48 = value; }
        }
        #endregion

        #region Fields
        private ResourceKey _Unknown30;
        private uint _Unknown40;
        private uint _Unknown44;
        private float _Unknown48;
        #endregion

        public void Serialize(Stream output)
        {
            const Endian endian = Endian.Little;

            output.WriteValueU32(1, endian);
            output.WriteValueU32(this._Unknown44, endian);
            output.WriteValueU32(this._Unknown40, endian);
            output.WriteValueF32(this._Unknown48, endian);
            output.WriteValueU64(this._Unknown30.InstanceId);
        }

        public void Deserialize(Stream input)
        {
            const Endian endian = Endian.Little;

            var version = input.ReadValueU32(endian);
            if (version != 1)
            {
                throw new FormatException();
            }

            this.Unknown44 = input.ReadValueU32(endian);
            this.Unknown40 = input.ReadValueU32(endian);
            this.Unknown48 = input.ReadValueF32(endian);

            var instance = input.ReadValueU64(endian);
            this.Unknown30 = instance != 0 ? new ResourceKey(instance, 0x025ED6F4, 0) : ResourceKey.Invalid;
        }
    }
}
