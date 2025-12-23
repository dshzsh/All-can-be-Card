using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CNmagicTreasure_2 : CNtreasure_1
{

}

public class SNmagicTreasure_2 : SNtreasure_1
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CNmagicTreasure_2 card = _card as CNmagicTreasure_2;
        card.tags.Clear();
        card.tags.Add(MyTag.CardTag.magic);
    }
    public override void Init()
    {
        base.Init();
        AddHandle(mTSummonTreasure, SummonTreasure);
    }
    void SummonTreasure(CardBase _card, MsgBase _msg)
    {
        CNmagicTreasure_2 card = _card as CNmagicTreasure_2;
        MsgSummonTreasure msg = _msg as MsgSummonTreasure;

        foreach(CardBase magic in msg.trea.cards)
        {
            Cqhc_38 qhc = Sqhc_38.QhcWithSlot(1);
            if (MyRandom.RandPer(0.5f, true))
            {
                CardBase qhs = CreateCard(Sitem_33.GetRandomItem(SummonRare(), MyTag.CardTag.qhstone));
                AddComponent(qhs, CreateCard<CGQwfxx_15>());
                qhc = Sqhc_38.QhcWithSlotCard(2, qhs);
            }
            AddComponent(magic, qhc);
        }
    }
}