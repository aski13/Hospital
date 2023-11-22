using System.Windows.Forms;
using System.Data.SQLite;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hospital
{
    public partial class _4ReceptionEntry : Form
    {
        private readonly string connString = @"Data Source=BD3.db;Version=3;"; // конект к бд

        #region передача ФИО Доктора через get и set 

        #endregion

        #region Get и Set для передачи данных с прошлого окна
        public int ID { get; set; }
        public string ТипЗаписи { get; set; }
        public string ФИОПациента { get; set; }
        public DateTime Дата { get; set; }
        public decimal Цена { get; set; }
        #endregion

        #region public _4ReceptionEntry()
        public _4ReceptionEntry()
        {
            InitializeComponent();


        }
        #endregion

        #region Заполнение текстбоксов данными
        private void _4ReceptionEntry_Load(object sender, EventArgs e)
        {
            textBox1.Text = ТипЗаписи;
            textBox2.Text = ФИОПациента;
            textBox3.Text = Дата.ToString();
            textBox4.Text = Цена.ToString();
        }
        #endregion

        #region Кнопки навигации
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

        #region Создание отчета/заключения в базу данных
        private void button3_Click(object sender, EventArgs e)
        {
            // Получаем логин и пароль из текстовых полей и удаляем пробелы в начале и конце строк
            string ФИО_Пациента = textBox2.Text;
            string Тип_Записи = textBox1.Text;
            string Дата = textBox3.Text;
            string Цена = textBox4.Text;
            string Заключение = textBox5.Text;
            string ФИО_Врача = textBox6.Text;

            // Проверяем, что поля логина и пароля не пустые
            if (ФИО_Пациента == "" || Тип_Записи == "" || Дата == "" || Цена == "" || Заключение == "" || ФИО_Врача == "")
            {
                _ = MessageBox.Show("Заполните все поля");
                return;
            }

            // Создаем новое подключение к базе данных SQLite
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                conn.Open();
                // Формируем запрос на удаление записи из таблицы "Приемы"
                string deleteQuery = $"DELETE FROM Приемы WHERE ID = {ID};";

                // Создаем новый объект команды SQL с запросом на удаление записи
                using (SQLiteCommand deleteCmd = new SQLiteCommand(deleteQuery, conn))
                {
                    // Выполняем запрос на удаление записи из таблицы "Приемы"
                    deleteCmd.ExecuteNonQuery();
                }
                // Формируем запрос на добавление нового пользователя в базу данных
                string insertQuery = $"INSERT INTO История (Тип_Записи , ФИО_Пациента, Дата, Цена, Заключение, Врач) VALUES ('{Тип_Записи}', '{ФИО_Пациента}', '{Дата}', '{Цена}', '{Заключение}' , '{ФИО_Врача}');";
                // Создаем новый объект команды SQL с запросом на добавление нового пользователя в базу данных
                using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn))
                {
                    // Выполняем запрос на добавление нового пользователя в базу данных
                    _ = insertCmd.ExecuteNonQuery();
                }
            }

            // Выводим сообщение об успешной регистрации пользователя
            _ = MessageBox.Show("Отчет успешно создан и занесен в базу данных");
        }
        #endregion

        #region Заполнение ФИО textbox6 данными доктора

        #endregion
    }
}
