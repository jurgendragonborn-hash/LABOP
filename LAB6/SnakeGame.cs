using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
namespace LAB6
{
    [Serializable]
    public class SnakeGameSettings
    {
        public int HollowColor { get; set; }
        public int SnakeColor { get; set; }
        public int FoodColor { get; set; }
        public int FieldHeight { get; set; }
        public int FieldWidth { get; set; }
        public int BlockHeight { get; set; }
        public int BlockWidth { get; set; }
        public int PastRecord { get; set; }
        public int UpdateDelay { get; set; }

    }
    public enum SnakeDirection
    {
        Forward,
        Backward,
        Left,
        Right
    }

    public partial class SnakeGame : Form
    {
        private readonly Timer _gameTimer;
        private SnakeGameSettings _settings;
        private readonly Random _random = new Random();
        private readonly PictureBox[,] _gameField;
        private readonly List<Point> _snake = new List<Point>();
        private SnakeDirection _direction = SnakeDirection.Backward;
        private bool _isAlive = true;
        private SnakeDirection _tempDirection = SnakeDirection.Backward;
        private Color HollowColor => Color.FromArgb(_settings.HollowColor);
        private Color SnakeColor => Color.FromArgb(_settings.SnakeColor);
        private Color FoodColor => Color.FromArgb(_settings.FoodColor);
        private readonly int _filledWidth, _filledHeight;
        private readonly Label _counterElement;
        private int _count = 0;
        private int Count
        {
            get => _count;
            set
            {
                _count = value;
                _counterElement.Text = "Ваш счёт: " + _count;
            }
        }
        public SnakeGame()
        {
            InitializeComponent();
            SetSettings();
            this.KeyDown += RequestKey;
            this.FormClosed += SnakeGame_FormClosed;
            _filledHeight = _settings.BlockHeight * _settings.FieldHeight;
            _filledWidth= _settings.BlockWidth * _settings.FieldWidth;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.DoubleBuffered = true;

            this.ClientSize = new Size(_filledWidth,_filledHeight + 100);

            _gameField = new PictureBox[_settings.FieldHeight, _settings.FieldWidth];
            _gameTimer = new Timer() { Interval = _settings.UpdateDelay };
            _gameTimer.Tick += Game_Tick;

            _counterElement = new Label();
            _counterElement.Text = "Ваш счёт: 0";
            _counterElement.Font = new Font("Arial", 12);
            _counterElement.Location = new Point(_filledWidth / 2 - _counterElement.Width / 2, _filledHeight + 50);
            _counterElement.AutoSize = true;

            this.Controls.Add(_counterElement);
            GenerateMap();
            SetupField();
            _gameTimer.Start();
        }

        private void SnakeGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            Serialization();
        }

