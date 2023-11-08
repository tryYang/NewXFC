using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XFC.Helper;
using XFC.Model;
using XFC.View;
using XFC.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace XFC.View
{
    public partial class Form_ShiYanCanShu : Form
    {

        //System.Windows.Forms.ComboBox.SelectedItem{get;set;};\
        private XfcTestViewModel viewModel;
        private BindingSource bindingSource;

        private static Form_ShiYanCanShu instance;
        public static Form_ShiYanCanShu GetInstance()
        {
            if(instance == null)
            {
                instance = new Form_ShiYanCanShu();
                return instance;
            }
            else 
                return instance;
        }
        public Form_ShiYanCanShu()
        {
            InitializeComponent();
            viewModel = new XfcTestViewModel();
            bindingSource = new BindingSource();
            // 将BindingSource与ViewModel绑定
            bindingSource.DataSource = viewModel;
            waterpumptype.DataSource = ConstantValue.WaterPumpType;
            cbx_Diya.DataSource = ConstantValue.L_Flowmeter;
            cbx_Gaoya.DataSource = ConstantValue.H_Flowmeter;
            cbx_CarId.DataSource = ConstantValue.ID;
            btn_confirm.Click += (sender, e) => viewModel.ConfirmCommand.Execute(null);
            btn_cancel.Click += (sender, e) => viewModel.CancelCommand.Execute(null);
           

        }

        private void waterpumptype_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (waterpumptype.SelectedIndex == 0)
            {
                panelLow.Visible=true;
                panelLowtype.Visible = true;
                panelHightype.Visible=false;
                panelHigh.Visible = false;
                
            }
            else if (waterpumptype.SelectedIndex == 1 || waterpumptype.SelectedIndex == 2)
            {
                panelLow.Visible = false;
                panelLowtype.Visible = false;
                panelHightype.Visible = true;
                panelHigh.Visible = true;
            }
            else
            {
                panelLow.Visible = true;
                panelLowtype.Visible = true;
                panelHightype.Visible = true;
                panelHigh.Visible = true;
            }

        }


        /// <summary>
        /// 【确认】按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_confirm_Click(object sender, EventArgs e)
        {
           
            if (!CheckInput())
            {
                MessageBox.Show("请在红色控件处输入正确的值！");
                return;
            }
            else if (tb_UnderpanVIN.Text.Length!=17)
            {
                MessageBox.Show("底盘VIN必须为17位");
                return;
            }
            else if(cbx_CarId.Text!="1"&& cbx_CarId.Text != "2")
            {
                MessageBox.Show("使用车辆Id只能为1和2");
                return;
            }
            int index = int.Parse(cbx_CarId.Text) - 1;

           
            ConstantValue.xfcInfos[index].carBasicInfo.CarID = GetCarId();
            ConstantValue.xfcInfos[index].carBasicInfo.CarName = cbx_CarName.Text;
            ConstantValue.xfcInfos[index].carBasicInfo.CarFac = cbx_carfac.Text;
            ConstantValue.xfcInfos[index].carBasicInfo.CarProduceTime = cbx_producetime.Value;
            ConstantValue.xfcInfos[index].carBasicInfo.UnderpanFac = cbx_under.Text;
            ConstantValue.xfcInfos[index].carBasicInfo.PumpFac = cbx_pumpfac.Text;
            ConstantValue.xfcInfos[index].carBasicInfo.CarNum = txt_num.Text;
            ConstantValue.xfcInfos[index].carBasicInfo.CarModel = cbx_cartype.Text;
            ConstantValue.xfcInfos[index].carBasicInfo.UnderpanModel = cbx_under.Text;
            ConstantValue.xfcInfos[index].carBasicInfo.UnderpanVIN = tb_UnderpanVIN.Text;
            ConstantValue.xfcInfos[index].carBasicInfo.PumpModel = cbx_pumpmodel.Text;
            ConstantValue.xfcInfos[index].carBasicInfo.PumpType = waterpumptype.Text;
            if (waterpumptype.Text.Equals("低压泵")|| waterpumptype.Text.Equals("中低压泵") || waterpumptype.Text.Equals("高低压泵"))
            {
                ConstantValue.xfcInfos[index].carBasicInfo.L_RatedFlow = Double.Parse(txt_lowflow.Text);
                ConstantValue.xfcInfos[index].carBasicInfo.L_RatedPress = Double.Parse(txt_lowp.Text);
            }
            if (!waterpumptype.Text.Equals("低压泵") )
            {
                ConstantValue.xfcInfos[index].carBasicInfo.H_RatedFlow = Double.Parse(txt_highflow.Text);
                ConstantValue.xfcInfos[index].carBasicInfo.H_RatedPress = Double.Parse(txt_highp.Text);
            }
                

            ConstantValue.xfcInfos[index].carLab.LabID = GetLabId();
            ConstantValue.xfcInfos[index].carLab.CarID = GetCarId();
            ConstantValue.xfcInfos[index].carLab.CustomerDepart = cbx_CustomerDepart.Text;
            ConstantValue.xfcInfos[index].carLab.LabDate = LabTime.Value;
            ConstantValue.xfcInfos[index].carLab.CheckPeople = tb_UserPeople.Text;
            ConstantValue.xfcInfos[index].carLab.L_Flowmeter = cbx_Diya.Text;
            ConstantValue.xfcInfos[index].carLab.H_Flowmeter = cbx_Gaoya.Text;

            ConstantValue.xfcInfos[index].carLab.ThreePress = double.Parse(ud4.Value.ToString());
            ConstantValue.xfcInfos[index].carLab.ThreeTemp= double.Parse(ud3.Value.ToString());
            ConstantValue.xfcInfos[index].carLab.SevenPress=double.Parse(ud7.Value.ToString());
            ConstantValue.xfcInfos[index].carLab.SevenTemp= double.Parse(ud6.Value.ToString());
            ConstantValue.xfcInfos[index].carLab.CarHeight = double.Parse(ud2.Value.ToString());
            ConstantValue.xfcInfos[index].carLab.Temp = double.Parse(ud9.Value.ToString());
            ConstantValue.xfcInfos[index].carLab.Pressure = double.Parse(ud1.Value.ToString());
            ConstantValue.xfcInfos[index].dic_Flowtype[FlowType.DN100] = false;
            ConstantValue.xfcInfos[index].dic_Flowtype[FlowType.DN200] = false;
            ConstantValue.xfcInfos[index].dic_Flowtype[FlowType.DN300] = false;
            ConstantValue.xfcInfos[index].dic_Flowtype[FlowType.DN50] = false;



            if (cbx_Diya.Visible)
            {
                if (cbx_Diya.Text == "DN100")
                {
                    ConstantValue.xfcInfos[index].dic_Flowtype[FlowType.DN100] = true;
                    ConstantValue.liuliangjiAndFlowtype[index][0] = FlowType.DN100;

                }
                else if (cbx_Diya.Text == "DN200")
                {
                    ConstantValue.xfcInfos[index].dic_Flowtype[FlowType.DN200] = true;
                    ConstantValue.liuliangjiAndFlowtype[index][0] = FlowType.DN200;

                }
                else if (cbx_Diya.Text == "DN300")
                {
                    ConstantValue.xfcInfos[index].dic_Flowtype[FlowType.DN300] = true;
                    ConstantValue.liuliangjiAndFlowtype[index][0] =FlowType.DN300;
                }

            }
            if (cbx_Gaoya.Visible)
            {
                if (cbx_Diya.Text == "DN100")
                {
                    ConstantValue.xfcInfos[index].dic_Flowtype[FlowType.DN100] = true;
                    ConstantValue.liuliangjiAndFlowtype[index][1] = FlowType.DN100;

                }
                else if (cbx_Diya.Text == "DN50")
                {
                    ConstantValue.xfcInfos[index].dic_Flowtype[FlowType.DN50] = true;
                    ConstantValue.liuliangjiAndFlowtype[index][1] = FlowType.DN50;

                }

            }
            
            SetMainWindowValue();

            ConstantValue.xfcInfos[index].IsChecked = true;
            ConstantValue.gkStatus = GkStatus.Checked;
            ConstantValue.EquipemntList[index] = Equipment.Car;
            switch (waterpumptype.SelectedIndex) { 
                case 0:
                    ConstantValue.PumpTypeList[index] = PumpType.DiYaPump;
                     break;
                case 1:
                    ConstantValue.PumpTypeList[index] = PumpType.ZhongYaPump;
                    break;
                case 2:
                    ConstantValue.PumpTypeList[index] = PumpType.GaoYaPump;
                    break;
                case 3:
                    ConstantValue.PumpTypeList[index] = PumpType.ZhongDiYaPump;
                    break;
                case 4:
                    ConstantValue.PumpTypeList[index] = PumpType.GaoDiYaPump;
                    break;
                
            }

            this.Close();
            Form_Main.getInstance().Tb_Tip.AppendText("消防车试验添加成功");
        }
        private void SetMainWindowValue()
        {
            Form_Main form_Main = Form_Main.getInstance();
            form_Main.Pressure.Text = ud1.Value.ToString();
            form_Main.Temp.Text = ud9.Value.ToString();
            form_Main.Temp_3m.Text = ud3.Value.ToString();
            form_Main.Temp_7m.Text = ud6.Value.ToString();
            form_Main.Depth_3m.Text = ud4.Value.ToString();
            form_Main.Depth_7m.Text = ud7.Value.ToString();

        }
        private int GetCarId()
        {
            return 0;
        }
        private int GetLabId()
        {
            return 0;
        }
        /// <summary>
        /// 检测红色控件输入是否正确
        /// </summary>
        /// <returns>  </returns>
        private bool CheckInput()
        {
            bool baseinfo = cbx_CustomerDepart.Text.Trim() ==string.Empty||cbx_CarName.Text.Trim() ==string.Empty||tb_UnderpanVIN.Text.Trim() ==string.Empty||cbx_CarId.Text.Trim() ==string.Empty||tb_UserPeople.Text.Trim() ==string.Empty;
            bool diya = txt_lowflow.Text.Trim() ==string.Empty||txt_lowp.Text.Trim() ==string.Empty||cbx_Diya.Text.Trim() ==string.Empty;
            bool gaoya = txt_highflow.Text.Trim() ==string.Empty||txt_highp.Text.Trim() ==string.Empty||cbx_Gaoya.Text.Trim() ==string.Empty;
            if (waterpumptype.Text == null)
            {
                return false;
            }
            else if(waterpumptype.Text == "低压泵")
            {
                if (baseinfo||diya)
                {
                    return false;
                }               
            }
            else if(waterpumptype.Text == "高压泵"||waterpumptype.Text == "中压泵")
            {
                if (baseinfo||gaoya)
                {
                    return false;
                }               
            }
            else if(waterpumptype.Text == "高低压泵"||waterpumptype.Text == "中低压泵")
            {
                if (baseinfo||diya||gaoya)
                {
                    return false;
                }              
            }
           
            return true;
        }
        /// <summary>
        /// 从数据库中提取数据到comboBox控件中
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="a"></param>控件名称
        private void load(Dictionary<string, Control> dic)
        {
            using (OledbHelper helper = new OledbHelper())
            {

                foreach (KeyValuePair<string, Control> kvp in dic)
                {
                    ArrayList list = new ArrayList();
                    helper.sqlstring = kvp.Key;
                    DataSet ds = helper.GetDataSet();
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        //dr[0]表示取结果的第一列，dr[1]就是第二列
                        list.Add(dr[0].ToString().Trim());
                    }
                    System.Windows.Forms.ComboBox a = (System.Windows.Forms.ComboBox)kvp.Value;
                    a.DataSource = list;

                }
            }

        }
        /// <summary>
        /// 初始化控件数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_ShiYanCanShu_Load(object sender, EventArgs e)
        {
            Dictionary<string, Control> dic = new Dictionary<string, Control>();
            dic.Add("select distinct CarName from CarBasicInfo",cbx_CarName);    //车辆名称
            dic.Add("select distinct CarFac from CarBasicInfo",cbx_carfac);  //车辆厂家
            dic.Add("select distinct UnderpanFac from CarBasicInfo",cbx_underfac); //底盘厂家
            dic.Add("select distinct PumpFac from CarBasicInfo",cbx_pumpfac); //水泵厂家
            dic.Add("select distinct CustomerDepart from CarLab", cbx_CustomerDepart);  //送检单位
            //dic.Add("select distinct CarProduceTime	from CarBasicInfo",dateTimePicker2);  //生产日期
            //dic.Add("select distinct PumpType from CarBasicInfo",waterpumptype);  //水泵类型
            dic.Add("select distinct CarModel from CarBasicInfo",cbx_cartype);  //车辆型号
            dic.Add("select distinct UnderpanModel from	CarBasicInfo",cbx_under);  //底盘型号
            dic.Add("select distinct PumpModel from	CarBasicInfo",cbx_pumpmodel);  //水泵型号                                                                      
           
            load(dic);

        }
    }  
}
