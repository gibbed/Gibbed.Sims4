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

namespace Gibbed.Sims4.FileFormats
{
    public static class ResourceKeyHelper
    {
        public static ResourceKey ReadResourceKeyTGI(this Stream stream, Endian endian)
        {
            var typeId = stream.ReadValueU32(endian);
            var groupId = stream.ReadValueU32(endian);
            var instanceId = stream.ReadValueU64(endian);
            return new ResourceKey(instanceId, typeId, groupId);
        }

        public static void WriteResourceKeyTGI(this Stream stream, ResourceKey key, Endian endian)
        {
            stream.WriteValueU32(key.Type, endian);
            stream.WriteValueU32(key.Group, endian);
            stream.WriteValueU64(key.Instance, endian);
        }

        public static ResourceKey ReadResourceKeyIGT(this Stream stream, Endian endian)
        {
            var instanceId = stream.ReadValueU64(endian);
            var groupId = stream.ReadValueU32(endian);
            var typeId = stream.ReadValueU32(endian);
            return new ResourceKey(instanceId, typeId, groupId);
        }

        public static void WriteResourceKeyIGT(this Stream stream, ResourceKey key, Endian endian)
        {
            stream.WriteValueU64(key.Instance, endian);
            stream.WriteValueU32(key.Group, endian);
            stream.WriteValueU32(key.Type, endian);
        }
    }

    public struct ResourceKey
    {
        public static readonly ResourceKey Invalid;

        static ResourceKey()
        {
            Invalid = new ResourceKey(0, 0, 0);
        }

        public readonly uint Type;
        public readonly uint Group;
        public readonly ulong Instance;

        public ResourceKey(ulong instanceId, uint typeId, uint groupId)
        {
            this.Instance = instanceId;
            this.Type = typeId;
            this.Group = groupId;
        }

        public ResourceKey(ulong instance, uint type)
            : this(instance, type, 0)
        {
        }

        public string ToPath()
        {
            return string.Format("{0:X8}-{1:X8}-{2:X16}",
                this.Type, this.Group, this.Instance);
        }

        public override string ToString()
        {
            return string.Format("{0:X8}:{1:X8}:{2:X16}",
                this.Type, this.Group, this.Instance);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            return (ResourceKey)obj == this;
        }

        public static bool operator !=(ResourceKey a, ResourceKey b)
        {
            return
                a.Type != b.Type ||
                a.Group != b.Group ||
                a.Instance != b.Instance;
        }

        public static bool operator ==(ResourceKey a, ResourceKey b)
        {
            return
                a.Type == b.Type &&
                a.Group == b.Group &&
                a.Instance == b.Instance;
        }

        public override int GetHashCode()
        {
            return this.Instance.GetHashCode() ^ ((int)(this.Type ^ (this.Group << 16)));
        }
    }
}
