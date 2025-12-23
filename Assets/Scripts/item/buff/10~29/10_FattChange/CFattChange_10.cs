using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFattChange_10 : Cbuffbase_36
{
    public AttAndRevise attAndRevise = null;
    public AttAndRevise attAndRevise2 = null;
}
public class DFattChange_10 : DataBase
{

}
public class SFattChange_10 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFattChange_10 card = _card as CFattChange_10;
        MsgOnItem msg = _msg as MsgOnItem;

        if (card.attAndRevise != null)
            card.attAndRevise.UseOnLive(card, msg.op, card.pow);
        if (card.attAndRevise2 != null)
            card.attAndRevise2.UseOnLive(card, msg.op, card.pow);
    }
}