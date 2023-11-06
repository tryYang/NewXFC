using System;
using System.Data.OleDb;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using XFC.Helper;

namespace XFC.View.Dialog.Product
{
    public partial class Form_ChanPinXiuGai : Form
    {
        public Form_ChanPinXiuGai( string Productname, string truckNo, string manufactureORG, string dipanORG, string dipanClass, string PumperORG, string PumperClass, string PumperType)
        {
            InitializeComponent();
          // textBox1.Text = ProductID.ToString();
            textBox2.Text = Productname;
            textBox3.Text = truckNo;
            textBox4.Text = manufactureORG;
            textBox5.Text = dipanORG;
            textBox6.Text = dipanClass;
            textBox7.Text = PumperORG;
            textBox8.Text = PumperClass;
            textBox9.Text = PumperType;
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
                helper.sqlstring = "update CarBasicInfo set [CarName]='{0}',[CarModel]='{1}',[CarFac]='{2}',[UnderpanFac]='{3}',[UnderModel]='{4}',[PumpFac]='{5}',[PumpModel]='{6}',[PumpType]='{7}' where CarID={8}";
                //填充占位符          
                helper.sqlstring = string.Format(helper.sqlstring, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text, textBox1.Text);
                // 执行SQL语句
                helper.ExecuteCommand();
                //弹出消息提示删除成功
                MessageBox.Show("修改成功!");
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

        }
    }
}

