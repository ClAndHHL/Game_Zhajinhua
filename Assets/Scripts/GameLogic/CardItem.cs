using UnityEngine;
using System.Collections;
using BattleFramework.Data;
using UnityEngine.UI;
public class CardItem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetData(CardData cardData)
    {
        this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/joker/" + cardData.resPath);
    }
}
