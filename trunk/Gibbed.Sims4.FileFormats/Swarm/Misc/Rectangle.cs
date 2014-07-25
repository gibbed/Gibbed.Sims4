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
    public class Rectangle : ISerializable
    {
        #region Properties
        public float Left
        {
            get { return this._Left; }
            set { this._Left = value; }
        }

        public float Top
        {
            get { return this._Top; }
            set { this._Top = value; }
        }

        public float Right
        {
            get { return this._Right; }
            set { this._Right = value; }
        }

        public float Bottom
        {
            get { return this._Bottom; }
            set { this._Bottom = value; }
        }
        #endregion

        #region Fields
        private float _Left;
        private float _Top;
        private float _Right;
        private float _Bottom;
        #endregion

        public void Serialize(Stream output)
        {
            Binary.Write(output, this._Left);
            Binary.Write(output, this._Top);
            Binary.Write(output, this._Right);
            Binary.Write(output, this._Bottom);
        }

        public void Deserialize(Stream input)
        {
            Binary.Read(input, out this._Left);
            Binary.Read(input, out this._Top);
            Binary.Read(input, out this._Right);
            Binary.Read(input, out this._Bottom);
        }
    }
}
