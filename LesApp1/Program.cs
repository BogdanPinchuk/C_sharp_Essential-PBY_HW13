using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesApp1
{
    class Program
    {
        /// <summary>
        /// Кількість рядків
        /// </summary>
        private static int row = Console.LargestWindowHeight;
        /// <summary>
        /// Кількість колонок
        /// </summary>
        private static int col = Console.LargestWindowWidth;
        /// <summary>
        /// Набір символів для виведення (рандомно)
        /// </summary>
        private static string arrayData = "PinchukBohdanYuriyovych";
        /// <summary>
        ///  Масив даних для виведення цілими словами
        /// </summary>
        private static string[] arrayI = new string[]
        {
            "Pinchuk",
            "Bohdan",
            "Yuriyovych"
        };
        /// <summary>
        /// Випадкові значення
        /// </summary>
        Random rnd = new Random();

        static void Main()
        {
            // Заголовок
            Console.Title = "Matrix";

            // Join Unicode
            Console.OutputEncoding = Encoding.Unicode;





            // Repeat
            Console.ReadKey();
        }

        /// <summary>
        /// Запуск дощового слова
        /// </summary>
        /// <param name="word">Слово</param>
        /// <param name="col">номер колонки падіння</param>
        private static void RainWords(string word, int col)
        {

        }


    }
}
