using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGCitemObj_13 : CObj_2
{
    public CardBase card;
}
public class DGCitemObj_13 : DataBase
{

}

public class SGCitemObj_13 : SObj_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeInteract, BeInteract);
    }
    void BeInteract(CardBase _card, MsgBase _msg)
    {
        CGCitemObj_13 card = _card as CGCitemObj_13;
        MsgBeInteract msg = _msg as MsgBeInteract;

        Sbag_40.LiveGetItem(msg.live, card.card);
        DestroyCard(card);
    }
}