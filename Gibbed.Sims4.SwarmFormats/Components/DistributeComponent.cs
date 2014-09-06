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

namespace Gibbed.Sims4.SwarmFormats.Components
{
    public class DistributeComponent : IComponent
    {
        #region Properties
        public uint Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public int Density
        {
            get { return this._Density; }
            set { this._Density = value; }
        }

        public string ComponentName
        {
            get { return this._ComponentName; }
            set { this._ComponentName = value; }
        }

        public int Start
        {
            get { return this._Start; }
            set { this._Start = value; }
        }

        public byte SourceType
        {
            get { return this._SourceType; }
            set { this._SourceType = value; }
        }

        public float SourceSize
        {
            get { return this._SourceSize; }
            set { this._SourceSize = value; }
        }

        public Transform PreTransform
        {
            get { return this._PreTransform; }
            set { this._PreTransform = value; }
        }

        public List<float> SizeCurve
        {
            get { return this._SizeCurve; }
        }

        public float SizeVary
        {
            get { return this._SizeVary; }
            set { this._SizeVary = value; }
        }

        public List<float> PitchCurve
        {
            get { return this._PitchCurve; }
        }

        public List<float> RollCurve
        {
            get { return this._RollCurve; }
        }

        public List<float> HeadingCurve
        {
            get { return this._HeadingCurve; }
        }

        public float PitchVary
        {
            get { return this._PitchVary; }
            set { this._PitchVary = value; }
        }

        public float RollVary
        {
            get { return this._RollVary; }
            set { this._RollVary = value; }
        }

        public float HeadingVary
        {
            get { return this._HeadingVary; }
            set { this._HeadingVary = value; }
        }

        public float PitchOffset
        {
            get { return this._PitchOffset; }
            set { this._PitchOffset = value; }
        }

        public float RollOffset
        {
            get { return this._RollOffset; }
            set { this._RollOffset = value; }
        }

        public float HeadingOffset
        {
            get { return this._HeadingOffset; }
            set { this._HeadingOffset = value; }
        }

        public List<Vector3> ColorCurve
        {
            get { return this._ColorCurve; }
        }

        public Vector3 ColorVary
        {
            get { return this._ColorVary; }
            set { this._ColorVary = value; }
        }

        public List<float> AlphaCurve
        {
            get { return this._AlphaCurve; }
        }

        public float AlphaVary
        {
            get { return this._AlphaVary; }
            set { this._AlphaVary = value; }
        }

        public List<SurfaceInfo> Surfaces
        {
            get { return this._Surfaces; }
        }

        public ulong EmitMapId
        {
            get { return this._EmitMapId; }
            set { this._EmitMapId = value; }
        }

        public ulong ColorMapId
        {
            get { return this._ColorMapId; }
            set { this._ColorMapId = value; }
        }

        public ulong PinMapId
        {
            get { return this._PinMapId; }
            set { this._PinMapId = value; }
        }

        public Vector2 AltitudeRange
        {
            get { return this._AltitudeRange; }
            set { this._AltitudeRange = value; }
        }

        public DrawInfo DrawInfo
        {
            get { return this._DrawInfo; }
            set { this._DrawInfo = value; }
        }

        public byte OverrideSet
        {
            get { return this._OverrideSet; }
            set { this._OverrideSet = value; }
        }

        public uint MessageId
        {
            get { return this._MessageId; }
            set { this._MessageId = value; }
        }
        #endregion

        #region Fields
        private uint _Flags;
        private int _Density;
        private string _ComponentName;
        private int _Start;
        private byte _SourceType;
        private float _SourceSize;
        private Transform _PreTransform;
        private readonly List<float> _SizeCurve;
        private float _SizeVary;
        private readonly List<float> _PitchCurve;
        private readonly List<float> _RollCurve;
        private readonly List<float> _HeadingCurve;
        private float _PitchVary;
        private float _RollVary;
        private float _HeadingVary;
        private float _PitchOffset;
        private float _RollOffset;
        private float _HeadingOffset;
        private readonly List<Vector3> _ColorCurve;
        private Vector3 _ColorVary;
        private readonly List<float> _AlphaCurve;
        private float _AlphaVary;
        private readonly List<SurfaceInfo> _Surfaces;
        private ulong _EmitMapId;
        private ulong _ColorMapId;
        private ulong _PinMapId;
        private Vector2 _AltitudeRange;
        private DrawInfo _DrawInfo;
        private byte _OverrideSet;
        private uint _MessageId;
        #endregion

