using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB5
{
    /// <summary>
    /// Структура, хранящая X Y координаты.
    /// </summary>
    public struct Point
    {
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum SnakeDirection
    {
        Forward,
        Backward,
        Left,
        Right
    }
    public enum SymbolIDs
    {
        Wall = -1,
        Hollow = 0,
        Snake = 1,
        Food = 2,
        Head=3
    }
    /// <summary>
    /// Класс с игрой "Змейка".
    /// </summary>
    public class SnakeGame
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateDelay">Задержка перед следующим кадром (в миллисекундах)</param>
        public SnakeGame(int updateDelay)
        {
            Delay = updateDelay;
        }
        /// <summary>
        /// Настраивает консоль для начала игры. Отрисовывает простейшее поле. В конце запускает саму игру.
        /// </summary>
        public void StartGame()
        {
            Console.Clear();
            Console.CursorVisible = false;
            _food = new Point(_random.Next(1, FIELD_HEIGHT-1), _random.Next(2, FIELD_WIDTH-1));
            _snakeHead = new Point(_random.Next(1, 19), 1);
            // Первичная отрисовка
            for (int i = 0; i < FIELD_HEIGHT; i++)
            {
                for (int j = 0; j < FIELD_WIDTH; j++)
                {
                    if (i == 0 || j == 0 || (i + 1) == FIELD_HEIGHT || (j + 1) == FIELD_WIDTH)
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
                        _gameField[i, j] = SymbolIDs.Head;
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

        /// <summary>
        /// Параллельно запускает метод отрисовки кадров с заданной в конструкторе задержкой. <br/>
        /// Синхронно запрашивает у пользователя нажатие клавиши. <br/>
        /// Метод завершается со смертью персонажа (Змейки).
        /// </summary>
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
        /// <summary>
        /// Отрисовывает кадр из пула объектов.
        /// </summary>
        private void Update()
        {
            _direction = _tempDirection;
            SnakeMove();
            foreach (Point point in _poolEditing)
            {
                SymbolIDs symbol = _gameField[point.Y, point.X];
                Console.SetCursorPosition(point.X, point.Y);
                switch (symbol)
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
                        Console.Write(SNAKE_BODY_SYMBOL);
                        break;
                    case SymbolIDs.Head:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(SNAKE_HEAD_SYMBOL);
                        break;
                    case SymbolIDs.Hollow:
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(HOLLOW_SYMBOL);
                        break;
                }
            }
            Console.SetCursorPosition(FIELD_WIDTH - 1, FIELD_HEIGHT - 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\nСобрано очков: {_snakeBody.Count}");
            _poolEditing.Clear();
        }

        private void SnakeBodyMove(bool isGetFood)
        {
            if (_snakeBody.Count == 0) return;
            if (!isGetFood) _gameField[_snakeBody.Last().Y, _snakeBody.Last().X] = SymbolIDs.Hollow;
            _poolEditing.Add(_snakeBody.Last());
            Point[] copy = new Point[_snakeBody.Count];
            _snakeBody.CopyTo<Point>(copy);
            for (int i = 1; i < copy.Length; i++)
            {
                _snakeBody[i] = copy[i - 1];

            }
            _snakeBody[0] = _snakeHead;
            _gameField[_snakeBody[0].Y, _snakeBody[0].X] = SymbolIDs.Snake;
        }

        private void GetFood()
        {
            static Point SetPoint(Point point)
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
            for (int i = 1; i < FIELD_HEIGHT-1; i++)
            {
                for (int j = 1; j < FIELD_WIDTH-1; j++)
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
            int yTemp = _snakeHead.Y;
            switch (_direction)
            {
                case SnakeDirection.Left:
                    xTemp -= 1;
                    break;
                case SnakeDirection.Backward:
                    yTemp += 1;
                    break;
                case SnakeDirection.Right:
                    xTemp += 1;
                    break;
                case SnakeDirection.Forward:
                    yTemp -= 1;
                    break;
            }

            if (_gameField[yTemp, xTemp] == SymbolIDs.Snake ||
                _gameField[yTemp, xTemp] == SymbolIDs.Wall) _isAlive = false;

            if (!_isAlive)
            {
                _gameField[_snakeHead.Y, _snakeHead.X] = SymbolIDs.Head;
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

            _gameField[_snakeHead.Y, _snakeHead.X] = SymbolIDs.Head;
            _poolEditing.Add(_snakeHead);
        }

        /// <summary>
        /// Сбрасывает состояние игры.
        /// </summary>
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

        private readonly Random _random = new Random();
        private readonly List<Point> _poolEditing = new List<Point>();
        private Point _food;
        private Point _snakeHead;
        private SnakeDirection _direction = SnakeDirection.Backward;
        private readonly List<Point> _snakeBody = new List<Point>();
        const char EDGE_SYMBOL = '#';
        const char SNAKE_BODY_SYMBOL = 'o';
        const char SNAKE_HEAD_SYMBOL = '@';
        const char FOOD_SYMBOL = '*';
        const char HOLLOW_SYMBOL = ' ';
        private bool _isAlive = true;
        private SnakeDirection _tempDirection = SnakeDirection.Backward;
        /// <summary>
        /// Задержка в мс между кадрами.
        /// </summary>
        public int Delay { get; set; }
        const int FIELD_HEIGHT = 20;
        const int FIELD_WIDTH = 20;
        private readonly SymbolIDs[,] _gameField = new SymbolIDs[FIELD_HEIGHT, FIELD_WIDTH];
    }
}
