using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCtlzs_34 : CardBase
{
    public float coin;
}
public class SGQCtlzs_34 : SystemBase
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.KillLive, KillLive);
    }
    void KillLive(CardBase _card, MsgBase _msg)
    {
        CGQCtlzs_34 card = _card as CGQCtlzs_34;
        MsgKillLive msg = _msg as MsgKillLive;

        if (!TryGetCobj(msg.live, out var cobj))
            return;

        if (Sysw_26.GetYsw(msg.live) != null) return;

        SMOcoin_3.GiveCoin(cobj.obj.Center, card.coin);
    }
}