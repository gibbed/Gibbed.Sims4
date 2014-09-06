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

namespace Gibbed.Sims4.SwarmFormats
{
    public class Filter : ISerializable
    {
        #region Properties
        public byte Type
        {
            get { return this._Type; }
            set { this._Type = value; }
        }

        public byte Destination
        {
            get { return this._Destination; }
            set { this._Destination = value; }
        }

        public ulong Source
        {
            get { return this._Source; }
            set { this._Source = value; }
        }

        public List<byte> Parameters
        {
            get { return this._Parameters; }
        }
        #endregion

        #region Fields
        private byte _Type;
        private byte _Destination;
        private ulong _Source;
        private readonly List<byte> _Parameters;
        #endregion

        public Filter()
        {
            this._Parameters = new List<byte>();
        }

        public void Serialize(Stream output)
        {
            Binary.Write(output, this._Type);
            Binary.Write(output, this._Destination);
            Binary.Write(output, this._Source);
            Binary.Write(output, this._Parameters);
        }

        public void Deserialize(Stream input)
        {
            Binary.Read(input, out this._Type);
            Binary.Read(input, out this._Destination);
            Binary.Read(input, out this._Source);
            Binary.Read____(input, this._Parameters);
        }
    }
}
