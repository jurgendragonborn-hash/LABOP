using System;
using System.Diagnostics;

enum ActionTypes
{
    Game = 1,
    Author = 2,
    Sorting=3,
    Exit = 4
}
static class Program
{
    public static double MathFuncResult(double x, double y)
    {
        double leftPart = Math.Sin((Math.Pow(x, 3) + Math.Pow(y, 5)) / (2 * Math.PI));
        double cosPart = Math.Cos(x + y);
        double rightPart = Math.Pow(Math.Abs(cosPart), 1.0 / 3) * Math.Sign(cosPart);
        return leftPart + rightPart;
    }
    static double RequestDouble(string textRequest, Predicate<double> predicate = null)
    {
        while (true) 
        {
            Console.Write(textRequest);
            if(!double.TryParse(Console.ReadLine(), out double temp)) 
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
    static int RequestInt(string textRequest, Predicate<int> predicate = null) 
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
                if(predicate==null || predicate(temp)) return temp;
                MessageAboutError();
            }
        }
    }
    static void MessageAboutError()
    {
        Console.WriteLine("Ошибка ввода данных!");
    }
    static ActionTypes Menu()
    {
        string text = "1. Отгадай ответ\n" +
            "2. Об авторе\n" +
            "3. Сортировка массивов\n" +
            "4. Выход\n" +
            "Введите действие: ";
        return (ActionTypes)RequestInt(text, value => Enum.IsDefined<ActionTypes>((ActionTypes)value));
    }
    static void AboutAuthor()
    {
        Console.WriteLine("Работу выполнил Галацков Артем из группы 6106-090301D");
    }
    static void Game()
    {
        Console.Clear();
        double a = RequestDouble("Введите a: ");
        double b = RequestDouble("Введите b: ");

        double result = Math.Round(MathFuncResult(a, b),2);
        TryToGuess(result);
    }
    static int RequestArrayLength() => RequestInt("Введите длину массива: ", value => value > 0);
    static int[] InitiazationArray(int length)
    {
        Random random = new Random();
        int[] result = new int[length];
        for(int i = 0; i < length; i++)
        {
            result[i] = random.Next(0, 1000);
        }
        return result;
    }
    static T[] ArrayCopy<T>(T[] array)
    {
        T[] result = new T[array.Length];
        for(int i = 0; i < array.Length;i++)
        {
            result[i]=array[i];
        }
        return result;
    }
    static bool ShowArray<T>(T[] array)
    {
        if (array.Length > 10)
        {
            Console.WriteLine("Массивы не могут быть выведены на экран, так как длина массива больше 10.");
            return false;
        }
        Console.Write("[");
        for(int i = 0;i < array.Length;i++)
        {
            Console.Write(array[i]);
            if ((i + 1) < array.Length) Console.Write(", ");
            else Console.Write("]\n");
        }
        return true;
    }
    static void GnomieSort<T>(T[] array) where T : IComparable<T>
    {
        int pointer = 0;
        while ((pointer) < array.Length)
        {
            if (pointer==0 || array[pointer].CompareTo(array[pointer-1]) >=0) pointer++;
            else
            {
                (array[pointer-1], array[pointer]) = (array[pointer], array[pointer-1]);
                pointer--;
            }
        }
    }
    static void ShellSort<T>(T[] array) where T : IComparable<T>
    {
        int distance = array.Length / 2;
        while (distance >=1)
        {
            for(int i =0; i<array.Length;i++)
            {
                int j = i;
                while((j>=distance) && (array[j-distance].CompareTo(array[j])>=1))
                {
                    (array[j], array[j - distance]) = (array[j - distance], array[j]);
                    j -= distance;
                }
            }
            distance /= 2;
        }
    }    
    static void ArrayMenu()
    {
        Console.Clear();
        int length = RequestArrayLength();
        int[] firstArray = InitiazationArray(length);
        int[] secondArray = ArrayCopy<int>(firstArray);
        Stopwatch watcher = Stopwatch.StartNew();
        GnomieSort(firstArray);
        watcher.Stop();
        Console.WriteLine($"Гномья сортировка заняла {watcher.Elapsed.TotalMilliseconds} миллисекунд");
        watcher.Reset();
        watcher.Start();
        ShellSort(secondArray);
        watcher.Stop();
        Console.WriteLine($"Сортировка Шелла заняла {watcher.Elapsed.TotalMilliseconds} миллисекунд");
        if(ShowArray(firstArray)) ShowArray(secondArray);
    }
    static void TryToGuess(double correctAnswer)
    {
        bool isHaveCorrectAnswer = false;
        for (int i = 0; i < 3 && !isHaveCorrectAnswer; i++)
        {
            double answer = RequestDouble("Введите предполагаемый ответ: ");

            if (answer == correctAnswer) isHaveCorrectAnswer = true;
            else Console.WriteLine($"Ответ неверный! Попыток {2 - i}/3");
        }
        if (isHaveCorrectAnswer) Console.WriteLine("Ответ правильный! Вы победили!");
        else Console.WriteLine($"Ответ был = {correctAnswer}");
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
            switch(action)
            {
                case ActionTypes.Game:
                    Game();
                    break;
                case ActionTypes.Exit:
                    isProgramWorking = !Exit();
                    break;
                case ActionTypes.Sorting:
                    ArrayMenu();
                    break;
                case ActionTypes.Author:
                    AboutAuthor();
                    break;

            }
        }
    }
}