using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGjtqh_9 : CGqhsbase_11
{

}
public class DGjtqh_9 : DataBase
{
    public float force;
    public float time;
}
public class SGjtqh_9 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGjtqh_9 card = _card as CGjtqh_9;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGjtqh_9 config = basicConfig as DGjtqh_9;

        if (msg.magic != Sqhc_38.GetQhMagic(card)) return;

        CGCjtqh_10 add = CreateCard<CGCjtqh_10>();
        add.force = config.force * card.pow;
        add.time = config.time;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, add);
    }
}