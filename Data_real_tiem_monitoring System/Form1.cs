using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Data_real_tiem_monitoring_System
{
    public partial class From1 : Form
    {

        
        

        string strtemp="";
        int numtemp;
        double dbtemp;
       
        private SerialPort serialPort;
        private Queue<double> queue=new Queue<double>(30);

        int count = 0;
        int countbase = 0;
        int countBug = 0;
        int countTry = 0;
        int Updatanum = 20;

        Series Series1 = new Series("温度");

        数据库基本信息采集 Form3;
        private SqlConnection connection;  //数据库连接
        string conecsatr = "";
        string Servertemp= "";
        string Usertemp = "";
        string Passtemp = "";
        string Dbstemp = "";
        string Tabletemp = "";

        public From1()
        {
            InitializeComponent();
           

        }

        private void UpInfo(string strtemp)
        {

            if (strtemp.Substring(5, 1) == "1")
            {
                textBox4.Text = "关";
            }
            if (strtemp.Substring(5, 1) == "0")
            {
                textBox4.Text = "开";
            }
            if (strtemp.Substring(7, 1) == "1")
            {
                textBox5.Text = "关";
            }
            if (strtemp.Substring(7, 1) == "0")
            {
                textBox5.Text = "开";
            }


        }
        private void MyDataReceied(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                strtemp = serialPort.ReadExisting();
               
                try
                {
                    if (count == 3)
                    {
                        count = 0;
                        MessageBox.Show("数据读取错误三次，已为您关闭程序");
                        System.Environment.Exit(0);
                        serialPort.Close();
                    }
                    textBox3.AppendText(strtemp + "\r\n");
                    if (strtemp != "" && strtemp .Length==9)
                    {
                     
                        countBug = 0;
                        countTry = 0;

                        numtemp = Convert.ToInt32(strtemp.Substring(1, 3));    //获取数字，例如266

                        dbtemp = (Convert.ToDouble(numtemp)) / 10;   // Convert.ToDouble(numtemp)转换为Double的266   ,然后除10等于 26.6  
                        textBox1.Text = dbtemp.ToString() + " ℃";
                        UpdataQuevalues(dbtemp);
                        UpInfo(strtemp);
                       
                        SaveData();
                       
                    }
                    else
                    {
                        countBug++;
                        textBox1.Text = "";
                        if (countBug==20)
                        {
                            countBug= 0;
                            countTry++;
                            if(countTry==3)
                            {
                                serialPort.Close();
                                System.Environment.Exit(0);
                                MessageBox.Show("经检测数据读取多次失败，以为您关闭程序", "注意");
                            }
                            MessageBox.Show("糟糕数据读取出错了,请等待片刻", "注意");
                            
                        }
                    }
            }
                catch
            {
                count++;
                textBox7.Text = "连接失败";
                MessageBox.Show("数据读取出错了,试试重启上位机", "注意");
            }
        }));
        }
        private void SaveData()
        {
            
            if (serialPort.IsOpen)
            {
               
                if (conecsatr != "")
                {
                  
                    if (checkBox4.Checked == true)
                    {
                      //  Form3.Show();
                        connection = new SqlConnection(conecsatr);
                        checkBox4.Text = "取消保存";
                        try
                        {   //数据库增加数据

                            if (countbase == 3)
                            {
                                countbase = 0;
                                MessageBox.Show("数据库写入错误三次，请检查数据库是否存在，将为您关闭程序");
                                System.Environment.Exit(0);

                            }
                          
                            this. connection.Open();
                           
                            if (this.connection.State == ConnectionState.Open)
                            {
                                string wendu = dbtemp.ToString() + "℃";
                                string dataCmd = "insert  into "+Tabletemp+"(timedata,temperature) values ('" + DateTime.Now.ToLocalTime().ToString() + "','" +wendu + "');";
                                SqlCommand cmd = new SqlCommand(dataCmd);

                                cmd.Connection = this.connection;
                                cmd.ExecuteNonQuery();
                      
                                this.connection.Close();
                            }
                            else
                            {
                                this.connection.Close();
                                MessageBox.Show("连接失败");
                            }

                    }
                        catch
                    {
                        countbase++;
                        checkBox4.Checked = false;                         
                        MessageBox.Show("数据库写入出错了，检查数据库是否存在");
                    }

                }
                    if (checkBox4.Checked == false)
                    {
                        checkBox4.Text = "保存数据";
                        
                    }
                }
            }

        }

        private void UpdataQuevalues(double DB)
        {
            if(queue.Count>30)
            {
                for(int i=0;i<Updatanum;i++)
                {
                    queue.Dequeue();
                }
            }
            queue.Enqueue(DB);//队列增加数据
            this.chart1.Series[0].Points.Clear();  //清空
            for(int i=0;i<queue.Count-1;i++)
            {
                if(queue.Count>=2 && queue.ElementAt(i)==queue.ElementAt(i+1))
                {
                    i++;
                }
                this.chart1.Series[0].Points.AddXY(i, queue.ElementAt(i));
            }


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

            {
                //下拉控件设置默认值
                comboBox1.SelectedIndex = 2;
                comboBox2.SelectedIndex = 4;
                comboBox3.SelectedIndex = 2;
            }
            {    //Chart属性
                Series1.ChartType = SeriesChartType.Spline;//设置曲线
                Series1.ToolTip = "#VALX,#VALY";

                chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
                Series1.BorderWidth = 2;//曲线宽度
                Series1.Color = Color.Red;// 曲线颜色
                chart1.Series.Add(Series1);//添加曲线
              
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if(checkBox1.Checked==false)
            {

                if (checkBox3.Checked == true)
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Write("1");
                        pictureBox1.Image = Properties.Resources.灭;
                        checkBox1.Text = "开灯";
                    }
                    else
                    {
                        MessageBox.Show("未连接串口");
                        pictureBox1.Image = Properties.Resources.灭;
                        checkBox1.Text = "开灯";
                    }
                }
               
            }

            if(checkBox1.Checked==true)
            {
                if (checkBox3.Checked == true)
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Write("2");
                        pictureBox1.Image = Properties.Resources.亮;
                        checkBox1.Text = "关灯";
                    }
                    else
                    {
                        MessageBox.Show("未连接串口");
                        pictureBox1.Image = Properties.Resources.灭;
                        checkBox1.Text = "开灯";
                    }
                }
                else
                {
                    MessageBox.Show("未连接串口");
                }
            }
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
            {
                if (checkBox3.Checked == true)
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Write("3");
                        pictureBox2.Image = Properties.Resources.风扇停;
                        checkBox2.Text = "启动";
                    }
                    else
                    {
                        MessageBox.Show("未连接串口");
                        pictureBox2.Image = Properties.Resources.风扇停;
                        checkBox2.Text = "启动";
                    }
                }
                
            }
            if (checkBox2.Checked == true)
            {
                if (checkBox3.Checked == true)
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Write("4");
                        pictureBox2.Image = Properties.Resources.风扇转;
                        checkBox2.Text = "停止";
                    }
                    else
                    {
                        MessageBox.Show("未连接串口");
                        pictureBox2.Image = Properties.Resources.风扇停;
                        checkBox2.Text = "启动";

                    }
                }
                else
                {
                    MessageBox.Show("未连接串口");
                }
            }
           
        }

       public void GetDbinfo()
        {
            Form3 = new 数据库基本信息采集();
            Form3.Show();
            
            Servertemp = "Server=" + Form3.comboBox1.Text + ";";
            Dbstemp = "Database=" + Form3.comboBox2.Text + ";";
            Usertemp = " User Id=" + Form3.comboBox3.Text + ";";
            Passtemp = "Password=" + Form3.comboBox4.Text + ";";
           
            Tabletemp = Form3.comboBox5.Text;
            conecsatr = Servertemp + Usertemp + Passtemp + Dbstemp;

      

            if (Form3.checkBox2.Checked == true)
            {
                conecsatr = "";
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox3.Checked==false)  //关闭串口
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Write("3");
                    pictureBox2.Image = Properties.Resources.风扇停;
                    checkBox2.Text = "启动";

                    serialPort.Write("1");
                    pictureBox1.Image = Properties.Resources.灭;
                    checkBox1.Text = "开灯";
                    serialPort.Close();
                    textBox1.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox7.Text = "";
                    checkBox3.Text = "打开串口";
                }
            }

            if (checkBox3.Checked == true)//打开串口
            {
                
                {   //串口连接
                    serialPort = new SerialPort(comboBox1.Text);
                    try
                    {
                       
                        serialPort.BaudRate = Convert.ToInt32(comboBox2.Text);
                        label7.Text ="来自"+comboBox1.Text + "的数据:";
                        label12.Text = comboBox1.Text+"状态：";
                        textBox7.Text = "连接成功";
                        serialPort.StopBits = StopBits.One;
                        serialPort.DataBits = Convert.ToInt32(comboBox3.Text);
                        serialPort.DataReceived += MyDataReceied;
                        serialPort.ReceivedBytesThreshold = 1;
                        serialPort.Open();
                        serialPort.Write("1");
                        serialPort.Write("3");
                        GetDbinfo();
                        checkBox3.Text = "关闭串口";

                        MessageBox.Show("连接成功");


                    }
                  catch
                    {
                        label12.Text = comboBox1.Text + "状态：";
                        textBox7.Text = "连接失败";
                        MessageBox.Show("端口不存在");

                    }

                }
               
            }
           
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 form2 = new Form2();
                form2.Show();
            }
            catch
            {
                MessageBox.Show("无法查看数据库");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox2.Text = DateTime.Now.ToLocalTime().ToString();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("连接成功");
            System.Threading.Thread.Sleep(5000);
            timer2.Stop();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                string textstr = textBox6.Text;

                if (textstr == "1")
                {


                    serialPort.Write("1");
                    pictureBox1.Image = Properties.Resources.灭;
                    checkBox1.Text = "开灯";

                }
                if (textstr == "3")
                {

                    serialPort.Write("3");
                    pictureBox2.Image = Properties.Resources.风扇停;
                    checkBox2.Text = "启动";

                }
                if (textstr == "2")
                {

                    serialPort.Write("2");
                    pictureBox1.Image = Properties.Resources.亮;
                    checkBox1.Text = "关灯";

                }

                if (textstr == "4")
                {

                    serialPort.Write("4");
                    pictureBox2.Image = Properties.Resources.风扇转;
                    checkBox2.Text = "停止";

                }
                textBox6.Text = "";
            }
            
        }
    }
}
