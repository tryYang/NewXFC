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

namespace XFC.View
{
    public partial class Form_Main : Form
    {


        private MainWindowViewModel viewModel;
        private static Form_Main instance;   

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
               
                initDataTimer();
                initChart();
                
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

                if (ConstantValue.xfcInfos[0].IsChecked)
                {
                    tb_Vacuum1.Text = NModubs4Helper.Instance.GetValue16(1, 0).ToString();
                    tb_LPress1.Text = NModubs4Helper.Instance.GetValue16(1, 1).ToString();
                    tb_HPress1.Text = NModubs4Helper.Instance.GetValue16(1, 2).ToString();
                    tb_CarPumpSpeed1.Text = NModubs4Helper.Instance.GetValue16(1, 3).ToString();
                    tb_InTemp1.Text = NModubs4Helper.Instance.GetValue16(1, 4).ToString();
                    tb_OutTemp1.Text = NModubs4Helper.Instance.GetValue16(1, 5).ToString();
                    
                    chart1.Series[0].Points.AddY(double.Parse(tb_InTemp1.Text));
                    chart1.Series[1].Points.AddY(double.Parse(tb_OutTemp1.Text));
                }
                if (ConstantValue.xfcInfos[1].IsChecked)
                {
                    tb_Vacuum2.Text = NModubs4Helper.Instance.GetValue16(4, 0).ToString();
                    tb_LPress2.Text = NModubs4Helper.Instance.GetValue16(4, 1).ToString();
                    tb_HPress2.Text = NModubs4Helper.Instance.GetValue16(4, 2).ToString();
                    tb_CarPumpSpeed2.Text = NModubs4Helper.Instance.GetValue16(4, 3).ToString();
                    tb_InTemp2.Text = NModubs4Helper.Instance.GetValue16(4, 4).ToString();
                    tb_OutTemp2.Text = NModubs4Helper.Instance.GetValue16(4, 5).ToString();
                    chart2.Series[0].Points.AddY(double.Parse(tb_InTemp2.Text));
                    chart2.Series[1].Points.AddY(double.Parse(tb_OutTemp2.Text));
                }

                
                DateTime time = DateTime.Now;
             
            }));

            
            Console.WriteLine("定时器触发的事件在 {0:HH:mm:ss.fff} 执行", e.SignalTime);
        }
        private void OnTimedCountdown(Object source, ElapsedEventArgs e)
        {
            if (ConstantValue.xfcInfos[0].IsChecked || ConstantValue.runtime1 > 0)
            {
                ConstantValue.runtime1 -= 1000;
                TimeSpan timeSpan1 = TimeSpan.FromMilliseconds(ConstantValue.runtime1);
                string timeLeft1 = timeSpan1.ToString(@"hh\:mm\:ss");
                this.Invoke(new System.Action(() => {
                    remainTime1.Text = timeLeft1;

                }));
            }
            if (ConstantValue.xfcInfos[1].IsChecked|| ConstantValue.runtime2>0)
            {
                ConstantValue.runtime2 -= 1000;
                TimeSpan timeSpan2 = TimeSpan.FromMilliseconds(ConstantValue.runtime2);
                string timeLeft2 = timeSpan2.ToString(@"hh\:mm\:ss");
                this.Invoke(new System.Action(() => {
                    remainTime2.Text = timeLeft2;

                }));
            }
            
            // 都运行结束
            if(ConstantValue.runtime1<0&& ConstantValue.runtime2 < 0)
            {
                
            }






        }
        
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

        

        private void stopDataTimer()
        {
             ConstantValue.DataShowTimer.Stop();
        }
        private void uninitDataTimer()
        {
            
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
            if (ConstantValue.gkStatus == GkStatus.Run || ConstantValue.gkStatus == GkStatus.Stop) {
                try
                {
                    uninitDataTimer();
                    NModubs4Helper.Instance.Close();
                    MessageBox.Show("工况结束");
                }
                catch (Exception ex){ 
                
                    MessageBox.Show(ex.Message);    
                }
                
            }
            else
            {
                MessageBox.Show("暂无工况在运行");
            }
                
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


    }
}



