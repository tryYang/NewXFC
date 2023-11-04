using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    public class SerialPortParams
    {
        public string serialPortName { set; get; }
        public int BaudRate { set; get; }
        public int DataBits { set; get; }
        public StopBits StopBits { set; get; }
        public Parity Parity { set; get; }
    }
    public  enum GkStatus
    {
        Uncheck,
        Checked,
        Selected,
        Run ,
        Stop ,
      

    }
    public enum Gk
    {
        Diya,
        Onedot3,
        Supper,
        Half,
        Gaoya,
        Zhongya,
        None

    }
    public enum Equipment
    {
       Car,
       Pump,
       None

    }
    public enum PumpType
    {
        DiYaPump,
        GaoYaPump,
        ZhongYaPump,
        GaoDiYaPump,
        ZhongDiYaPump,
        None

    }
}
