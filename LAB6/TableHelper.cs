using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace LAB6
{
    public class TableHelper
    {
        public DataGridView Table { get; }

        public static DataGridView InitDefaultTable()
        {
            DataGridView table = new DataGridView();
            table.AutoSize = true;
            table.MaximumSize = new Size(1003, 253);
            table.ColumnHeadersVisible = false;
            table.AllowUserToResizeColumns = false;
            table.AllowUserToResizeRows = false;
            table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            table.RowHeadersVisible = false;
            table.AllowUserToAddRows = false;

            return table;
        }
        public int[] TableArray { get; }
        private List<int> SelectedElements { get; } = new List<int>();
        private readonly Color MaxColor = Color.Red;
        private readonly Color MinColor = Color.Green;
        public bool IsMinMaxSelected => SelectedElements.Count > 0;
        public TableHelper(DataGridView table, int[] tableArray)
        {
            Table = table;
            TableArray = tableArray;
            Table.CellEndEdit += Table_CellEndEdit;
            Table.DataError += Table_DataError;
            Init();
        }

        private void Table_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string exceptionName = e.Exception.GetType().Name;
            if (exceptionName != typeof(FormatException).Name)
            {
                e.ThrowException = true;
                return;
            }
            MessageBox.Show("Введите корректное значение!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void SelectMinMax()
        {
            if (IsMinMaxSelected) return;
            SelectedElements.Clear();
            int minValue = this.TableArray.Min();
            int maxValue = this.TableArray.Max();

            int minIndex = Array.IndexOf<int>(this.TableArray, minValue);
            int maxIndex = Array.LastIndexOf<int>(this.TableArray, maxValue);

            var minIndexes = GetTabledIndex(minIndex);
            var maxIndexes = GetTabledIndex(maxIndex);

            SelectedElements.Add(minIndex);
            SelectedElements.Add(maxIndex);
            Table[minIndexes.columnIndex, minIndexes.rowIndex].Style.BackColor = MinColor;
            Table[maxIndexes.columnIndex, maxIndexes.rowIndex].Style.BackColor = MaxColor;
            
        }

        public void UnselectMinMax()
        {
            if (!IsMinMaxSelected) return;
            var minIndexes = GetTabledIndex(SelectedElements[0]);
            var maxIndexes = GetTabledIndex(SelectedElements[1]);

            Table[minIndexes.columnIndex, minIndexes.rowIndex].Style.BackColor = Color.White;
            Table[maxIndexes.columnIndex, maxIndexes.rowIndex].Style.BackColor = Color.White;

            SelectedElements.Clear();
        }

        public (int columnIndex, int rowIndex) GetTabledIndex(int index)
        {
            int rowIndex = index % this.Table.RowCount;
            int columnIndex = (int)Math.Floor((double)index / this.Table.RowCount);
            return (columnIndex, rowIndex);
        } 

        private void Table_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int index = Table.RowCount * e.ColumnIndex + e.RowIndex;
            string strValue = Table[e.ColumnIndex, e.RowIndex].Value.ToString();
            int value;
            if(strValue== String.Empty)
            {
                Table[e.ColumnIndex, e.RowIndex].Value = TableArray[index];
                return;
            }
            else value= (int)Table[e.ColumnIndex, e.RowIndex].Value;
            TableArray[index] = value;
            if (IsMinMaxSelected)
            {
                int min = TableArray.Min();
                int max = TableArray.Max();
                var pastTabledMinIndex = GetTabledIndex(SelectedElements[0]);
                var pastTabledMaxIndex = GetTabledIndex(SelectedElements[1]);
                if (index == SelectedElements[0])
                {
                    if(value>min)
                    {
                        Table[pastTabledMinIndex.columnIndex, pastTabledMinIndex.rowIndex].Style.BackColor = Color.White;

                        int newMinIndex = Array.IndexOf(TableArray, min);
                        var newMinIndexTabled = GetTabledIndex(newMinIndex);
                        Table[newMinIndexTabled.columnIndex, newMinIndexTabled.rowIndex].Style.BackColor = MinColor;

                        SelectedElements[0] = newMinIndex;
                    }
                }
                else if(index == SelectedElements[1])
                {
                    if(value<max)
                    {
                        Table[pastTabledMaxIndex.columnIndex, pastTabledMaxIndex.rowIndex].Style.BackColor = Color.White;

                        int newMaxIndex = Array.IndexOf(TableArray, max);
                        var newMaxIndexTabled = GetTabledIndex(newMaxIndex);
                        Table[newMaxIndexTabled.columnIndex, newMaxIndexTabled.rowIndex].Style.BackColor = MaxColor;

                        SelectedElements[1] = newMaxIndex;
                    }
                }
                if (max == value)
                {
                    Table[pastTabledMaxIndex.columnIndex, pastTabledMaxIndex.rowIndex].Style.BackColor = Color.White;

                    Table[e.ColumnIndex, e.RowIndex].Style.BackColor = MaxColor;
                    SelectedElements[1] = index;
                }
                else if (min == value)
                {
                    Table[pastTabledMinIndex.columnIndex, pastTabledMinIndex.rowIndex].Style.BackColor = Color.White;
                    Table[e.ColumnIndex, e.RowIndex].Style.BackColor = MinColor;
                    SelectedElements[0] = index;
                }
            }
            
        }

        /// <summary>
        /// Заполнение массива случайными элементами от -1000 до 1000
        /// </summary>
        /// <param name="array">Массив, который надо заполнить случайными элементами</param>
        public static void FillingArray(int[] array)
        {
            Random random = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(-1000, 1001);
            }
        }
        /// <summary>
        /// Реализация сортировки Шелла
        /// </summary>
        /// <typeparam name="T">Тип массива.</typeparam>
        /// <param name="array">Массив, который надо отсортировать.</param>
        public static void ShellSort<T>(T[] array) where T : IComparable<T>
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

        /// <summary>
        /// Обобщенный метод для копирования содержимого массива <b>array</b>.
        /// </summary>
        /// <typeparam name="T">Тип массива.</typeparam>
        /// <param name="array">Массив, из которого будут копироваться данные.</param>
        /// <returns>Возвращает массива типа <typeparamref name="T"/>.</returns>
        public static T[] ArrayCopy<T>(T[] array)
        {
            T[] result = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i];
            }
            return result;
        }
        public void Update()
        {
            int minIndex = Array.IndexOf(TableArray, TableArray.Min());
            int maxIndex = Array.LastIndexOf(TableArray, TableArray.Max());
            for (int i = 0, k = 0; i < Table.ColumnCount; i++)
            {
                for (int j = 0; j < Table.RowCount; j++, k++)
                {
                    if (k < TableArray.Length)
                    {
                        if(IsMinMaxSelected)
                        {
                            if (k==maxIndex)
                            {
                                Table[i, j].Style.BackColor = MaxColor;
                                SelectedElements[1] = k;
                            }
                            else if (k==minIndex)
                            {
                                Table[i, j].Style.BackColor = MinColor;
                                SelectedElements[0] = k;
                            }
                            else
                            {
                                Table[i, j].Style.BackColor = Color.White;
                            }
                        }
                        Table[i, j].ValueType = typeof(int);
                        Table[i, j].Value = TableArray[k];
                        continue;
                    }
                    Table[i, j].ValueType = typeof(string);
                    Table[i, j].Value = "-";
                    Table[i, j].ReadOnly = true;
                }
            }
        }

        private void Init()
        {
            for (int i = 0, k = 0; i < Table.ColumnCount; i++)
            {
                bool isHaveValue = false;
                for (int j = 0; j < Table.RowCount; j++, k++)
                {
                    if (k < TableArray.Length)
                    {
                        Table[i, j].ValueType = typeof(int);
                        Table[i, j].Value = TableArray[k];
                        isHaveValue = true;
                        continue;
                    }
                    Table[i, j].ValueType = typeof(string);
                    Table[i, j].Value = "-";
                    Table[i, j].ReadOnly = true;
                }
                if(!isHaveValue)
                {
                    Table.Columns[i].Visible = false;
                }
            }
        }


    }
}

