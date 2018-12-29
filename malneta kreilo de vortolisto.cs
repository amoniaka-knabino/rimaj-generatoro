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
            var input = new string[] { @"C:\scripts\esperanto\parser\input\Ulrich_Fajron-sentas-mi-interne_RuLit_Me.txt",
            @"C:\scripts\esperanto\parser\input\Tolkin_La-Mastro-de-l-Ringoj_RuLit_Me.txt",
            @"C:\scripts\esperanto\parser\input\Lermontov_Rusa-literaturo_3_Princidino-Mary_RuLit_Me.txt",
            @"C:\scripts\esperanto\parser\input\Dikkens_La-postlasitaj-paperoj-de-la-Klubo-Pikvika_RuLit_Me.txt",
            @"C:\scripts\esperanto\parser\input\Gashek_Aventuroj-de-la-brava-soldato-Svejk-dum-la-mondmilito_RuLit_Me.txt"};
            Console.WriteLine("Start");
            var WordList = new List<string>(); // words will be added, but don't replaced
            var RadikoList = new List<string>();
            foreach (var specialaVorto in SpecialWords)
            {
                WordList.Add(specialaVorto);
            }

            for (int i = 0; i < input.Length; i++)
            {
                Console.WriteLine("Work with " + i);
                AddWordsToList(input[i], WordList);
            }
            Console.WriteLine("Slow Sort");
            SlowSort(WordList);
            WriteListToFile(@"C:\scripts\esperanto\parser\temp.txt", WordList);
            Console.WriteLine("Done");

        }

        static void SlowSort(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                char[] arr = list[i].ToCharArray();
                Array.Reverse(arr);
                list[i] = new string(arr);
            }
            list.Sort();
            for (int i = 0; i < list.Count; i++)
            {
                char[] arr = list[i].ToCharArray();
                Array.Reverse(arr);
                list[i] = new string(arr);
            }
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
                        if (curArr[i].Length>1 && !(SpecialWords.Contains(curArr[i])))
                            curArr[i] = DeleteEnding(curArr[i]); // make it dynamic
                        if (!(list.Contains(curArr[i])))
                            list.Add(curArr[i]);
                    }

                }
            }
        }

        static string DeleteEnding(string vorto)
        {
            var lastChar = vorto[vorto.Length - 1];
            if ((lastChar == 's' || lastChar == 'j' || lastChar == 'n' && vorto[vorto.Length-2] != 'j') && vorto.Length>2)
            {
                return vorto.Substring(0, vorto.Length - 2) + "'";
            }
            if (lastChar == 'o' || lastChar == 'a' || lastChar == 'e' || lastChar == 'u' || lastChar =='i' || lastChar == '\'') // ' излишен, но на всякий случай
            {
                return vorto.Substring(0, vorto.Length - 1) + "'";
            }
            if ((lastChar == 'n' && vorto[vorto.Length - 2] == 'j') && vorto.Length>3)
            {
                return vorto.Substring(0, vorto.Length - 3) + "'";
            }
            return vorto; // program should ever go here
        }

        static void WriteListToFile(string path, List<string> list)
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                //Console.WriteLine("Vortolisto"); // should it be input parameter?
                foreach (var vorto in list)
                {
                    sw.WriteLine( vorto);
                    //Console.WriteLine(vorto);
                }
            }
        }

        static Dictionary<string, string> MakeRhymesDictionary(List<string> wordList)
        {
            var rhymesDictionary = new Dictionary<string, string>();

            return rhymesDictionary;
        }

        static List<string> MakeTempWordList(List<string> wordList) // заменить пары глухих и звонких на одну из пары
        {
            var tempList = new List<string>();
            foreach (var vorto in wordList) // заменять на звонкие
            {
                var novaVorto = vorto;
                novaVorto.Replace('p','b');
                novaVorto.Replace('f', 'v');
                novaVorto.Replace('t', 'd');
                novaVorto.Replace('k', 'g');
                novaVorto.Replace('h', 'ĥ');
                novaVorto.Replace('ŝ', 'ĵ');
            }
            return tempList;
        }


        /*static readonly string[] SpecialWords1 = new string[] { "ĉu", "mi", "vi", "li", "ŝi", "ĝi", "ni", "ili",
                    "kaj", "aŭ", "sed", "ĉar", "se", "do", "kvankam", "kvazaŭ", "tamen", "ke",
                    "kiam", "kiel", "kiom", "kial", "kien", "kies", "ki,",
                    "tiam", "ĉiam","iam", "ti'", "i'", "ĉi'",
                    "nenio", "nenie", "neniam",
                    "jes", "ne", "jam", "ankoraŭ", "ĵus", "nun", "tuj",
                    "hieraŭ", "hodiaŭ", "morgaŭ", "baldaŭ", "ankaŭ",
                    "eĉ", "nur", "preskaŭ", "tre", "tro", "ja", "mem", "ambaŭ", "for", "jen", "nul",
                    "unu'", "du'", "tri'", "kvar'", "kvin'", "ses'", "sep'", "ok'", "naŭ'", "dek'", "cent'", "mil'", // unu du tri will make an error...
                    "en", "al", "el", "inter", "sub", "sur", "super", "antaŭ", "ĉe", "trans", "tra", "ĉirkaŭ", "ĝis",
                    "de", "ekster", "apud", "preter", "pri", "kun", "sen", "kontraŭ", "anstataŭ", "post", "per", "laŭ",
                    "dum", "krom", "malgraŭ", "por", "pro", "po", "da", "je", "la", "pli", "plej" }; */
        static readonly string[] SpecialWords = new string[] { "ĉu", "mi", "vi", "li", "ŝi", "ĝi", "ni", "ili",
                "kaj", "aŭ", "sed", "ĉar", "se", "do", "kvankam", "kvazaŭ", "tamen", "ke",
                "kiam", "kiel", "kiom", "kial", "kien", "kies", "kio", "kiu", "kie", "kia",
                "tiam", "ĉiam","iam", "tio", "tia", "tie", "tiu", "io", "ia", "iu", "ie", "ĉio","ĉie","ĉia","ĉiu",
                "nenio", "nenie", "neniam",
                "jes", "ne", "jam", "ankoraŭ", "ĵus", "nun", "tuj",
                "hieraŭ", "hodiaŭ", "morgaŭ", "baldaŭ", "ankaŭ", 
                "eĉ", "nur", "preskaŭ", "tre", "tro", "ja", "mem", "ambaŭ", "for", "jen", "nul",
                "unu'", "du'", "tri'", "kvar'", "kvin'", "ses'", "sep'", "ok'", "naŭ'", "dek'", "cent'", "mil'", // unu du tri will make an error...
                "en", "al", "el", "inter", "sub", "sur", "super", "antaŭ", "ĉe", "trans", "tra", "ĉirkaŭ", "ĝis",
                "de", "ekster", "apud", "preter", "pri", "kun", "sen", "kontraŭ", "anstataŭ", "post", "per", "laŭ",
                "dum", "krom", "malgraŭ", "por", "pro", "po", "da", "je", "la", "pli", "plej" };



    }

}
