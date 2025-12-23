using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQzsfj_65 : CGqhsbase_11
{

}
public class DGQzsfj_65 : DataBase
{
    public float damageRate;
}
public class SGQzsfj_65 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, priorityQhstoneMagicUse);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQzsfj_65 card = _card as CGQzsfj_65;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQzsfj_65 config = basicConfig as DGQzsfj_65;

        CGQCzsfj_66 buff = CreateCard<CGQCzsfj_66>();
        buff.damageRate = config.damageRate * card.pow;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
}