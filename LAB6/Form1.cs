using System;
using System.Linq;
using System.Windows.Forms;

namespace LAB6
{
   
    public partial class MainForm : Form
    {
        private SnakeGame _snakeGame;
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        
        public MainForm()
        {
           
            InitializeComponent();
        }

        private void PressOnlyRealNumbers(object sender, KeyPressEventArgs e)
        {
            GeneralPressOnlyNumbers(sender, e, true);
        }
        private void PressOnlyPositiveInteger(object sender, KeyPressEventArgs e)
        {
            GeneralPressOnlyNumbers(sender, e, false, true);
        }
        private void PressOnlyInteger(object sender, KeyPressEventArgs e) => GeneralPressOnlyNumbers(sender, e);

        private void GeneralPressOnlyNumbers(object sender, KeyPressEventArgs e, bool isReal = false, bool isOnlyPositive=false)
        {
            char pressedKey = e.KeyChar;
            TextBox textBoxSender = sender as TextBox;
            bool[] allCondition = {
                pressedKey==1,
                ((textBoxSender.Text.Length == 0 || textBoxSender.SelectedText.Length==textBoxSender.Text.Length) && pressedKey == 45) && !isOnlyPositive,
                (pressedKey == 44 && textBoxSender.Text.IndexOf(pressedKey) == -1) && isReal,
                Char.IsDigit(pressedKey) || pressedKey == 8
            };
            if (allCondition.All(x => !x)) e.Handled = true;
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            _snakeGame = new SnakeGame();
            _snakeGame.Show();
            _snakeGame.FormClosed += _snakeGame_FormClosed;
            btnStartGame.Enabled = false;
        }

        private void _snakeGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            _snakeGame = null;
            btnStartGame.Enabled = true;
        }
    }
}
