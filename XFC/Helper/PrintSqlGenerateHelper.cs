using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFC.Model;

namespace XFC.Helper
{
    public class PrintSqlGenerateHelper
    {
        string select_template = @"Avg(CollectTime) as {0} , Avg(L_Press) as {1},Avg(L_Flow) as {2}, Avg(H_Press) as {3}, Avg(H_Flow) as {4}, Avg(VacuumDegree) as {5}, Avg(Speed) as {6}, Avg(InTemp) as {7}, Avg(OutTemp) as {8}";
        List<List<string>> FieldList = new List<List<string>>() {
           new List<string>(){ "低压工况采集时间","低压工况低压压力","低压工况低压流量","低压工况中高压压力","低压工况中高压流量","低压工况真空度","低压工况消防泵转速","低压工况输入轴温度","低压工况输出轴温度"    },
           new List<string>(){ "一点三工况采集时间", "一点三工况低压压力","一点三工况低压流量","一点三工况中高压压力","一点三工况中高压流量","一点三工况真空度","一点三工况消防泵转速","一点三工况输入轴温度","一点三工况输出轴温度"    },
           new List<string>(){ "超负荷工况采集时间", "超负荷工况低压压力","超负荷工况低压流量","超负荷工况中高压压力","超负荷工况中高压流量","超负荷工况真空度","超负荷工况消防泵转速","超负荷工况输入轴温度","超负荷工况输出轴温度"    },
           new List<string>(){ "半流量工况采集时间", "半流量工况低压压力","半流量工况低压流量","半流量工况中高压压力","半流量工况中高压流量","半流量工况真空度","半流量工况消防泵转速","半流量工况输入轴温度","半流量工况输出轴温度"    },
           new List<string>(){ "高压工况采集时间","高压工况低压压力","高压工况低压流量","高压工况中高压压力","高压工况中高压流量","高压工况真空度","高压工况消防泵转速","高压工况输入轴温度","高压工况输出轴温度"    },
           new List<string>(){ "中压工况采集时间","中压工况低压压力","中压工况低压流量","中压工况中高压压力","中压工况中高压流量","中压工况真空度","中压工况消防泵转速","中压工况输入轴温度","中压工况输出轴温度"    },

        };
        //List<List<string>> FieldList = new List<List<string>>() {
        //   new List<string>(){ "低压工况采集时间","低压工况低压压力","低压工况低压流量","低压工况中高压压力","低压工况中高压流量","低压工况真空度","低压工况消防泵转速","低压工况输入轴温度","低压工况输出轴温度"    },
        //   new List<string>(){ "一点三工况采集时间", "一点三工况低压压力","一点三工况低压流量","一点三工况中高压压力","一点三工况中高压流量","一点三工况真空度","一点三工况消防泵转速","一点三工况输入轴温度","一点三工况输出轴温度"    },
        //   new List<string>(){ "超负荷工况采集时间", "超负荷工况低压压力","超负荷工况低压流量","超负荷工况中高压压力","超负荷工况中高压流量","超负荷工况真空度","超负荷工况消防泵转速","超负荷工况输入轴温度","超负荷工况输出轴温度"    },
        //   new List<string>(){ "半流量工况采集时间", "半流量工况低压压力","半流量工况低压流量","半流量工况中高压压力","半流量工况中高压流量","半流量工况真空度","半流量工况消防泵转速","半流量工况输入轴温度","半流量工况输出轴温度"    },
        //   new List<string>(){ "高压工况采集时间","高压工况低压压力","高压工况低压流量","高压工况中高压压力","高压工况中高压流量","高压工况真空度","高压工况消防泵转速","高压工况输入轴温度","高压工况输出轴温度"    },
        //   new List<string>(){ "中压工况采集时间","中压工况低压压力","中压工况低压流量","中压工况中高压压力","中压工况中高压流量","中压工况真空度","中压工况消防泵转速","中压工况输入轴温度","中压工况输出轴温度"    },

        //};
        string select = string.Empty;
        string fromtable = string.Empty;
        string template = "Select {0} from {1} where {2} ";
        string condition = string.Empty;
        Equipment _equipment = Equipment.None;
        List<int> gkList = new List<int>();
        int LabId = 0;
        int id= 0;// 车则是CarId 泵是PumpId
      
        public PrintSqlGenerateHelper(List<int> list,Equipment equipment, List<int> Id)
        {
            gkList = list;
            _equipment = equipment;
            id = Id[0];
            LabId = Id[1];
        }


        public string singleGenerate(int i)
        {
            SelectGenerate(i);
            TableGenerate();
            ConditionGenerate(i);            
            return string.Format(template, select, fromtable, condition);
        }


