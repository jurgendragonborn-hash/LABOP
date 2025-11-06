using System;

enum ActionTypes
{
    Game =1,
    Author =2,
    Exit=3
}
static class Program
{
    static void MessageAboutError()
    {
        Console.WriteLine("Ошибка ввода данных!");
    }
    static ActionTypes Menu()
    {
        while (true)
        {
            Console.WriteLine("1. Отгадай ответ");
            Console.WriteLine("2. Об авторе");
            Console.WriteLine("3. Выход");
            Console.Write("Введите действие: ");
            if (!int.TryParse(Console.ReadLine(), out int action)) MessageAboutError();
            else
            {
                if (Enum.IsDefined<ActionTypes>((ActionTypes)action)) return (ActionTypes)action;
                else MessageAboutError();
            }
        }
    }
    static void AboutAuthor()
    {
        Console.WriteLine("Работу выполнил Галацков Артем из группы 6106-090301D");
        Console.Write("Нажмите любую клавишу, чтобы продолжить: ");
        Console.ReadKey();
        Console.Clear();
    }
    public static double MathFuncResult(double x, double y)
    {
        double leftPart = Math.Sin((Math.Pow(x, 3) + Math.Pow(y, 5)) / (2 * Math.PI));
        double cosPart = Math.Cos(x + y);
        double rightPart = Math.Pow(Math.Abs(cosPart), 1.0 / 3) * Math.Sign(cosPart);
        return leftPart + rightPart;
    }
    static void Game()
    {
        Console.Clear();
        double a=0, b=0, result=0;
        bool isAParamCorrect = false, isBParamCorrect = false;
        while(!isAParamCorrect)
        {
            Console.Write("Введите a: ");
            if(!double.TryParse(Console.ReadLine(), out double temp))
            {
                MessageAboutError();
            }
            else
            {
                a = temp;
                isAParamCorrect = true;
            }
        }

        while (!isBParamCorrect)
        {
            Console.Write("Введите b: ");
            if (!double.TryParse(Console.ReadLine(), out double temp))
            {
                MessageAboutError();
            }
            else
            {
                b = temp;
                isBParamCorrect = true;
            }
        }

        result = MathFuncResult(a,b);
        result = Math.Round(result, 2);
        bool isHaveCorrectAnswer = false;
        for(int i =0; i<3 && !isHaveCorrectAnswer; i++)
        {
            bool isParamCorrect = false;
            double answer = 0;
            while (!isParamCorrect)
            {
                Console.Write("Введите предполагаемый ответ: ");
                if (!double.TryParse(Console.ReadLine(), out double temp))
                {
                    MessageAboutError();
                }
                else
                {
                    answer = temp;
                    isParamCorrect = true;
                }
            }

            if (answer == result) isHaveCorrectAnswer = true;
            else Console.WriteLine($"Ответ неверный! Попыток {2-i}/3");
        }
        if (isHaveCorrectAnswer) Console.WriteLine("Ответ правильный! Вы победили!");
        else Console.WriteLine($"Ответ был = {result}");
    }
    static bool Exit()
    {
        string positive = "д";
        string negative = "н";
        while (true)
        {
            Console.Write("Вы уверены? (д/н): ");
            string result = Console.ReadLine();
            if (string.Equals(result, positive, StringComparison.OrdinalIgnoreCase)) return true;
            else if (string.Equals(result, negative, StringComparison.OrdinalIgnoreCase)) return false;
            else MessageAboutError();
        }
    }
    static void Main(string[] args)
    {
        bool isProgramWorking = true;
        while (isProgramWorking) 
        {
             ActionTypes action = Menu();
            if (action == ActionTypes.Exit) isProgramWorking = !Exit();
            else if (action == ActionTypes.Game) Game();
            else AboutAuthor();
        }
    }
}