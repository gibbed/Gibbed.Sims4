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
    public class ParticlesComponent : IComponent
    {
        #region Properties
        public short MinimumVersion
        {
            get { return 1; }
        }

        public short MaximumVersion
        {
            get { return 7; }
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

        public float EmitTorusWidth
        {
            get { return this._EmitTorusWidth; }
            set { this._EmitTorusWidth = value; }
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

        public float SizeVary
        {
            get { return this._SizeVary; }
            set { this._SizeVary = value; }
        }

        public List<float> AspectCurve
        {
            get { return this._AspectCurve; }
        }

        public float AspectVary
        {
            get { return this._AspectVary; }
            set { this._AspectVary = value; }
        }

        public float RotationVary
        {
            get { return this._RotationVary; }
            set { this._RotationVary = value; }
        }

        public float RotationOffset
        {
            get { return this._RotationOffset; }
            set { this._RotationOffset = value; }
        }

        public List<float> RotationCurve
        {
            get { return this._RotationCurve; }
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

        public List<Vector3> ColorCurve
        {
            get { return this._ColorCurve; }
        }

        public Vector3 ColorVary
        {
            get { return this._ColorVary; }
            set { this._ColorVary = value; }
        }

        public DrawInfo DrawInfo
        {
            get { return this._DrawInfo; }
            set { this._DrawInfo = value; }
        }

        public byte Unknown1DC
        {
            get { return this._Unknown1DC; }
            set { this._Unknown1DC = value; }
        }

        public uint Unknown1F0
        {
            get { return this._Unknown1F0; }
            set { this._Unknown1F0 = value; }
        }

        public byte Unknown1DD
        {
            get { return this._Unknown1DD; }
            set { this._Unknown1DD = value; }
        }

        public ushort Unknown1DE
        {
            get { return this._Unknown1DE; }
            set { this._Unknown1DE = value; }
        }

        public byte Unknown1E0
        {
            get { return this._Unknown1E0; }
            set { this._Unknown1E0 = value; }
        }

        public ushort Unknown1E2
        {
            get { return this._Unknown1E2; }
            set { this._Unknown1E2 = value; }
        }

        public uint Unknown1E4
        {
            get { return this._Unknown1E4; }
            set { this._Unknown1E4 = value; }
        }

        public ulong Unknown1E8
        {
            get { return this._Unknown1E8; }
            set { this._Unknown1E8 = value; }
        }

        public byte PhysicsType
        {
            get { return this._PhysicsType; }
            set { this._PhysicsType = value; }
        }

        public byte OverrideSet
        {
            get { return this._OverrideSet; }
            set { this._OverrideSet = value; }
        }

        public byte TileCountU
        {
            get { return this._TileCountU; }
            set { this._TileCountU = value; }
        }

        public byte TileCountV
        {
            get { return this._TileCountV; }
            set { this._TileCountV = value; }
        }

        public byte AlignMode
        {
            get { return this._AlignMode; }
            set { this._AlignMode = value; }
        }

        public float FrameSpeed
        {
            get { return this._FrameSpeed; }
            set { this._FrameSpeed = value; }
        }

        public byte FrameStart
        {
            get { return this._FrameStart; }
            set { this._FrameStart = value; }
        }

        public byte FrameCount
        {
            get { return this._FrameCount; }
            set { this._FrameCount = value; }
        }

        public byte FrameRandom
        {
            get { return this._FrameRandom; }
            set { this._FrameRandom = value; }
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

        public float VelocityStretch
        {
            get { return this._VelocityStretch; }
            set { this._VelocityStretch = value; }
        }

        public float ScrewRate
        {
            get { return this._ScrewRate; }
            set { this._ScrewRate = value; }
        }

        public List<Wiggle> Wiggles
        {
            get { return this._Wiggles; }
        }

        public byte ScreenBloomAlphaRate
        {
            get { return this._ScreenBloomAlphaRate; }
            set { this._ScreenBloomAlphaRate = value; }
        }

        public byte ScreenBloomAlphaBase
        {
            get { return this._ScreenBloomAlphaBase; }
            set { this._ScreenBloomAlphaBase = value; }
        }

        public byte ScreenBloomSizeRate
        {
            get { return this._ScreenBloomSizeRate; }
            set { this._ScreenBloomSizeRate = value; }
        }

        public byte ScreenBloomSizeBase
        {
            get { return this._ScreenBloomSizeBase; }
            set { this._ScreenBloomSizeBase = value; }
        }

        public List<Vector3> LoopBoxColorCurve
        {
            get { return this._LoopBoxColorCurve; }
        }

        public List<float> LoopBoxAlphaCurve
        {
            get { return this._LoopBoxAlphaCurve; }
        }

        public List<SurfaceInfo> Surfaces
        {
            get { return this._Surfaces; }
        }

        public float MapBounce
        {
            get { return this._MapBounce; }
            set { this._MapBounce = value; }
        }

        public float MapRepulseHeight
        {
            get { return this._MapRepulseHeight; }
            set { this._MapRepulseHeight = value; }
        }

        public float MapRepulseStrength
        {
            get { return this._MapRepulseStrength; }
            set { this._MapRepulseStrength = value; }
        }

        public float MapRepulseScoutDistance
        {
            get { return this._MapRepulseScoutDistance; }
            set { this._MapRepulseScoutDistance = value; }
        }

        public float MapRepulseVertical
        {
            get { return this._MapRepulseVertical; }
            set { this._MapRepulseVertical = value; }
        }

        public float MapRepulseKillHeight
        {
            get { return this._MapRepulseKillHeight; }
            set { this._MapRepulseKillHeight = value; }
        }

        public float ProbDeath
        {
            get { return this._ProbDeath; }
            set { this._ProbDeath = value; }
        }

        public Vector2 AltitudeRange
        {
            get { return this._AltitudeRange; }
            set { this._AltitudeRange = value; }
        }

        public ulong ForceMapId
        {
            get { return this._ForceMapId; }
            set { this._ForceMapId = value; }
        }

        public ulong EmitRateMapId
        {
            get { return this._EmitRateMapId; }
            set { this._EmitRateMapId = value; }
        }

        public ulong EmitColorMapId
        {
            get { return this._EmitColorMapId; }
            set { this._EmitColorMapId = value; }
        }

        public RandomWalk RandomWalk
        {
            get { return this._RandomWalk; }
            set { this._RandomWalk = value; }
        }

        public Vector3 AttractorOrigin
        {
            get { return this._AttractorOrigin; }
            set { this._AttractorOrigin = value; }
        }

        public Attractor Attractor
        {
            get { return this._Attractor; }
            set { this._Attractor = value; }
        }

        public List<PathPoint> PathPoints
        {
            get { return this._PathPoints; }
        }

        public Vector3 Unknown0F0
        {
            get { return this._Unknown0F0; }
            set { this._Unknown0F0 = value; }
        }

        public List<Vector3> Unknown0E0
        {
            get { return this._Unknown0E0; }
        }

        public byte Unknown100
        {
            get { return this._Unknown100; }
            set { this._Unknown100 = value; }
        }

        public uint Unknown340
        {
            get { return this._Unknown340; }
            set { this._Unknown340 = value; }
        }

        public Vector3 Unknown130
        {
            get { return this._Unknown130; }
            set { this._Unknown130 = value; }
        }

        public byte Unknown170
        {
            get { return this._Unknown170; }
            set { this._Unknown170 = value; }
        }

        public Vector2 Unknown140
        {
            get { return this._Unknown140; }
            set { this._Unknown140 = value; }
        }

        public Vector2 Unknown150
        {
            get { return this._Unknown150; }
            set { this._Unknown150 = value; }
        }

        public uint Unknown160
        {
            get { return this._Unknown160; }
            set { this._Unknown160 = value; }
        }

        public uint Unknown164
        {
            get { return this._Unknown164; }
            set { this._Unknown164 = value; }
        }

        public bool Unknown1C1
        {
            get { return this._Unknown1C1; }
            set { this._Unknown1C1 = value; }
        }

        public Vector3 Unknown180
        {
            get { return this._Unknown180; }
            set { this._Unknown180 = value; }
        }

        public byte Unknown1C0
        {
            get { return this._Unknown1C0; }
            set { this._Unknown1C0 = value; }
        }

        public Vector2 Unknown190
        {
            get { return this._Unknown190; }
            set { this._Unknown190 = value; }
        }

        public Vector2 Unknown1A0
        {
            get { return this._Unknown1A0; }
            set { this._Unknown1A0 = value; }
        }

        public uint Unknown1B0
        {
            get { return this._Unknown1B0; }
            set { this._Unknown1B0 = value; }
        }

        public uint Unknown1B4
        {
            get { return this._Unknown1B4; }
            set { this._Unknown1B4 = value; }
        }

        public bool Unknown208
        {
            get { return this._Unknown208; }
            set { this._Unknown208 = value; }
        }

        public uint Unknown20C
        {
            get { return this._Unknown20C; }
            set { this._Unknown20C = value; }
        }

        public bool Unknown209
        {
            get { return this._Unknown209; }
            set { this._Unknown209 = value; }
        }

        public bool Unknown20A
        {
            get { return this._Unknown20A; }
            set { this._Unknown20A = value; }
        }

        public bool Unknown20B
        {
            get { return this._Unknown20B; }
            set { this._Unknown20B = value; }
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
        private float _EmitTorusWidth;
        private readonly List<float> _RateCurve;
        private float _RateCurveTime;
        private ushort _RateCurveCycles;
        private float _RateSpeedScale;
        private readonly List<float> _SizeCurve;
        private float _SizeVary;
        private readonly List<float> _AspectCurve;
        private float _AspectVary;
        private float _RotationVary;
        private float _RotationOffset;
        private readonly List<float> _RotationCurve;
        private readonly List<float> _AlphaCurve;
        private float _AlphaVary;
        private readonly List<Vector3> _ColorCurve;
        private Vector3 _ColorVary;
        private DrawInfo _DrawInfo;
        private byte _Unknown1DC;
        private uint _Unknown1F0;
        private byte _Unknown1DD;
        private ushort _Unknown1DE;
        private byte _Unknown1E0;
        private ushort _Unknown1E2;
        private uint _Unknown1E4;
        private ulong _Unknown1E8;
        private byte _PhysicsType;
        private byte _OverrideSet;
        private byte _TileCountU;
        private byte _TileCountV;
        private byte _AlignMode;
        private float _FrameSpeed;
        private byte _FrameStart;
        private byte _FrameCount;
        private byte _FrameRandom;
        private Vector3 _DirectionalForcesSum;
        private float _WindStrength;
        private float _GravityStrength;
        private float _RadialForce;
        private Vector3 _RadialForceLocation;
        private float _Drag;
        private float _VelocityStretch;
        private float _ScrewRate;
        private readonly List<Wiggle> _Wiggles;
        private byte _ScreenBloomAlphaRate;
        private byte _ScreenBloomAlphaBase;
        private byte _ScreenBloomSizeRate;
        private byte _ScreenBloomSizeBase;
        private readonly List<Vector3> _LoopBoxColorCurve;
        private readonly List<float> _LoopBoxAlphaCurve;
        private readonly List<SurfaceInfo> _Surfaces;
        private float _MapBounce;
        private float _MapRepulseHeight;
        private float _MapRepulseStrength;
        private float _MapRepulseScoutDistance;
        private float _MapRepulseVertical;
        private float _MapRepulseKillHeight;
        private float _ProbDeath;
        private Vector2 _AltitudeRange;
        private ulong _ForceMapId;
        private ulong _EmitRateMapId;
        private ulong _EmitColorMapId;
        private RandomWalk _RandomWalk;
        private Vector3 _AttractorOrigin;
        private Attractor _Attractor;
        private readonly List<PathPoint> _PathPoints;
        private Vector3 _Unknown0F0;
        private readonly List<Vector3> _Unknown0E0;
        private byte _Unknown100;
        private uint _Unknown340;
        private Vector3 _Unknown130;
        private byte _Unknown170;
        private Vector2 _Unknown140;
        private Vector2 _Unknown150;
        private uint _Unknown160;
        private uint _Unknown164;
        private bool _Unknown1C1;
        private Vector3 _Unknown180;
        private byte _Unknown1C0;
        private Vector2 _Unknown190;
        private Vector2 _Unknown1A0;
        private uint _Unknown1B0;
        private uint _Unknown1B4;
        private bool _Unknown208;
        private uint _Unknown20C;
        private bool _Unknown209;
        private bool _Unknown20A;
        private bool _Unknown20B;
        #endregion

        public ParticlesComponent()
        {
            this._RateCurve = new List<float>();
            this._SizeCurve = new List<float>();
            this._AspectCurve = new List<float>();
            this._RotationCurve = new List<float>();
            this._AlphaCurve = new List<float>();
            this._ColorCurve = new List<Vector3>();
            this._Wiggles = new List<Wiggle>();
            this._LoopBoxColorCurve = new List<Vector3>();
            this._LoopBoxAlphaCurve = new List<float>();
            this._Surfaces = new List<SurfaceInfo>();
            this._PathPoints = new List<PathPoint>();
            this._Unknown0E0 = new List<Vector3>();
        }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Flags);
            Binary.Read(input, out this._ParticleLifetime);
            Binary.Read(input, out this._PrerollTime);
            Binary.Read(input, out this._EmitDelay);
            Binary.Read(input, out this._EmitRetrigger);
            Binary.Read(input, out this._EmitDirection);
            Binary.Read(input, out this._EmitSpeed);
            Binary.Read(input, out this._EmitVolume);
            Binary.Read(input, out this._EmitTorusWidth);
            Binary.Read____(input, this._RateCurve);
            Binary.Read(input, out this._RateCurveTime);
            Binary.Read(input, out this._RateCurveCycles);
            Binary.Read(input, out this._RateSpeedScale);
            Binary.Read____(input, this._SizeCurve);
            Binary.Read(input, out this._SizeVary);
            Binary.Read____(input, this._AspectCurve);
            Binary.Read(input, out this._AspectVary);
            Binary.Read(input, out this._RotationVary);
            Binary.Read(input, out this._RotationOffset);
            Binary.Read____(input, this._RotationCurve);
            Binary.Read____(input, this._AlphaCurve);
            Binary.Read(input, out this._AlphaVary);
            Binary.Read____(input, this._ColorCurve);
            Binary.Read(input, out this._ColorVary);
            Binary.Read(input, out this._DrawInfo, version);
            Binary.Read(input, out this._PhysicsType);
            Binary.Read(input, out this._OverrideSet);
            Binary.Read(input, out this._TileCountU);
            Binary.Read(input, out this._TileCountV);
            Binary.Read(input, out this._AlignMode);
            Binary.Read(input, out this._FrameSpeed);
            Binary.Read(input, out this._FrameStart);
            Binary.Read(input, out this._FrameCount);
            Binary.Read(input, out this._FrameRandom);
            Binary.Read(input, out this._DirectionalForcesSum);
            Binary.Read(input, out this._WindStrength);
            Binary.Read(input, out this._GravityStrength);
            Binary.Read(input, out this._RadialForce);
            Binary.Read(input, out this._RadialForceLocation);
            Binary.Read(input, out this._Drag);
            Binary.Read(input, out this._VelocityStretch);
            Binary.Read(input, out this._ScrewRate);
            Binary.Read____(input, this._Wiggles);
            Binary.Read(input, out this._ScreenBloomAlphaRate);
            Binary.Read(input, out this._ScreenBloomAlphaBase);
            Binary.Read(input, out this._ScreenBloomSizeRate);
            Binary.Read(input, out this._ScreenBloomSizeBase);
            Binary.Read____(input, this._LoopBoxColorCurve);
            Binary.Read____(input, this._LoopBoxAlphaCurve);
            Binary.Read____(input, this._Surfaces);
            Binary.Read(input, out this._MapBounce);
            Binary.Read(input, out this._MapRepulseHeight);
            Binary.Read(input, out this._MapRepulseStrength);
            Binary.Read(input, out this._MapRepulseScoutDistance);
            Binary.Read(input, out this._MapRepulseVertical);
            Binary.Read(input, out this._MapRepulseKillHeight);
            Binary.Read(input, out this._ProbDeath);
            Binary.Read(input, out this._AltitudeRange);
            Binary.Read(input, out this._ForceMapId);
            Binary.Read(input, out this._EmitRateMapId);
            Binary.Read(input, out this._EmitColorMapId);
            Binary.Read(input, out this._RandomWalk);
            Binary.Read(input, out this._AttractorOrigin);
            Binary.Read(input, out this._Attractor);
            Binary.Read____(input, this._PathPoints);

            if (version >= 2)
            {
                Binary.Read(input, out this._Unknown0F0);
                Binary.Read____(input, this._Unknown0E0);
            }

            if (version >= 3)
            {
                Binary.Read(input, out this._Unknown100);
            }

            if (version >= 4)
            {
                Binary.Read(input, out this._Unknown340);
            }

            if (version >= 5)
            {
                Binary.Read(input, out this._Unknown130);
                Binary.Read(input, out this._Unknown170);
            }

            if (version >= 6)
            {
                Binary.Read(input, out this._Unknown140);
            }
            else
            {
                float value;
                Binary.Read(input, out value);
                this.Unknown140 = new Vector2(value, value);
                throw new NotImplementedException();
            }

            if (version >= 7)
            {
                Binary.Read(input, out this._Unknown150);
            }

            Binary.Read(input, out this._Unknown160);
            Binary.Read(input, out this._Unknown164);

            Binary.Read(input, out this._Unknown1C1);
            if (this.Unknown1C1 == true)
            {
                Binary.Read(input, out this._Unknown180);
                Binary.Read(input, out this._Unknown1C0);

                if (version >= 6)
                {
                    Binary.Read(input, out this._Unknown190);
                }
                else
                {
                    float value;
                    Binary.Read(input, out value);
                    this._Unknown190 = new Vector2(value, value);
                    throw new NotImplementedException();
                }

                if (version >= 7)
                {
                    Binary.Read(input, out this._Unknown1A0);
                }

                Binary.Read(input, out this._Unknown1B0);
                Binary.Read(input, out this._Unknown1B4);
            }

            if (version >= 6)
            {
                Binary.Read(input, out this._Unknown208);
                if (this._Unknown208 == true)
                {
                    Binary.Read(input, out this._Unknown20C);
                    Binary.Read(input, out this._Unknown209);
                    Binary.Read(input, out this._Unknown20A);
                    Binary.Read(input, out this._Unknown20B);
                }
            }
        }
    }
}
