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
    public partial class 数据库基本信息采集 : Form
    {
        private SqlConnection connection;
        public string conecsatr = "";
        public string Servertemp = "";
        public string Dbstemp = "";
        public string Usertemp = "";
        public string Passtemp = "";
        public string Tabletemp = "";
        public 数据库基本信息采集()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

       public void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {   
                Servertemp = comboBox1.Text;
                Dbstemp = comboBox2.Text;
                Usertemp = comboBox3.Text;
                Passtemp = comboBox4.Text;
                Tabletemp = comboBox5.Text;
                this.Close();
            }

           
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
           this.checkBox2.Checked = true;
            this.Close();
        }

        private void 数据库基本信息采集_Load(object sender, EventArgs e)
        {
            
            
           
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)   //检测数据库是否存在，不存在创建
        {

            Servertemp = "Server=" + comboBox1.Text + ";";
            Dbstemp = "Database=" + comboBox2.Text + ";";
            Usertemp = " User Id=" + comboBox3.Text + ";";
            Passtemp = "Password=" + comboBox4.Text + ";";

            Tabletemp = comboBox5.Text;
            conecsatr = Servertemp + Usertemp + Passtemp + Dbstemp;
            textBox1.Text = "此功能还未实现,如需存放请手动创库\r\n"+ "无法连接数据库请按以下格式创建数据库！\r\n" + "create database test;\r\n" + "use test;\r\n" + "create table Datastruct(id int primary key identity,\r\ntimedata varchar(50) not null,\r\ntemperature varchar(50) not null);\r\n\n使用sa用户密码为123456\r\n谢谢配合";
            try
            {
                if (conecsatr != "")
                {
                    connection = new SqlConnection(conecsatr);
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {

                    }
                    else
                    {

                        string createDB = "create database " + comboBox2.Text + ";";
                        string UseDb = "use " + comboBox2.Text;
                        SqlCommand CmdSQL = new SqlCommand(createDB);
                        CmdSQL.Connection = connection;
                        CmdSQL.ExecuteNonQuery();
                        connection.Close();
                    }


                }
            }
            catch
            {
                textBox1.Text = "注意无法连接数据库请按以下格式创建数据库！\r\n" + "create database test;\r\n" + "use test;\r\n" + "create table Datastruct(id int primary key identity,\r\ntimedata varchar(50) not null,\r\ntemperature varchar(50) not null);\r\n\n使用sa用户密码为123456\r\n谢谢配合";

     
                MessageBox.Show("无法连接到"+ Dbstemp);
            }


        }
    }
}
