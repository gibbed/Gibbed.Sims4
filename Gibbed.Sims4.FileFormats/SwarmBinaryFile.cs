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
using System.Text;
using Gibbed.IO;
using Gibbed.Sims4.FileFormats.Swarm;

namespace Gibbed.Sims4.FileFormats
{
    public class SwarmBinaryFile
    {
        #region Properties
        public short Version
        {
            get { return this._Version; }
            set { this._Version = value; }
        }

        public List<TypeVersionGroup<IComponent>> Components
        {
            get { return this._Components; }
        }

        public List<TypeVersionGroup<IAuxiliary>> Auxiliaries
        {
            get { return this._Auxiliaries; }
        }

        public VersionGroup<VisualEffect> VisualEffects
        {
            get { return this._VisualEffects; }
            set { this._VisualEffects = value; }
        }

        public Dictionary<int, ulong> EffectIds
        {
            get { return this._EffectIds; }
        }

        public Dictionary<int, string> EffectNames
        {
            get { return this._EffectNames; }
        }
        #endregion

        #region Fields
        private short _Version;
        private readonly List<TypeVersionGroup<IComponent>> _Components;
        private readonly List<TypeVersionGroup<IAuxiliary>> _Auxiliaries;
        private VersionGroup<VisualEffect> _VisualEffects;
        private readonly Dictionary<int, ulong> _EffectIds;
        private readonly Dictionary<int, string> _EffectNames;
        #endregion

        public SwarmBinaryFile()
        {
            this._Components = new List<TypeVersionGroup<IComponent>>();
            this._Auxiliaries = new List<TypeVersionGroup<IAuxiliary>>();
            this._VisualEffects = new VersionGroup<VisualEffect>();
            this._EffectIds = new Dictionary<int, ulong>();
            this._EffectNames = new Dictionary<int, string>();
        }

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            var version = input.ReadValueS16(Endian.Big);
            if (version != 2)
            {
                throw new FormatException("not version 2");
            }

            var components = ReadGroupList(input, i => ComponentTable.GetFactory(i));
            var auxiliaries = ReadGroupList(input, i => AuxiliaryTable.GetFactory(i));
            var visualEffects = ReadGroup(input, () => new VisualEffect());

            int effectIndex;

            var effectIds = new Dictionary<int, ulong>();
            effectIndex = input.ReadValueS32(Endian.Big);
            while (effectIndex >= 0)
            {
                effectIds.Add(effectIndex, input.ReadValueU64(Endian.Big));
                effectIndex = input.ReadValueS32(Endian.Big);
            }

            var effectNames = new Dictionary<int, string>();
            effectIndex = input.ReadValueS32(Endian.Big);
            while (effectIndex >= 0)
            {
                effectNames.Add(effectIndex, input.ReadStringZ(Encoding.ASCII));
                effectIndex = input.ReadValueS32(Endian.Big);
            }

            this._Version = version;
            this._Components.Clear();
            this._Components.AddRange(components);
            this._Auxiliaries.Clear();
            this._Auxiliaries.AddRange(auxiliaries);
            this._VisualEffects = visualEffects;

            this._EffectIds.Clear();
            foreach (var kv in effectIds)
            {
                this._EffectIds.Add(kv.Key, kv.Value);
            }

            this._EffectNames.Clear();
            foreach (var kv in effectNames)
            {
                this._EffectNames.Add(kv.Key, kv.Value);
            }
        }

        private static VersionGroup<TType> ReadGroup<TType>(
            Stream input,
            Func<TType> create)
            where TType : class, IFormat
        {
            return ReadGroup<TType, VersionGroup<TType>>(input, create);
        }

        private static TVersionList ReadGroup<TType, TVersionList>(
            Stream input,
            Func<TType> create)
            where TType : class, IFormat
            where TVersionList : VersionGroup<TType>, new()
        {
            short version = input.ReadValueS16(Endian.Big);

            var list = new TVersionList()
            {
                Version = version,
            };

            int count = input.ReadValueS32(Endian.Big);
            for (int i = 0; i < count; i++)
            {
                var instance = create();
                if (instance == null)
                {
                    throw new InvalidOperationException();
                }

                if (version < instance.MinimumVersion || version > instance.MaximumVersion)
                {
                    throw new FormatException(string.Format("unsupported version {0} for {1}",
                                                            version,
                                                            instance.GetType().Name));
                }
                instance.Deserialize(input, version);
                list.Items.Add(instance);
            }
            return list;
        }

        private static List<TypeVersionGroup<TType>> ReadGroupList<TType>(
            Stream input,
            Func<int, Func<TType>> factoryFactory)
            where TType : class, IFormat
        {
            var list = new List<TypeVersionGroup<TType>>();

            var type = input.ReadValueS16(Endian.Big);
            while (type != -1)
            {
                var factory = factoryFactory(type);
                if (factory == null)
                {
                    throw new InvalidOperationException();
                }

                var typeList = ReadGroup<TType, TypeVersionGroup<TType>>(input, factory);
                typeList.Type = type;
                list.Add(typeList);

                type = input.ReadValueS16(Endian.Big);
            }
            return list;
        }
    }
}