        void TableGenerate()//表名需改变
        {
            switch (_equipment)
            {
                case Equipment.None:
                    return;
                case Equipment.Car:
                    fromtable = @"ConditionRecord";
                    break;
                case Equipment.Pump:
                    fromtable = @"PumpConditionRecord";
                    break;
            }
        }
        void SelectGenerate(int i)//基本信息需改变
        {

            select = string.Format(select_template, FieldList[i][0], FieldList[i][1], FieldList[i][2], FieldList[i][3], FieldList[i][4], FieldList[i][5], FieldList[i][6], FieldList[i][7], FieldList[i][8]);
            //switch (_equipment)
            //{
            //    case Equipment.None:
            //        return;
            //    case Equipment.Car:
            //        select = string.Format(select_template, FieldList[i][0], FieldList[i][1], FieldList[i][2], FieldList[i][3], FieldList[i][4], FieldList[i][5], FieldList[i][6]);
            //        break;
            //    case Equipment.Pump:
            //        select = string.Format(select_template, FieldList[i][0], FieldList[i][1], FieldList[i][2], FieldList[i][3], FieldList[i][4], FieldList[i][5], FieldList[i][6]);
            //        break;
            //}            
        }
        void ConditionGenerate(int i)//需改变，且有两辆车实验时主要为这里改变
        {
            if (_equipment == Equipment.Car)
                condition = $"LabID = {LabId} And ConditionNum={i}";
            else if (_equipment == Equipment.Pump)            
                condition = $"PumpLabID = {LabId} And ConditionNum={i}";
        }

       
        string carbasicInfosql = "Select CheckPeople as 实验人员,CustomerDepart as 送检单位,CarName as 车辆名称,UnderpanFac as 底盘厂家,PumpFac as 水泵厂家,CarNum as 车牌号,CarModel as 车辆型号,UnderpanModel as 底盘型号,PumpModel as 水泵型号,CarFac as 生产厂家, UnderpanVIN as 底盘VIN,PumpType as 水泵类型,Pressure as 大气压力, ThreeTemp as 三米水池温度, ThreePress as 三米水池修正吸深, SevenTemp as 七米水池温度, SevenPress as 七米水池修正吸深  from CarLab inner join CarBasicInfo on CarLab.CarID=CarBasicInfo.CarID where CarLab.CarID ={0}";
        string pumpbasicInfosql_template = "Select CheckPeople as 实验人员,CustomerDepart as 送检单位,PumpName as 水泵名称,PumpFac as 水泵厂家,PumpType as 水泵类型,InPipeD as 进口管径,OutPipeD as 出口管径,EpitopeDifference as 表位差,PumpModel as 水泵型号,Pressure as 大气压力,ThreeTemp as 三米水池温度,ThreePress as 三米水池修正吸深,SevenTemp as 七米水池温度,SevenPress as 七米水池修正吸深 from PumpLab inner join PumpBasicInfo on PumpLab.PumpID=PumpBasicInfo.PumpID where PumpLab.PumpID ={0}";
        public DataSet GetReportDataSet()
        {
            DataSet reportDataSet = new DataSet();
            DataSet basicInfoDataSet = new DataSet();

            string basicInfoSql=string.Empty;
            if (_equipment == Equipment.Car)
            {
                basicInfoSql = string.Format(carbasicInfosql, id);
            }
            else if(_equipment == Equipment.Pump)
            {

                basicInfoSql =string.Format(pumpbasicInfosql_template, id);
            }
            using (OledbHelper helper = new OledbHelper())
            {
                helper.sqlstring= basicInfoSql;
                basicInfoDataSet = helper.GetDataSet();
                reportDataSet.Merge(basicInfoDataSet.Tables[0]);
            }



            for (int i = 0; i < gkList.Count; i++)
            {
                DataSet temp = new DataSet();
                using (OledbHelper helper =new OledbHelper())
                {
                    helper.sqlstring = singleGenerate(gkList[i]);
                    temp = helper.GetDataSet();
                }
                DataRow row = temp.Tables[0].Rows[0];
                foreach (DataColumn col in temp.Tables[0].Columns)
                {
                    AddFieldAndValue(reportDataSet.Tables[0], col.ColumnName, row[col]);
                }
               
            }
          
            return reportDataSet;
        }
        static void AddFieldAndValue(System.Data.DataTable dataTable, string fieldName, object value)
        {
            // 判断字段是否已存在
            if (!dataTable.Columns.Contains(fieldName))
            {
                // 添加新的列
                DataColumn newColumn = new DataColumn(fieldName, value.GetType());
                dataTable.Columns.Add(newColumn);

                Console.WriteLine($"Added new field: {fieldName}");

                // 在所有行中添加相应的值
                foreach (DataRow row in dataTable.Rows)
                {
                    row[fieldName] = value;
                }
            }
            else
            {
                Console.WriteLine($"Field {fieldName} already exists.");
            }
        }
    }

}
