using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace ProJect
{
    public partial class Login : Form
    {
        public User CurrentUser { get; set; }

        //구분값을form1로 전송 위한 chk
        public static string chk;
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("아이디와 비밀번호를 입력하세요.");
                return;
            }
            try
            {
                string myConnection = "server=localhost;port=3306;database=pos;uid=root;pwd=1234";
                MySqlConnection myConn = new MySqlConnection(myConnection);
                //각각 항목 db 대조
                MySqlCommand selectCommand = new MySqlCommand("select * from pos.member where id='" + this.textBox1.Text + "' and password='" + this.textBox2.Text + "' and position='" + this.comboBox1.Text + "'", myConn);
                MySqlDataReader myReader;

                myConn.Open();
                myReader = selectCommand.ExecuteReader();

                int count = 0;

                while (myReader.Read())
                {
                    count = count + 1;
                }

                if (count == 1)
                {
                    //포지션 값을 0 혹은 1로 나뉘어 매니저 직원 판별
                    if (comboBox1.Text == "manager")
                    {
                        chk = "0";
                    }
                    else
                    {
                        chk = "1";
                    }
                    MessageBox.Show("로그인 되었습니다.");
                    this.Visible = false;
                    this.DialogResult = DialogResult.OK;


                    //Form1 showfrmSales = new Form1();
                    //showfrmSales.ShowDialog();                    
                }
                else if (count > 1)
                {
                    MessageBox.Show("중복된 유저가 존재합니다.");
                }
                else
                {
                    MessageBox.Show("아이디, 비밀번호나 직책이 일치하지 않습니다.");
                }
                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Register dlg = new Register();
            dlg.ShowDialog();
        }
    }
}