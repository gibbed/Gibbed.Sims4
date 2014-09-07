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
using Gibbed.IO;
using System.Collections.Generic;

namespace Gibbed.Sims4.FileFormats
{
    public class ObjectDefinitionFile
    {
        /* What does "with fallback" mean?
         *   If the game tries to load the array property, and it is missing, it will
         *   look for a string8 property of the same name. If the string8 property is
         *   present, it will hash the string and put it in the array with a predefined
         *   type and group of 0.
         * 
         * In TS4:
         *  0xE7F07786 = fnv32(name) ^ fnv32(string8)
         *  0xECD5A95F = fnv32(materialvariant) ^ fnv32(string8)
         *  0x4233F8A0 = fnv32(thumbnailgeometrystate) ^ fnv32(uint32)
         *  0xAC8E1BC0 = 0xFE510AB7 ^ fnv32(uint8)
         *  0x8D20ACC6 = fnv32(model) ^ 0xA4744BF2, resource key array, with fallback (0x01661233)
         *  0x6C737AD8 = fnv32(footprint) ^ 0xA4744BF2, resource key array, with fallback (0xD382BF57)
         *  0xE206AE4F = fnv32(rig) ^ 0xA4744BF2, resource key array, with fallback (0x8EAF13DE)
         *  0x8A85AFF3 = fnv32(slot) ^ 0xA4744BF2, resource key array, with fallback (0xD3044521)
         *  0xE6E421FB = fnv32(components) ^ 0xA4744BF2, uint32 array
         *  0xCADED888 = fnv32(icon)  ^ 0xA4744BF2, resource key array, with fallback (0x2F7D0004)
         *    Important note:
         *      Native code (TS4.exe) doesn't support icon as a resource key array,
         *      only string. The game python scripts do though. Not sure why there is
         *      a difference here, probably a mistake/bug.
         *  0xE4F4FAA4 = fnv32(simoleonprice) ^ fnv32(uint32), default is 100
         *  0xEC3712E6 = 0x84C94DBF ^ fnv32(bool)
         *  0x1F1C4F7B = 0x4DC35E0C ^ fnv32(uint8)
         *  0x2172AEBE = fnv32(environmentscoreemotiontags) ^ 0xA6744498, uint16 array
         *  0xDCD08394 = fnv32(environmentscores) ^ 0x1B801A5D, float array
         *  0x44FC7512 = fnv32(negativeenvironmentscore) ^ fnv32(float)
         *  0x7236BEEA = fnv32(positiveenvironmentscore) ^ fnv32(float)
         *  0x52F7F4BC = 0xBCDF75F3 ^ fnv32(uint64)
         *  0xAEE67A1C = fnv32(isbaby) ^ fnv32(bool)
         * *0xF3936A90 = 0xF410B613 ^ 0x0783DC83, uint8 array
         *  0xB994039B = fnv32(tuningid) ^ fnv32(int64)
         */

        /* Unknown:
         *  0x790FA4BC = fnv32(tuning) ^ fnv32(string8)
         */

        #region Properties
        #endregion

        #region Fields
        #endregion

        public ObjectDefinitionFile()
        {
        }

        public void Serialize(Stream output)
        {
            const Endian endian = Endian.Little;

            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            const Endian endian = Endian.Little;

            var startPosition = input.Position;

            var version = input.ReadValueU16(endian);
            if (version < 1)
            {
                throw new FormatException();
            }

            var propertyTableOffset = input.ReadValueU32(endian);
            input.Position = startPosition + propertyTableOffset;

            var propertyOffsets = new Dictionary<uint, uint>();
            var propertyCount = input.ReadValueU16(endian);
            for (int i = 0; i < propertyCount; i++)
            {
                var propertyId = input.ReadValueU32(endian);
                var propertyOffset = input.ReadValueU32(endian);
                propertyOffsets.Add(propertyId, propertyOffset);
            }

            // TODO: deserialize properties, perhaps make propertystream a generic that supports serializing C# classes?
            throw new NotImplementedException();
        }
    }
}
