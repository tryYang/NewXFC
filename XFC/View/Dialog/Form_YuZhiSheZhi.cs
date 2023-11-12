using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using XFC.Helper;
using XFC.Model;
using System.Web.UI;

namespace XFC.View.Dialog
{
    public partial class Form_YuZhiSheZhi : Form
    {
        // public Threshold threshold { get; set; } = new Threshold();
        public static Form_YuZhiSheZhi instance;
        public static Form_YuZhiSheZhi GetInstance()
        {
            if (instance == null)
            {
                instance = new Form_YuZhiSheZhi();
                return instance;
            }
            return instance;
        }
        public Form_YuZhiSheZhi()
        {
            InitializeComponent();
            DataShow();

        }
        /// <summary>
        /// 【取消】按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            instance?.Close();
        }

        /// <summary>
        /// 显示具体阈值数据
        /// </summary>
        public void DataShow()
        {
            using (OledbHelper helper = new OledbHelper())
            {

                //真空压力最大值
                helper.sqlstring = "select VacuumPressMax from Threshold";
                ConstantValue.threshold.VacuumPressMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_VacuumPressMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力最小值
                helper.sqlstring = "select VacuumPressMin from Threshold";
                ConstantValue.threshold.VacuumPressMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_VacuumPressMin.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select LowPressMax from Threshold";
                ConstantValue.threshold.LowPressMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_LowPressMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select LowPressMin from Threshold";
                ConstantValue.threshold.LowPressMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_LowPressMin.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select HighPressMax from Threshold";
                ConstantValue.threshold.HighPressMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_HighPressMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select HighPressMin from Threshold";
                ConstantValue.threshold.HighPressMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_HighPressMin.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select PumpSpeedMax from Threshold";
                ConstantValue.threshold.PumpSpeedMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_PumpSpeedMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select PumpSpeedMin from Threshold";
                ConstantValue.threshold.PumpSpeedMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_PumpSpeedMin.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select InTempMax from Threshold";
                ConstantValue.threshold.InTempMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_InTempMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select InTempMin from Threshold";
                ConstantValue.threshold.InTempMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_InTempMin.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select OutTempMax from Threshold";
                ConstantValue.threshold.OutTempMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_OutTempMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select OutTempMin from Threshold";
                ConstantValue.threshold.OutTempMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_OutTempMin.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [SpeedMax110kw] from Threshold";
                ConstantValue.threshold.SpeedMax110kw = Convert.ToDouble(helper.ExecuteScalar());
                tb_SpeedMax110kw.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [SpeedMin110kw] from Threshold";
                ConstantValue.threshold.SpeedMin110kw = Convert.ToDouble(helper.ExecuteScalar());
                tb_SpeedMin110kw.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [FlowmeterMax50] from Threshold";
                ConstantValue.threshold.FlowmeterMax50 = Convert.ToDouble(helper.ExecuteScalar());
                tb_FlowmeterMax50.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [FlowmeterMin50] from Threshold";
                ConstantValue.threshold.FlowmeterMin50 = Convert.ToDouble(helper.ExecuteScalar());
                tb_FlowmeterMin50.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ValveMax50] from Threshold";
                ConstantValue.threshold.ValveMax50 = Convert.ToDouble(helper.ExecuteScalar());
                tb_ValveMax50.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ValveMin50] from Threshold";
                ConstantValue.threshold.ValveMin50 = Convert.ToDouble(helper.ExecuteScalar());
                tb_ValveMin50.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [FlowmeterMax100] from Threshold";
                ConstantValue.threshold.FlowmeterMax100 = Convert.ToDouble(helper.ExecuteScalar());
                tb_FlowmeterMax100.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [FlowmeterMin100] from Threshold";
                ConstantValue.threshold.FlowmeterMin100 = Convert.ToDouble(helper.ExecuteScalar());
                tb_FlowmeterMin100.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ValveMax100] from Threshold";
                ConstantValue.threshold.ValveMax100 = Convert.ToDouble(helper.ExecuteScalar());
                tb_ValveMax100.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ValveMin100] from Threshold";
                ConstantValue.threshold.ValveMin100 = Convert.ToDouble(helper.ExecuteScalar());
                tb_ValveMin100.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [FlowmeterMax200] from Threshold";
                ConstantValue.threshold.FlowmeterMax200 = Convert.ToDouble(helper.ExecuteScalar());
                tb_FlowmeterMax200.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [FlowmeterMin200] from Threshold";
                ConstantValue.threshold.FlowmeterMin200 = Convert.ToDouble(helper.ExecuteScalar());
                tb_FlowmeterMin200.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ValveMax200] from Threshold";
                ConstantValue.threshold.ValveMax200 = Convert.ToDouble(helper.ExecuteScalar());
                tb_ValveMax200.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ValveMin200] from Threshold";
                ConstantValue.threshold.ValveMin200 = Convert.ToDouble(helper.ExecuteScalar());
                tb_ValveMin200.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [FlowmeterMax300] from Threshold";
                ConstantValue.threshold.FlowmeterMax300 = Convert.ToDouble(helper.ExecuteScalar());
                tb_FlowmeterMax300.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [FlowmeterMin300] from Threshold";
                ConstantValue.threshold.FlowmeterMin300 = Convert.ToDouble(helper.ExecuteScalar());
                tb_FlowmeterMin300.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ValveMax300] from Threshold";
                ConstantValue.threshold.ValveMax300 = Convert.ToDouble(helper.ExecuteScalar());
                tb_ValveMax300.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ValveMin300] from Threshold";
                ConstantValue.threshold.ValveMin300 = Convert.ToDouble(helper.ExecuteScalar());
                tb_ValveMin300.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [AirPressMax] from Threshold";
                ConstantValue.threshold.AirPressMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_AirPressMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [AirPressMin] from Threshold";
                ConstantValue.threshold.AirPressMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_AirPressMin.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [EnvironmentTempMax] from Threshold";
                ConstantValue.threshold.EnvironmentTempMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_EnvironmentTempMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [EnvironmentTempMin] from Threshold";
                ConstantValue.threshold.EnvironmentTempMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_EnvironmentTempMin.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ThreeDepthMax] from Threshold";
                ConstantValue.threshold.ThreeDepthMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_ThreeDepthMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ThreeDepthMin] from Threshold";
                ConstantValue.threshold.ThreeDepthMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_ThreeDepthMin.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ThreeTempMax] from Threshold";
                ConstantValue.threshold.ThreeTempMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_ThreeTempMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [ThreeTempMin] from Threshold";
                ConstantValue.threshold.ThreeTempMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_ThreeTempMin.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [SevenDepthMax] from Threshold";
                ConstantValue.threshold.SevenDepthMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_SevenDepthMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [SevenDepthMin] from Threshold";
                ConstantValue.threshold.SevenDepthMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_SevenDepthMin.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [SevenTempMax] from Threshold";
                ConstantValue.threshold.SevenTempMax = Convert.ToDouble(helper.ExecuteScalar());
                tb_SevenTempMax.Text = Convert.ToString(helper.ExecuteScalar());

                //真空压力
                helper.sqlstring = "select [SevenTempMin] from Threshold";
                ConstantValue.threshold.SevenTempMin = Convert.ToDouble(helper.ExecuteScalar());
                tb_SevenTempMin.Text = Convert.ToString(helper.ExecuteScalar());


            }
        }
        private bool checkinput()
        {


            return true;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!setThreshold())
            {
                MessageBox.Show("请检查阈值是否输入正确");
            }
            using (OledbHelper helper = new OledbHelper())
            {
                //删除阈值表数据
                helper.sqlstring = "delete * from Threshold";
                helper.ExecuteCommand();

                //填充阈值表数据
                helper.InsertData(ConstantValue.threshold);
                //填充占位符
                //helper.sqlstring = string.Format(helper.sqlstring, tb_VacuumPressMax.Text, tb_VacuumPressMin.Text, tb_LowPressMax.Text, tb_LowPressMin.Text, tb_HighPressMax.Text, tb_HighPressMin.Text, tb_PumpSpeedMax.Text, tb_PumpSpeedMin.Text, tb_InTempMax.Text, tb_InTempMin.Text, tb_OutTempMax.Text, tb_OutTempMin.Text, tb_SpeedMax110kw.Text, tb_SpeedMin110kw.Text, tb_FlowmeterMax50.Text, tb_FlowmeterMin50.Text, tb_ValveMax50.Text, tb_ValveMin50.Text, tb_FlowmeterMax100.Text, tb_FlowmeterMin100.Text, tb_ValveMax100.Text, tb_ValveMin100.Text, tb_FlowmeterMax200.Text, tb_FlowmeterMin200.Text, tb_ValveMax200.Text, tb_ValveMin200.Text, tb_FlowmeterMax300.Text, tb_FlowmeterMin300.Text, tb_ValveMax300.Text, tb_ValveMin300.Text, tb_AirPressMax.Text, tb_AirPressMin.Text, tb_EnvironmentTempMax.Text, tb_EnvironmentTempMin.Text, tb_ThreeDepthMax.Text, tb_ThreeDepthMin.Text, tb_ThreeTempMax.Text, tb_ThreeTempMin.Text, tb_SevenDepthMax.Text, tb_SevenDepthMin.Text, tb_SevenTempMax.Text, tb_SevenTempMin.Text);
                //DataSet ds = helper.GetDataSet();
                //helper.ExecuteCommand();
                MessageBox.Show("阈值修改成功！");
            }
        }

        private bool setThreshold()
        {

            if (double.TryParse(tb_VacuumPressMax.Text, out double result))
            {
                ConstantValue.threshold.VacuumPressMax = result;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_VacuumPressMin.Text, out double result1))
            {
                ConstantValue.threshold.VacuumPressMin = result1;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_LowPressMax.Text, out double result2))
            {
                ConstantValue.threshold.LowPressMax = result2;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_LowPressMin.Text, out double result3))
            {
                ConstantValue.threshold.LowPressMin = result3;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_HighPressMax.Text, out double result4))
            {
                ConstantValue.threshold.HighPressMax = result4;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_HighPressMin.Text, out double result5))
            {
                ConstantValue.threshold.HighPressMin = result5;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_PumpSpeedMax.Text, out double result6))
            {
                ConstantValue.threshold.PumpSpeedMax = result6;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_PumpSpeedMin.Text, out double result7))
            {
                ConstantValue.threshold.PumpSpeedMin = result7;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_InTempMax.Text, out double result8))
            {
                ConstantValue.threshold.InTempMax = result8;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_InTempMin.Text, out double result9))
            {
                ConstantValue.threshold.InTempMin = result9;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_OutTempMax.Text, out double result10))
            {
                ConstantValue.threshold.OutTempMax = result10;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_OutTempMin.Text, out double result11))
            {
                ConstantValue.threshold.OutTempMin = result11;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_SpeedMax110kw.Text, out double result12))
            {
                ConstantValue.threshold.SpeedMax110kw = result12;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_SpeedMin110kw.Text, out double result13))
            {
                ConstantValue.threshold.SpeedMin110kw = result13;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_FlowmeterMax50.Text, out double result14))
            {
                ConstantValue.threshold.FlowmeterMax50 = result14;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_FlowmeterMin50.Text, out double result15))
            {
                ConstantValue.threshold.FlowmeterMin50 = result15;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_ValveMax50.Text, out double result16))
            {
                ConstantValue.threshold.ValveMax50 = result16;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_ValveMin50.Text, out double result17))
            {
                ConstantValue.threshold.ValveMin50 = result17;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_FlowmeterMax100.Text, out double result18))
            {
                ConstantValue.threshold.FlowmeterMax100 = result18;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_FlowmeterMin100.Text, out double result19))
            {
                ConstantValue.threshold.FlowmeterMin100 = result19;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_ValveMax100.Text, out double result20))
            {
                ConstantValue.threshold.ValveMax100 = result20;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_ValveMin100.Text, out double result21))
            {
                ConstantValue.threshold.ValveMin100 = result21;
            }
            else
            {
                return false;
            }
            if (double.TryParse(tb_FlowmeterMax200.Text, out double result22))
            {
                ConstantValue.threshold.FlowmeterMax200 = result22;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_FlowmeterMin200.Text, out double result23))
            {
                ConstantValue.threshold.FlowmeterMin200 = result23;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_ValveMax200.Text, out double result24))
            {
                ConstantValue.threshold.ValveMax200 = result24;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_ValveMin200.Text, out double result25))
            {
                ConstantValue.threshold.ValveMin200 = result25;
            }
            else
            {
                return false;
            }
            if (double.TryParse(tb_FlowmeterMax300.Text, out double result26))
            {
                ConstantValue.threshold.FlowmeterMax300 = result26;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_FlowmeterMin300.Text, out double result27))
            {
                ConstantValue.threshold.FlowmeterMin300 = result27;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_ValveMax300.Text, out double result28))
            {
                ConstantValue.threshold.ValveMax300 = result28;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_ValveMin300.Text, out double result29))
            {
                ConstantValue.threshold.ValveMin300 = result29;
            }
            else
            {
                return false;
            }



            if (double.TryParse(tb_AirPressMax.Text, out double result30))
            {
                ConstantValue.threshold.AirPressMax = result30;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_AirPressMin.Text, out double result31))
            {
                ConstantValue.threshold.AirPressMin = result31;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_EnvironmentTempMax.Text, out double result32))
            {
                ConstantValue.threshold.EnvironmentTempMax = result32;
            }
            else
            {
                return false;

            }

            if (double.TryParse(tb_EnvironmentTempMin.Text, out double result33))
            {
                ConstantValue.threshold.EnvironmentTempMin = result33;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_ThreeDepthMax.Text, out double result34))
            {
                ConstantValue.threshold.ThreeDepthMax = result34;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_ThreeDepthMin.Text, out double result35))
            {
                ConstantValue.threshold.ThreeDepthMin = result35;
            }
            else
            {
                return false;
            }


            if (double.TryParse(tb_ThreeTempMax.Text, out double result36))
            {
                ConstantValue.threshold.ThreeTempMax = result36;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_ThreeTempMin.Text, out double result37))
            {
                ConstantValue.threshold.ThreeTempMin = result37;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_SevenDepthMax.Text, out double result38))
            {
                ConstantValue.threshold.SevenDepthMax = result38;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_SevenDepthMin.Text, out double result39))
            {
                ConstantValue.threshold.SevenDepthMin = result39;
            }
            else
            {
                return false;
            }

            if (double.TryParse(tb_SevenTempMax.Text, out double result40))
            {
                ConstantValue.threshold.SevenTempMax = result40;
            }
            else
            {
                return false;

            }


            if (double.TryParse(tb_SevenTempMin.Text, out double result41))
            {
                ConstantValue.threshold.SevenTempMin = result41;
            }
            else
            {
                return false;
            }


            return true;
        }
    }
}
