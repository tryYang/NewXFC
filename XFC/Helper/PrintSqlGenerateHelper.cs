using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFC.Model;

namespace XFC.Helper
{
    public class PrintSqlGenerateHelper
    {
        string select = string.Empty;
        string fromtable = string.Empty;
        string template = "Select {0} from {1} where {2} GROUP BY {3}";
        string condition = string.Empty;
        string groupby = string.Empty;
        Equipment _equipment = Equipment.None;
        List<int> gkList = new List<int>();
        int LabId = 0;
        List<string> carselectList = new List<string>() {
         @"(select Avg(CollectTime) from ConditionRecord where ConditionNum =1) as 低压工况采集时间,(select Avg(L_Press) from ConditionRecord where ConditionNum = 1) as 低压工况低压压力,(select Avg(L_Flow) from ConditionRecord where ConditionNum = 1) as 低压工况低压流量,(select Avg(H_Press) from ConditionRecord where ConditionNum = 1) as 低压工况中高压压力,(select Avg(H_Flow) from ConditionRecord where ConditionNum = 1) as 低压工况中高压流量,(select avg(VacuumDegree) from ConditionRecord where ConditionNum = 1) as 低压工况真空度,(select avg(Speed) from ConditionRecord where ConditionNum = 1) as 低压工况消防泵转速,(select avg(InTemp) from ConditionRecord where ConditionNum = 1) as 低压工况输入轴温度,(select avg(OutTemp) from ConditionRecord where ConditionNum = 1) as 低压工况输出轴温度",
         @"(select Avg(CollectTime) from ConditionRecord where ConditionNum = 2) as 一点三工况采集时间,(select Avg(L_Press) from ConditionRecord where ConditionNum = 2) as 一点三工况低压压力,(select Avg(L_Flow) from ConditionRecord where ConditionNum = 2) as 一点三工况低压流量,(select Avg(H_Press) from ConditionRecord where ConditionNum = 2) as 一点三工况中高压压力,(select Avg(H_Flow) from ConditionRecord where ConditionNum = 2) as 一点三工况中高压流量,(select  Avg(VacuumDegree) from ConditionRecord where ConditionNum = 2) as 一点三工况真空度,(select Avg(Speed )from ConditionRecord where ConditionNum = 2) as 一点三工况消防泵转速,(select Avg(InTemp )from ConditionRecord where ConditionNum = 2) as 一点三工况输入轴温度,(select Avg(OutTemp )from ConditionRecord where ConditionNum = 2) as 一点三工况输出轴温度",
         @"(select Avg(CollectTime) from ConditionRecord where ConditionNum = 3) as 超负荷工况采集时间,(select Avg(L_Press) from ConditionRecord where ConditionNum = 3) as 超负荷工况低压压力,(select Avg(L_Flow) from ConditionRecord where ConditionNum = 3) as 超负荷工况低压流量,(select Avg(H_Press) from ConditionRecord where ConditionNum = 3) as 超负荷工况中高压压力,(select Avg(H_Flow) from ConditionRecord where ConditionNum = 3) as 超负荷工况中高压流量,(select Avg(VacuumDegree )from ConditionRecord where ConditionNum = 3) as 超负荷工况真空度,(select Avg(Speed )from ConditionRecord where ConditionNum = 3) as 超负荷工况消防泵转速,(select Avg(InTemp )from ConditionRecord where ConditionNum = 3) as 超负荷工况输入轴温度,(select Avg(OutTemp )from ConditionRecord where ConditionNum = 3) as 超负荷工况输出轴温度",
         @"(select Avg(CollectTime) from ConditionRecord where ConditionNum = 4) as 半流量工况采集时间,(select Avg(L_Press) from ConditionRecord where ConditionNum = 4) as 半流量工况低压压力,(select Avg(L_Flow) from ConditionRecord where ConditionNum = 4) as 半流量工况低压流量,(select Avg(H_Press) from ConditionRecord where ConditionNum = 4) as 半流量工况中高压压力,(select Avg(H_Flow) from ConditionRecord where ConditionNum = 4) as 半流量工况中高压流量,(select Avg(VacuumDegree )from ConditionRecord where ConditionNum = 4) as 半流量工况真空度,(select Avg(Speed )from ConditionRecord where ConditionNum = 4) as 半流量工况消防泵转速,(select Avg(InTemp )from ConditionRecord where ConditionNum = 4) as 半流量工况输入轴温度,(select Avg(OutTemp )from ConditionRecord where ConditionNum = 4) as 半流量工况输出轴温度",
         @"(select Avg(CollectTime) from ConditionRecord where ConditionNum = 5) as 高压工况采集时间,(select Avg(L_Press) from ConditionRecord where ConditionNum = 5) as 高压工况低压压力,(select Avg(L_Flow) from ConditionRecord where ConditionNum = 5) as 高压工况低压流量,(select Avg(H_Press) from ConditionRecord where ConditionNum = 5) as 高压工况中高压压力,(select Avg(H_Flow) from ConditionRecord where ConditionNum = 5) as 高压工况中高压流量,(select Avg(VacuumDegree )from ConditionRecord where ConditionNum = 5) as 高压工况真空度,(select Avg(Speed )from ConditionRecord where ConditionNum = 5) as 高压工况消防泵转速,(select Avg(InTemp )from ConditionRecord where ConditionNum = 5) as 高压工况输入轴温度,(select Avg(OutTemp )from ConditionRecord where ConditionNum = 5) as 高压工况输出轴温度",
         @"(select Avg(CollectTime) from ConditionRecord where ConditionNum = 6) as 中压工况采集时间,(select Avg(L_Press) from ConditionRecord where ConditionNum = 6) as 中压工况低压压力,(select Avg(L_Flow) from ConditionRecord where ConditionNum = 6) as 中压工况低压流量,(select Avg(H_Press) from ConditionRecord where ConditionNum = 6) as 中压工况中高压压力,(select Avg(H_Flow) from ConditionRecord where ConditionNum = 6) as 中压工况中高压流量,(select Avg(VacuumDegree )from ConditionRecord where ConditionNum = 6) as 中压工况真空度,(select Avg(Speed )from ConditionRecord where ConditionNum = 6) as 中压工况消防泵转速,(select Avg(InTemp )from ConditionRecord where ConditionNum = 6) as 中压工况输入轴温度,(select Avg(OutTemp )from ConditionRecord where ConditionNum = 6) as 中压工况输出轴温度"
    };
        List<string> pumpselectList = new List<string>() {
              @"(select Avg(CollectTime) from PumpConditionRecord where ConditionNum =1) as 低压工况采集时间,(select Avg(L_Press) from PumpConditionRecord where ConditionNum = 1) as 低压工况低压压力,(select Avg(L_Flow) from PumpConditionRecord where ConditionNum = 1) as 低压工况低压流量,(select Avg(H_Press) from PumpConditionRecord where ConditionNum = 1) as 低压工况中高压压力,(select Avg(H_Flow) from PumpConditionRecord where ConditionNum = 1) as 低压工况中高压流量,(select avg(VacuumDegree) from PumpConditionRecord where ConditionNum = 1) as 低压工况真空度,(select avg(Speed) from PumpConditionRecord where ConditionNum = 1) as 低压工况消防泵转速,(select avg(InTemp) from PumpConditionRecord where ConditionNum = 1) as 低压工况输入轴温度,(select avg(OutTemp) from PumpConditionRecord where ConditionNum = 1) as 低压工况输出轴温度",
              @"(select Avg(CollectTime) from PumpConditionRecord where ConditionNum = 2) as 一点三工况采集时间,(select Avg(L_Press) from PumpConditionRecord where ConditionNum = 2) as 一点三工况低压压力,(select Avg(L_Flow) from PumpConditionRecord where ConditionNum = 2) as 一点三工况低压流量,(select Avg(H_Press) from PumpConditionRecord where ConditionNum = 2) as 一点三工况中高压压力,(select Avg(H_Flow) from PumpConditionRecord where ConditionNum = 2) as 一点三工况中高压流量,(select  Avg(VacuumDegree) from PumpConditionRecord where ConditionNum = 2) as 一点三工况真空度,(select Avg(Speed )from PumpConditionRecord where ConditionNum = 2) as 一点三工况消防泵转速,(select Avg(InTemp )from PumpConditionRecord where ConditionNum = 2) as 一点三工况输入轴温度,(select Avg(OutTemp )from PumpConditionRecord where ConditionNum = 2) as 一点三工况输出轴温度",
              @"(select Avg(CollectTime) from PumpConditionRecord where ConditionNum = 3) as 超负荷工况采集时间,(select Avg(L_Press) from PumpConditionRecord where ConditionNum = 3) as 超负荷工况低压压力,(select Avg(L_Flow) from PumpConditionRecord where ConditionNum = 3) as 超负荷工况低压流量,(select Avg(H_Press) from PumpConditionRecord where ConditionNum = 3) as 超负荷工况中高压压力,(select Avg(H_Flow) from PumpConditionRecord where ConditionNum = 3) as 超负荷工况中高压流量,(select Avg(VacuumDegree )from PumpConditionRecord where ConditionNum = 3) as 超负荷工况真空度,(select Avg(Speed )from PumpConditionRecord where ConditionNum = 3) as 超负荷工况消防泵转速,(select Avg(InTemp )from PumpConditionRecord where ConditionNum = 3) as 超负荷工况输入轴温度,(select Avg(OutTemp )from PumpConditionRecord where ConditionNum = 3) as 超负荷工况输出轴温度",
              @"(select Avg(CollectTime) from PumpConditionRecord where ConditionNum = 4) as 半流量工况采集时间,(select Avg(L_Press) from PumpConditionRecord where ConditionNum = 4) as 半流量工况低压压力,(select Avg(L_Flow) from PumpConditionRecord where ConditionNum = 4) as 半流量工况低压流量,(select Avg(H_Press) from PumpConditionRecord where ConditionNum = 4) as 半流量工况中高压压力,(select Avg(H_Flow) from PumpConditionRecord where ConditionNum = 4) as 半流量工况中高压流量,(select Avg(VacuumDegree )from PumpConditionRecord where ConditionNum = 4) as 半流量工况真空度,(select Avg(Speed )from PumpConditionRecord where ConditionNum = 4) as 半流量工况消防泵转速,(select Avg(InTemp )from PumpConditionRecord where ConditionNum = 4) as 半流量工况输入轴温度,(select Avg(OutTemp )from PumpConditionRecord where ConditionNum = 4) as 半流量工况输出轴温度",
              @"(select Avg(CollectTime) from PumpConditionRecord where ConditionNum = 5) as 高压工况采集时间,(select Avg(L_Press) from PumpConditionRecord where ConditionNum = 5) as 高压工况低压压力,(select Avg(L_Flow) from PumpConditionRecord where ConditionNum = 5) as 高压工况低压流量,(select Avg(H_Press) from PumpConditionRecord where ConditionNum = 5) as 高压工况中高压压力,(select Avg(H_Flow) from PumpConditionRecord where ConditionNum = 5) as 高压工况中高压流量,(select Avg(VacuumDegree )from PumpConditionRecord where ConditionNum = 5) as 高压工况真空度,(select Avg(Speed )from PumpConditionRecord where ConditionNum = 5) as 高压工况消防泵转速,(select Avg(InTemp )from PumpConditionRecord where ConditionNum = 5) as 高压工况输入轴温度,(select Avg(OutTemp )from PumpConditionRecord where ConditionNum = 5) as 高压工况输出轴温度",
              @"(select Avg(CollectTime) from PumpConditionRecord where ConditionNum = 6) as 中压工况采集时间,(select Avg(L_Press) from PumpConditionRecord where ConditionNum = 6) as 中压工况低压压力,(select Avg(L_Flow) from PumpConditionRecord where ConditionNum = 6) as 中压工况低压流量,(select Avg(H_Press) from PumpConditionRecord where ConditionNum = 6) as 中压工况中高压压力,(select Avg(H_Flow) from PumpConditionRecord where ConditionNum = 6) as 中压工况中高压流量,(select Avg(VacuumDegree )from PumpConditionRecord where ConditionNum = 6) as 中压工况真空度,(select Avg(Speed )from PumpConditionRecord where ConditionNum = 6) as 中压工况消防泵转速,(select Avg(InTemp )from PumpConditionRecord where ConditionNum = 6) as 中压工况输入轴温度,(select Avg(OutTemp )from PumpConditionRecord where ConditionNum = 6) as 中压工况输出轴温度"
 };

