using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQylyx_58 : CGqhsbase_11
{

}
public class DGQylyx_58 : DataBase
{
    public float maxCnt;
    public BasicAtt sizeAdd, powAdd;
    public BasicAtt initSizeAdd, initPowAdd;
}
public class SGQylyx_58 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, priorityQhstoneMagicUse);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQylyx_58 card = _card as CGQylyx_58;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQylyx_58 config = basicConfig as DGQylyx_58;

        CCylyd_56 buff = CreateCard<CCylyd_56>();
        buff.powAdd = config.powAdd;
        buff.sizeAdd = config.sizeAdd;
        buff.initSizeAdd = config.initSizeAdd.WithPow(card.pow);
        buff.accPowAdd = config.initPowAdd.WithPow(card.pow);
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
}