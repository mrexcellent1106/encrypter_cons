using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace encrypter_console
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Добро пожаловать в приложение Шифр Цезаря v1.0 (beta)");
                Menu();
                string answer = Console.ReadLine();
                if (answer == "1") //ЗАШИФРОВАТЬ ТЕКСТ
                {
                    Console.Clear();
                    Console.WriteLine("Введите текст для шифрования: ");
                    string toCrypt = Console.ReadLine();
                    Console.WriteLine("Введите смещение(ключ): ");
                    try
                    {
                        int key = int.Parse(Console.ReadLine());
                        while (key < -33 || key > 33)
                        {
                            Console.Clear();
                            Console.WriteLine("Ключ выходит за допустимые пределы [-33, 33]");
                            Console.WriteLine("Введите смещение(ключ): ");
                            key = int.Parse(Console.ReadLine());
                        }
                        Console.Clear();

                        CaesarEncrypt(toCrypt, key, out string result);

                        Console.WriteLine("Результат шифрования: " + "\n");
                        Console.WriteLine(result);
                        Console.ReadKey();

                        Save(result);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        Console.Clear();
                        Console.WriteLine("Путь не найден. Проверьте правильность пути!");
                        Console.ReadKey();
                    }
                    catch (FormatException)
                    {
                        Console.Clear();
                        Console.WriteLine("Ошибка! Вы ввели недопустимый символ!");
                        Console.ReadKey();
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Пустое имя пути!");
                        Console.ReadKey();
                    }
                }
                else if (answer == "2") //РАСШИФРОВАТЬ ТЕКСТ
                {
                    try
                    {
                        Console.Clear();
                        Console.WriteLine("Укажите путь к файлу: ");
                        string openPath = Console.ReadLine();
                        Console.WriteLine("Введите смещение(ключ): ");
                        int key = int.Parse(Console.ReadLine());
                        while (key < -33 || key > 33)
                        {
                            Console.Clear();
                            Console.WriteLine("Ключ выходит за допустимые пределы [-33, 33]");
                            Console.WriteLine("Введите смещение(ключ): ");
                            key = int.Parse(Console.ReadLine());
                        }
                        Console.Clear();
                        Console.WriteLine("Выберите нужный вариант: ");
                        Console.WriteLine("1. Вывести расшифрованный текст на экран");
                        Console.WriteLine("2. Сохранить расшифрованный текст в файл");
                        
                        string ans = Console.ReadLine();
                        string code = GetCode(openPath);
                        Encoding charcode = Encoding.GetEncoding(code == "0" ? 65001 : 1251);
                        string res = File.ReadAllText(openPath, charcode);

                        if (ans == "1")
                        {
                            CaesarDecrypt_console(key, res, out string result);
                            Console.WriteLine("Результат расшифровки: " + "\n");
                            Console.WriteLine(result);
                            Console.ReadKey();
                        }
                        else if (ans == "2")
                        {
                            CaesarDecrypt_file(openPath, key, charcode);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Ошибка! Некорректный выбор.");
                            Console.ReadKey();
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        Console.Clear();
                        Console.WriteLine("Файл не найден. Проверьте правильность пути!");
                        Console.ReadKey();
                    }
                    catch (FormatException)
                    {
                        Console.Clear();
                        Console.WriteLine("Ошибка! Вы ввели недопустимый символ!");
                        Console.ReadKey();
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Пустое имя пути!");
                        Console.ReadKey();
                    }
                }
                else if (answer == "3") //ВЫХОД
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Ошибка. Некорректный выбор!");
                    Console.ReadKey();
                }
            }
        }


        public static void CaesarDecrypt_console(int key, string res, out string result) //РАСШИФРОВКА В КОНСОЛЬ (v)
        {
            int x;
            string small = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            string big = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            Console.Clear();
            char[] resArray = new char[res.Length];
            resArray = res.ToCharArray();

            for (int j = 0; j < resArray.Length; j++)
            {
               for (int i = 0; i < small.Length; i++)
               {
                  x = (i + 33 - key) % 33;
                  if (resArray[j].Equals(small[i]))
                  {
                     resArray[j] = small[x];
                     break;
                  }
                  else if (resArray[j] == big[i])
                  {
                     resArray[j] = big[x];
                     break;
                  }
               }
            }
            result = new string(resArray);
        }

        public static void CaesarDecrypt_file(string openPath, int key, Encoding charcode) //РАСШИФРОВКА В ФАЙЛ
        { 
            Console.Clear();
            int x;
            string small = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            string big = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            using (StreamReader sd = new StreamReader(openPath, charcode))
            {
               string cd = sd.ReadToEnd();
               Console.WriteLine("Укажите путь для сохранения расшифрованного файла: ");
               string writePath = Console.ReadLine();

                 using (StreamWriter sq = new StreamWriter(writePath, false, charcode))
                 {
                    for (int j = 0; j < cd.Length; j++)
                    {
                        if (cd[j] == ' ')
                          sq.Write(string.Format(" "));
                        if (cd[j] == '\n')
                          sq.Write(string.Format("\n"));
                        if (cd[j] == ',')
                          sq.Write(string.Format(","));
                        if (cd[j] == '!')
                          sq.Write(string.Format("!"));
                        if (cd[j] == '.')
                          sq.Write(string.Format("."));
                        if (cd[j] == '#')
                          sq.Write(string.Format("#"));
                        if (cd[j] == 'F')
                          sq.Write(string.Format("FirstLineSoftware"));
                        if (cd[j] == 'N')
                          sq.Write(string.Format("Net"));
                        for (int i = 0; i < 10; i++)
                        {
                           string k = i.ToString();
                           string l = cd[j].ToString();
                           if (String.Compare(l, k) == 0)
                           {
                              sq.Write(string.Format(k));
                           }
                        }
                        for (int i = 0; i <= 32; i++)
                        {
                           x = (i + 33 - key) % 33;
                           if (cd[j] == small[i])
                           {
                              sq.Write(string.Format(@"{0}", small[x]));
                           }
                           if (cd[j] == big[i])
                              sq.Write(string.Format(@"{0}", big[x]));
                        }
                    }
                    Console.WriteLine("Файл сохранен");
                    Console.ReadKey();
                 }
            }
        }

        public static void CaesarEncrypt(string toCrypt, int key, out string result) //ШИФРОВАНИЕ ТЕКСТА
        {
            string small = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            string big = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
            int x;
            char[] resArray = new char[toCrypt.Length];
            resArray = toCrypt.ToCharArray();

            for (int j = 0; j < resArray.Length; j++)
            {
                for (int i = 0; i < small.Length; i++)
                {
                    x = (i + 33 + key) % 33;
                    if (resArray[j].Equals(small[i]))
                    {
                        resArray[j] = small[x];
                        break;
                    }
                    else if (resArray[j] == big[i])
                    {
                        resArray[j] = big[x];
                        break;
                    }
                }
            }
            result = new string(resArray);
        }

        public static void Save(string result)
        {
            Console.WriteLine();
            Console.WriteLine("Сохранить результат в файл? (Y/N)");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                Console.Clear();
                Console.WriteLine("Укажите путь: ");
                string recordPath = Console.ReadLine();
                if (recordPath.Contains(":"))
                {
                    File.WriteAllText(recordPath, result, Encoding.UTF8);
                    Console.WriteLine("Файл сохранен");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Некорректный путь!");
                    Console.ReadKey();
                }
            }
            else if (answer.ToLower() == "n")
            {

            }
            else
            {
                Console.WriteLine("Некорректный ответ!");
                Console.ReadKey();
            }
        
        }

        public static string GetCode(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                Ude.CharsetDetector cdet = new Ude.CharsetDetector();
                cdet.Feed(fs);
                cdet.DataEnd();
                string ans;
                if (cdet.Charset.Contains("UTF"))
                {
                    ans = "0";
                    return (ans);
                }
                else if (cdet.Charset.Contains("windows"))
                {
                    ans = "1";
                    return (ans);
                }
                return ("0");
            }
        }

        public static void Menu()
        {
            Console.WriteLine("Выберите пункт меню: ");
            Console.WriteLine("1. Зашифровать текст");
            Console.WriteLine("2. Расшифровать текст");
            Console.WriteLine("3. Выход");
        }
    }
}