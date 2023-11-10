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

namespace XFC.View
{
    public partial class Form_Main : Form
    {


        private MainWindowViewModel viewModel;
        private static Form_Main instance;
        private List<TextBox> DNcontrols1;
        private List<TextBox> DNcontrols2;
        GridPrinter gridPrinter;

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
            //工况
            tp_gkchoose.Click += (sender, e) => viewModel.GkChooseCommand.Execute(null);
            tp_gkzhanting.Click += (sender, e) => viewModel.GkPauseCommand.Execute(null);
            tp_gkrun.Click += (sender, e) => viewModel.GkRunCommand.Execute(null);
            tp_gkexit.Click += (sender, e) => viewModel.GkExitCommand.Execute(null);
            
            //打印
            tp_printtable1.Click += (sender, e) => viewModel.PrintClickCommand1.Execute(null);
            tp_printtable2.Click += (sender, e) => viewModel.PrintClickCommand2.Execute(null);
            tp_printtable3.Click += (sender, e) => viewModel.PrintClickCommand3.Execute(null);

            //退出
            tp_exit.Click += (sender, e) => viewModel.ExitClickCommand.Execute(null);
            this.FormClosed += (sender, e) => viewModel.ExitClickCommand.Execute(null);

            cmb_Baudrate.DataSource = ConstantValue.Baurates;
            cmb_Parity.DataSource = Enum.GetValues(typeof(Parity));
            cmb_StopBits.DataSource = new List<string> { "1","2"};
            cmb_PortNames.DataSource = SerialPort.GetPortNames();
            var textBoxes = this.Controls.OfType<System.Windows.Forms.TextBox>();
            DNcontrols1 = new List<TextBox>() { DN50Flow1, DN50Value1, DN100Flow1, DN100Value1, DN200Flow1, DN200Value1, DN300Flow1, DN300Value1 };
            DNcontrols2 = new List<TextBox>() { DN50Flow2, DN50Value2, DN100Flow2, DN100Value2, DN200Flow2, DN200Value2, DN300Flow2, DN300Value2 };




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
            
            this.Invoke(new System.Action(() => {

                //Task.Run(() => DataShow(0, ConstantValue.EquipemntList[0]));
                //Task.Run(() => DataShow(1, ConstantValue.EquipemntList[1]));
                DataShow(0, ConstantValue.EquipemntList[0]);
                DataShow(1, ConstantValue.EquipemntList[1]);
                DateTime time = DateTime.Now;
             
            }));

            
            Console.WriteLine("定时器触发的事件在 {0:HH:mm:ss.fff} 执行", e.SignalTime);
        }

