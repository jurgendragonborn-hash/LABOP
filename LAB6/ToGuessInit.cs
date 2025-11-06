using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB6
{
    public partial class MainForm
    {
        private void submitBtn_Click(object sender, EventArgs e)
        {
            if(double.TryParse(inputA.Text, out double first) && double.TryParse(inputB.Text, out double second) && 
                int.TryParse(enterCountTryes.Text, out int counts) && counts>0)
            {
                InitSecondPartTGG(new ToGuessGameHelper(first, second, counts));
                inputA.Text = inputB.Text = enterCountTryes.Text = "";
            }
            else
            {
                MessageBox.Show("Введите корректные данные!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void InitFirstPartTGG()
        {
            ToGuessGame.Controls.Clear();
            ToGuessGame.Controls.Add(formulaLabel);
            ToGuessGame.Controls.Add(inputA);
            ToGuessGame.Controls.Add(inputB);
            ToGuessGame.Controls.Add(submitBtn);
            ToGuessGame.Controls.Add(label3);
            ToGuessGame.Controls.Add(label4);
            ToGuessGame.Controls.Add(label2);
            ToGuessGame.Controls.Add(enterCountTryes);
        }
        private void InitSecondPartTGG(ToGuessGameHelper GameHelper)
        {
            ToGuessGame.Controls.Clear();
            const int BTN_WIDTH = 300;
            const int BTN_HEIGHT = 50;
            Button btn = new Button();
            btn.Text = "Подтвердить";
            btn.Size = new Size(BTN_WIDTH, BTN_HEIGHT);
            btn.Location = new Point((ToGuessGame.ClientSize.Width) / 2 - BTN_WIDTH, ToGuessGame.ClientSize.Height - BTN_HEIGHT);
            ToGuessGame.Controls.Add(btn);

            const int INPUT_WIDTH = 500;
            const int INPUT_HEIGHT = 30;
            TextBox inputAnswer = new TextBox();
            inputAnswer.Size = new Size(INPUT_WIDTH, BTN_HEIGHT);
            inputAnswer.Location = new Point((ToGuessGame.ClientSize.Width - INPUT_WIDTH) / 2, btn.Location.Y - INPUT_HEIGHT);
            inputAnswer.KeyPress += PressOnlyRealNumbers;
            ToGuessGame.Controls.Add(inputAnswer);

            ToGuessGame.Controls.Add(formulaLabel);

            Button btn2 = new Button();
            btn2.Text = "Вернуться назад";
            btn2.Size = new Size(BTN_WIDTH, BTN_HEIGHT);
            btn2.Location = new Point((ToGuessGame.ClientSize.Width) / 2, ToGuessGame.ClientSize.Height - BTN_HEIGHT);
            ToGuessGame.Controls.Add(btn2);
            btn2.Click += (sender, e) => InitFirstPartTGG();

            Label counter = new Label();
            counter.AutoSize = true;
            counter.Text = "Попытки: " + GameHelper.MaxCountOfTryes + "/" + GameHelper.MaxCountOfTryes;
            counter.Location = new Point(0, ToGuessGame.ClientSize.Height / 3 - counter.Height);
            ToGuessGame.Controls.Add(counter);

            Label values = new Label();
            values.AutoSize = true;
            values.Text = $"a = {GameHelper.X}\nb = {GameHelper.Y}";
            values.Location = new Point((ToGuessGame.ClientSize.Width - values.Width), ToGuessGame.ClientSize.Height / 3 - values.Height);
            ToGuessGame.Controls.Add(values);

            btn.Click += (sender, e) => {
                if(!double.TryParse(inputAnswer.Text, out double value))
                {
                    MessageBox.Show("Введите корректные данные!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (GameHelper.Try(value))
                {
                    MessageBox.Show("Вы угадали!");
                    InitFirstPartTGG();
                }
                else
                {
                    if (GameHelper.CountOfTryes == GameHelper.MaxCountOfTryes)
                    {
                        MessageBox.Show($"Вы не угадали. Ответ был {GameHelper.Result}", "Miss", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        InitFirstPartTGG();
                        return;
                    }
                    MessageBox.Show("Вы не угадали. Попробуйте ещё раз", "Miss", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    counter.Text = "Попытки: " + (GameHelper.MaxCountOfTryes - GameHelper.CountOfTryes) + "/" + GameHelper.MaxCountOfTryes;
                }
            };
        }
    }
}
