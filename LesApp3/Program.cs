using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// Можна було б гратися із часом/затримкою/сном щоб пустити
// дві краплі в колонці, але можна зробити хитріше - просто 
// в певний момент видалити значення із колекції і воно стане
// в чергу на додавання

// також важливо перевірити як працюватиме програма при зміні
// розмірів консолі при розкритті на весь екран і звенренні назад

namespace LesApp3
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
        /// Набір даних
        /// </summary>
        private static readonly string matrix = "0123456789AEIOUYBCDFGHJKLMNPQRSTVWXZ@#$%&";
        /// <summary>
        /// Випадкові значення
        /// </summary>
        private static Random rnd = new Random();
        /// <summary>
        /// Блокування консолі
        /// </summary>
        public static readonly object blockConsole = new object();
        /// <summary>
        /// Блокування рандому, якщо не поставити блокування, 
        /// то через короткий час почнуть падати лише нулі і слова мінімального розміру
        /// </summary>
        public static readonly object blockRandom = new object();
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

            // безкінечний цикл із первіркою потоків
            while (true)
            {
                // перевіряємо чи не зайняті всі стовбці + обмежуємо їх кількість 
                if (list.Count < colM)
                {
                    // для економыъ ресурсів
                    var s = new StringBuilder();

                    // величина кодового виразу
                    lock (blockRandom)
                    {
                        int length = rnd.Next(3, 13);
                        for (int i = 0; i < length; i++)
                        {
                            s.Append(matrix[rnd.Next(0, matrix.Length)]);
                        }
                    }

                    // створення потоків і запуск
                    new Thread(() => RainWords(s.ToString(), ChangeValue.RandomValue(0, colM, ref list))).Start();
                    Thread.Sleep(90); // при 80 вже помітно, але з тормозінням
                    // адекватно працює при 100, коли затримати консоль при такій швидкості, 
                    // то все нормально вирівнюється і не тормозить
                }
                // оновлюємо розміри, згідно розмірів вікна
                if (UpdateSize())
                {
                    // якщо була зміна розміру
                    lock (blockConsole)
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

                lock (blockConsole)
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
                                // вимкнення курсора, якщо ставити вище, то непрацює
                                Console.CursorVisible = false;
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

                // зміна слова
                ChangeWord();

                // якщо дійшов до половини екрану, видаляємо із колекції,
                // що дасть змогу запстити ще один потік в цій колонці
                if (counter.Last().LastValue > rowM / 4)
                {
                    // блокуємо колекцію і видаляємо номер
                    lock (blockConsole)
                    {
                        // чистимо місце в списку
                        list.Remove(col);
                    }
                }


                // якщо дойшов до кінця і повністю сховався то вбиваємо потік
                if (counter.Last().LastValue > rowM)
                {
                    // зупиняємо потік
                    Thread.Sleep(1000);

                    // вбиваємо потік - що і буде варіантом виходу
                    Thread.CurrentThread.Abort();
                }
            }

            // налаштування величини лічильників слова
            void UpdateCounter()
            {
                for (int i = 0; i < counter.Length; i++)
                {
                    // установка початкових значень лічильника (-1 для рядків, бо уходить за межі і губиться одна буква)
                    counter[i].MaxSize = rowM + word.Length;
                }
            }

            // зміна літер в слові
            void ChangeWord()
            {
                var s = new StringBuilder();

                lock (blockRandom)
                {
                    for (int i = 0; i < word.Length - 1; i++)
                    {
                        s.Append(matrix[rnd.Next(0, matrix.Length)]);
                    }
                }

                word = s.Append(" ").ToString();
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
