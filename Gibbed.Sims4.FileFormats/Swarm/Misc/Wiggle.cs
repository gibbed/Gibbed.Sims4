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
    public class Wiggle : ISerializable
    {
        #region Properties
        public uint TimeRate
        {
            get { return this._TimeRate; }
            set { this._TimeRate = value; }
        }

        public Vector3 RateDir
        {
            get { return this._RateDir; }
            set { this._RateDir = value; }
        }

        public Vector3 WiggleDir
        {
            get { return this._WiggleDir; }
            set { this._WiggleDir = value; }
        }
        #endregion

        #region Fields
        private uint _TimeRate;
        private Vector3 _RateDir;
        private Vector3 _WiggleDir;
        #endregion

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            Binary.Read(input, out this._TimeRate);
            Binary.Read(input, out this._RateDir);
            Binary.Read(input, out this._WiggleDir);
        }
    }
}
