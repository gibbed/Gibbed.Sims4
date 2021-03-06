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
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace Gibbed.Sims4.Unpack
{
    [XmlRoot(ElementName = "groups")]
    public class PackageGroupLookup
    {
        public PackageGroupLookup()
        {
            this.Groups = new List<GroupInfo>();
        }

        [XmlElement("group")]
        public List<GroupInfo> Groups { get; set; }

        public GroupInfo GetGroupInfo(uint id)
        {
            return this.Groups.FirstOrDefault(t => t.Id == id);
        }

        public class GroupInfo
        {
            [XmlIgnore]
            public uint Id { get; set; }

            [XmlAttribute("id")]
            public string IdAsText
            {
                get { return "0x" + this.Id.ToString("X8", CultureInfo.InvariantCulture); }
                set
                {
                    if (value == null)
                    {
                        this.Id = 0;
                    }
                    else if (value.StartsWith("0x") == true)
                    {
                        uint dummy;
                        if (uint.TryParse(value.Substring(2),
                                          NumberStyles.HexNumber,
                                          CultureInfo.InvariantCulture,
                                          out dummy) == false)
                        {
                            throw new FormatException("failed to parse uint");
                        }
                        this.Id = dummy;
                    }
                    else
                    {
                        uint dummy;
                        if (uint.TryParse(value, out dummy) == false)
                        {
                            throw new FormatException("failed to parse uint");
                        }
                        this.Id = dummy;
                    }
                }
            }

            [XmlText]
            public string Name { get; set; }
        }
    }
}
