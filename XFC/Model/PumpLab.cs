using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    /// <summary>
    /// 水泵实验表
    /// </summary>
    public class PumpLab
    {
        public int PumpLabID { get; set; }  //	水泵实验 ID
        public int PumpID { get; set; } //	水泵 ID
        public string CustomerDepart { get; set; }  //	送检单位
        public DateTime LabDate { get; set; }   //	实验日期
        public string CheckPeople { get; set; } //	检查人员
        public string L_Flowmeter { get; set; } //	低压流量计
        public string H_Flowmeter { get; set; } //	中高压流量计
        public string ElectricalMachinery { get; set; } //	电机
        public Double ThreeTemp { get; set; }   //	三米水池温度
        public Double ThreePress { get; set; }  //	三米水池修正吸深
        public Double SevenTemp { get; set; }   //	七米水井温度
        public Double SevenPress { get; set; }  //	七米水井修正吸深
        public Double CarHeight { get; set; }   //	车泵高度
        public Double Temp { get; set; }    //	环境温度
        public Double Pressure { get; set; }	//	大气压力

    }
}
