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

namespace Gibbed.Sims4.FileFormats.Swarm
{
    internal static class Binary
    {
        public static void Read(Stream input, out bool value)
        {
            value = input.ReadValueB8();
        }

        public static void Read(Stream input, out byte value)
        {
            value = input.ReadValueU8();
        }

        public static void Read(Stream input, out short value)
        {
            value = input.ReadValueS16(Endian.Big);
        }

        public static void Read(Stream input, out ushort value)
        {
            value = input.ReadValueU16(Endian.Big);
        }

        public static void Read(Stream input, out int value)
        {
            value = input.ReadValueS32(Endian.Big);
        }

        public static void Read(Stream input, out uint value)
        {
            value = input.ReadValueU32(Endian.Big);
        }

        public static void Read(Stream input, out ulong value)
        {
            value = input.ReadValueU64(Endian.Big);
        }

        public static void Read(Stream input, out float value)
        {
            value = input.ReadValueF32(Endian.Big);
        }

        public static void Read(Stream input, out string value)
        {
            value = input.ReadStringZ(Encoding.ASCII);
        }

        public static void Read<T>(Stream input, out T value)
            where T : ISerializable, new()
        {
            var dummy = new T();
            dummy.Deserialize(input);
            value = dummy;
        }

        public static void Read<T>(Stream input, out T value, short version)
            where T : IVersionedSerializable, new()
        {
            var dummy = new T();
            dummy.Deserialize(input, version);
            value = dummy;
        }

        // ReSharper disable InconsistentNaming
        public static void Read____(Stream input, List<float> list)
            // ReSharper restore InconsistentNaming
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            var count = input.ReadValueU32(Endian.Big);
            var items = new List<float>();
            for (uint i = 0; i < count; i++)
            {
                items.Add(input.ReadValueF32(Endian.Big));
            }
            list.Clear();
            list.AddRange(items);
        }

        // ReSharper disable InconsistentNaming
        public static void Read____<T>(Stream input, T[] array)
            // ReSharper restore InconsistentNaming
            where T : ISerializable, new()
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            var count = array.Length;
            for (int i = 0; i < count; i++)
            {
                (array[i] = new T()).Deserialize(input);
            }
        }

        // ReSharper disable InconsistentNaming
        public static void Read____<T>(Stream input, List<T> list)
            // ReSharper restore InconsistentNaming
            where T : ISerializable, new()
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            var count = input.ReadValueU32(Endian.Big);
            var items = new List<T>();
            for (uint i = 0; i < count; i++)
            {
                var item = new T();
                item.Deserialize(input);
                items.Add(item);
            }
            list.Clear();
            list.AddRange(items);
        }

        // ReSharper disable InconsistentNaming
        public static void Read____<T>(Stream input, List<T> list, short version)
            // ReSharper restore InconsistentNaming
            where T : IVersionedSerializable, new()
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            var count = input.ReadValueU32(Endian.Big);
            var items = new List<T>();
            for (uint i = 0; i < count; i++)
            {
                var item = new T();
                item.Deserialize(input, version);
                items.Add(item);
            }
            list.Clear();
            list.AddRange(items);
        }
    }
}
