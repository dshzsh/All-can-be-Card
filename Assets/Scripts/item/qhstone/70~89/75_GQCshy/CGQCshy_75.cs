using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCshy_75 : CGqhsbase_11
{
    [NoActiveCard]
    public CMshy_64 fromMagic;
}
public class DGQCshy_75 : DataBase
{

}
public class SGQCshy_75 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGQCshy_75 card = _card as CGQCshy_75;
        MsgOnItem msg = _msg as MsgOnItem;
        DGQCshy_75 config = basicConfig as DGQCshy_75;

        if (!TryGetCobj(card, out var cobj)) return;

        if(!CardValid(card.fromMagic))
        {
            card.fromMagic = null;
            return;
        }

        if(msg.op==1)
        {
            card.fromMagic.shys.Add(cobj);
        }
    }
}