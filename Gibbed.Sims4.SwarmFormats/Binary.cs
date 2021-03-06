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
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.IO;

namespace Gibbed.Sims4.SwarmFormats
{
    internal static class Binary
    {
        public static void Read(Stream input, out bool value)
        {
            value = input.ReadValueB8();
        }

        public static void Write(Stream output, bool value)
        {
            output.WriteValueB8(value);
        }

        public static void Read(Stream input, out byte value)
        {
            value = input.ReadValueU8();
        }

        public static void Write(Stream output, byte value)
        {
            output.WriteValueU8(value);
        }

        public static void Read(Stream input, out short value)
        {
            value = input.ReadValueS16(Endian.Big);
        }

        public static void Write(Stream output, short value)
        {
            output.WriteValueS16(value, Endian.Big);
        }

        public static void Read(Stream input, out ushort value)
        {
            value = input.ReadValueU16(Endian.Big);
        }

        public static void Write(Stream output, ushort value)
        {
            output.WriteValueU16(value, Endian.Big);
        }

        public static void Read(Stream input, out int value)
        {
            value = input.ReadValueS32(Endian.Big);
        }

        public static void Write(Stream output, int value)
        {
            output.WriteValueS32(value, Endian.Big);
        }

        public static void Read(Stream input, out uint value)
        {
            value = input.ReadValueU32(Endian.Big);
        }

        public static void Write(Stream output, uint value)
        {
            output.WriteValueU32(value, Endian.Big);
        }

        public static void Read(Stream input, out ulong value)
        {
            value = input.ReadValueU64(Endian.Big);
        }

        public static void Write(Stream output, ulong value)
        {
            output.WriteValueU64(value, Endian.Big);
        }

        public static void Read(Stream input, out float value)
        {
            value = input.ReadValueF32(Endian.Big);
        }

        public static void Write(Stream output, float value)
        {
            output.WriteValueF32(value, Endian.Big);
        }

        public static void Read(Stream input, out string value)
        {
            value = input.ReadStringZ(Encoding.ASCII);
        }

        public static void Write(Stream output, string value)
        {
            output.WriteStringZ(value, Encoding.ASCII);
        }

        public static void Read<T>(Stream input, out T value)
            where T : ISerializable, new()
        {
            var dummy = new T();
            dummy.Deserialize(input);
            value = dummy;
        }

        public static void Write<T>(Stream output, T value)
            where T : ISerializable
        {
            value.Serialize(output);
        }

        public static void Read<T>(Stream input, out T value, short version)
            where T : IVersionedSerializable, new()
        {
            var dummy = new T();
            dummy.Deserialize(input, version);
            value = dummy;
        }

        public static void Write<T>(Stream output, T value, short version)
            where T : IVersionedSerializable
        {
            value.Serialize(output, version);
        }

        // ReSharper disable InconsistentNaming
        public static void Read____(Stream input, List<byte> list)
            // ReSharper restore InconsistentNaming
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            var count = input.ReadValueU32(Endian.Big);
            var bytes = input.ReadBytes(count);
            list.Clear();
            list.AddRange(bytes);
        }

        public static void Write(Stream output, List<byte> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            var bytes = list.ToArray();
            var count = bytes.Length;
            output.WriteValueS32(count, Endian.Big);
            output.Write(bytes, 0, count);
        }

