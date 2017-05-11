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
    private int minBipaiGenzhuNum = 3;//比较牌的最小轮数
    private int maxZuidalun = 30;
    private Transform buttonParentCon;
    private JiazhuPanel jiazhupanel;
    private BiPaiPanel bipaipanel;
    private ResultChildPanel resultPanel;
    private GuizeChildPanel guizePanel;
    private PlayerHead winnerPlayer;

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

        resultPanel = new ResultChildPanel();
        resultPanel.panelObj = Find<Transform>("ResultPanel").gameObject;
        resultPanel.Start(false);
        resultPanel.Hide();

        minJettionData = JettonData.GetByID(1);

        guizePanel= new GuizeChildPanel();
        guizePanel.panelObj = Find<Transform>("GuizePanel").gameObject;
        guizePanel.Start(false);
        ResetGuize();
    }
    private void ResetGuize()
    {
        guizePanel.Reset();
        guizePanel.SetGuoDi(100);
        guizePanel.SetDingZhu(1000);
        guizePanel.SetZongZhuAdd(0);
        guizePanel.SetZuidalun(0, maxZuidalun);
        guizePanel.SetKebilun(0, minBipaiGenzhuNum);
    }
    private int GenzhuTotalNum
    {
        get
        {
            return genzhuTotalNum;
        }
        set
        {
            genzhuTotalNum = value;
            guizePanel.SetZuidalun(genzhuTotalNum, maxZuidalun);
            guizePanel.SetKebilun(genzhuTotalNum, minBipaiGenzhuNum);
        }
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


        bipaipanel.Show(obj, headList[0]);
        HideAllBiPaiButton();
        Mogo.Util.TimerHeap.AddTimer<PlayerHead, PlayerHead>(2000, 0, BiPaiResult, obj, headList[0]);
    }
    private void SetWinner(PlayerHead head)
    {
        winnerPlayer = head;
        if (bipaipanel.IsHide() == false)
        {
            bipaipanel.Hide();
        }
        if (winnerPlayer != null)
        {
            Debug.LogError("本轮结束，胜出者：" + head.data.name);
        }
        EnabledAllButton(false);
        ReadlyButton.gameObject.SetActive(true);
        ReadlyButton.interactable = true;
        buttonParentCon.gameObject.SetActive(false);

        for (int i = 0; i < jettonObjList.Count; i++)
        {
            jettonObjList[i].transform.DOMove(head.transform.position, 1.5f);
        }
        int jettonTotal = 0;
        for (int i = 0; i < headList.Count; i++)
        {
            jettonTotal += headList[i].GetChangeJetton();
        }
        head.ChangeJetton(Mathf.Abs(jettonTotal));
        Mogo.Util.TimerHeap.AddTimer(1500, 0, AddJetton);
    }
    /// <summary>
    /// 收获筹码
    /// </summary>
    private void AddJetton()
    {
        for (int i = 0; i < jettonObjList.Count; i++)
        {
            jettonObjList[i].gameObject.SetActive(false);
        }
        resultPanel.Show(headList);
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

        PlayerHead nextPlayer = GetCanBipaiPlayer(head);
        if(nextPlayer == null)
        {
            SetWinner(head);
            return;
        }
        //Debug.LogError("最大的牌：" + head.data.name);
        NextPlayerOpaHandler(head2);
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
        ResetGuize();
        ReadlyButton.gameObject.SetActive(false);
        buttonParentCon.gameObject.SetActive(true);
        //数据清理还原
        tempCardIndex = 0;
        if (readlyCardItemList.Count>0)//已经准备好了
        {
            for (int i = 0; i < readlyCardItemList.Count; i++)
            {
                readlyCardItemList[i].Reset();
            }
        }
        for (int i = 0; i < headList.Count; i++)
        {
            headList[i].Reset();
        }
        winnerPlayer = null;
        minJettionData = JettonData.GetByID(1);
        ResetData();
        ReadlyCard();
        resultPanel.Hide();
        //下锅底
        for (int i = 0; i < headList.Count; i++)
        {
            BetJetton(headList[i]);
        }
        GenzhuTotalNum = 1;
    }
    /// <summary>
    /// 整理牌
    /// </summary>
    private void ReadlyCard()
    {
        if (readlyCardItemList.Count > 0)
        {
            
        }
        else
        {
            for (int i = 0; i < readlyMaxNum; i++)
            {
                GameObject card = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("GUI/CardPrefab"));
                card.transform.SetParent(CardItemParent);
                card.transform.localScale = Vector3.one;
                CardItem item = card.AddComponent<CardItem>();
                readlyCardItemList.Add(item);
            }
        }
        for (int i = 0; i < readlyCardItemList.Count; i++)
        {
            readlyCardItemList[i].transform.Reset();
            readlyCardItemList[i].gameObject.SetActive(false);
        }
        
        ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer(500, 50, ReadlyCardAnimation);
    }
    private void ReadlyCardAnimation()
    {
        if (tempCardIndex >= readlyMaxNum)
        {
            Mogo.Util.TimerHeap.DelTimer(ShowCardTimeId);
            tempCardIndex = 0;
            ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer(1000, 80, FaPaiHandler);
            return;
        }
        GameObject card = readlyCardItemList[tempCardIndex].gameObject;
        card.SetActive(true);
        card.transform.SetAsLastSibling();
        //布局
        int x = tempCardIndex * gap - (readlyMaxNum / 2) * gap;
        card.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 130);
        tempCardIndex++;
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
    private void QiButtonOnClick()
    {
        EnabledAllButton(false);
        GiveUp(0);
        NextPlayerOpaHandler(headList[0]);
    }
    private void GiveUp(int playerIndex)
    {
        headList[playerIndex].GiveUp();
    }
    private int dijilun_num = 0;
    private void GenZhuButtonOnClick()
    {
        if(GenzhuTotalNum >1)
        {
            KanButton.interactable = true;
            QiButton.interactable = true;
        }
        GenzhuTotalNum++;

        GenzhuButton.interactable = false;
        JiazhuButton.interactable = false;
        BiButton.interactable = false;
        QiButton.interactable = false;
        //ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer(0, 500, GenZhuHandler);
        BetJetton(headList[0]);
        ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer<PlayerHead>(500, 0, NextPlayerOpaHandler, headList[0]);
    }
    private List<GameObject> jettonObjList = new List<GameObject>();//
    
    /// <summary>
    /// 下注
    /// </summary>
    /// <param name="head"></param>
    private void BetJetton(PlayerHead head)
    {
        GameObject obj = GetJettonObj();
        obj.transform.position = head.transform.position;
        jettonObjList.Add(obj);
        obj.gameObject.SetActive(true);
        obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/room/jetton/" + minJettionData.resPath);
        Vector3 v = new Vector3(Random.Range(-100, 100), Random.Range(20, 120), 0);
        Tweener tweener = obj.transform.DOLocalMove(v, 0.2f);
        head.ChangeJetton(-minJettionData.value);

        guizePanel.SetZongZhuAdd(minJettionData.value);
    }
    /// <summary>
    /// 获得筹码对象
    /// </summary>
    /// <returns></returns>
    private GameObject GetJettonObj()
    {
        GameObject obj;
        if(jettonObjList.Count<60)
        {
            obj = GameObject.Instantiate<GameObject>(moneyObj.gameObject);
            obj.SetActive(true);
            obj.transform.SetParent(moneyParentCon);
            obj.transform.Reset();
        }
        else
        {
            obj = jettonObjList[0];
            jettonObjList.RemoveAt(0);
        }
        
        return obj;
    }
    /// <summary>
    /// 返回剩的人数
    /// </summary>
    /// <returns></returns>
    private int GetSurplusPlayerNum()
    {
        int n = 0;
        for (int i = 0; i < headList.Count; i++)
        {
            if(headList[i].IsOver == false)
            {
                n++;
            }
        }
        return n;
    }
    /// <summary>
    /// 返回看牌的人数
    /// </summary>
    /// <returns></returns>
    private int GetKanPlayerNum()
    {
        int n = 0;
        for (int i = 0; i < headList.Count; i++)
        {
            if (headList[i].IsOver == false && headList[i].dataList != null && headList[i].dataList.Count == 3)
            {
                n++;
            }
        }
        return n;
    }
    private PlayerHead GetCanBipaiPlayer(PlayerHead head)
    {

        List<PlayerHead> list = new List<PlayerHead>();
        for (int i = 0; i < headList.Count; i++)
        {
            if(headList[i] != head && headList[i].IsOver == false)
            {
                list.Add(headList[i]);
            }
        }
        if(list.Count == 0)
        {
            return null;
        }
        return list[Random.Range(0, list.Count)];

    }
    private bool IsGenZhu(PlayerHead head,out bool isBipai)
    {
        isBipai = false;
        //
        int randomMax = 10000;
        int surplusPlayerNum = GetSurplusPlayerNum();
        int kanPlayerNum = GetKanPlayerNum();
        bool isChujetton = true;
        CardShapeType type = GetShapeType(head.dataList);//获取牌的类型
        bool isCanBipai = GenzhuTotalNum >= minBipaiGenzhuNum;//是否可以比牌
        if(GenzhuTotalNum>20)
        {
            isBipai = true;
        }
        if (type >= CardShapeType.TONG_HUA_SHUN)
        {

        }else if (type == CardShapeType.JIN_HUA)
        {

        }
        else if (type == CardShapeType.SHUN_ZI)
        {

        }
        else if (type == CardShapeType.DUI_ZI)
        {
            if (isCanBipai == false && kanPlayerNum > 3 && surplusPlayerNum > 3)
            {
                isChujetton = false;
            }
            if (isChujetton)
            {
                if (isCanBipai)
                {
                    isBipai = true;
                }
            }
        }
        else//单牌
        {
            if(kanPlayerNum>2 && surplusPlayerNum>2)
            {
                isChujetton = false;
            }
            if(isChujetton)
            {
                if(isCanBipai)
                {
                    isBipai = true;
                }
            }
        }

        return isChujetton;
    }
    /// <summary>
    /// 获取胜利者
    /// </summary>
    /// <returns></returns>
    private List<PlayerHead> GetNotOverPlayer()
    {
        List<PlayerHead> list = new List<PlayerHead>();
        for (int i = 0; i < headList.Count; i++)
        {
            if(headList[i].IsOver == false)
            {
                list.Add(headList[i]);
            }
        }
        return list;
    }
    private void NextPlayerOpaHandler(PlayerHead head)
    {
        List<PlayerHead> notOverPlayerList = GetNotOverPlayer();
        if(notOverPlayerList.Count == 1)
        {
            SetWinner(notOverPlayerList[0]);
            return;
        }
        PlayerHead next = null;
        if(head.playerIndex + 1 == headList.Count)
        {
            //到了玩家自己
            GenzhuTotalNum++;
            next = headList[0];
            if (next.IsOver == false)
            {
                GenzhuButton.interactable = true;
                JiazhuButton.interactable = true;
                QiButton.interactable = true;
                if (GenzhuTotalNum >= minBipaiGenzhuNum)
                {
                    BiButton.interactable = true;
                }
                return;//如果自己还没输则返回等待自己手动操作
            }
            else
            {
                EnabledAllButton(false);
            }
        }
        else
        {
            next = headList[head.playerIndex + 1];
            GenzhuButton.interactable = false;
            JiazhuButton.interactable = false;
            QiButton.interactable = false;
            if (GenzhuTotalNum >= minBipaiGenzhuNum)
            {
                BiButton.interactable = false;
            }
        }
        if(next.IsOver)
        {
            NextPlayerOpaHandler(next);
            return;
        }
        ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer<PlayerHead>(500, 0, NextPlayerOpaHandler, next);
        bool isBipai = false;
        int randomMax = 10000;
        int random = Random.Range(0, randomMax);
        bool isChujetton = false;
        int surplusPlayerNum = GetSurplusPlayerNum();
        int kanPlayerNum = GetKanPlayerNum();
        bool isKan = false;
        if (next.dataList != null && next.dataList.Count == 3)
        {
            isKan = true;
        }
        if (isKan == false)
        {
            //看牌的几率30%
            if (random < randomMax * 0.5f)
            {
                CreatePlayerCardData(next.playerIndex);//看牌
                next.SetResultText("看牌");
                isChujetton = IsGenZhu(next, out isBipai);
            }
            else
            {
                isChujetton = true;
            }
        }
        else
        {
            isChujetton = IsGenZhu(next, out isBipai);
        }
        if (isChujetton == true)
        {
            BetJetton(next);
            if (isBipai)
            {
                PlayerHead bipaiPlayer = GetCanBipaiPlayer(next);
                if (bipaiPlayer == null)
                {
                    SetWinner(next);
                    return;
                }
                else
                {
                    bipaipanel.Show(bipaiPlayer, next);
                    Mogo.Util.TimerHeap.DelTimer(ShowCardTimeId);
                    ShowCardTimeId = Mogo.Util.TimerHeap.AddTimer<PlayerHead, PlayerHead>(2000, 0, BiPaiResult, bipaiPlayer, next);
                }
            }
        }
        else
        {
            next.GiveUp();
        }

    }
    protected override void OnShow(params object[] args)
    {
        InitPlayer();
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
            data.glodNum = 100000;
            head.SetData(data);
            headList.Add(head);
            head.playerIndex = i;
        }
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
        //测试可以打开
        //ShowPlayerCard(index);
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
            if (GenzhuTotalNum >= minBipaiGenzhuNum)
            {
                BiButton.interactable = true;
            }
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
        if(head1.IsHaveData() == false)
        {
            CreatePlayerCardData(head1.playerIndex);
        }
        if (head2.IsHaveData() == false)
        {
            CreatePlayerCardData(head2.playerIndex);
        }
        if (BiJiao(head1.dataList, head2.dataList))
        {
            head2.IsOver = true;
            return head1;
        }
        else
        {
            head1.IsOver = true;
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
