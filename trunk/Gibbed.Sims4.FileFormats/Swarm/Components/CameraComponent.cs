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
    public class CameraComponent : IComponent
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

        public uint Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public ushort ViewFlags
        {
            get { return this._ViewFlags; }
            set { this._ViewFlags = value; }
        }

        public float Lifetime
        {
            get { return this._Lifetime; }
            set { this._Lifetime = value; }
        }

        public List<float> HeadingCurve
        {
            get { return this._HeadingCurve; }
        }

        public List<float> PitchCurve
        {
            get { return this._PitchCurve; }
        }

        public List<float> RollCurve
        {
            get { return this._RollCurve; }
        }

        public List<float> DistanceCurve
        {
            get { return this._DistanceCurve; }
        }

        public List<float> FOVCurve
        {
            get { return this._FOVCurve; }
        }

        public List<float> NearClipCurve
        {
            get { return this._NearClipCurve; }
        }

        public List<float> FarClipCurve
        {
            get { return this._FarClipCurve; }
        }

        public ulong CameraId
        {
            get { return this._CameraId; }
            set { this._CameraId = value; }
        }

        public ushort CubemapResource
        {
            get { return this._CubemapResource; }
            set { this._CubemapResource = value; }
        }
        #endregion

        #region Fields
        private uint _Flags;
        private ushort _ViewFlags;
        private float _Lifetime;
        private readonly List<float> _HeadingCurve;
        private readonly List<float> _PitchCurve;
        private readonly List<float> _RollCurve;
        private readonly List<float> _DistanceCurve;
        private readonly List<float> _FOVCurve;
        private readonly List<float> _NearClipCurve;
        private readonly List<float> _FarClipCurve;
        private ulong _CameraId;
        private ushort _CubemapResource;
        #endregion

        public CameraComponent()
        {
            this._HeadingCurve = new List<float>();
            this._PitchCurve = new List<float>();
            this._RollCurve = new List<float>();
            this._DistanceCurve = new List<float>();
            this._FOVCurve = new List<float>();
            this._NearClipCurve = new List<float>();
            this._FarClipCurve = new List<float>();
        }

        public void Serialize(Stream output, short version)
        {
            Binary.Write(output, this._Flags);
            Binary.Write(output, this._ViewFlags);
            Binary.Write(output, this._Lifetime);
            Binary.Write(output, this._HeadingCurve);
            Binary.Write(output, this._PitchCurve);
            Binary.Write(output, this._RollCurve);
            Binary.Write(output, this._DistanceCurve);
            Binary.Write(output, this._FOVCurve);
            Binary.Write(output, this._NearClipCurve);
            Binary.Write(output, this._FarClipCurve);
            Binary.Write(output, this._CameraId);
            Binary.Write(output, this._CubemapResource);
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Flags);
            this._Flags &= 0x3FF;

            Binary.Read(input, out this._ViewFlags);
            Binary.Read(input, out this._Lifetime);
            Binary.Read____(input, this._HeadingCurve);
            Binary.Read____(input, this._PitchCurve);
            Binary.Read____(input, this._RollCurve);
            Binary.Read____(input, this._DistanceCurve);
            Binary.Read____(input, this._FOVCurve);
            Binary.Read____(input, this._NearClipCurve);
            Binary.Read____(input, this._FarClipCurve);
            Binary.Read(input, out this._CameraId);
            Binary.Read(input, out this._CubemapResource);
        }
    }
}