        // ReSharper disable InconsistentNaming
        private static void Read_s__<T>(Stream input, List<T> list, Func<T> readItem)
            // ReSharper restore InconsistentNaming
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            var count = input.ReadValueU16(Endian.Big);
            var items = new List<T>();
            for (uint i = 0; i < count; i++)
            {
                var value = readItem();
                items.Add(value);
            }
            list.Clear();
            list.AddRange(items);
        }

        // ReSharper disable InconsistentNaming
        private static void Write_s<T>(Stream output, List<T> list, Action<T> writeItem)
            // ReSharper restore InconsistentNaming
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            var count = list.Count;
            if (count > 0xFFFF)
            {
                throw new ArgumentOutOfRangeException();
            }

            output.WriteValueU16((ushort)count, Endian.Big);
            foreach (var item in list)
            {
                writeItem(item);
            }
        }

        // ReSharper disable InconsistentNaming
        private static void Read____<T>(Stream input, List<T> list, Func<T> readItem)
            // ReSharper restore InconsistentNaming
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            var count = input.ReadValueU32(Endian.Big);
            var items = new List<T>();
            for (uint i = 0; i < count; i++)
            {
                var value = readItem();
                items.Add(value);
            }
            list.Clear();
            list.AddRange(items);
        }

        private static void Write<T>(Stream output, List<T> list, Action<T> writeItem)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            var count = list.Count;
            output.WriteValueS32(count, Endian.Big);
            foreach (var item in list)
            {
                writeItem(item);
            }
        }

        // ReSharper disable InconsistentNaming
        public static void Read_s__(Stream input, List<bool> list)
            // ReSharper restore InconsistentNaming
        {
            // ReSharper disable ConvertClosureToMethodGroup
            Read_s__(input, list, () => input.ReadValueB8());
            // ReSharper restore ConvertClosureToMethodGroup
        }

        // ReSharper disable InconsistentNaming
        public static void Write_s(Stream output, List<bool> list)
            // ReSharper restore InconsistentNaming
        {
            // ReSharper disable ConvertClosureToMethodGroup
            Write_s(output, list, v => output.WriteValueB8(v));
            // ReSharper restore ConvertClosureToMethodGroup
        }

        // ReSharper disable InconsistentNaming
        public static void Read____(Stream input, List<bool> list)
            // ReSharper restore InconsistentNaming
        {
            // ReSharper disable ConvertClosureToMethodGroup
            Read____(input, list, () => input.ReadValueB8());
            // ReSharper restore ConvertClosureToMethodGroup
        }

        public static void Write(Stream output, List<bool> list)
        {
            // ReSharper disable ConvertClosureToMethodGroup
            Write(output, list, v => output.WriteValueB8(v));
            // ReSharper restore ConvertClosureToMethodGroup
        }

        // ReSharper disable InconsistentNaming
        public static void Read_s__(Stream input, List<int> list)
            // ReSharper restore InconsistentNaming
        {
            Read_s__(input, list, () => input.ReadValueS32(Endian.Big));
        }

        // ReSharper disable InconsistentNaming
        public static void Write_s(Stream output, List<int> list)
            // ReSharper restore InconsistentNaming
        {
            Write_s(output, list, v => output.WriteValueS32(v, Endian.Big));
        }

        // ReSharper disable InconsistentNaming
        public static void Read____(Stream input, List<int> list)
            // ReSharper restore InconsistentNaming
        {
            Read____(input, list, () => input.ReadValueS32(Endian.Big));
        }

        public static void Write(Stream output, List<int> list)
        {
            Write(output, list, v => output.WriteValueS32(v, Endian.Big));
        }

        // ReSharper disable InconsistentNaming
        public static void Read_s__(Stream input, List<float> list)
            // ReSharper restore InconsistentNaming
        {
            Read_s__(input, list, () => input.ReadValueF32(Endian.Big));
        }

        // ReSharper disable InconsistentNaming
        public static void Write_s(Stream output, List<float> list)
            // ReSharper restore InconsistentNaming
        {
            Write_s(output, list, v => output.WriteValueF32(v, Endian.Big));
        }

        // ReSharper disable InconsistentNaming
        public static void Read____(Stream input, List<float> list)
            // ReSharper restore InconsistentNaming
        {
            Read____(input, list, () => input.ReadValueF32(Endian.Big));
        }

        public static void Write(Stream output, List<float> list)
        {
            Write(output, list, v => output.WriteValueF32(v, Endian.Big));
        }

        // ReSharper disable InconsistentNaming
        public static void Read____(Stream input, List<ulong> list)
            // ReSharper restore InconsistentNaming
        {
            Read____(input, list, () => input.ReadValueU64(Endian.Big));
        }

        public static void Write(Stream output, List<ulong> list)
        {
            Write(output, list, v => output.WriteValueU64(v, Endian.Big));
        }

        // ReSharper disable InconsistentNaming
        public static void Read____<T>(Stream input, List<T> list)
            // ReSharper restore InconsistentNaming
            where T : ISerializable, new()
        {
            Read____(input,
                     list,
                     () =>
                     {
                         var item = new T();
                         item.Deserialize(input);
                         return item;
                     });
        }

        public static void Write<T>(Stream output, List<T> list)
            where T : ISerializable, new()
        {
            Write(output, list, v => v.Serialize(output));
        }

        // ReSharper disable InconsistentNaming
        public static void Read____<T>(Stream input, List<T> list, short version)
            // ReSharper restore InconsistentNaming
            where T : IVersionedSerializable, new()
        {
            Read____(input,
                     list,
                     () =>
                     {
                         var item = new T();
                         item.Deserialize(input, version);
                         return item;
                     });
        }

        public static void Write<T>(Stream output, List<T> list, short version)
            where T : IVersionedSerializable, new()
        {
            Write(output, list, v => v.Serialize(output, version));
        }

        // ReSharper disable InconsistentNaming
        private static void Read____<T>(T[] array, Func<T> readItem)
            // ReSharper restore InconsistentNaming
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            var count = array.Length;
            for (int i = 0; i < count; i++)
            {
                array[i] = readItem();
            }
        }

        private static void Write<T>(T[] array, Action<T> writeItem)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            var count = array.Length;
            for (int i = 0; i < count; i++)
            {
                writeItem(array[i]);
            }
        }

        // ReSharper disable InconsistentNaming
        public static void Read____(Stream input, ulong[] array)
            // ReSharper restore InconsistentNaming
        {
            Read____(array, () => input.ReadValueU64(Endian.Big));
        }

        public static void Write(Stream output, ulong[] array)
        {
            Write(array, v => output.WriteValueU64(v, Endian.Big));
        }

        // ReSharper disable InconsistentNaming
        public static void Read____<T>(Stream input, T[] array)
            // ReSharper restore InconsistentNaming
            where T : ISerializable, new()
        {
            Read____(array,
                     () =>
                     {
                         var item = new T();
                         item.Deserialize(input);
                         return item;
                     });
        }

        public static void Write<T>(Stream output, T[] array)
            where T : ISerializable, new()
        {
            Write(array, v => v.Serialize(output));
        }
    }
}
