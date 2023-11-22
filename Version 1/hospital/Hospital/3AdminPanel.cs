using System;
using System.Data.SQLite;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hospital
{
    public partial class _3AdminPanel : Form
    {
        private readonly string connectionString = "Data Source=BD3.db;Version=3;";

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

        #region public _3AdminPanel()
        public _3AdminPanel()
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

        #region кнопки свернуть и закрыть
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
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

        private void panel5_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            start_point = new Point(e.X, e.Y);
        }

        private void panel5_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void panel5_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }
        #endregion

        #region Тень у окна
        private const int CS_DROPSHADOW = 0x20000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        #endregion

        #region Authorization_FormClosed
        private void _3AdminPanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Таблица с запросами на прием
        private void _3AdminPanel_Load(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Приемы";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable Приемы = new DataTable();
                _ = adapter.Fill(Приемы);

                // добавляем столбы для datagridview, для их отображения
                dataGridView1.DataSource = Приемы;
                dataGridView1.Columns["ID"].Visible = false; // Прячем эту колонку
                dataGridView1.Columns["Тип_Записи"].HeaderText = "Тип записи";
                dataGridView1.Columns["ФИО_Пациента"].HeaderText = "ФИО Пациента";
                dataGridView1.Columns["Дата"].HeaderText = "Дата";
                dataGridView1.Columns["Цена"].HeaderText = "Цена";
            }
        }
        #endregion

        #region написать заключениеи дабл кликом
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Получение данных выбранной строки
            DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
            string типЗаписи = Convert.ToString(selectedRow.Cells["Тип_Записи"].Value);
            string фиоПациента = Convert.ToString(selectedRow.Cells["ФИО_Пациента"].Value);
            DateTime дата = Convert.ToDateTime(selectedRow.Cells["Дата"].Value);
            decimal цена = Convert.ToDecimal(selectedRow.Cells["Цена"].Value);

            // Передача данных второй форме через свойства (get и set)
            _4ReceptionEntry Pipez = new _4ReceptionEntry();
            Pipez.ТипЗаписи = типЗаписи;
            Pipez.ФИОПациента = фиоПациента;
            Pipez.Дата = дата;
            Pipez.Цена = цена;

            Pipez.ID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

            // Отображение второй формы
            Pipez.Show();
        }
        #endregion

        #region написать заключение кнопкой
        private void button4_Click(object sender, EventArgs e)
        {
            // Проверка, что строка выбрана
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Получение данных выбранной строки
            DataGridViewRow selectedRow = dataGridView1.CurrentRow;
            string типЗаписи = Convert.ToString(selectedRow.Cells["Тип_Записи"].Value);
            string фиоПациента = Convert.ToString(selectedRow.Cells["ФИО_Пациента"].Value);
            DateTime дата = Convert.ToDateTime(selectedRow.Cells["Дата"].Value);
            decimal цена = Convert.ToDecimal(selectedRow.Cells["Цена"].Value);

            // Передача данных второй форме через свойства (get и set)
            _4ReceptionEntry Pipez = new _4ReceptionEntry();
            Pipez.ТипЗаписи = типЗаписи;
            Pipez.ФИОПациента = фиоПациента;
            Pipez.Дата = дата;
            Pipez.Цена = цена;

            Pipez.ID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

            // Отображение второй формы
            Pipez.Show();
        }
        #endregion

        #region кнопки - просмтр истории заключений, премии докторов, скидка пациентов
        private void button3_Click(object sender, EventArgs e)
        {
            _6HistoryRecord newForm = new _6HistoryRecord();
            newForm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _7DoctorsAwards newForm2 = new _7DoctorsAwards();
            newForm2.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _8PatientDiscount newForm3 = new _8PatientDiscount();
            newForm3.Show();
        }
        #endregion

    }
}
