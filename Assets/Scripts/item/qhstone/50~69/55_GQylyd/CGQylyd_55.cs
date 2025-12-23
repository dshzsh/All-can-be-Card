using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQylyd_55 : CGqhsbase_11
{

}
public class DGQylyd_55 : DataBase
{
    public float maxCnt;
    public BasicAtt sizeAdd, powAdd;
}
public class SGQylyd_55 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, priorityQhstoneMagicUse);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQylyd_55 card = _card as CGQylyd_55;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQylyd_55 config = basicConfig as DGQylyd_55;

        CCylyd_56 buff = CreateCard<CCylyd_56>();
        buff.powAdd = config.powAdd.WithPow(card.pow);
        buff.sizeAdd = config.sizeAdd.WithPow(card.pow);
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
}