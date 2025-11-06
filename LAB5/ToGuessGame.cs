using System;
namespace LAB5
{
    /// <summary>
    /// Статический класс, содержащий методы для игры в "Угадай число".
    /// </summary>
    public static class ToGuessGame
    {
        /// <summary>
        /// Запуск игры. <br/>
        /// Метод запрашивает параметры a и b, которые, в дальнейшем, используется для расчетов математической функции. <br/>
        /// Пользователю необходимо угадать результат этой функции (сама функция скрыта от него).
        /// </summary>
        public static void Play()
        {
            Console.Clear();
            double a = RequestHelper.RequestDouble("Введите a: ");
            double b = RequestHelper.RequestDouble("Введите b: ");

            double result = Math.Round(MathFuncResult(a, b), 2);
            TryToGuess(result);
        }
        static void TryToGuess(double correctAnswer)
        {
            bool isHaveCorrectAnswer = false;
            for (int i = 0; i < 3 && !isHaveCorrectAnswer; i++)
            {
                double answer = RequestHelper.RequestDouble("Введите предполагаемый ответ: ");

                if (answer == correctAnswer) isHaveCorrectAnswer = true;
                else Console.WriteLine($"Ответ неверный! Попыток {2 - i}/3");
            }
            if (isHaveCorrectAnswer) Console.WriteLine("Ответ правильный! Вы победили!");
            else Console.WriteLine($"Ответ был = {correctAnswer}");
        }
        public static double MathFuncResult(double x, double y)
        {
            double leftPart = Math.Sin((Math.Pow(x, 3) + Math.Pow(y, 5)) / (2 * Math.PI));
            double cosPart = Math.Cos(x + y);
            double rightPart = Math.Pow(Math.Abs(cosPart), 1.0 / 3) * Math.Sign(cosPart);
            return leftPart + rightPart;
        }
    }
}
