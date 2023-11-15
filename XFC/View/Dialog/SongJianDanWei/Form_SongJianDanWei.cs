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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using XFC.Helper;
using XFC.View.Dialog.Product;

namespace XFC.View.Dialog.SongJianDanWei
{
    public partial class Form_SongJianDanWei : Form
    {
        public Form_SongJianDanWei()
        {
            InitializeComponent();
        }
        private void Form_SongJianDanWei_Load(object sender, EventArgs e)
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
                helper.sqlstring = "select CustomerDepart,ContactPeople,PhoneNum,Address from CustomerInfo";
                DataSet ds = helper.GetDataSet();
                //设置表格控件的DataSource属性
                dataGridView1.DataSource = ds.Tables[0];
                //设置数据表格上显示的列标题
                //dataGridView1.Columns[0].HeaderText = "车辆ID";
                dataGridView1.Columns[0].HeaderText = "单位名称";
                dataGridView1.Columns[1].HeaderText = "联系人";
                dataGridView1.Columns[2].HeaderText = "联系电话";
                dataGridView1.Columns[3].HeaderText = "联系地址";
            
            }
        }
        /// <summary>
        /// 【查询】按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_select_Click(object sender, EventArgs e)
        {
            if (tb_CustomerName.Text.Trim() != "")
            {
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "select CustomerDepart,ContactPeople,PhoneNum,Address from CustomerInfo where CustomerDepart like '%{0}%'";
                    //填充占位符
                    helper.sqlstring = string.Format(helper.sqlstring, tb_CustomerName.Text);
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
            // int ProductID = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            string CustomerDepart = "";
            string ContactPeople = "";
            string PhoneNum = "";
            string Address = "";

            //创建updateForm类的对象，并将课程信息传递给修改界面
            Form_SongJianTianJia form_SongJianTianJia = new Form_SongJianTianJia(CustomerDepart, ContactPeople, PhoneNum, Address);

            ////弹出修改信息窗口
            DialogResult dr = form_SongJianTianJia.ShowDialog();
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
            string CustomerDepart = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string ContactPeople = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string PhoneNum = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string Address = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
          
            //创建updateForm类的对象，并将课程信息传递给修改界面
            Form_SongJianXiuGai form_SongJianXiuGai = new Form_SongJianXiuGai(CustomerDepart, ContactPeople, PhoneNum, Address);
            //弹出修改信息窗口
            DialogResult dr = form_SongJianXiuGai.ShowDialog();
            //判断是否单击确定按钮
            if (dr == DialogResult.OK)//()内的意义是？
            {
                //调用查询全部课程方法
                QueryAll();
            }
        }
        /// <summary>
        /// 【删除】按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            //获取DataGridView控件中选中行的编号列的值
            //int id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

            using (OledbHelper helper = new OledbHelper())
            {
                helper.sqlstring = "delete from CustomerInfo where CustomerDepart ='{0}'";
                //填充占位符
                helper.sqlstring = string.Format(helper.sqlstring, dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
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
