using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retab
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (!args.Any())
                {
                    throw new Exception($"You must specify one or more asciidoc filenames to retabulate");
                }

                foreach (var filename in args)
                {
                    var retabber = new FileRetabber(filename);
                    retabber.Process();
                }
            }
            catch (Exception ex)
            {
                var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
                var progname = Path.GetFileNameWithoutExtension(fullname);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
