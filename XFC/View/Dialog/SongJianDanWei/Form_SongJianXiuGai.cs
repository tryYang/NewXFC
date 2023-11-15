using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.OleDb;
using XFC.Helper;
using Microsoft.Reporting.Map.WebForms.BingMaps;

namespace XFC.View.Dialog.SongJianDanWei
{
    public partial class Form_SongJianXiuGai : Form
    {
        public Form_SongJianXiuGai(string CustomerDepart, string ContactPeople, string PhoneNum, string Address)
        {
            InitializeComponent();
           // textBox1.Text = CustomerDepart;
            textBox2.Text = CustomerDepart;
            textBox3.Text = ContactPeople;
            textBox4.Text = PhoneNum;
            textBox5.Text = Address;
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
                helper.sqlstring = "update CustomerInfo set [CustomerDepart]='{0}',[ContactPeople]='{1}',[PhoneNum]='{2}',[Address]='{3}' where CustomerDepart='{4}'";
                //填充占位符          
                helper.sqlstring = string.Format(helper.sqlstring, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox2.Text);
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
            this.Close();
        }
    }
}