        public PrintSqlGenerateHelper(List<int> list,Equipment equipment, int labId)
        {
            gkList = list;
            _equipment = equipment;
            LabId = labId;
        }
        public string Generate()
        {
            SelectGenerate();
            TableGenerate();
            ConditionGenerate();
            GroupByGenerate();
            return string.Format(template, select, fromtable, condition, groupby);
        }
        void TableGenerate()//表名需改变
        {
            if(_equipment==Equipment.Car)
                fromtable = @"(CarLab inner join CarBasicInfo on CarLab.CarID=CarBasicInfo.CarID) inner join ConditionRecord on CarLab.LabID=ConditionRecord.LabID";
            else if (_equipment == Equipment.Pump)            
                fromtable = @"(PumpLab inner join PumpBasicInfo on PumpLab.PumpID=PumpBasicInfo.PumpID) inner join PumpConditionRecord on PumpLab.PumpLabID=PumpConditionRecord.PumpLabID";
            
        }
        void SelectGenerate()//基本信息需改变
        {                        
            List<string> selectList = new List<string>();
            switch (_equipment)
            {
                case Equipment.None:
                    return;
                case Equipment.Car:
                    select = @"CheckPeople as 实验人员,CustomerDepart as 送检单位,CarName as 车辆名称,UnderpanFac as 底盘厂家,PumpFac as 水泵厂家,CarNum as 车牌号,CarModel as 车辆型号,UnderpanModel as 底盘型号,PumpModel as 水泵型号,CarFac as 生产厂家,UnderpanVIN as 底盘VIN,PumpType as 水泵类型,Pressure as 大气压力,ThreeTemp as 三米水池温度,ThreePress as 三米水池修正吸深,SevenTemp as 七米水池温度,SevenPress as 七米水池修正吸深,";
                    selectList = carselectList;
                    break;
                case Equipment.Pump:
                    select = @"CheckPeople as 实验人员,CustomerDepart as 送检单位,PumpName as 水泵名称,PumpFac as 水泵厂家,PumpType as 水泵类型,InPipeD as 进口管径,OutPipeD as 出口管径,EpitopeDifference as 表位差,PumpModel as 水泵型号,Pressure as 大气压力,ThreeTemp as 三米水池温度,ThreePress as 三米水池修正吸深,SevenTemp as 七米水池温度,SevenPress as 七米水池修正吸深,";
                    selectList = pumpselectList;
                    break;
            }
            for (int i = 0; i < gkList.Count; i++)
            {
               
                string temp = selectList[gkList[i]];
                if (i + 1 != gkList.Count)
                {
                    temp += ',';
                }
                select += temp;
            }
        }
        void ConditionGenerate()//需改变，且有两辆车实验时主要为这里改变
        {
            if (_equipment == Equipment.Car)
                condition = $"CarLab.LabID = {LabId}";
            else if (_equipment == Equipment.Pump)            
                condition = $"PumpLab.PumpLabID = {LabId}";
        }
        void GroupByGenerate()//对应非聚合函数字段，主要为基本信息，需改变
        {
            if (_equipment == Equipment.Car)
                groupby = @"CheckPeople ,CustomerDepart ,CarName ,UnderpanFac ,PumpFac ,CarNum ,CarModel ,UnderpanModel ,PumpModel ,CarFac ,UnderpanVIN ,PumpType ,Pressure ,ThreeTemp ,ThreePress  ,SevenTemp ,SevenPress";
            else if (_equipment == Equipment.Pump)
                groupby = @"CheckPeople ,CustomerDepart ,PumpName,PumpFac,PumpType,InPipeD,OutPipeD,EpitopeDifference,PumpModel,Pressure,ThreeTemp ,ThreePress  ,SevenTemp ,SevenPress";

        }
    }
}
