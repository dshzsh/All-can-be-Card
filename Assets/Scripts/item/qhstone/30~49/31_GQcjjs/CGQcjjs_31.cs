using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQcjjs_31 : CGqhsbase_11
{

}
public class DGQcjjs_31 : DataBase
{
    public float damageRate;
    public float range;
}
public class SGQcjjs_31 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQcjjs_31 card = _card as CGQcjjs_31;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQcjjs_31 config = basicConfig as DGQcjjs_31;

        CGQCcjqh_32 add = CreateCard<CGQCcjqh_32>();
        add.damageRate = config.damageRate * card.pow;
        add.range = config.range;

        msg.AddMk(MsgMagicUse.AddCardType.bullet, add);
    }
}