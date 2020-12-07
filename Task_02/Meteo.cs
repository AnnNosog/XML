using System;
using System.IO;
using System.Xml;

namespace Task_02
{
    internal class Meteo
    {
        #region Methods
        public void Go()
        {
            bool exit = true;
            int count = 0;

            while (exit)
            {
                Console.WriteLine("Выберите город. Для выхода нажмите 0(нуль).");

                GetWeather(CitySelection(), ref exit, ref count);

                for (int i = 0; i < 50; i++)
                {
                    Console.Write("_");
                }
                Console.WriteLine();
            }

            if (count == 0)
            {
                return;
            }

            Console.WriteLine("Вывести самую тёплую погоду? y/n");
            string input = Console.ReadLine();

            if (input == "y")
            {
                WarmSity();
            }

            File.Delete("Statistic.txt");
        }

        private string[] CitySelection(string sityFilePath = "Sity.txt")
        {
            using (StreamReader streamReader = new StreamReader(sityFilePath))
            {
                string[] keysSitys = File.ReadAllLines(sityFilePath);
                int quantityLine = File.ReadAllLines(sityFilePath).Length;

                for (int i = 0; i < quantityLine; i++)
                {
                    string[] splitedLine = keysSitys[i].Split('>');

                    Console.Write($"{i + 1} - ");
                    Console.WriteLine(splitedLine[1]);
                }

                int numberSity = Convert.ToInt32(Console.ReadLine());

                if (numberSity == 0)
                {
                    string[] bufNull = new string[] { "0" };
                    return bufNull;
                }

                if (numberSity < 0 || numberSity > quantityLine)
                {
                    throw new ArgumentException("Неверный ввод города");
                }

                string[] buf = keysSitys[numberSity - 1].Split('>');

                return buf;
            }
        }

        private void GetWeather(string[] keySity, ref bool exit, ref int count)
        {
            if (Convert.ToInt32(keySity[0]) == 0)
            {
                count += 0;
                exit = false;
                return;
            }

            string url = $"http://informer.gismeteo.by/rss/{keySity[0]}.xml";
            XmlDocument document = new XmlDocument();

            document.Load(url);
            document.Save($"{DateTime.Now.ToShortDateString()}_{keySity[1]}.xml");

            XmlNodeList nodes = document.GetElementsByTagName("item");

            foreach (XmlNode item in nodes)
            {
                string titleText = $"{item["title"].InnerText}";
                Console.WriteLine(titleText);
                WriteWeather(titleText);

                string descriptionText = $"{item["description"].InnerText}";
                Console.WriteLine(descriptionText);
                WriteWeather(descriptionText);
            }

            count++;
        }

        public void AddSity(int code, string sity, string filePath = "Sity.txt")
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine($"{code}>{sity}");
            }
        }

        private void WriteWeather(string text, string filePath = "Statistic.txt")
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                streamWriter.WriteLine(text);
            }
        }

        private void WarmSity(string filePath = "Statistic.txt")
        {
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string[] statistic = File.ReadAllLines(filePath);
                int quantityLine = File.ReadAllLines(filePath).Length;
                string findString1 = " С,";
                string findString2 = "..";
                int maxTemperature = -100;
                int indexMax = 0;

                for (int i = 1; i < quantityLine; i += 2)
                {
                    int indextFinish = statistic[i].IndexOf(findString1);
                    int indextBegin = statistic[i].IndexOf(findString2) + 2;
                    int lenght = indextFinish - indextBegin;
                    int temperature = Convert.ToInt32(statistic[i].Substring(indextBegin, lenght));

                    if (temperature > maxTemperature)
                    {
                        maxTemperature = temperature;
                        indexMax = i;
                    }
                }

                Console.WriteLine("Самая тёплая погода в:");
                Console.WriteLine(statistic[indexMax - 1]);
                Console.WriteLine(statistic[indexMax]);
            }
        }
        #endregion
    }
}
