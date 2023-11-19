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

namespace XFC.View.Dialog.ShowLab
{
    public partial class Form_OpenPumpLab : Form
    {
        public Form_OpenPumpLab()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 定义查询信息方法
        /// </summary>
        private void Query()
        {
            string time1 = dateTimePicker1.Text;
            string time2 = dateTimePicker2.Text;

            using (OledbHelper helper = new OledbHelper())
            {
                helper.sqlstring = $"select [PumpLab.PumpLabID],[PumpLab.PumpID],[PumpLab.CustomerDepart],[PumpBasicInfo.PumpName],[PumpBasicInfo.PumpFac],[PumpBasicInfo.PumpType],[PumpBasicInfo.PumpModel],[PumpLab.L_Flowmeter],[PumpLab.H_Flowmeter],[PumpLab.ElectricalMachinery],[PumpLab.CheckPeople],[PumpLab.LabDate] from PumpBasicInfo,PumpLab where [PumpBasicInfo.PumpID] = [PumpLab.PumpID] and [PumpLab.LabDate] >= #{time1}# and [PumpLab.LabDate] <= #{time2}# ";
                DataSet ds = helper.GetDataSet();
                //设置表格控件的DataSource属性
                dataGridView1.DataSource = ds.Tables[0];
                //设置数据表格上显示的列标题        
                dataGridView1.Columns[0].HeaderText = "实验ID";
                dataGridView1.Columns[1].HeaderText = "水泵ID";
                dataGridView1.Columns[2].HeaderText = "送检单位";
                dataGridView1.Columns[3].HeaderText = "水泵名称";//基本信息-PumpBasicInfo
                dataGridView1.Columns[4].HeaderText = "水泵厂家";//基本信息
                dataGridView1.Columns[5].HeaderText = "水泵类型";//基本信息
                dataGridView1.Columns[6].HeaderText = "水泵型号";//基本信息
                dataGridView1.Columns[7].HeaderText = "低压流量计";
                dataGridView1.Columns[8].HeaderText = "中高压流量计";
                dataGridView1.Columns[9].HeaderText = "电机";
                dataGridView1.Columns[10].HeaderText = "检查人员";
                dataGridView1.Columns[11].HeaderText = "实验日期";
            }
        }

        private void Form_OpenPumpLab_Load(object sender, EventArgs e)
        {
            Query();
        }

        private void btn_Query_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close ();  
        }
    }
}
