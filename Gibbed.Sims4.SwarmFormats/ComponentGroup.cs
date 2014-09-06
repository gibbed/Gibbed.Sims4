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
using System.Linq;

namespace Gibbed.Sims4.SwarmFormats
{
    internal sealed class ComponentGroup<T> : BaseGroup<T>, IComponentGroup, IComponentGroup<T>
        where T : IComponent, new()
    {
        #region Properties
        public ComponentType Type
        {
            get { return this._Type; }
        }
        #endregion

        #region Fields
        private readonly ComponentType _Type;
        #endregion

        public ComponentGroup(ComponentType type, short minimumVersion, short maximumVersion)
            : base(minimumVersion, maximumVersion)
        {
            this._Type = type;
        }

        IEnumerable<IComponent> IComponentGroup.Items
        {
            get { return this.Items.Cast<IComponent>(); }
        }

        IList<T> IComponentGroup<T>.Items
        {
            get { return this.Items; }
        }
    }
}
