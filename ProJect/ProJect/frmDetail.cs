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
    public partial class frmDetail : Form
    {
        //서버 사용 선언
        MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;database=pos;username=root;password=1234;");
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataTable table = new DataTable();

        int selectedRow;
        public frmDetail()
        {
            InitializeComponent();
        }

        //데이터 조회 함수
        public void searchData(string valueToSearch)
        {
            string query = "SELECT * FROM sales WHERE CONCAT(`name`, `price`, `count`, `total`) like '%" + valueToSearch + "%'";
            ;
            command = new MySqlCommand(query, connection);
            adapter = new MySqlDataAdapter(command);
            table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }

        //DB에서 데이터 불러온 후 텍스트박스 초기화
        public void LoadData()
        {
            string sql = "server=localhost;port=3306;database=pos;uid=root;pwd=1234";
            MySqlConnection con = new MySqlConnection(sql);
            MySqlCommand cmd_db = new MySqlCommand("SELECT * FROM sales;", con);

            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmd_db;
                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bSource = new BindingSource();
                bSource.DataSource = dbdataset;
                dataGridView1.DataSource = bSource;
                sda.Update(dbdataset);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //텍스트박스 초기화
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";

        }
        private void button5_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            searchData("");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("검색 정보를 입력해주세요");
            }
            else
            {
                string valueToSearch = textBox1.Text.ToString();
                searchData(valueToSearch);
                //텍스트 박스 초기화
                textBox1.Text = "";
            }
        }
        //셀 클릭시 해당 행의 정보를 텍스트박스에 채움
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[selectedRow];
            textBox2.Text = row.Cells[0].Value.ToString();
            textBox3.Text = row.Cells[1].Value.ToString();
            textBox4.Text = row.Cells[2].Value.ToString();
            textBox5.Text = row.Cells[3].Value.ToString();
            textBox6.Text = row.Cells[4].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string constring = "server=localhost;port=3306;database=pos;uid=root;pwd=1234";
            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("항목을 정확히 입력해주세요");
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
            }
            else
            {
                decimal price = decimal.Parse(textBox4.Text);
                decimal count = decimal.Parse(textBox5.Text);
                decimal total = price * count;

                textBox6.Text = total.ToString();

                //업데이트를 통해 DB로 수정된 데이터 전송
                string Query = "update pos.sales set no ='" + this.textBox2.Text + "',name='" + this.textBox3.Text + "',price='" + this.textBox4.Text + "'," + "count='" + this.textBox5.Text + "',total='" + this.textBox6.Text + "' where no = '" + this.textBox2.Text + "'";
                MySqlConnection conDataBase = new MySqlConnection(constring);
                MySqlCommand cmdDatabase = new MySqlCommand(Query, conDataBase);
                MySqlDataReader myReader;

                try
                {
                    conDataBase.Open();
                    myReader = cmdDatabase.ExecuteReader();
                    MessageBox.Show("수정완료");
                    while (myReader.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            LoadData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string constring = "Server=localhost;Port=3306;Database=pos;Uid=root;Pwd=1234";
            if (textBox2.Text == "")
            {
                MessageBox.Show("삭제할 항목을 찾지 못했습니다.");
            }
            else
            {
                //delete를 통해 DB로 삭제된 데이터 전송
                string Query = "delete from pos.sales where no ='" + this.textBox2.Text + "';";
                MySqlConnection conDataBase = new MySqlConnection(constring);
                MySqlCommand cmdDatabase = new MySqlCommand(Query, conDataBase);
                MySqlDataReader myReader;

                try
                {
                    conDataBase.Open();
                    myReader = cmdDatabase.ExecuteReader();
                    MessageBox.Show("삭제완료");

                    while (myReader.Read())
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                LoadData();
            }
        }
    }
}