using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XFC.Helper;
using XFC.Model;

namespace XFC.View.Dialog.ProductPump
{
    public partial class Form_SavePumpTianJia : Form
    {
        public Form_SavePumpTianJia(string PumpName,string PumpFac,string PumpType,string Speed,string InPipeD,string OutPipeD,string EpitopeDifference,string PumpModel)
        {
            InitializeComponent();
            textBox1.Text = PumpName;
            textBox2.Text = PumpFac;
            textBox3.Text = PumpType;
            textBox4.Text = Speed;
            textBox5.Text = InPipeD;
            textBox6.Text = OutPipeD;
            textBox7.Text = EpitopeDifference;
            textBox8.Text = PumpModel;
        }
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_affirm_Click(object sender, EventArgs e)
        {
            using (OledbHelper helper = new OledbHelper())
            {
                //helper.sqlstring = "Select Max(CarID) from CarBasicInfo";
                //int cmd1 = helper.ExecuteCommand() + 1;
                //string MaxID = cmd1.ToString();


                helper.sqlstring = "insert into PumpBasicInfo (PumpName,PumpFac,PumpType,Speed,InPipeD,OutPipeD,EpitopeDifference,PumpModel) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
                //填充占位符
                helper.sqlstring = string.Format(helper.sqlstring, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text);
                //执行修改操作的SQL
                helper.ExecuteCommand();
                MessageBox.Show("添加成功！");
                //设置当前窗体DislogResult结果为OK
                this.DialogResult = DialogResult.OK;
                //关闭窗体
                this.Close();

            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
