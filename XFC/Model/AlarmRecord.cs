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
        public int AlarmID{ get; set; }  //	报警 ID
        public int ConditionID { get; set; }  //	工况 ID

        public string CustomerID { get; set; } //	报警文本
        public DateTime LabDate  { get; set; } //	报警时间
        public string CheckPeople { get; set; } //	工况运行时间


    }
}
