using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using Modbus.Device;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.OleDb;
using System.Runtime.CompilerServices;
using System.Drawing.Printing;
using Excel = Microsoft.Office.Interop.Excel;
using XFC;
using XFC.ViewModel;
using XFC.Helper;
using XFC.Model;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Timers;
using Microsoft.Office.Interop.Excel;
using TextBox = System.Windows.Forms.TextBox;
using log4net;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Web;
using System.Runtime.Remoting.Messaging;

namespace XFC.View
{
    public partial class Form_Main : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Form_Main));

        private MainWindowViewModel viewModel;
        private static Form_Main instance;
        private List<TextBox> DNcontrols1;
        private List<TextBox> DNcontrols2;
        GridPrinter gridPrinter;
        private bool isDatagridViewShowRealTime1 = true;
        private bool isDatagridViewShowRealTime2 = true;
        private bool isDatagridViewShowAlarmRealTime = true;
        bool alarm1 = false;
        bool alarm2 = false;

        System.Data.DataTable dataTable1 = CreateDataTable("设备1");
        System.Data.DataTable dataTable2 = CreateDataTable("设备2");
        System.Data.DataTable dataTable_alarm = CreateAlarmTable("报警记录表");
        public static Form_Main getInstance()
        {
            if (instance == null)
            {
                instance = new Form_Main();
                return instance;
            }
            else
                return instance;
        }
        
        public Form_Main()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            //Show Event
            tp_xfc_test.Click+= (sender, e) => viewModel.XfcTestClickCommand.Execute(null);
            tp_xfb_test.Click += (sender, e) => viewModel.XfbTestClickCommand.Execute(null);
            tp_threshold.Click += (sender, e) => viewModel.thresholdClickCommand.Execute(null);
            tp_userinfo.Click += (sender, e) => viewModel.UserInfoClickCommand.Execute(null);
            tp_xfcproduct.Click += (sender, e) => viewModel.XfcProductClickCommand.Execute(null);
            tp_xfbproduct.Click += (sender, e) => viewModel.XfbProductClickCommand.Execute(null);
            tp_clientinfo.Click += (sender, e) => viewModel.ClientInfoClickCommand.Execute(null);
            openPumptest.Click += (sender, e) => viewModel.OpenXfbTestClickCommand.Execute(null);
            openCartest.Click += (sender, e) => viewModel.OpenXfcTestClickCommand.Execute(null);


            //工况
            tp_gkchoose.Click += (sender, e) => viewModel.GkChooseCommand.Execute(null);
            tp_gkzhanting.Click += (sender, e) => viewModel.GkPauseCommand.Execute(null);
            tp_gkrun.Click += (sender, e) => viewModel.GkRunCommand.Execute(null);
            tp_gkexit.Click += (sender, e) => viewModel.GkExitCommand.Execute(null);
            
            //打印
            tp_printtable1.Click += (sender, e) => viewModel.PrintClickCommand1.Execute(null);
            tp_printtable2.Click += (sender, e) => viewModel.PrintClickCommand2.Execute(null);

            //退出
            tp_exit.Click += (sender, e) => viewModel.ExitClickCommand.Execute(null);
            this.FormClosed += (sender, e) => viewModel.ExitClickCommand.Execute(null);

            test_exit1.Click += (sender, e) => viewModel.TestExitClickCommand1.Execute(null);
            test_exit2.Click += (sender, e) => viewModel.TestExitClickCommand2.Execute(null);

            cmb_Baudrate.DataSource = ConstantValue.Baurates;
            cmb_Parity.DataSource = Enum.GetValues(typeof(Parity));
            cmb_StopBits.DataSource = new List<string> { "1","2"};
            cmb_PortNames.DataSource = SerialPort.GetPortNames();
            var textBoxes = this.Controls.OfType<System.Windows.Forms.TextBox>();
            DNcontrols1 = new List<TextBox>() { DN50Flow1, DN50Value1, DN100Flow1, DN100Value1, DN200Flow1, DN200Value1, DN300Flow1, DN300Value1 };
            DNcontrols2 = new List<TextBox>() { DN50Flow2, DN50Value2, DN100Flow2, DN100Value2, DN200Flow2, DN200Value2, DN300Flow2, DN300Value2 };

            
            dataGridView1.DataSource = dataTable1;
            dataGridView2.DataSource = dataTable2;
            dataGridView4.DataSource = dataTable_alarm;



        }


        private void btn_Connect_Click(object sender, EventArgs e)
        {
            if (ConstantValue.gkStatus == GkStatus.Uncheck)
                MessageBox.Show("请先新建试验");
            if (ConstantValue.gkStatus == GkStatus.Checked)
                MessageBox.Show("请先选择工况");
           
            if (ConstantValue.gkStatus != GkStatus.Selected)
            {
                if (ConstantValue.gkStatus == GkStatus.Stop)
                {
                   
                    ConstantValue.DataShowTimer.Start();
                    Tb_Tip.AppendText("工况开始运行......\n");
                    ConstantValue.gkStatus = GkStatus.Run;
                }

                return;
            }

            //if (NModubs4Helper.Instance.PortIsOpen())
            //{

            //}
            InitSerialPort();
            if (NModubs4Helper.Instance.Open())
            {

                Save2Table(0);
                Save2Table(1);
                initDataTimer();
                initChart();
                
            }
          
        }

        private void Save2Table(int i)
        {
            if (ConstantValue.EquipemntList[i] == Equipment.None)
            {
                return;
            }
            else if (ConstantValue.EquipemntList[i] == Equipment.Car)
            {
                using(OledbHelper helper=new OledbHelper())
                {
                    helper.InsertData(ConstantValue.xfcInfos[i].carBasicInfo);
                    helper.InsertData(ConstantValue.xfcInfos[i].carLab);
                }
               
            }
            else if (ConstantValue.EquipemntList[i] == Equipment.Pump)
            {
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.InsertData(ConstantValue.xfbInfos[i].pumpBasicInfo);
                    helper.InsertData(ConstantValue.xfbInfos[i].pumpLab);
                }
            }
        }

        private void InitSerialPort()
        {
            SerialPortParams _params = new SerialPortParams();
            _params.BaudRate = int.Parse(cmb_Baudrate.Text);

            _params.DataBits = int.Parse(txt_DataBits.Text);
            _params.serialPortName = cmb_PortNames.Text;
            _params.StopBits = (StopBits)int.Parse(cmb_StopBits.Text);
            if (cmb_Parity.Text.Equals("None"))
                _params.Parity = System.IO.Ports.Parity.None;
            else if (cmb_Parity.Text.Equals("Odd"))
                _params.Parity = System.IO.Ports.Parity.Odd;
            else if (cmb_Parity.Text.Equals("Even"))
                _params.Parity = System.IO.Ports.Parity.Even;
            else if (cmb_Parity.Text.Equals("Mark"))
                _params.Parity = System.IO.Ports.Parity.Mark;
            else if (cmb_Parity.Text.Equals("Space"))
                _params.Parity = System.IO.Ports.Parity.Space;

            NModubs4Helper.Instance = new NModubs4Helper(_params);
        }
        private  void OnTimedDataShow(Object source, ElapsedEventArgs e)
        {
            
          

                //Task.Run(() => DataShow(0, ConstantValue.EquipemntList[0]));
                //Task.Run(() => DataShow(1, ConstantValue.EquipemntList[1]));
                DataShow(0, ConstantValue.EquipemntList[0]);
                DataShow(1, ConstantValue.EquipemntList[1]);
                DateTime time = DateTime.Now;
           

            
            Console.WriteLine("定时器触发的事件在 {0:HH:mm:ss.fff} 执行", e.SignalTime);
        }
        /// <summary>
        /// 报警记录与展示函数
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="equipmentType"></param>
        /// <param name="alarmMessage"></param>
        private void alarming(int conditionID, string equipmentType, string alarmMessage)
        {
            if (isDatagridViewShowAlarmRealTime)
            {
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "insert into AlarmRecord ([ConditionID],[EquipmentType],[AlarmMessage],[SpecificCollectTime]) values ('{0}','{1}','{2}','{3}')";
                    string time = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                    object[] alarmrecords = new object[4];
                    alarmrecords[0] = conditionID;
                    alarmrecords[1] = equipmentType;
                    alarmrecords[2] = alarmMessage;
                    alarmrecords[3] = time;
                    dataTable_alarm.Rows.Add(alarmrecords);
                    dataGridView4.DataSource = dataTable1;
                    dataGridView4.Refresh();
                    //填充占位符
                    helper.sqlstring = string.Format(helper.sqlstring, conditionID, equipmentType, alarmMessage, time);
                    helper.ExecuteCommand();

                }

            }
           
        }
        /// <summary>
        /// 显示设备的实时数据
        /// </summary>
        /// <param name="i"></param>
        /// <param name="eq"></param>
        private void DataShow(int i , Equipment eq)
        {
            List<string> ERROR1 = new List<string>();
            double ThreeDepth = ValueConverter.ThreeDepthConverter(NModubs4Helper.Instance.GetValue16(3, 0));//水位3米
            double ThreeTemp = ValueConverter.ThreeTempConverter(NModubs4Helper.Instance.GetValue16(3, 1));//水温3米
            double Pressure0 = ValueConverter.PressureConverter(NModubs4Helper.Instance.GetValue16(3, 2));//大气压力
            double Temp0 = ValueConverter.Temp0Converter(NModubs4Helper.Instance.GetValue16(3, 3));//环境温度
            double SevenPress = ValueConverter.SevenDepthConverter(NModubs4Helper.Instance.GetValue16(3, 4));//水位7米
            double SevenTemp = ValueConverter.SevenTempConverter(NModubs4Helper.Instance.GetValue16(3, 5));//水温7米
            High_3m.Text = ThreeDepth.ToString();//真空度                   
            Temp_3m.Text = ThreeTemp.ToString();//低压压力
            Pressure.Text = Pressure0.ToString();//中高压压力
            Temp.Text = Temp0.ToString();//车载泵转速
            High_7m.Text = SevenPress.ToString();//输入轴温度
            Temp_7m.Text = SevenTemp.ToString();//输出轴温度
                                                //水位3米
            if (ThreeDepth < ConstantValue.threshold.ThreeDepthMin || ThreeDepth > ConstantValue.threshold.ThreeDepthMax)
            {
                ERROR1.Add("水位3米异常，异常值：" + High_3m.Text);
                alarm1 = true;
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                    int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                    string equipmentType = "消防车";
                    string alarmMessage = "【设备1】水位3米异常，异常值：" + High_3m.Text;
                    alarming(conditionID, equipmentType, alarmMessage);
                }
            }
            //水温3米
            if (ThreeTemp < ConstantValue.threshold.ThreeTempMin || ThreeTemp > ConstantValue.threshold.ThreeTempMax)
            {
                ERROR1.Add("水温3米异常，异常值：" + Temp_3m.Text);
                alarm1 = true;
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                    int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                    string equipmentType = "消防车";
                    string alarmMessage = "【设备1】水温3米异常，异常值：" + Temp_3m.Text;
                    alarming(conditionID, equipmentType, alarmMessage);
                }
            }
            //大气压力
            if (Pressure0 < ConstantValue.threshold.AirPressMin || Pressure0 > ConstantValue.threshold.AirPressMax)
            {
                ERROR1.Add("大气压力异常，异常值：" + Pressure.Text);
                alarm1 = true;
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                    int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                    string equipmentType = "消防车";
                    string alarmMessage = "【设备1】大气压力异常，异常值：" + Pressure.Text;
                    alarming(conditionID, equipmentType, alarmMessage);
                }
            }
            //环境温度
            if (Temp0 < ConstantValue.threshold.EnvironmentTempMin || Temp0 > ConstantValue.threshold.EnvironmentTempMax)
            {
                ERROR1.Add("环境温度异常，异常值：" + Temp.Text);
                alarm1 = true;
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                    int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                    string equipmentType = "消防车";
                    string alarmMessage = "【设备1】环境温度异常，异常值：" + Temp.Text;
                    alarming(conditionID, equipmentType, alarmMessage);
                }
            }
            //水位7米
            if (SevenPress < ConstantValue.threshold.SevenDepthMin || SevenPress > ConstantValue.threshold.SevenDepthMax)
            {
                ERROR1.Add("水位7米异常，异常值：" + High_7m.Text);
                alarm1 = true;
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                    int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                    string equipmentType = "消防车";
                    string alarmMessage = "【设备1】水位7米异常，异常值：" + High_7m.Text;
                    alarming(conditionID, equipmentType, alarmMessage);
                }
            }
            //水温7米
            if (SevenTemp < ConstantValue.threshold.SevenTempMin || SevenTemp > ConstantValue.threshold.SevenTempMax)
            {
                ERROR1.Add("水温7米异常，异常值：" + Temp_7m.Text);
                alarm1 = true;
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                    int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                    string equipmentType = "消防车";
                    string alarmMessage = "【设备1】水温7米异常，异常值：" + Temp_7m.Text;
                    alarming(conditionID, equipmentType, alarmMessage);
                }
            }










            if (eq == Equipment.Car && ConstantValue.xfcInfos[i].currentGk!=Gk.None)
            {
                this.Invoke(new System.Action(() => {
                    DataShow_xfc(i);
                }));

            }
            else if (eq == Equipment.Pump && ConstantValue.xfbInfos[i].currentGk != Gk.None)
            {
                this.Invoke(new System.Action(() => {

                    DataShow_xfb(i);
                }));

            }
            ConstantValue.Tick_Num++;

            return;

        }
        /// <summary>
        ///显示消防车的实时数据
        /// </summary>
        /// <param name="i"></param>
        private void DataShow_xfc(int i)
        {
            Gk gk = ConstantValue.xfcInfos[i].currentGk;
       
            switch (i) {
     
                /**************************z【值转换与报警】****************************/
                case 0:
                    List<string> ERROR1 = new List<string>();
                    double Vacuum = ValueConverter.RealPressConverter(NModubs4Helper.Instance.GetValue16(1, 0));
                    double LPress = ValueConverter.LPressConverter(NModubs4Helper.Instance.GetValue16(1, 1));
                    double HPress = ValueConverter.LHPressConverter(NModubs4Helper.Instance.GetValue16(1, 2));
                    double CarPumpSpeed = ValueConverter.PumpSpeedConverter(NModubs4Helper.Instance.GetValue16(1, 3));
                    double InTemp = ValueConverter.InTempConverter(NModubs4Helper.Instance.GetValue16(1, 4));
                    double OutTemp = ValueConverter.OutTempConverter(NModubs4Helper.Instance.GetValue16(1, 5));                  
                    Vacuum1.Text = tb_Vacuum1.Text = Vacuum.ToString();//真空度                   
                    LPress1.Text = tb_LPress1.Text = LPress.ToString();//低压压力
                    HPress1.Text = tb_HPress1.Text = HPress.ToString();//中高压压力
                    tb_CarPumpSpeed1.Text = CarPumpSpeed1.Text= CarPumpSpeed.ToString();//车载泵转速
                    InTemp1.Text = tb_InTemp1.Text = lbl_InTemp1.Text = InTemp.ToString();//输入轴温度
                    OutTemp1.Text = tb_OutTemp1.Text = lbl_OutTemp1.Text =OutTemp.ToString();//输出轴温度

                    

                    //真空度
                    if (Vacuum < ConstantValue.threshold.VacuumPressMin || Vacuum > ConstantValue.threshold.VacuumPressMax)
                    {
                        ERROR1.Add("真空度异常，异常值：" + Vacuum1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备1】真空度异常，异常值：" + Vacuum1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //低压压力
                    if (LPress < ConstantValue.threshold.LowPressMin || LPress > ConstantValue.threshold.LowPressMax)
                    {
                        ERROR1.Add("低压压力异常，异常值：" + LPress1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备1】低压压力异常，异常值：" + LPress1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //中高压压力
                    if (HPress < ConstantValue.threshold.HighPressMin || HPress > ConstantValue.threshold.HighPressMax)
                    {
                        ERROR1.Add("中高压压力异常，异常值：" + HPress1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备1】中高压压力异常，异常值：" + HPress1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //车载泵转速
                    if (CarPumpSpeed < ConstantValue.threshold.PumpSpeedMin || CarPumpSpeed > ConstantValue.threshold.PumpSpeedMax)
                    {
                        ERROR1.Add("车载泵转速异常，异常值：" + tb_CarPumpSpeed1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备1】车载泵转速异常，异常值：" + tb_CarPumpSpeed1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //输入轴温度
                    if (InTemp < ConstantValue.threshold.InTempMin || InTemp > ConstantValue.threshold.InTempMax)
                    {
                        ERROR1.Add("输入轴温度异常，异常值：" + InTemp1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备1】输入轴温度异常，异常值：" + InTemp1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //输出轴温度
                    if (OutTemp < ConstantValue.threshold.OutTempMin || OutTemp > ConstantValue.threshold.OutTempMax)
                    {
                        ERROR1.Add("输出轴温度异常，异常值：" + OutTemp1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备1】输出轴温度异常，异常值：" + OutTemp1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }

                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN50])
                    {
                        double DN50Flow = ValueConverter.DN50Converter(NModubs4Helper.Instance.GetValue16(2, 0));
                        double DN50Valve = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 1));                        
                        DN50Flow1.Text = DN50Flow.ToString();//50流量仪
                        DN50Value1.Text = DN50Valve.ToString();//50阀门仪

                        //DN50流量仪
                        if (DN50Flow < ConstantValue.threshold.FlowmeterMin50 || DN50Flow > ConstantValue.threshold.FlowmeterMax50)
                        {
                            ERROR1.Add("DN50流量仪流量异常，异常值：" + DN50Flow1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备1】DN50流量仪流量异常，异常值：" + DN50Flow1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN50阀门仪
                        if (DN50Valve < ConstantValue.threshold.ValveMin50 || DN50Valve > ConstantValue.threshold.ValveMax50)
                        {
                            ERROR1.Add("DN50阀门仪开度异常，异常值：" + DN50Value1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备1】DN50阀门仪开度异常，异常值：" + DN50Value1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN100])
                    {
                        double DN100Flow = ValueConverter.DN100Converter(NModubs4Helper.Instance.GetValue16(2, 2));
                        double DN100Valve = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 3));
                     
                        DN100Flow1.Text = DN100Flow.ToString();//100流量仪
                        DN100Value1.Text = DN100Valve.ToString().ToString();//100阀门仪

                        //DN100流量仪
                        if (DN100Flow < ConstantValue.threshold.FlowmeterMin100 || DN100Flow > ConstantValue.threshold.FlowmeterMax100)
                        {
                            ERROR1.Add("DN100流量仪流量异常，异常值：" + DN100Flow1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备1】DN100流量仪流量异常，异常值：" + DN100Flow1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN100阀门仪
                        if (DN100Valve < ConstantValue.threshold.ValveMin100 || DN100Valve > ConstantValue.threshold.ValveMax100)
                        {
                            ERROR1.Add("DN100阀门仪开度异常，异常值：" + DN100Value1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备1】DN100阀门仪开度异常，异常值：" + DN100Value1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        double DN200Flow = ValueConverter.DN200Converter(NModubs4Helper.Instance.GetValue16(2, 4));
                        double DN200Valve = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 5));
                        
                        DN200Flow1.Text = DN200Flow.ToString();//200流量仪
                        DN200Value1.Text = DN200Valve.ToString().ToString();//200阀门

                        //DN200流量仪
                        if (DN200Flow < ConstantValue.threshold.FlowmeterMin200 || DN200Flow > ConstantValue.threshold.FlowmeterMax200)
                        {
                            ERROR1.Add("DN200流量仪流量异常，异常值：" + DN200Flow1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备1】DN200流量仪流量异常，异常值：" + DN200Flow1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN200阀门仪
                        if (DN200Valve < ConstantValue.threshold.ValveMin200 || DN200Valve > ConstantValue.threshold.ValveMax200)
                        {
                            ERROR1.Add("DN200阀门仪开度异常，异常值：" + DN200Value1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备1】DN200阀门仪开度异常，异常值：" + DN200Value1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }

                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN300])
                    {
                        double DN300Flow = ValueConverter.DN300Converter(NModubs4Helper.Instance.GetValue16(2, 6));
                        double DN300Valve = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 7));
                        DN300Flow1.Text = DN300Flow.ToString();//300流量仪
                        DN300Flow1.Text = DN300Valve.ToString().ToString();//300阀门

                        //DN300流量仪
                        if (DN300Flow < ConstantValue.threshold.FlowmeterMin300 || DN300Flow > ConstantValue.threshold.FlowmeterMax300)
                        {
                            ERROR1.Add("DN300流量仪流量异常，异常值：" + DN300Flow1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备1】DN300流量仪流量异常，异常值：" + DN300Flow1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN300阀门仪
                        if (DN300Valve < ConstantValue.threshold.ValveMin300 || DN300Valve > ConstantValue.threshold.ValveMax300)
                        {
                            ERROR1.Add("DN300阀门仪开度异常，异常值：" + DN300Value1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备1】DN300阀门仪开度异常，异常值：" + DN300Value1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }

                    }
                    if (alarm1)
                    {
                        string final = "【设备1】";
                        foreach (string er in ERROR1)
                        {
                            final += "\n" + er;
                        }
                        MessageBox.Show(final);

                    }

                    chart1.Series[0].Points.AddY(double.Parse(tb_InTemp1.Text));
                    chart1.Series[1].Points.AddY(double.Parse(tb_OutTemp1.Text));
                    break;
                case 1:
                    List<string> ERROR2 = new List<string>();
                    double Vacuum2d = ValueConverter.RealPressConverter(NModubs4Helper.Instance.GetValue16(4, 0));
                    double LPress2d = ValueConverter.LPressConverter(NModubs4Helper.Instance.GetValue16(4, 1));
                    double HPress2d = ValueConverter.LHPressConverter(NModubs4Helper.Instance.GetValue16(4, 2));
                    double CarPumpSpeed2d = ValueConverter.PumpSpeedConverter(NModubs4Helper.Instance.GetValue16(4, 3));
                    double InTemp2d = ValueConverter.InTempConverter(NModubs4Helper.Instance.GetValue16(4, 4));
                    double OutTemp2d = ValueConverter.OutTempConverter(NModubs4Helper.Instance.GetValue16(4, 5));

    
                    Vacuum2.Text = tb_Vacuum2.Text = Vacuum2d.ToString();//真空度
                    LPress2.Text = tb_LPress2.Text = LPress2d.ToString();//低压压力
                    HPress2.Text = tb_HPress2.Text = HPress2d.ToString();//中高压压力
                    tb_CarPumpSpeed2.Text = CarPumpSpeed2.Text = CarPumpSpeed2d.ToString();//车载泵转速
                    InTemp2.Text = tb_InTemp2.Text = lbl_InTemp2.Text =InTemp2d.ToString();//输入轴温度
                    OutTemp2.Text = tb_OutTemp2.Text = lbl_OutTemp2.Text=OutTemp2d.ToString();//输出轴温度

                    //真空度
                    if (Vacuum2d < ConstantValue.threshold.VacuumPressMin || Vacuum2d > ConstantValue.threshold.VacuumPressMax)
                    {
                        ERROR2.Add("真空度异常，异常值：" + Vacuum2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备2】真空度异常，异常值：" + Vacuum2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //低压压力
                    if (LPress2d < ConstantValue.threshold.LowPressMin || LPress2d > ConstantValue.threshold.LowPressMax)
                    {
                        ERROR2.Add("低压压力异常，异常值：" + LPress2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备2】低压压力异常，异常值：" + LPress2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //中高压压力
                    if (HPress2d < ConstantValue.threshold.HighPressMin || HPress2d > ConstantValue.threshold.HighPressMax)
                    {
                        ERROR2.Add("中高压压力异常，异常值：" + HPress2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备2】中高压压力异常，异常值：" + HPress2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //车载泵转速
                    if (CarPumpSpeed2d < ConstantValue.threshold.PumpSpeedMin || CarPumpSpeed2d > ConstantValue.threshold.PumpSpeedMax)
                    {
                        ERROR2.Add("车载泵转速异常，异常值：" + tb_CarPumpSpeed2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备2】车载泵转速异常，异常值：" + tb_CarPumpSpeed2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //输入轴温度
                    if (InTemp2d < ConstantValue.threshold.InTempMin || InTemp2d > ConstantValue.threshold.InTempMax)
                    {
                        ERROR2.Add("输入轴温度异常，异常值：" + InTemp2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备2】输入轴温度异常，异常值：" + InTemp2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //输出轴温度
                    if (OutTemp2d < ConstantValue.threshold.OutTempMin || OutTemp2d > ConstantValue.threshold.OutTempMax)
                    {
                        ERROR2.Add("输出轴温度异常，异常值：" + OutTemp2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防车";
                            string alarmMessage = "【设备2】输出轴温度异常，异常值：" + OutTemp2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }


                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN50])
                    {
                        double DN50Flow2d = ValueConverter.DN50Converter(NModubs4Helper.Instance.GetValue16(2, 0));
                        double DN50Valve2d = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 1));
                       

                        DN50Flow2.Text = DN50Flow2d.ToString();
                        DN50Value2.Text = DN50Valve2d.ToString();

                        //DN50流量仪
                        if (DN50Flow2d < ConstantValue.threshold.FlowmeterMin50 || DN50Flow2d > ConstantValue.threshold.FlowmeterMax50)
                        {
                            ERROR2.Add("DN50流量仪流量异常，异常值：" + DN50Flow2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备2】DN50流量仪流量异常，异常值：" + DN50Flow2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN50阀门仪
                        if (DN50Valve2d < ConstantValue.threshold.ValveMin50 || DN50Valve2d > ConstantValue.threshold.ValveMax50)
                        {
                            ERROR2.Add("DN50阀门仪开度异常，异常值：" + DN50Value2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备2】DN50阀门仪开度异常，异常值：" + DN50Value2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }

                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN100])
                    {
                        double DN100Flow2d = ValueConverter.DN100Converter(NModubs4Helper.Instance.GetValue16(2, 2));
                        double DN100Valve2d = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 3));
                       
                        DN100Flow2.Text = DN100Flow2d.ToString();
                        DN100Value2.Text = DN100Valve2d.ToString();

                        //DN100流量仪
                        if (DN100Flow2d < ConstantValue.threshold.FlowmeterMin100 || DN100Flow2d > ConstantValue.threshold.FlowmeterMax100)
                        {
                            ERROR2.Add("DN100流量仪流量异常，异常值：" + DN100Flow2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备2】DN100流量仪流量异常，异常值：" + DN100Flow2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN100阀门仪
                        if (DN100Valve2d < ConstantValue.threshold.ValveMin100 || DN100Valve2d > ConstantValue.threshold.ValveMax100)
                        {
                            ERROR2.Add("DN100阀门仪开度异常，异常值：" + DN100Value2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备2】DN100阀门仪开度异常，异常值：" + DN100Value2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }

                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        double DN200Flow2d = ValueConverter.DN200Converter(NModubs4Helper.Instance.GetValue16(2, 4));
                        double DN200Valve2d = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 5));
                        
                        DN200Flow2.Text = DN200Flow2d.ToString();
                        DN200Value2.Text = DN200Valve2d.ToString();                     

                        //DN200流量仪
                        if (DN200Flow2d < ConstantValue.threshold.FlowmeterMin200 || DN200Flow2d > ConstantValue.threshold.FlowmeterMax200)
                        {
                            ERROR2.Add("DN200流量仪流量异常，异常值：" + DN200Flow2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备2】DN200流量仪流量异常，异常值：" + DN200Flow2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN200阀门仪
                        if (DN200Valve2d < ConstantValue.threshold.ValveMin200 || DN200Valve2d > ConstantValue.threshold.ValveMax200)
                        {
                            ERROR2.Add("DN200阀门仪开度异常，异常值：" + DN200Value2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备2】DN200阀门仪开度异常，异常值：" + DN200Value2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }

                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN300])
                    {
                        double DN300Flow2d = ValueConverter.DN300Converter(NModubs4Helper.Instance.GetValue16(2, 6));
                        double DN300Valve2d = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 7));
                        DN300Flow2.Text = DN300Flow2d.ToString();
                        DN300Flow2.Text = DN300Valve2d.ToString();

                        //DN300流量仪
                        if (DN300Flow2d < ConstantValue.threshold.FlowmeterMin300 || DN300Flow2d > ConstantValue.threshold.FlowmeterMax300)
                        {
                            ERROR2.Add("DN300流量仪流量异常，异常值：" + DN300Flow2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备2】DN300流量仪流量异常，异常值：" + DN300Flow2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN300阀门仪
                        if (DN300Valve2d < ConstantValue.threshold.ValveMin300 || DN300Valve2d > ConstantValue.threshold.ValveMax300)
                        {
                            ERROR2.Add("DN300阀门仪开度异常，异常值：" + DN300Value2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from ConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防车";
                                string alarmMessage = "【设备2】DN300阀门仪开度异常，异常值：" + DN300Value2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }

                    }
                    if (alarm2)
                    {
                        string final = "【设备2】";
                        foreach (string er in ERROR2)
                        {
                            final += "\n" + er;
                        }
                        MessageBox.Show(final);

                    }
                    chart2.Series[0].Points.AddY(double.Parse(tb_InTemp2.Text));
                    chart2.Series[1].Points.AddY(double.Parse(tb_OutTemp2.Text));

                    break;

            }
            if (ConstantValue.Tick_Num % ConstantValue.SAVE_DATA_INTEINTERVALS == 0)
            {
                bool flag_L = false;
                bool flag_H = false;
                ConditionRecord temp = new ConditionRecord();
                temp.LabID = ConstantValue.IdList[i][1];
                temp.ConditionNum = (int)ConstantValue.xfcInfos[i].currentGk;
                temp.SpecificCollectTime = DateTime.Now;
                object[] records = new object[10];
                if (i == 0)
                {


                    temp.CollectTime = ConstantValue.runtime1 / 1000 / 60;
                    if (ConstantValue.PumpTypeList[i] != PumpType.GaoYaPump && ConstantValue.PumpTypeList[i] != PumpType.ZhongYaPump && ConstantValue.PumpTypeList[i] != PumpType.None)
                    {
                        temp.L_Press = double.Parse(tb_LPress1.Text);
                        flag_L = true;
                    }

                    if (ConstantValue.PumpTypeList[i] != PumpType.DiYaPump && ConstantValue.PumpTypeList[i] != PumpType.None)
                    {
                        temp.H_Press = double.Parse(tb_HPress1.Text);
                        flag_H = true;

                    }
                    temp.VacuumDegree = double.Parse(Vacuum1.Text);
                    temp.Speed = double.Parse(tb_CarPumpSpeed1.Text);
                    temp.InTemp = double.Parse(InTemp1.Text);
                    temp.OutTemp = double.Parse(OutTemp1.Text);
                    if (flag_L)
                    {
                        switch (ConstantValue.liuliangjiAndFlowtype[i][0])
                        {

                            case FlowType.DN100:
                                temp.L_Flow = double.Parse(DN100Flow1.Text);
                                break;
                            case FlowType.DN200:
                                temp.L_Flow = double.Parse(DN200Flow1.Text);
                                break;
                            case FlowType.DN300:
                                temp.L_Flow = double.Parse(DN300Flow1.Text);
                                break;

                        }
                    }
                    if (flag_H)
                    {
                        switch (ConstantValue.liuliangjiAndFlowtype[i][1])
                        {
                            case FlowType.DN50:
                                temp.H_Flow = double.Parse(DN50Flow1.Text);
                                break;
                            case FlowType.DN100:
                                temp.H_Flow = double.Parse(DN100Flow1.Text);
                                break;

                        }
                    }


                    //流量添加

                }
                else if (i == 1)
                {
                    temp.CollectTime = ConstantValue.runtime2 / 60 / 1000;
                    if (ConstantValue.PumpTypeList[i] != PumpType.GaoYaPump && ConstantValue.PumpTypeList[i] != PumpType.ZhongYaPump && ConstantValue.PumpTypeList[i] != PumpType.None)
                    {
                        temp.L_Press = double.Parse(tb_LPress2.Text);
                        flag_L = true;
                    }

                    if (ConstantValue.PumpTypeList[i] != PumpType.DiYaPump && ConstantValue.PumpTypeList[i] != PumpType.None)
                    {
                        temp.H_Press = double.Parse(tb_HPress2.Text);
                        flag_H = true;

                    }
                    temp.VacuumDegree = double.Parse(Vacuum2.Text);
                    temp.Speed = double.Parse(tb_CarPumpSpeed2.Text);
                    temp.InTemp = double.Parse(InTemp2.Text);
                    temp.OutTemp = double.Parse(OutTemp2.Text);
                    //流量添加
                    if (flag_L)
                    {
                        switch (ConstantValue.liuliangjiAndFlowtype[i][0])
                        {

                            case FlowType.DN100:
                                temp.L_Flow = double.Parse(DN100Flow2.Text);
                                break;
                            case FlowType.DN200:
                                temp.L_Flow = double.Parse(DN200Flow2.Text);
                                break;
                            case FlowType.DN300:
                                temp.L_Flow = double.Parse(DN300Flow2.Text);
                                break;

                        }
                    }
                    if (flag_H)
                    {
                        switch (ConstantValue.liuliangjiAndFlowtype[i][1])
                        {
                            case FlowType.DN50:
                                temp.H_Flow = double.Parse(DN50Flow2.Text);
                                break;
                            case FlowType.DN100:
                                temp.H_Flow = double.Parse(DN100Flow2.Text);
                                break;

                        }
                    }

                }
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.InsertData(temp);

                }
                if(ConstantValue.Tick_Num % ConstantValue.Sampling_Time == 0)
                {

                    //Datagridview 显示数据
                    if (isDatagridViewShowRealTime1 && ConstantValue.EquipemntList[0] == Equipment.Car)
                    {
                        records[0] = temp.LabID;
                        records[1] = temp.SpecificCollectTime;
                        records[2] = temp.L_Press;
                        records[3] = temp.L_Flow;
                        records[4] = temp.H_Press;
                        records[5] = temp.H_Flow;
                        records[6] = temp.VacuumDegree;
                        records[7] = temp.Speed;
                        records[8] = temp.InTemp;
                        records[9] = temp.OutTemp;
                        dataTable1.Rows.Add(records);
                        dataGridView1.DataSource = dataTable1;
                        dataGridView1.Refresh();


                    }
                    if (isDatagridViewShowRealTime2 && ConstantValue.EquipemntList[1] == Equipment.Car)
                    {
                        records[0] = temp.LabID;
                        records[1] = temp.SpecificCollectTime;
                        records[2] = temp.L_Press;
                        records[3] = temp.L_Flow;
                        records[4] = temp.H_Press;
                        records[5] = temp.H_Flow;
                        records[6] = temp.VacuumDegree;
                        records[7] = temp.Speed;
                        records[8] = temp.InTemp;
                        records[9] = temp.OutTemp;

                        dataTable2.Rows.Add(records);
                        dataGridView2.DataSource = dataTable2;
                        dataGridView2.Refresh();





                    }
                }
              



            }
         


        }


        /// <summary>
        ///显示消防泵的实时数据
        /// </summary>
        /// <param name="i"></param>
        private void DataShow_xfb(int i)
        {  
            switch (i)
            {
                case 0:
                    List<string> ERROR1 = new List<string>();
                    double Vacuum = ValueConverter.RealPressConverter(NModubs4Helper.Instance.GetValue16(1, 0));
                    double LPress = ValueConverter.LPressConverter(NModubs4Helper.Instance.GetValue16(1, 1));
                    double HPress = ValueConverter.LHPressConverter(NModubs4Helper.Instance.GetValue16(1, 2));
                    double CarPumpSpeed = ValueConverter.PumpSpeedConverter(NModubs4Helper.Instance.GetValue16(1, 3));
                    double InTemp = ValueConverter.InTempConverter(NModubs4Helper.Instance.GetValue16(1, 4));
                    double OutTemp = ValueConverter.OutTempConverter(NModubs4Helper.Instance.GetValue16(1, 5));

                   

                    Vacuum1.Text = tb_Vacuum1.Text = Vacuum.ToString();//真空度                   
                    LPress1.Text = tb_LPress1.Text = LPress.ToString();//低压压力
                    HPress1.Text = tb_HPress1.Text = HPress.ToString();//中高压压力
                    tb_CarPumpSpeed1.Text = CarPumpSpeed1.Text = CarPumpSpeed.ToString();//车载泵转速
                    InTemp1.Text = tb_InTemp1.Text = lbl_InTemp1.Text = InTemp.ToString();//输入轴温度
                    OutTemp1.Text = tb_OutTemp1.Text = lbl_OutTemp1.Text = OutTemp.ToString();//输出轴温度

                    //真空度
                    if (Vacuum < ConstantValue.threshold.VacuumPressMin || Vacuum > ConstantValue.threshold.VacuumPressMax)
                    {
                        ERROR1.Add("真空度异常，异常值：" + Vacuum1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备1】真空度异常，异常值：" + Vacuum1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //低压压力
                    if (LPress < ConstantValue.threshold.LowPressMin || LPress > ConstantValue.threshold.LowPressMax)
                    {
                        ERROR1.Add("低压压力异常，异常值：" + LPress1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备1】低压压力异常，异常值：" + LPress1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //中高压压力
                    if (HPress < ConstantValue.threshold.HighPressMin || HPress > ConstantValue.threshold.HighPressMax)
                    {
                        ERROR1.Add("中高压压力异常，异常值：" + HPress1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备1】中高压压力异常，异常值：" + HPress1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //车载泵转速
                    if (CarPumpSpeed < ConstantValue.threshold.PumpSpeedMin || CarPumpSpeed > ConstantValue.threshold.PumpSpeedMax)
                    {
                        ERROR1.Add("车载泵转速异常，异常值：" + tb_CarPumpSpeed1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备1】车载泵转速异常，异常值：" + tb_CarPumpSpeed1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //输入轴温度
                    if (InTemp < ConstantValue.threshold.InTempMin || InTemp > ConstantValue.threshold.InTempMax)
                    {
                        ERROR1.Add("输入轴温度异常，异常值：" + InTemp1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备1】输入轴温度异常，异常值：" + InTemp1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //输出轴温度
                    if (OutTemp < ConstantValue.threshold.OutTempMin || OutTemp > ConstantValue.threshold.OutTempMax)
                    {
                        ERROR1.Add("输出轴温度异常，异常值：" + OutTemp1.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备1】输出轴温度异常，异常值：" + OutTemp1.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }


                    chart1.Series[0].Points.AddY(double.Parse(tb_InTemp1.Text));
                    chart1.Series[1].Points.AddY(double.Parse(tb_OutTemp1.Text));
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN50])
                    {
                        double DN50Flow = ValueConverter.DN50Converter(NModubs4Helper.Instance.GetValue16(2, 0));
                        double DN50Valve = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 1));
                       
                        DN50Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 0).ToString();
                        DN50Value1.Text = NModubs4Helper.Instance.GetValue16(2, 1).ToString();

                        //DN50流量仪
                        if (DN50Flow < ConstantValue.threshold.FlowmeterMin50 || DN50Flow > ConstantValue.threshold.FlowmeterMax50)
                        {
                            ERROR1.Add("DN50流量仪流量异常，异常值：" + DN50Flow1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                // Id  ，试验Id ， 设备类型 ，水泵类型，工况， 报警时间，  异常信息
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备1】DN50流量仪流量异常，异常值：" + DN50Flow1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN50阀门仪
                        if (DN50Valve < ConstantValue.threshold.ValveMin50 || DN50Valve > ConstantValue.threshold.ValveMax50)
                        {
                            ERROR1.Add("DN50阀门仪开度异常，异常值：" + DN50Value1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备1】DN50阀门仪开度异常，异常值：" + DN50Value1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }

                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN100])
                    {
                        double DN100Flow = ValueConverter.DN100Converter(NModubs4Helper.Instance.GetValue16(2, 2));
                        double DN100Valve = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 3));
                       
                        DN100Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 2).ToString();
                        DN100Value1.Text = NModubs4Helper.Instance.GetValue16(2, 3).ToString();

                        //DN100流量仪
                        if (DN100Flow < ConstantValue.threshold.FlowmeterMin100 || DN100Flow > ConstantValue.threshold.FlowmeterMax100)
                        {
                            ERROR1.Add("DN100流量仪流量异常，异常值：" + DN100Flow1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备1】DN100流量仪流量异常，异常值：" + DN100Flow1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN100阀门仪
                        if (DN100Valve < ConstantValue.threshold.ValveMin100 || DN100Valve > ConstantValue.threshold.ValveMax100)
                        {
                            ERROR1.Add("DN100阀门仪开度异常，异常值：" + DN100Value1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备1】DN100阀门仪开度异常，异常值：" + DN100Value1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        double DN200Flow = ValueConverter.DN200Converter(NModubs4Helper.Instance.GetValue16(2, 4));
                        double DN200Valve = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 5));
                        
                        DN200Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 4).ToString();
                        DN200Value1.Text = NModubs4Helper.Instance.GetValue16(2, 5).ToString();

                        //DN200流量仪
                        if (DN200Flow < ConstantValue.threshold.FlowmeterMin200 || DN200Flow > ConstantValue.threshold.FlowmeterMax200)
                        {
                            ERROR1.Add("DN200流量仪流量异常，异常值：" + DN200Flow1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备1】DN200流量仪流量异常，异常值：" + DN200Flow1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN200阀门仪
                        if (DN200Valve < ConstantValue.threshold.ValveMin200 || DN200Valve > ConstantValue.threshold.ValveMax200)
                        {
                            ERROR1.Add("DN200阀门仪开度异常，异常值：" + DN200Value1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备1】DN200阀门仪开度异常，异常值：" + DN200Value1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN300])
                    {
                        double DN300Flow = ValueConverter.DN300Converter(NModubs4Helper.Instance.GetValue16(2, 6));
                        double DN300Valve = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 7));
                        DN300Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 6).ToString();
                        DN300Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 7).ToString();

                        //DN300流量仪
                        if (DN300Flow < ConstantValue.threshold.FlowmeterMin300 || DN300Flow > ConstantValue.threshold.FlowmeterMax300)
                        {
                            ERROR1.Add("DN300流量仪流量异常，异常值：" + DN300Flow1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备1】DN300流量仪流量异常，异常值：" + DN300Flow1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN300阀门仪
                        if (DN300Valve < ConstantValue.threshold.ValveMin300 || DN300Valve > ConstantValue.threshold.ValveMax300)
                        {
                            ERROR1.Add("DN300阀门仪开度异常，异常值：" + DN300Value1.Text);
                            alarm1 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备1】DN300阀门仪开度异常，异常值：" + DN300Value1.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }

                    }
                    if (alarm1)
                    {
                        string final = "【设备1】";
                        foreach (string er in ERROR1)
                        {
                            final += "\n" + er;
                        }
                        MessageBox.Show(final);

                    }

                    break;
                case 1:
                    List<string> ERROR2 = new List<string>();
                    double Vacuum2d = ValueConverter.RealPressConverter(NModubs4Helper.Instance.GetValue16(4, 0));
                    double LPress2d = ValueConverter.LPressConverter(NModubs4Helper.Instance.GetValue16(4, 1));
                    double HPress2d = ValueConverter.LHPressConverter(NModubs4Helper.Instance.GetValue16(4, 2));
                    double CarPumpSpeed2d = ValueConverter.PumpSpeedConverter(NModubs4Helper.Instance.GetValue16(4, 3));
                    double InTemp2d = ValueConverter.InTempConverter(NModubs4Helper.Instance.GetValue16(4, 4));
                    double OutTemp2d = ValueConverter.OutTempConverter(NModubs4Helper.Instance.GetValue16(4, 5));

                

                    Vacuum2.Text = tb_Vacuum2.Text = Vacuum2d.ToString();//真空度
                    LPress2.Text = tb_LPress2.Text = LPress2d.ToString();//低压压力
                    HPress2.Text = tb_HPress2.Text = HPress2d.ToString();//中高压压力
                    tb_CarPumpSpeed2.Text = CarPumpSpeed2.Text = CarPumpSpeed2d.ToString();//车载泵转速
                    InTemp2.Text = tb_InTemp2.Text = lbl_InTemp2.Text = InTemp2d.ToString();//输入轴温度
                    OutTemp2.Text = tb_OutTemp2.Text = lbl_OutTemp2.Text = OutTemp2d.ToString();//输出轴温度
                    //真空度
                    if (Vacuum2d < ConstantValue.threshold.VacuumPressMin || Vacuum2d > ConstantValue.threshold.VacuumPressMax)
                    {
                        ERROR2.Add("真空度异常，异常值：" + Vacuum2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备2】真空度异常，异常值：" + Vacuum2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //低压压力
                    if (LPress2d < ConstantValue.threshold.LowPressMin || LPress2d > ConstantValue.threshold.LowPressMax)
                    {
                        ERROR2.Add("低压压力异常，异常值：" + LPress2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备2】低压压力异常，异常值：" + LPress2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //中高压压力
                    if (HPress2d < ConstantValue.threshold.HighPressMin || HPress2d > ConstantValue.threshold.HighPressMax)
                    {
                        ERROR2.Add("中高压压力异常，异常值：" + HPress2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备2】中高压压力异常，异常值：" + HPress2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //车载泵转速
                    if (CarPumpSpeed2d < ConstantValue.threshold.PumpSpeedMin || CarPumpSpeed2d > ConstantValue.threshold.PumpSpeedMax)
                    {
                        ERROR2.Add("车载泵转速异常，异常值：" + tb_CarPumpSpeed2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备2】车载泵转速异常，异常值：" + tb_CarPumpSpeed2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //输入轴温度
                    if (InTemp2d < ConstantValue.threshold.InTempMin || InTemp2d > ConstantValue.threshold.InTempMax)
                    {
                        ERROR2.Add("输入轴温度异常，异常值：" + InTemp2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备2】输入轴温度异常，异常值：" + InTemp2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }
                    //输出轴温度
                    if (OutTemp2d < ConstantValue.threshold.OutTempMin || OutTemp2d > ConstantValue.threshold.OutTempMax)
                    {
                        ERROR2.Add("输出轴温度异常，异常值：" + OutTemp2.Text);
                        alarm1 = true;
                        using (OledbHelper helper = new OledbHelper())
                        {
                            helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                            int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                            string equipmentType = "消防泵";
                            string alarmMessage = "【设备2】输出轴温度异常，异常值：" + OutTemp2.Text;
                            alarming(conditionID, equipmentType, alarmMessage);
                        }
                    }


                    chart2.Series[0].Points.AddY(double.Parse(tb_InTemp2.Text));
                    chart2.Series[1].Points.AddY(double.Parse(tb_OutTemp2.Text));
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN50])
                    {
                        double DN50Flow2d = ValueConverter.DN50Converter(NModubs4Helper.Instance.GetValue16(2, 0));
                        double DN50Valve2d = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 1));
                       
                        DN50Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 0).ToString();
                        DN50Value2.Text = NModubs4Helper.Instance.GetValue16(2, 1).ToString();

                        //DN50流量仪
                        if (DN50Flow2d < ConstantValue.threshold.FlowmeterMin50 || DN50Flow2d > ConstantValue.threshold.FlowmeterMax50)
                        {
                            ERROR2.Add("DN50流量仪流量异常，异常值：" + DN50Flow2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备2】DN50流量仪流量异常，异常值：" + DN50Flow2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN50阀门仪
                        if (DN50Valve2d < ConstantValue.threshold.ValveMin50 || DN50Valve2d > ConstantValue.threshold.ValveMax50)
                        {
                            ERROR2.Add("DN50阀门仪开度异常，异常值：" + DN50Value2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备2】DN50阀门仪开度异常，异常值：" + DN50Value2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                       
                        

                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN100])
                    {
                        double DN100Flow2d = ValueConverter.DN100Converter(NModubs4Helper.Instance.GetValue16(2, 2));
                        double DN100Valve2d = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 3));
                        
                        DN100Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 2).ToString();
                        DN100Value2.Text = NModubs4Helper.Instance.GetValue16(2, 3).ToString();

                        //DN100流量仪
                        if (DN100Flow2d < ConstantValue.threshold.FlowmeterMin100 || DN100Flow2d > ConstantValue.threshold.FlowmeterMax100)
                        {
                            ERROR2.Add("DN100流量仪流量异常，异常值：" + DN100Flow2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备2】DN100流量仪流量异常，异常值：" + DN100Flow2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN100阀门仪
                        if (DN100Valve2d < ConstantValue.threshold.ValveMin100 || DN100Valve2d > ConstantValue.threshold.ValveMax100)
                        {
                            ERROR2.Add("DN100阀门仪开度异常，异常值：" + DN100Value2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备2】DN100阀门仪开度异常，异常值：" + DN100Value2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        double DN200Flow2d = ValueConverter.DN200Converter(NModubs4Helper.Instance.GetValue16(2, 4));
                        double DN200Valve2d = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 5));
                        
                        DN200Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 4).ToString();
                        DN200Value2.Text = NModubs4Helper.Instance.GetValue16(2, 5).ToString();

                        //DN200流量仪
                        if (DN200Flow2d < ConstantValue.threshold.FlowmeterMin200 || DN200Flow2d > ConstantValue.threshold.FlowmeterMax200)
                        {
                            ERROR2.Add("DN200流量仪流量异常，异常值：" + DN200Flow2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备2】DN200流量仪流量异常，异常值：" + DN200Flow2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN200阀门仪
                        if (DN200Valve2d < ConstantValue.threshold.ValveMin200 || DN200Valve2d > ConstantValue.threshold.ValveMax200)
                        {
                            ERROR2.Add("DN200阀门仪开度异常，异常值：" + DN200Value2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备2】DN200阀门仪开度异常，异常值：" + DN200Value2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }

                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN300])
                    {
                        double DN300Flow2d = ValueConverter.DN300Converter(NModubs4Helper.Instance.GetValue16(2, 6));
                        double DN300Valve2d = ValueConverter.ValveConverter(NModubs4Helper.Instance.GetValue16(2, 7));
                        DN300Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 6).ToString();
                        DN300Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 7).ToString();

                        //DN300流量仪
                        if (DN300Flow2d < ConstantValue.threshold.FlowmeterMin300 || DN300Flow2d > ConstantValue.threshold.FlowmeterMax300)
                        {
                            ERROR2.Add("DN300流量仪流量异常，异常值：" + DN300Flow2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备2】DN300流量仪流量异常，异常值：" + DN300Flow2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                        //DN300阀门仪
                        if (DN300Valve2d < ConstantValue.threshold.ValveMin300 || DN300Valve2d > ConstantValue.threshold.ValveMax300)
                        {
                            ERROR2.Add("DN300阀门仪开度异常，异常值：" + DN300Value2.Text);
                            alarm2 = true;
                            using (OledbHelper helper = new OledbHelper())
                            {
                                helper.sqlstring = "select Max(ConditionID) from PumpConditionRecord";
                                int conditionID = Convert.ToInt32(helper.ExecuteScalar());
                                string equipmentType = "消防泵";
                                string alarmMessage = "【设备2】DN300阀门仪开度异常，异常值：" + DN300Value2.Text;
                                alarming(conditionID, equipmentType, alarmMessage);
                            }
                        }
                    }
                    if (alarm2)
                    {
                        string final = "【设备2】";
                        foreach (string er in ERROR2)
                        {
                            final += "\n" + er;
                        }
                        MessageBox.Show(final);

                    }

                    break;
            }
            if (ConstantValue.Tick_Num % ConstantValue.SAVE_DATA_INTEINTERVALS == 0)
            {
                bool flag_L = false;
                bool flag_H = false;
                PumpConditionRecord temp = new PumpConditionRecord();
                temp.PumpLabID = ConstantValue.IdList[i][1];
                temp.ConditionNum = (int)ConstantValue.xfbInfos[i].currentGk;
                temp.SpecificCollectTime = DateTime.Now;

                if (i == 0)
                {


                    temp.CollectTime = ConstantValue.runtime1 / 1000 / 60;
                    if (ConstantValue.PumpTypeList[i] != PumpType.GaoYaPump && ConstantValue.PumpTypeList[i] != PumpType.ZhongYaPump && ConstantValue.PumpTypeList[i] != PumpType.None)
                    {
                        temp.L_Press = double.Parse(tb_LPress1.Text);
                        flag_L = true;
                    }

                    if (ConstantValue.PumpTypeList[i] != PumpType.DiYaPump && ConstantValue.PumpTypeList[i] != PumpType.None)
                    {
                        temp.H_Press = double.Parse(tb_HPress1.Text);
                        flag_H = true;

                    }
                    temp.VacuumDegree = double.Parse(Vacuum1.Text);
                    temp.Speed = double.Parse(tb_CarPumpSpeed1.Text);
                    temp.InTemp = double.Parse(InTemp1.Text);
                    temp.OutTemp = double.Parse(OutTemp1.Text);
                    if (flag_L)
                    {
                        switch (ConstantValue.liuliangjiAndFlowtype[i][0])
                        {

                            case FlowType.DN100:
                                temp.L_Flow = double.Parse(DN100Flow1.Text);
                                break;
                            case FlowType.DN200:
                                temp.L_Flow = double.Parse(DN200Flow1.Text);
                                break;
                            case FlowType.DN300:
                                temp.L_Flow = double.Parse(DN300Flow1.Text);
                                break;

                        }
                    }
                    if (flag_H)
                    {
                        switch (ConstantValue.liuliangjiAndFlowtype[i][1])
                        {
                            case FlowType.DN50:
                                temp.H_Flow = double.Parse(DN50Flow1.Text);
                                break;
                            case FlowType.DN100:
                                temp.H_Flow = double.Parse(DN100Flow1.Text);
                                break;

                        }
                    }

                    //流量添加

                }
                else if (i == 1)
                {
                    temp.CollectTime = ConstantValue.runtime2 / 60 / 1000;
                    if (ConstantValue.PumpTypeList[i] != PumpType.GaoYaPump && ConstantValue.PumpTypeList[i] != PumpType.ZhongYaPump && ConstantValue.PumpTypeList[i] != PumpType.None)
                    {
                        temp.L_Press = double.Parse(tb_LPress2.Text);
                        flag_L = true;
                    }

                    if (ConstantValue.PumpTypeList[i] != PumpType.DiYaPump && ConstantValue.PumpTypeList[i] != PumpType.None)
                    {
                        temp.H_Press = double.Parse(tb_HPress2.Text);
                        flag_H = true;

                    }
                    temp.VacuumDegree = double.Parse(Vacuum2.Text);
                    temp.Speed = double.Parse(tb_CarPumpSpeed2.Text);
                    temp.InTemp = double.Parse(InTemp2.Text);
                    temp.OutTemp = double.Parse(OutTemp2.Text);
                    //流量添加
                    if (flag_L)
                    {
                        switch (ConstantValue.liuliangjiAndFlowtype[i][0])
                        {

                            case FlowType.DN100:
                                temp.L_Flow = double.Parse(DN100Flow2.Text);
                                break;
                            case FlowType.DN200:
                                temp.L_Flow = double.Parse(DN200Flow2.Text);
                                break;
                            case FlowType.DN300:
                                temp.L_Flow = double.Parse(DN300Flow2.Text);
                                break;

                        }
                    }
                    if (flag_H)
                    {
                        switch (ConstantValue.liuliangjiAndFlowtype[i][1])
                        {
                            case FlowType.DN50:
                                temp.H_Flow = double.Parse(DN50Flow2.Text);
                                break;
                            case FlowType.DN100:
                                temp.H_Flow = double.Parse(DN100Flow2.Text);
                                break;

                        }
                    }

                }
                object[] records = new object[10];

                if (ConstantValue.Tick_Num % ConstantValue.Sampling_Time == 0)
                {

                    //Datagridview 显示数据
                    if (isDatagridViewShowRealTime1 && ConstantValue.EquipemntList[0] == Equipment.Pump)
                    {
                        records[0] = temp.PumpLabID;
                        records[1] = temp.SpecificCollectTime;
                        records[2] = temp.L_Press;
                        records[3] = temp.L_Flow;
                        records[4] = temp.H_Press;
                        records[5] = temp.H_Flow;
                        records[6] = temp.VacuumDegree;
                        records[7] = temp.Speed;
                        records[8] = temp.InTemp;
                        records[9] = temp.OutTemp;
                        dataTable1.Rows.Add(records);
                        dataGridView1.DataSource = dataTable1;
                        dataGridView1.Refresh();


                    }
                    if (isDatagridViewShowRealTime2 && ConstantValue.EquipemntList[1] == Equipment.Pump)
                    {
                        records[0] = temp.PumpLabID;
                        records[1] = temp.SpecificCollectTime;
                        records[2] = temp.L_Press;
                        records[3] = temp.L_Flow;
                        records[4] = temp.H_Press;
                        records[5] = temp.H_Flow;
                        records[6] = temp.VacuumDegree;
                        records[7] = temp.Speed;
                        records[8] = temp.InTemp;
                        records[9] = temp.OutTemp;

                        dataTable2.Rows.Add(records);
                        dataGridView2.DataSource = dataTable2;
                        dataGridView2.Refresh();




                    }
                }

                using (OledbHelper helper = new OledbHelper())
                {
                    helper.InsertData(temp);

                }

            }
            

        }
        /// <summary>
        ///倒计时事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedCountdown(Object source, ElapsedEventArgs e)
        {
            if (ConstantValue.EquipemntList[0]!=Equipment.None && ConstantValue.runtime1 > 0)
            {
                ConstantValue.runtime1 -= 1000;
                TimeSpan timeSpan1 = TimeSpan.FromMilliseconds(ConstantValue.runtime1);
                string timeLeft1 = timeSpan1.ToString(@"hh\:mm\:ss");
                this.Invoke(new System.Action(() => {
                    remainTime1.Text = timeLeft1;

                }));
            }
            if (ConstantValue.EquipemntList[1] != Equipment.None && ConstantValue.runtime2>0)
            {
                ConstantValue.runtime2 -= 1000;
                TimeSpan timeSpan2 = TimeSpan.FromMilliseconds(ConstantValue.runtime2);
                string timeLeft2 = timeSpan2.ToString(@"hh\:mm\:ss");
                this.Invoke(new System.Action(() => {
                    remainTime2.Text = timeLeft2;

                }));
            }
        

            // 都运行结束
            if (ConstantValue.runtime1<=0&& ConstantValue.runtime2 <=0)
            {
                    this.Invoke(new System.Action( ()=>DisConnect()));
            }






        }
        /// <summary>
        /// 初始化工况运行的定时器
        /// </summary>
        private void initDataTimer()
        {
           
            ConstantValue.DataShowTimer = new System.Timers.Timer(ConstantValue.DATA_SHOW_INTERVALS);
            ConstantValue.DataShowTimer.Elapsed += OnTimedDataShow;          
            ConstantValue.DataShowTimer.Elapsed += OnTimedCountdown;
            ConstantValue.DataShowTimer.Elapsed += OnTimedChartShow;
            ConstantValue.DataShowTimer.AutoReset = true;
            
            ConstantValue.DataShowTimer.Start();
            ConstantValue.gkStatus = GkStatus.Run;
        }


        /// <summary>
        /// 停止工况运行的定时器
        /// </summary>
        private void stopDataTimer()
        {          
            ConstantValue.DataShowTimer.Stop();
        }
        /// <summary>
        /// 关闭工况运行的定时器
        /// </summary>
        private void uninitDataTimer()
        {
            ConstantValue.DataShowTimer.Elapsed -= OnTimedDataShow;
            ConstantValue.DataShowTimer.Elapsed -= OnTimedCountdown;
            ConstantValue.DataShowTimer.Elapsed -= OnTimedChartShow;
            ConstantValue.DataShowTimer.Close();

        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (ConstantValue.gkStatus == GkStatus.Run)
            {
                stopDataTimer();
                ConstantValue.gkStatus = GkStatus.Stop;
                MessageBox.Show("工况已暂停");
                Form_Main.getInstance().Tb_Tip.AppendText("工况已暂停\n");

            }
            else
            {
                MessageBox.Show("暂无工况在运行");
            }
                
        }

        private void btn_DisConnect_Click(object sender, EventArgs e)
        {
            DisConnect();
            
                
        }

        private void DisConnect()
        {
            if (ConstantValue.gkStatus == GkStatus.Run || ConstantValue.gkStatus == GkStatus.Stop)
            {
                try
                {

                    uninitDataTimer();
                    NModubs4Helper.Instance.Close();


                    EndHandle();
                    MessageBox.Show("工况结束");
                    Util.ClearAllTextBoxes(Form_Main.getInstance());

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("暂无工况在运行");
            }
        }

        /// <summary>
        /// 工况运行结束后的处理
        /// </summary>
        private void EndHandle()
        {
         
            for(int i =0;i<=1; i++)
            {
                //打印信息添加
                if (ConstantValue.EquipemntList[i] != Equipment.None) {
                    ConstantValue.Print[i] = new KeyValuePair<Equipment, List<int>>(ConstantValue.EquipemntList[i], ConstantValue.IdList[i]);
                }
            
                //gk完成信息添加
                switch (ConstantValue.EquipemntList[i])
                {                    
                    case Equipment.Car:
                        int cargkindex= (int)ConstantValue.xfcInfos[i].currentGk;   
                        if(cargkindex < 6)   
                            ConstantValue.xfcInfos[i].IsGkCompleted[cargkindex] =true;
                            Gk curGk = ConstantValue.xfcInfos[i].currentGk;
                            this.Invoke(new System.Action(() => {
                                Tb_Tip.AppendText($"设备{i + 1}----消防车的{ConstantValue.gkString[(int)curGk]}结束\n");
                            }));
                        break;
                    case Equipment.Pump:
                        int pumpgkindex = (int)ConstantValue.xfbInfos[i].currentGk;
                        if (pumpgkindex < 6)
                            ConstantValue.xfbInfos[i].IsGkCompleted[pumpgkindex] = true;
                            Gk curGk_pump = ConstantValue.xfbInfos[i].currentGk;
                            this.Invoke(new System.Action(() => {
                                Tb_Tip.AppendText($"设备{i + 1}----消防泵的{ConstantValue.gkString[(int)curGk_pump]}结束\n");
                            }));
                        break;
                    case Equipment.None:
                        break;
                }
               
                
            }
            ConstantValueinit();
        }

        private void ConstantValueinit()
        {
            ConstantValue.gkStatus = GkStatus.Checked;
            //ConstantValue.IdList = new List<List<int>>()
            //{
            //new List<int>() { -1, -1},//INDEX 0:CarId  1: labId
            //new List<int>() { -1, -1}

            //};
            //LastId 更新
            Program.init();
            //運行時間初始化
            ConstantValue.runtime1= ConstantValue.runtime2=0;
            ConstantValue.Tick_Num = 0;
            
        }

        private void OnTimedChartShow(object sender, ElapsedEventArgs e)
        {
           
        }
        private void initChart()
        {
            // 设置图表属性和样式  
            chart1.Series[0].ChartType= SeriesChartType.Line;
            chart2.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[1].ChartType = SeriesChartType.Line;
            chart2.Series[1].ChartType = SeriesChartType.Line;
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart2.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
        }

        private void btn_Query_Click(object sender, EventArgs e)
        {
            isDatagridViewShowRealTime1 = false;
            dataTimeSpanQuery(0, dataGridView1, time_start1, time_end1);

        }
       

        private void btn_Query2_Click(object sender, EventArgs e)
        {
            isDatagridViewShowRealTime2= false;
            dataTimeSpanQuery(1, dataGridView2, time_start2, time_end2);
        }
        private void dataTimeSpanQuery(int index, DataGridView dataGridView, DateTimePicker starttime, DateTimePicker endtime)
        {
            if (ConstantValue.gkStatus != GkStatus.Run)
            {

                MessageBox.Show($"设备{index+1}未有运行工况");
                return;
            }
            if (ConstantValue.EquipemntList[index] == Equipment.None)
            {
                MessageBox.Show($"设备{index + 1}未有运行工况");

                return;
            }
            string start = starttime.Text;
            string end = endtime.Text;
            using (OledbHelper helper = new OledbHelper())
            {
                string tablename = ConstantValue.EquipemntList[index] == Equipment.Car ? "ConditionRecord" : "PumpConditionRecord";
                string Idfield = ConstantValue.EquipemntList[index] == Equipment.Car ? "LabID" : "PumpLabID";
               
                helper.sqlstring = string.Format("select [ConditionID],[SpecificCollectTime],[L_Press],[L_Flow],[H_Press],[H_Flow],[VacuumDegree],[Speed],[InTemp],[OutTemp] from {0} where [SpecificCollectTime] >= #{1}# and [SpecificCollectTime] <= #{2}# and [{3}] ={4}", tablename, start, end,Idfield, ConstantValue.IdList[index][1]);
                DataSet ds = helper.GetDataSet();

                dataGridView.DataSource = ds.Tables[0];
                //设置数据表格上显示的列标题
                dataGridView.Columns[0].HeaderText = "ID";
                dataGridView.Columns[1].HeaderText = "采集时间";
                dataGridView.Columns[2].HeaderText = "低压压力";
                dataGridView.Columns[3].HeaderText = "低压流量";
                dataGridView.Columns[4].HeaderText = "中高压压力";
                dataGridView.Columns[5].HeaderText = "中高压流量";
                dataGridView.Columns[6].HeaderText = "真空度";
                dataGridView.Columns[7].HeaderText = "消防泵转速";
                dataGridView.Columns[8].HeaderText = "输入轴温度";
                dataGridView.Columns[9].HeaderText = "输出轴温度";

            }
        }

        private void btn_Export1_Click(object sender, EventArgs e)
        {
            Util.ExportExcel("",dataGridView1);
        }

        private void btn_Export2_Click(object sender, EventArgs e)
        {
            Util.ExportExcel("", dataGridView2);
        }
        private bool InitializePrinting(DataGridView dataGridView , PrintDocument printDocument )
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() != DialogResult.OK)
                return false;
            printDocument.DocumentName = "实时数据";
            printDocument.PrinterSettings = printDialog.PrinterSettings;
            printDocument.DefaultPageSettings = printDialog.PrinterSettings.DefaultPageSettings;
            printDocument.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(40, 40, 40, 40);
            gridPrinter = new GridPrinter(dataGridView, printDocument, true, true, "实时数据", new System.Drawing.Font("黑体", 18, FontStyle.Bold, GraphicsUnit.Point), Color.Blue, true);
            return true;
        }

        private void btn_Print2_Click(object sender, EventArgs e)
        {
            if (InitializePrinting(dataGridView2, printDocument2))
            {
                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.Document = printDocument2;
                printPreviewDialog.ShowDialog();
            }
        }

        private void btn_Print1_Click(object sender, EventArgs e)
        {
            if (InitializePrinting(dataGridView1, printDocument1))
            {
                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.Document = printDocument1;
                printPreviewDialog.ShowDialog();
            }
        }
        private void printDocument1_PrintPage_1(object sender, PrintPageEventArgs e)
        {
            bool more = gridPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }
        private void printDocument2_PrintPage(object sender, PrintPageEventArgs e)
        {
            bool more = gridPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }
        static System.Data.DataTable CreateDataTable(string tableName)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(tableName);
            dataTable.Columns.Add("工况实验ID", typeof(int));
            dataTable.Columns.Add("采集时间", typeof(DateTime));
            dataTable.Columns.Add("低压压力", typeof(double));
            dataTable.Columns.Add("低压流量", typeof(double));
            dataTable.Columns.Add("中高压压力", typeof(double));
            dataTable.Columns.Add("中高压流量", typeof(double));
            dataTable.Columns.Add("真空度", typeof(double));
            dataTable.Columns.Add("消防泵转速", typeof(double));
            dataTable.Columns.Add("输入轴温度", typeof(double));
            dataTable.Columns.Add("输出轴温度", typeof(double));
            return dataTable;
        }
        static System.Data.DataTable CreateAlarmTable(string tableName)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(tableName);
            dataTable.Columns.Add("工况ID", typeof(int));
            dataTable.Columns.Add("设备类型", typeof(double));
            dataTable.Columns.Add("报警信息", typeof(string));
            dataTable.Columns.Add("采集时间", typeof(DateTime));


            return dataTable;
        }

        private void btn_Collect1_Click(object sender, EventArgs e)
        {
            isDatagridViewShowRealTime1 = true;
            dataGridView1.DataSource = dataTable1;
            dataGridView1.Refresh();
        }

        private void btn_Collect2_Click(object sender, EventArgs e)
        {
            isDatagridViewShowRealTime2 = true;
            dataGridView2.DataSource = dataTable1;
            dataGridView2.Refresh();
        }

        private void btn_Refresh_Sampling_Click(object sender, EventArgs e)
        {
            if(int.TryParse(Sampling_Interval.Text,out int result))
            {
                ConstantValue.Sampling_Time = result * 60;
            }

            else
            {
                MessageBox.Show("请检查输入的采样时间是否符合规范");
            }
           
        }

        private void btn_Export_alarm_Click(object sender, EventArgs e)
        {
            Util.ExportExcel("", dataGridView4);
        }

        private void btn_print_alarm_Click(object sender, EventArgs e)
        {
            if (InitializePrinting(dataGridView4, printDocument4))
            {
                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.Document = printDocument4;
                printPreviewDialog.ShowDialog();
            }
        }

        private void btn_Query_alarm_Click(object sender, EventArgs e)
        {
            isDatagridViewShowAlarmRealTime = false;
            string start = dateTime_alarmstart.Text;
            string end = dateTime_alarmend.Text;
            using (OledbHelper helper = new OledbHelper())
            {
                string tablename = @"AlarmRecord";

                helper.sqlstring = string.Format("select [AlarmId],[LabID],[EquipmentType],[AlarmMessage],[SpecificCollectTime] from {0} where [SpecificCollectTime] >= #{1}# and [SpecificCollectTime] <= #{2}# ", tablename, start, end);
                DataSet ds = helper.GetDataSet();

                dataGridView4.DataSource = ds.Tables[0];
                //设置数据表格上显示的列标题
                dataGridView4.Columns[0].HeaderText = "报警ID";
                dataGridView4.Columns[1].HeaderText = "工况ID";
                dataGridView4.Columns[2].HeaderText = "设备类型";
                dataGridView4.Columns[3].HeaderText = "报警信息";
                dataGridView4.Columns[4].HeaderText = "采集时间";

                dataGridView4.Refresh(); 
            }


        }

        private void btn_refresh_alarm_Click(object sender, EventArgs e)
        {
            isDatagridViewShowAlarmRealTime = true;
            dataGridView4.DataSource = dataTable_alarm;
            dataGridView4.Refresh();
        }
    }
}



