using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB5
{
    /// <summary>
    /// Статический класс, хранящий методы для получения данных от пользователя, пока он не введет их корректно.
    /// </summary>
    public static class RequestHelper
    {
        /// <summary>
        /// Сообщение об ошибке, когда пользователь вводит некорректные данные.
        /// </summary>
        public static string ErrorMessage { get; set; } = "Ошибка ввода данных!";

        private static void MessageAboutError() => Console.WriteLine(ErrorMessage);
        /// <summary>
        /// Запрос у пользователя значения, конвертируемого в тип double.
        /// </summary>
        /// <param name="textRequest">Текст, высвечиваемый пользователю при запросе данных.</param>
        /// <param name="predicate">Дополнительное условие, помимо валидности данных. <br/>
        /// Если predicate == null, то дополнительной проверки не происходит.</param>
        /// <returns>Возвращает значение, введенное пользователем, просшедшее проверку валидации и доп. условие.</returns>
        public static double RequestDouble(string textRequest, Predicate<double> predicate = null)
        {
            while (true)
            {
                Console.Write(textRequest);
                if (!double.TryParse(Console.ReadLine(), out double temp))
                {
                    MessageAboutError();
                }
                else
                {
                    if (predicate == null || predicate(temp)) return temp;
                    MessageAboutError();
                }
            }
        }
        /// <summary>
        /// Запрос у пользователя значения, конвертируемого в тип int.
        /// </summary>
        /// <inheritdoc cref="RequestDouble(string, Predicate{double})"/>
        public static int RequestInt(string textRequest, Predicate<int> predicate = null)
        {
            while (true)
            {
                Console.Write(textRequest);
                if (!int.TryParse(Console.ReadLine(), out int temp))
                {
                    MessageAboutError();
                }
                else
                {
                    if (predicate == null || predicate(temp)) return temp;
                    MessageAboutError();
                }
            }
        }
        /// <summary>
        /// Запрос у пользователя строку для конвертации её в bool-значение.
        /// </summary>
        /// <param name="textRequest">Текст, высвечиваемый пользователю при запросе данных.</param>
        /// <param name="positive">Положительное значение.</param>
        /// <param name="negative">Отрицательное значение.</param>
        /// <param name="settings">Настройки сравнения строк.</param>
        /// <returns>Если текст == <b>positive</b>, то вернет <b>true</b>. <br/>
        /// Если текст == <b>negative</b>, то вернёт <b>false</b>.</returns>
        public static bool RequestBool(string textRequest, string positive, string negative,
            StringComparison settings = StringComparison.OrdinalIgnoreCase)
        {
            while (true)
            {
                Console.Write(textRequest);
                string temp = Console.ReadLine();
                if (temp.Equals(positive, settings)) return true;
                else if (temp.Equals(negative, settings)) return false;
                else MessageAboutError();
            }
        }
    }
}
