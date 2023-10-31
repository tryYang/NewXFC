using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XFC.Model;

namespace XFC
{
    public class ConstantValue
    {

       
       public static GkStatus gkStatus = GkStatus.Uncheck;
       public static List<string> WaterPumpType = new List<string> { "低压泵","中压泵","高压泵","中低压泵", "高低压泵" };
       public static string AppSetting=" ";
       public static List<XfcInfo> xfcInfos =new List<XfcInfo>() { new XfcInfo(1), new XfcInfo(2)};
       public static List<string> Baurates =new List<string>() {
                         "2400", "4800", "9600", "19200" 
       };
       public  const int DATA_SHOW_INTERVALS = 1000;
       public static System.Timers.Timer DataShowTimer = new System.Timers.Timer(1000) ;

        //ValueConvter
        public static double minMAValue = 4;//4 mA
        public static double maxMAValue = 20;//20 mA

        public static ConditionRecord RealTimeData1;
        public static ConditionRecord RealTimeData2;

        public static int runtime1= 0;
        public static int runtime2 = 0;






    }
}
