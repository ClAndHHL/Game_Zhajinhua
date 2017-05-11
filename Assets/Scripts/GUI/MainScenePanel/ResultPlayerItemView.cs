using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResultPlayerItemView : IViewBase {

    private Text nameText;
    private Image success;
    private Image lose;
    private Text numText;
    private List<CardItem> cardList = new List<CardItem>();
    private Transform cardParentCon;
    private PlayerHead player;
    protected override void OnStart()
    {
        nameText = Find<Text>("nameText");
        success = Find<Image>("success");
        lose = Find<Image>("lose");
        numText = Find<Text>("numText");
        cardParentCon = Find<Transform>("CardCon");
        for (int i = 0; i < 3; i++)
        {
            cardList.Add(Find<Transform>("CardCon/CardCon" + i).gameObject.AddComponent<CardItem>());
        }
    }
    
    protected override void OnShow(params object[] args)
    {
        if(args != null && args.Length>0)
        {
            player = args[0] as PlayerHead;
            nameText.text = player.data.name;
            success.enabled = !player.IsOver;
            lose.enabled = !success.enabled;
            numText.text = player.GetChangeJetton() + "";
            if(player.IsHaveData())
            {
                for (int i = 0; i < cardList.Count; i++)
                {
                    cardList[i].SetData(player.dataList[i]);
                }
            }
        }
    }
    protected override void OnDestory()
    {
    }
    protected override void OnHide()
    {
    }
}
