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
using System.Linq;
using System.Text;
using Gibbed.IO;

namespace Gibbed.Sims4.SwarmFormats
{
    public class SwarmBinaryFile
    {
        #region Properties
        public short Version
        {
            get { return this._Version; }
            set { this._Version = value; }
        }

        public VisualEffectGroup VisualEffectGroup
        {
            get { return this._VisualEffectGroup; }
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
        private readonly Dictionary<ComponentType, IComponentGroup> _ComponentTypeToGroup;
        private readonly Dictionary<Type, IComponentGroup> _ComponentNativeToGroup;
        private readonly Dictionary<AuxiliaryType, IAuxiliaryGroup> _AuxiliaryTypeToGroup;
        private readonly Dictionary<Type, IAuxiliaryGroup> _AuxiliaryNativeToGroup;
        private readonly VisualEffectGroup _VisualEffectGroup;
        private readonly Dictionary<int, ulong> _EffectIds;
        private readonly Dictionary<int, string> _EffectNames;
        #endregion

        public SwarmBinaryFile()
        {
            this._ComponentTypeToGroup = new Dictionary<ComponentType, IComponentGroup>();
            this._ComponentNativeToGroup = new Dictionary<Type, IComponentGroup>();
            this._AuxiliaryTypeToGroup = new Dictionary<AuxiliaryType, IAuxiliaryGroup>();
            this._AuxiliaryNativeToGroup = new Dictionary<Type, IAuxiliaryGroup>();
            this.SetupGroups();
            this._VisualEffectGroup = new VisualEffectGroup(1, 3);
            this._EffectIds = new Dictionary<int, ulong>();
            this._EffectNames = new Dictionary<int, string>();
        }

        private void Setup<T>(ComponentType type, short minimumVersion, short maximumVersion)
            where T: IComponent, new()
        {
            var group = new ComponentGroup<T>(type, minimumVersion, maximumVersion);
            this._ComponentTypeToGroup.Add(type, group);
            this._ComponentNativeToGroup.Add(typeof(T), group);
        }

        private void Setup<T>(AuxiliaryType type, short minimumVersion, short maximumVersion)
            where T : IAuxiliary, new()
        {
            var group = new AuxiliaryGroup<T>(type, minimumVersion, maximumVersion);
            this._AuxiliaryTypeToGroup.Add(type, group);
            this._AuxiliaryNativeToGroup.Add(typeof(T), group);
        }

        private void SetupGroups()
        {
            this._ComponentTypeToGroup.Clear();
            this.Setup<Components.ParticlesComponent>(ComponentType.Particles, 1, 7);
            this.Setup<Components.MetaParticlesComponent>(ComponentType.MetaParticles, 1, 1);
            this.Setup<Components.DecalComponent>(ComponentType.Decal, 1, 2);
            this.Setup<Components.SequenceComponent>(ComponentType.Sequence, 1, 1);
            this.Setup<Components.SoundComponent>(ComponentType.Sound, 1, 2);
            this.Setup<Components.ShakeComponent>(ComponentType.Shake, 1, 1);
            this.Setup<Components.CameraComponent>(ComponentType.Camera, 1, 1);
            this.Setup<Components.ModelComponent>(ComponentType.Model, 1, 1);
            this.Setup<Components.ScreenComponent>(ComponentType.Screen, 1, 1);
            this.Setup<Components.GameComponent>(ComponentType.Game, 1, 1);
            this.Setup<Components.FastParticlesComponent>(ComponentType.FastParticles, 1, 1);
            this.Setup<Components.DistributeComponent>(ComponentType.Distribute, 1, 1);
            this.Setup<Components.RibbonComponent>(ComponentType.Ribbon, 1, 2);

            this._AuxiliaryTypeToGroup.Clear();
            this.Setup<Auxiliaries.MapsAuxiliary>(AuxiliaryType.Maps, 0, 0);
            this.Setup<Auxiliaries.MaterialAuxiliary>(AuxiliaryType.Material, 0, 0);
        }

        public IComponentGroup<T> GetComponentGroup<T>()
            where T: IComponent
        {
            var type = typeof(T);
            if (this._ComponentNativeToGroup.ContainsKey(type) == false)
            {
                return null;
            }
            return this._ComponentNativeToGroup[type] as IComponentGroup<T>;
        }

        public IAuxiliaryGroup<T> GetAuxiliaryGroup<T>()
            where T : IAuxiliary
        {
            var type = typeof(T);
            if (this._AuxiliaryNativeToGroup.ContainsKey(type) == false)
            {
                return null;
            }
            return this._AuxiliaryNativeToGroup[type] as IAuxiliaryGroup<T>;
        }

        public void Serialize(Stream output)
        {
            var version = this._Version;
            if (version != 2)
            {
                throw new FormatException("not version 2");
            }
            output.WriteValueS16(version, Endian.Big);

            foreach (var kv in this._ComponentTypeToGroup.OrderBy(kv => (short)kv.Key))
            {
                output.WriteValueS16((short)kv.Key, Endian.Big);
                kv.Value.Serialize(output);
            }
            output.WriteValueS16(-1, Endian.Big);

            foreach (var kv in this._AuxiliaryTypeToGroup.OrderBy(kv => (short)kv.Key))
            {
                output.WriteValueS16((short)kv.Key, Endian.Big);
                kv.Value.Serialize(output);
            }
            output.WriteValueS16(-1, Endian.Big);

            this.VisualEffectGroup.Serialize(output);

            foreach (var kv in this._EffectIds)
            {
                output.WriteValueS32(kv.Key, Endian.Big);
                output.WriteValueU64(kv.Value, Endian.Big);
            }
            output.WriteValueS32(-1, Endian.Big);

            foreach (var kv in this._EffectNames.OrderBy(kv => kv.Value))
            {
                output.WriteValueS32(kv.Key, Endian.Big);
                output.WriteStringZ(kv.Value, Encoding.ASCII);
            }
            output.WriteValueS32(-1, Endian.Big);
        }

        public void Deserialize(Stream input)
        {
            var version = input.ReadValueS16(Endian.Big);
            if (version != 2)
            {
                throw new FormatException("not version 2");
            }

            this._Version = version;

            this._EffectIds.Clear();
            this._EffectNames.Clear();

            var componentType = (ComponentType)input.ReadValueS16(Endian.Big);
            while (componentType != ComponentType.Invalid)
            {
                var componentGroup = this._ComponentTypeToGroup[componentType];
                componentGroup.Deserialize(input);
                componentType = (ComponentType)input.ReadValueS16(Endian.Big);
            }

            var auxiliaryType = (AuxiliaryType)input.ReadValueS16(Endian.Big);
            while (auxiliaryType != AuxiliaryType.Invalid)
            {
                var auxiliaryGroup = this._AuxiliaryTypeToGroup[auxiliaryType];
                auxiliaryGroup.Deserialize(input);
                auxiliaryType = (AuxiliaryType)input.ReadValueS16(Endian.Big);
            }

            this.VisualEffectGroup.Deserialize(input);

            int effectIndex;

            effectIndex = input.ReadValueS32(Endian.Big);
            while (effectIndex >= 0)
            {
                this._EffectIds.Add(effectIndex, input.ReadValueU64(Endian.Big));
                effectIndex = input.ReadValueS32(Endian.Big);
            }

            effectIndex = input.ReadValueS32(Endian.Big);
            while (effectIndex >= 0)
            {
                this._EffectNames.Add(effectIndex, input.ReadStringZ(Encoding.ASCII));
                effectIndex = input.ReadValueS32(Endian.Big);
            }
        }
    }
}
