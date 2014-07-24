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

namespace Gibbed.Sims4.FileFormats.Swarm.Components
{
    public class SoundComponent : IComponent
    {
        #region Properties
        public short MinimumVersion
        {
            get { return 1; }
        }

        public short MaximumVersion
        {
            get { return 2; }
        }

        public uint Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public ulong SoundId
        {
            get { return this._SoundId; }
            set { this._SoundId = value; }
        }

        public float LocationUpdateDelta
        {
            get { return this._LocationUpdateDelta; }
            set { this._LocationUpdateDelta = value; }
        }

        public float PlayTime
        {
            get { return this._PlayTime; }
            set { this._PlayTime = value; }
        }

        public float Volume
        {
            get { return this._Volume; }
            set { this._Volume = value; }
        }

        public byte Unknown24
        {
            get { return this._Unknown24; }
            set { this._Unknown24 = value; }
        }
        #endregion

        #region Fields
        private uint _Flags;
        private ulong _SoundId;
        private float _LocationUpdateDelta;
        private float _PlayTime;
        private float _Volume;
        private byte _Unknown24;
        #endregion

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Flags);
            this._Flags &= 0xF;

            Binary.Read(input, out this._SoundId);
            Binary.Read(input, out this._LocationUpdateDelta);
            Binary.Read(input, out this._PlayTime);
            Binary.Read(input, out this._Volume);

            if (version >= 2)
            {
                Binary.Read(input, out this._Unknown24);
            }
            else
            {
                this._Unknown24 = 0;
            }
        }
    }
}
