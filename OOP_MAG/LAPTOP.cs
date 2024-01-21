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
    public partial class LAPTOP : Form
    {

        private NpgsqlConnection conn;
        private string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=6360; Database=oop_app;";
        public LAPTOP()
        {
            InitializeComponent();
            conn = new NpgsqlConnection(connstring);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            MainPage mainPage = MainPage.GetInstance();
            mainPage.Show();
            this.Close();
        }    
        private int GetUserIdByEmail(string email)
        {
            int userId = -1;

            try
            {
                conn.Open();

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
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return userId;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(conn);
            shoppingCartManager.AddProductToCart("Ноутбук 1");
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(conn);
            shoppingCartManager.AddProductToCart("Ноутбук 2");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(conn);
            shoppingCartManager.AddProductToCart("Ноутбук 3");
        }
    }
}