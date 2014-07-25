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

namespace Gibbed.Sims4.FileFormats.Swarm
{
    public class DrawInfo : ISerializable, IVersionedSerializable
    {
        #region Properties
        public ulong ResourceId
        {
            get { return this._ResourceId; }
            set { this._ResourceId = value; }
        }

        public byte Format
        {
            get { return this._Format; }
            set { this._Format = value; }
        }

        public uint Unknown20
        {
            get { return this._Unknown20; }
            set { this._Unknown20 = value; }
        }

        public byte DrawMode
        {
            get { return this._DrawMode; }
            set { this._DrawMode = value; }
        }

        public ushort DrawFlags
        {
            get { return this._DrawFlags; }
            set { this._DrawFlags = value; }
        }

        public byte Buffer
        {
            get { return this._Buffer; }
            set { this._Buffer = value; }
        }

        public short Layer
        {
            get { return this._Layer; }
            set { this._Layer = value; }
        }

        public float SortOffset
        {
            get { return this._SortOffset; }
            set { this._SortOffset = value; }
        }

        public ulong ResourceId2
        {
            get { return this._ResourceId2; }
            set { this._ResourceId2 = value; }
        }
        #endregion

        #region Fields
        private ulong _ResourceId;
        private byte _Format;
        private uint _Unknown20;
        private byte _DrawMode;
        private ushort _DrawFlags;
        private byte _Buffer;
        private short _Layer;
        private float _SortOffset;
        private ulong _ResourceId2;
        #endregion

        public void Serialize(Stream output)
        {
            this.Serialize(output, 6);
        }

        public void Serialize(Stream output, short version)
        {
            Binary.Write(output, this._ResourceId);
            Binary.Write(output, this._Format);

            byte drawMode = this._DrawMode;
            //if (this._Unknown20 != 0)
            {
                drawMode |= 0x80;
            }
            Binary.Write(output, drawMode);

            if ((drawMode & 0x80) != 0)
            {
                Binary.Write(output, this._Unknown20);
            }

            if (version >= 6)
            {
                Binary.Write(output, this._DrawFlags);
            }
            else
            {
                Binary.Write(output, (byte)this._DrawFlags);
            }

            Binary.Write(output, this._Buffer);
            Binary.Write(output, this._Layer);
            Binary.Write(output, this._SortOffset);
            Binary.Write(output, this._ResourceId2);
        }

        public void Deserialize(Stream input)
        {
            this.Deserialize(input, 6);
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._ResourceId);
            Binary.Read(input, out this._Format);

            Binary.Read(input, out this._DrawMode);

            if ((this._DrawMode & 0x80) == 0)
            {
                this._Unknown20 = 0;
            }
            else
            {
                this._DrawMode &= 0x7F;
                Binary.Read(input, out this._Unknown20);
            }

            if (version >= 6)
            {
                Binary.Read(input, out this._DrawFlags);
            }
            else
            {
                byte dummy;
                Binary.Read(input, out dummy);
                this._DrawFlags = dummy;
            }

            Binary.Read(input, out this._Buffer);
            Binary.Read(input, out this._Layer);
            Binary.Read(input, out this._SortOffset);
            Binary.Read(input, out this._ResourceId2);
        }
    }
}
