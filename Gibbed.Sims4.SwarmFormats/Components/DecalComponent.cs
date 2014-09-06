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

using System.Collections.Generic;
using System.IO;

namespace Gibbed.Sims4.SwarmFormats.Components
{
    public class DecalComponent : IComponent
    {
        #region Properties
        public uint Flags
        {
            get { return this._Flags; }
            set { this._Flags = value; }
        }

        public ulong Unknown10
        {
            get { return this._Unknown10; }
            set { this._Unknown10 = value; }
        }

        public byte Unknown18
        {
            get { return this._Unknown18; }
            set { this._Unknown18 = value; }
        }

        public uint Unknown1C
        {
            get { return this._Unknown1C; }
            set { this._Unknown1C = value; }
        }

        public byte Unknown20
        {
            get { return this._Unknown20; }
            set { this._Unknown20 = value; }
        }

        public uint Unknown24
        {
            get { return this._Unknown24; }
            set { this._Unknown24 = value; }
        }

        public float Lifetime
        {
            get { return this._Lifetime; }
            set { this._Lifetime = value; }
        }

        public List<float> RotationCurve
        {
            get { return this._RotationCurve; }
        }

        public List<float> SizeCurve
        {
            get { return this._SizeCurve; }
        }

        public List<float> AlphaCurve
        {
            get { return this._AlphaCurve; }
        }

        public List<Vector3> ColorCurve
        {
            get { return this._ColorCurve; }
        }

        public List<float> AspectCurve
        {
            get { return this._AspectCurve; }
        }

        public float AlphaVary
        {
            get { return this._AlphaVary; }
            set { this._AlphaVary = value; }
        }

        public float SizeVary
        {
            get { return this._SizeVary; }
            set { this._SizeVary = value; }
        }

        public float RotationVary
        {
            get { return this._RotationVary; }
            set { this._RotationVary = value; }
        }

        public float TextureRepeat
        {
            get { return this._TextureRepeat; }
            set { this._TextureRepeat = value; }
        }

        public Vector2 TextureOffset
        {
            get { return this._TextureOffset; }
            set { this._TextureOffset = value; }
        }

        public ulong EmitColorMapId
        {
            get { return this._EmitColorMapId; }
            set { this._EmitColorMapId = value; }
        }

        public byte Unknown78
        {
            get { return this._Unknown78; }
            set { this._Unknown78 = value; }
        }
        #endregion

        #region Fields
        private uint _Flags;
        private ulong _Unknown10;
        private byte _Unknown18;
        private uint _Unknown1C;
        private byte _Unknown20;
        private uint _Unknown24;
        private float _Lifetime;
        private readonly List<float> _RotationCurve;
        private readonly List<float> _SizeCurve;
        private readonly List<float> _AlphaCurve;
        private readonly List<Vector3> _ColorCurve;
        private readonly List<float> _AspectCurve;
        private float _AlphaVary;
        private float _SizeVary;
        private float _RotationVary;
        private float _TextureRepeat;
        private Vector2 _TextureOffset;
        private ulong _EmitColorMapId;
        private byte _Unknown78;
        #endregion

        public DecalComponent()
        {
            this._RotationCurve = new List<float>();
            this._SizeCurve = new List<float>();
            this._AlphaCurve = new List<float>();
            this._ColorCurve = new List<Vector3>();
            this._AspectCurve = new List<float>();
        }

        public void Serialize(Stream output, short version)
        {
            Binary.Write(output, this._Flags);
            Binary.Write(output, this._Unknown10);
            Binary.Write(output, this._Unknown18);
            Binary.Write(output, this._Unknown1C);
            Binary.Write(output, this._Unknown20);
            Binary.Write(output, this._Unknown24);
            Binary.Write(output, this._Lifetime);
            Binary.Write(output, this._RotationCurve);
            Binary.Write(output, this._SizeCurve);
            Binary.Write(output, this._AlphaCurve);
            Binary.Write(output, this._ColorCurve);
            Binary.Write(output, this._AspectCurve);
            Binary.Write(output, this._AlphaVary);
            Binary.Write(output, this._SizeVary);
            Binary.Write(output, this._RotationVary);
            Binary.Write(output, this._TextureRepeat);
            Binary.Write(output, this._TextureOffset);
            Binary.Write(output, this._EmitColorMapId);

            if (version >= 2)
            {
                Binary.Write(output, this._Unknown78);
            }
        }

        public void Deserialize(Stream input, short version)
        {
            Binary.Read(input, out this._Flags);
            this._Flags &= 0x7F;

            Binary.Read(input, out this._Unknown10);
            Binary.Read(input, out this._Unknown18);
            Binary.Read(input, out this._Unknown1C);
            Binary.Read(input, out this._Unknown20);
            Binary.Read(input, out this._Unknown24);
            Binary.Read(input, out this._Lifetime);
            Binary.Read____(input, this._RotationCurve);
            Binary.Read____(input, this._SizeCurve);
            Binary.Read____(input, this._AlphaCurve);
            Binary.Read____(input, this._ColorCurve);
            Binary.Read____(input, this._AspectCurve);
            Binary.Read(input, out this._AlphaVary);
            Binary.Read(input, out this._SizeVary);
            Binary.Read(input, out this._RotationVary);
            Binary.Read(input, out this._TextureRepeat);
            Binary.Read(input, out this._TextureOffset);
            Binary.Read(input, out this._EmitColorMapId);

            if (version >= 2)
            {
                Binary.Read(input, out this._Unknown78);
            }
            else
            {
                this._Unknown78 = 0;
            }
        }
    }
}
