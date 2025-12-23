using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQjbrf_36 : CGqhsbase_11
{

}
public class DGQjbrf_36 : DataBase
{
    public DbasicAtt.AttAndReviseData attData;

    public AttAndRevise att;
    public override void Init(int id)
    {
        base.Init(id);
        att = attData.ToRevise();
    }
}
public class SGQjbrf_36 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQjbrf_36 card = _card as CGQjbrf_36;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQjbrf_36 config = basicConfig as DGQjbrf_36;

        CFattChange_10 buff = CreateCard<CFattChange_10>();
        buff.attAndRevise = config.att.WithPow(card.pow);
        msg.AddMk(MsgMagicUse.AddCardType.ysw, buff);
    }
}