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
using System.Linq;
using Gibbed.ProjectData;
using Gibbed.Sims4.FileFormats;
using NDesk.Options;

namespace FindTypes
{
    internal class Program
    {
        private static string GetExecutableName()
        {
            return Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        private static void Main(string[] args)
        {
            bool showHelp = false;
            string currentProject = null;

            var options = new OptionSet()
            {
                { "h|help", "show this message and exit", v => showHelp = v != null },
                { "p|project=", "override current project", v => currentProject = v },
            };

            List<string> extras;

            try
            {
                extras = options.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("{0}: ", GetExecutableName());
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `{0} --help' for more information.", GetExecutableName());
                return;
            }

            if (extras.Count != 0 || showHelp == true)
            {
                Console.WriteLine("Usage: {0} [OPTIONS]+", GetExecutableName());
                Console.WriteLine();
                Console.WriteLine("Options:");
                options.WriteOptionDescriptions(Console.Out);
                return;
            }

            var manager = Manager.Load(currentProject);
            if (manager.ActiveProject == null)
            {
                Console.WriteLine("Nothing to do: no active project loaded.");
                return;
            }

            var project = manager.ActiveProject;
            var installPath = project.InstallPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            var inputPaths = Directory.GetFiles(installPath, "*.package", SearchOption.AllDirectories);

            var typeIds = new List<uint>();
            foreach (var inputPath in inputPaths)
            {
                var dbpf = new DatabasePackedFile();
                using (var input = File.OpenRead(inputPath))
                {
                    dbpf.Read(input);
                    typeIds.AddRange(dbpf.Entries.Select(e => e.Key.Type).Distinct());
                }
            }

            foreach (var typeId in typeIds.OrderBy(ti => ti))
            {
                Console.WriteLine("{0:X8}", typeId);
            }
        }
    }
}
