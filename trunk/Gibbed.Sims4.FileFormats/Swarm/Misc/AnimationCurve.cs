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
    public class AnimationCurve : ISerializable
    {
        #region Properties
        public Vector2 LengthRange
        {
            get { return this._LengthRange; }
            set { this._LengthRange = value; }
        }

        public List<float> Curve
        {
            get { return this._Curve; }
        }

        public float CurveVary
        {
            get { return this._CurveVary; }
            set { this._CurveVary = value; }
        }

        public float SpeedScale
        {
            get { return this._SpeedScale; }
            set { this._SpeedScale = value; }
        }

        public byte ChannelId
        {
            get { return this._ChannelId; }
            set { this._ChannelId = value; }
        }

        public byte Mode
        {
            get { return this._Mode; }
            set { this._Mode = value; }
        }
        #endregion

        #region Fields
        private Vector2 _LengthRange;
        private readonly List<float> _Curve;
        private float _CurveVary;
        private float _SpeedScale;
        private byte _ChannelId;
        private byte _Mode;
        #endregion

        public AnimationCurve()
        {
            this._Curve = new List<float>();
        }

        public void Serialize(Stream output)
        {
            Binary.Write(output, this._LengthRange);
            Binary.Write(output, this._Curve);
            Binary.Write(output, this._CurveVary);
            Binary.Write(output, this._SpeedScale);
            Binary.Write(output, this._ChannelId);
            Binary.Write(output, this._Mode);
        }

        public void Deserialize(Stream input)
        {
            Binary.Read(input, out this._LengthRange);
            Binary.Read____(input, this._Curve);
            Binary.Read(input, out this._CurveVary);
            Binary.Read(input, out this._SpeedScale);
            Binary.Read(input, out this._ChannelId);
            Binary.Read(input, out this._Mode);
        }
    }
}
