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
using System.IO;

namespace Gibbed.Sims4.FileFormats.Swarm
{
    public class PathPoint : ISerializable
    {
        #region Properties
        public Vector3 Position
        {
            get { return this._Position; }
            set { this._Position = value; }
        }

        public Vector3 Velocity
        {
            get { return this._Velocity; }
            set { this._Velocity = value; }
        }

        public float Time
        {
            get { return this._Time; }
            set { this._Time = value; }
        }
        #endregion

        #region Fields
        private Vector3 _Position;
        private Vector3 _Velocity;
        private float _Time;
        #endregion

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            Binary.Read(input, out this._Position);
            Binary.Read(input, out this._Velocity);
            Binary.Read(input, out this._Time);
        }
    }
}
