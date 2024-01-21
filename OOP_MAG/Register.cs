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
    public partial class Register : Form
    {
        private NpgsqlConnection conn;
        private string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=6360; Database=oop_app;";
        public Register()
        {
            InitializeComponent();
            conn = new NpgsqlConnection(connstring);
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                UserManager UM = new UserManager(conn);

                string email = EmailInsert.Text.Trim();
                string password = PasswordInsert.Text;
                string full_name = FullNameInsert.Text;
                string address = AddressInsert.Text;
                string phone_number = PhoneInsert.Text;

                // Проверка на пустые строки
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(full_name) || string.IsNullOrEmpty(phone_number) || string.IsNullOrEmpty(address))
                {
                    MessageBox.Show("Пожалуйста, проверьте правильность введённых данных.", "Неверно введенные данные", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (UM.DoesUseRegistered(email))
                {
                    MessageBox.Show("Пользователь с такой электронной почтой уже существует.", "Проверка пользователя", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO users (email, password, full_name, address, phone_number) VALUES (@email, @password, @full_name, @address, @phone_number)", conn))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@full_name", full_name);
                        command.Parameters.AddWithValue("@address", address);
                        command.Parameters.AddWithValue("@phone_number", phone_number);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            UserSession.SetUserEmail(email); 
                            MessageBox.Show("Регистрация успешна!", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MainPage mainPage = new MainPage();
                            mainPage.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка регистрации", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
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

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainPage mainPage = MainPage.GetInstance();
            mainPage.Show();
            this.Close();
        }
    }
}