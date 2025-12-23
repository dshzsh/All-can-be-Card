using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQpzlq_60 : CGqhsbase_11
{

}
public class DGQpzlq_60 : DataBase
{
    public float cdRecover;
}
public class SGQpzlq_60 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, priorityQhstoneMagicUse);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQpzlq_60 card = _card as CGQpzlq_60;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQpzlq_60 config = basicConfig as DGQpzlq_60;

        CGQCpzlq_61 buff = CreateCard<CGQCpzlq_61>();
        buff.cdRecover = config.cdRecover * card.pow;
        buff.key = msg.costKey;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
}