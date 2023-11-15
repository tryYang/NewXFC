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
using XFC.View.Dialog.Product;

namespace XFC.View.Dialog.ProductPump
{
    public partial class Form_SavePump : Form
    {
        public Form_SavePump()
        {
            InitializeComponent();
        }
        private void Form_SavePump_Load(object sender, EventArgs e)
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
                helper.sqlstring = "select PumpName,PumpFac,PumpType,Speed,InPipeD,OutPipeD,EpitopeDifference,PumpModel from SavePumpBasicInfo";
                DataSet ds = helper.GetDataSet();
                //设置表格控件的DataSource属性
                dataGridView1.DataSource = ds.Tables[0];
                //设置数据表格上显示的列标题
                //dataGridView1.Columns[0].HeaderText = "车辆ID";
                dataGridView1.Columns[0].HeaderText = "水泵名称";
                dataGridView1.Columns[1].HeaderText = "水泵厂家";
                dataGridView1.Columns[2].HeaderText = "水泵类型";
                dataGridView1.Columns[3].HeaderText = "额定转速";
                dataGridView1.Columns[4].HeaderText = "进口管径";
                dataGridView1.Columns[5].HeaderText = "出口管径";
                dataGridView1.Columns[6].HeaderText = "表位差";
                dataGridView1.Columns[7].HeaderText = "水泵型号";

            }
        }
        /// <summary>
        /// 【查询】按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_select_Click(object sender, EventArgs e)
        {
            if (tb_CarName.Text.Trim() != "")
            {
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "select PumpName,PumpFac,PumpType,Speed,InPipeD,OutPipeD,EpitopeDifference,PumpModel from SavePumpBasicInfo where PumpName like '%{0}%'";
                    //填充占位符
                    helper.sqlstring = string.Format(helper.sqlstring, tb_CarName.Text);
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
            string PumpName = "";
            string PumpFac = "";
            string PumpType = "";
            string Speed = "";
            string InPipeD = "";
            string OutPipeD = "";
            string EpitopeDifference = "";
            string PumpModel = "";

            //创建updateForm类的对象，并将课程信息传递给修改界面
            Form_SavePumpTianJia form_SavePumpTianJia = new Form_SavePumpTianJia(PumpName, PumpFac, PumpType, Speed, InPipeD, OutPipeD, EpitopeDifference, PumpModel);

            ////弹出修改信息窗口
            DialogResult dr = form_SavePumpTianJia.ShowDialog();
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
            string PumpName = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string PumpFac = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string PumpType = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string Speed = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            string InPipeD = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            string OutPipeD = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            string EpitopeDifference = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            string PumpModel = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();

            //创建updateForm类的对象，并将课程信息传递给修改界面
            Form_SavePumpXiuGai form_SavePumpXiuGai = new Form_SavePumpXiuGai(PumpName, PumpFac, PumpType, Speed, InPipeD, OutPipeD, EpitopeDifference, PumpModel);

            ////弹出修改信息窗口
            DialogResult dr = form_SavePumpXiuGai.ShowDialog();
            //判断是否单击确定按钮
            if (dr == DialogResult.OK)//()内的意义是？
            {
                //调用查询方法
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
            using (OledbHelper helper = new OledbHelper())
            {
                helper.sqlstring = "delete from SavePumpBasicInfo where PumpName ='{0}'";///用哪个作为唯一值删除呢
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
