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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace XFC.View.Dialog.User
{
    public partial class Form_UserTianJia : Form
    {
        public Form_UserTianJia(string UserName, string UserPassWord)
        {
            InitializeComponent();
            tb_UserName.Text = UserName;
            tb_UserPassWord.Text = UserPassWord;
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


                helper.sqlstring = "insert into UserInfo (UserName,UserPassWord) values ('{0}','{1}')";
                //填充占位符
                helper.sqlstring = string.Format(helper.sqlstring, tb_UserName.Text, tb_UserPassWord.Text );
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
    }
}