        public DistributeComponent()
        {
            this._SizeCurve = new List<float>();
            this._PitchCurve = new List<float>();
            this._RollCurve = new List<float>();
            this._HeadingCurve = new List<float>();
            this._ColorCurve = new List<Vector3>();
            this._AlphaCurve = new List<float>();
            this._Surfaces = new List<SurfaceInfo>();
        }

        public void Serialize(Stream output, short version)
        {
            Binary.Write(output, this._Flags);
            Binary.Write(output, this._Density);
            Binary.Write(output, this._ComponentName);
            Binary.Write(output, this._Start);
            Binary.Write(output, this._SourceType);
            Binary.Write(output, this._SourceSize);
            Binary.Write(output, this._PreTransform);
            Binary.Write(output, this._SizeCurve);
            Binary.Write(output, this._SizeVary);
            Binary.Write(output, this._PitchCurve);
            Binary.Write(output, this._RollCurve);
            Binary.Write(output, this._HeadingCurve);
            Binary.Write(output, this._PitchVary);
            Binary.Write(output, this._RollVary);
            Binary.Write(output, this._HeadingVary);
            Binary.Write(output, this._PitchOffset);
            Binary.Write(output, this._RollOffset);
            Binary.Write(output, this._HeadingOffset);
            Binary.Write(output, this._ColorCurve);
            Binary.Write(output, this._ColorVary);
            Binary.Write(output, this._AlphaCurve);
            Binary.Write(output, this._AlphaVary);
            Binary.Write(output, this._Surfaces);
            Binary.Write(output, this._EmitMapId);
            Binary.Write(output, this._ColorMapId);
            Binary.Write(output, this._PinMapId);
            Binary.Write(output, this._AltitudeRange);
            Binary.Write(output, this._DrawInfo);
            Binary.Write(output, this._OverrideSet);
            Binary.Write(output, this._MessageId);
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Flags);
            this._Flags &= 0x3FFF;

            Binary.Read(input, out this._Density);
            Binary.Read(input, out this._ComponentName);
            Binary.Read(input, out this._Start);
            Binary.Read(input, out this._SourceType);
            Binary.Read(input, out this._SourceSize);
            Binary.Read(input, out this._PreTransform);
            Binary.Read____(input, this._SizeCurve);
            Binary.Read(input, out this._SizeVary);
            Binary.Read____(input, this._PitchCurve);
            Binary.Read____(input, this._RollCurve);
            Binary.Read____(input, this._HeadingCurve);
            Binary.Read(input, out this._PitchVary);
            Binary.Read(input, out this._RollVary);
            Binary.Read(input, out this._HeadingVary);
            Binary.Read(input, out this._PitchOffset);
            Binary.Read(input, out this._RollOffset);
            Binary.Read(input, out this._HeadingOffset);
            Binary.Read____(input, this._ColorCurve);
            Binary.Read(input, out this._ColorVary);
            Binary.Read____(input, this._AlphaCurve);
            Binary.Read(input, out this._AlphaVary);
            Binary.Read____(input, this._Surfaces);
            Binary.Read(input, out this._EmitMapId);
            Binary.Read(input, out this._ColorMapId);
            Binary.Read(input, out this._PinMapId);
            Binary.Read(input, out this._AltitudeRange);
            Binary.Read(input, out this._DrawInfo);
            Binary.Read(input, out this._OverrideSet);
            Binary.Read(input, out this._MessageId);
        }
    }
}
