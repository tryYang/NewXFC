using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFC.Helper
{
    public  class ValueConverter
    {
        public static double minMavalue = ConstantValue.minMAValue;
        public static double maxMavalue = ConstantValue.maxMAValue;
        //------水利性能---------
        //低压压力
        public const double LPRESS_BEGIN = 0;
        public const double LPRESS_END = 4;
        //中高压压力
        public const double LHPRESS_BEGIN = 0;
        public const double LHPRESS_END = 6;
        //真空压力
        public const double REALPRESS_BEGIN = -0.1;
        public const double REALPRESS_END = 0;
        //DN50 流量
        public const double DN50_BEGIN = 1.4;
        public const double DN50_END = 13;
        //DN100 流量
        public const double DN100_BEGIN = 7;
        public const double DN100_END = 44;
        //DN200 流量
        public const double DN200_BEGIN = 29;
        public const double DN200_END = 167;
        //DN300 流量
        public const double DN300_BEGIN = 36;
        public const double DN300_END = 500;
        //DN50 DN100 DN200 DN300 阀门（MPa）
        public const double Valve_BEGIN = 0;
        public const double Valve_END = 6.4;
       

        //------车辆状态---------
        //消防泵转速----------------------------注意
        public const double PUMPSPEED_BEGIN = 1;
        public const double PUMPSPEED_END = 30000;
        //输入轴温度
        public const double INTEMP_BEGIN = 0;
        public const double INTEMP_END = 150;
        //输出轴温度
        public const double OUTTEMP_BEGIN = 0;
        public const double OUTTEMP_END = 150;

        //------环境参数---------

        //大气压力
        public const double TEMP_BEGIN = 60;
        public const double TEMP_END = 110;
        //环境温度
        public const double PREESURE_BEGIN = -40;
        public const double PREESURE_END = 60;
        //三米水池水温
        public const double TEMP_3M_BEGIN = 0;
        public const double TEMP_3M_END = 50;
        //三米水池水位
        public const double HIGH_3M_BEGIN = 0;
        public const double HIGH_3M_END = 10;
        //七米水池水温
        public const double TEMP_7M_BEGIN = 0;
        public const double TEMP_7M_END = 50;
        //七米水池水位
        public const double HIGH_7M_BEGIN = 0;
        public const double HIGH_7M_END = 10;

        public static double LPressConverter(double value)
        {
            return ValueConverterTemplate(LPRESS_BEGIN,LPRESS_END, value);
        }
        public static double LHPressConverter(double value)
        {
            return ValueConverterTemplate(LHPRESS_BEGIN, LHPRESS_END, value);
        }

        public static double RealPressConverter(double value)
        {
            return ValueConverterTemplate(REALPRESS_BEGIN, REALPRESS_END, value);
        }
        public static double DN50Converter(double value)
        {
            return ValueConverterTemplate(DN50_BEGIN, DN50_END, value);
        }
        public static double DN100Converter(double value)
        {
            return ValueConverterTemplate(DN100_BEGIN, DN100_END, value);
        }
        public static double DN200Converter(double value)
        {
            return ValueConverterTemplate(DN200_BEGIN, DN200_END, value);
        }
        public static double DN300Converter(double value)
        {
            return ValueConverterTemplate(DN300_BEGIN, DN300_END, value);
        }
        public static double ValveConverter(double value)
        {
            return ValueConverterTemplate(Valve_BEGIN, Valve_END, value);
        }
        public static double PumpSpeedConverter(double value)
        {
            return ValueConverterTemplate(PUMPSPEED_BEGIN,PUMPSPEED_END, value);
        }
        public static double InTempConverter(double value)
        {
            return ValueConverterTemplate(INTEMP_BEGIN, INTEMP_END, value);
        }
        public static double OutTempConverter(double value)
        {
            return ValueConverterTemplate(OUTTEMP_BEGIN, OUTTEMP_END, value);
        }
        public static double TempConverter(double value)
        {
            return ValueConverterTemplate(TEMP_BEGIN, TEMP_END, value);
        }
        public static double PreesureConverter(double value)
        {
            return ValueConverterTemplate(PREESURE_BEGIN, PREESURE_END, value);
        }
        public static double Temp_3MConverter(double value)
        {
            return ValueConverterTemplate(TEMP_3M_BEGIN, TEMP_3M_END, value);
        }
        public static double High_3MConverter(double value)
        {
            return ValueConverterTemplate(HIGH_3M_BEGIN, HIGH_3M_END, value);
        }
        public static double Temp_7MConverter(double value)
        {
            return ValueConverterTemplate(TEMP_7M_BEGIN, TEMP_7M_END, value);
        }
        public static double High_7MConverter(double value)
        {
            return ValueConverterTemplate(HIGH_7M_BEGIN, HIGH_7M_END, value);
        }
        public static double ValueConverterTemplate(double minvalue,double maxvalue,double realMavalue)
        {
            return ((maxvalue-minvalue)/(maxMavalue-minMavalue))*realMavalue;
        }
    }
}
