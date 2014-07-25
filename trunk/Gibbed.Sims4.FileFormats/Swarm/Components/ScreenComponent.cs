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

using System.Collections.Generic;
using System.IO;

namespace Gibbed.Sims4.FileFormats.Swarm.Components
{
    public class ScreenComponent : IComponent
    {
        #region Properties
        public short MinimumVersion
        {
            get { return 1; }
        }

        public short MaximumVersion
        {
            get { return 1; }
        }

        public byte Mode
        {
            get { return this._Mode; }
            set { this._Mode = value; }
        }

        public uint Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public List<Vector3> ColorCurve
        {
            get { return this._ColorCurve; }
        }

        public List<float> StrengthCurve
        {
            get { return this._StrengthCurve; }
        }

        public List<float> DistanceCurve
        {
            get { return this._DistanceCurve; }
        }

        public float Lifetime
        {
            get { return this._Lifetime; }
            set { this._Lifetime = value; }
        }

        public float Delay
        {
            get { return this._Delay; }
            set { this._Delay = value; }
        }

        public float Falloff
        {
            get { return this._Falloff; }
            set { this._Falloff = value; }
        }

        public float DistanceBase
        {
            get { return this._DistanceBase; }
            set { this._DistanceBase = value; }
        }

        public ulong TextureId
        {
            get { return this._TextureId; }
            set { this._TextureId = value; }
        }

        public FilterChain FilterChain
        {
            get { return this._FilterChain; }
            set { this._FilterChain = value; }
        }
        #endregion

        #region Fields
        private byte _Mode;
        private uint _Flags;
        private readonly List<Vector3> _ColorCurve;
        private readonly List<float> _StrengthCurve;
        private readonly List<float> _DistanceCurve;
        private float _Lifetime;
        private float _Delay;
        private float _Falloff;
        private float _DistanceBase;
        private ulong _TextureId;
        private FilterChain _FilterChain;
        #endregion

        public ScreenComponent()
        {
            this._ColorCurve = new List<Vector3>();
            this._StrengthCurve = new List<float>();
            this._DistanceCurve = new List<float>();
        }

        public void Serialize(Stream output, short version)
        {
            Binary.Write(output, this._Mode);
            Binary.Write(output, this._Flags);
            Binary.Write(output, this._ColorCurve);
            Binary.Write(output, this._StrengthCurve);
            Binary.Write(output, this._DistanceCurve);
            Binary.Write(output, this._Lifetime);
            Binary.Write(output, this._Delay);
            Binary.Write(output, this._Falloff);
            Binary.Write(output, this._DistanceBase);
            Binary.Write(output, this._TextureId);
            Binary.Write(output, this._FilterChain);
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Mode);

            Binary.Read(input, out this._Flags);
            this._Flags &= 3;

            Binary.Read____(input, this._ColorCurve);
            Binary.Read____(input, this._StrengthCurve);
            Binary.Read____(input, this._DistanceCurve);
            Binary.Read(input, out this._Lifetime);
            Binary.Read(input, out this._Delay);
            Binary.Read(input, out this._Falloff);
            Binary.Read(input, out this._DistanceBase);
            Binary.Read(input, out this._TextureId);
            Binary.Read(input, out this._FilterChain);
        }
    }
}
