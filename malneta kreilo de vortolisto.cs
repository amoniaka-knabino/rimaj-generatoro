using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParserEsperanto
{
    class Program
    {
        static void Main(string[] args)
        {
            var WordList = new List<string>();
            AddWordsToList(@"C:\scripts\esperanto\parser\test.txt", WordList);
            Console.WriteLine("Vortolisto");
            foreach (var vorto in WordList)
            {
                Console.WriteLine( vorto);
            }
            WriteListToFile(@"C:\scripts\esperanto\parser\temp.txt", WordList);
        }

        static void AddWordsToList(string path, List<string> list)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.ToLower();
                    var patternChooseWords = @"[^a-z'ĝĥĵŝŭĉ]";
                    Regex regex = new Regex(patternChooseWords);
                    line = regex.Replace(line, " ");
                    string[] curArr = line.Split(' ');
                    for (int i = 0; i < curArr.Length; i++)
                    {
                        if (curArr[i] == "")
                        {
                            continue;
                        }
                        var len = curArr[i].Length;
                        if (len!=1 && curArr[i][len - 1] == '\'')
                        {
                            curArr[i] = curArr[i].Substring(0, len - 1) + "o";
                        }
                        if (!(list.Contains(curArr[i])))
                            list.Add(curArr[i]);
                    }

                }
            }
        }

        static void WriteListToFile(string path, List<string> list)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                foreach (var vorto in list)
                {
                    sw.WriteLine(vorto);
                }
            }
        }
    }
}
