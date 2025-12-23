using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMfyjs_56 : Cmagicbase_17
{

}
public class DMfyjs_56 : DataBase
{
    public DbasicAtt.AttAndReviseData attData;

    public AttAndRevise att;
    public float time;
    public override void Init(int id)
    {
        base.Init(id);
        att = attData.ToRevise();
    }
}
public class SMfyjs_56 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }

    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMfyjs_56 card = _card as CMfyjs_56;
        DMfyjs_56 config = basicConfig as DMfyjs_56;

        CFfyjs_23 buff = CreateCard<CFfyjs_23>();
        buff.att = config.att.WithPow(msg.pow);
        Sbuff_35.GiveBuff(msg.live, msg.live, new MsgBeBuff(buff, config.time));
    }
}