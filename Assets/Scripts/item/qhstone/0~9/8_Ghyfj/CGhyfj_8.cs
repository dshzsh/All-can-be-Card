using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGhyfj_8 : CGqhsbase_11
{

}
public class DGhyfj_8 : DataBase
{
    public float interval;
    public float damage;
    public float time;
}
public class SGhyfj_8 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGhyfj_8 card = _card as CGhyfj_8;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGhyfj_8 config = basicConfig as DGhyfj_8;

        CFzs_1 buff = CreateCard<CFzs_1>();
        buff.interval = config.interval;
        buff.pow = config.damage * card.pow * Shealth_4.GetAttf(msg.live, BasicAttID.atk);
        MsgBeBuff bebuff = new(buff, config.time, 0, Sbuff_35.BeBuffMode.coverByBig);
        bebuff.from = msg.live;
        CcolAddBuff_43 colbuff = CreateCard<CcolAddBuff_43>();
        colbuff.bebuff = bebuff;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, colbuff);
    }
}