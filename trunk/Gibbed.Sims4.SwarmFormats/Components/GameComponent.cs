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

namespace Gibbed.Sims4.SwarmFormats.Components
{
    public class GameComponent : IComponent
    {
        #region Properties
        public uint Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public uint MessageId
        {
            get { return this._MessageId; }
            set { this._MessageId = value; }
        }

        public uint MessageData1
        {
            get { return this._MessageData1; }
            set { this._MessageData1 = value; }
        }

        public uint MessageData2
        {
            get { return this._MessageData2; }
            set { this._MessageData2 = value; }
        }

        public uint MessageData3
        {
            get { return this._MessageData3; }
            set { this._MessageData3 = value; }
        }

        public uint MessageData4
        {
            get { return this._MessageData4; }
            set { this._MessageData4 = value; }
        }

        public string MessageString
        {
            get { return this._MessageString; }
            set { this._MessageString = value; }
        }

        public float Life
        {
            get { return this._Life; }
            set { this._Life = value; }
        }
        #endregion

        #region Fields
        private uint _Flags;
        private uint _MessageId;
        private uint _MessageData1;
        private uint _MessageData2;
        private uint _MessageData3;
        private uint _MessageData4;
        private string _MessageString;
        private float _Life;
        #endregion

        public void Serialize(Stream output, short version)
        {
            Binary.Write(output, this._Flags);
            Binary.Write(output, this._MessageId);
            Binary.Write(output, this._MessageData1);
            Binary.Write(output, this._MessageData2);
            Binary.Write(output, this._MessageData3);
            Binary.Write(output, this._MessageData4);
            Binary.Write(output, this._MessageString);
            Binary.Write(output, this._Life);
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Flags);
            this._Flags &= 0x3FF;

            Binary.Read(input, out this._MessageId);
            Binary.Read(input, out this._MessageData1);
            Binary.Read(input, out this._MessageData2);
            Binary.Read(input, out this._MessageData3);
            Binary.Read(input, out this._MessageData4);
            Binary.Read(input, out this._MessageString);
            Binary.Read(input, out this._Life);
        }
    }
}
