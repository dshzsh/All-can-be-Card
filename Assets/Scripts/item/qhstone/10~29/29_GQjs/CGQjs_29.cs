using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQjs_29 : CGqhsbase_11
{

}
public class DGQjs_29 : DataBase
{
    public float damageRate;
    public float range;
}
public class SGQjs_29 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, priorityQhstoneMagicUse);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQjs_29 card = _card as CGQjs_29;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQjs_29 config = basicConfig as DGQjs_29;

        CGQCjs_30 add = CreateCard<CGQCjs_30>();
        add.damageRate = config.damageRate * card.pow;
        add.range = config.range;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, add);
    }
}