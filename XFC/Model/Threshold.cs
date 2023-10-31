using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    //阈值设置表
    public  class Threshold
    {
        public int ThresholdID      { get; set; }   //	阈值 ID
        public Double VacuumPressMax{ get; set; }    //	真空压力最大值
        public Double VacuumPressMin{ get; set; }   //	真空压力最小值
        public Double LowPressMax   { get; set; }//	低压压力最大值
        public Double LowPressMin   { get; set; } //	低压压力最小值
        public Double HighPressMax  { get; set; } //	中高压压力最大值
        public Double HighPressMin  { get; set; }  //	中高压压力最小值
        public Double PumpSpeedMax  { get; set; } //	车载泵转速最大值
        public Double PumpSpeedMin  { get; set; } //	车载泵转速最小值
        public Double InTempMax     { get; set; }  //	输入轴温度最大值
        public Double InTempMin     { get; set; }   //	输入轴温度最小值
        public Double OutTempMax    { get; set; }   //	输出轴温度最大值
        public Double OutTempMin    { get; set; }    //	输出轴温度最小值
        public Double SpeedMax110kw { get; set; }    //	110kw转速最大值
        public Double SpeedMin110kw { get; set; }    //	110kw转速最小值
        public Double FlowmeterMax50{ get; set; }    //	50 流量仪最大值
        public Double FlowmeterMin50{ get; set; }  //	50 流量仪最小值
        public Double ValveMax50    { get; set; }  //	50 阀门仪最大值
        public Double ValveMin50 { get; set; }  //	50 阀门仪最小值
        public Double FlowmeterMax100  { get; set; } //	100流量仪最大值
        public Double FlowmeterMin100  { get; set; } //	100流量仪最小值
        public Double ValveMax100      { get; set; }//	100 阀门仪最大值
        public Double ValveMin100      { get; set; }  //	100 阀门仪最小值
        public Double FlowmeterMax200  { get; set; } //	200 流量仪最大值
        public Double FlowmeterMin200  { get; set; }   //	200 流量仪最小值
        public Double ValveMax200      { get; set; }  //	200 阀门仪最大值
        public Double ValveMin200      { get; set; }    //	200 阀门仪最小值
        public Double FlowmeterMax300  { get; set; }  //	300流量仪最大值
        public Double FlowmeterMin300  { get; set; }  //	300 流量仪最小值
        public Double ValveMax300      { get; set; }    //	300 阀门仪最大值
        public Double ValveMin300      { get; set; }   //	300阀门仪最小值
        public Double AirPressMax      { get; set; }   //	大气压力最大值
        public Double AirPressMin { get; set; }     //	大气压力最小值
        public Double EnvironmentTempMax{ get; set; }    //	环境温度最大值
        public Double EnvironmentTempMin{ get; set; }   //	环境温度最小值
        public Double ThreeDepthMax     { get; set; }       //	三米水池深度最大值
        public Double ThreeDepthMin     { get; set; }      //	三米水池深度最小值
        public Double ThreeTempMax      { get; set; } //	三米水池温度最大值
        public Double ThreeTempMin      { get; set; } //	三米水池温度最小值
        public Double SevenDepthMax     { get; set; }       //	七米水井深度最大值
        public Double SevenDepthMin     { get; set; }     //	七米水井深度最小值
        public Double SevenTempMax      { get; set; }  //	七米水井温度最大值
        public Double SevenTempMin { get; set; }      	//	七米水井温度最小值

    }
}
