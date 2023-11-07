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
    public partial class Form_ShiYanCanShu_xfb : Form
    {

        //System.Windows.Forms.ComboBox.SelectedItem{get;set;};\
        private XfcTestViewModel viewModel;
        private BindingSource bindingSource;

        private static Form_ShiYanCanShu_xfb instance;
        public static Form_ShiYanCanShu_xfb GetInstance()
        {
            if(instance == null)
            {
                instance = new Form_ShiYanCanShu_xfb();
                return instance;
            }
            else 
                return instance;
        }
        public Form_ShiYanCanShu_xfb()
        {
            InitializeComponent();
            viewModel = new XfcTestViewModel();
            bindingSource = new BindingSource();
            // 将BindingSource与ViewModel绑定
            bindingSource.DataSource = viewModel;
            waterpumptype.DataSource = ConstantValue.WaterPumpType;
            cbx_Diya.DataSource = ConstantValue.L_Flowmeter;
            cbx_Gaoya.DataSource = ConstantValue.H_Flowmeter;
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
            else if (tb_Speed.Text.Length!=17)
            {
                MessageBox.Show("底盘VIN必须为17位");
                return;
            }
            int index = int.Parse(cbx_PumpId.Text) - 1;

                     
            ConstantValue.xfbInfos[index].pumpBasicInfo.PumpID = GetPumpId(); 
            ConstantValue.xfbInfos[index].pumpBasicInfo.PumpName = cbx_PumpName.Text;               //水泵名称
            ConstantValue.xfbInfos[index].pumpBasicInfo.PumpFac = cbx_pumpfac.Text;                 //水泵厂家
            ConstantValue.xfbInfos[index].pumpBasicInfo.PumpProduceTime = cbx_PumpProduceTime.Value;//水泵生产日期
            ConstantValue.xfbInfos[index].pumpBasicInfo.EpitopeDifference = cbx_EpitopeDifference.Text;   //表位差
            ConstantValue.xfbInfos[index].pumpBasicInfo.InPipeD = tb_InPipeD.Text;                      //进口管径
            ConstantValue.xfbInfos[index].pumpBasicInfo.OutPipeD = tb_OutPipeD.Text;                    //出口管径
            ConstantValue.xfbInfos[index].pumpBasicInfo.Speed = tb_Speed.Text;                          //额定转速
            ConstantValue.xfbInfos[index].pumpBasicInfo.PumpModel = cbx_PumpModel.Text;                 //水泵型号
            ConstantValue.xfbInfos[index].pumpBasicInfo.PumpType = waterpumptype.Text;                  //水泵类型

            ConstantValue.xfbInfos[index].pumpLab.PumpLabID = GetPumpLabId();
            ConstantValue.xfbInfos[index].pumpLab.PumpID = GetPumpId();
            ConstantValue.xfbInfos[index].pumpLab.CustomerDepart = cbx_CustomerDepart.Text;
            ConstantValue.xfbInfos[index].pumpLab.LabDate = LabTime.Value;
            ConstantValue.xfbInfos[index].pumpLab.CheckPeople = tb_UserPeople.Text;
            ConstantValue.xfbInfos[index].pumpLab.L_Flowmeter = cbx_Diya.Text;
            ConstantValue.xfbInfos[index].pumpLab.H_Flowmeter = cbx_Gaoya.Text;
            ConstantValue.xfbInfos[index].pumpLab.ThreePress = double.Parse(ud4.Value.ToString());
            ConstantValue.xfbInfos[index].pumpLab.ThreeTemp= double.Parse(ud3.Value.ToString());
            ConstantValue.xfbInfos[index].pumpLab.SevenPress=double.Parse(ud7.Value.ToString());
            ConstantValue.xfbInfos[index].pumpLab.SevenTemp= double.Parse(ud6.Value.ToString());
            ConstantValue.xfbInfos[index].pumpLab.CarHeight = double.Parse(ud2.Value.ToString());
            ConstantValue.xfbInfos[index].pumpLab.Temp = double.Parse(ud9.Value.ToString());
            ConstantValue.xfbInfos[index].pumpLab.Pressure = double.Parse(ud1.Value.ToString());

            ConstantValue.xfbInfos[index].dic_Flowtype[FlowType.DN100] = false;
            ConstantValue.xfbInfos[index].dic_Flowtype[FlowType.DN200] = false;
            ConstantValue.xfbInfos[index].dic_Flowtype[FlowType.DN300] = false;
            ConstantValue.xfbInfos[index].dic_Flowtype[FlowType.DN50] = false;



            if (cbx_Diya.Visible)
            {
                if (cbx_Diya.Text == "DN100")
                {
                    ConstantValue.xfbInfos[index].dic_Flowtype[FlowType.DN100] = true;
                }
                else if (cbx_Diya.Text == "DN200")
                {
                    ConstantValue.xfbInfos[index].dic_Flowtype[FlowType.DN200] = true;
                }
                else if (cbx_Diya.Text == "DN300")
                {
                    ConstantValue.xfbInfos[index].dic_Flowtype[FlowType.DN300] = true;
                }

            }
            if (cbx_Diya.Visible)
            {
                if (cbx_Diya.Text == "DN100")
                {
                    ConstantValue.xfbInfos[index].dic_Flowtype[FlowType.DN100] = true;
                }
                else if (cbx_Diya.Text == "DN50")
                {
                    ConstantValue.xfbInfos[index].dic_Flowtype[FlowType.DN50] = true;
                }
                
            }

            ConstantValue.xfbInfos[index].IsChecked = true;
            ConstantValue.gkStatus = GkStatus.Checked;
            ConstantValue.EquipemntList[index] = Equipment.Pump;
            switch (waterpumptype.SelectedIndex)
            {
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
            SetMainWindowValue();
           
            this.Close();
            Form_Main.getInstance().Tb_Tip.AppendText("消防泵试验添加成功");
        }

        private void SetMainWindowValue()
        {
            
        }

        private int GetPumpId()
        {
            return 0;
        }
        private int GetPumpLabId()
        {
            return 0;
        }
        /// <summary>
        /// 检测红色控件输入是否正确
        /// </summary>
        /// <returns>  </returns>
        private bool CheckInput()
        {
            bool baseinfo = cbx_CustomerDepart.Text.Trim() ==string.Empty||cbx_PumpName.Text.Trim() ==string.Empty||tb_Speed.Text.Trim() ==string.Empty||cbx_PumpId.Text.Trim() ==string.Empty||tb_UserPeople.Text.Trim() ==string.Empty;
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
            dic.Add("select distinct CustomerDepart from	PumpLab", cbx_CustomerDepart);  //送检单位
            dic.Add("select distinct PumpName from PumpBasicInfo", cbx_PumpName);    //水泵名称
            dic.Add("select distinct PumpFac from PumpBasicInfo", cbx_pumpfac);  //水泵厂家            
            dic.Add("select distinct Speed  from PumpBasicInfo", tb_Speed);  //额定转速     
            //dic.Add("select distinct CarProduceTime	from CarBasicInfo",dateTimePicker2);  //生产日期
            //dic.Add("select distinct PumpType from CarBasicInfo",waterpumptype);  //水泵类型
            dic.Add("select distinct EpitopeDifference  from PumpBasicInfo", tb_Speed);  //表位差    
            dic.Add("select distinct PumpModel from	PumpBasicInfo ", cbx_PumpModel);  //水泵型号                                                                      
            dic.Add("select distinct InPipeD  from	PumpBasicInfo ", tb_OutPipeD);//出口管径
            dic.Add("select distinct OutPipeD  from	PumpBasicInfo ", tb_InPipeD);//进口管径
            load(dic);

        }
    }  
}
