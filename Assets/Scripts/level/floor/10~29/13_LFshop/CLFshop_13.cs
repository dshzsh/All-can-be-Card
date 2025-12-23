using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFshop_13 : CfloorBase_2
{

}

public class SLFshop_13 : SfloorBase_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(mTFloorStart, FloorStart);
    }
    private void GiveShop(CfloorBase_2 card ,SumInfo sumInfo, params int[] tag)
    {
        sumInfo = ConvertSumInfo(card, sumInfo);
        CNshop_3 shop = CreateCard<CNshop_3>();

        CardBase item = CreateCard(Sitem_33.GetRandomItem(-1, tag));
        if (tag[0] == MyTag.CardTag.magic)
            AddComponent(item, Sqhc_38.QhcWithSlot(2));

        SNshop_3.SetShop(shop, item);

        AddToWorld(shop);
        sumInfo.UseTo(shop.obj.transform);
    }
    void FloorStart(CardBase _card, MsgBase _msg)
    {
        CLFshop_13 card = _card as CLFshop_13;
        MsgFloorStart msg = _msg as MsgFloorStart;

        GiveNextCsm(card);

        // 产生一大堆的商店位，卖物品和技能和强化石
        // 0是卖道具的位置
        foreach(SumInfo sumInfo in card.enemySumInfos[0])
        {
            GiveShop(card, sumInfo, MyTag.CardTag.good, MyTag.CardTag.Pnormal);
        }
        // 1是卖技能的位置
        foreach (SumInfo sumInfo in card.enemySumInfos[1])
        {
            GiveShop(card, sumInfo, MyTag.CardTag.magic);
        }
        // 2是卖强化石的位置
        foreach (SumInfo sumInfo in card.enemySumInfos[2])
        {
            GiveShop(card, sumInfo, MyTag.CardTag.qhstone);
        }
    }
}