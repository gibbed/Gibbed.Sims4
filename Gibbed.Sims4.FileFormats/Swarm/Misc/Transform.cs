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
    public class Transform : ISerializable
    {
        #region Properties
        public ushort Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public float Scale
        {
            get { return this._Scale; }
            set { this._Scale = value; }
        }

        public Matrix33 Rotation
        {
            get { return this._Rotation; }
            set { this._Rotation = value; }
        }

        public Vector3 Translation
        {
            get { return this._Translation; }
            set { this._Translation = value; }
        }
        #endregion

        #region Fields
        private ushort _Flags;
        private float _Scale;
        private Matrix33 _Rotation;
        private Vector3 _Translation;
        #endregion

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            Binary.Read(input, out this._Flags);
            Binary.Read(input, out this._Scale);
            Binary.Read(input, out this._Rotation);
            Binary.Read(input, out this._Translation);
        }
    }
}
