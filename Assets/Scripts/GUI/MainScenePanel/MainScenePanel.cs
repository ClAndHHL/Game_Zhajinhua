using UnityEngine;
using System.Collections;
using BattleFramework.Data;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class MainScenePanel : IViewBase
{
    private List<CardItem> itemList = new List<CardItem>();
    private Transform CardCon = null;
    private uint ShowCardTimeId;
    private List<PlayerHead> headList = new List<PlayerHead>();
    private int playerNum = 5;
    private int index = 0;
    private int maxNum = 20;
    private List<CardItem> playerItemList = new List<CardItem>();//发到玩家手中的牌
    int gap = 8;

    private Button KanButton;
    private Button GenzhuButton;
    private Button JiazhuButton;
    private Button BiButton;
    private Button QiButton;
    private Button GendaodiFalseButton;
    private Button GendaodiTrueButton;
    private List<CardData> cardDataList = new List<CardData>();
    private Transform moneyObj;
    protected override void OnStart()
    {
        uiLayer = UIPanelLayers.NormalLayer;
        CardCon = Find<Transform>("CardCon");

        KanButton = Find<Button>("BottomButtons/KanButton");
        GenzhuButton = Find<Button>("BottomButtons/GenzhuButton");
        JiazhuButton = Find<Button>("BottomButtons/JiazhuButton");
        BiButton = Find<Button>("BottomButtons/BiButton");
        QiButton = Find<Button>("BottomButtons/QiButton");
        GendaodiFalseButton = Find<Button>("BottomButtons/GendaodiFalseButton");
        GendaodiTrueButton = Find<Button>("BottomButtons/GendaodiTrueButton");
        moneyObj = Find<Transform>("Money");
        moneyObj.gameObject.SetActive(false);
        for (int i = 0; i < CardData.dataList.Count; i++)
        {
            cardDataList.Add(CardData.dataList[i]);
        }
    }
    protected override void AddEventListener()
    {
        base.AddEventListener();
        KanButton.onClick.AddListener(KanButtonOnClick);
        GenzhuButton.onClick.AddListener(GenZhuButtonOnClick);
        JiazhuButton.onClick.AddListener(KanButtonOnClick);
        BiButton.onClick.AddListener(KanButtonOnClick);
        QiButton.onClick.AddListener(KanButtonOnClick);
        GendaodiFalseButton.onClick.AddListener(KanButtonOnClick);
        GendaodiTrueButton.onClick.AddListener(KanButtonOnClick);
    }
    private int genzhuIndex = 0;
    private void GenZhuButtonOnClick()
    {
        ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer(0, 500, GenZhuHandler);
        
        //tweener.OnComplete(delegate()
        //{
        //    FapaiUnitComplete(item, head);
        //});
    }

    private void GenZhuHandler()
    {
        GameObject obj = GameObject.Instantiate<GameObject>(moneyObj.gameObject);
        obj.SetActive(true);
        obj.transform.SetParent(panelObj.transform);
        obj.transform.Reset();
        obj.transform.position = headList[genzhuIndex].transform.position;
        Vector3 v = new Vector3(Random.Range(-100, 100), Random.Range(0, 120), 0);
        Tweener tweener = obj.transform.DOLocalMove(v, 0.2f);
        genzhuIndex++;
        if (genzhuIndex >= headList.Count)
        {
            Mogo.Util.TimerHeap.DelTimer(ShowCardTimeId);
            genzhuIndex = 0;
            return;
        }
    }
    
    protected override void RemoveEventListener()
    {
        base.RemoveEventListener();
        KanButton.onClick.RemoveListener(KanButtonOnClick);
        GenzhuButton.onClick.RemoveListener(GenZhuButtonOnClick);
        JiazhuButton.onClick.RemoveListener(KanButtonOnClick);
        BiButton.onClick.RemoveListener(KanButtonOnClick);
        QiButton.onClick.RemoveListener(KanButtonOnClick);
        GendaodiFalseButton.onClick.RemoveListener(KanButtonOnClick);
        GendaodiTrueButton.onClick.RemoveListener(KanButtonOnClick);
    }
    private void ResetData()
    {
        cardDataList.Clear();
        for (int i = 0; i < CardData.dataList.Count; i++)
        {
            cardDataList.Add(CardData.dataList[i]);
        }
    }
    private void KanButtonOnClick()
    {
        ResetData();
        for (int i = 0; i < headList.Count; i++)
        {
            List<CardData> dataList = new List<CardData>();
            for (int j = 0; j < 3; j++)
            {
                int dataIndex = Random.Range(0, cardDataList.Count);
                dataList.Add(cardDataList[dataIndex]);
                cardDataList.RemoveAt(dataIndex);
            }
            headList[i].ShowCard(dataList);
        }

        //for (int i = 0; i < headList.Count; i++)
        //{
        //    BiJiao(headList[0],headList[1])
        //}
       
    }
    protected override void OnShow(params object[] args)
    {
        InitPlayer();
        ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer(500, 50, ShowCardHandler);
    }
    /// <summary>
    /// 初始化玩家
    /// </summary>
    private void InitPlayer()
    {
        for (int i = 0; i < playerNum; i++)
        {
            GameObject obj = panelObj.transform.Find("PlayerHead0" + i).gameObject;
            PlayerHead head = obj.AddComponent<PlayerHead>();
            PlayerData data = new PlayerData();
            data.name = "我是" + i;
            data.glodNum = i * 10000;
            head.SetData(data);
            headList.Add(head);
        }
    }
    /// <summary>
    /// 整理牌
    /// </summary>
    private void ShowCardHandler()
    {
        GameObject card = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("GUI/CardPrefab"));
        card.transform.SetParent(CardCon);
        card.transform.localScale = Vector3.one;
        CardItem item = card.AddComponent<CardItem>();
        itemList.Add(item);
        //布局
        int x = index * gap - (maxNum / 2) * gap;
        card.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 130);
        if (index >= maxNum)
        {
            Mogo.Util.TimerHeap.DelTimer(ShowCardTimeId);
            index = 0;
            ShowCardTimeId =  Mogo.Util.TimerHeap.AddTimer(1000, 80, FapaiHandler);
            return;
        }
        index++;
    }
    /// <summary>
    /// 发牌
    /// </summary>
    private void FapaiHandler()
    {
        CardItem item = itemList[itemList.Count - index -1];
        playerItemList.Add(item);
        PlayerHead head = headList[index % 5];
        Tweener tweener = item.transform.DOMove(head.GetCardCon().position, 0.2f);
        tweener.OnComplete(delegate() {
            FapaiUnitComplete(item,head);
        });
        index++;
        if(index >= 15)
        {
            Mogo.Util.TimerHeap.DelTimer(ShowCardTimeId);
            
        }
    }
    /// <summary>
    /// 发牌到位
    /// </summary>
    /// <param name="item"></param>
    private void FapaiUnitComplete(CardItem item, PlayerHead head)
    {
        item.transform.SetAsLastSibling();
        head.AddCard(item);
        if (index >= playerNum*3)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (playerItemList.IndexOf(itemList[i]) < 0)
                {
                    itemList[i].gameObject.SetActive(false);
                }
            }
        }
    }
    /// <summary>
    /// 是否dataList1比dataList2大
    /// </summary>
    /// <param name="dataList1"></param>
    /// <param name="dataList2"></param>
    /// <returns></returns>
    private PlayerHead BiJiao(PlayerHead head1, PlayerHead head2)
    {
        List<CardData> dataList1 = head1.dataList;
        List<CardData> dataList2 = head2.dataList;

        int max_1;
        CardShapeType type1 = GetShapeType(dataList1,out max_1);

        int max_2;
        CardShapeType type2 = GetShapeType(dataList1, out max_2);
        if(type1>type2)
        {
            return head1;
        }
        if (type1 == type2)
        {
            if(max_1>=max_2)
            {
                return head1;
            }
            else
            {
                return head2;
            }
        }
        return head2;
        
    }
    private int GetMaxNum(int a,int b,int c)
    {
        int t = 0;
        if(a==b)
        {
            t = a;
            return t;
        }
        if (a == c)
        {
            t = a;
            return t;
        }
        if (b == c)
        {
            t = b;
            return t;
        }
        if(a>t)
        {
            t = a;
        }
        if (b > t)
        {
            t = b;
        }
        if (c > t)
        {
            t = c;
        }
        return t;
    }
    private CardShapeType GetShapeType(List<CardData> dataList,out int max)
    {
        CardShapeType type = CardShapeType.DAN_PAI;
        max = GetMaxNum(dataList[0].value, dataList[1].value, dataList[2].value);
        if (dataList[0].value == dataList[1].value && dataList[0].value == dataList[2].value)
        {
            type = CardShapeType.BAO_ZI;
            return type;
        }
        if (dataList[0].type == dataList[1].type && dataList[0].value == dataList[2].type)
        {
            type = CardShapeType.JIN_HUA;
            if (Mathf.Abs(dataList[0].value - dataList[1].value) <= 2
                && Mathf.Abs(dataList[0].value - dataList[2].value) <= 2
                && Mathf.Abs(dataList[1].value - dataList[0].value) <= 2
                && Mathf.Abs(dataList[1].value - dataList[2].value) <= 2)
            {
                type = CardShapeType.TONG_HUA_SHUN;
            }
            return type;
        }
        if (Mathf.Abs(dataList[0].value - dataList[1].value) <= 2
                && Mathf.Abs(dataList[0].value - dataList[2].value) <= 2
                && Mathf.Abs(dataList[1].value - dataList[0].value) <= 2
                && Mathf.Abs(dataList[1].value - dataList[2].value) <= 2)
        {
            type = CardShapeType.SHUN_ZI;
            return type;
        }
        if (dataList[0].value == dataList[1].value || dataList[0].value == dataList[2].value || dataList[1].value == dataList[2].value)
        {
            type = CardShapeType.DUI_ZI;
            return type;
        }
        return type;
    }
    private void Reset()
    {
        playerItemList.Clear();
    }
    protected override void OnHide()
    {

    }
    protected override void OnDestory()
    {

    }

    
}
