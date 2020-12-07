using System;
using System.Xml.Linq;

namespace Task_01
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = @"http://www.nbrb.by/Services/XmlExRates.aspx";
            XDocument document = XDocument.Load(url);
            XElement root = document.Root;

            Console.WriteLine($"Курсы на дату: {root.Attribute("Date").Value}");

            foreach (var item in root.Elements())
            {
                Console.Write($"{item.Element("Name").Value} ");
                Console.Write($"{item.Element("Scale").Value} ");
                Console.Write($"{item.Element("CharCode").Value} - ");
                Console.WriteLine($"{item.Element("Rate").Value}");
            }

            Console.ReadKey();
        }
    }
}
