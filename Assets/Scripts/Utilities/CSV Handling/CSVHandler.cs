using System;
using System.Collections.Generic;
using System.IO;

namespace SymptomsPlease.Utilities
{
    public static class CSVHandler
    {
        public static List<string[]> ReadCSVFile(string fileLocation)
        {
            return SplitCSVText(File.ReadAllText(fileLocation));
        }

        public static string[] SplitCSVIntoLines(string text)
        {
            return text.Contains("\r") ? text.Split(new string[] { "\r\n" }, StringSplitOptions.None) : text.Split(new char[] { '\n' });
        }

        public static List<string[]> SplitCSVText(string text)
        {
            string[] lines = SplitCSVIntoLines(text);

            var data = new List<string[]>();
            foreach (string line in lines)
            {
                var splitData = new List<string>();
                bool inQuote = false;
                string current = "";

                for (int i = 0; i < line.Length; i++)
                {
                    char chr = line[i];
                    if (chr == '"')
                    {
                        if (inQuote && current != "")
                        {
                            splitData.Add(current);
                            current = "";
                        }

                        inQuote = !inQuote;
                        continue;
                    }

                    if (chr == ',')
                    {
                        if (inQuote)
                        {
                            current += chr;
                            continue;
                        }

                        if (current == "")
                        {
                            if (line[i - 1] == ',')
                            {
                                splitData.Add(current);
                            }

                            continue;
                        }

                        splitData.Add(current);
                        current = "";
                        continue;
                    }

                    current += chr;
                }

                splitData.Add(current);

                data.Add(splitData.ToArray());
            }

            return data;
        }
    }
}
