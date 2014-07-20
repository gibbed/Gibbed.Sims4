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
        public int OffsetB;
        public int OffsetC;
        public int OffsetD;
        public int OffsetE;

        public void Serialize(Stream output, Endian endian)
        {
            output.WriteValueS32(this.CommandOffset, endian);
            output.WriteValueS32(this.OffsetB, endian);
            output.WriteValueS32(this.OffsetC, endian);
            output.WriteValueS32(this.OffsetD, endian);
            output.WriteValueS32(this.OffsetE, endian);
        }

        public void Deserialize(Stream input, Endian endian)
        {
            this.CommandOffset = input.ReadValueS32(endian);
            this.OffsetB = input.ReadValueS32(endian);
            this.OffsetC = input.ReadValueS32(endian);
            this.OffsetD = input.ReadValueS32(endian);
            this.OffsetE = input.ReadValueS32(endian);
        }

        public static MipHeader Read(Stream input, Endian endian)
        {
            var header = new MipHeader();
            header.Deserialize(input, endian);
            return header;
        }

        public static void Write(Stream output, MipHeader header, Endian endian)
        {
            header.Serialize(output, endian);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}",
                                 this.CommandOffset,
                                 this.OffsetB,
                                 this.OffsetC,
                                 this.OffsetD,
                                 this.OffsetE);
        }
    }
}
