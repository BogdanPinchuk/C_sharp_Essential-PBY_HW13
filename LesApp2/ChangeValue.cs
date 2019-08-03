using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesApp2
{
    /// <summary>
    /// Вибір випадкового значення
    /// </summary>
    class ChangeValue
    {
        /// <summary>
        /// для рандомного вибору
        /// </summary>
        private static readonly Random rnd = new Random();

        /// <summary>
        /// Вибір випадвого значення окрім уже вибраних
        /// </summary>
        /// <param name="begin">початкове значення діапазону</param>
        /// <param name="end">кінцеве значення діапазону</param>
        /// <param name="block">блокування доступу до списку</param>
        /// <param name="values">значення які були попередньо вибрані</param>
        /// <returns></returns>
        public static int RandomValue(int begin, int end, ref List<int> values)
        {
            // необхідно видалити копії значень, якщо наявні
            List<int> tempArray;

            lock (Program.block)
            {
                tempArray = values.Distinct().ToList();
            }

            // якщо кількість елементів в масиві попередньо вибраних значень
            // рівна (більше бути не може) максимальній кількості значень в перерахунку enum
            // виводимо повідомлення, що вибрати випадковий відмінний від наявних кольорів неможливо
            int countValues = Math.Abs(end - begin);

            if (tempArray.Count == countValues)
            {
                Console.WriteLine("\n\tВже всі значення вибрані і вибрати відмінний від навних неможливо.");
                return default;  // показних закінчення кольорів
            }

            // створення масиву із заповненими значеннями
            var dataArray = Enumerable.Range(begin, countValues).ToList();

            // щоб заекономити час і не перебирати велику кількість випадкових елементів
            // доки не попадеться відмінний від наявних, просто виведемо ті які лишилися
            var value = dataArray.Except(tempArray).ToList();

            // вибір номеру
            int index = rnd.Next(0, value.Count);

            // додаємо значення до масиву
            lock (Program.block)
            {
                values.Add(value[index]);
            }

            // повертаємо необхідне значення
            return values.Last();
        }
    }
}
