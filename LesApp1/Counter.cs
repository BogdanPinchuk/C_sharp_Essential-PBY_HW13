using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesApp1
{
    /// <summary>
    /// Циклічний лічильник
    /// </summary>
    class Counter
    {
        private int iteration = -1;

        /// <summary>
        /// Номер ітерації
        /// </summary>
        public int Iteration
        {
            get
            {
                if (iteration == MaxSize)
                {
                    iteration = 0;
                }
                else
                {
                    iteration++;
                }
                return iteration;
            }
            private set { iteration = value; }
        }
        /// <summary>
        /// Максимальна величиниа лічильника
        /// </summary>
        public int MaxSize { get; private set; }

        /// <summary>
        /// конструктор який встановлює мінімальний розмір
        /// </summary>
        /// <param name="maxSize">максимальний розмір лічильника</param>
        public Counter(int maxSize)
        {
            MaxSize = maxSize;
        }

        /// <summary>
        /// конструктор який становлює мінімальний розмір і початок ітератора
        /// </summary>
        /// <param name="maxSize">максимальний розмір лічильника</param>
        /// <param name="iteration">звідки починає стартувати, але потім ни заходить нижче нуля</param>
        public Counter(int maxSize, int iteration)
        {
            MaxSize = maxSize;
            Iteration = iteration;

            if (iteration == 0)
            {
                Color = ConsoleColor.White;
            }
            else if (iteration == -1)
            {
                Color = ConsoleColor.Green;
            }
            else
            {
                Color = ConsoleColor.DarkGreen;
            }
        }

        /// <summary>
        /// Колір букви
        /// </summary>
        public ConsoleColor Color { get; set; }
    }
}
