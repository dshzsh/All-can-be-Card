using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQbh_54 : CGqhsbase_11
{

}
public class DGQbh_54 : DataBase
{
    public BasicAtt powRevise;
}
public class SGQbh_54 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQbh_54 card = _card as CGQbh_54;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQbh_54 config = basicConfig as DGQbh_54;

        if(Shealth_4.GetNowHealth(card, true)>=0.99f)
        {
            msg.pow = config.powRevise.WithPow(card.pow).UseAttTo(msg.pow, 1);
        }
    }
}