using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Model
{
    //客户信息表
    public class CustomerInfo
    {
        public int CustomerID              { get; set; }       //	客户 ID
        public string CustomerDepart { get; set; } //	送检单位
        public string ContactPeople        { get; set; }    //	联系人
        public string PhoneNum             { get; set; }     //	联系电话   
        public string Address { get; set; }        //	联系地址

    }
}
