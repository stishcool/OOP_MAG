using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace OOP_MAG
{
    public partial class MainPage : Form,Time
    {
        private static MainPage _instance;

        private NpgsqlConnection conn;
        private string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=6360; Database=oop_app;";

        private void OpenLoginFormButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserSession.Email))
            {
                // Если пользователь не вошел в систему, открываем форму входа
                this.Hide();
                Login login = new Login();
                login.Show();
            }
            else
            {
                MessageBox.Show("Вы уже вошли в аккаунт");
                OpenLoginFormButton.Visible = false;
                OpenLoginFormButton.Enabled = false;
                accountButton.Visible = true; 
                accountButton.Enabled = true;
                crt.Visible = true;
                crt.Enabled = true;
            }
        }

        public MainPage()
        {
            InitializeComponent();
            conn = new NpgsqlConnection(connstring);
            timeTimer.Interval = 1000; // устанавливаем интервал в 1 секунду
            timeTimer.Tick += TimeTimer_Tick;
            timeTimer.Start(); // запускаем таймер
        }
        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            // Обработчик таймера, вызывается каждую секунду
            DisplayTime(DateTime.Now);
        }
        private Timer timeTimer = new Timer();
        public static MainPage GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MainPage();
            }
            return _instance;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            PC pc = new PC();
            pc.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            PC pc = new PC();
            pc.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LAPTOP laptop = new LAPTOP();
            laptop.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LAPTOP laptop = new LAPTOP();
            laptop.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            CP cp = new CP();
            cp.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            CP cp = new CP();
            cp.Show();
        }

        private void accountButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Account account = new Account();
            account.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Hide();
            LAPTOP laptop = new LAPTOP();
            laptop.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Hide();
            PC pc = new PC();
            pc.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Hide();
            CP cp = new CP();
            cp.Show();
        }

        private void crt_Click(object sender, EventArgs e)
        {
            this.Hide();
            cart cart = new cart();
            cart.Show();
        }
        public void DisplayTime(DateTime time)
        {
            label9.Text = time.ToString();
        }
    }

}
