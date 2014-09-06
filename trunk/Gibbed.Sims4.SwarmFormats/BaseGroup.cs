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

namespace Gibbed.Sims4.SwarmFormats
{
    public abstract class BaseGroup<T>
        where T : IDescription, new()
    {
        #region Properties
        public short Version
        {
            get { return this._Version; }
            set { this._Version = value; }
        }

        public List<T> Items
        {
            get { return this._Items; }
        }

        public Type NativeType
        {
            get { return this._NativeType; }
        }
        #endregion

        #region Fields
        private readonly short _MinimumVersion;
        private readonly short _MaximumVersion;
        private short _Version;
        private readonly List<T> _Items;
        private readonly Type _NativeType;
        #endregion

        protected BaseGroup(short minimumVersion, short maximumVersion)
        {
            this._MinimumVersion = minimumVersion;
            this._MaximumVersion = maximumVersion;

            this._Version = -1;
            this._Items = new List<T>();
            this._NativeType = typeof(T);
        }

        public void Serialize(Stream output)
        {
            short version = this._Version;
            Binary.Write(output, version);

            int count = this._Items.Count;
            Binary.Write(output, count);

            foreach (var component in this._Items)
            {
                component.Serialize(output, version);

                if (output.Position >= 0x48585)
                {
                }
            }
        }

        public void Deserialize(Stream input)
        {
            short version;
            Binary.Read(input, out version);

            if (version < this._MinimumVersion || version > this._MaximumVersion)
            {
                throw new FormatException(
                    string.Format("unsupported version {0} for {1}", version, this._NativeType.Name));
            }

            int count;
            Binary.Read(input, out count);
            var list = new List<T>();
            for (int i = 0; i < count; i++)
            {
                var instance = new T();
                instance.Deserialize(input, version);
                list.Add(instance);
            }

            this._Version = version;
            this._Items.Clear();
            this._Items.AddRange(list);
        }
    }
}
