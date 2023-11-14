using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    //报警记录表
    public class AlarmRecord
    {
        public int AlarmId { get; set; }  //	报警 ID
        public int LabID { get; set; }  //	工况 ID

        public string AlarmMessage { get; set; } //	报警文本
        public DateTime SpecificCollectTime { get; set; } //	采集时间
        public string EquipmentType { get; set; } //设备类型


    }
}
