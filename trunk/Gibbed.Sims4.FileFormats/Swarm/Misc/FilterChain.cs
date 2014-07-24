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

namespace Gibbed.Sims4.FileFormats.Swarm
{
    public class FilterChain : ISerializable
    {
        #region Properties
        public List<Filter> Filters
        {
            get { return this._Filters; }
        }

        public List<FilterTemporaryBuffer> TemporaryBuffers
        {
            get { return this._TemporaryBuffers; }
        }

        public List<float> FloatParameters
        {
            get { return this._FloatParameters; }
        }

        public List<Vector3> Vector3Parameters
        {
            get { return this._Vector3Parameters; }
        }

        public List<Vector2> Vector2Parameters
        {
            get { return this._Vector2Parameters; }
        }

        public List<ulong> ResourceIdParameters
        {
            get { return this._ResourceIdParameters; }
        }
        #endregion

        #region Fields
        private readonly List<Filter> _Filters;
        private readonly List<FilterTemporaryBuffer> _TemporaryBuffers;
        private readonly List<float> _FloatParameters;
        private readonly List<Vector3> _Vector3Parameters;
        private readonly List<Vector2> _Vector2Parameters;
        private readonly List<ulong> _ResourceIdParameters;
        #endregion

        public FilterChain()
        {
            this._Filters = new List<Filter>();
            this._TemporaryBuffers = new List<FilterTemporaryBuffer>();
            this._FloatParameters = new List<float>();
            this._Vector3Parameters = new List<Vector3>();
            this._Vector2Parameters = new List<Vector2>();
            this._ResourceIdParameters = new List<ulong>();
        }

        public void Serialize(Stream output)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream input)
        {
            Binary.Read____(input, this._Filters);
            Binary.Read____(input, this._TemporaryBuffers);
            Binary.Read____(input, this._FloatParameters);
            Binary.Read____(input, this._Vector3Parameters);
            Binary.Read____(input, this._Vector2Parameters);
            Binary.Read____(input, this._ResourceIdParameters);
        }
    }
}
