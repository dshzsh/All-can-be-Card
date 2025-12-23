using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFwzd_12 : CfloorBase_2
{

}

public class SLFwzd_12 : SfloorBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(mTFloorStart, FloorStart);
    }
    void FloorStart(CardBase _card, MsgBase _msg)
    {
        CLFwzd_12 card = _card as CLFwzd_12;
        MsgFloorStart msg = _msg as MsgFloorStart;

        GiveNextCsm(card);
        int qhsCnt = Random.Range(1, 2 + 1);
        int coinCnt = Random.Range(3, 5 + 1) + 2 * (2 - qhsCnt);
        for (int i = 0; i < coinCnt; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 3;pos.y = pos.y / 3 + 1.5f;
            SumInfo sumInfo = GetEnemyPos(card);
            pos += sumInfo.pos;
            SMOcoin_3.GiveCoin(pos, Random.rotation, Random.Range(3, 6 + 1));
        }
        for (int i = 0; i < qhsCnt; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 3; pos.y = pos.y / 3 + 1.5f;
            SumInfo sumInfo = GetEnemyPos(card);
            pos += sumInfo.pos;
            CardBase qhs = CreateCard(Sitem_33.GetRandomItem(SNtreasure_1.SummonRare(), MyTag.CardTag.qhstone));
            SMOitemObj_4.GiveItemObj(qhs, pos, Random.rotation);
        }
    }
}