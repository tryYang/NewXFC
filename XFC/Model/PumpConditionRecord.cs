using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    /// <summary>
    /// 水泵工况记录表
    /// </summary>
    public class PumpConditionRecord
    {
        public int ConditonID { get; set; } //	工况ID
        public int PumpLabID { get; set; }  //	水泵实验 ID
        public int ConditionNum { get; set; }   //	工况序号
        public int CollectTime { get; set; }    //	采集时间
        public Double L_Press { get; set; } //	低压压力
        public Double L_Flow { get; set; }  //	低压流量
        public Double H_Press { get; set; } //	中高压压力
        public Double H_Flow { get; set; }  //	中高压流量
        public Double VacuumDegree { get; set; }    //	真空度
        public Double Speed { get; set; }   //	消防泵转速
        public Double InTemp { get; set; }  //	输入轴温度
        public Double OutTemp { get; set; }	//	输出轴温度

    }
}
