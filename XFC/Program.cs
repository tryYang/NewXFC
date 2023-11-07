using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using XFC.Helper;
using XFC.Model;
using XFC.View;
namespace XFC
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            init();
            Application.Run(new Form_Login());
            
            
        }
        public static void init()
        {
            using (OledbHelper helper = new OledbHelper())
            {
                helper.sqlstring = "select Max(CarID) from CarBasicInfo";
                ConstantValue.LastCarID = helper.GetMaxID();
                helper.sqlstring = "select Max(LabID) from CarLab";
                ConstantValue.LastCarLabID = helper.GetMaxID();
                helper.sqlstring = "select Max(PumpID) from PumpBasicInfo";
                ConstantValue.LastPumpID = helper.GetMaxID();
                helper.sqlstring = "select Max(PumpLabID) from PumpLab";
                ConstantValue.LastPumpLabID = helper.GetMaxID();
            }
        }
    }
}
