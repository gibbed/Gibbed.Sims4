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
    public class MetaParticlesComponent : IComponent
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

        public ulong Flags
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

        public float RateCurveCycles
        {
            get { return this._RateCurveCycles; }
            set { this._RateCurveCycles = value; }
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

        public string ComponentName
        {
            get { return this._ComponentName; }
            set { this._ComponentName = value; }
        }

        public string ComponentType
        {
            get { return this._ComponentType; }
            set { this._ComponentType = value; }
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

        public Vector3 GlobalForcesSum
        {
            get { return this._GlobalForcesSum; }
            set { this._GlobalForcesSum = value; }
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

        public RandomWalk[] RandomWalks
        {
            get { return this._RandomWalks; }
        }

        public Vector3 RwPreferredDir
        {
            get { return this._RWPreferredDir; }
            set { this._RWPreferredDir = value; }
        }

        public float AlignDamping
        {
            get { return this._AlignDamping; }
            set { this._AlignDamping = value; }
        }

        public float BankAmount
        {
            get { return this._BankAmount; }
            set { this._BankAmount = value; }
        }

        public float BankRestore
        {
            get { return this._BankRestore; }
            set { this._BankRestore = value; }
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

        public float TractorResetSpeed
        {
            get { return this._TractorResetSpeed; }
            set { this._TractorResetSpeed = value; }
        }
        #endregion

        #region Fields
        private ulong _Flags;
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
        private float _RateCurveCycles;
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
        private string _ComponentName;
        private string _ComponentType;
        private byte _AlignMode;
        private Vector3 _DirectionalForcesSum;
        private Vector3 _GlobalForcesSum;
        private float _WindStrength;
        private float _GravityStrength;
        private float _RadialForce;
        private Vector3 _RadialForceLocation;
        private float _Drag;
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
        private readonly RandomWalk[] _RandomWalks;
        private Vector3 _RWPreferredDir;
        private float _AlignDamping;
        private float _BankAmount;
        private float _BankRestore;
        private Vector3 _AttractorOrigin;
        private Attractor _Attractor;
        private readonly List<PathPoint> _PathPoints;
        private float _TractorResetSpeed;
        #endregion

        public MetaParticlesComponent()
        {
            this._RateCurve = new List<float>();
            this._SizeCurve = new List<float>();
            this._PitchCurve = new List<float>();
            this._RollCurve = new List<float>();
            this._HeadingCurve = new List<float>();
            this._ColorCurve = new List<Vector3>();
            this._AlphaCurve = new List<float>();
            this._Wiggles = new List<Wiggle>();
            this._LoopBoxColorCurve = new List<Vector3>();
            this._LoopBoxAlphaCurve = new List<float>();
            this._Surfaces = new List<SurfaceInfo>();
            this._RandomWalks = new RandomWalk[2];
            this._PathPoints = new List<PathPoint>();
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
            Binary.Read(input, out this._ComponentName);
            Binary.Read(input, out this._ComponentType);
            Binary.Read(input, out this._AlignMode);
            Binary.Read(input, out this._DirectionalForcesSum);
            Binary.Read(input, out this._GlobalForcesSum);
            Binary.Read(input, out this._WindStrength);
            Binary.Read(input, out this._GravityStrength);
            Binary.Read(input, out this._RadialForce);
            Binary.Read(input, out this._RadialForceLocation);
            Binary.Read(input, out this._Drag);
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
            Binary.Read____(input, this._RandomWalks);
            Binary.Read(input, out this._RWPreferredDir);
            Binary.Read(input, out this._AlignDamping);
            Binary.Read(input, out this._BankAmount);
            Binary.Read(input, out this._BankRestore);
            Binary.Read(input, out this._AttractorOrigin);
            Binary.Read(input, out this._Attractor);
            Binary.Read____(input, this._PathPoints);
            Binary.Read(input, out this._TractorResetSpeed);
        }
    }
}
