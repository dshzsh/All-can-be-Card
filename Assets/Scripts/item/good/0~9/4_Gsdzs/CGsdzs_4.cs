using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGsdzs_4 : Citem_33
{

}
public class DGsdzs_4 : DataBase
{
    public BasicAtt defReduce;
}
public class SGsdzs_4 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(SFzs_1.mTgiveZsDamageAfter, GiveZsDamageAfter);
    }
    void GiveZsDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGsdzs_4 card = _card as CGsdzs_4;
        MsgGiveZsDamageAfter msg = _msg as MsgGiveZsDamageAfter;
        DGsdzs_4 config = basicConfig as DGsdzs_4;

        new AttAndRevise(BasicAttID.def, config.defReduce).UseOnLive(msg.dmsg.to, 1, card.pow);
    }
}