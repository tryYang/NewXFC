using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    public class XfcInfo
    {
        public int KeyId;
        public CarBasicInfo carBasicInfo { get; set; }=new CarBasicInfo();
        public CarLab carLab 
        {
            get; set;
        }=new CarLab();

        public List<int> rumtimeList = new List<int>() { 0, 0, 0, 0, 0, 0};
        public List<bool> IsGkCompleted = new List<bool>() { false, false, false, false, false, false  };
        public bool IsChecked = false;
        public int runtime = -1;
        public Dictionary<FlowType, bool> dic_Flowtype = new Dictionary<FlowType, bool>() {
            { FlowType.DN50, false },
            { FlowType.DN100, false },
            { FlowType.DN200, false },
            { FlowType.DN300, false },

        };
        public Gk currentGk=Gk.None ;
        public XfcInfo(int ketid)
        {
            KeyId = ketid;
        }
        public XfcInfo( int ketid, CarBasicInfo BasicInfo, CarLab carlab) {


            KeyId = ketid;
            carBasicInfo = BasicInfo;
            carLab = carlab;
        }

    }
}
