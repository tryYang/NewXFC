using Microsoft.Reporting.Map.WebForms.BingMaps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XFC.Helper;
using XFC.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace XFC.View.Dialog.SongJianDanWei
{
    public partial class Form_SongJianTianJia : Form
    {
        public Form_SongJianTianJia(string CustomerDepartment,string  ContactPeople,string  PhoneNum,string Address)
        {
            InitializeComponent();
            textBox1.Text = CustomerDepartment;
            textBox2.Text = ContactPeople;
            textBox3.Text = PhoneNum;
            textBox4.Text = Address;
           

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


                helper.sqlstring = "insert into CustomerInfo (CustomerDepartment,ContactPeople,PhoneNum,Address) values ('{0}','{1}','{2}','{3}')";
                //填充占位符
                helper.sqlstring = string.Format(helper.sqlstring, textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
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
    }
}
