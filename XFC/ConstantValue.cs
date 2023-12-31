﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XFC.Model;

namespace XFC
{
    public class ConstantValue
    {
        public static Threshold threshold = new Threshold();
        public static List<int> ID = new List<int> { 1, 2 };
        public static List<string> L_Flowmeter = new List<string> {"DN100","DN200","DN300" };
        public static List<string> H_Flowmeter = new List<string> { "DN50", "DN100" };
        public static GkStatus gkStatus = GkStatus.Uncheck;
        public static List<string> WaterPumpType = new List<string> { "低压泵","中压泵","高压泵","中低压泵", "高低压泵" };
        public static string AppSetting=" ";
        public static List<XfcInfo> xfcInfos =new List<XfcInfo>() { new XfcInfo(1), new XfcInfo(2)};
        public static List<XfbInfo> xfbInfos = new List<XfbInfo>() { new XfbInfo(1), new XfbInfo(2) };
        public static List<string> Baurates =new List<string>() {
                         "2400", "4800", "9600", "19200" 
       };
        //控制保存数据间隔
        public const int SAVE_DATA_INTEINTERVALS = 4;
        public  const int DATA_SHOW_INTERVALS = 1000;
        public const int DATA_SAVE_INTERVALS = 1000*60;
        public static int Tick_Num = 0;//定时器被触发的次数
        //采样间隔  默认60次  即1分钟
        public static int Sampling_Time = 60;
        public static System.Timers.Timer DataShowTimer = new System.Timers.Timer(1000) ;
       
        //ValueConvter
        public static double minMAValue = 4;//4 mA
        public static double maxMAValue = 20;//20 mA

        public static ConditionRecord RealTimeData1;
        public static ConditionRecord RealTimeData2;

        public static long runtime1= 0;
        public static long runtime2 = 0;

        public static List<Equipment> EquipemntList = new List<Equipment>() { Equipment.None, Equipment.None};
        public static List<PumpType>PumpTypeList = new List<PumpType>() { PumpType.None,PumpType.None};//怎么把水泵类型信息传过来呢


        public static int LastCarID = 0;
        public static int LastCarLabID = 0;
        public static int LastPumpID = 0;
        public static int LastPumpLabID = 0;

        /// <summary>
        /// 記錄需要插入ID 第一個索引為設備號，第二個為 0為 ID 1為labId
        /// </summary>
        public static List<List<int>> IdList = new List<List<int>>()
        {
            new List<int>() { -1, -1},//INDEX 0:CarId  1: labId
            new List<int>() { -1, -1}

        };

       

        public static List<Dictionary<int, FlowType>> liuliangjiAndFlowtype = new List<Dictionary<int, FlowType>>
        {
            //0 低压流量计 1 高压流量计
            new Dictionary<int, FlowType>() {
            { 0, FlowType.None },
            { 1, FlowType.None }
            },
            new Dictionary<int, FlowType>() {
            { 0, FlowType.None },
            { 1, FlowType.None }
            }
        };
        //报表该打印的车辆ID 或 水泵Id
        public static List<KeyValuePair<Equipment, List<int>>> Print = new List<KeyValuePair<Equipment, List<int>>>() {
             new KeyValuePair<Equipment, List<int>>(Equipment.None, IdList[0]),
             new KeyValuePair<Equipment, List<int>>(Equipment.None,  IdList[1])

        };

        // 工况对应字符串
        public static List<string> gkString= new List<string>() { "低压工况", "1.3工况","超流量工况","半流量工况","高压工况","中压工况"};

    }
}
