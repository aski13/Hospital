using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SQLite;

namespace Hospital
{
    public partial class _5Record : Form
    {
        private readonly string connString = @"Data Source=BD3.db;Version=3;"; // конект к бд

        #region белые границы у окна
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const int GWL_STYLE = -16;
        private const int WS_BORDER = 0x00800000;

        private const uint SWP_FRAMECHANGED = 0x0020;
        #endregion

        #region Перетаскивание окна 
        // Перетаскивание окна 
        private bool drag = false;
        private Point start_point = new Point(0, 0);
        #endregion

        #region public _5Record()
        public _5Record()
        {
            InitializeComponent();

            ApplyWhiteBorder(); // белая граница у окна 

            // Заполнить комбобокс значениями
            comboBox1.Items.Add(new Item { Name = "стоматолог", Price = 5000 });
            comboBox1.Items.Add(new Item { Name = "терапевт", Price = 1000 });
            comboBox1.Items.Add(new Item { Name = "кардиолог", Price = 500 });
            comboBox1.Items.Add(new Item { Name = "невролог", Price = 1200 });
            comboBox1.Items.Add(new Item { Name = "окулист", Price = 1500 });
            comboBox1.Items.Add(new Item { Name = "эндокринолог", Price = 2000 });
            comboBox1.Items.Add(new Item { Name = "психолог", Price = 2500 });


        }
        #endregion

        #region белые границы
        private void ApplyWhiteBorder()
        {
            // Получить дескриптор окна формы
            IntPtr handle = this.Handle;

            // Получить текущий стиль окна
            int currentStyle = GetWindowLong(handle, GWL_STYLE);

            // Установить стиль окна с белыми границами
            SetWindowLong(handle, GWL_STYLE, currentStyle | WS_BORDER);

            // Обновить внешний вид окна
            SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 0x0001 | 0x0002 | 0x0020);
        }
        #endregion

        #region кнопки свернуть, закрыть и назад
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            _2Registration fMain = new _2Registration();
            fMain.Show();
            fMain.FormClosed += new FormClosedEventHandler(_5Record_FormClosed);
            Hide();
        }
        #endregion

        #region _5Record_FormClosed
        private void _5Record_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region перетаскивание за шкирку
        private void label4_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            start_point = new Point(e.X, e.Y);
        }

        private void label4_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void label4_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            start_point = new Point(e.X, e.Y);
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            start_point = new Point(e.X, e.Y);
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }
        #endregion

        #region кнопки записаться, просмотр записей, отмена записи
        private void button5_Click(object sender, EventArgs e)
        {
            _6HistoryRecord fMain = new _6HistoryRecord();
            fMain.FormClosed += _5Record_FormClosed;
            fMain.Show();
        }
        #endregion

        #region цены для типа записей
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получить выбранный объект из комбобокса
            Item selectedItem = (Item)comboBox1.SelectedItem;

            // Отобразить цену в текстбоксе
            textBox1.Text = selectedItem.Price.ToString();
        }


        #endregion

        #region отмена записи
        private void button6_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Кнопка создания записи приема в базу данных
        private void button3_Click(object sender, EventArgs e)
        {
            string Имя = textBox3.Text;
            string Фамилия = textBox4.Text;
            string Отчество = textBox5.Text;
            string fio = $"{Фамилия} {Имя} {Отчество}";
            string Тип_Записи = comboBox1.SelectedItem.ToString();
            DateTime Дата = dateTimePicker1.Value;
            decimal Цена = decimal.Parse(textBox1.Text);

            string insertQuery = "INSERT INTO Приемы (ФИО_Пациента, Тип_Записи, Дата, Цена) VALUES (@ФИО_Пациента, @Тип_Записи, @Дата, @Цена)";

            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, conn))
                {
                    command.Parameters.AddWithValue("@ФИО_Пациента", fio);
                    command.Parameters.AddWithValue("@Тип_Записи", Тип_Записи);
                    command.Parameters.AddWithValue("@Дата", Дата);
                    command.Parameters.AddWithValue("@Цена", Цена);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Запись сохранена!");
        }
        #endregion
    }
}
