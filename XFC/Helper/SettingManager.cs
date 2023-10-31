using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using XFC;
using System.IO;

namespace XFC.Helper { 

    public  class SettingManager
    {
        public SettingConfiguration Data { get; }

        private static SettingManager Instance;
        static SettingManager()
        {
            Instance = new SettingManager();
        }

        private SettingManager()
        {
           
        }


        public void Save()
        {
            try
            {
                string savePath = ConstantValue.AppSetting;
                string json = JsonConvert.SerializeObject(Data);
                File.WriteAllText(savePath, json);
            }
            catch (Exception ex)
            {
               
            }
        }

        
    }


    public class SettingConfiguration
    {


    }

  
}


