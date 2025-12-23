using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGjh_21 : Citem_33
{

}
public class DGjh_21 : DataBase
{
    public BasicAtt atkSpeedAdd;
    public float time;
    public float lestTime;
}
public class SGjh_21 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.UseMagicAfter, UseMagicAfter);
    }
    void UseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CGjh_21 card = _card as CGjh_21;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGjh_21 config = basicConfig as DGjh_21;

        if (!(msg.mdata.cd >= config.lestTime - MyMath.SmallFloat)) return;

        CFattChange_10 buff = CreateCard<CFattChange_10>();
        buff.attAndRevise = new AttAndRevise(BasicAttID.atkSpeed, config.atkSpeedAdd.WithPow(card.pow));
        CardBase live = GetTop(card);
        Sbuff_35.GiveBuff(live, live, new MsgBeBuff(buff, config.time));
    }
}