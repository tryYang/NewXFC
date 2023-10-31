using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using XFC.Helper;

namespace XFC.View.Dialog
{
    public partial class Form_YuZhiSheZhi : Form
    {
        public static Form_YuZhiSheZhi instance;
        public static Form_YuZhiSheZhi GetInstance()
        {
            if(instance == null)
            {
                instance =new Form_YuZhiSheZhi();
                return instance;
            }
            return instance;
        }
        public Form_YuZhiSheZhi()
        {
            InitializeComponent();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            instance?.Close();
        }



       
        //【确定】


        //显示具体数据




    }
}
