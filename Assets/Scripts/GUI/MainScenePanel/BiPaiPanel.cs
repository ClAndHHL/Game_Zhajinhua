using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BiPaiPanel : IViewBase {

    private List<BiPaiPlayerHead> headList = new List<BiPaiPlayerHead>();
    
    protected override void OnStart()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject head = Find<Transform>("Player" + i).gameObject;
            BiPaiPlayerHead headSprite = new BiPaiPlayerHead();
            headSprite.SetGameObject(head);
            headList.Add(headSprite);
        }
    }
    protected override void AddEventListener()
    {
        base.AddEventListener();
    }
    protected override void RemoveEventListener()
    {
        base.RemoveEventListener();
    }
    public void SetData(PlayerHead head1,PlayerHead head2)
    {
        headList[0].SetPlayerData(head1);
        headList[1].SetPlayerData(head2);
    }
    protected override void OnShow(params object[] args)
    {
        
    }

    protected override void OnHide()
    {
        headList[0].Reset();
        headList[1].Reset();
    }
    protected override void OnDestory()
    {

    }
}
