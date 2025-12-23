using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFthing_10 : CfloorBase_2
{

}

public class SLFthing_10: SfloorBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CLFthing_10 card = _card as CLFthing_10;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op==1)
        {
            // 产生一个随机事件在敌人的点位
            int tid = SLTthingbase_1.GetRandThing(SCmap_45.mainMap.nowDifficulty, true);
            CLTthingbase_1 tcard = CreateCard(tid) as CLTthingbase_1;tcard.fromFloor = card;
            AddToWorld(tcard);
            tcard.obj.transform.SetParent(card.obj.transform);
            SumInfo sumInfo = GetEnemyPos(card);
            tcard.obj.transform.SetPositionAndRotation(sumInfo.pos, sumInfo.rotation);
        }
        
    }
}