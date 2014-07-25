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

namespace Gibbed.Sims4.FileFormats.Swarm
{
    public enum ComponentType : short
    {
        Invalid = -1,

        Particles = 1,
        MetaParticles = 2,
        Decal = 3,
        Sequence = 4,
        Sound = 5,
        Shake = 6,
        Camera = 7,
        Model = 8,
        Screen = 9,
        Game = 11,
        FastParticles = 12,
        Distribute = 13,
        Ribbon = 14,
    }
}
