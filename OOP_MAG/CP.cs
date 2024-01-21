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
    public partial class CP : Form
    {
        private NpgsqlConnection conn;
        private string connstring = "Server=localhost; Port=5432; User Id=postgres; Password=6360; Database=oop_app;";
        public CP()
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(conn);
            shoppingCartManager.AddProductToCart("Процессор 1");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(conn);
            shoppingCartManager.AddProductToCart("Процессор 2");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            ShoppingCartManager shoppingCartManager = new ShoppingCartManager(conn);
            shoppingCartManager.AddProductToCart("Процессор 3");
        }
    }
}
