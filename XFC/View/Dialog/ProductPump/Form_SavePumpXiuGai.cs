using Microsoft.Office.Interop.Excel;
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
    public partial class Form_SavePumpXiuGai : Form
    {
        public Form_SavePumpXiuGai(string PumpName, string PumpFac, string PumpType, string Speed, string InPipeD, string OutPipeD, string EpitopeDifference, string PumpModel)
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
                helper.sqlstring = "update SavePumpBasicInfo set [PumpName]='{0}',[PumpFac]='{1}',[PumpType]='{2}',[Speed]='{3}',[InPipeD]='{4}',[OutPipeD]='{5}',[EpitopeDifference]='{6}',[PumpModel]='{7}' where PumpName={8}";
                //填充占位符          
                helper.sqlstring = string.Format(helper.sqlstring, textBox1.Text,textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox1.Text);
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

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
