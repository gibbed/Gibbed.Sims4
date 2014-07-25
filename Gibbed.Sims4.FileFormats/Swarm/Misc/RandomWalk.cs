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
    public class RandomWalk : ISerializable
    {
        #region Properties
        public Vector2 Time
        {
            get { return this._Time; }
            set { this._Time = value; }
        }

        public Vector2 Strength
        {
            get { return this._Strength; }
            set { this._Strength = value; }
        }

        public float TurnRange
        {
            get { return this._TurnRange; }
            set { this._TurnRange = value; }
        }

        public float TurnOffset
        {
            get { return this._TurnOffset; }
            set { this._TurnOffset = value; }
        }

        public float Mix
        {
            get { return this._Mix; }
            set { this._Mix = value; }
        }

        public List<float> TurnOffsets
        {
            get { return this._TurnOffsets; }
        }

        public byte WalkLoopType
        {
            get { return this._WalkLoopType; }
            set { this._WalkLoopType = value; }
        }
        #endregion

        #region Fields
        private Vector2 _Time;
        private Vector2 _Strength;
        private float _TurnRange;
        private float _TurnOffset;
        private float _Mix;
        private readonly List<float> _TurnOffsets;
        private byte _WalkLoopType;
        #endregion

        public RandomWalk()
        {
            this._TurnOffsets = new List<float>();
        }

        public void Serialize(Stream output)
        {
            Binary.Write(output, this._Time);
            Binary.Write(output, this._Strength);
            Binary.Write(output, this._TurnRange);
            Binary.Write(output, this._TurnOffset);
            Binary.Write(output, this._Mix);
            Binary.Write(output, this._TurnOffsets);
            Binary.Write(output, this._WalkLoopType);
        }

        public void Deserialize(Stream input)
        {
            Binary.Read(input, out this._Time);
            Binary.Read(input, out this._Strength);
            Binary.Read(input, out this._TurnRange);
            Binary.Read(input, out this._TurnOffset);
            Binary.Read(input, out this._Mix);
            Binary.Read____(input, this._TurnOffsets);
            Binary.Read(input, out this._WalkLoopType);
        }
    }
}
