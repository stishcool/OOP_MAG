using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace OOP_MAG
{
    public partial class Account : Form
    {
        private NpgsqlConnection conn;
        private string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=6360; Database=oop_app;";

        public Account()
        {
            InitializeComponent();
            conn = new NpgsqlConnection(connstring);
            DisplayUserData();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainPage mainPage = MainPage.GetInstance();
            mainPage.Show();
            this.Close();
        }
        private void UpdateUserData()
        {
            try
            {
               
                using (NpgsqlConnection connection = new NpgsqlConnection(connstring))
                {
                    connection.Open();

                    // Получаем email пользователя из UserSession
                    string userEmail = UserSession.Email;

                    // Запрос для обновления данных пользователя в базе данных
                    string query = "UPDATE users SET full_name = @fullName, phone_number = @phone, password = @password, address = @address WHERE email = @userEmail";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        // Параметры для обновления данных
                        command.Parameters.AddWithValue("@fullName", FullNameInsert.Text);
                        command.Parameters.AddWithValue("@phone", PhoneInsert.Text);
                        command.Parameters.AddWithValue("@password", PasswordInsert.Text);
                        command.Parameters.AddWithValue("@address", AddressInsert.Text);
                        command.Parameters.AddWithValue("@userEmail", userEmail);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Данные пользователя успешно обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // После успешного обновления данных, обновляем отображение на форме
                            DisplayUserData();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить данные пользователя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка обновления данных пользователя", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            // Вызываем метод для обновления данных пользователя
            UpdateUserData();
        }

        private void DisplayUserData()
        {
            try
            {
                conn.Open();

                // Получаем email пользователя из UserSession
                string userEmail = UserSession.Email;

                // Запрос для получения данных пользователя по email
                string query = "SELECT email, full_name, phone_number, password, address FROM users WHERE email = @userEmail";

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@userEmail", userEmail);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EmailInsert.Text = reader["email"].ToString();
                            FullNameInsert.Text = reader["full_name"].ToString();
                            PhoneInsert.Text = reader["phone_number"].ToString();
                            PasswordInsert.Text = reader["password"].ToString();
                            AddressInsert.Text = reader["address"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка отображения данных пользователя", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

    }
}
