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

namespace Gibbed.Sims4.FileFormats
{
    public class StringTableFile
    {
        public const uint Signature = 0x4C425453;

        #region Properties
        public Dictionary<uint, string> Entries
        {
            get { return this._Entries; }
        }
        #endregion

        #region Fields
        private readonly Dictionary<uint, string> _Entries;
        #endregion

        public StringTableFile()
        {
            this._Entries = new Dictionary<uint, string>();
        }

        public void Serialize(Stream output)
        {
            const Endian endian = Endian.Little;

            uint size;
            byte[] bytes;

            using (var temp = new MemoryStream())
            {
                size = 0;
                foreach (var kv in this.Entries)
                {
                    var entryBytes = Encoding.UTF8.GetBytes(kv.Value);
                    if (entryBytes.Length > 0xFFFF)
                    {
                        throw new InvalidOperationException();
                    }

                    var length = (ushort)entryBytes.Length;
                    temp.WriteValueU32(kv.Key);
                    temp.WriteValueU8(0);
                    temp.WriteValueU16(length, endian);
                    temp.Write(entryBytes, 0, length);
                    size += length + 1u;
                }

                bytes = temp.ToArray();
            }

            output.WriteValueU32(Signature, endian);
            output.WriteValueU16(5, endian);
            output.Seek(1, SeekOrigin.Current);
            output.WriteValueS64(this.Entries.Count, endian);
            output.Seek(2, SeekOrigin.Current);
            output.WriteValueU32(size, endian);
            output.Write(bytes, 0, bytes.Length);
        }

        public void Deserialize(Stream input)
        {
            const Endian endian = Endian.Little;

            var magic = input.ReadValueU32(endian);
            if (magic != Signature)
            {
                throw new FormatException();
            }

            var version = input.ReadValueU16(endian);
            if (version != 5)
            {
                throw new FormatException();
            }

            this.Entries.Clear();

            // input.ReadValueU8(); // unused
            input.Seek(1, SeekOrigin.Current);

            var count = input.ReadValueS64(endian);

            // input.ReadValueU8(); // unused
            // input.ReadValueU8(); // unused
            input.Seek(2, SeekOrigin.Current);

            var runtimeMemorySize = input.ReadValueU32(endian);
            var actualSize = runtimeMemorySize + (count * (4 + 1 + 2 - 1));
            using (var temp = input.ReadToMemoryStream(actualSize))
            {
                for (long i = 0; i < count; i++)
                {
                    var id = temp.ReadValueU32(endian);

                    var unused = temp.ReadValueU8();
                    if (unused != 0)
                    {
                        throw new FormatException();
                    }

                    var length = temp.ReadValueU16(endian);
                    var text = temp.ReadString(length, true, Encoding.UTF8);

                    this.Entries.Add(id, text);
                }
            }
        }
    }
}
