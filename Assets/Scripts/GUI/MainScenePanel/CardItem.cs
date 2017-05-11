using UnityEngine;
using System.Collections;
using BattleFramework.Data;
using UnityEngine.UI;
public class CardItem : MonoBehaviour {

    private CardData data;
    private Sprite sourceSprite;
    private Sprite graySprite;

    public void SetData(CardData cardData)
    {
        if (sourceSprite == null)
        {
            sourceSprite = this.GetComponent<Image>().sprite;
        }
        data = cardData;
        this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/joker/" + cardData.resPath);
    }

    internal void GiveUp()
    {
        this.GetComponent<Image>().color = Color.gray;
    }
    public void Reset()
    {
        if (sourceSprite !=null)
        {
            this.GetComponent<Image>().sprite = sourceSprite;
        }
        this.GetComponent<Image>().color = Color.white;
        data = null;
    }
}
