﻿/* Copyright (c) 2014 Rick (rick 'at' gibbed 'dot' us)
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

namespace Gibbed.Sims4.FileFormats.Swarm
{
    internal static class AuxiliaryTable
    {
        private static readonly Func<IAuxiliary>[] _Factories;

        static AuxiliaryTable()
        {
            _Factories = new Func<IAuxiliary>[]
            {
                () => new Auxiliaries.MapsAuxiliary(),
                () => new Auxiliaries.MaterialAuxiliary(),
            };
        }

        public static Func<IAuxiliary> GetFactory(int index)
        {
            if (index < 0 || index >= _Factories.Length)
            {
                return null;
            }

            return _Factories[index];
        }
    }
}