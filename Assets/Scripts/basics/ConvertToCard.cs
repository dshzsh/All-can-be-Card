using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertToCard : MonoBehaviour
{
    public int id;
    public CardField cardField;

    private void Start()
    {
        CObj_2 card = CardManager.CreateCard(DataManager.VidToPid(id, cardField)) as CObj_2;
        if(card==null)
        {
            Debug.LogWarning("构建物体时失败 id:" + id + " cardFiled:" + cardField);
            return;
        }
        card.needGiveObj = false;
        CardManager.BondCardObj(gameObject, card);
        CardManager.AddToWorld(card);
    }
}
