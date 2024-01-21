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
    public partial class Login : Form
    {  
        Register register = new Register();
        private NpgsqlConnection conn;
        private string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=6360; Database=oop_app;";
        public Login()
        {
            InitializeComponent();
            conn = new NpgsqlConnection(connstring);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                UserManager UM = new UserManager(conn);
                string email = EmailInsert.Text.Trim();
                string password = PasswordInsert.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Пожалуйста, введите электронную почту и пароль.", "Неверно введенные данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (UM.DoesUserExist(email, password))
                {
                    if ((email == "admin") && (password == "admin"))
                    {
                        this.Close();
                        admin admin = new admin();
                        admin.Show();
                    }
                    else
                    { 
                    UserSession.SetUserEmail(email);
                    MainPage mainPage = MainPage.GetInstance();
                    mainPage.Show();
                    this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Неверный email или пароль.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка ввода данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        private bool UserExists(string email, string password)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM users WHERE email = @email AND password = @password";
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", password);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка выполнения запроса", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void RegisterLabel_Click(object sender, EventArgs e)
        {
            register.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainPage mainPage = MainPage.GetInstance();
            mainPage.Show();
            this.Close();
        }
    }
}