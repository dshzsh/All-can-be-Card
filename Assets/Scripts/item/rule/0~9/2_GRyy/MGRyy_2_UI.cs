using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGRyy_2_UI : MonoBehaviour
{
    public MItemCntUI yang, yin;

    CGRyy_2 card;
    public void Set(CGRyy_2 card)
    {
        this.card = card;
    }
    private void Update()
    {
        if(!CardManager.CardValid(card))
        {
            card = null;
            return;
        }

        yang.UpdateSet(card.yang.coin);
        yin.UpdateSet(card.yin.coin);
    }
}
