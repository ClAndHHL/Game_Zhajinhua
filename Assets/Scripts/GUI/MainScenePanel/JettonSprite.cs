using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BattleFramework.Data;

public class JettonSprite {

    private GameObject jettonObj;
    private JettonData data;
    public void SetGameObject(GameObject jettonObj)
    {
        this.jettonObj = jettonObj;
        jettonObj.GetComponent<Button>().onClick.AddListener(OnClickHandler);
    }

    private void OnClickHandler()
    {
        if(data!=null)
        {
            Mogo.Util.EventDispatcher.TriggerEvent<JettonData>(GUIEvent.JIA_ZHU, data);
        }
    }
    public void SetData(BattleFramework.Data.JettonData data)
    {
        this.data = data;
        jettonObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/room/jetton/" + data.resPath);
    }
    public void SetEnabled(bool flag)
    {

    }

    internal void SetMinJetton(JettonData minJettionData)
    {
        if (data != null)
        {
            jettonObj.GetComponent<Button>().interactable = data.value >= minJettionData.value;
        }
    }
}
