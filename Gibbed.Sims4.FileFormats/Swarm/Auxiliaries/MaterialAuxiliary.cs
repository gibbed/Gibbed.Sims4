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

namespace Gibbed.Sims4.FileFormats.Swarm.Auxiliaries
{
    public class MaterialAuxiliary : IAuxiliary
    {
        #region Properties
        public ulong MaterialId
        {
            get { return this._MaterialId; }
            set { this._MaterialId = value; }
        }

        public ulong ShaderId
        {
            get { return this._ShaderId; }
            set { this._ShaderId = value; }
        }

        public List<Parameter> Parameters
        {
            get { return this._Parameters; }
        }
        #endregion

        #region Fields
        private ulong _MaterialId;
        private ulong _ShaderId;
        private readonly List<Parameter> _Parameters;
        #endregion

        public MaterialAuxiliary()
        {
            this._Parameters = new List<Parameter>();
        }

        public void Serialize(Stream output, short version)
        {
            Binary.Write(output, this._MaterialId);
            Binary.Write(output, this._ShaderId);
            Binary.Write(output, this._Parameters);
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._MaterialId);
            Binary.Read(input, out this._ShaderId);
            Binary.Read____(input, this._Parameters);
        }

        public enum ParameterType : byte
        {
            Float = 0,
            Int = 1,
            Bool = 2,
            Floats = 3,
            Ints = 4,
            Bools = 5,
            TextureId = 6,
        }

        public class Parameter : ISerializable
        {
            #region Properties
            public ulong Id
            {
                get { return this._Id; }
                set { this._Id = value; }
            }

            public ParameterType Type
            {
                get { return (ParameterType)this._Type; }
                set { this._Type = (byte)value; }
            }

            public ulong TextureId
            {
                get { return this._TextureId; }
                set { this._TextureId = value; }
            }

            public float Float
            {
                get { return this._Float; }
                set { this._Float = value; }
            }

            public int Int
            {
                get { return this._Int; }
                set { this._Int = value; }
            }

            public bool Bool
            {
                get { return this._Bool; }
                set { this._Bool = value; }
            }

            public List<float> Floats
            {
                get { return this._Floats; }
            }

            public List<int> Ints
            {
                get { return this._Ints; }
            }

            public List<bool> Bools
            {
                get { return this._Bools; }
            }
            #endregion

            #region Fields
            private ulong _Id;
            private byte _Type;
            private ulong _TextureId;
            private float _Float;
            private int _Int;
            private bool _Bool;
            private readonly List<float> _Floats;
            private readonly List<int> _Ints;
            private readonly List<bool> _Bools;
            #endregion

            public Parameter()
            {
                this._Floats = new List<float>();
                this._Ints = new List<int>();
                this._Bools = new List<bool>();
            }

            public void Serialize(Stream output)
            {
                Binary.Write(output, this._Id);
                Binary.Write(output, this._Type);

                switch (this.Type)
                {
                    case ParameterType.Float:
                    {
                        Binary.Write(output, this._Float);
                        break;
                    }

                    case ParameterType.Int:
                    {
                        Binary.Write(output, this._Int);
                        break;
                    }

                    case ParameterType.Bool:
                    {
                        Binary.Write(output, this._Bool);
                        break;
                    }

                    case ParameterType.Floats:
                    {
                        Binary.Write_s(output, this._Floats);
                        break;
                    }

                    case ParameterType.Ints:
                    {
                        Binary.Write_s(output, this._Ints);
                        break;
                    }

                    case ParameterType.Bools:
                    {
                        Binary.Write_s(output, this._Bools);
                        break;
                    }

                    case ParameterType.TextureId:
                    {
                        Binary.Write(output, this._TextureId);
                        break;
                    }

                    default:
                    {
                        throw new NotSupportedException();
                    }
                }
            }

            public void Deserialize(Stream input)
            {
                Binary.Read(input, out this._Id);
                Binary.Read(input, out this._Type);

                switch (this.Type)
                {
                    case ParameterType.Float:
                    {
                        Binary.Read(input, out this._Float);
                        break;
                    }

                    case ParameterType.Int:
                    {
                        Binary.Read(input, out this._Int);
                        break;
                    }

                    case ParameterType.Bool:
                    {
                        Binary.Read(input, out this._Bool);
                        break;
                    }

                    case ParameterType.Floats:
                    {
                        Binary.Read_s__(input, this._Floats);
                        break;
                    }

                    case ParameterType.Ints:
                    {
                        Binary.Read_s__(input, this._Ints);
                        break;
                    }

                    case ParameterType.Bools:
                    {
                        Binary.Read_s__(input, this._Bools);
                        break;
                    }

                    case ParameterType.TextureId:
                    {
                        Binary.Read(input, out this._TextureId);
                        break;
                    }

                    default:
                    {
                        throw new NotSupportedException();
                    }
                }
            }
        }
    }
}
