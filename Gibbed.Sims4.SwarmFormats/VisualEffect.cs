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

namespace Gibbed.Sims4.SwarmFormats
{
    public class VisualEffect : IDescription
    {
        #region Properties
        public uint Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public uint ComponentAppFlagsMask
        {
            get { return this._ComponentAppFlagsMask; }
            set { this._ComponentAppFlagsMask = value; }
        }

        public uint NotifyMessageId
        {
            get { return this._NotifyMessageId; }
            set { this._NotifyMessageId = value; }
        }

        public Vector2 ScreenSizeRange
        {
            get { return this._ScreenSizeRange; }
            set { this._ScreenSizeRange = value; }
        }

        public float CursorActiveDistance
        {
            get { return this._CursorActiveDistance; }
            set { this._CursorActiveDistance = value; }
        }

        public byte CursorButton
        {
            get { return this._CursorButton; }
            set { this._CursorButton = value; }
        }

        public List<float> LODDistances
        {
            get { return this._LODDistances; }
        }

        public Vector3 ExtendedLODWeights
        {
            get { return this._ExtendedLODWeights; }
            set { this._ExtendedLODWeights = value; }
        }

        public uint Seed
        {
            get { return this._Seed; }
            set { this._Seed = value; }
        }

        public List<Description> Descriptions
        {
            get { return this._Descriptions; }
        }
        #endregion

        #region Fields
        private uint _Flags;
        private uint _ComponentAppFlagsMask;
        private uint _NotifyMessageId;
        private Vector2 _ScreenSizeRange;
        private float _CursorActiveDistance;
        private byte _CursorButton;
        private readonly List<float> _LODDistances;
        private Vector3 _ExtendedLODWeights;
        private uint _Seed;
        private readonly List<Description> _Descriptions;
        #endregion

        public VisualEffect()
        {
            this._LODDistances = new List<float>();
            this._Descriptions = new List<Description>();
        }

        public void Serialize(Stream output, short version)
        {
            Binary.Write(output, this._Flags);
            Binary.Write(output, this._ComponentAppFlagsMask);
            Binary.Write(output, this._NotifyMessageId);
            Binary.Write(output, this._ScreenSizeRange);
            Binary.Write(output, this._CursorActiveDistance);
            Binary.Write(output, this._CursorButton);
            Binary.Write(output, this._LODDistances);
            Binary.Write(output, this._ExtendedLODWeights);
            Binary.Write(output, this._Seed);
            Binary.Write(output, this._Descriptions, version);
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Flags);
            this._Flags &= 0xFFFFFF;

            Binary.Read(input, out this._ComponentAppFlagsMask);
            Binary.Read(input, out this._NotifyMessageId);
            Binary.Read(input, out this._ScreenSizeRange);
            Binary.Read(input, out this._CursorActiveDistance);
            Binary.Read(input, out this._CursorButton);
            Binary.Read____(input, this._LODDistances);
            Binary.Read(input, out this._ExtendedLODWeights);
            Binary.Read(input, out this._Seed);
            Binary.Read____(input, this._Descriptions, version);
        }

        public class Description : IVersionedSerializable
        {
            #region Properties
            public ComponentType ComponentType
            {
                get { return (ComponentType)this._ComponentType; }
                set { this._ComponentType = (byte)value; }
            }

            public uint Flags
            {
                get { return this._Flags; }
                set { this._Flags = value; }
            }

            public Transform LocalXForm
            {
                get { return this._LocalXForm; }
                set { this._LocalXForm = value; }
            }

            public byte LODBegin
            {
                get { return this._LODBegin; }
                set { this._LODBegin = value; }
            }

            public byte LODEnd
            {
                get { return this._LODEnd; }
                set { this._LODEnd = value; }
            }

            public List<LODScale> LODScales
            {
                get { return this._LODScales; }
            }

            public float EmitScaleBegin
            {
                get { return this._EmitScaleBegin; }
                set { this._EmitScaleBegin = value; }
            }

            public float EmitScaleEnd
            {
                get { return this._EmitScaleEnd; }
                set { this._EmitScaleEnd = value; }
            }

            public float SizeScaleBegin
            {
                get { return this._SizeScaleBegin; }
                set { this._SizeScaleBegin = value; }
            }

            public float SizeScaleEnd
            {
                get { return this._SizeScaleEnd; }
                set { this._SizeScaleEnd = value; }
            }

            public float AlphaScaleBegin
            {
                get { return this._AlphaScaleBegin; }
                set { this._AlphaScaleBegin = value; }
            }

            public float AlphaScaleEnd
            {
                get { return this._AlphaScaleEnd; }
                set { this._AlphaScaleEnd = value; }
            }

            public uint AppFlags
            {
                get { return this._AppFlags; }
                set { this._AppFlags = value; }
            }

            public uint AppFlagsMask
            {
                get { return this._AppFlagsMask; }
                set { this._AppFlagsMask = value; }
            }

            public ushort SelectionGroup
            {
                get { return this._SelectionGroup; }
                set { this._SelectionGroup = value; }
            }

