using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XFC.Helper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace XFC.View.Dialog.Product
{
    public partial class Form_ChanPinTianJia : Form
    {
        public Form_ChanPinTianJia(string Productname, string truckNo, string manufactureORG, string dipanORG, string dipanClass, string PumperORG, string PumperClass, string PumperType)
        {
            InitializeComponent();
            textBox1.Text = Productname;
            textBox2.Text = truckNo;
            textBox3.Text = manufactureORG;
            textBox4.Text = dipanORG;
            textBox5.Text = dipanClass;
            textBox6.Text = PumperORG;
            textBox7.Text = PumperClass;
            textBox8.Text = PumperType;

        }
        /// <summary>
        /// 【确认】按钮
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


                helper.sqlstring = "insert into SaveCarBasicInfo (CarName,CarModel,CarFac,UnderpanModel,UnderpanFac,PumpModel,PumpFac,PumpType) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
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
        /// <summary>
        /// 【取消】按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
