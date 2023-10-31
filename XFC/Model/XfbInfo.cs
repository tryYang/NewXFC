using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    public class XfbInfo
    {
        public int KeyId;
        public PumpBasicInfo pumpBasicInfo { get; set; } = new PumpBasicInfo();
        public PumpLab pumpLab
        {
            get; set;
        } = new PumpLab();

        public List<int> rumtimeList = new List<int>() { 0 };
        public List<bool> IsGkCompleted = new List<bool>() { false };
        public bool IsChecked = false;
        public int runtime = -1;
        public Enum currentGk;
        public XfbInfo(int ketid)
        {
            KeyId = ketid;
        }
        public XfbInfo(int ketid, PumpBasicInfo BasicInfo, PumpLab lab)
        {


            KeyId = ketid;
            pumpBasicInfo = BasicInfo;
            pumpLab = lab;
        }
    }
}
