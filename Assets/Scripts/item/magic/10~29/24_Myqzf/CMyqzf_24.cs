using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMyqzf_24 : Cmagicbase_17
{

}
public class DMyqzf_24 : DataBase
{
    public float time;
    public BasicAtt atkAdd;
}
public class SMyqzf_24 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMyqzf_24 card = _card as CMyqzf_24;
        DMyqzf_24 config = basicConfig as DMyqzf_24;

        CFyqzf_8 buff = CreateCard<CFyqzf_8>();
        buff.atkAdd = config.atkAdd.WithPow(msg.pow);
        Sbuff_35.GiveBuff(msg.live, msg.live, new MsgBeBuff(buff, config.time));
    }
}