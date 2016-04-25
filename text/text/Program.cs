using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace text
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            StringBuilder Text = new StringBuilder("", 10000);
            StringBuilder StopText = new StringBuilder("", 10000);
            StringBuilder TextResult = new StringBuilder("", 1000);
            string ReadPath = @"C:\Users\Ilya\Documents\visual studio 2015\Projects\text\text.txt";
            string ReadPathStop = @"C:\Users\Ilya\Documents\visual studio 2015\Projects\text\stop.txt";
            string WritePath = @"C:\Users\Ilya\Documents\visual studio 2015\Projects\text\newtext.txt";
          
            
            

            using (StreamReader sr = new StreamReader(ReadPath, System.Text.Encoding.UTF8))
            {
                Text.Append(sr.ReadToEnd());
            }
            using (StreamReader sr = new StreamReader(ReadPathStop, System.Text.Encoding.UTF8))
            {
                StopText.Append(sr.ReadToEnd());
            }
            List<string> list = StopText.ToString().ToLower().Split(' ').ToList<string>();
            List<string> tempText = Text.ToString().ToLower().Split(new char[] { ' ', '2', '0', '1', '3', '4', '5', '6', '7', '8', '9', '-', ')', '(', '\n', '\t', ';', ':', ',', '.', '?' }).ToList<string>();
            List<string> TextAfterStemming = new List<string>();
            Console.WriteLine("Введите ключевые слова статьи через пробел");
            string[] KeyWords = Console.ReadLine().Split(' ');
            List<string> AlgKeyWords = new List<string>();
            Console.WriteLine("Введите поисковые запросы");
            foreach (string i in KeyWords)
            {
                if (Algoritm(i, list) != "NoNe")
                {
                    AlgKeyWords.Add(Algoritm(i, list));
                }
            }
            foreach (string i in tempText)
            {
                if (Algoritm(i, list) != "NoNe")
                {
                    TextAfterStemming.Add(Algoritm(i, list));
                }
            }
            Dictionary<string, double> DicWords = new Dictionary<string, double>();
            foreach(string i in TextAfterStemming)
            {
               if(i!=" ") if (!DicWords.ContainsKey(i))
                {
                    DicWords.Add(i, 1);
                }
                else
                {
                    DicWords[i] += 1;
                }
            }
            double sum = 0;
            foreach(var i in DicWords)
            {
                sum += i.Value;
            }
            foreach (var key in DicWords.Keys.ToList())
            {
                DicWords[key]= Math.Round(DicWords[key] / sum * 100, 2);
            }
            foreach (var i in DicWords)
            {
                sum += i.Value;
            }
            foreach (var i in AlgKeyWords)
            {
                if(DicWords.ContainsKey(i) ) DicWords[i] +=1;
            }
            foreach (var i in DicWords.OrderByDescending(pair => pair.Value))
            {
                    if(0.2< i.Value) TextResult.Append(i.Key + " - " + i.Value+'%').Append("\r\n");
            }
            Dictionary<string, double> summ = new Dictionary<string, double>();
            while (true) {
            string FindString = Console.ReadLine();
                if(FindString == "exit") { break; }
                if (FindString == "result")
                {
                    foreach (var i in summ.OrderByDescending(pair => pair.Value))
                    {
                        Console.WriteLine(i.Key + " - " + i.Value + " балов");
                    }
                        break;
                }
                string tempFindString = FindString;
                summ.Add(FindString, 0);
                string[] temp = FindString.Split(' ');
                List<string> AlgFindText = new List<string>();
                foreach (string i in temp)
                {
                    if (Algoritm(i, list) != "NoNe")
                    {
                        AlgFindText.Add( Algoritm(i, list));
                    }
                }
                
                double tempsum = 0;
                foreach (string i in AlgFindText)
                {
                   
                    if (DicWords.ContainsKey(i))
                    {
                        tempsum += DicWords[i];
                    }
                }
                tempsum /= AlgFindText.Count;
                summ[tempFindString] = tempsum;
                
            }

            using (StreamWriter sw = new StreamWriter(WritePath, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(TextResult);
            }
            Console.WriteLine("ОТРАБОТАЛ");
            Console.Read();
        }

        static public bool FindWord( List<string> stop, string s)
        {
            foreach(var i in stop)
            {
                if(s == i.ToString())
                {
                    return true;
                }
            }
            return false;
        }
        static public string Algoritm(string s, List<string> list)
        {
            IStemmer stemmer = new RussianStemmer();
            if (s != "" && !(FindWord(list, s.ToString())))
            {
                
                s = stemmer.Stem(s);
                return s;
            }
            else return "NoNe";
        }
      
      
    }

}
