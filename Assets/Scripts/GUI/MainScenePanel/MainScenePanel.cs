using UnityEngine;
using System.Collections;
using BattleFramework.Data;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class MainScenePanel : IViewBase
{
    private List<CardItem> readlyCardItemList = new List<CardItem>();//准备好要发的牌准备发牌
    private List<CardItem> playerCardItemList = new List<CardItem>();//发到玩家手中的牌
    private int readlyMaxNum = 20;//要准备牌的张数
    private Transform CardItemParent = null;//牌的父容器
    private uint ShowCardTimeId;//动画ID
    private List<PlayerHead> headList = new List<PlayerHead>();//玩家
    private int playerNum = 5;//玩家个数
    private int tempCardIndex = 0;//牌到第几张了
    private int gap = 8;//牌和牌之间的缝隙距离
    private Button KanButton;
    private Button GenzhuButton;
    private Button JiazhuButton;
    private Button BiButton;
    private Button QiButton;
    private Button GendaodiFalseButton;
    private Button GendaodiTrueButton;
    private Button AAAButton;
    private Button ReadlyButton;
    private List<CardData> cardDataList = new List<CardData>();
    private Transform moneyObj;
    private Transform moneyParentCon;
    private int genzhuTotalNum = 0;//跟住次数
    private Transform buttonParentCon;
    private JiazhuPanel jiazhupanel;
    private BiPaiPanel bipaipanel;

    private JettonData minJettionData;
    protected override void OnStart()
    {
        uiLayer = UIPanelLayers.NormalLayer;
        CardItemParent = Find<Transform>("CardCon");

        buttonParentCon = Find<Transform>("BottomButtons");
        KanButton = Find<Button>("BottomButtons/KanButton");
        GenzhuButton = Find<Button>("BottomButtons/GenzhuButton");
        JiazhuButton = Find<Button>("BottomButtons/JiazhuButton");
        BiButton = Find<Button>("BottomButtons/BiButton");
        QiButton = Find<Button>("BottomButtons/QiButton");
        GendaodiFalseButton = Find<Button>("BottomButtons/GendaodiFalseButton");
        GendaodiTrueButton = Find<Button>("BottomButtons/GendaodiTrueButton");
        AAAButton = Find<Button>("BottomButtons/Button");
        ReadlyButton = Find<Button>("ReadlyButton");
        moneyObj = Find<Transform>("Money");
        moneyParentCon = Find<Transform>("MoneyCon");
        moneyObj.gameObject.SetActive(false);
        for (int i = 0; i < CardData.dataList.Count; i++)
        {
            cardDataList.Add(CardData.dataList[i]);
        }
        EnabledAllButton(false);

        jiazhupanel = new JiazhuPanel();
        jiazhupanel.panelObj = Find<Transform>("JiazhuPanel").gameObject;
        jiazhupanel.Start(false);
        jiazhupanel.Hide();

        bipaipanel = new BiPaiPanel();
        bipaipanel.panelObj = Find<Transform>("BiPaiPanel").gameObject;
        bipaipanel.Start(false);
        bipaipanel.Hide();

        minJettionData = JettonData.GetByID(1);
    }
    private void EnabledAllButton(bool flag)
    {
        KanButton.interactable = flag;
        GenzhuButton.interactable = flag;
        JiazhuButton.interactable = flag;
        BiButton.interactable = flag;
        QiButton.interactable = flag;
        GendaodiFalseButton.interactable = flag;
        GendaodiTrueButton.interactable = flag;
        AAAButton.interactable = flag;
    }
    protected override void AddEventListener()
    {
        base.AddEventListener();
        KanButton.onClick.AddListener(KanButtonOnClick);
        GenzhuButton.onClick.AddListener(GenZhuButtonOnClick);
        JiazhuButton.onClick.AddListener(JiazhuButtonOnClick);
        BiButton.onClick.AddListener(BiButtonOnClick);
        QiButton.onClick.AddListener(QiButtonOnClick);
        GendaodiFalseButton.onClick.AddListener(KanButtonOnClick);
        GendaodiTrueButton.onClick.AddListener(KanButtonOnClick);
        AAAButton.onClick.AddListener(KanButtonOnClick);
        ReadlyButton.onClick.AddListener(ReadlyButtonOnClick);
        Mogo.Util.EventDispatcher.AddEventListener<JettonData>(GUIEvent.JIA_ZHU, RealyJiaZhuHandler);
        Mogo.Util.EventDispatcher.AddEventListener<PlayerHead>(GUIEvent.BI_PAI_BUTTON_CLICK,BiPaiHandler);
    }

    private void BiPaiHandler(PlayerHead obj)
    {
        int playerIndex = headList.IndexOf(obj);
        CreatePlayerCardData(0);
        CreatePlayerCardData(playerIndex);

        
        bipaipanel.Show();
        bipaipanel.SetData(obj, headList[0]);
        HideAllBiPaiButton();
        Mogo.Util.TimerHeap.AddTimer<PlayerHead, PlayerHead>(2000, 0, BiPaiResult, obj, headList[0]);
    }

    private void BiPaiResult(PlayerHead head1, PlayerHead head2)
    {
        bipaipanel.Hide();
        PlayerHead head = BiJiao(head1, head2);
        if(head == head1)
        {
            head2.IsOver = true;
        }
        else
        {
            head1.IsOver = true;
        }
        if (head is MySelfHead == false)
        {
            EnabledAllButton(false);
            ReadlyButton.gameObject.SetActive(true);
            ReadlyButton.interactable = false;
            buttonParentCon.gameObject.SetActive(false);
        }
        Debug.LogError("最大的牌：" + head.data.name);
    }
    private void HideAllBiPaiButton()
    {
        for (int i = 0; i < headList.Count; i++)
        {
            headList[i].HideBiPaiButton();
        }
    }

    private void RealyJiaZhuHandler(JettonData obj)
    {
        minJettionData = obj;
        jiazhupanel.Hide();
        GenZhuButtonOnClick();
    }

    private void JiazhuButtonOnClick()
    {
        jiazhupanel.Show(1,2,3,4);
        jiazhupanel.SetMinJetton(minJettionData);
    }
    protected override void RemoveEventListener()
    {
        base.RemoveEventListener();
        KanButton.onClick.RemoveListener(KanButtonOnClick);
        GenzhuButton.onClick.RemoveListener(GenZhuButtonOnClick);
        JiazhuButton.onClick.RemoveListener(JiazhuButtonOnClick);
        BiButton.onClick.RemoveListener(BiButtonOnClick);
        QiButton.onClick.RemoveListener(KanButtonOnClick);
        GendaodiFalseButton.onClick.RemoveListener(KanButtonOnClick);
        GendaodiTrueButton.onClick.RemoveListener(KanButtonOnClick);
        AAAButton.onClick.RemoveListener(KanButtonOnClick);
        ReadlyButton.onClick.RemoveListener(ReadlyButtonOnClick);
        Mogo.Util.EventDispatcher.RemoveEventListener<JettonData>(GUIEvent.JIA_ZHU, RealyJiaZhuHandler);
        Mogo.Util.EventDispatcher.RemoveEventListener<PlayerHead>(GUIEvent.BI_PAI_BUTTON_CLICK, BiPaiHandler);
    }
    /// <summary>
    /// 准备按钮事件
    /// </summary>
    private void ReadlyButtonOnClick()
    {
        ReadlyButton.gameObject.SetActive(false);
        buttonParentCon.gameObject.SetActive(true);
        ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer(500, 50, ReadlyCard);
    }
    /// <summary>
    /// 比较大小
    /// </summary>
    private void BiButtonOnClick()
    {
        for (int i = 0; i < headList.Count; i++)
        {
            headList[i].ShowBiPaiButton();
        }
        
    }
    private int genzhuIndex = 0;
    private void QiButtonOnClick()
    {
        EnabledAllButton(false);
        GiveUp(0);
    }
    private void GiveUp(int playerIndex)
    {
        headList[playerIndex].GiveUp();
    }
    private void GenZhuButtonOnClick()
    {
        if (genzhuIndex>0)
        {
            return;
        }
        if(genzhuTotalNum == 0)
        {
            KanButton.interactable = true;
            QiButton.interactable = true;
        }
        if (genzhuTotalNum>=3)
        {
            BiButton.interactable = true;
        }
        genzhuTotalNum++;
        GenzhuButton.interactable = false;
        JiazhuButton.interactable = false;
        //ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer(0, 500, GenZhuHandler);
        ChuJetton(headList[0]);
        ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer<PlayerHead>(500, 0, NextPlayerOpaHandler, headList[1]);
    }
    private List<GameObject> jettonObjList = new List<GameObject>();//
    private void ChuJetton(PlayerHead head)
    {
        GameObject obj = GameObject.Instantiate<GameObject>(moneyObj.gameObject);
        obj.SetActive(true);
        obj.transform.SetParent(moneyParentCon);
        obj.transform.Reset();
        obj.transform.position = head.transform.position;
        obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/room/jetton/" + minJettionData.resPath);
        jettonObjList.Add(obj);
        Vector3 v = new Vector3(Random.Range(-100, 100), Random.Range(20, 120), 0);
        Tweener tweener = obj.transform.DOLocalMove(v, 0.2f);
    }
    private void NextPlayerOpaHandler(PlayerHead head)
    {
        int random = Random.Range(0, 10000);

        ChuJetton(head);
        if (head.playerIndex + 1 >= playerNum)
        {
            //一轮结束
            GenzhuButton.interactable = true;
            JiazhuButton.interactable = true;
        }
        else
        {
            ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer<PlayerHead>(500, 0, NextPlayerOpaHandler, headList[head.playerIndex+1]);
        }
    }
    
    protected override void OnShow(params object[] args)
    {
        InitPlayer();
        ResetData();
        buttonParentCon.gameObject.SetActive(false);
        ReadlyButton.gameObject.SetActive(true);
        
    }
    /// <summary>
    /// 初始化玩家
    /// </summary>
    private void InitPlayer()
    {
        for (int i = 0; i < playerNum; i++)
        {
            GameObject obj = panelObj.transform.Find("PlayerHead0" + i).gameObject;
            PlayerHead head = null;
            if(i == 0)
            {
                head = obj.AddComponent<MySelfHead>();
            }
            else
            {
                head = obj.AddComponent<PlayerHead>();
            }
            
            PlayerData data = new PlayerData();
            data.name = "我是" + i;
            data.glodNum = i * 10000;
            head.SetData(data);
            headList.Add(head);
            head.playerIndex = i;
        }
    }
    /// <summary>
    /// 整理牌
    /// </summary>
    private void ReadlyCard()
    {
        GameObject card = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("GUI/CardPrefab"));
        card.transform.SetParent(CardItemParent);
        card.transform.localScale = Vector3.one;
        CardItem item = card.AddComponent<CardItem>();
        readlyCardItemList.Add(item);
        //布局
        int x = tempCardIndex * gap - (readlyMaxNum / 2) * gap;
        card.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 130);
        if (tempCardIndex >= readlyMaxNum)
        {
            Mogo.Util.TimerHeap.DelTimer(ShowCardTimeId);
            tempCardIndex = 0;
            ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer(1000, 80, FaPaiHandler);
            return;
        }
        tempCardIndex++;
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
        KanButton.interactable = false;
        ShowPlayerCard(0);
        
    }
    /// <summary>
    /// 生成数据
    /// </summary>
    /// <param name="index"></param>
    private void CreatePlayerCardData(int index)
    {
        if(headList[index].IsHaveData() == false)
        {
            List<CardData> dataList = new List<CardData>();
            for (int j = 0; j < 3; j++)
            {
                int dataIndex = Random.Range(0, cardDataList.Count);
                dataList.Add(cardDataList[dataIndex]);
                cardDataList.RemoveAt(dataIndex);
            }
            headList[index].SetCardData(dataList);
        }
    }
    /// <summary>
    /// 显示卡牌
    /// </summary>
    /// <param name="index"></param>
    private void ShowPlayerCard(int index)
    {
        if(headList[index].IsHaveData() == false)
        {
            CreatePlayerCardData(index);
        }
        headList[index].ShowCard();
        
    }
    private void test()
    {
        List<CardData> list1 = new List<CardData>();
        list1.Add(CardData.GetByID(47));
        list1.Add(CardData.GetByID(37));
        list1.Add(CardData.GetByID(24));
        List<CardData> list2 = new List<CardData>();
        list2.Add(CardData.GetByID(15));
        list2.Add(CardData.GetByID(7));
        list2.Add(CardData.GetByID(33));
        Debug.LogError(BiJiao(list2,list1));
    }
    
    /// <summary>
    /// 发牌
    /// </summary>
    private void FaPaiHandler()
    {
        CardItem item = readlyCardItemList[readlyCardItemList.Count - tempCardIndex -1];
        item.transform.SetAsLastSibling();
        playerCardItemList.Add(item);
        PlayerHead head = headList[tempCardIndex % 5];
        Tweener tweener = item.transform.DOMove(head.GetCardCon().position, 0.2f);
        tweener.OnComplete(delegate() {
            FapaiUnitComplete(item,head);
        });
        tempCardIndex++;
        if(tempCardIndex >= 15)
        {
            Mogo.Util.TimerHeap.DelTimer(ShowCardTimeId);
            GenzhuButton.interactable = true;
            JiazhuButton.interactable = true;
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
        if (tempCardIndex >= playerNum*3)
        {
            for (int i = 0; i < readlyCardItemList.Count; i++)
            {
                if (playerCardItemList.IndexOf(readlyCardItemList[i]) < 0)
                {
                    readlyCardItemList[i].gameObject.SetActive(false);
                }
            }
        }
    }
    private PlayerHead BiJiao(PlayerHead head1, PlayerHead head2)
    {
        if (BiJiao(head1.dataList, head2.dataList))
        {
            return head1;
        }
        else
        {
            return head2;
        }
    }
    /// <summary>
    /// 是否dataList1比dataList2大
    /// </summary>
    /// <param name="dataList1"></param>
    /// <param name="dataList2"></param>
    /// <returns></returns>
    
    private bool BiJiao(List<CardData> dataList1, List<CardData> dataList2)
    {
        dataList1.Sort(SortCardData);
        dataList2.Sort(SortCardData);

        CardShapeType type1 = GetShapeType(dataList1);
        CardShapeType type2 = GetShapeType(dataList2);
        if(type1>type2)
        {
            return true;
        }
        if (type1 == type2)
        {

            if(type1 == CardShapeType.DUI_ZI)//牌型为对子时
            {
                int duizinum_1;
                int sanpai_1;
                int duizinum_2;
                int sanpai_2;
                GetDuiZi(dataList1,out duizinum_1,out sanpai_1);
                GetDuiZi(dataList2,out duizinum_2,out sanpai_2);
                if(duizinum_1>duizinum_2)
                {
                    return true;
                }else if(duizinum_1 == duizinum_2)
                {
                    if(sanpai_1>=sanpai_2)
                    {
                        return true;
                    }else{
                        return false;
                    }
                }else{
                    return false;
                }
            }else{//牌型为顺子或金花或是散牌是
                if(dataList1[2].value!=dataList2[2].value)
                {
                    if(dataList1[2].value>dataList2[2].value)
                    {
                        return true;
                    }else{
                        return false;
                    }
                }else if(dataList1[1].value!=dataList2[1].value){
                    if(dataList1[1].value>dataList2[1].value)
                    {
                        return true;
                    }else{
                        return false;
                    }
                }else{
                    if(dataList1[0].value>=dataList2[0].value)
                    {
                        return true;
                    }else{
                        return false;
                    }
                }
            }
            
        }
        return false;
    }
    /// <summary>
    /// 获取对子的牌值
    /// </summary>
    /// <param name="dataList"></param>
    /// <param name="duizinum"></param>
    /// <param name="sanpai"></param>
    private void GetDuiZi(List<CardData> dataList,out int duizinum,out int sanpai)
    {
        if(dataList[0].value == dataList[1].value)
        {
            duizinum = dataList[0].value;
            sanpai = dataList[2].value;
        }else if (dataList[0].value == dataList[2].value)
        {
            duizinum = dataList[0].value;
            sanpai = dataList[1].value;
        }else if (dataList[1].value == dataList[2].value)
        {
            duizinum = dataList[1].value;
            sanpai = dataList[0].value;
        }
        else
        {
            duizinum = 0;
            sanpai = 0;
            Debuger.LogError("不是对子：");
        }
        
    }
    private int SortCardData(CardData x, CardData y)
    {
        return x.value - y.value;
    }
    private CardShapeType GetShapeType(List<CardData> dataList)
    {
        //dataList.Sort(SortCardData);

        CardShapeType type = CardShapeType.DAN_PAI;
        if (dataList[0].value == dataList[1].value && dataList[0].value == dataList[2].value)
        {
            type = CardShapeType.BAO_ZI;
            return type;
        }
        if (dataList[0].type == dataList[1].type && dataList[0].type == dataList[2].type)
        {
            type = CardShapeType.JIN_HUA;
            if (dataList[0].value + 1 == dataList[1].value && dataList[0].value + 2 == dataList[2].value)
            {
                type = CardShapeType.TONG_HUA_SHUN;
            }
            return type;
        }
        if (dataList[0].value + 1 == dataList[1].value && dataList[0].value + 2 == dataList[2].value)
        {
            type = CardShapeType.SHUN_ZI;
            return type;
        }
        if (dataList[0].value == dataList[1].value || dataList[1].value == dataList[2].value)
        {
            type = CardShapeType.DUI_ZI;
            return type;
        }
        return type;
    }
    private void Reset()
    {
        playerCardItemList.Clear();
    }
    private void ResetGame()
    {

    }
    protected override void OnHide()
    {

    }
    protected override void OnDestory()
    {

    }

    
}
