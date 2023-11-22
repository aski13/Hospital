using System;
using System.Data.SQLite;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hospital
{
    public partial class _6HistoryRecord : Form
    {
        private readonly string connectionString = "Data Source=BD3.db;Version=3;"; // + бд

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

        #region public _6HistoryRecord()
        public _6HistoryRecord()
        {
            InitializeComponent();
            ApplyWhiteBorder(); // белая граница у окна 
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
            Close();
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

        #region таблица
        private void _6HistoryRecord_Load(object sender, EventArgs e)
        {
            string query = "SELECT * FROM История";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable История = new DataTable();
                _ = adapter.Fill(История);

                // добавляем столбы для datagridview, для их отображения
                dataGridView1.DataSource = История;
                dataGridView1.Columns["ID"].Visible = false; // Прячем эту колонку
                dataGridView1.Columns["Тип_Записи"].HeaderText = "Тип записи";
                dataGridView1.Columns["ФИО_Пациента"].HeaderText = "ФИО Пациента";
                dataGridView1.Columns["Дата"].HeaderText = "Дата";
                dataGridView1.Columns["Цена"].HeaderText = "Цена";
                dataGridView1.Columns["Заключение"].HeaderText = "Заключение";
                dataGridView1.Columns["Врач"].HeaderText = "ФИО Врача";
            }
        }
        #endregion
    }
}
