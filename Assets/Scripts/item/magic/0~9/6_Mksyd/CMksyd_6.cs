using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMksyd_6 : Cmagicbase_17
{

}
public class DMksyd_6 : DataBase
{
    public BasicAtt speedUp;
    public float buffTime;
}
public class SMksyd_6 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMksyd_6 card = _card as CMksyd_6;
        DMksyd_6 config = basicConfig as DMksyd_6;

        CFksyd_2 buff = CreateCard<CFksyd_2>();buff.speedUp = config.speedUp.WithPow(msg.pow);
        Sbuff_35.GiveBuff(msg.live, msg.live, new MsgBeBuff() { time = config.buffTime, itemFrom = id, buff = buff });
    }
}