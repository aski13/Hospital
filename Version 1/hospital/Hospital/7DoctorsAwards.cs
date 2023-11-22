using System;
using System.Drawing;
using System.Windows.Forms;

namespace Hospital
{
    public partial class _7DoctorsAwards : Form
    {

        #region Перетаскивание окна 

        private bool drag = false;
        private Point start_point = new Point(0, 0);

        #endregion

        #region public _7DoctorsAwards()
        public _7DoctorsAwards()
        {
            InitializeComponent();

            AddTooltip(label3, "Доктор закрывший большее кол-во приемов за месяц, получает прибавку к зарплате!");
        }
        #endregion

        #region Кнопки - закрыть приложение, Свернуть окно, закрыть окно
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
        #endregion

        #region подсказка
        private void AddTooltip(Control control, string tooltipText)
        {
            // создаем объект ToolTip и привязываем его к control
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(control, tooltipText);
        }
        #endregion

    }
}
