using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    /// <summary>
    /// 水泵基本信息表
    /// </summary>
    public class PumpBasicInfo
    {
        public int PumpID { get; set; } //	水泵 ID
        public string PumpName { get; set; }    //	水泵名称
        public string PumpFac { get; set; } //	水泵厂家
        public DateTime PumpProduceTime { get; set; }   //	水泵生产日期
        public string PumpType { get; set; }    //	水泵类型
        public string Speed { get; set; }   //	额定转速
        public string InPipeD { get; set; } //	进口管径
        public string OutPipeD { get; set; }    //	出口管径
        public string EpitopeDifference { get; set; }   //	表位差
        public string PumpModel { get; set; }	//	水泵型号

    }
}
