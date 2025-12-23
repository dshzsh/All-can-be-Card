using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQyxfj_20 : CGqhsbase_11
{

}
public class DGQyxfj_20 : DataBase
{
    public float time;
}
public class SGQyxfj_20 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQyxfj_20 card = _card as CGQyxfj_20;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQyxfj_20 config = basicConfig as DGQyxfj_20;

        CFyx_11 buff = CreateCard<CFyx_11>();
        MsgBeBuff bebuff = new MsgBeBuff(buff, config.time, 0, Sbuff_35.BeBuffMode.coverByBig);
        CcolAddBuff_43 colbuff = CreateCard<CcolAddBuff_43>();
        colbuff.bebuff = bebuff;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, colbuff);
    }
}