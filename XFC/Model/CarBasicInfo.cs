using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    //车辆基本信息表
    public class CarBasicInfo
    {
        public int CarID { get; set; } //	车辆 ID
        public string CarName { get; set; } //	车辆名称
        public string CarFac { get; set; }   //	车辆厂家
        public DateTime CarProduceTime { get; set; } //	车辆生产日期
        public string UnderpanFac { get; set; }  //	底盘厂家
        public string PumpFac { get; set; }  //	水泵厂家
        public string CarNum { get; set; }  //	车牌号
        public string CarModel { get; set; }//	车辆型号
        public string UnderpanModel { get; set; }//底盘型号
        public string UnderpanVIN { get; set; }  //	底盘 VIN
        public string PumpModel { get; set; }    //	水泵型号
        public string PumpType { get; set; } //	水泵类型
        public double L_RatedFlow { get; set; }  //	额定低压流量
        public double L_RatedPress { get; set; } //	额定低压压力
        public double H_RatedFlow { get; set; } //	额定中高压流量
        public double H_RatedPress{ get; set; }	//	额定中高压压力

    }
}
