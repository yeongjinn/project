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
{    public partial class frmSales : Form
    {
        public User CurrentUser { get; set; }

        DataTable table = new DataTable();
        public frmSales()
        {
            InitializeComponent();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Price", typeof(string));
            table.Columns.Add("Count", typeof(string));
            table.Columns.Add("Total", typeof(string));

            dataGridView1.DataSource = table;
            numericUpDown1.Value = 1;
            //login 폼에서 public static string chk 설정 , 0을 매니저로
            if(Login.chk == "0")
            {
                textBox4.Text = "매니저님 반갑습니다.";
                button1.Visible = true;
                button2.Visible = true;
            }
            else
            {
                textBox4.Text = "사원님 반갑습니다.";
                button1.Visible = false;
                button2.Visible = false;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
            Login login = new Login();
            //if (login.ShowDialog() == DialogResult.OK)
            //{
                //  {

                //      label1.Text = $"{CurrentUser.name}님 환영합니다.";
                //      if (CurrentUser.position == "Y")
                //       {
                //           button1.Visible = true;
                //           button2.Visible = true;
                //      }
                //       else
                //       {
                //           button1.Visible = false;
                //           button2.Visible = false;
                //      }
                //  }
                //   else
                //   {
                //       Application.Exit();
                //   }
                DialogResult aa = login.ShowDialog();
                if (aa == DialogResult.OK)
                {
                    if (login.ShowDialog() == DialogResult.OK)
                    {

                        textBox4.Text = $"{CurrentUser.name}님 환영합니다.";
                        if (CurrentUser.position == "Y")
                        {
                            button1.Visible = true;
                            button2.Visible = true;
                        }
                        else
                        {
                            button1.Visible = false;
                            button2.Visible = false;
                        }
                    }
                    else
                    {
                        Application.Exit();
                  
                }
            }
            MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=1234;");
            connection.Open();
            if(connection.State == ConnectionState.Open)
            {
                label6.Text = "Connected";
                label6.ForeColor = Color.Black;
            }
            else
            {
                label6.Text = "DisConnected";
                label6.ForeColor = Color.Red;
            }
        }

        //담기
        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("항목을 정확히 입력해 주세요.");
                textBox1.Clear();
                textBox2.Clear();
            }
            else
            {
                decimal price = decimal.Parse(textBox2.Text);
                decimal count = numericUpDown1.Value;
                decimal total = price * count;

                //텍스트박스 정보 표에 삽입
                table.Rows.Add(textBox1.Text, textBox2.Text, numericUpDown1.Value, total);
                dataGridView1.DataSource = table;

                //텍스트박스 정보 초기화
                textBox1.Clear();
                textBox2.Clear();
                numericUpDown1.Value = 1;

                //합계
                decimal all = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    all += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
                }
                textBox3.Text = all.ToString();
            }
        }

        //삭제(취소)
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }

            //합계창에 수정된 값 넣기
            decimal all = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                all += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
            }
            textBox3.Text = all.ToString();
        }

        //계산
        private void button5_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Port=3306;Database=pos;uid=root;pwd=1234"))
            {
                conn.Open();
                //각 행의 정보를 반복문으로 불러옴
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    String Name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    String Price = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    String Count = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    String Total = dataGridView1.Rows[i].Cells[3].Value.ToString();

                    //INSERT INTO 쿼리문으로 받아온 정보 DB로 전송
                    string sql = string.Format("INSERT INTO sales(name, price, count, total, c_num) VALUES ('{0}', {1}, {2}, {3}, {4})", @Name, @Price, @Count, @Total, i);

                    //DB전송 진행 , 실패시 에러 메세지 출력
                    try
                    {
                        MySqlCommand command = new MySqlCommand(sql, conn);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            MessageBox.Show("계산되었습니다.");

            //데이터그리드뷰 초기호
            int rowCount = dataGridView1.Rows.Count;
            for (int n = 0; n < rowCount; n++)
            {
                if (dataGridView1.Rows[0].IsNewRow == false)
                    dataGridView1.Rows.RemoveAt(0);
            }

            //합계창 초기화
            textBox3.Text = "0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmDetail dlg = new frmDetail();
            dlg.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmItem dlg = new frmItem();
            dlg.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Text = "로그인";
            Login dlg = new Login();
            dlg.ShowDialog();
            this.Close();
        }
    }
}
