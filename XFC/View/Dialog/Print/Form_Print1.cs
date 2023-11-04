using Microsoft.Reporting.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
        public void ShuJuYuan(string sqlDi, List<string> print)
        {
            using (OledbHelper helper = new OledbHelper())
            {
                helper.sqlstring = sqlDi;
                System.Data.DataSet ds1 = helper.GetDataSet();
                ReportDataSource rds = new ReportDataSource();
                // rds.Name = "车载泵低压泵1";
                rds.Name = print[0];
                rds.Value = ds1.Tables[0];
                // this.reportViewer1.LocalReport.ReportPath = "Report_Car_DiYa.rdlc";
                this.reportViewer1.LocalReport.ReportPath = print[1];
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
            Printload(0);
        }

        private void Printload( int index)
        {
            
            List<List<string>> PrintList_car = new List<List<string>>();
            PrintList_car.Add(new List<string>() { "车载泵低压泵1", "Report_Car_DiYa.rdlc" });
            PrintList_car.Add(new List<string>() { "车载泵高压泵和中压泵1", "Report_Car_GaoAndZhongYa.rdlc" });
            PrintList_car.Add(new List<string>() { "车载泵高低压泵1", "Report_Car_GaoDiYa.rdlc" });
            PrintList_car.Add(new List<string>() { "车载泵中低压泵1", "Report_Car_zhongDiYa.rdlc" });
            List<List<string>> PrintList_pump = new List<List<string>>();
            PrintList_pump.Add(new List<string>() { "消防泵低压泵1", "Report_Pump_DiYa.rdlc" });
            PrintList_pump.Add(new List<string>() { "消防泵高压泵和中压泵1", "Report_Pump_DiYa.rdlc" });
            PrintList_pump.Add(new List<string>() { "消防泵高低压泵1", "Report_Pump_GaoDiYa.rdlc" });
            PrintList_pump.Add(new List<string>() { "消防泵中低压泵1", "Report_Car_zhongDiYa.rdlc" });

            using (OledbHelper helper = new OledbHelper())
            {
                if (ConstantValue.EquipemntList[index] == Equipment.Car)
                {   List<int> gklist = new List<int>(6);
                    for(int i = 0; i < ConstantValue.xfcInfos[index].IsGkCompleted.Count; i++)
                    {
                        if (ConstantValue.xfcInfos[index].IsGkCompleted[i])
                        {
                            gklist[i] = 1;
                        }
                        else
                        {
                            gklist[i] = 0;
                        }
                    }
                    PrintSqlGenerateHelper helper1 = new PrintSqlGenerateHelper(gklist);

                    switch (ConstantValue.PumpTypeList[index])
                    {
                        case PumpType.DiYaPump:
                            ShuJuYuan(helper1.Generate(), PrintList_car[0]);
                            break;
                        case PumpType.ZhongYaPump:
                        case PumpType.GaoYaPump:
                            ShuJuYuan(helper1.Generate(), PrintList_car[1]);
                            break;
                        case PumpType.ZhongDiYaPump:
                            ShuJuYuan(helper1.Generate(), PrintList_car[2]);
                            break;
                        case PumpType.GaoDiYaPump:
                            ShuJuYuan(helper1.Generate(), PrintList_car[3]);
                            break;

                    }
                }
                else if (ConstantValue.EquipemntList[index] == Equipment.Pump)
                {
                    List<int> gklist = new List<int>(6);
                    for (int i = 0; i < ConstantValue.xfbInfos[index].IsGkCompleted.Count; i++)
                    {
                        if (ConstantValue.xfbInfos[index].IsGkCompleted[i])
                        {
                            gklist[i] = 1;
                        }
                        else
                        {
                            gklist[i] = 0;
                        }
                    }
                    PrintSqlGenerateHelper_Pump helper1 = new PrintSqlGenerateHelper_Pump(gklist);
                    switch (ConstantValue.PumpTypeList[index])
                    {
                        case PumpType.DiYaPump:
                            ShuJuYuan(helper1.Generate(), PrintList_car[0]);
                            break;
                        case PumpType.ZhongYaPump:
                        case PumpType.GaoYaPump:
                            ShuJuYuan(helper1.Generate(), PrintList_car[1]);
                            break;
                        case PumpType.ZhongDiYaPump:
                            ShuJuYuan(helper1.Generate(), PrintList_car[2]);
                            break;
                        case PumpType.GaoDiYaPump:
                            ShuJuYuan(helper1.Generate(), PrintList_car[3]);
                            break;


                    }
                }
            }
        }
    }
}
