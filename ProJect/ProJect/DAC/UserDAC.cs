using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProJect
{
    public class User
    {
        public string UID { get; set; }
        public string ID { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string position { get; set; }
    }

    public class UserDAC : IDisposable
    {
        MySqlConnection conn;

        public UserDAC()
        {
            conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["myDB"].ConnectionString);
            conn.Open();
        }

        public void Dispose()
        {
            conn.Close();
        }

        public User Login(string uID)
        {
            string sql = "select UID, ID, password, name, position from TBUser where UID=@UID";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@userID", uID);

            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                User loginUser = new User();
                loginUser.UID = reader["UID"].ToString();
                loginUser.ID = reader["ID"].ToString();
                loginUser.password = reader["password"].ToString();
                loginUser.name = reader["name"].ToString();
                loginUser.position = reader["position"].ToString();

                return loginUser;
            }
            else
            {
                return null;
            }
        }

        public bool ConfirmUser(string id, string name)
        {
            string sql = "select count(*) from TBUser where UID=@UID and name=@name";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@UID", id);
            cmd.Parameters.AddWithValue("@name", name);

            int cnt = Convert.ToInt32(cmd.ExecuteScalar());
            return (cnt > 0);
        }

        public bool UpdatePWD(string uid, string newPwd)
        {
            string sql = "update TBUser set password = @password where ID=@ID";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@password", newPwd);
            cmd.Parameters.AddWithValue("@ID", uid);
            int iRowsAffect = cmd.ExecuteNonQuery();
            return (iRowsAffect > 0);
        }

    }
}