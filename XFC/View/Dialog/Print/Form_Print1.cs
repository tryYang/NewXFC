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
        /// 【高压泵】获取数据源函数
        /// </summary>
        /// <param name="sqlGao"></param>
        public void ShuJuYuanGao(string sqlGao)
        {
            string conn_String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\消防车性能测试系统\\Sql\\消防水力测试系统.mdb";
            OleDbConnection conn = new OleDbConnection(conn_String);  //Sql链接类的实例化  //数据库连接串

            try
            {
                //打开数据库
                conn.Open();

                string sql = sqlGao;
                OleDbDataAdapter ad = new OleDbDataAdapter(sql, conn);
                System.Data.DataSet ds1 = new System.Data.DataSet();
                ad.Fill(ds1);

                ReportDataSource rds = new ReportDataSource();
                rds.Name = "高压泵1";
                rds.Value = ds1.Tables[0];

                this.reportViewer1.LocalReport.ReportPath = "Report1.rdlc";
                // this.reportViewer1.LocalReport.ReportEmbeddedResource = "Report1.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(rds);
                this.reportViewer1.RefreshReport();
            }

            catch (Exception ex)
            {
                MessageBox.Show("查询错误！" + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    //关闭数据库连接
                    conn.Close();
                }

            }
        }
        /// <summary>
        ///【低压泵】获取数据源函数
        /// </summary>
        /// <param name="sqlDi"></param>
        public void ShuJuYuanDi(string sqlDi)
        {
            using (OledbHelper helper = new OledbHelper())
            {
                helper.sqlstring = sqlDi;
                System.Data.DataSet ds1 = helper.GetDataSet();
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "低压泵1";
                rds.Value = ds1.Tables[0];
                this.reportViewer1.LocalReport.ReportPath = "Report_Car_DiYa.rdlc";
                // this.reportViewer1.LocalReport.ReportEmbeddedResource = "Report1.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(rds);
                this.reportViewer1.RefreshReport();

            }


        }

        /// <summary>
        /// 1=低压工况；2=一点三工况；3=超负荷工况；4=半流量工况；5=高压工况；6=中高压工况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Print1_Load(object sender, EventArgs e)
        {
            using (OledbHelper helper = new OledbHelper())
            {
                List<int> gkList = new List<int> { 0,1 };
                PrintSqlGenerateHelper helper1 = new PrintSqlGenerateHelper(gkList);
                MessageBox.Show(helper1.Generate());
                ShuJuYuanDi(helper1.Generate());
            }
        }


    }
}
