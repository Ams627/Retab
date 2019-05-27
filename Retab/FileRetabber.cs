using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Retab
{
    class FileRetabber
    {
        readonly string _filename;
        StreamWriter _stream;

        public FileRetabber(string filename)
        {
            _filename = filename;
        }

        public void Process()
        {
            var outputFilename = $"{_filename}.retabbed";
            Console.WriteLine($"writing to {outputFilename}");
            using (_stream = new StreamWriter(outputFilename))
            {
                bool inTable = false;
                List<string> linelist = null;
                foreach (var line in File.ReadLines(_filename))
                {
                    if (line.StartsWith("|==="))
                    {
                        inTable = !inTable;
                        if (inTable)
                        {
                            linelist = new List<string>();
                        }
                        else
                        {
                            PrintLineList(linelist);
                        }
                    }
                    else if (inTable)
                    {
                        linelist.Add(line);
                    }
                    else
                    {
                        _stream.WriteLine(line);
                    }
                }
            }
        }

        private void PrintLineList(List<string> linelist)
        {
            Console.WriteLine("Printing");
            var columns = linelist.Select(x => x.Count(c => c == '|'));
            var columnCount = columns.GroupBy(x => x);
            if (columnCount.Count() != 1)
            {
                throw new Exception("table with variable number of columns cannot be formatted.");
            }

            var maximums = new int[columns.First()];
            foreach (var line in linelist)
            {
                var splitLine = line.Split('|').Select(x => x.Trim()).ToArray();
                for (int i = 1; i < splitLine.Length; ++i)
                {
                    if (splitLine[i].Length > maximums[i - 1])
                    {
                        maximums[i - 1] = splitLine[i].Length;
                    }
                }
            }

            _stream.WriteLine("|===");
            foreach (var line in linelist)
            {
                var splitLine = line.Split('|').Select(x => x.Trim()).ToArray();
                _stream.Write("|");
                for (int i = 1; i < splitLine.Length; ++i)
                {
                    var spaceLength = maximums[i - 1] - splitLine[i].Length;
                    _stream.Write($"{splitLine[i].Trim()}{new string(' ', spaceLength)} | ");
                }
                _stream.WriteLine();
            }
            _stream.WriteLine("|===");
        }
    }
}
