using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGhmjs_14 : CGqhsbase_11
{
    public BasicAtt costRevise = new();
}
public class DGhmjs_14 : DataBase
{
    public float reduce;
}
public class SGhmjs_14 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGhmjs_14 card = _card as CGhmjs_14;
        DGhmjs_14 config = basicConfig as DGhmjs_14;
        card.costRevise.DirectMul += -config.reduce;
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGhmjs_14 card = _card as CGhmjs_14;
        MsgOnItem msg = _msg as MsgOnItem;
        DGhmjs_14 config = basicConfig as DGhmjs_14;

        CardBase magic = Sqhc_38.GetQhMagic(card);
        if(magic != null&&magic is Cmagicbase_17 cmagic)
        {
            cmagic.mdata.cost = card.costRevise.WithPow(card.pow).UseAttTo(cmagic.mdata.cost, msg.op);
        }
    }
}