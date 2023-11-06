using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XFC.Helper;
using XFC.View.Dialog.Product;
using XFC.View.Dialog.User;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace XFC.View.Dialog
{
    public partial class Form_Userinfo : Form
    {
        public Form_Userinfo()
        {
            InitializeComponent();

        }

        private void Form_Userinfo_Load(object sender, EventArgs e)
        {
            QueryAll();
        }
        /// <summary>
        /// 从数据库获取表格所需数据
        /// </summary>
        public void QueryAll()
        {
            using (OledbHelper helper = new OledbHelper())
            {
                helper.sqlstring = "select UserName,UserPassWord from UserInfo";
                DataSet ds = helper.GetDataSet();
                //设置表格控件的DataSource属性
                dataGridView1.DataSource = ds.Tables[0];
                //设置数据表格上显示的列标题
                //dataGridView1.Columns[0].HeaderText = "车辆ID";
                dataGridView1.Columns[0].HeaderText = "登录名";
                dataGridView1.Columns[1].HeaderText = "登录密码";
                
            }
        }
        /// <summary>
        /// 【查询】按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_select_Click(object sender, EventArgs e)
        {
            if (tb_UserName.Text.Trim() != "")
            {
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "select UserName,UserPassWord from UserInfo where UserName like '%{0}%'";
                    //填充占位符
                    helper.sqlstring = string.Format(helper.sqlstring, tb_UserName.Text);
                    DataSet ds = helper.GetDataSet();
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
        }
        /// <summary>
        /// 【添加】按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_add_Click(object sender, EventArgs e)
        {
            string UserName = "";
            string UserPassWord = "";

            //创建updateForm类的对象，并将课程信息传递给修改界面
            Form_UserTianJia form_UserTianJia = new Form_UserTianJia(UserName, UserPassWord);
            ////弹出修改信息窗口
            DialogResult dr = form_UserTianJia.ShowDialog();
            //判断是否单击确定按钮
            if (dr == DialogResult.OK)//()内的意义是？
            {
                //调用查询方法
                QueryAll();
            }
        }
        /// <summary>
        /// 【修改】按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_updata_Click(object sender, EventArgs e)
        {
            //获取DataGridView控件中的值

            //int ProductID = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());         
            string UserName = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string UserPassWord = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

            //创建updateForm类的对象，并将课程信息传递给修改界面
            Form_UserXiuGai form_UserXiuGai = new Form_UserXiuGai(UserName, UserPassWord);
            DialogResult dr = form_UserXiuGai.ShowDialog();
            //判断是否单击确定按钮
            if (dr == DialogResult.OK)//()内的意义是？
            {
                //调用查询全部课程方法
                QueryAll();
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            //获取DataGridView控件中选中行的编号列的值
            int id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

            using (OledbHelper helper = new OledbHelper())
            {
                helper.sqlstring = "delete from UserInfo where UserID ={0}";
                //填充占位符
                helper.sqlstring = string.Format(helper.sqlstring, id);
                // 执行SQL语句
                helper.ExecuteCommand();
                //弹出消息提示删除成功
                MessageBox.Show("删除成功!");
                //调用查询全部的方法，刷新DataGridView控件中的数据
                QueryAll();

            }
        }
    }
}
