using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class JettonData {
        public static string csvFilePath = "Configs/JettonData";
        public static string[] columnNameArray = new string[3];
        public static List<JettonData> dataList;
        public static Dictionary<int, JettonData> dataMap;
        public static List<JettonData> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            dataList = new List<JettonData>();
            dataMap = new Dictionary<int, JettonData>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[3];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                JettonData data = new JettonData();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                int.TryParse(csvFile.mapData[i].data[1],out data.value);
                columnNameArray [1] = "value";
                data.resPath = csvFile.mapData[i].data[2];
                columnNameArray [2] = "resPath";
                dataList.Add(data);
                if (!dataMap.ContainsKey(data.id))
                    dataMap.Add(data.id,data);
            }
            return dataList;
        }
  
        public static JettonData GetByID (int id,List<JettonData> data)
        {
            foreach (JettonData item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
  
        public static JettonData GetByID (int id)
        {
            return GetByID(id,dataList);
        }
  
        public int id;//数据ID
        public int value;//数值
        public string resPath;//资源
    }
}
