using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLTtlzq_2 : CLTthingbase_1
{

}
public class DLTtlzq_2 : DataBase
{
    public int coin0;
    public int coin1;
    public int coin2;
}

public class SLTtlzq_2 : SLTthingbase_1
{
    public override void ChatCallBack(CLTthingbase_1 _card, int nowChat, int choose, CardBase live)
    {
        CLTtlzq_2 card = _card as CLTtlzq_2;
        DLTtlzq_2 config = basicConfig as DLTtlzq_2;

        // Debug.Log($"{nowChat} {choose}");

        if (nowChat == 0)
        {
            switch (choose)
            {
                case 0:
                    {
                        SGqb_24.GetCoin(live, config.coin0);
                        break;
                    }
                case 1:
                    {
                        SGqb_24.GetCoin(live, config.coin1);
                        Sbag_40.LiveGetItem(live, CreateCard<CGch_25>());
                        break;
                    }
                case 2:
                    {
                        if(!SGqb_24.CostCoin(live, config.coin2))
                        {
                            ToChat(card, 0);
                            UIBasic.GiveErrorText("金币不足！");
                            return;
                        }
                        Cbag_40 bag = Sbag_40.LiveGetBag(live);
                        if (bag == null) break;
                        List<CardBase> list = new List<CardBase>(bag.goods);
                        foreach(CardBase good in list)
                        {
                            if (good.id != SGch_25.sid) continue;

                            Sbag_40.OffItem(bag, good);
                        }
                        break;
                    }
            }
            EndChat(card);
        }
        else
        {
            EndChat(card);
        }
    }
}