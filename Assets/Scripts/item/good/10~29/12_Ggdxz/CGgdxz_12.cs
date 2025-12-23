using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGgdxz_12 : Citem_33
{

}
public class DGgdxz_12 : DataBase
{
    public float baseRate;
}
public class SGgdxz_12 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(SNtreasure_1.mTSummonTreasureBefore, mTSummonTreasureBefore);
    }
    void mTSummonTreasureBefore(CardBase _card, MsgBase _msg)
    {
        CGgdxz_12 card = _card as CGgdxz_12;
        SNtreasure_1.MsgSummonTreasure msg = _msg as SNtreasure_1.MsgSummonTreasure;
        DGgdxz_12 config = basicConfig as DGgdxz_12;

        if (msg.trea.id != DataManager.VidToPid(1, CardField.npc)) return;//必须是宝箱

        msg.choose++;

        float rate = SGAluck_13.LuckedOdds(card, config.baseRate) * card.pow;
        if (MyRandom.RandPer(rate))
        {
            msg.choose++;
        }
    }
}