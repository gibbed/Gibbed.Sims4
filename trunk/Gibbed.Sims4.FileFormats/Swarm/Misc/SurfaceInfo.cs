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
    public class SurfaceInfo : ISerializable
    {
        #region Properties
        public uint Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public ulong Id
        {
            get { return this._Id; }
            set { this._Id = value; }
        }

        public uint Bounce
        {
            get { return this._Bounce; }
            set { this._Bounce = value; }
        }

        public uint Slide
        {
            get { return this._Slide; }
            set { this._Slide = value; }
        }

        public uint CollisionRadius
        {
            get { return this._CollisionRadius; }
            set { this._CollisionRadius = value; }
        }

        public uint DeathProbability
        {
            get { return this._DeathProbability; }
            set { this._DeathProbability = value; }
        }

        public uint PinOffset
        {
            get { return this._PinOffset; }
            set { this._PinOffset = value; }
        }

        public string Unknown38
        {
            get { return this._Unknown38; }
            set { this._Unknown38 = value; }
        }

        public string Unknown48
        {
            get { return this._Unknown48; }
            set { this._Unknown48 = value; }
        }

        public List<Vector3> Data
        {
            get { return this._Data; }
        }
        #endregion

        #region Fields
        private uint _Flags;
        private ulong _Id;
        private uint _Bounce;
        private uint _Slide;
        private uint _CollisionRadius;
        private uint _DeathProbability;
        private uint _PinOffset;
        private string _Unknown38;
        private string _Unknown48;
        private readonly List<Vector3> _Data;
        #endregion

        public SurfaceInfo()
        {
            this._Data = new List<Vector3>();
        }

        public void Serialize(Stream output)
        {
            Binary.Write(output, this._Flags);
            Binary.Write(output, this._Id);
            Binary.Write(output, this._Bounce);
            Binary.Write(output, this._Slide);
            Binary.Write(output, this._CollisionRadius);
            Binary.Write(output, this._DeathProbability);
            Binary.Write(output, this._PinOffset);
            Binary.Write(output, this._Unknown38);
            Binary.Write(output, this._Unknown48);
            Binary.Write(output, this._Data);
        }

        public void Deserialize(Stream input)
        {
            Binary.Read(input, out this._Flags);
            this._Flags &= 0x3FFF;

            Binary.Read(input, out this._Id);
            Binary.Read(input, out this._Bounce);
            Binary.Read(input, out this._Slide);
            Binary.Read(input, out this._CollisionRadius);
            Binary.Read(input, out this._DeathProbability);
            Binary.Read(input, out this._PinOffset);
            Binary.Read(input, out this._Unknown38);
            Binary.Read(input, out this._Unknown48);
            Binary.Read____(input, this._Data);
        }
    }
}
