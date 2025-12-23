using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMpqbb_19 : CMpressUseMagic_20
{
    public List<CardBase> coms = new();
}
public class DMpqbb_19 : DataBase
{
    public float interval;
    public float force;
    public float baseForce;
}
public class SMpqbb_19 : SMpressUseMagic_20
{
    public override void Init()
    {
        base.Init();
    }
    public override void BeginPressUse(CardBase _card, MsgMagicUse msg)
    {
        CMpqbb_19 card = _card as CMpqbb_19;
        DMpqbb_19 config = basicConfig as DMpqbb_19;

        CMCpqbb_21 com = CreateCard<CMCpqbb_21>();
        com.interval = config.interval;
        com.costPerShot = msg.mdata.cost;
        com.force = config.force * msg.pow + config.baseForce;

        card.coms.Add(com);
        ActiveComponent(card, com);
    }
    public override void EndPressUse(CardBase _card, MsgBase _msg)
    {
        CMpqbb_19 card = _card as CMpqbb_19;
        foreach (CardBase com in card.coms)
            InactiveComponent(card, com);
        card.coms.Clear();
    }
}