        private void Serialization()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SnakeGameSettings));
            using(FileStream fs = new FileStream("settings.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, _settings);
            }
        }

        private void SetDefaultSettings()
        {
            _settings = new SnakeGameSettings() 
            {
                HollowColor = Color.White.ToArgb(),
                SnakeColor = Color.Green.ToArgb(),
                FoodColor = Color.Red.ToArgb(),
                FieldHeight = 20,
                FieldWidth = 20,
                BlockHeight = 30,
                BlockWidth = 30,
                PastRecord = 0,
                UpdateDelay = 200
            };
        }

        private void SetSettings()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SnakeGameSettings));
            if (!File.Exists("settings.xml"))
            {
                SetDefaultSettings();
                return;
            }
            using(FileStream fs = new FileStream("settings.xml", FileMode.Open))
            {
                _settings = xmlSerializer.Deserialize(fs) as SnakeGameSettings;
                if(_settings==null)
                {
                    SetDefaultSettings();
                }
            }
        }
        private void Game_Tick(object sender, EventArgs e)
        {
            SnakeMove();
        }

        private void GenerateMap()
        {
            for(int i =0; i<=_settings.FieldWidth; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = Color.Black;
                pictureBox.Location = new Point(0, _settings.BlockWidth * i);
                pictureBox.Size = new Size(_filledWidth, 1);
                this.Controls.Add(pictureBox);
            }
            for (int i = 0; i < _settings.FieldWidth; i++)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.BackColor = Color.Black;
                pictureBox.Location = new Point(_settings.BlockHeight*i, 0);
                pictureBox.Size = new Size(1, _filledHeight);
                this.Controls.Add(pictureBox);
            }
        }

        private void SetupField()
        {
            int snakeY = 0;
            int snakeX = _random.Next(0, _settings.FieldWidth);

            int foodY = 1;
            int foodX = _random.Next(0,_settings.FieldWidth);
            for(int i = 0; i<_gameField.GetLength(0); i++)
            {
                for(int j = 0; j<_gameField.GetLength(1); j++)
                {
                    PictureBox box = new PictureBox();
                    box.Location=new Point(j*_settings.BlockWidth, i*_settings.BlockHeight);
                    box.Size = new Size(_settings.BlockWidth, _settings.BlockHeight);

                    if(i == snakeY && j == snakeX)
                    {
                        _snake.Add(new Point(j, i));
                        box.BackColor = SnakeColor;
                    }
                    else if(i == foodY && j == foodX)
                    {
                        box.BackColor = FoodColor;
                    }
                    else
                    {
                        box.BackColor = HollowColor;
                    }

                    this.Controls.Add(box);
                    _gameField[i, j] = box;
                }
            }
        }

        private void SnakeMove()
        {
            if (!_isAlive) return;
            _direction = _tempDirection;
            Point snakeHead = _snake[0];
            int tempX = snakeHead.X, tempY = snakeHead.Y;
            switch(_direction)
            {
                case SnakeDirection.Left:
                    tempX -= 1;
                    break;
                case SnakeDirection.Right:
                    tempX += 1;
                    break;
                case SnakeDirection.Forward:
                    tempY -= 1;
                    break;
                case SnakeDirection.Backward:
                    tempY += 1;
                    break;
            }

            if(tempX<0 || tempX>=_settings.FieldWidth || tempY<0 || tempY>=_settings.FieldHeight || _gameField[tempY, tempX].BackColor==SnakeColor)
            {
                _isAlive = false;
                _gameTimer.Stop();
                
                if(Count>_settings.PastRecord)
                {
                    _counterElement.Text = $"Новый рекорд: {Count} / Прошлый рекорд: {_settings.PastRecord}";
                    _settings.PastRecord = Count;
                    _counterElement.ForeColor = Color.Green;
                }
                else
                {
                    _counterElement.Text = $"Игра окончилась на счёте: {Count} / Рекорд: {_settings.PastRecord}";
                    _counterElement.ForeColor = Color.Red;
                }
                _counterElement.Location = new Point(_filledWidth / 2 - _counterElement.Width / 2, _filledHeight + 50);
                return;
            }

            if (_gameField[tempY, tempX].BackColor == FoodColor)
            {
                GetFood();
                SetFood();
            }
            SnakeBodyMove();
            _gameField[tempY, tempX].BackColor = SnakeColor;
            _snake[0]=new Point(tempX, tempY);
        }

        private void SnakeBodyMove()
        {
            _gameField[_snake.First().Y, _snake.First().X].BackColor = HollowColor;
            if (_snake.Count < 2) return;
            _gameField[_snake.Last().Y, _snake.Last().X].BackColor = HollowColor;
            Point[] copy = new Point[_snake.Count];
            _snake.CopyTo(copy, 0);
            for (int i = 1; i < copy.Length; i++)
            {
                _snake[i] = copy[i - 1];

            }
            foreach (Point p in _snake)
            {
                _gameField[p.Y, p.X].BackColor = SnakeColor;
            }
        }

        private void GetFood()
        {
            Point SetPoint(Point point)
            {
                Point result = point;
                if (result.X < _settings.FieldWidth-1) result.X += 1;
                else result.X -= 1;
                return result;
            }

            Point p = SetPoint(_snake.Last()); 

            _snake.Add(p);
            Count++;
        }

        private void SetFood()
        {
            List<Point> freePoints = new List<Point>();
            for (int i = 0; i < _settings.FieldHeight - 1; i++)
            {
                for (int j = 0; j < _settings.FieldWidth - 1; j++)
                {
                    List<bool> bools = new List<bool>();
                    bools.Add(_gameField[i, j].BackColor ==HollowColor);
                    if (j > 0) bools.Add(_gameField[i, j - 1].BackColor != SnakeColor);
                    if (j < _settings.FieldWidth - 1) bools.Add(_gameField[i, j + 1].BackColor != SnakeColor);
                    if (bools.All(x=>x)) freePoints.Add(new Point(j, i));
                }
            }

            Point randomPoint = freePoints[_random.Next(0, freePoints.Count)];
            _gameField[randomPoint.Y, randomPoint.X].BackColor = FoodColor;
        }

        private void RequestKey(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Up:
                    if (_direction != SnakeDirection.Backward) _tempDirection = SnakeDirection.Forward;
                    break;
                case Keys.Down:
                    if (_direction != SnakeDirection.Forward) _tempDirection = SnakeDirection.Backward;
                    break;
                case Keys.Left:
                    if (_direction != SnakeDirection.Right) _tempDirection = SnakeDirection.Left;
                    break;
                case Keys.Right:
                    if (_direction != SnakeDirection.Left) _tempDirection = SnakeDirection.Right;
                    break;
            }
        }

    }
}
