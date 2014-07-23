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

namespace Gibbed.Sims4.FileFormats.Swarm.Components
{
    public class RibbonComponent : IComponent
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

        public Vector2 RibbonLifetime
        {
            get { return this._RibbonLifetime; }
            set { this._RibbonLifetime = value; }
        }

        public List<float> OffsetCurve
        {
            get { return this._OffsetCurve; }
        }

        public List<float> WidthCurve
        {
            get { return this._WidthCurve; }
        }

        public float Taper
        {
            get { return this._Taper; }
            set { this._Taper = value; }
        }

        public float Fade
        {
            get { return this._Fade; }
            set { this._Fade = value; }
        }

        public float AlphaDecay
        {
            get { return this._AlphaDecay; }
            set { this._AlphaDecay = value; }
        }

        public List<Vector3> ColorCurve
        {
            get { return this._ColorCurve; }
        }

        public List<float> AlphaCurve
        {
            get { return this._AlphaCurve; }
        }

        public List<Vector3> LengthColorCurve
        {
            get { return this._LengthColorCurve; }
        }

        public List<float> LengthAlphaCurve
        {
            get { return this._LengthAlphaCurve; }
        }

        public List<Vector3> EdgeColorCurve
        {
            get { return this._EdgeColorCurve; }
        }

        public List<float> EdgeAlphaCurve
        {
            get { return this._EdgeAlphaCurve; }
        }

        public List<float> StartEdgeAlphaCurve
        {
            get { return this._StartEdgeAlphaCurve; }
        }

        public List<float> EndEdgeAlphaCurve
        {
            get { return this._EndEdgeAlphaCurve; }
        }

        public int NumSegments
        {
            get { return this._NumSegments; }
            set { this._NumSegments = value; }
        }

        public float SegmentLength
        {
            get { return this._SegmentLength; }
            set { this._SegmentLength = value; }
        }

        public DrawInfo DrawInfo
        {
            get { return this._DrawInfo; }
            set { this._DrawInfo = value; }
        }

        public int TileUv
        {
            get { return this._TileUV; }
            set { this._TileUV = value; }
        }

        public float SlipCurveSpeed
        {
            get { return this._SlipCurveSpeed; }
            set { this._SlipCurveSpeed = value; }
        }

        public float SlipUvSpeed
        {
            get { return this._SlipUVSpeed; }
            set { this._SlipUVSpeed = value; }
        }

        public float UvRepeat
        {
            get { return this._UVRepeat; }
            set { this._UVRepeat = value; }
        }

        public Vector3 DirectionalForcesSum
        {
            get { return this._DirectionalForcesSum; }
            set { this._DirectionalForcesSum = value; }
        }

        public float WindStrength
        {
            get { return this._WindStrength; }
            set { this._WindStrength = value; }
        }

        public float GravityStrength
        {
            get { return this._GravityStrength; }
            set { this._GravityStrength = value; }
        }

        public ulong EmitColorMapId
        {
            get { return this._EmitColorMapId; }
            set { this._EmitColorMapId = value; }
        }

        public ulong ForceMapId
        {
            get { return this._ForceMapId; }
            set { this._ForceMapId = value; }
        }

        public float MapRepulseStrength
        {
            get { return this._MapRepulseStrength; }
            set { this._MapRepulseStrength = value; }
        }
        #endregion

        #region Fields
        private uint _Flags;
        private Vector2 _RibbonLifetime;
        private readonly List<float> _OffsetCurve;
        private readonly List<float> _WidthCurve;
        private float _Taper;
        private float _Fade;
        private float _AlphaDecay;
        private readonly List<Vector3> _ColorCurve;
        private readonly List<float> _AlphaCurve;
        private readonly List<Vector3> _LengthColorCurve;
        private readonly List<float> _LengthAlphaCurve;
        private readonly List<Vector3> _EdgeColorCurve;
        private readonly List<float> _EdgeAlphaCurve;
        private readonly List<float> _StartEdgeAlphaCurve;
        private readonly List<float> _EndEdgeAlphaCurve;
        private int _NumSegments;
        private float _SegmentLength;
        private DrawInfo _DrawInfo;
        private int _TileUV;
        private float _SlipCurveSpeed;
        private float _SlipUVSpeed;
        private float _UVRepeat;
        private Vector3 _DirectionalForcesSum;
        private float _WindStrength;
        private float _GravityStrength;
        private ulong _EmitColorMapId;
        private ulong _ForceMapId;
        private float _MapRepulseStrength;
        #endregion

        public RibbonComponent()
        {
            this._OffsetCurve = new List<float>();
            this._WidthCurve = new List<float>();
            this._ColorCurve = new List<Vector3>();
            this._AlphaCurve = new List<float>();
            this._LengthColorCurve = new List<Vector3>();
            this._LengthAlphaCurve = new List<float>();
            this._EdgeColorCurve = new List<Vector3>();
            this._EdgeAlphaCurve = new List<float>();
            this._StartEdgeAlphaCurve = new List<float>();
            this._EndEdgeAlphaCurve = new List<float>();
        }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Flags);
            this.Flags &= 0x3FFF;

            Binary.Read(input, out this._RibbonLifetime);
            Binary.Read____(input, this._OffsetCurve);
            Binary.Read____(input, this._WidthCurve);
            Binary.Read(input, out this._Taper);
            Binary.Read(input, out this._Fade);
            Binary.Read(input, out this._AlphaDecay);
            Binary.Read____(input, this._ColorCurve);
            Binary.Read____(input, this._AlphaCurve);
            Binary.Read____(input, this._LengthColorCurve);
            Binary.Read____(input, this._LengthAlphaCurve);
            Binary.Read____(input, this._EdgeColorCurve);
            Binary.Read____(input, this._EdgeAlphaCurve);
            Binary.Read____(input, this._StartEdgeAlphaCurve);
            Binary.Read____(input, this._EndEdgeAlphaCurve);
            Binary.Read(input, out this._NumSegments);
            Binary.Read(input, out this._SegmentLength);
            Binary.Read(input, out this._DrawInfo);
            Binary.Read(input, out this._TileUV);
            Binary.Read(input, out this._SlipCurveSpeed);
            Binary.Read(input, out this._SlipUVSpeed);

            if (version >= 2)
            {
                Binary.Read(input, out this._UVRepeat);
            }
            else
            {
                this.UvRepeat = 1.0f;
            }

            Binary.Read(input, out this._DirectionalForcesSum);
            Binary.Read(input, out this._WindStrength);
            Binary.Read(input, out this._GravityStrength);
            Binary.Read(input, out this._EmitColorMapId);
            Binary.Read(input, out this._ForceMapId);
            Binary.Read(input, out this._MapRepulseStrength);
        }
    }
}
