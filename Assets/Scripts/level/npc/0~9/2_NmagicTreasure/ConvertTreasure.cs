using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertTreasure : MonoBehaviour
{
    public int id;
    public CardField cardField;
    public int rare = -1;

    private void Start()
    {
        CNtreasure_1 card = CardManager.CreateCard(DataManager.VidToPid(id, cardField)) as CNtreasure_1;
        if (card == null)
        {
            Debug.LogWarning("构建宝箱时失败 id:" + id + " cardFiled:" + cardField);
            return;
        }
        card.rare = rare;
        card.needGiveObj = false;
        CardManager.BondCardObj(gameObject, card);
        CardManager.AddToWorld(card);
    }
}
