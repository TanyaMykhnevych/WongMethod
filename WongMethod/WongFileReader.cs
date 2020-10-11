using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WongMethod
{
    public static class WongFileReader
    {
        public static void ReadFile(string file, List<Clause> leftClauses, List<Clause> rightClauses)
        {
            try
            {
                using StreamReader reader = new StreamReader(file);
                string line;
                bool left = true;
                while ((line = reader.ReadLine()) != null)
                {
                    Clause c = new Clause();
                    if (string.IsNullOrEmpty(line))
                    {
                        left = false;
                        continue;
                    }

                    line.Split(' ').ToList().ForEach(v =>
                        c.AddVariable(new Variable(v.StartsWith('~') ? v.Substring(1) : v, v.StartsWith('~'))));

                    if (left) leftClauses.Add(c);
                    else rightClauses.Add(c);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found.");
            }
        }
    }
}
