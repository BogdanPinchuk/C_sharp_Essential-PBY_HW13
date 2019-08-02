using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesApp0
{
    /// <summary>
    /// Вибір випадкового кольору
    /// </summary>
    public class ChangeColor
    {
        // для рандомного вибору кольору
        private static Random rnd = new Random();

        /// <summary>
        /// Вибір випадвого кольору окрім уже вибраних
        /// статичний метод дозволяє без ініціалізації класу його використовувати
        /// </summary>
        /// <param name="colors">кольори які були попередньо вибрані</param>
        /// <returns></returns>
        public static ConsoleColor RandomColor(params ConsoleColor[] colors)
        {
            // необхідно видалити копії кольорів, якщо наявні
            var tempColor = colors.Distinct().ToArray();

            // якщо кількість елементів в масиві попередньо вибраних кольорів
            // рівна (більше бути не може) максимальній кількості кольорів в перерахунку enum
            // виводимо повідомлення, що вибрати випадковий відмінний від наявних кольорів неможливо
            // http://qaru.site/questions/52601/getting-the-max-value-of-an-enum
            int maxOfConsoleColor = Enum.GetValues(typeof(ConsoleColor)).Length;

            if (tempColor.Length == maxOfConsoleColor)
            {
                Console.WriteLine("\n\tВже всі кольори вибрані і вибрати відмінний від навних неможливо.");
                return ConsoleColor.Black;  // показних закінчення кольорів
            }

            // щоб заекономити час і не перебирати велику кількість випадкових елементів
            // доки не попадатьсся відмінний від наявних, просто виведемо ті які лишилися
            var remainder = Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>().Except(tempColor).ToArray();

            // вибір номеру
            int color = rnd.Next(0, remainder.Length);

            // повертаэмо необхідний колір
            return remainder[color];
        }

    }
}
