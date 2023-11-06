using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using XFC.Helper;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace XFC.View.Dialog.User
{
    public partial class Form_UserXiuGai : Form
    {
        public Form_UserXiuGai(string UserName,string  UserPassWord)
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
                helper.sqlstring = "update UserInfo set [UserName]='{0}',[UserPassWord]='{1}'";
                //填充占位符          
                helper.sqlstring = string.Format(helper.sqlstring, tb_UserName,tb_UserPassWord,tb_UserID);
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
