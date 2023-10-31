using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Helper
{
    public class PrintSqlGenerateHelper
    {
        string select = string.Empty;
        string fromtable = string.Empty;
        string template = "Select {0} from {1} where {2} GROUP BY {3}";
        string condition= string.Empty;
        string groupby = string.Empty;
        List<int> gkList = new List<int>();
        List<string> selectList = new List<string>() {
         @"(select CollecTime from ConditionRecord where LabID = 1)  as 低压工况采集时间,avg(select L_Press from ConditionRecord where LabID = 1) as 低压工况低压压力,avg(select L_Flow from ConditionRecord where LabID = 1) as 低压工况低压流量,avg(select H_Press from ConditionRecord where LabID = 1) as 低压工况中高压压力,avg(select H_Flow from ConditionRecord where LabID = 1) as 低压工况中高压流量,avg(select VacuumDegree from ConditionRecord where LabID = 1) as 低压工况真空度,avg(select Speed from ConditionRecord where LabID = 1) as 低压工况消防泵转速,avg(select InTemp from ConditionRecord where LabID = 1) as 低压工况输入轴温度,avg(select OutTemp from ConditionRecord where LabID = 1) as 低压工况输出轴温度",
         @"(select CollecTime from ConditionRecord where LabID = 2) as 一点三工况采集时间,avg(select L_Press from ConditionRecord where LabID = 2) as 一点三工况低压压力,avg(select L_Flow from ConditionRecord where LabID = 2) as 一点三工况低压流量,avg(select H_Press from ConditionRecord where LabID = 2) as 一点三工况中高压压力,avg(select H_Flow from ConditionRecord where LabID = 2) as 一点三工况中高压流量,avg(select VacuumDegree from ConditionRecord where LabID = 2) as 一点三工况真空度,avg(select Speed from ConditionRecord where LabID = 2) as 一点三工况消防泵转速,avg(select InTemp from ConditionRecord where LabID = 2) as 一点三工况输入轴温度,avg(select OutTemp from ConditionRecord where LabID = 2) as 一点三工况输出轴温度",
         @"(select CollecTime from ConditionRecord where LabID = 3) as 超负荷工况采集时间,avg(select L_Press from ConditionRecord where LabID = 3) as 超负荷工况低压压力,avg(select L_Flow from ConditionRecord where LabID = 3) as 超负荷工况低压流量,avg(select H_Press from ConditionRecord where LabID = 3) as 超负荷工况中高压压力,avg(select H_Flow from ConditionRecord where LabID = 3) as 超负荷工况中高压流量,avg(select VacuumDegree from ConditionRecord where LabID = 3) as 超负荷工况真空度,avg(select Speed from ConditionRecord where LabID = 3) as 超负荷工况消防泵转速,avg(select InTemp from ConditionRecord where LabID = 3) as 超负荷工况输入轴温度,avg(select OutTemp from ConditionRecord where LabID = 3) as 超负荷工况输出轴温度",
         @"(select CollecTime from ConditionRecord where LabID = 4) as 半流量工况采集时间,avg(select L_Press from ConditionRecord where LabID = 4) as 半流量工况低压压力,avg(select L_Flow from ConditionRecord where LabID = 4) as 半流量工况低压流量,avg(select H_Press from ConditionRecord where LabID = 4) as 半流量工况中高压压力,avg(select H_Flow from ConditionRecord where LabID = 4) as 半流量工况中高压流量,avg(select VacuumDegree from ConditionRecord where LabID = 4) as 半流量工况真空度,avg(select Speed from ConditionRecord where LabID = 4) as 半流量工况消防泵转速,avg(select InTemp from ConditionRecord where LabID = 4) as 半流量工况输入轴温度,avg(select OutTemp from ConditionRecord where LabID = 4) as 半流量工况输出轴温度",
         @"(select CollecTime from ConditionRecord where LabID = 5) as 高压工况采集时间,avg(select L_Press from ConditionRecord where LabID = 5) as 高压工况低压压力,avg(select L_Flow from ConditionRecord where LabID = 5) as 高压工况低压流量,avg(select H_Press from ConditionRecord where LabID = 5) as 高压工况中高压压力,avg(select H_Flow from ConditionRecord where LabID = 5) as 高压工况中高压流量,avg(select VacuumDegree from ConditionRecord where LabID = 5) as 高压工况真空度,avg(select Speed from ConditionRecord where LabID = 5) as 高压工况消防泵转速,avg(select InTemp from ConditionRecord where LabID = 5) as 高压工况输入轴温度,avg(select OutTemp from ConditionRecord where LabID = 5) as 高压工况输出轴温度",
         @"(select CollecTime from ConditionRecord where LabID = 6) as 中压工况采集时间,avg(select L_Press from ConditionRecord where LabID = 6) as 中压工况低压压力,avg(select L_Flow from ConditionRecord where LabID = 6) as 中压工况低压流量,avg(select H_Press from ConditionRecord where LabID = 6) as 中压工况中高压压力,avg(select H_Flow from ConditionRecord where LabID = 6) as 中压工况中高压流量,avg(select VacuumDegree from ConditionRecord where LabID = 6) as 中压工况真空度,avg(select Speed from ConditionRecord where LabID = 6) as 中压工况消防泵转速,avg(select InTemp from ConditionRecord where LabID = 6) as 中压工况输入轴温度,avg(select OutTemp from ConditionRecord where LabID = 6) as 中压工况输出轴温度"
        };
        
        public PrintSqlGenerateHelper(List<int> list)
        {
            gkList=list;
        }
        public string Generate()
        {
            SelectGenerate();
            TableGenerate();
            ConditionGenerate();
            GroupByGenerate();
            return string.Format(template, select, fromtable, condition, groupby);
        }
        void TableGenerate()
        {
            fromtable = @"(CarLab inner join CarBasicInfo on CarLab.CarID=CarBasicInfo.CarID) inner join ConditionRecord on CarLab.LabID=ConditionRecord.LabID";
        }
        void SelectGenerate()
        {
            select = @"CheckPeople as 实验人员,CutomerDepart as 送检单位,CarName as 车辆名称,UnderpanFac as 底盘厂家,PumpFac as 水泵厂家,CarNum as 车牌号,CarModel as 车辆型号,UnderpanModel as 底盘型号,PumpModel as 水泵型号,CarFac as 生产厂家,UnderpanVIN as 底盘VIN,PumpType as 水泵类型,Pressure as 大气压力,ThreeTemp as 三米水池温度,ThreePress as 三米水池修正吸深,SevenTemp as 七米水池温度,SevenPress as 七米水池修正吸深,";            
            for(int i=0;i< gkList.Count;i++)
            {
                string temp = selectList[i];
                if (i+1!= gkList.Count) {
                    temp+=',';
                }
                select += temp;
            }
        }
        void ConditionGenerate()
        {
            condition = @"CarLab.CarID =(select Max(CarID) from CarLab)";
        }
        void GroupByGenerate()
        {
            groupby = @"CheckPeople ,CutomerDepart ,CarName ,UnderpanFac ,PumpFac ,CarNum ,CarModel ,UnderpanModel ,PumpModel ,CarFac ,UnderpanVIN ,PumpType ,Pressure ,ThreeTemp ,ThreePress  ,SevenTemp ,SevenPress ,CollecTime";
        }
    }
}
