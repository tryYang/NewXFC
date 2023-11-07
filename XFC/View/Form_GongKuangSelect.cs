using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XFC.Helper;
using XFC.Model;
using XFC.View;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace XFC.View
{
    public partial class Form_GongKuangSelect : Form
    {
        private static Form_GongKuangSelect instance;
        public static Form_GongKuangSelect Instance {
            get { return instance; }
            set { instance = value; }
        
        }


        public Form_GongKuangSelect()
        {
            InitializeComponent();
            
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (ConstantValue.gkStatus==GkStatus.Uncheck)
            {
                MessageBox.Show("请先新建试验");
                return;
            }
            if (ConstantValue.gkStatus == GkStatus.Run || ConstantValue.gkStatus == GkStatus.Stop)
            {
                MessageBox.Show("请先退出运行的工况");
                return;
            }
            for (int i=0;i<ConstantValue.EquipemntList.Count;i++)
            {
                if(ConstantValue.EquipemntList[i]!=Equipment.None)
                {
                    GkSelect(i, ConstantValue.EquipemntList[i]);

                }
                
            }
           
        }

        private void GkSelect(int i, Equipment eq)
        {
            Control control_runtime = i == 0 ? tb_runtime1 : tb_runtime2;
            if (eq == Equipment.Car)
            {
               
                
                if (int.TryParse(control_runtime.Text, out int result))
                {
                    ConstantValue.xfcInfos[i].runtime = result;
                    if(i == 0)
                        ConstantValue.runtime1 = result * 60 * 1000;
                    else
                        ConstantValue.runtime2 = result * 60 * 1000;
                    setcargk(i);
                    ConstantValue.gkStatus = GkStatus.Selected;
                    int other = Math.Abs(i - 1);
                    ConstantValue.IdList[i][0] = ConstantValue.IdList[other][0] != -1 && ConstantValue.EquipemntList[other] == Equipment.Car ? ConstantValue.IdList[other][0] + 1 : ConstantValue.LastCarID + 1;
                    ConstantValue.IdList[i][1] = ConstantValue.IdList[other][1] != -1 && ConstantValue.EquipemntList[other] == Equipment.Car ? ConstantValue.IdList[other][1] + 1 : ConstantValue.LastCarLabID + 1;
                    ConstantValue.xfcInfos[i].carBasicInfo.CarID = ConstantValue.xfcInfos[i].carLab.CarID = ConstantValue.IdList[i][0];
                    ConstantValue.xfcInfos[i].carLab.LabID = ConstantValue.IdList[i][1];

                }
                else
                {
                    MessageBox.Show($"请输入设备{i+1}运行时间");
                    return;
                }
            }
            else if(eq == Equipment.Pump)
            {

                if (int.TryParse(control_runtime.Text, out int result))
                {
                    ConstantValue.xfbInfos[i].runtime = result;
                    if (i == 1)
                        ConstantValue.runtime1 = result * 60 * 1000;
                    else
                        ConstantValue.runtime2 = result * 60 * 1000;
                    setpumpgk(i);
                    ConstantValue.gkStatus = GkStatus.Selected;
                    int other = Math.Abs(i - 1);
                    ConstantValue.IdList[i][0] = ConstantValue.IdList[other][0] != -1 && ConstantValue.EquipemntList[other] == Equipment.Pump ? ConstantValue.IdList[other][0] + 1 : ConstantValue.LastPumpID + 1;
                    ConstantValue.IdList[i][1] = ConstantValue.IdList[other][1] != -1 && ConstantValue.EquipemntList[other] == Equipment.Pump ? ConstantValue.IdList[other][1] + 1 : ConstantValue.LastPumpLabID + 1;
                    ConstantValue.xfbInfos[i].pumpLab.PumpLabID = ConstantValue.IdList[i][1];
                    ConstantValue.xfbInfos[i].pumpLab.PumpID = ConstantValue.IdList[i][1];
                    ConstantValue.xfbInfos[i].pumpBasicInfo.PumpID = ConstantValue.IdList[i][1];

                }
                else
                {
                    MessageBox.Show($"请输入设备{i+1}运行时间");
                    return;
                }
            }
            this.Close();
        }

        public void Update()
        {

            switch (ConstantValue.EquipemntList[0]) {
                case Equipment.Car:
                    lb_Equipment1.Text = "消防车";
                    break;
                case Equipment.Pump:
                    lb_Equipment1.Text = "消防泵";
                    break;
                case Equipment.None:
                    lb_Equipment1.Text = "无";
                    break;
            }
            switch (ConstantValue.EquipemntList[1])
            {
                case Equipment.Car:
                    lb_Equipment2.Text = "消防车";
                    break;
                case Equipment.Pump:
                    lb_Equipment2.Text = "消防泵";
                    break;
                case Equipment.None:
                    lb_Equipment2.Text = "无";
                    break;
            }

            //试验状态更新
            CarStatusUpdate();
            //工况状态更新
            GkStatusUpdate();
            SelectUpdate();

        }

        private void SelectUpdate()
        {
            List<System.Windows.Forms.RadioButton> RadioButtons1=new List<System.Windows.Forms.RadioButton>() {
                rb_standard1,
                rb_13_1,
                rb_super1,
                rb_half1,
                rb_high1,
                rb_mid1
            };
            List<System.Windows.Forms.RadioButton> RadioButtons2 = new List<System.Windows.Forms.RadioButton>() {
                rb_standard2,
                rb_13_2,
                rb_super2,
                rb_half2,
                rb_high2,
                rb_mid2
            };
            List<bool> selectindex1 = new List<bool>() { false, false, false, false, false, false };
            List<bool> selectindex2 = new List<bool>() { false, false, false, false, false, false };
            

            switch (ConstantValue.PumpTypeList[0])
            {
                case PumpType.DiYaPump:
                    selectindex1[0]=true; selectindex1[1] = true; selectindex1[2] = true; selectindex1[3] = true;
                    break;
                case PumpType.GaoYaPump:
                    selectindex1[0] = true;  selectindex1[3] = true;

                    break;
                case PumpType.ZhongYaPump:
                    selectindex1[0] = true; selectindex1[3] = true;

                    break;
                case PumpType.GaoDiYaPump:
                    selectindex1[0] = true; selectindex1[5] = true; selectindex1[2] = true; selectindex1[3] = true;

                    break;
                case PumpType.ZhongDiYaPump :
                    selectindex1[0] = true; selectindex1[6] = true; selectindex1[2] = true; selectindex1[3] = true;

                    break;
               
            }
            switch (ConstantValue.PumpTypeList[1])
            {
                case PumpType.DiYaPump:
                    selectindex2[0] = true; selectindex2[1] = true; selectindex2[2] = true; selectindex2[3] = true;
                    break;
                case PumpType.GaoYaPump:
                    selectindex2[0] = true; selectindex2[3] = true;

                    break;
                case PumpType.ZhongYaPump:
                    selectindex2[0] = true; selectindex2[3] = true;

                    break;
                case PumpType.GaoDiYaPump:
                    selectindex2[0] = true; selectindex2[5] = true; selectindex2[2] = true; selectindex2[3] = true;

                    break;
                case PumpType.ZhongDiYaPump:
                    selectindex2[0] = true; selectindex2[6] = true; selectindex2[2] = true; selectindex2[3] = true;

                    break;

            }
            for(int i = 0; i < selectindex1.Count; i++)
            {
                if (selectindex1[i]) {
                    RadioButtons1[i].Show();
                }
                else
                {
                    RadioButtons1[i].Hide();
                }
                
            }
            for (int i = 0; i < selectindex2.Count; i++)
            {
                if (selectindex2[i])
                {
                    RadioButtons2[i].Show();
                }
                else
                {
                    RadioButtons2[i].Hide();
                }

            }
            //Dictionary<PumpType, List<System.Windows.Forms.RadioButton> >  dic=null;
            //List<System.Windows.Forms.RadioButton> DiyaRadioButtons = new List<System.Windows.Forms.RadioButton>() { };
            //List<System.Windows.Forms.RadioButton> GaoyaRadioButtons = new List<System.Windows.Forms.RadioButton>();
            //List<System.Windows.Forms.RadioButton> ZhongyaRadioButtons = new List<System.Windows.Forms.RadioButton>();
            //List<System.Windows.Forms.RadioButton> DaodiyaRadioButtons = new List<System.Windows.Forms.RadioButton>();
            //List<System.Windows.Forms.RadioButton> ZhogndiyaRadioButtons = new List<System.Windows.Forms.RadioButton>();


            //dic.Add(PumpType.DiYaPump, );


        }

        private void setcargk(int i)
        {
            int k = i;
            string s = string.Empty;
            if (i == 0)
            {

                if (rb_13_1.Checked && rb_13_1.Visible)
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Onedot3;
                    s = "1.3工况";
                }
                else if (rb_standard1.Checked && rb_standard1.Visible)
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Diya;
                    s = "低压工况";
                }
                else if (rb_high1.Checked && rb_high1.Visible) 
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Gaoya;
                    s = "高压工况";
                }
                else if (rb_mid1.Checked && rb_mid1.Visible) 
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Zhongya;
                    s = "中压工况";
                }
                else if (rb_half1.Checked && rb_half1.Visible) 
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Half;
                    s = "半流量工况";
                }
                else if (rb_super1.Checked && rb_super1.Visible) 
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Supper;
                    s = "超流量工况";
                }                    
                else
                {
                    MessageBox.Show("未选择具体工况");
                    return;
                }
                Form_Main.getInstance().Tb_Tip.AppendText($"{i}--消防车试验--{s}选择成功");
            }
            else if (i == 1)
            {
                if (rb_13_2.Checked && rb_13_2.Visible)
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Onedot3;
                    s = "1.3工况";
                }

                else if (rb_standard2.Checked && rb_standard2.Visible)
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Diya;
                    s = "低压工况";
                }
                else if (rb_high2.Checked && rb_high2.Visible)
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Gaoya;
                    s = "高压工况";
                }
                else if (rb_mid2.Checked && rb_mid2.Visible)
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Zhongya;
                    s = "中压工况";
                }

                else if (rb_half2.Checked && rb_half2.Visible)
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Half;
                    s = "半流量工况";
                }
                else if (rb_super2.Checked && rb_super2.Visible) 
                {
                    ConstantValue.xfcInfos[k].currentGk = Gk.Supper;
                    s = "超流量工况";
                }
                   
                else
                {
                    MessageBox.Show("未选择具体工况");
                    return;
                }
                Form_Main.getInstance().Tb_Tip.AppendText($"{i}--消防车试验--{s}选择成功");
                
            }
            

        }
        private void setpumpgk(int i)
        {
            int k = i ;
            if (i+1 == 1)
            {

                if (rb_13_1.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Onedot3;
                else if (rb_standard1.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Diya;
                else if (rb_high1.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Gaoya;
                else if (rb_mid1.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Zhongya;
                else if (rb_half1.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Half;
                else if (rb_super1.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Supper;
            }
            else if (i+1 == 2)
            {
                if (rb_13_2.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Onedot3;
                else if (rb_standard2.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Diya;
                else if (rb_high2.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Gaoya;
                else if (rb_mid2.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Zhongya;
                else if (rb_half2.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Half;
                else if (rb_super2.Checked)
                    ConstantValue.xfbInfos[k].currentGk = Gk.Supper;
            }

        }


        public void GkStatusUpdate()
        {
            foreach (var xfcinfo in ConstantValue.xfcInfos)
            {
                if (xfcinfo.KeyId == 1)
                {
                    for(int i = 0; i < xfcinfo.IsGkCompleted.Count; i++)
                    {
                        bool flag = xfcinfo.IsGkCompleted[i];
                        switch (i)
                        {
                            case 0: UpdateLb(lb_standard1, flag);
                                break;
                            case 1: UpdateLb(lb_13_1, flag);
                                break;
                            case 2: UpdateLb(lb_super1, flag);
                                break;
                            case 3: UpdateLb(lb_half1, flag);
                                break;
                            case 4: UpdateLb(lb_high1, flag);
                                break;
                            case 5: UpdateLb(lb_mid1, flag);
                                break;
                        }
                    }
                }
                else if (xfcinfo.KeyId == 2) 
                {
                    for (int i = 0; i < xfcinfo.IsGkCompleted.Count; i++)
                    {
                        bool flag = xfcinfo.IsGkCompleted[i];
                        switch (i)
                        {
                            case 0:
                                UpdateLb(lb_standard2, flag);
                                break;
                            case 1:
                                UpdateLb(lb_13_2, flag);
                                break;
                            case 2:
                                UpdateLb(lb_super2, flag);
                                break;
                            case 3:
                                UpdateLb(lb_half2, flag);
                                break;
                            case 4:
                                UpdateLb(lb_high2, flag);
                                break;
                            case 5:
                                UpdateLb(lb_mid2, flag);
                                break;
                        }
                    }
                }
            }
        }
        public void UpdateLb(Control control,bool flag)
        {
            if (flag)
            {
                control.Text = "已做";
            }
            if (flag)
            {
                control.Text = "未做";
            }
        }
        public void CarStatusUpdate()
        {
            bool ischeck1 = false;
            bool ischeck2 = false;
            foreach (var xfcinfo in ConstantValue.xfcInfos)
            {

                if (xfcinfo.IsChecked)
                {
                    if (xfcinfo.KeyId == 1)
                    {
                        ischeck1 = true;
                    }
                    else
                    {
                        ischeck2 = true;
                    }
                }
            }
            if (ischeck1)
            {
                lb_status1.Text = "已完成";
                lb_status1.BackColor = Color.White;
                lb_pumptype1.Text = ConstantValue.xfcInfos[0].carBasicInfo.PumpType;
                

            }
            else
            {
                lb_status1.Text = "未建立";
                lb_status1.BackColor = Color.Red;
            }
            if (ischeck2)
            {
                lb_status2.Text = "已完成";
                lb_status2.BackColor = Color.White;
                lb_pumptype2.Text = ConstantValue.xfcInfos[1].carBasicInfo.PumpType;
            }
            else
            {
                lb_status2.Text = "未建立";
                lb_status2.BackColor = Color.Red;
                
            }
        }
    }
}
