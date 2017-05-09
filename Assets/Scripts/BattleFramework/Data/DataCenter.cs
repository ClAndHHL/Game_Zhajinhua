using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
//if you want to update this,please go to CreateClassFromCSV.cs
namespace BattleFramework.Data
{
    public class DataCenter : MonoBehaviour
    {
        static DataCenter instance;
  
        public List<CardData> list_CardData;
        public List<GameBaseData> list_GameBaseData;
        public List<GameSceneData> list_GameSceneData;
        public List<GoodsData> list_GoodsData;
        public List<ResourceData> list_ResourceData;
  
        public static DataCenter Instance ()
        {
            if (instance == null) {
                Debug.Log ("new _DataCenter");
                GameObject go = new GameObject ("_DataCenter");
                DataCenter dataCenter = go.AddComponent<DataCenter> ();
                dataCenter.LoadCSV ();
                DontDestroyOnLoad (go);
                instance = dataCenter;
            }
            return instance;
        }
   
   
        public void LoadCSV ()
        {
            list_CardData = CardData.LoadDatas ();
            list_GameBaseData = GameBaseData.LoadDatas ();
            list_GameSceneData = GameSceneData.LoadDatas ();
            list_GoodsData = GoodsData.LoadDatas ();
            list_ResourceData = ResourceData.LoadDatas ();
        }
 
 
    }
}
