using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LAB6
{
    public partial class MainForm
    {
        private TableHelper _tableHelper;
        const int MAX_COLUMNS = 10;
        private void InitFirstPart()
        {
            this.ArrayHelper.Controls.Clear();
            this.ArrayHelper.Controls.Add(this.reqLength);
            this.ArrayHelper.Controls.Add(this.inputLength);
            this.ArrayHelper.Controls.Add(this.btnDefaultLength);
            this.ArrayHelper.Controls.Add(this.btnLengthArray);
            this.ArrayHelper.Controls.Add(this.btnRandomArray);
            _tableHelper = null;
        }
        private DataGridView CreateTable(int[] generatedArray)
        {
            const int COLUMN_WIDTH = 100;
            const int ROW_HEIGHT = 25;
            this.ArrayHelper.Controls.Clear();
            DataGridView table = TableHelper.InitDefaultTable();
            table.EditingControlShowing += Table_EditingControlShowing;
           
            int tempLength = generatedArray.Length;
            int countColumns = (tempLength < MAX_COLUMNS) ? tempLength : MAX_COLUMNS;
            for (int i = 0; i < countColumns; i++)
            {
                DataGridViewColumn column = new DataGridViewColumn();
                column.CellTemplate = new DataGridViewTextBoxCell();
                column.Width = COLUMN_WIDTH;
                table.Columns.Add(column);
            }
            while(tempLength>0)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.Height = ROW_HEIGHT;
                table.Rows.Add(row);
                tempLength -= MAX_COLUMNS;
            }
            _tableHelper = new TableHelper(table, generatedArray);
            return table;
        }
 

        private void Table_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= PressOnlyInteger;
            e.Control.KeyPress += PressOnlyInteger;
           
        }

        private Button[] AddControlButtons(DataGridView table)
        {

            Button btnSortingArray = new Button();
            Button btnGetMaxValue = new Button();
            Button btnGetMinValue = new Button();
            Button btnGetAVG = new Button();
            Button btnSelectMinAndMax = new Button();
            Button btnGetBack = new Button();

            const int BUTTON_HEIGHT = 50;
            const int BUTTON_WIDTH = 170;

            Button[] allButtons = { btnSortingArray, btnGetMaxValue, btnGetMinValue, btnGetAVG, btnSelectMinAndMax, btnGetBack };
            btnSortingArray.Size = btnGetMaxValue.Size = btnGetMinValue.Size
                = btnGetAVG.Size = btnSelectMinAndMax.Size = btnGetBack.Size = new Size(BUTTON_WIDTH, BUTTON_HEIGHT);

            btnSortingArray.Text = "Отсортировать";
            btnGetMaxValue.Text = "Вывести наибольшее значение";
            btnGetMinValue.Text = "Вывести минимальное значение";
            btnGetAVG.Text = "Вывести среднее арифметическое";
            btnSelectMinAndMax.Text = "Выделить мин и макс";
            btnGetBack.Text = "Вернуться назад";
            btnSortingArray.Click += (sender, e) =>
            {
                TableHelper.ShellSort(_tableHelper.TableArray);
                _tableHelper.Update();
            };
            btnGetMaxValue.Click += (sender, e) => MessageBox.Show(_tableHelper.TableArray.Max().ToString());
            btnGetMinValue.Click += (sender, e) => MessageBox.Show(_tableHelper.TableArray.Min().ToString());
            btnGetAVG.Click += (sender, e) => MessageBox.Show((((double)_tableHelper.TableArray.Sum()) / _tableHelper.TableArray.Length).ToString());
            btnSelectMinAndMax.Click += SelectMinMaxHandler;
            btnGetBack.Click += (sender,e)=> InitFirstPart();

            const int START_OFFSET = 10;
            for (int i = 0; i < allButtons.Length; i++)
            {
                allButtons[i].Location = new Point(START_OFFSET + i * BUTTON_WIDTH, ArrayHelper.ClientSize.Height - allButtons[i].Height);
            }
           
            return allButtons;
            void SelectMinMaxHandler(object sender, EventArgs e)
            {
                if (_tableHelper.IsMinMaxSelected)
                {
                    _tableHelper.UnselectMinMax();
                    btnSelectMinAndMax.Text = "Выделить мин и макс";
                }
                else 
                {
                    _tableHelper.SelectMinMax();
                    btnSelectMinAndMax.Text = "Снять выделение";
                }
            }
        }
        
        private void InitSecondPart(int length)
        {
            int[] array = new int[length];
            InitSecondPart(array);
        }
        private void InitSecondPart(int[] generatedArray)
        {
            ArrayHelper.Controls.Clear();
            DataGridView table = CreateTable(generatedArray); 
            this.ArrayHelper.Controls.Add(table);
            Button[] allButtons = AddControlButtons(table);
            this.ArrayHelper.Controls.AddRange(allButtons);
        }
        private void btnLengthArray_Click(object sender, EventArgs e)
        {
            string text = inputLength.Text;
            if(text.Length==0)
            {
                MessageBox.Show("Заполните все поля!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int.TryParse(text, out int length);
            if(length <= 0 )
            {
                MessageBox.Show("Длина массива должна быть больше нуля!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            InitSecondPart(length);
        }

        private void btnDefaultLength_Click(object sender, EventArgs e) => InitSecondPart(10);

        private void btnRandomArray_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            string text = inputLength.Text;
            if (text.Length == 0)
            {
                MessageBox.Show("Заполните все поля!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int.TryParse(text, out int length);
            if (length <= 0)
            {
                MessageBox.Show("Длина массива должна быть больше нуля!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int[] array = new int[length];
            TableHelper.FillingArray(array);
            InitSecondPart(array);
        }
    }
}
