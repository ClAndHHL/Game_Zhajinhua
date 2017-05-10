using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using BattleFramework.Data;

public class JiazhuPanel : IViewBase
{
    private Button closeButton;
    private List<JettonSprite> jettonList = new List<JettonSprite>();
    protected override void OnStart()
    {
        closeButton = panelObj.transform.Find("CloseButton").GetComponent<Button>();
        for (int i = 0; i < 4; i++)
        {
            GameObject jet = Find<Transform>("Jetton" + i).gameObject;
            JettonSprite jettonSprite = new JettonSprite();
            jettonSprite.SetGameObject(jet);
            jettonList.Add(jettonSprite);
        }
    }
    protected override void AddEventListener()
    {
        base.AddEventListener();
        closeButton.onClick.AddListener(CloseButtonClickHandler);
    }

    private void CloseButtonClickHandler()
    {
        Hide();
    }
    protected override void RemoveEventListener()
    {
        base.RemoveEventListener();
        closeButton.onClick.RemoveListener(CloseButtonClickHandler);
    }

    protected override void OnShow(params object[] args)
    {
        if(args != null && args.Length>0)
        {
            for (int i = 0; i < args.Length; i++)
            {
                JettonData data = JettonData.GetByID((int)args[i]);
                jettonList[i].SetData(data);
            }
        }
    }

    protected override void OnHide()
    {

    }
    protected override void OnDestory()
    {

    }

    internal void SetMinJetton(JettonData minJettionData)
    {
        for (int i = 0; i < jettonList.Count; i++)
        {
            jettonList[i].SetMinJetton(minJettionData);
        }
    }
}
