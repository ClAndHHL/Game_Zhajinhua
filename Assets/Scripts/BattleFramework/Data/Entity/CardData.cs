using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace BattleFramework.Data{
    [System.Serializable]
    public class CardData {
        public static string csvFilePath = "Configs/CardData";
        public static string[] columnNameArray = new string[4];
        public static List<CardData> dataList;
        public static Dictionary<int, CardData> dataMap;
        public static List<CardData> LoadDatas(){
            CSVFile csvFile = new CSVFile();
            csvFile.Open (csvFilePath);
            dataList = new List<CardData>();
            dataMap = new Dictionary<int, CardData>();
            string[] strs;
            string[] strsTwo;
            List<int> listChild;
            columnNameArray = new string[4];
            for(int i = 0;i < csvFile.mapData.Count;i ++){
                CardData data = new CardData();
                int.TryParse(csvFile.mapData[i].data[0],out data.id);
                columnNameArray [0] = "id";
                int.TryParse(csvFile.mapData[i].data[1],out data.type);
                columnNameArray [1] = "type";
                int.TryParse(csvFile.mapData[i].data[2],out data.value);
                columnNameArray [2] = "value";
                data.resPath = csvFile.mapData[i].data[3];
                columnNameArray [3] = "resPath";
                dataList.Add(data);
                if (!dataMap.ContainsKey(data.id))
                    dataMap.Add(data.id,data);
            }
            return dataList;
        }
  
        public static CardData GetByID (int id,List<CardData> data)
        {
            foreach (CardData item in data) {
                if (id == item.id) {
                     return item;
                }
            }
            return null;
        }
  
  
        public static CardData GetByID (int id)
        {
            return GetByID(id,dataList);
        }
  
        public int id;//数据ID
        public int type;//名字
        public int value;//数值
        public string resPath;//资源
    }
}
