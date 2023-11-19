using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    public class SlaveValue
    {
        //采集卡1
        public double Vacuum {  get; set; }//通道0（1，0）
        public double LPress{  get; set; }
        public double HPress {  get; set; }
        public double CarPumpSpeed {  get; set; }
        public double InTemp {  get; set; }
        public double OutTemp { get; set; }
        public double Speed110kw { get; set; }//110kw转速
        public double Torque110kw { get; set; }//110kw转矩

        //采集卡2
        public double DN50Flow { get; set; }//通道0（2，0）
        public double DN50Valve { get; set; }
        public double DN100Flow { get; set; }
        public double DN100Valve { get; set; }
        public double DN200Flow { get; set; }
        public double DN200Valve { get; set; }
        public double DN300Flow { get; set; }
        public double DN300Valve { get; set; }

        //采集卡3
        public double ThreePress { get; set; }//通道0（3，0）
        public double ThreeTemp { get; set; }
        public double Pressure0 { get; set; }
        public double Temp0 { get; set; }
        public double SevenPress { get; set; }
        public double SevenTemp { get; set; }

        //采集卡4
        public double Vacuum2d { get; set; }//通道0（4，0）
        public double LPress2d { get; set; }
        public double HPress2d { get; set; }
        public double CarPumpSpeed2d { get; set; }
        public double InTemp2d { get; set; }
        public double OutTemp2d { get; set; }
        public double Speed110kw2d { get; set; }//110kw转速
        public double Torque110kw2d { get; set; }//110kw转矩


    }
}
