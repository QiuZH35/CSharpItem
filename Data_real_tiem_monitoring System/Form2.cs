using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Data_real_tiem_monitoring_System
{
    public partial class Form2 : Form
    {
        数据库基本信息采集 Form3 ;
        private SqlConnection connection;
        string conecsatr = "Server=localhost; User Id=sa;Password=123456;Database=test";


        //private SqlConnection connection;  //数据库连接
       // string conecsatr ="";
        string Tabletemp = "Datastruct";

        public Form2()
        {
            InitializeComponent();
            //连接数据库
            connection = new SqlConnection(conecsatr);



        }
        private void button1_Click(object sender, EventArgs e)    //查询数据库
        {
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("select * from " + Tabletemp, connection);//数据库连接桥
                    DataSet mysql = new DataSet(); //实例化数据集对象
                    adapter.Fill(mysql);
                    dataGridView1.DataSource = mysql.Tables[0];
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("数据库连接失败", "提示");
                    connection.Close();
                }

            }
            catch
            {
                MessageBox.Show("数据库连接失败", "提示");
            }
        }
      



       
        private void ReUpdata()
        {
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand(" dbcc checkident( "+Tabletemp+",reseed,0)");

                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                else
                {
                    connection.Close();
                   // MessageBox.Show("连接失败");
                }
                
            }
            catch
            {
                MessageBox.Show("连接失败");

            }
        }

        private void DelAlldata()   //删除数据
        {
            try
            {

                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand("delete from "+Tabletemp );

                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("删除成功", "提示");
                }
                else
                {
                    connection.Close();
                   // MessageBox.Show("连接失败");
                }
               
            }
            catch
            {
                MessageBox.Show("连接失败");
            }

        }


        private void button2_Click(object sender, EventArgs e)  //清空数据
        {

            DelAlldata();
            ReUpdata();
            
            //using (testEntities1 SetDel=new testEntities1())
            //{


            //    
            //}

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

       
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
           
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“testDataSet.Datastruct”中。您可以根据需要移动或移除它。
            // this.datastructTableAdapter.Fill(this.testDataSet.Datastruct);
            //GetDbinfo();
            //Form3 = new 数据库基本信息采集();
            //conecsatr = Form3.conecsatr;
            //Tabletemp = Form3.Tabletemp;
            //Console.WriteLine(Tabletemp + " " + conecsatr);
            //connection = new SqlConnection(conecsatr);
        }
        private void Updata()
        {

            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("select * from "+Tabletemp, connection);//数据库连接桥
                    DataSet mysql = new DataSet(); //实例化数据集对象
                    adapter.Fill(mysql);
                    dataGridView1.DataSource = mysql.Tables[0];
                    dataGridView1.MultiSelect = false;
                    dataGridView1.CurrentCell = dataGridView1.Rows[this.dataGridView1.Rows.Count - 1].Cells[0];
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("数据库连接失败", "提示");
                    connection.Close();
                }
        }
            catch
            {
                checkBox1.Text = "自动刷新";
                MessageBox.Show("连接失败");
            }

}

        private void button3_Click(object sender, EventArgs e)
        {
            Updata();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          
            Updata();
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                
                timer1.Start();
                checkBox1.Text = "取消自动";
               
                
            }
            if(checkBox1.Checked==false)
            {
                checkBox1.Text = "自动刷新";
                timer1.Stop();
            }
        }
    }
}
