using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMyl_60 : Cmagicbase_17
{

}
public class DMyl_60 : DataBase
{
    public float time;

    public BasicAtt usePowAdd;
}
public class SMyl_60 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMyl_60 card = _card as CMyl_60;
        DMyl_60 config = basicConfig as DMyl_60;

        CFyl_27 buff = CreateCard<CFyl_27>();
        buff.usePowAdd = config.usePowAdd.WithPow(msg.pow);
        Sbuff_35.GiveBuff(msg.live, msg.live, new MsgBeBuff(buff, config.time));
    }
}