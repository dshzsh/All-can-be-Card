using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQfl_41 : CGqhsbase_11
{

}
public class DGQfl_41 : DataBase
{
    public float timePer;
    public float flPow;
    public float cnt;
    public float scatter;
    public float posShift;
}
public class SGQfl_41 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQfl_41 card = _card as CGQfl_41;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQfl_41 config = basicConfig as DGQfl_41;

        CGQCfl_42 buff = CreateCard<CGQCfl_42>();
        buff.flPow = config.flPow * card.pow;
        buff.cnt = config.cnt;
        buff.timePer = config.timePer;
        buff.scatter = config.scatter;
        buff.posShift = config.posShift;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
}