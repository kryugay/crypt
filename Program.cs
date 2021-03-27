using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu();
            int k;
            Console.WriteLine("Введите k:");
            k = Convert.ToInt32(Console.ReadLine());
            generate_text(k);

            Console.ReadKey();
        }

        static void Menu()
        {
            int x = 1;
            while (x!=0){
                Console.WriteLine("Введите код операции:");

                Console.WriteLine("1. Привести символы к нижнему регистру");
                Console.WriteLine("2. Распечатать содержимое файла");
                Console.WriteLine("3. Показать символы");
                Console.WriteLine("4. Удаление символа");
                Console.WriteLine("0. Завершить обработку");

                x = Convert.ToInt32(Console.ReadLine());

                switch (x)
                {
                    case 1:
                        gotolow();
                        break;
                    case 2:
                        printfile();
                        break;
                    case 3:
                        printsymbols();
                        break;
                    case 4:
                        deletesymbol();
                        break;
                    default:
                        break;

                }
            }
        }

        static void gotolow()
        {
            FileStream file1 = new FileStream("C:\\text.txt", FileMode.Open);
            FileStream file2 = new FileStream("C:\\t.txt", FileMode.Create); 

            StreamWriter writer = new StreamWriter(file2);
            StreamReader reader = new StreamReader(file1);
            writer.Write(reader.ReadToEnd().ToLower());

            reader.Close();
            writer.Close();

            file1 = new FileStream("C:\\text.txt", FileMode.Create);
            file2 = new FileStream("C:\\t.txt", FileMode.Open);
            writer = new StreamWriter(file1);
            reader = new StreamReader(file2);

            writer.Write(reader.ReadToEnd());
            reader.Close();
            writer.Close();
            File.Delete("C:\\t.txt");
        }
        static void printfile()
        {
            FileStream file1 = new FileStream("C:\\text.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file1);
            Console.WriteLine(reader.ReadToEnd());
            reader.Close();
        }

        static void printsymbols()
        {
            List<char> symbols = new List<char>();
            FileStream file1 = new FileStream("C:\\text.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file1);
            foreach (var j in reader.ReadToEnd())
            {
                symbols.Sort();
                if (symbols.BinarySearch(j) < 0)
                {
                    symbols.Add(j);
                }
            }
            foreach (var q in symbols)
            {
                Console.WriteLine(q);
            }
            reader.Close();
        }

        static void deletesymbol()
        {
            FileStream file1 = new FileStream("C:\\text.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file1);

            Console.WriteLine("Введите удаляемый символ:");
            var x = Console.ReadLine();

            string data = reader.ReadToEnd();
            data = data.Replace(x, "");

            //Console.Write(data);
            
            reader.Close();

            file1 = new FileStream("C:\\text.txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(file1);
            writer.Write(data);

            writer.Close();

        }

        static Dictionary<string, double> k_gramms(int k)
        {
            Dictionary<string, int> KGRAMMS = new Dictionary<string, int>();
            Dictionary<string, double> K_GRAMMS = new Dictionary<string, double>();

            List<string> k_gramm = new List<string>();

            FileStream file1 = new FileStream("C:\\text.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file1);
            string text = reader.ReadToEnd();
            reader.Close();

            while (text.Length >= k)
            {
                //Console.WriteLine(text.Length);

                k_gramm.Sort();
                if (k_gramm.BinarySearch(text.Substring(0, k)) < 0)
                {
                    k_gramm.Add(text.Substring(0, k));
                    KGRAMMS.Add(text.Substring(0, k), 1);
                    K_GRAMMS.Add(text.Substring(0, k), 1.0);
                }
                else
                    KGRAMMS[text.Substring(0, k)]++;

                text = text.Substring(1);
                //Console.WriteLine(text);
                //Console.WriteLine(text.Length);
            }
            
            int s = 0;

            foreach (KeyValuePair<string, int> keyValue in KGRAMMS)
            {
                s += keyValue.Value;
                
            }

            foreach (KeyValuePair<string, int> keyValue in KGRAMMS)
            {
                K_GRAMMS[keyValue.Key] = keyValue.Value/Convert.ToDouble(s);
            }
            
            /*foreach (KeyValuePair<string, int> keyValue in KGRAMMS)
            {
                Console.WriteLine(keyValue.Key + " - " + keyValue.Value);
            }

            foreach (KeyValuePair<string, double> keyValue in K_GRAMMS)
            {
                Console.WriteLine(keyValue.Key + " - " + keyValue.Value);
            }
            Console.ReadLine();*/

            return K_GRAMMS;
        }


        static Dictionary<string, double> frec_k1(string gram)//gram длины k-1
        {
            FileStream file1 = new FileStream("C:\\text.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file1);
            string text = reader.ReadToEnd();
            reader.Close();

            int k = gram.Length + 1;

            List<string> k_gramm = new List<string>();

            Dictionary<string, int> KGRAMMS = new Dictionary<string, int>();
            Dictionary<string, double> K_GRAMMS = new Dictionary<string, double>();

            while (text.Length >= k)
            {
                k_gramm.Sort();
                
                if (text.Substring(0, k - 1) == gram)
                {
                    if (k_gramm.BinarySearch(text.Substring(0, k)) < 0)
                    {
                        k_gramm.Add(text.Substring(0, k));
                        KGRAMMS.Add(text.Substring(0, k), 1);
                        K_GRAMMS.Add(text.Substring(0, k), 1.0);
                    }
                    else
                        KGRAMMS[text.Substring(0, k)]++;
                }
                text = text.Substring(1);
            }

            int s = 0;

            foreach (KeyValuePair<string, int> keyValue in KGRAMMS)
            {
                s += keyValue.Value;

            }
            foreach (KeyValuePair<string, int> keyValue in KGRAMMS)
            {
                K_GRAMMS[keyValue.Key] = keyValue.Value / Convert.ToDouble(s);
            }
            /*foreach (KeyValuePair<string, double> keyValue in K_GRAMMS)
            {
                Console.WriteLine(keyValue.Key + " - " + keyValue.Value);
            }
            Console.ReadLine();*/


            return K_GRAMMS;
        }



        static string frec_gramm(Dictionary<string, double> words, double a)
        {
            double s = 0.0;
            string flag = "";

            foreach (KeyValuePair<string, double> keyValue in words)
            {

                if (s <= a)
                {
                    flag = keyValue.Key;
                    s += keyValue.Value;
                }
                else
                    break;

            }
            
            return flag;
        }

        static void generate_text(int k)
        {
           
            Random rnd = new Random();
            double a = rnd.NextDouble();

            string text = "";

            Dictionary<string, double> words = new Dictionary<string, double>();
            words = k_gramms(k);

            text += frec_gramm(words, a);

            string word;

            for(var i = 0; i<1000; i++)
            {
                a = rnd.NextDouble();
                
                words = frec_k1(text.Substring(text.Length - k + 1, k - 1));
                word = frec_gramm(words, a);
                if (word.Length == 0)
                {
                    int j = 2;
                    while (word.Length == 0)
                    {
                        a = rnd.NextDouble();
                        words = frec_k1(text.Substring(text.Length - k + j, k - j));
                        word = frec_gramm(words, a);
                        j++;
                    }
                    text += word[word.Length-1];
                }
                
                else
                    text += word[k - 1];
                //Console.WriteLine(word[word.Length - 1]);
            }

            FileStream file1 = new FileStream("C:\\text1.txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(file1);
            writer.Write(text);

            writer.Close();

            Console.WriteLine(text);
            Console.ReadKey();
         

        }

    }
}
