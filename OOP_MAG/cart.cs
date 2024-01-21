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
    public partial class cart : Form
    {
        private NpgsqlConnection conn;
        private string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=6360; Database=oop_app;";
        public cart()
        {
            InitializeComponent();
            conn = new NpgsqlConnection(connstring);
            DisplayCartItems();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainPage mainPage = MainPage.GetInstance();
            mainPage.Show();
            this.Close();
        }
        private void DisplayCartItems()
        {
            try
            {
                conn.Open();

                // Получаем email пользователя из UserSession
                string userEmail = UserSession.Email;

                // Получаем userId по email
                int userId = GetUserIdByEmail(userEmail);

                // Запрос для получения товаров в корзине пользователя
                string query = "SELECT carts.cart_id, products.product_id, products.name AS Название, " +
                               "products.description AS Описание, products.price AS Цена " +
                               "FROM carts " +
                               "JOIN products ON carts.product_id = products.product_id " +
                               "WHERE carts.user_id = @userId";

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Очищаем DataGridView перед добавлением новых данных
                        dataGridView1.DataSource = null;

                        // Устанавливаем источник данных для DataGridView
                        dataGridView1.DataSource = dataTable;

                        // Добавляем кнопку "Удалить" в каждую строку DataGridView
                        DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                        deleteButton.HeaderText = "Удалить";
                        deleteButton.Text = "Удалить";
                        deleteButton.UseColumnTextForButtonValue = true;
                        dataGridView1.Columns.Add(deleteButton);

                        dataGridView1.Columns["cart_id"].HeaderText = "ID корзины";
                        dataGridView1.Columns["product_id"].HeaderText = "ID товара";
                        dataGridView1.Columns["Название"].HeaderText = "Название товара";
                        dataGridView1.Columns["Описание"].HeaderText = "Описание товара";
                        dataGridView1.Columns["Цена"].HeaderText = "Цена товара";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка отображения товаров в корзине", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["deleteButtonColumn"].Index && e.RowIndex >= 0)
            {
                // Получаем значение ID корзины из выделенной строки
                int cartId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["cart_id"].Value);

                // Вызываем метод удаления товара из корзины
                DeleteProductFromCart(cartId);

                // Обновляем отображение корзины после удаления
                DisplayCartItems();
            }
        }

        private void DeleteProductFromCart(int cartId)
        {
            try
            {
                conn.Open();
                string query = "DELETE FROM carts WHERE cart_id = @cartId";

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@cartId", cartId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка удаления товара из корзины", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private int GetProductIdFromRow(int rowIndex)
        {
            // Получаем значение в столбце "product_id" текущей строки
            return Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["product_id"].Value);
        }
   
        private int GetUserIdByEmail(string email)
        {
            int userId = -1;

            try
            {
                string query = "SELECT user_id FROM users WHERE email = @email";
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@email", email);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        userId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка получения ID пользователя", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return userId;
        }    
    }
}