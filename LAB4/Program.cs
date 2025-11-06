using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

enum ActionTypes
{
    TryToGuess = 1,
    Author = 2,
    Sorting = 3,
    Game=4,
    Exit = 5
}
struct Point
{
    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }
}

enum SnakeDirection
{
    Forward,
    Backward,
    Left,
    Right
}

enum SymbolIDs
{
    Wall = -1,
    Hollow=0,
    Snake=1,
    Food=2
}

public class SnakeGame
{
    public SnakeGame(int updateDelay) 
    {
        Delay = updateDelay;
    }


    public void StartGame()
    {
        Console.Clear();
        Console.CursorVisible = false;
        _food = new Point(_random.Next(1, 19), _random.Next(2, 19));
        _snakeHead = new Point(_random.Next(1, 19), 1);
        for(int i=0; i<_gameField.GetLength(0); i++)
        {
            for(int j=0; j<_gameField.GetLength(1); j++)
            {
                if (i == 0 || j == 0 || (i + 1) == _gameField.GetLength(0) || (j + 1) == _gameField.GetLength(1))
                {
                    Console.Write(EDGE_SYMBOL);
                    _gameField[i, j] = SymbolIDs.Wall;
                }
                else if (j == _food.X && i == _food.Y)
                {
                    Console.Write(HOLLOW_SYMBOL);
                    _poolEditing.Add(_food);
                    _gameField[i, j] = SymbolIDs.Food;
                }
                else if (j == _snakeHead.X && i == _snakeHead.Y)
                {
                    Console.Write(HOLLOW_SYMBOL);
                    _poolEditing.Add(_snakeHead);
                    _gameField[i, j] = SymbolIDs.Snake;
                }
                else
                {
                    _gameField[i, j] = SymbolIDs.Hollow;
                    Console.Write(HOLLOW_SYMBOL);
                }
            }
            Console.WriteLine();
        }
        Game();
    }

