using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;

namespace OOP_MAG
{
    public partial class admin : Form
    {
        private NpgsqlConnection conn;
        private string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=6360; Database=oop_app;";

        public admin()
        {
            InitializeComponent();
            conn = new NpgsqlConnection(connstring);

            comboBox2.Items.Add("Ноутбук 1");
            comboBox2.Items.Add("Компьютер 1");
            comboBox2.Items.Add("Процессор 1");
            comboBox2.Items.Add("Ноутбук 2");
            comboBox2.Items.Add("Компьютер 2");
            comboBox2.Items.Add("Процессор 2");
            comboBox2.Items.Add("Ноутбук 3");
            comboBox2.Items.Add("Компьютер 3");
            comboBox2.Items.Add("Процессор 3");

            dataGridView1.Columns.Add("product_id", "ID");
            dataGridView1.Columns.Add("name", "Название");
            dataGridView1.Columns.Add("description", "Описание");
            dataGridView1.Columns.Add("price", "Цена");

            dataGridView2.Columns.Add("user_id", "ID");
            dataGridView2.Columns.Add("email", "Email");
            dataGridView2.Columns.Add("password", "Password");
            dataGridView2.Columns.Add("full_name", "Full Name");
            dataGridView2.Columns.Add("phone_number", "Phone Number");
            dataGridView2.Columns.Add("address", "Address");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int numberOfProducts = Convert.ToInt32(textBox3.Text);

                for (int i = 0; i < numberOfProducts; i++)
                {
                    int selectedProductId = GenerateUniqueRandomNumber(); 
                    string productName = comboBox2.SelectedItem.ToString();
                    string productDescription = textBox1.Text;
                    decimal productPrice = Convert.ToDecimal(textBox2.Text);

                    // Вставка нового товара в базу данных
                    InsertProductIntoDatabase(selectedProductId, productName, productDescription, productPrice);
                }

                MessageBox.Show($"Добавлено {numberOfProducts} товаров", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка добавления товара", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertProductIntoDatabase(int productId, string name, string description, decimal price)
        {
            try
            {
                conn.Open();

                // запрос для вставки нового товара в базу данных
                string query = "INSERT INTO products (product_id, name, description, price) VALUES (@productId, @name, @description, @price)";

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@price", price);

                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private int GenerateUniqueRandomNumber()
        {
            Random random = new Random();
            int newRandomNumber;

            // Повторяем генерацию до тех пор, пока не найдем уникальное значение
            do
            {
                newRandomNumber = random.Next(1000, 999999);
            } while (IsProductIdExists(newRandomNumber)); // Проверка на уникальность

            return newRandomNumber;
        }

        private bool IsProductIdExists(int productId)
        {
            try
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM products WHERE product_id = @productId";

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка при выполнении запроса", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string productName = textBox5.Text;
                decimal? productPrice = string.IsNullOrEmpty(textBox6.Text) ? (decimal?)null : Convert.ToDecimal(textBox6.Text);
                int? productId = string.IsNullOrEmpty(textBox7.Text) ? (int?)null : Convert.ToInt32(textBox7.Text);

                SearchAndDisplayProducts(productName, productPrice, productId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка при выполнении запроса", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchAndDisplayProducts(string productName, decimal? productPrice, int? productId)
        {
            try
            {
                conn.Open();

                // Формируем динамический запрос SELECT с учетом заполненных критериев поиска
                string query = "SELECT product_id, name, description, price FROM products WHERE 1=1 ";

                if (!string.IsNullOrEmpty(productName))
                {
                    query += " AND name ILIKE @productName";
                }

                if (productPrice.HasValue)
                {
                    query += " AND price = @productPrice";
                }

                if (productId.HasValue)
                {
                    query += " AND product_id = @productId";
                }

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    // Добавляем параметры для условий поиска
                    if (!string.IsNullOrEmpty(productName))
                    {
                        command.Parameters.AddWithValue("@productName", "%" + productName + "%");
                    }

                    if (productPrice.HasValue)
                    {
                        command.Parameters.AddWithValue("@productPrice", productPrice);
                    }

                    if (productId.HasValue)
                    {
                        command.Parameters.AddWithValue("@productId", productId);
                    }

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        // Очищаем предыдущие данные перед обновлением
                        dataGridView1.Rows.Clear();

                        // Читаем данные из базы данных и добавляем их в DataGridView
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(
                                reader["product_id"],
                                reader["name"],
                                reader["description"],
                                reader["price"]
                            );
                        }
                    }
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string userEmail = textBox10.Text;
                string userName = textBox9.Text;

                SearchAndDisplayUsers(userEmail, userName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка при выполнении запроса", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SearchAndDisplayUsers(string userEmail, string userName)
        {
            try
            {
                conn.Open();

                // Формируем динамический запрос SELECT с учетом заполненных критериев поиска
                string query = "SELECT user_id, email, password, full_name, phone_number, address FROM users WHERE 1=1 ";

                if (!string.IsNullOrEmpty(userEmail))
                {
                    query += " AND email ILIKE @userEmail";
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    query += " AND (full_name ILIKE @userName OR phone_number ILIKE @userName)";
                }

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    // Добавляем параметры для условий поиска
                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        command.Parameters.AddWithValue("@userEmail", "%" + userEmail + "%");
                    }

                    if (!string.IsNullOrEmpty(userName))
                    {
                        command.Parameters.AddWithValue("@userName", "%" + userName + "%");
                    }

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        // Очищаем предыдущие данные перед обновлением
                        dataGridView2.Rows.Clear();

                        // Читаем данные из базы данных и добавляем их в DataGridView
                        while (reader.Read())
                        {
                            dataGridView2.Rows.Add(
                                reader["user_id"],
                                reader["email"],
                                reader["password"],
                                reader["full_name"],
                                reader["phone_number"],
                                reader["address"]
                            );
                        }
                    }
                }
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