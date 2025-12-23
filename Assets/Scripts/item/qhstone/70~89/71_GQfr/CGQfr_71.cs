using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQfr_71 : CGqhsbase_11
{

}
public class DGQfr_71 : DataBase
{
    public float zsPow;
}
public class SGQfr_71 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, priorityQhstoneMagicUse);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQfr_71 card = _card as CGQfr_71;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQfr_71 config = basicConfig as DGQfr_71;

        CGQCfr_74 buff = CreateCard<CGQCfr_74>();
        buff.zsPow = config.zsPow * card.pow;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
}