﻿using System;
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

        public List<int> rumtimeList = new List<int>() { 0};
        public List<bool> IsGkCompleted = new List<bool>() { false };
        public bool IsChecked = false;
        public int runtime = -1;
        public Enum currentGk ;
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