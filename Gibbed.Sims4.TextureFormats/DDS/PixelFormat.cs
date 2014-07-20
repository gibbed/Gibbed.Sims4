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

namespace Gibbed.Sims4.TextureFormats.DDS
{
    public class PixelFormat
    {
        public uint Size;
        public PixelFormatFlags Flags;
        public uint FourCC;
        public uint RGBBitCount;
        public uint RedBitMask;
        public uint GreenBitMask;
        public uint BlueBitMask;
        public uint AlphaBitMask;

        public static uint StructureSize
        {
            get { return 8 * 4; }
        }

        public void Serialize(Stream output, Endian endian)
        {
            output.WriteValueU32(this.Size, endian);
            output.WriteValueEnum<PixelFormatFlags>(this.Flags, endian);
            output.WriteValueU32(this.FourCC, endian);
            output.WriteValueU32(this.RGBBitCount, endian);
            output.WriteValueU32(this.RedBitMask, endian);
            output.WriteValueU32(this.GreenBitMask, endian);
            output.WriteValueU32(this.BlueBitMask, endian);
            output.WriteValueU32(this.AlphaBitMask, endian);
        }

        public void Deserialize(Stream input, Endian endian)
        {
            this.Size = input.ReadValueU32(endian);
            this.Flags = input.ReadValueEnum<PixelFormatFlags>(endian);
            this.FourCC = input.ReadValueU32(endian);
            this.RGBBitCount = input.ReadValueU32(endian);
            this.RedBitMask = input.ReadValueU32(endian);
            this.GreenBitMask = input.ReadValueU32(endian);
            this.BlueBitMask = input.ReadValueU32(endian);
            this.AlphaBitMask = input.ReadValueU32(endian);
        }
    }
}
