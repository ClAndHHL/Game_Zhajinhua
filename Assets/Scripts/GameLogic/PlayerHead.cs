using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using BattleFramework.Data;
using DG.Tweening;

public class PlayerHead : MonoBehaviour {
    private Text nameText;
    private Image headImage;
    private Text glodNumText;
    private Transform cardCon;
    private List<Transform> conList = new List<Transform>();
    private int index = 0;
    public PlayerData data;
    public List<CardItem> cardItemList = new List<CardItem>();
    public List<CardData> dataList;
    private Button bipaiButton;
    private bool isOver;
    public int playerIndex;
    public void SetData(PlayerData data)
    {
        this.data = data;
        nameText = this.transform.Find("PlayerNameText").GetComponent<Text>();
        headImage = this.transform.Find("Head").GetComponent<Image>();
        glodNumText = this.transform.Find("GlodNumText").GetComponent<Text>();
        cardCon = this.transform.Find("CardCon");
        if(this is MySelfHead == false)
        {
            bipaiButton = this.transform.Find("BiPaiButton").GetComponent<Button>();
            bipaiButton.onClick.AddListener(BiPaiButtonOnClick);
            bipaiButton.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < 3; i++)
        {
            conList.Add(cardCon.Find("CardCon" + i));
        }
        cardCon.gameObject.SetActive(false);
        nameText.text = data.name;
        glodNumText.text = data.glodNum.ToString();
    }

    private void BiPaiButtonOnClick()
    {
        Mogo.Util.EventDispatcher.TriggerEvent<PlayerHead>(GUIEvent.BI_PAI_BUTTON_CLICK, this);
    }
    public void ShowBiPaiButton()
    {
        if(bipaiButton != null && IsOver == false)
            bipaiButton.gameObject.SetActive(true);
    }
    public void HideBiPaiButton()
    {
        if (bipaiButton != null)
            bipaiButton.gameObject.SetActive(false);
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
    /// <summary>
    /// 设置牌数据
    /// </summary>
    /// <param name="dataList"></param>
    public void SetCardData(List<CardData> dataList)
    {
        dataList.Sort(SortCardData);
        this.dataList = dataList;
    }
    /// <summary>
    /// 显示牌
    /// </summary>
    public void ShowCard()
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            cardItemList[i].SetData(dataList[i]);
        }
    }

    private int SortCardData(CardData x, CardData y)
    {
        return x.value - y.value;
    }
    public void AddCard(CardItem item)
    {
        cardItemList.Add(item);
    }
    public void GiveUp()
    {
        for (int i = 0; i < cardItemList.Count; i++)
        {
            cardItemList[i].GiveUp();
        }
    }
    /// <summary>
    /// 是否有数据
    /// </summary>
    /// <returns></returns>
    public bool IsHaveData()
    {
        return dataList != null && dataList.Count == 3;
    }
    public bool IsOver
    {
        get
        {
            return isOver;
        }
        set
        {
            isOver = value;
            if (isOver)
            {
                GiveUp();
            }
        }
    }

    internal void GoBack()
    {
        for (int i = 0; i < cardItemList.Count; i++)
        {
            cardItemList[i].transform.DOMove(conList[i].position, 0.2f);
        }
    }
}
