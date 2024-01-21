using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace OOP_MAG
{
    public class ShoppingCartManager
    {
        private NpgsqlConnection conn;

        public ShoppingCartManager(NpgsqlConnection connection)
        {
            conn = connection;
        }

        public void AddProductToCart(string productName)
        {
            try
            {
                conn.Open();
                string userEmail = UserSession.Email;
                int userId = GetUserIdByEmail(userEmail);
                int productId = GetProductIdByName(productName);

                if (productId != -1) 
                {
                    string query = "INSERT INTO Carts (user_id, product_id) VALUES (@userId, @productId)";
                    using (NpgsqlCommand insertCommand = new NpgsqlCommand(query, conn))
                    {
                        insertCommand.Parameters.AddWithValue("@userId", userId);
                        insertCommand.Parameters.AddWithValue("@productId", productId);

                        insertCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Товар успешно добавлен в корзину", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Товар с таким названием не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка добавления товара в корзину", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
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

        private int GetProductIdByName(string productName)
        {
            int productId = -1;

            try
            {
                string query = "SELECT product_id FROM products WHERE name = @productName";
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@productName", productName);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        productId = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка получения ID товара", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return productId;
        }
    }
}