    private void Game()
    {
        Task task = null;
        task = Task.Run(() =>
        {
            while (_isAlive)
            {
                Update();
                task.Wait(Delay);
            }
            Console.WriteLine("\nКонец игры!");
            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
        });



        while (_isAlive)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.LeftArrow:
                    if (_direction != SnakeDirection.Right) _tempDirection = SnakeDirection.Left;
                    break;
                case ConsoleKey.RightArrow:
                    if (_direction != SnakeDirection.Left) _tempDirection = SnakeDirection.Right;
                    break;
                case ConsoleKey.UpArrow:
                    if (_direction != SnakeDirection.Backward) _tempDirection = SnakeDirection.Forward;
                    break;
                case ConsoleKey.DownArrow:
                    if (_direction != SnakeDirection.Forward) _tempDirection = SnakeDirection.Backward;
                    break;
            }
        }
        if (task.Status == TaskStatus.Running) task.Wait();
        Console.Clear();

    }
    private void Update()
    {
        _direction = _tempDirection;
        SnakeMove();
        foreach (Point point in _poolEditing)
        {
            SymbolIDs symbol = _gameField[point.Y, point.X];
            Console.SetCursorPosition(point.X, point.Y);
            switch(symbol) 
            {
                case SymbolIDs.Wall:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(EDGE_SYMBOL);
                    break;
                case SymbolIDs.Food:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(FOOD_SYMBOL);
                    break;
                case SymbolIDs.Snake:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(SNAKE_SYMBOL);
                    break;
                case SymbolIDs.Hollow:
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(HOLLOW_SYMBOL);
                    break;
            }
        }
        Console.SetCursorPosition(_gameField.GetLength(0) - 1, _gameField.GetLength(1) - 1);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"\nСобрано очков: {_snakeBody.Count}");
        _poolEditing.Clear();
    }

    private void SnakeBodyMove(bool isGetFood)
    {
        if(_snakeBody.Count==0) return;
        if(!isGetFood) _gameField[_snakeBody.Last().Y, _snakeBody.Last().X] = SymbolIDs.Hollow;
        _poolEditing.Add(_snakeBody.Last());
        Point[] copy = new Point[_snakeBody.Count];
        _snakeBody.CopyTo<Point>(copy);
        for (int i=1; i<copy.Length; i++)
        {
            _snakeBody[i] = copy[i - 1];

        }
        _snakeBody[0] = _snakeHead;
        _gameField[_snakeBody[0].Y, _snakeBody[0].X] = SymbolIDs.Snake;
    }

    

    private void GetFood()
    {
        Point SetPoint(Point point)
        {
            Point result = point;
            if (result.X < 18) result.X += 1;
            else result.X -= 1;

            return result;
        }

        Point point = (_snakeBody.Count == 0) ? SetPoint(_snakeHead) : SetPoint(_snakeBody.Last());

        _snakeBody.Add(point);
    }

    private void SetFood()
    {
        List<Point> freePoints = new List<Point>();
        for(int i=0; i<_gameField.GetLength(0); i++)
        {
            for(int j=0; j<_gameField.GetLength(1); j++)
            {
                if (_gameField[i, j] == SymbolIDs.Hollow && !(_gameField[i, j + 1] == SymbolIDs.Snake || _gameField[i, j - 1] == SymbolIDs.Snake)) freePoints.Add(new Point(j, i));
            }
        }

        Point randomPoint = freePoints[_random.Next(0, freePoints.Count)];
        _gameField[randomPoint.Y, randomPoint.X] = SymbolIDs.Food;
        _poolEditing.Add(randomPoint);
    }

    private void SnakeMove()
    {
        _gameField[_snakeHead.Y, _snakeHead.X] = SymbolIDs.Hollow;
        _poolEditing.Add(_snakeHead);
        int xTemp = _snakeHead.X;
        int yTemp=_snakeHead.Y;
        switch (_direction)
        {
            case SnakeDirection.Left:
                if (_snakeHead.X > 1) xTemp -= 1;
                else _isAlive = false;
                break;
            case SnakeDirection.Backward:
                if (_snakeHead.Y < 18) yTemp += 1;
                else _isAlive = false;
                break;
            case SnakeDirection.Right:
                if (_snakeHead.X < 18) xTemp += 1;
                else _isAlive = false;
                break;
            case SnakeDirection.Forward:
                if (_snakeHead.Y > 1) yTemp -= 1;
                else _isAlive = false;
                break;
        }

        if (_gameField[yTemp, xTemp] == SymbolIDs.Snake) _isAlive = false;
        

        if (!_isAlive)
        {
            _gameField[_snakeHead.Y, _snakeHead.X] = SymbolIDs.Snake;
            _poolEditing.Add(_snakeHead);
            return;
        }

        if (_gameField[yTemp, xTemp] == SymbolIDs.Food)
        {
            GetFood();
            SetFood();
            SnakeBodyMove(true);
        }
        else SnakeBodyMove(false);
        _snakeHead.X = xTemp;
        _snakeHead.Y = yTemp;

        _gameField[_snakeHead.Y, _snakeHead.X] = SymbolIDs.Snake;
        _poolEditing.Add(_snakeHead);
    }

    public void Reset()
    {
        _isAlive = true;
        _snakeBody.Clear();
        Console.CursorVisible = true;
        Console.ResetColor();
        _direction = SnakeDirection.Backward;
        _tempDirection = _direction;
        _poolEditing.Clear();
    }

    private Random _random = new Random();
    private List<Point> _poolEditing= new List<Point>();
    private SymbolIDs[,] _gameField = new SymbolIDs[20,20];
    private Point _food;
    private Point _snakeHead;
    private SnakeDirection _direction = SnakeDirection.Backward;
    private List<Point> _snakeBody = new List<Point>();
    const char EDGE_SYMBOL = '#';
    const char SNAKE_SYMBOL = '@';
    const char FOOD_SYMBOL = '*';
    const char HOLLOW_SYMBOL = ' ';
    private bool _isAlive = true;
    private SnakeDirection _tempDirection = SnakeDirection.Backward;
    public int Delay { get; set; }
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
                if (predicate == null || predicate(temp)) return temp;
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
            "4. Игра\n"+
            "5. Выход\n" +
            "Введите действие: ";
        return (ActionTypes)RequestInt(text, value => Enum.IsDefined<ActionTypes>((ActionTypes)value));
    }
    static void AboutAuthor()
    {
        Console.WriteLine("Работу выполнил Галацков Артем из группы 6106-090301D");
    }
    static void TryToGuessGame()
    {
        Console.Clear();
        double a = RequestDouble("Введите a: ");
        double b = RequestDouble("Введите b: ");

        double result = Math.Round(MathFuncResult(a, b), 2);
        TryToGuess(result);
    }
    static int RequestArrayLength() => RequestInt("Введите длину массива: ", value => value > 0);
    static int[] InitiazationArray(int length)
    {
        Random random = new Random();
        int[] result = new int[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = random.Next(0, 1000);
        }
        return result;
    }
    static T[] ArrayCopy<T>(T[] array)
    {
        T[] result = new T[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            result[i] = array[i];
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
        for (int i = 0; i < array.Length; i++)
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
            if (pointer == 0 || array[pointer].CompareTo(array[pointer - 1]) >= 0) pointer++;
            else
            {
                (array[pointer - 1], array[pointer]) = (array[pointer], array[pointer - 1]);
                pointer--;
            }
        }
    }
    static void ShellSort<T>(T[] array) where T : IComparable<T>
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
        if (ShowArray(firstArray)) ShowArray(secondArray);
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
        SnakeGame game = new SnakeGame(200);
        while (isProgramWorking)
        {
            ActionTypes action = Menu();
            switch (action)
            {
                case ActionTypes.TryToGuess:
                    TryToGuessGame();
                    break;
                case ActionTypes.Exit:
                    isProgramWorking = !Exit();
                    break;
                case ActionTypes.Sorting:
                    ArrayMenu();
                    break;
                case ActionTypes.Game:
                    game.StartGame();
                    game.Reset();
                    break;
                case ActionTypes.Author:
                    AboutAuthor();
                    break;

            }
        }
    }
}