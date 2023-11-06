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
using Microsoft.Reporting.WinForms;
using System.Data.SqlTypes;
using XFC;

namespace XFC.View.Dialog.Product
{
    public partial class Form_ChanPin : Form
    {
        public Form_ChanPin()
        {
            InitializeComponent();
        }

        private void Form_ChanPin_Load(object sender, EventArgs e)
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
                helper.sqlstring = "select CarName,CarModel,CarFac,UnderpanModel,UnderpanFac,PumpModel,PumpFac,PumpType from CarBasicInfo";
                DataSet ds = helper.GetDataSet();
                //设置表格控件的DataSource属性
                dataGridView1.DataSource = ds.Tables[0];
                //设置数据表格上显示的列标题
                //dataGridView1.Columns[0].HeaderText = "车辆ID";
                dataGridView1.Columns[0].HeaderText = "车辆名称";
                dataGridView1.Columns[1].HeaderText = "车辆型号";
                dataGridView1.Columns[2].HeaderText = "车辆厂家";
                dataGridView1.Columns[3].HeaderText = "底盘型号";
                dataGridView1.Columns[4].HeaderText = "底盘厂家";
                dataGridView1.Columns[5].HeaderText = "水泵型号";
                dataGridView1.Columns[6].HeaderText = "水泵厂家";
                dataGridView1.Columns[7].HeaderText = "水泵类型";
            }
        }
        /// <summary>
        /// 【查询】按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_select_Click(object sender, EventArgs e)
        {
            if(tb_CarName.Text.Trim()  != "")
            {
                using(OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "select CarName,CarModel,CarFac,UnderpanModel,UnderpanFac,PumpModel,PumpFac,PumpType from CarBasicInfo where CarName like '%{0}%'";
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
           // int ProductID = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            string Productname = "";
            string truckNo = "";
            string manufactureORG = "";
            string dipanORG = "";
            string dipanClass = "";
            string PumperORG = "";
            string PumperClass = "";
            string PumperType = "";
            //创建updateForm类的对象，并将课程信息传递给修改界面
           Form_ChanPinTianJia form_ChanPinTianJia = new Form_ChanPinTianJia(Productname, truckNo, manufactureORG, dipanORG, dipanClass, PumperORG, PumperClass, PumperType);

            ////弹出修改信息窗口
            DialogResult dr = form_ChanPinTianJia.ShowDialog();
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
            
           // int ProductID = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());         
            string Productname = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();            
            string truckNo = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();           
            string manufactureORG = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();           
            string dipanORG = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();    
            string dipanClass = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();  
            string PumperORG = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            string PumperClass = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            string PumperType = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
            //创建updateForm类的对象，并将课程信息传递给修改界面
            Form_ChanPinXiuGai form_ChanPinXiuGai = new Form_ChanPinXiuGai(Productname, truckNo, manufactureORG, dipanORG, dipanClass, PumperORG, PumperClass, PumperType);
            //弹出修改信息窗口
            DialogResult dr = form_ChanPinXiuGai.ShowDialog();
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
            int id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());

            using (OledbHelper helper = new OledbHelper())
            {
                helper.sqlstring = "delete from CarBasicInfo where CarID ={0}";
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
