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
    public class ShakeComponent : IComponent
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

        public float Lifetime
        {
            get { return this._Lifetime; }
            set { this._Lifetime = value; }
        }

        public float FadeTime
        {
            get { return this._FadeTime; }
            set { this._FadeTime = value; }
        }

        public List<float> StrengthCurve
        {
            get { return this._StrengthCurve; }
        }

        public List<float> FrequencyCurve
        {
            get { return this._FrequencyCurve; }
        }

        public float AspectRatio
        {
            get { return this._AspectRatio; }
            set { this._AspectRatio = value; }
        }

        public byte BaseTableType
        {
            get { return this._BaseTableType; }
            set { this._BaseTableType = value; }
        }

        public float Falloff
        {
            get { return this._Falloff; }
            set { this._Falloff = value; }
        }
        #endregion

        #region Fields
        private float _Lifetime;
        private float _FadeTime;
        private readonly List<float> _StrengthCurve;
        private readonly List<float> _FrequencyCurve;
        private float _AspectRatio;
        private byte _BaseTableType;
        private float _Falloff;
        #endregion

        public ShakeComponent()
        {
            this._StrengthCurve = new List<float>();
            this._FrequencyCurve = new List<float>();
        }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Lifetime);
            Binary.Read(input, out this._FadeTime);
            Binary.Read____(input, this._StrengthCurve);
            Binary.Read____(input, this._FrequencyCurve);
            Binary.Read(input, out this._AspectRatio);
            Binary.Read(input, out this._BaseTableType);
            Binary.Read(input, out this._Falloff);
        }
    }
}
