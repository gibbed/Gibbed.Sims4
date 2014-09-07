namespace Gibbed.Sims4.FileFormats
{
    public static class PropertyStream
    {
        public const uint ChildPropertyKey = 0xFFF75A95u; // fnv32("child")
        public const uint BoolPropertyKey = 0x68FE5F59u; // fnv32("bool")
        public const uint Int8PropertyKey = 0x53C63F28u; // fnv32("int8")
        public const uint Int16PropertyKey = 0x021560C5u; // fnv32("int16")
        public const uint Int32PropertyKey = 0x0415642Bu; // fnv32("int32")
        public const uint Int64PropertyKey = 0x071568E6u; // fnv32("int64")
        public const uint UInt8PropertyKey = 0x52DF1177u; // fnv32("uint8")
        public const uint UInt16PropertyKey = 0xF328896Cu; // fnv32("uint16")
        public const uint UInt32PropertyKey = 0xF1288606u; // fnv32("uint32")
        public const uint UInt64PropertyKey = 0xEE28814Fu; // fnv32("uint64")
        public const uint FloatPropertyKey = 0x4EDCD7A9u; // fnv32("float")
        public const uint String8PropertyKey = 0xC87B4C72u; // fnv32("string8")
        public const uint String16PropertyKey = 0x15196597u; // fnv32("string16")
        public const uint Int8ArrayPropertyKey = 0x069AF2DCu;
        public const uint Int16ArrayPropertyKey = 0x5749AD31u;
        public const uint Int32ArrayPropertyKey = 0x5149A9DFu;
        public const uint Int64ArrayPropertyKey = 0x5249A512u;
        public const uint UInt8ArrayPropertyKey = 0x0783DC83u;
        public const uint UInt16ArrayPropertyKey = 0xA6744498u;
        public const uint UInt32ArrayPropertyKey = 0xA4744BF2u;
        public const uint UInt64ArrayPropertyKey = 0xBB744CBBu;
        public const uint FloatArrayPropertyKey = 0x1B801A5Du;
        public const uint ResourceKeyPropertyKey = 0xA4744BF2u;
        public const uint ResourceKeyArrayPropertyKey = 0xA4744BF2u;
    }
}
