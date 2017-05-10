using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
public class BiPaiPlayerHead {

    private GameObject headObj;
    private Text nameText;
    private List<Transform> conList = new List<Transform>();
    private Transform cardCon;
    private PlayerHead playerHead;
    internal void SetGameObject(GameObject head)
    {
        headObj = head;
        nameText = headObj.transform.Find("playerNameText").GetComponent<Text>();
        cardCon = headObj.transform.Find("CardCon");
        for (int i = 0; i < 3; i++)
        {
            conList.Add(cardCon.Find("CardCon" + i));
        }
        cardCon.gameObject.SetActive(false);
    }
    public void SetPlayerData(PlayerHead playerHead)
    {
        this.playerHead = playerHead;
        nameText.text = playerHead.data.name;
        for (int i = 0; i < playerHead.cardItemList.Count; i++)
        {
            Tweener tweener = playerHead.cardItemList[i].transform.DOMove(conList[i].position, 0.2f);
        }
    }

    public void Reset()
    {
        if (playerHead!=null)
            playerHead.GoBack();
    }
}