        /// <summary>
        /// 显示设备的实时数据
        /// </summary>
        /// <param name="i"></param>
        /// <param name="eq"></param>
        private void DataShow(int i , Equipment eq)
        {
            if (eq == Equipment.Car && ConstantValue.xfcInfos[i].currentGk!=Gk.None)
            {
                DataShow_xfc(i);
            }
            else if (eq == Equipment.Pump && ConstantValue.xfbInfos[i].currentGk != Gk.None)
            {
                DataShow_xfb(i);
            }
           
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
                case 0:
                    
                    Vacuum1.Text=tb_Vacuum1.Text = NModubs4Helper.Instance.GetValue16(1, 0).ToString();
                    LPress1.Text= tb_LPress1.Text = NModubs4Helper.Instance.GetValue16(1, 1).ToString();
                    HPress1.Text=tb_HPress1.Text = NModubs4Helper.Instance.GetValue16(1, 2).ToString();
                    tb_CarPumpSpeed1.Text = NModubs4Helper.Instance.GetValue16(1, 3).ToString();
                    InTemp1.Text=tb_InTemp1.Text = NModubs4Helper.Instance.GetValue16(1, 4).ToString();
                    OutTemp1.Text=tb_OutTemp1.Text = NModubs4Helper.Instance.GetValue16(1, 5).ToString();
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN50])
                    {
                        DN50Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 0).ToString();
                        DN50Value1.Text = NModubs4Helper.Instance.GetValue16(2, 1).ToString();
                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN100])
                    {
                        DN100Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 2).ToString();
                        DN100Value1.Text = NModubs4Helper.Instance.GetValue16(2, 3).ToString();
                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        DN200Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 4).ToString();
                        DN200Value1.Text = NModubs4Helper.Instance.GetValue16(2, 5).ToString();
                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        DN300Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 6).ToString();
                        DN300Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 7).ToString();
                    }

                    chart1.Series[0].Points.AddY(double.Parse(tb_InTemp1.Text));
                    chart1.Series[1].Points.AddY(double.Parse(tb_OutTemp1.Text));
                    break;
                case 1:
                    Vacuum2.Text = tb_Vacuum2.Text = NModubs4Helper.Instance.GetValue16(4, 0).ToString();
                    LPress2.Text = tb_LPress2.Text = NModubs4Helper.Instance.GetValue16(4, 1).ToString();
                    HPress2.Text = tb_HPress2.Text = NModubs4Helper.Instance.GetValue16(4, 2).ToString();
                    tb_CarPumpSpeed2.Text = NModubs4Helper.Instance.GetValue16(4, 3).ToString();
                    InTemp2.Text = tb_InTemp2.Text = NModubs4Helper.Instance.GetValue16(4, 4).ToString();
                    OutTemp2.Text = tb_OutTemp2.Text = NModubs4Helper.Instance.GetValue16(4, 5).ToString();

                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN50])
                    {
                        DN50Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 0).ToString();
                        DN50Value2.Text = NModubs4Helper.Instance.GetValue16(2, 1).ToString();
                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN100])
                    {
                        DN100Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 2).ToString();
                        DN100Value2.Text = NModubs4Helper.Instance.GetValue16(2, 3).ToString();
                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        DN200Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 4).ToString();
                        DN200Value2.Text = NModubs4Helper.Instance.GetValue16(2, 5).ToString();
                    }
                    if (ConstantValue.xfcInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        DN300Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 6).ToString();
                        DN300Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 7).ToString();
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
                temp.ConditionNum =(int) ConstantValue.xfcInfos[i].currentGk;
                temp.SpecificCollectTime = DateTime.Now;

                if (i == 0)
                {
                    

                    temp.CollectTime = ConstantValue.runtime1/1000/60;
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
                    temp.Speed= double.Parse(tb_CarPumpSpeed1.Text);
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
                else if(i == 1)
                {
                    temp.CollectTime = ConstantValue.runtime2/60/1000;
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


            }
            ConstantValue.Tick_Num++;


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
                    Vacuum1.Text=tb_Vacuum1.Text = NModubs4Helper.Instance.GetValue16(1, 0).ToString();
                    LPress1.Text =tb_LPress1.Text = NModubs4Helper.Instance.GetValue16(1, 1).ToString();
                    HPress1.Text = tb_HPress1.Text = NModubs4Helper.Instance.GetValue16(1, 2).ToString();
                    tb_CarPumpSpeed1.Text = NModubs4Helper.Instance.GetValue16(1, 3).ToString();
                    InTemp1.Text=tb_InTemp1.Text = NModubs4Helper.Instance.GetValue16(1, 4).ToString();
                    OutTemp1.Text = tb_OutTemp1.Text = NModubs4Helper.Instance.GetValue16(1, 5).ToString();
                    chart1.Series[0].Points.AddY(double.Parse(tb_InTemp1.Text));
                    chart1.Series[1].Points.AddY(double.Parse(tb_OutTemp1.Text));
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN50])
                    {
                        DN50Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 0).ToString();
                        DN50Value1.Text = NModubs4Helper.Instance.GetValue16(2, 1).ToString();
                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN100])
                    {
                        DN100Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 2).ToString();
                        DN100Value1.Text = NModubs4Helper.Instance.GetValue16(2, 3).ToString();
                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        DN200Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 4).ToString();
                        DN200Value1.Text = NModubs4Helper.Instance.GetValue16(2, 5).ToString();
                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        DN300Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 6).ToString();
                        DN300Flow1.Text = NModubs4Helper.Instance.GetValue16(2, 7).ToString();
                    }
                   

                    break;
                case 1:
                    Vacuum2.Text = tb_Vacuum2.Text = NModubs4Helper.Instance.GetValue16(4, 0).ToString();
                    LPress2.Text = tb_LPress2.Text = NModubs4Helper.Instance.GetValue16(4, 1).ToString();
                    HPress2.Text = tb_HPress2.Text = NModubs4Helper.Instance.GetValue16(4, 2).ToString();
                    tb_CarPumpSpeed2.Text = NModubs4Helper.Instance.GetValue16(4, 3).ToString();
                    InTemp2.Text=tb_InTemp2.Text = NModubs4Helper.Instance.GetValue16(4, 4).ToString();
                    OutTemp2.Text = tb_OutTemp2.Text = NModubs4Helper.Instance.GetValue16(4, 5).ToString();
                    chart2.Series[0].Points.AddY(double.Parse(tb_InTemp2.Text));
                    chart2.Series[1].Points.AddY(double.Parse(tb_OutTemp2.Text));
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN50])
                    {
                        DN50Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 0).ToString();
                        DN50Value2.Text = NModubs4Helper.Instance.GetValue16(2, 1).ToString();
                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN100])
                    {
                        DN100Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 2).ToString();
                        DN100Value2.Text = NModubs4Helper.Instance.GetValue16(2, 3).ToString();
                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        DN200Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 4).ToString();
                        DN200Value2.Text = NModubs4Helper.Instance.GetValue16(2, 5).ToString();
                    }
                    if (ConstantValue.xfbInfos[i].dic_Flowtype[FlowType.DN200])
                    {
                        DN300Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 6).ToString();
                        DN300Flow2.Text = NModubs4Helper.Instance.GetValue16(2, 7).ToString();
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
            ConstantValue.IdList = new List<List<int>>()
            {
            new List<int>() { -1, -1},//INDEX 0:CarId  1: labId
            new List<int>() { -1, -1}

            };
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
            dataTimeSpanQuery(1, dataGridView1, time_start1, time_end1);

        }
       

        private void btn_Query2_Click(object sender, EventArgs e)
        {
            dataTimeSpanQuery(2, dataGridView2, time_start2, time_end2);
        }
        private void dataTimeSpanQuery(int index, DataGridView dataGridView, DateTimePicker starttime, DateTimePicker endtime)
        {
            if (ConstantValue.gkStatus != GkStatus.Run)
            {

                MessageBox.Show("设备1未有运行工况");
                return;
            }
            if (ConstantValue.EquipemntList[index] == Equipment.None)
            {
                MessageBox.Show("设备1未有运行工况");
                return;
            }
            string start = starttime.Text;
            string end = endtime.Text;
            using (OledbHelper helper = new OledbHelper())
            {
                string tablename = ConstantValue.EquipemntList[0] == Equipment.Car ? "ConditionRecord" : "PumpConditionRecord";

                helper.sqlstring = string.Format("select [ConditonID],[CollectTime],[L_Press],[L_Flow],[H_Press],[H_Flow],[VacuumDegree],[Speed],[InTemp],[OutTemp] from {0} where [CollectTime] >= #{1}# and [CollectTime] <= #{2}# ", tablename, start, end);
                DataSet ds = helper.GetDataSet();

                dataGridView.DataSource = ds.Tables[0];
                //设置数据表格上显示的列标题
                dataGridView.Columns[0].HeaderText = "工况实验ID";
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
       
    }
}



