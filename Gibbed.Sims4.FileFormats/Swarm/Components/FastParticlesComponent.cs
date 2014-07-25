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
    public class FastParticlesComponent : IComponent
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

        public Vector2 ParticleLifetime
        {
            get { return this._ParticleLifetime; }
            set { this._ParticleLifetime = value; }
        }

        public float PrerollTime
        {
            get { return this._PrerollTime; }
            set { this._PrerollTime = value; }
        }

        public Vector2 EmitDelay
        {
            get { return this._EmitDelay; }
            set { this._EmitDelay = value; }
        }

        public Vector2 EmitRetrigger
        {
            get { return this._EmitRetrigger; }
            set { this._EmitRetrigger = value; }
        }

        public BoundingBox EmitDirection
        {
            get { return this._EmitDirection; }
            set { this._EmitDirection = value; }
        }

        public Vector2 EmitSpeed
        {
            get { return this._EmitSpeed; }
            set { this._EmitSpeed = value; }
        }

        public BoundingBox EmitVolume
        {
            get { return this._EmitVolume; }
            set { this._EmitVolume = value; }
        }

        public List<float> RateCurve
        {
            get { return this._RateCurve; }
        }

        public float RateCurveTime
        {
            get { return this._RateCurveTime; }
            set { this._RateCurveTime = value; }
        }

        public ushort RateCurveCycles
        {
            get { return this._RateCurveCycles; }
            set { this._RateCurveCycles = value; }
        }

        public float RateSpeedScale
        {
            get { return this._RateSpeedScale; }
            set { this._RateSpeedScale = value; }
        }

        public List<float> SizeCurve
        {
            get { return this._SizeCurve; }
        }

        public List<Vector3> ColorCurve
        {
            get { return this._ColorCurve; }
        }

        public List<float> AlphaCurve
        {
            get { return this._AlphaCurve; }
        }

        public DrawInfo DrawInfo
        {
            get { return this._DrawInfo; }
            set { this._DrawInfo = value; }
        }

        public byte AlignMode
        {
            get { return this._AlignMode; }
            set { this._AlignMode = value; }
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

        public float RadialForce
        {
            get { return this._RadialForce; }
            set { this._RadialForce = value; }
        }

        public Vector3 RadialForceLocation
        {
            get { return this._RadialForceLocation; }
            set { this._RadialForceLocation = value; }
        }

        public float Drag
        {
            get { return this._Drag; }
            set { this._Drag = value; }
        }
        #endregion

        #region Fields
        private uint _Flags;
        private Vector2 _ParticleLifetime;
        private float _PrerollTime;
        private Vector2 _EmitDelay;
        private Vector2 _EmitRetrigger;
        private BoundingBox _EmitDirection;
        private Vector2 _EmitSpeed;
        private BoundingBox _EmitVolume;
        private readonly List<float> _RateCurve;
        private float _RateCurveTime;
        private ushort _RateCurveCycles;
        private float _RateSpeedScale;
        private readonly List<float> _SizeCurve;
        private readonly List<Vector3> _ColorCurve;
        private readonly List<float> _AlphaCurve;
        private DrawInfo _DrawInfo;
        private byte _AlignMode;
        private Vector3 _DirectionalForcesSum;
        private float _WindStrength;
        private float _GravityStrength;
        private float _RadialForce;
        private Vector3 _RadialForceLocation;
        private float _Drag;
        #endregion

        public FastParticlesComponent()
        {
            this._RateCurve = new List<float>();
            this._SizeCurve = new List<float>();
            this._ColorCurve = new List<Vector3>();
            this._AlphaCurve = new List<float>();
        }

        public void Serialize(Stream output, short version)
        {
            Binary.Write(output, this._Flags);
            Binary.Write(output, this._ParticleLifetime);
            Binary.Write(output, this._PrerollTime);
            Binary.Write(output, this._EmitDelay);
            Binary.Write(output, this._EmitRetrigger);
            Binary.Write(output, this._EmitDirection);
            Binary.Write(output, this._EmitSpeed);
            Binary.Write(output, this._EmitVolume);
            Binary.Write(output, this._RateCurve);
            Binary.Write(output, this._RateCurveTime);
            Binary.Write(output, this._RateCurveCycles);
            Binary.Write(output, this._RateSpeedScale);
            Binary.Write(output, this._SizeCurve);
            Binary.Write(output, this._ColorCurve);
            Binary.Write(output, this._AlphaCurve);
            Binary.Write(output, this._DrawInfo);
            Binary.Write(output, this._AlignMode);
            Binary.Write(output, this._DirectionalForcesSum);
            Binary.Write(output, this._WindStrength);
            Binary.Write(output, this._GravityStrength);
            Binary.Write(output, this._RadialForce);
            Binary.Write(output, this._RadialForceLocation);
            Binary.Write(output, this._Drag);
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Flags);
            this._Flags &= 0x1FFF;

            Binary.Read(input, out this._ParticleLifetime);
            Binary.Read(input, out this._PrerollTime);
            Binary.Read(input, out this._EmitDelay);
            Binary.Read(input, out this._EmitRetrigger);
            Binary.Read(input, out this._EmitDirection);
            Binary.Read(input, out this._EmitSpeed);
            Binary.Read(input, out this._EmitVolume);
            Binary.Read____(input, this._RateCurve);
            Binary.Read(input, out this._RateCurveTime);
            Binary.Read(input, out this._RateCurveCycles);
            Binary.Read(input, out this._RateSpeedScale);
            Binary.Read____(input, this._SizeCurve);
            Binary.Read____(input, this._ColorCurve);
            Binary.Read____(input, this._AlphaCurve);
            Binary.Read(input, out this._DrawInfo);
            Binary.Read(input, out this._AlignMode);
            Binary.Read(input, out this._DirectionalForcesSum);
            Binary.Read(input, out this._WindStrength);
            Binary.Read(input, out this._GravityStrength);
            Binary.Read(input, out this._RadialForce);
            Binary.Read(input, out this._RadialForceLocation);
            Binary.Read(input, out this._Drag);
        }
    }
}
