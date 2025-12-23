using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMfx_63 : Cmagicbase_17
{

}
public class DMfx_63 : DataBase
{
    public float distance;
    public float moveTime;
}
public class SMfx_63 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.UseMagicAfter, UseMagicAfter);
    }
    void UseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CMfx_63 card = _card as CMfx_63;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DMfx_63 config = basicConfig as DMfx_63;

        if (!msg.isConUse) return;

        Smagic_14.UseMagicWithBA(new MsgMagicUse(msg.live, card, msg.pos, bdUse: 1));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMfx_63 card = _card as CMfx_63;
        DMfx_63 config = basicConfig as DMfx_63;

        if(msg.bdUse == 1)
        {
            SMcc_10.GiveSprint(card, config.distance * msg.pow * Shealth_4.GetAttf(msg.live, BasicAttID.speed),
                config.moveTime, SMcc_10.GetSprintDir(card, msg.pos));

            Sbuff_35.GiveBuff(msg.live, msg.live, new MsgBeBuff(CreateCard<CFtywy_28>(), config.moveTime));
        }
    }
}