using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_MAG
{
    internal class UserManager
    {
        private NpgsqlConnection conn; 

        public UserManager(NpgsqlConnection connection)
        {
            conn = connection;
        }

        public bool DoesUserExist(string email, string password)
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

        public bool DoesUseRegistered(string email)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM users WHERE email = @email";
                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@email", email);

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

    }
}
