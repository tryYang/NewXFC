using Microsoft.Reporting.WinForms;
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
using XFC.Helper;
using XFC.Model;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace XFC.View.Dialog.Print
{
    public partial class Form_Print1 : Form
    {
        public Form_Print1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取数据源函数
        /// </summary>
        /// <param name="sqlDi"></param>
        /// <param name="rds_Name"></param>
        /// <param name="ReportPath"></param>
        public void ShuJuYuan(string sqlDi, string rds_Name, string ReportPath)
        {
            using (OledbHelper helper = new OledbHelper())
            {
                helper.sqlstring = sqlDi;
                System.Data.DataSet ds1 = helper.GetDataSet();
                ReportDataSource rds = new ReportDataSource();
                // rds.Name = "车载泵低压泵1";
                rds.Name = rds_Name;
                rds.Value = ds1.Tables[0];
                // this.reportViewer1.LocalReport.ReportPath = "Report_Car_DiYa.rdlc";
                this.reportViewer1.LocalReport.ReportPath = ReportPath;
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(rds);
                this.reportViewer1.RefreshReport();
            }
        }

        /// <summary>
        /// flag1=低压工况；flag2=一点三工况；flag3=超负荷工况；flag4=半流量工况；flag5=高压工况；flag6=中高压工况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Print1_Load(object sender, EventArgs e)
        {
            bool flag1 = true;// flag1=低压工况；
            bool flag2 = true;//flag2=一点三工况；
            bool flag3 = true;//flag3=超负荷工况；
            bool flag4 = true;//flag4=半流量工况；
            bool flag5 = true;//flag5=高压工况；
            bool flag6 = true;//flag6=中压工况
            

            using (OledbHelper helper = new OledbHelper())
            {
                if (ConstantValue.EquipemntList[0]==Equipment.Car)
                {
                    if (ConstantValue.PumpTypeList[0] == PumpType.DiYaPump)
                    {

                    }
                }
                if (textBox1.Text == "消防车")
                {
                    if (textBox2.Text == "低压泵")
                    {
                        List<int> gkList = new List<int>();
                        if (flag1 == true)// flag1=低压工况；运行了低压工况
                        {
                            gkList.Add(1);
                        }
                        if (flag2 == true)
                        {
                            gkList.Add(2);
                        }
                        if (flag3 == true)
                        {
                            gkList.Add(3);
                        }
                        if (flag4 == true)
                        {
                            gkList.Add(4);
                        }
                        PrintSqlGenerateHelper helper1 = new PrintSqlGenerateHelper(gkList);
                       // MessageBox.Show(helper1.Generate());
                        ShuJuYuan(helper1.Generate(), "车载泵低压泵1", "Report_Car_DiYa.rdlc");
                    }
                    else if (textBox2.Text == "高压泵" || textBox2.Text == "中压泵")
                    {
                        List<int> gkList = new List<int>();
                        if (flag1 == true)// flag1=低压工况；运行了低压工况
                        {
                            gkList.Add(1);
                        }
                        if (flag4 == true)
                        {
                            gkList.Add(4);
                        }
                        PrintSqlGenerateHelper helper1 = new PrintSqlGenerateHelper(gkList);
                        ShuJuYuan(helper1.Generate(), "车载泵高压泵和中压泵1", "Report_Car_GaoAndZhongYa.rdlc");
                    }
                    else if (textBox2.Text == "高低压泵")
                    {
                        List<int> gkList = new List<int>();
                        if (flag1 == true)// flag1=低压工况；运行了低压工况
                        {
                            gkList.Add(1);
                        }
                        if (flag5 == true)
                        {
                            gkList.Add(5);
                        }
                        if (flag3 == true)
                        {
                            gkList.Add(3);
                        }
                        if (flag4 == true)
                        {
                            gkList.Add(4);
                        }
                        PrintSqlGenerateHelper helper1 = new PrintSqlGenerateHelper(gkList);
                        ShuJuYuan(helper1.Generate(), "车载泵高低压泵1", "Report_Car_GaoDiYa.rdlc");
                    }
                    else if (textBox2.Text == "中低压泵")
                    {
                        List<int> gkList = new List<int>();
                        if (flag1 == true)// flag1=低压工况；运行了低压工况
                        {
                            gkList.Add(1);
                        }
                        if (flag6 == true)
                        {
                            gkList.Add(6);
                        }
                        if (flag3 == true)
                        {
                            gkList.Add(3);
                        }
                        if (flag4 == true)
                        {
                            gkList.Add(4);
                        }
                        PrintSqlGenerateHelper helper1 = new PrintSqlGenerateHelper(gkList);
                        //ShuJuYuan(helper1.Generate(), "车载泵中低压泵1", "Report_Car_zhongDiYa.rdlc");
                    }
                    else if (textBox2.Text == "")
                    {
                        MessageBox.Show("未选择具体工况，请选择工况！");
                    }

                }
                else if (textBox1.Text == "消防泵")
                {
                    if (textBox2.Text == "低压泵")
                    {
                        List<int> gkList = new List<int>();
                        if (flag1 == true)// flag1=低压工况；运行了低压工况
                        {
                            gkList.Add(1);
                        }
                        if (flag2 == true)
                        {
                            gkList.Add(2);
                        }
                        if (flag3 == true)
                        {
                            gkList.Add(3);
                        }
                        if (flag4 == true)
                        {
                            gkList.Add(4);
                        }
                        PrintSqlGenerateHelper_Pump helper1 = new PrintSqlGenerateHelper_Pump(gkList);
                        //MessageBox.Show(helper1.Generate());
                        ShuJuYuan(helper1.Generate(), "消防泵低压泵1", "Report_Pump_DiYa.rdlc");
                    }
                    else if (textBox2.Text == "高压泵" || textBox2.Text == "中压泵")
                    {
                        List<int> gkList = new List<int>();
                        if (flag1 == true)// flag1=低压工况；运行了低压工况
                        {
                            gkList.Add(1);
                        }
                        if (flag4 == true)
                        {
                            gkList.Add(4);
                        }
                        PrintSqlGenerateHelper_Pump helper1 = new PrintSqlGenerateHelper_Pump(gkList);
                        ShuJuYuan(helper1.Generate(), "消防泵高压泵和中压泵1", "Report_Pump_GaoAndZhongYa.rdlc");
                    }
                    else if (textBox2.Text == "高低压泵")
                    {
                        List<int> gkList = new List<int>();
                        if (flag1 == true)// flag1=低压工况；运行了低压工况
                        {
                            gkList.Add(1);
                        }
                        if (flag5 == true)
                        {
                            gkList.Add(5);
                        }
                        if (flag3 == true)
                        {
                            gkList.Add(3);
                        }
                        if (flag4 == true)
                        {
                            gkList.Add(4);
                        }
                        PrintSqlGenerateHelper_Pump helper1 = new PrintSqlGenerateHelper_Pump(gkList);
                        ShuJuYuan(helper1.Generate(), "消防泵高低压泵1", "Report_Pump_GaoDiYa.rdlc");
                    }
                    else if (textBox2.Text == "中低压泵")
                    {
                        List<int> gkList = new List<int>();
                        if (flag1 == true)// flag1=低压工况；运行了低压工况
                        {
                            gkList.Add(1);
                        }
                        if (flag6 == true)
                        {
                            gkList.Add(6);
                        }
                        if (flag3 == true)
                        {
                            gkList.Add(3);
                        }
                        if (flag4 == true)
                        {
                            gkList.Add(4);
                        }
                        PrintSqlGenerateHelper_Pump helper1 = new PrintSqlGenerateHelper_Pump(gkList);
                        ShuJuYuan(helper1.Generate(), "消防泵中低压泵1", "Report_Pump_zhongDiYa.rdlc");
                    }
                    else if (textBox2.Text == "")
                    {
                        MessageBox.Show("未选择具体工况，请选择工况！");
                    }

                }
                else if (textBox1.Text == "")
                {
                     MessageBox.Show("当前无实验进行，请新建实验！");

                }

            }
        }


    }
}
