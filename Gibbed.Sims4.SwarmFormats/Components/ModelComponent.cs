﻿/* Copyright (c) 2014 Rick (rick 'at' gibbed 'dot' us)
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
    public class ModelComponent : IComponent
    {
        #region Properties
        public uint Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public ulong ResourceId
        {
            get { return this._ResourceId; }
            set { this._ResourceId = value; }
        }

        public float Size
        {
            get { return this._Size; }
            set { this._Size = value; }
        }

        public Vector3 Colour
        {
            get { return this._Colour; }
            set { this._Colour = value; }
        }

        public float Alpha
        {
            get { return this._Alpha; }
            set { this._Alpha = value; }
        }

        public List<AnimationCurve> AnimationCurves
        {
            get { return this._AnimationCurves; }
        }

        public ulong MaterialId
        {
            get { return this._MaterialId; }
            set { this._MaterialId = value; }
        }

        public byte OverrideSet
        {
            get { return this._OverrideSet; }
            set { this._OverrideSet = value; }
        }
        #endregion

        #region Fields
        private uint _Flags;
        private ulong _ResourceId;
        private float _Size;
        private Vector3 _Colour;
        private float _Alpha;
        private readonly List<AnimationCurve> _AnimationCurves;
        private ulong _MaterialId;
        private byte _OverrideSet;
        #endregion

        public ModelComponent()
        {
            this._AnimationCurves = new List<AnimationCurve>();
        }

        public void Serialize(Stream output, short version)
        {
            Binary.Write(output, this._Flags);
            Binary.Write(output, this._ResourceId);
            Binary.Write(output, this._Size);
            Binary.Write(output, this._Colour);
            Binary.Write(output, this._Alpha);
            Binary.Write(output, this.AnimationCurves);
            Binary.Write(output, this._MaterialId);
            Binary.Write(output, this._OverrideSet);
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Flags);
            this._Flags &= 3;

            Binary.Read(input, out this._ResourceId);
            Binary.Read(input, out this._Size);
            Binary.Read(input, out this._Colour);
            Binary.Read(input, out this._Alpha);
            Binary.Read____(input, this.AnimationCurves);
            Binary.Read(input, out this._MaterialId);
            Binary.Read(input, out this._OverrideSet);
        }
    }
}
