using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQjj_37 : CGqhsbase_11
{

}
public class DGQjj_37 : DataBase
{
    public float damPerM;
}
public class SGQjj_37 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQjj_37 card = _card as CGQjj_37;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQjj_37 config = basicConfig as DGQjj_37;

        CGQCjj_38 buff = CreateCard<CGQCjj_38>();
        buff.damPerM = card.pow * config.damPerM;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
}