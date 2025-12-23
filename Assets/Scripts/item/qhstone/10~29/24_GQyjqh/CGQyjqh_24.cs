using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQyjqh_24 : CGqhsbase_11
{

}
public class DGQyjqh_24 : DataBase
{
    public BasicAtt powQh;
}
public class SGQyjqh_24 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem, HandlerPriority.Highest);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGQyjqh_24 card = _card as CGQyjqh_24;
        MsgOnItem msg = _msg as MsgOnItem;
        DGQyjqh_24 config = basicConfig as DGQyjqh_24;

        if(msg.op==1)
        {
            if(Sqhc_38.GetQhMagic(card) is Citem_33 citem)
            {
                citem.pow = config.powQh.WithPow(card.pow).UseAttTo(citem.pow, 1);
                DestroyCard(card);
            }
        }
    }
}