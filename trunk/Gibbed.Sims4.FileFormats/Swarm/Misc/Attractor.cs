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

namespace Gibbed.Sims4.FileFormats.Swarm
{
    public class Attractor : ISerializable
    {
        #region Properties
        public List<float> StrengthCurve
        {
            get { return this._StrengthCurve; }
        }

        public float Range
        {
            get { return this._Range; }
            set { this._Range = value; }
        }

        public float KillRange
        {
            get { return this._KillRange; }
            set { this._KillRange = value; }
        }
        #endregion

        #region Fields
        private readonly List<float> _StrengthCurve;
        private float _Range;
        private float _KillRange;
        #endregion

        public Attractor()
        {
            this._StrengthCurve = new List<float>();
        }

        public void Serialize(Stream output)
        {
            Binary.Write(output, this._StrengthCurve);
            Binary.Write(output, this._Range);
            Binary.Write(output, this._KillRange);
        }

        public void Deserialize(Stream input)
        {
            Binary.Read____(input, this._StrengthCurve);
            Binary.Read(input, out this._Range);
            Binary.Read(input, out this._KillRange);
        }
    }
}
