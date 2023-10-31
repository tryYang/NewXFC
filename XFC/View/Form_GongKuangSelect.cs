using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
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
            Update();
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
            if (ConstantValue.xfcInfos[0].IsChecked)
            {
                if(int.TryParse(tb_runtime1.Text,out int result)){
                    ConstantValue.xfcInfos[0].runtime = result;
                    ConstantValue.runtime1 = result*60*1000;
                    setgk(1);
                    ConstantValue.gkStatus = GkStatus.Selected;
                    this.Close();
                    
                }
                else
                {
                    MessageBox.Show("请输入车辆1运行时间");
                    return;
                }
                
            }
            if (ConstantValue.xfcInfos[1].IsChecked)
            {
                if (int.TryParse(tb_runtime2.Text, out int result))
                {
                    ConstantValue.xfcInfos[1].runtime = result;
                    ConstantValue.runtime2 = result * 60 * 1000;
                    setgk(2);
                    ConstantValue.gkStatus = GkStatus.Selected;
                    this.Close();
                   
                }
                else
                {
                    MessageBox.Show("请输入车辆2运行时间");
                    return;
                }
            }
        }
        public void Update()
        {
            rb_standard1.Select();
            rb_standard2.Select();
            
            //试验状态更新
            CarStatusUpdate();
            //工况状态更新
            GkStatusUpdate();


        }

        private void setgk(int i)
        {
            int k = i - 1;
            if (i == 1)
            {
                
                if (rb_13_1.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Onedot3;
                else if (rb_standard1.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Diya;
                else if (rb_high1.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Gaoya;
                else if (rb_mid1.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Zhongya;
                else if (rb_half1.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Half;
                else if (rb_super1.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Supper;
            }
            else if (i == 2)
            {
                if (rb_13_2.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Onedot3;
                else if (rb_standard2.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Diya;
                else if (rb_high2.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Gaoya;
                else if (rb_mid2.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Zhongya;
                else if (rb_half2.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Half;
                else if (rb_super2.Checked)
                    ConstantValue.xfcInfos[k].currentGk = Gk.Supper;
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
