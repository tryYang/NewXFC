using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XFC
{
    internal class PrintSqlGenerate
    {
        string select=string.Empty;
        string fromtable = string.Empty;
        string condition = string.Empty;
        string template = "Select {0} from {1} where {2} GROUP BY{3}";
        string carinfo = "车辆基本信息{0}";
        string shiyaninfo = "试验基本信息表{0}";
        string gongkuanginfo = "工况记录表{0}{1}";
        string groupby= string.Empty;
        public List<string> fields = new List<string>();
        int carnum;
        public PrintSqlGenerate(int cheliang, List<string> gongkuang) // gongkuang 1:半流量工况 2:超负荷工况 3:高压工况 4:低压工况
        {
            
            carnum = cheliang;
            carinfo = string.Format(carinfo, cheliang);
            shiyaninfo = string.Format(shiyaninfo, cheliang);
            foreach(string s in gongkuang)
            {
                fields.Add(string.Format(gongkuanginfo,cheliang,s));
            }
            groupby = $"[{shiyaninfo}.检查人员], [{carinfo}.车辆厂家], [{carinfo}.车辆名称], [{carinfo}.底盘厂家], [{carinfo}.水泵厂家], [{carinfo}.车辆牌号], [{carinfo}.车辆型号], [{carinfo}.底盘型号], [{carinfo}.水泵型号], [{carinfo}.车辆厂家], [{carinfo}.底盘VIN], [{carinfo}.水泵类型], [{shiyaninfo}.大气压力], [{shiyaninfo}.3米水池温度], [{shiyaninfo}.7米水池温度], [{shiyaninfo}.3米水池修正吸深], [{shiyaninfo}.7米水池修正吸深]";

        }
        public string Generate()
        {
            SelectGenerate();
            TableGenerate();
            ConditionGenerate();
            return string.Format(template, select, fromtable, condition,groupby);
        }
        void SelectGenerate() {
            List<string> gkfields = new List<string> {"时间","低压压力","低压流量","中高压压力","中高压流量","真空度", "消防泵转速", "输入轴温度", "输出轴温度" };
            select = $"[{shiyaninfo}.检查人员] AS 试验人员, [{carinfo}.车辆厂家] AS 车辆单位, [{carinfo}.车辆名称] AS 产品名称, [{carinfo}.底盘厂家] AS 底盘厂家, [{carinfo}.水泵厂家] AS 水泵厂家, [{carinfo}.车辆牌号] AS 车牌号, [{carinfo}.车辆型号] AS 车辆型号, [{carinfo}.底盘型号] AS 底盘型号, [{carinfo}.水泵型号] AS 水泵型号, [{carinfo}.车辆厂家] AS 生产厂家, [{carinfo}.底盘VIN] AS 底盘VIN, [{carinfo}.水泵类型] AS 水泵类型, [{shiyaninfo}.大气压力] AS 大气压力, [{shiyaninfo}.3米水池温度] AS 三米水池温度, [{shiyaninfo}.7米水池温度] AS 七米水池温度, [{shiyaninfo}.3米水池修正吸深] AS 三米水池修正吸深, [{shiyaninfo}.7米水池修正吸深] AS 七米水池修正吸深";
            foreach (string s in fields) {


            foreach (string s2 in gkfields)
                {
                    if (s2.Equals("时间")) 
                        select += $",AVG({s}.采集时间) AS {s2}_{s.Substring(9)}";
                    else
                        select+= $",AVG({s}.{s2}) AS {s2}_{s.Substring(9)}";

                }

            }
        }
        void TableGenerate() {
            string template = $"({carinfo} INNER JOIN {shiyaninfo} ON {carinfo}.车辆ID = {shiyaninfo}.车辆ID)";
            fromtable = template;
            List<string> joins = new List<string>();
            string template2 = " INNER JOIN 工况记录表3_高_高压工况 ON 车辆基本信息3.车辆ID = 工况记录表3_高_高压工况.车辆ID";
            foreach (string s in fields)
            {
                joins.Add($"INNER JOIN {s} ON {carinfo}.车辆ID = {s}.车辆ID");
            }
            foreach (string s in joins)
            {
                fromtable = $"({fromtable}{s})";
            }
        }
        void ConditionGenerate() {
            condition = $"{carinfo}.车辆ID = (SELECT MAX(车辆ID) FROM {carinfo})";
        }
    }
}
