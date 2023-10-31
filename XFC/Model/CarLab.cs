using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    //车辆实验表
    public class CarLab 
    {
        public int  LabID{ get; set; } //	实验 IDyin
        public int   CarID { get; set; } //	车辆 ID
 
        public string CustomerDepart { get; set; }//送检单位
        public DateTime LabDate { get; set; }  //	实验日期
        public string CheckPeople{ get; set; }  //	检查人员
        public string L_Flowmeter{ get; set; }  //	低压流量计
        public string H_Flowmeter{ get; set; }  //	中高压流量计
        public double ThreeTemp  { get; set; } //	三米水池温度
        public double ThreePress { get; set; }  //	三米水池修正吸深
        public double SevenTemp  { get; set; }  //	七米水井温度
        public double SevenPress { get; set; }  //	七米水井修正吸深
        public double CarHeight	 { get; set; }  //	车泵高度
        public double Pressure { get; set; }  //	大气压力
        public double Temp { get; set; }  //	车泵高度
    }
}
