using System;
using System.Diagnostics;

namespace LAB5
{
    /// <summary>
    /// Класс, содержащий статические методы с реализацией двух видов сортировок (гномьей и Шелла). <br/>
    /// Экземпляр классы хранит в себе массив и метод для замерения скорости сортировок для этого массива.
    /// </summary>
    public class ArrayHelper
    {
        /// <summary>
        /// GET свойство, возвращающее массив типа <b>int</b>.
        /// </summary>
        public int[] ArrayInt { get; }
        /// <summary>
        /// GET свойство, возвращающее длину массива <b>ArrayInt</b>.
        /// </summary>
        public int Length => ArrayInt.Length;
        /// <summary>
        /// Инициализирует массив класса и заполняет его.
        /// </summary>
        /// <param name="length">Длина массива класса.</param>
        public ArrayHelper(int length)
        {
            ArrayInt = new int[length];
            FillingArray();
        }
        /// <summary>
        /// Инициализирует массив класса и заполняет его.
        /// </summary>
        public ArrayHelper() : this(10) { }

        private void FillingArray()
        {
            Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                ArrayInt[i] = random.Next(0, 1000);
            }
        }
        /// <summary>
        /// Обобщенный метод для копирования содержимого массива <b>array</b>.
        /// </summary>
        /// <typeparam name="T">Тип массива.</typeparam>
        /// <param name="array">Массив, из которого будут копироваться данные.</param>
        /// <returns>Возвращает массива типа <typeparamref name="T"/>.</returns>
        public static T[] ArrayCopy<T>(T[] array)
        {
            T[] result = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i];
            }
            return result;
        }
        /// <summary>
        /// Метод для отображения массива. Длина массива не должна быть больше 10.
        /// </summary>
        /// <typeparam name="T">Тип массива</typeparam>
        /// <param name="array">Массив для отображения.</param>
        /// <returns>Возвращает true, если удалось отобразить массив, иначе false.</returns>
        public bool ShowArray<T>(T[] array)
        {
            if (array.Length > 10)
            {
                Console.WriteLine("Массивы не могут быть выведены на экран, так как длина массива больше 10.");
                return false;
            }
            Console.Write("[");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i]);
                if ((i + 1) < array.Length) Console.Write(", ");
                else Console.Write("]\n");
            }
            return true;
        }
        /// <summary>
        /// Реализация гномьей сортировки для массивов.
        /// </summary>
        /// <typeparam name="T">Тип массива.</typeparam>
        /// <param name="array">Массив, который надо отсортировать</param>
        public static void GnomieSort<T>(T[] array) where T : IComparable<T>
        {
            int pointer = 0;
            while ((pointer) < array.Length)
            {
                if (pointer == 0 || array[pointer].CompareTo(array[pointer - 1]) >= 0) pointer++;
                else
                {
                    (array[pointer - 1], array[pointer]) = (array[pointer], array[pointer - 1]);
                    pointer--;
                }
            }
        }
        /// <summary>
        /// Реализация сортировки Шелла
        /// </summary>
        /// <typeparam name="T">Тип массива.</typeparam>
        /// <param name="array">Массив, который надо отсортировать.</param>
        public static void ShellSort<T>(T[] array) where T : IComparable<T>
        {
            int distance = array.Length / 2;
            while (distance >= 1)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    int j = i;
                    while ((j >= distance) && (array[j - distance].CompareTo(array[j]) >= 1))
                    {
                        (array[j], array[j - distance]) = (array[j - distance], array[j]);
                        j -= distance;
                    }
                }
                distance /= 2;
            }
        }
        /// <summary>
        /// Метод, который замеряет скорость двух сортировок (гномьей и Шелла) и выводит их скорость выполнения на экран. <br/>
        /// В конце выводит на экран массив.
        /// </summary>
        public void ArrayMenu()
        {
            Console.Clear();
            FillingArray();
            int[] secondArray = ArrayCopy<int>(ArrayInt);
            Stopwatch watcher = Stopwatch.StartNew();
            GnomieSort(ArrayInt);
            watcher.Stop();
            Console.WriteLine($"Гномья сортировка заняла {watcher.Elapsed.TotalMilliseconds} миллисекунд");
            watcher.Reset();
            watcher.Start();
            ShellSort(secondArray);
            watcher.Stop();
            Console.WriteLine($"Сортировка Шелла заняла {watcher.Elapsed.TotalMilliseconds} миллисекунд");
            if (ShowArray(ArrayInt)) ShowArray(secondArray);
        }
    }
}
