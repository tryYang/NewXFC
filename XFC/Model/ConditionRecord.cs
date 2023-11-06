using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    //工况记录表
    public  class ConditionRecord
    {
        public int LabID         { get; set; }  //	实验 ID
        public int ConditionNum   { get; set; }    //	工况序号
        public long  CollectTime  { get; set; } //	采集时间
        public double L_Press      { get; set; } //	低压压力
        public double L_Flow       { get; set; } //	低压流量
        public double H_Press      { get; set; }//	中高压压力
        public double H_Flow      { get; set; }//	中高压流量
        public double VacuumDegree { get; set; }//	真空度
        public double Speed        { get; set; } //	消防泵转速
        public double InTemp       { get; set; }//	输入轴温度
        public double OutTemp { get; set; }   //	输出轴温度
        public DateTime SpecificCollectTime { get; set; }	  //实时采集时间


    }
}
