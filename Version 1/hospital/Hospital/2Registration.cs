using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Hospital
{
    public partial class _2Registration : Form
    {
        private readonly string connString = @"Data Source=BD3.db;Version=3;"; // конект к бд

        #region Хранение Авторизированного пользователя

        #endregion

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

        #region  public _2Registration()
        public _2Registration()
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

        #region кнопки закрыть и свернуть
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

        #region скрыть/открыть пароль
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = true; // Отключаем скрытие пароля
            }
            else
            {
                textBox2.UseSystemPasswordChar = false; // Включаем скрытие пароля
            }
        }

        #endregion

        #region _2Registration_FormClosed
        private void _2Registration_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region кнопка сыллка на сайт
        private void button4_Click(object sender, EventArgs e)
        {
            // URL ссылки
            string url = "https://8gkb.ru/";

            // Открыть ссылку
            System.Diagnostics.Process.Start(url);
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

        #region регистрация в бд
        private void button3_Click(object sender, EventArgs e)
        {
            // Получаем логин и пароль из текстовых полей и удаляем пробелы в начале и конце строк
            string Имя = textBox1.Text.Trim();
            string Фамилия = textBox4.Text.Trim();
            string Отчество = textBox5.Text.Trim();
            string email = textBox3.Text.Trim();
            string password = textBox2.Text.Trim();

            // Проверяем, что поля логина и пароля не пустые
            if (Имя == "" || Фамилия == "" || Отчество == "" || password == "" || email == "")
            {
                _ = MessageBox.Show("Заполните все поля");
                return;
            }

            // Формируем запрос на проверку наличия пользователя с таким же логином в базе данных
            string checkQuery = $"SELECT COUNT(*) FROM Пользователи WHERE Имя='{Имя}' AND Фамилия='{Фамилия}' AND Отчество='{Отчество}' AND email='{email}'";

            // Создаем новое подключение к базе данных SQLite
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();

                using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                {
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    // Проверяем, что пользователь с таким логином уже не зарегистрирован в базе данных
                    if (count > 0)
                    {
                        _ = MessageBox.Show("Пользователь с таким логином или почтой уже зарегистрирован");
                        return;
                    }
                }

                // Формируем запрос на добавление нового пользователя в базу данных
                string insertQuery = $"INSERT INTO Пользователи (Имя, Фамилия, Отчество, email, Пароль, Тип_Доктора) VALUES ('{Имя}', '{Фамилия}', '{Отчество}', '{email}', '{password}', '1');";

                // Создаем новый объект команды SQL с запросом на добавление нового пользователя в базу данных
                using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn))
                {
                    // Выполняем запрос на добавление нового пользователя в базу данных
                    _ = insertCmd.ExecuteNonQuery();
                }
            }

            // Выводим сообщение об успешной регистрации пользователя
            _ = MessageBox.Show("Вы успешно зарегистрировались");
        }
        #endregion

        #region кнопка авториззации
        private void button5_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM Пользователи WHERE Имя=@Имя AND Фамилия=@Фамилия AND Отчество=@Отчество AND Email=@email AND Пароль=@password";

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    _ = command.Parameters.AddWithValue("@Имя", textBox10.Text);
                    _ = command.Parameters.AddWithValue("@Фамилия", textBox7.Text);
                    _ = command.Parameters.AddWithValue("@Отчество", textBox6.Text);    
                    _ = command.Parameters.AddWithValue("@email", textBox8.Text);
                    _ = command.Parameters.AddWithValue("@password", textBox9.Text);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string Тип_Доктора = reader.GetString(reader.GetOrdinal("Тип_Доктора"));
                            if (Тип_Доктора == "2")
                            {
                                // вход для админа
                                _3AdminPanel fMain = new _3AdminPanel();
                                fMain.Show();
                                fMain.FormClosed += new FormClosedEventHandler(_2Registration_FormClosed);
                                Hide();
                            }
                            else
                            {
                                // вход для обычных людей 
                                _5Record fMain = new _5Record();
                                fMain.Show();
                                fMain.FormClosed += new FormClosedEventHandler(_2Registration_FormClosed);
                                Hide();
                            }
                        }
                        else
                        {
                            //Неверный пароль
                            MessageBox.Show("Ваши данные не были найдены в системе. Пожалуйста, проверьте введенные вами данные на наличие ошибок. 🔍");
                        }
                    }
                }
            }
        }

        #endregion

        #region показать скрыть пароль у авторизации
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox9.UseSystemPasswordChar = true; // Отключаем скрытие пароля
            }
            else
            {
                textBox9.UseSystemPasswordChar = false; // Включаем скрытие пароля
            }
        }
        #endregion

    }
}