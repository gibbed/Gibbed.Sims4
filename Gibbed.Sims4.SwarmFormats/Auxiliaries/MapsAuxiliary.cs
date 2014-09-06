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

using System.IO;

namespace Gibbed.Sims4.SwarmFormats.Auxiliaries
{
    public class MapsAuxiliary : IAuxiliary
    {
        #region Properties
        public uint Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public byte MapType
        {
            get { return this._MapType; }
            set { this._MapType = value; }
        }

        public ulong ImageId
        {
            get { return this._ImageId; }
            set { this._ImageId = value; }
        }

        public Rectangle Bounds
        {
            get { return this._Bounds; }
            set { this._Bounds = value; }
        }

        public byte Channel
        {
            get { return this._Channel; }
            set { this._Channel = value; }
        }

        public byte OpKind
        {
            get { return this._OpKind; }
            set { this._OpKind = value; }
        }

        public ulong[] OpArgMapId
        {
            get { return this._OpArgMapId; }
        }

        public Vector4[] OpArgValue
        {
            get { return this._OpArgValue; }
        }
        #endregion

        #region Fields
        private ulong _MapId;
        private uint _Flags;
        private byte _MapType;
        private ulong _ImageId;
        private Rectangle _Bounds;
        private byte _Channel;
        private byte _OpKind;
        private readonly ulong[] _OpArgMapId;
        private readonly Vector4[] _OpArgValue;
        #endregion

        public MapsAuxiliary()
        {
            this._OpArgMapId = new ulong[4];
            this._OpArgValue = new Vector4[4];
        }

        public void Serialize(Stream output, short version)
        {
            Binary.Write(output, this._MapId);
            Binary.Write(output, this._Flags);
            Binary.Write(output, this._MapType);
            Binary.Write(output, this._ImageId);
            Binary.Write(output, this._Bounds);
            Binary.Write(output, this._Channel);
            Binary.Write(output, this._OpKind);
            Binary.Write(output, this._OpArgMapId);
            Binary.Write(output, this._OpArgValue);
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._MapId);

            Binary.Read(input, out this._Flags);
            this._Flags &= 0x3FF;

            Binary.Read(input, out this._MapType);
            Binary.Read(input, out this._ImageId);
            Binary.Read(input, out this._Bounds);
            Binary.Read(input, out this._Channel);
            Binary.Read(input, out this._OpKind);
            Binary.Read____(input, this._OpArgMapId);
            Binary.Read____(input, this._OpArgValue);
        }
    }
}
