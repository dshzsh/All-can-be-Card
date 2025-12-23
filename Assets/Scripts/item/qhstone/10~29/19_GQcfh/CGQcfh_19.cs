using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQcfh_19 : CGqhsbase_11
{

}
public class DGQcfh_19 : DataBase
{
    public BasicAtt usePowAdd;
    public float stunTimeRate;
}
public class SGQcfh_19 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagicAfter, MyUseMagicAfter);
    }
    void MyUseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CGQcfh_19 card = _card as CGQcfh_19;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQcfh_19 config = basicConfig as DGQcfh_19;

        if (msg.mdata.cd == 0) return;

        CFyx_11 buff = CreateCard<CFyx_11>();
        Sbuff_35.GiveBuff(msg.live, msg.live, new MsgBeBuff(buff, config.stunTimeRate * msg.mdata.cd, 0, Sbuff_35.BeBuffMode.coverByBig));
    }
}