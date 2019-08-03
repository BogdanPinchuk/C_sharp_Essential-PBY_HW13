using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// Просто для себе, зі зміною розміру і букв - LesApp2

namespace LesApp2
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
        ///  Масив даних для виведення цілими словами
        /// </summary>
        private static readonly string[] arrayI = new string[]
        {
            "Pinchuk",
            "Bohdan",
            "Yuriyovych"
        };
        /// <summary>
        /// Відсилака до фільму Матриця
        /// </summary>
        private static readonly string matrix = "Matrix";
        /// <summary>
        /// Випадкові значення
        /// </summary>
        private static Random rnd = new Random();
        /// <summary>
        /// Блокування консолі
        /// </summary>
        public static readonly object block = new object();
        /// <summary>
        /// створення колекції 
        /// </summary>
        private static List<int> list = new List<int>();

        static void Main()
        {
            // Заголовок
            Console.Title = "Matrix";

            // Join Unicode
            Console.OutputEncoding = Encoding.Unicode;

            // вимкнення курсора
            Console.CursorVisible = false;

            // безкінечний цикл із первіркою потоків
            while (true)
            {
                // перевіряємо чи не зайняті всі стовбці + обмежуємо їх кількість 
                if (list.Count < colM)
                {
                    // створення потоків і запуск
                    new Thread(() => RainWords(arrayI[rnd.Next(0, arrayI.Length)], ChangeValue.RandomValue(0, colM, ref list))).Start();
                    Thread.Sleep(100);
                }
                // оновлюємо розміри, згідно розмірів вікна
                if (UpdateSize())
                {
                    // якщо була зміна розміру
                    lock (block)
                    {
                        // очистка - убирає артефакти після зміни розмірів екрану
                        Console.Clear();
                    }
                }
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
            // для оптимізації + пропуск для очищення за собою
            word += " ";

            // створення лічильника для кожної букви слова
            Counter[] counter = new Counter[word.Length];

            // налаштування лічильників слова
            for (int i = 0; i < counter.Length; i++)
            {
                // установка початкових значень лічильника (-1 для рядків, бо уходить за межі і губиться одна буква)
                counter[i] = new Counter(rowM + word.Length, -i);
            }

            // безкінечний цикл, пускаємо по кругу слово згори в низ
            while (true)
            {
                // перевірка внутрішнього лічильника
                if (counter[0].MaxSize - word.Length != rowM)
                {
                    // оновлення лічильників
                    UpdateCounter();
                }

                lock (block)
                {
                    // пробіжка по всьому слову
                    for (int i = 0; i < word.Length; i++)
                    {
                        int j = counter[i].Iteration;
                        // запис символа певного кольору у відповідній позиції / -1 - щоб не глючило через повзунок зліва
                        if (0 < j && j < rowM - 1)
                        {
                            Console.ForegroundColor = counter[i].Color;
                            // якщо змінюється ширина вікна, то ловимо помилку і убиваємо процес
                            try
                            {
                                Console.SetCursorPosition(col, j);
                                Console.Write(word[i]);
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                // чистимо місце в списку (блокування зроблено вище)
                                list.Remove(col);

                                // вбиваємо потік - що і буде варіантом виходу
                                Thread.CurrentThread.Abort();
                            }
                        }
                    }
                }
                Thread.Sleep(75);

                // якщо дойшов до кінця і повністю сховався то вбиваємо потік
                if (counter.Last().LastValue > rowM)
                {
                    // блокуємо колекцію і видаляємо номер
                    lock (block)
                    {
                        // чистимо місце в списку
                        list.Remove(col);
                    }

                    // зупиняємо потік
                    Thread.Sleep(500);

                    // вбиваємо потік - що і буде варіантом виходу
                    Thread.CurrentThread.Abort();
                }


            }

            // налаштування величини тыл лічильників слова
            void UpdateCounter()
            {
                for (int i = 0; i < counter.Length; i++)
                {
                    // установка початкових значень лічильника (-1 для рядків, бо уходить за межі і губиться одна буква)
                    counter[i].MaxSize = rowM + word.Length;
                }
            }
        }

        /// <summary>
        /// Оновлення розмірів консолі
        /// </summary>
        private static bool UpdateSize()
        {
            // чи змінювалися розміри екрану
            bool change = false;

            // перевіряємо чи були зміни, якщо так,  то знаносимо їх і 
            // чистимо екран, що дозволить  убрати артефакти
            if (rowM != Console.WindowHeight || colM != Console.WindowWidth)
            {
                rowM = Console.WindowHeight;
                colM = Console.WindowWidth;
                change = true;
            }

            return change;
        }

    }
}
