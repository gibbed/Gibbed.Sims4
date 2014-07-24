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
using System.Collections.Generic;
using System.IO;

namespace Gibbed.Sims4.FileFormats.Swarm.Components
{
    public class SequenceComponent : IComponent
    {
        #region Properties
        public short MinimumVersion
        {
            get { return 1; }
        }

        public short MaximumVersion
        {
            get { return 1; }
        }

        public List<Item> Items
        {
            get { return this._Items; }
        }

        public uint Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }
        #endregion

        #region Fields
        private readonly List<Item> _Items;
        private uint _Flags;
        #endregion

        public SequenceComponent()
        {
            this._Items = new List<Item>();
        }

        public void Serialize(Stream output, short version)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read____(input, this._Items);

            Binary.Read(input, out this._Flags);
            this._Flags &= 0xF;
        }

        public class Item : ISerializable
        {
            #region Properties
            public Vector2 TimeRange
            {
                get { return this._TimeRange; }
                set { this._TimeRange = value; }
            }

            public string EffectName
            {
                get { return this._EffectName; }
                set { this._EffectName = value; }
            }
            #endregion

            #region Fields
            private Vector2 _TimeRange;
            private string _EffectName;
            #endregion

            public void Serialize(Stream output)
            {
                throw new NotImplementedException();
            }

            public void Deserialize(Stream input)
            {
                Binary.Read(input, out this._TimeRange);
                Binary.Read(input, out this._EffectName);
            }
        }
    }
}
