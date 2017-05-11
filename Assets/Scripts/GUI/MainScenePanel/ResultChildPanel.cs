using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultChildPanel : IViewBase
{
    private List<ResultPlayerItemView> playerList = new List<ResultPlayerItemView>();
    protected override void OnStart()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject player = Find<Transform>("Player" + i).gameObject;
            ResultPlayerItemView resultPlayerItemView = new ResultPlayerItemView();
            resultPlayerItemView.panelObj = player;
            resultPlayerItemView.Start(false);
            playerList.Add(resultPlayerItemView);
            resultPlayerItemView.Hide();
        }
    }

    protected override void OnShow(params object[] args)
    {
        if (args != null && args.Length > 0)
        {
            List<PlayerHead> list = args[0] as List<PlayerHead>;
            for (int i = 0; i < list.Count; i++)
            {
                playerList[i].Show(list[i]);
            }
        }
    }

    protected override void OnHide()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].Hide();
        }
    }
    protected override void OnDestory()
    {

    }
}