            public ushort SelectionChance
            {
                get { return this._SelectionChance; }
                set { this._SelectionChance = value; }
            }

            public float TimeScale
            {
                get { return this._TimeScale; }
                set { this._TimeScale = value; }
            }

            public int ComponentIndex
            {
                get { return this._ComponentIndex; }
                set { this._ComponentIndex = value; }
            }

            public byte Unknown62
            {
                get { return this._Unknown62; }
                set { this._Unknown62 = value; }
            }

            public byte Unknown63
            {
                get { return this._Unknown63; }
                set { this._Unknown63 = value; }
            }

            public List<float> Unknown9C
            {
                get { return this._Unknown9C; }
            }
            #endregion

            #region Fields
            private byte _ComponentType;
            private uint _Flags;
            private Transform _LocalXForm;
            private byte _LODBegin;
            private byte _LODEnd;
            private readonly List<LODScale> _LODScales;
            private float _EmitScaleBegin;
            private float _EmitScaleEnd;
            private float _SizeScaleBegin;
            private float _SizeScaleEnd;
            private float _AlphaScaleBegin;
            private float _AlphaScaleEnd;
            private uint _AppFlags;
            private uint _AppFlagsMask;
            private ushort _SelectionGroup;
            private ushort _SelectionChance;
            private float _TimeScale;
            private int _ComponentIndex;
            private byte _Unknown62;
            private byte _Unknown63;
            private readonly List<float> _Unknown9C;
            #endregion

            public Description()
            {
                this._LODScales = new List<LODScale>();
                this._Unknown9C = new List<float>();
            }

            public void Serialize(Stream output, short version)
            {
                Binary.Write(output, this._ComponentType);
                Binary.Write(output, this._Flags);
                Binary.Write(output, this._LocalXForm);
                Binary.Write(output, this._LODBegin);
                Binary.Write(output, this._LODEnd);
                Binary.Write(output, this._LODScales);
                Binary.Write(output, this._EmitScaleBegin);
                Binary.Write(output, this._EmitScaleEnd);
                Binary.Write(output, this._SizeScaleBegin);
                Binary.Write(output, this._SizeScaleEnd);
                Binary.Write(output, this._AlphaScaleBegin);
                Binary.Write(output, this._AlphaScaleEnd);
                Binary.Write(output, this._AppFlags);
                Binary.Write(output, this._AppFlagsMask);
                Binary.Write(output, this._SelectionGroup);
                Binary.Write(output, this._SelectionChance);
                Binary.Write(output, this._TimeScale);
                Binary.Write(output, this._ComponentIndex);

                if (version >= 2)
                {
                    Binary.Write(output, this._Unknown62);
                    Binary.Write(output, this._Unknown63);
                }

                if (version >= 3)
                {
                    Binary.Write(output, this._Unknown9C);
                }
            }

            public void Deserialize(Stream input, short version)
            {
                Binary.Read(input, out this._ComponentType);

                Binary.Read(input, out this._Flags);
                this._Flags &= 0x3FF;

                Binary.Read(input, out this._LocalXForm);
                Binary.Read(input, out this._LODBegin);
                Binary.Read(input, out this._LODEnd);
                Binary.Read____(input, this._LODScales);
                Binary.Read(input, out this._EmitScaleBegin);
                Binary.Read(input, out this._EmitScaleEnd);
                Binary.Read(input, out this._SizeScaleBegin);
                Binary.Read(input, out this._SizeScaleEnd);
                Binary.Read(input, out this._AlphaScaleBegin);
                Binary.Read(input, out this._AlphaScaleEnd);
                Binary.Read(input, out this._AppFlags);
                Binary.Read(input, out this._AppFlagsMask);
                Binary.Read(input, out this._SelectionGroup);
                Binary.Read(input, out this._SelectionChance);
                Binary.Read(input, out this._TimeScale);
                Binary.Read(input, out this._ComponentIndex);

                if (version >= 2)
                {
                    Binary.Read(input, out this._Unknown62);
                    Binary.Read(input, out this._Unknown63);
                }

                if (version >= 3)
                {
                    Binary.Read____(input, this._Unknown9C);
                }
            }
        }

        public class LODScale : ISerializable
        {
            #region Properties
            public float EmitScale
            {
                get { return this._EmitScale; }
                set { this._EmitScale = value; }
            }

            public float SizeScale
            {
                get { return this._SizeScale; }
                set { this._SizeScale = value; }
            }

            public float AlphaScale
            {
                get { return this._AlphaScale; }
                set { this._AlphaScale = value; }
            }
            #endregion

            #region Fields
            private float _EmitScale;
            private float _SizeScale;
            private float _AlphaScale;
            #endregion

            public void Serialize(Stream output)
            {
                Binary.Write(output, this._EmitScale);
                Binary.Write(output, this._SizeScale);
                Binary.Write(output, this._AlphaScale);
            }

            public void Deserialize(Stream input)
            {
                Binary.Read(input, out this._EmitScale);
                Binary.Read(input, out this._SizeScale);
                Binary.Read(input, out this._AlphaScale);
            }
        }
    }
}
