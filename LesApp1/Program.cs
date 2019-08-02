using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LesApp1
{
    class Program
    {
        /// <summary>
        /// Кількість рядків
        /// </summary>
        private static int rowM = Console.WindowHeight;
        /// <summary>
        /// Кількість колонок
        /// </summary>
        private static int colM = Console.WindowWidth;
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
        private Random rnd = new Random();
        /// <summary>
        /// Блокування консолі
        /// </summary>
        private static object block = new object();

        static void Main()
        {
            // Заголовок
            Console.Title = "Matrix";

            // Join Unicode
            Console.OutputEncoding = Encoding.Unicode;

            #region testing
            /*
            Counter count = new Counter(10, 0);

            while (true)
            {
                Console.WriteLine(count.Iteration);
                Thread.Sleep(50);
            }
            */
            #endregion

            for (int i = 0; i < colM; i++)
            {
                new Thread(() => RainWords(arrayI[1], i)).Start();
                Thread.Sleep(500);
            }

            // Repeat
            //Console.ReadKey();
        }

        /// <summary>
        /// Запуск дощового слова
        /// </summary>
        /// <param name="word">Слово</param>
        /// <param name="col">номер колонки падіння</param>
        private static void RainWords(string word, int col)
        {
            // створення лічильника для кожної букви слова
            Counter[] counter = new Counter[word.Length + 1];

            // налаштування лічильників слова
            for (int i = 0; i < counter.Length; i++)
            {
                // установка початкових значень лічильника (-1 для рядків, бо уходить за межі і губиться одна буква)
                counter[i] = new Counter(rowM - 1, -i);
                // Останній для закреслення
            }

            // безкінечний цикл, пускаємо по кругу слово згори в низ
            while (true)
            {
                // пробіжка по всьому слову
                for (int i = 0; i < word.Length + 1; i++)
                {
                    // блокування доступу до виводу в консоль
                    lock (block)
                    {
                        int j = counter[i].Iteration;
                        // запис символа пвного кольору у відповідній позиції
                        if (j >= 0)
                        {
                            Console.ForegroundColor = counter[i].Color;
                            Console.SetCursorPosition(col, j);

                            // перевірка довжини слова, якщо іретація більша довжини 
                            // то пишемо пробіл
                            if (i < word.Length)
                            {
                                Console.Write(word[i]);
                            }
                            else
                            {
                                Console.Write(" ");
                            }
                        }

                    }
                }
                Thread.Sleep(50);
            }
        }


    }
}
