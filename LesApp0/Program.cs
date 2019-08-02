using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// гра в перегонки

namespace LesApp0
{
    class Program
    {
        // масив вибраних кольорів 
        private static ConsoleColor[] colorArray =
            new ConsoleColor[Enum.GetValues(typeof(ConsoleColor)).Length];
        /// <summary>
        /// Лічильник рівня рядків консолі
        /// </summary>
        private static int counter = 0;
        /// <summary>
        /// Продовження виконання дії
        /// </summary>
        private static int iterration = 80;
        /// <summary>
        /// для блокуваня доступу до консолі
        /// </summary>
        private static object block = new object();
        /// <summary>
        /// Для уствновки курсора в кінець
        /// </summary>
        private static int rowLast;
        /// <summary>
        /// Для рандомного часу, щоб влаштувати перегони
        /// </summary>
        private static Random rnd = new Random();

        static void Main()
        {
            // join unicode
            Console.OutputEncoding = Encoding.Unicode;

            // додаючи чорний колір, ми його виключимо із перебору
            //colorArray = new ConsoleColor[colorArray.Length];
            //counter = 0;
            colorArray[counter] = ConsoleColor.Black;

            // запуск рекурсивного методу
            RecursiveMethod();

            // repeat
            Console.ReadKey();
            //DoExitOrRepeat();
        }

        /// <summary>
        /// Щоб установити курсор в кінці виведеного консолі
        /// </summary>
        private static void StandInTheEnd()
        {
            Console.SetCursorPosition(0, Enum.GetValues(typeof(ConsoleColor)).Length - 1);
        }

        /// <summary>
        /// Рекурсивний метод
        /// </summary>
        /// <param name="colorArray">Масив вибраних кольорів</param>
        /// <param name="row">Рівень рядка</param>
        private static void RecursiveMethod()
        {
            // вибір кольору для певного рядка
            ConsoleColor color = ChangeColor.RandomColor(colorArray);
            // Створення змінної яка вказуватиме на рядок (+ збільшуємо лічильник для наступних кроків)
            int row = counter++;
            rowLast = Math.Max(rowLast, row);

            // занесення його в масив уже вибраних кольорів
            colorArray[counter] = color;
            // перевірка чи не повернено чорний колір, 
            // який вважатиметься сигнальним для закінчення
            // рекурсивного заглиблення
            if (ConsoleColor.Black != color)
            {
                // якщо колір відмінний від чорного, то запускаємо
                // цей метод рекурсивно в іншому потоці
                if (counter < Enum.GetValues(typeof(ConsoleColor)).Length - 1)
                {
                    new Thread(RecursiveMethod).Start();
                }
            }
            else
            {
                return;
            }

            // далі виконуємо якесь завдання
            for (int i = 0; i < iterration; i++)
            {
                lock (block)
                {
                    Console.SetCursorPosition(i, row);
                    Console.ForegroundColor = color;
                    Console.Write("*");
                    Thread.Sleep(rnd.Next(0, 7));
                    //Thread.Sleep(5);
                }
            }

            lock (block)
            {
                Console.ResetColor();
                StandInTheEnd();
            }
        }

        /// <summary>
        /// Метод виходу або повторення методу Main()
        /// </summary>
        static void DoExitOrRepeat()
        {
            Console.WriteLine("\n\nСпробувати ще раз: [т, н]");
            Console.Write("\t");
            var button = Console.ReadKey(true);

            if ((button.KeyChar.ToString().ToLower() == "т") ||
                (button.KeyChar.ToString().ToLower() == "n")) // можливо забули переключити розкладку клавіатури
            {
                Console.Clear();
                Main();
                // без використання рекурсії
                //Process.Start(Assembly.GetExecutingAssembly().Location);
                //Environment.Exit(0);
            }
            else
            {
                // закриває консоль
                Environment.Exit(0);
            }
        }
    }
}
