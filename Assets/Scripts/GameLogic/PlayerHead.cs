using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using BattleFramework.Data;

public class PlayerHead : MonoBehaviour {
    private Text nameText;
    private Image headImage;
    private Text glodNumText;
    private Transform cardCon;
    private List<Transform> conList = new List<Transform>();
    private int index = 0;
    public void SetData(PlayerData data)
    {
        nameText = this.transform.Find("PlayerNameText").GetComponent<Text>();
        headImage = this.transform.Find("Head").GetComponent<Image>();
        glodNumText = this.transform.Find("GlodNumText").GetComponent<Text>();
        cardCon = this.transform.Find("CardCon");
        for (int i = 0; i < 3; i++)
        {
            conList.Add(cardCon.Find("CardCon" + i));
        }
        cardCon.gameObject.SetActive(false);
        nameText.text = data.name;
        glodNumText.text = data.glodNum.ToString();
    }
    public Transform GetCardCon()
    {
        int tempIndex = index;
        index++;
        return conList[tempIndex];
    }
    public void Reset()
    {
        index = 0;
    }
    public void ShowCard(List<CardData> dataList)
    {
        
        dataList.Sort(SortCardData);
        for (int i = 0; i < dataList.Count; i++)
        {
            itemList[i].SetData(dataList[i]);
        }
        this.dataList = dataList;
    }

    private int SortCardData(CardData x, CardData y)
    {
        return x.value - y.value;
    }
    private List<CardItem> itemList = new List<CardItem>();
    public List<CardData> dataList;
    internal void AddCard(CardItem item)
    {
        itemList.Add(item);
    }
}
