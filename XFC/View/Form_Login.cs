using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.OleDb;
using XFC.ViewModel;

namespace XFC.View
{
    public partial class Form_Login : Form
    {
        float x, y = 0;
        private LoginViewModel viewModel;
        private BindingSource bindingSource;
        private static Form_Login instance;
        public static Form_Login getInstance()
        {
            if (instance == null)
            {
                instance = new Form_Login();
                return instance;
            }
            else
                return instance;

        }
        public Form_Login()
        {
            InitializeComponent();
            //username = "admin";
            instance=this;
            viewModel = new LoginViewModel();
            bindingSource = new BindingSource();
            // 将BindingSource与ViewModel绑定
            bindingSource.DataSource = viewModel;
            // 将TextBox控件与BindingSource的Name属性绑定
            text_username.DataBindings.Add("Text", bindingSource, "UserName");
            text_password.DataBindings.Add("Text", bindingSource, "PassWord");
            btn_login.Click += (sender, e) => viewModel.ClickCommand.Execute(null);


            x = this.Width;
            y = this.Height;
            setTag(this);

        }

        private void Form_Login_Resize(object sender, EventArgs e)
        {
            float newx = this.Width / x;//宽度增长倍数
            float newy = this.Height / y;
            setControl(newx, newy, this);
        }
        void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }

            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        void setControl(float newx, float newy, Control cons)
        {
            foreach (Control con in cons.Controls)
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(';');
                    //根据窗体的宽度和高度比值确定新控件的位置和大小
                    con.Width = Convert.ToInt32(Convert.ToSingle(mytag[0]) * newx);
                    con.Height = Convert.ToInt32(Convert.ToSingle(mytag[1]) * newy);
                    con.Left = Convert.ToInt32(Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(Convert.ToSingle(mytag[3]) * newy);//顶边距
                    con.Font = new Font(con.Font.Name, Convert.ToSingle(mytag[4]) * newy, con.Font.Style, con.Font.Unit);//设置字体大小

                    if (con.Controls.Count > 0)
                    {
                        setControl(newx, newy, con);
                    }
                }
        }

        //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\消防车性能测试系统\Sql\消防水力测试系统.mdb 







    }

}
