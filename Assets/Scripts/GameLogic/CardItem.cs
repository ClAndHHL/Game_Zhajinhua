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
        if (data != null)
        {
            return;
        }
        if (graySprite == null)
        {
            graySprite = Resources.Load<Sprite>("Textures/joker/gray");
        }
        this.GetComponent<Image>().sprite = graySprite;
    }
    public void Reset()
    {
        this.GetComponent<Image>().sprite = sourceSprite;
    }
}
