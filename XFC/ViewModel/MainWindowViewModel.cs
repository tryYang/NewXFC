using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using XFC.View;
using XFC.View.Dialog;
using XFC.View.Dialog.Product;
using XFC.View.Dialog.SongJianDanWei;
using XFC.View.Dialog.Print;
using XFC.Model;
using System.Reflection;
using System.Diagnostics;
using System.Collections;

namespace XFC.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        //Show 
        public ICommand XfcTestClickCommand { get; }//消防车试验
        public ICommand XfbTestClickCommand { get; }//消防泵试验
        public ICommand thresholdClickCommand { get; }//阈值管理
        public ICommand UserInfoClickCommand { get; }//操作人员
        public ICommand XfcProductClickCommand { get; }//消防车产品信息
        public ICommand XfbProductClickCommand { get; }//消防泵产品信息
        public ICommand ClientInfoClickCommand { get; }//客户信息

        //工况
        public ICommand GkChooseCommand { get; }//
        public ICommand GkPauseCommand { get; }//
        public ICommand GkRunCommand { get; }//
        public ICommand GkExitCommand { get; }//

        //Print  
        public ICommand PrintClickCommand1 { get; }
        public ICommand PrintClickCommand2 { get; }
        public ICommand PrintClickCommand3 { get; }

        //System Settings
        public ICommand TestParamsClickCommand { get; }
        public ICommand SignalSourceClickCommand { get; }

        //Exit

        public ICommand GkExitClickCommand { get; }
        public ICommand ExitClickCommand { get; }

        public ICommand TestExitClickCommand1{ get; }

        public ICommand TestExitClickCommand2{ get; }

        public MainWindowViewModel()
        {

            //Dialog Show
            XfcTestClickCommand = new RelayCommand(Xfc_Test_Show);
            XfbTestClickCommand = new RelayCommand(Xfb_Test_Show);
            thresholdClickCommand = new RelayCommand(thresholdShow);
            UserInfoClickCommand = new RelayCommand(UserInfoShow);
            XfcProductClickCommand = new RelayCommand(XfcProductShow);
            XfbProductClickCommand = new RelayCommand(XfbProductShow);
            ClientInfoClickCommand = new RelayCommand(ClientInfoShow);

            //GK choose

            GkChooseCommand = new RelayCommand(GkChooseClick);
            GkPauseCommand = new RelayCommand(GkPauseClick);
            GkRunCommand = new RelayCommand(GkRunClick);
            GkExitCommand = new RelayCommand(GkExitClick);



            //Print
            PrintClickCommand1 = new RelayCommand(PrintReportTable1);
            PrintClickCommand2 = new RelayCommand(PrintReportTable2);
            PrintClickCommand3 = new RelayCommand(PrintReportTable3);

            //System Settings
            TestParamsClickCommand = new RelayCommand(TestParamsClick);
            SignalSourceClickCommand = new RelayCommand(SignalSourceClick);

            //Exit
            ExitClickCommand = new RelayCommand(Exit);//Application Exit

            TestExitClickCommand1= new RelayCommand(TestExit1);
            TestExitClickCommand2 = new RelayCommand(TestExit2);
        }
        private void TestExit1( )
        {

            TestExit(0);
        }
        private void TestExit2()
        {
            TestExit(1);

        }
        private void TestExit(int index)
        {
            if (ConstantValue.EquipemntList[index] == Equipment.None)
            {
                MessageBox.Show($"设备{index+1}无试验");
            }
            if (ConstantValue.gkStatus == GkStatus.Run || ConstantValue.gkStatus == GkStatus.Stop) {
                MessageBox.Show($"请先结束正在运行的工况");
            }
            if (ConstantValue.gkStatus == GkStatus.Run)
            {

            }
            ConstantValue.xfcInfos[index] = new XfcInfo(index+1);
            ConstantValue.xfbInfos[index] = new XfbInfo(index+1);
            
            ConstantValue.EquipemntList[index] = Equipment.None;
            if (index == 0) {
                ConstantValue.runtime1 = 0;
            }
            else
            {
                ConstantValue.runtime2 = 0;
            }
            ConstantValue.IdList[index] = new List<int>() { -1, -1 };


        }
        private void Xfb_Test_Show()
        {
            Form_ShiYanCanShu_xfb.GetInstance().ShowDialog();

        }

        //对话框显示
        private void Xfc_Test_Show()
        {

            Form_ShiYanCanShu.GetInstance().ShowDialog();
        }
        private void thresholdShow()
        {

            Form_YuZhiSheZhi.GetInstance().ShowDialog(); 
        }
        private void UserInfoShow()
        {

            Form_Userinfo form = new Form_Userinfo();
            form.ShowDialog();
        }
        private void XfcProductShow()
        {

            Form_ChanPin form = new Form_ChanPin();
            form.ShowDialog();
        }
        private void ClientInfoShow()
        {
            Form_SongJianDanWei form = new Form_SongJianDanWei();
            form.ShowDialog();
        }
        private void XfbProductShow()
        {

         
        }
        //工况

        private void GkChooseClick()
        {
            if(ConstantValue.gkStatus ==GkStatus.Uncheck)
            {
                MessageBox.Show("请先新建试验");
                return;
            }
            if(ConstantValue.gkStatus == GkStatus.Run|| ConstantValue.gkStatus == GkStatus.Stop)
            {
                MessageBox.Show("工况运行中，若要选择工况，请先结束工况");
                return;
            }

            if (Form_GongKuangSelect.Instance == null)
            {
                Form_GongKuangSelect.Instance=new Form_GongKuangSelect();
            }
            Form_GongKuangSelect.Instance.Update();
            Form_GongKuangSelect.Instance.ShowDialog();



        }
        private void GkPauseClick()
        {


        }
        private void GkRunClick()
        {


        }
        private void GkExitClick()
        {


        }



        //打印报表
        private void PrintReportTable1()
        {
            var keyvalue = ConstantValue.Print[0];
            if(keyvalue.Key==Equipment.None)
            {
                MessageBox.Show("设备1没有可打印的报表");
                return;
            }
            Form_Print1 print= new Form_Print1();
            print.ShowDialog();
        }
        private void PrintReportTable2()
        {
            var keyvalue = ConstantValue.Print[1];
            if (keyvalue.Key == Equipment.None)
            {
                MessageBox.Show("设备2没有可打印的报表");
                return;
            }
            Form_Print2 print = new Form_Print2();
            print.ShowDialog();

        }

        private void PrintReportTable3()
        {

        }
        //系统设置
        private void TestParamsClick()
        {

        }
        private void SignalSourceClick()
        {

        }

        //退出
        private void Exit()
        {
            Application.Exit();
        }
    }
}
