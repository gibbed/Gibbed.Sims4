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
using Gibbed.IO;

namespace Gibbed.Sims4.TextureConvert
{
    internal struct MipHeader
    {
        public int CommandOffset;
        public int Offset2;
        public int Offset3;
        public int Offset0;
        public int Offset1;
        public int Offset4;

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                                 this.CommandOffset,
                                 this.Offset2,
                                 this.Offset3,
                                 this.Offset0,
                                 this.Offset1,
                                 this.Offset4);
        }
    }
}
