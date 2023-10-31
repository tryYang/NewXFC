using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using XFC.Helper;
using XFC.View;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace XFC.ViewModel
{
    public  class LoginViewModel: ViewModelBase
    {
        Form form_main = Form_Main.getInstance();
        public ICommand ClickCommand { get; }
        public LoginViewModel() {
            
            UserName = "admin"; 
            PassWord = "admin";
            ClickCommand = new RelayCommand(ExecuteClick);
        }

        private string _username;
        public string UserName 
        {   get { return _username; }
            set {
                _username = value; OnPropertyChanged(nameof(UserName)); 
            } 
        }
        private string _password;
        public string PassWord
        {
            get { return _password; }
            set { _password = value;
                    OnPropertyChanged(nameof(PassWord));
            }
        }
        private void ExecuteClick()
        {
            if (CheckInput())
            {
                using (OledbHelper helper = new OledbHelper())
                {
                    helper.sqlstring = "select UserPassWord  from UserInfo where UserName=\"" + PassWord + "\"";
                    OleDbDataReader reader = helper.GetDataReader();
                    if (reader.Read())//结果集，一行一行循环
                    {
                        string pwd = reader.GetString(0);//获取查询结果第1列
                                                               // string sqlpwd = reader.GetString(2);//获取查询结果第三列
                        if (PassWord == pwd)
                        {
                               
                               form_main.Show();
                               Form_Login.getInstance().Hide();
                        }
                        else
                        {
                            MessageBox.Show("密码错误！");
                        }

                    }

                }

            }

        }
        private bool CheckInput()
        {
            if (UserName.Trim() == "" && PassWord.Trim() == "")
            {
                MessageBox.Show("请输入用户名和密码！");
                return false;

            }

            else if (UserName.Trim() == "")
            {
                MessageBox.Show("请输入用户名！");
                return false;
            }
            else if (PassWord.Trim() == "")
            {
                MessageBox.Show("请输入密码！");
                return false;
            }
            return true;
        }

    }
}
