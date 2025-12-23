using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGwlqh_3 : CGqhsbase_11
{

}
public class DGwlqh_3 : DataBase
{
    public float exPow;
    public BasicAtt powRevise = null;

    public override void Init(int id)
    {
        powRevise = new BasicAtt();
        powRevise.DirectMul += exPow;
    }
}
public class SGwlqh_3 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGwlqh_3 card = _card as CGwlqh_3;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGwlqh_3 config = basicConfig as DGwlqh_3;

        msg.pow = config.powRevise.WithPow(card.pow).UseAttTo(msg.pow, 1);
    }
}