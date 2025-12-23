using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CEBmagic_32 : CardBase
{
    public float giveAtkDis = -1f;

    public float findLiveMaxDis = 50f;
}

public class SEBmagic_32: SystemBase
{
    public virtual Vector3 GetMagicPos(CEBmagic_32 card, CardBase _card, Cmagic_14 mymagic)
    {
        Clive_19 live = _card as Clive_19;
        if (live == null) return Vector3.zero;
        Clive_19 mb = Slive_19.FindLive(live, Slive_19.FindLiveMode.enemy, card.findLiveMaxDis);

        if (mb == null || Slive_19.GetDistance(mb, live) > card.findLiveMaxDis) return live.obj.transform.forward + live.obj.Center;
        return mb.obj.Center;
    }
    public virtual bool CanUseMagic(CEBmagic_32 card, CardBase live, Cmagic_14 mymagic)
    {
        if (live == null) return false;
        if (mymagic == null) return false;
        if (mymagic.nowUse == null) return true;
        return mymagic.nowUse.doWindDown;
    }
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CEBmagic_32 card = _card as CEBmagic_32;
        MsgUpdate msg = _msg as MsgUpdate;

        CardBase mylive = GetTop(card);

        if (!TryGetComponent<Cmagic_14>(mylive, out var mymagic)) return;
        if (!CanUseMagic(card, mylive, mymagic)) return;

        // 如果存在目标，且目标的距离比较远，就不施法了，等靠近再施法
        if(card.giveAtkDis > 0)
        {
            Clive_19 live = mylive as Clive_19;
            Clive_19 mb = Slive_19.FindLive(live, Slive_19.FindLiveMode.enemy, card.findLiveMaxDis);
            if (mb != null && Slive_19.GetDistance(mb, live) > card.giveAtkDis)
                return;
        }
        
        Vector3 pos = GetMagicPos(card, mylive, mymagic);

        for (int i = 0; i < mymagic.holdMax; i++)
        {
            SendMsg(mylive, MsgType.MagicCon, 
                new MsgMagicCon() { key = i, pos = pos });
        }
        
    }
}