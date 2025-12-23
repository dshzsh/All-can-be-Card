using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLTdjs_3 : CLTthingbase_1
{
    public CGbox_26 box;
}
public class DLTdjs_3 : DataBase
{
    public int maxNum;
    public float cost1;
    public float coin2;
}

public class SLTdjs_3 : SLTthingbase_1
{
    public override void Create(CardBase _card)
    {
        CLTdjs_3 card = _card as CLTdjs_3;
        DLTdjs_3 config = basicConfig as DLTdjs_3;
        card.box = SGbox_26.BoxWithSlot(config.maxNum);
        ActiveComponent(card, card.box);
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.SelfContainerGet, SelfContainerGet);
    }
    private float CalItemsValue(CLTdjs_3 card)
    {
        float sum = 0;
        foreach(var item in card.box.items)
        {
            sum += SGqb_24.ItemValue(item);
        }
        return sum;
    }
    void SelfContainerGet(CardBase _card, MsgBase _msg)
    {
        CLTdjs_3 card = _card as CLTdjs_3;

        //Debug.Log("1");
        ChangeUITitle(card, $"选择转化的物品\n预计金币：{MyTool.SuperNumText(CalItemsValue(card))}");
    }
    public override void ChatCallBack(CLTthingbase_1 _card, int nowChat, int choose, CardBase live)
    {
        CLTdjs_3 card = _card as CLTdjs_3;
        DLTdjs_3 config = basicConfig as DLTdjs_3;

        if (nowChat == 0)
        {
            switch (choose)
            {
                case 0:
                    {
                        ChooseItem(card, 1, $"选择转化的物品\n预计金币：{MyTool.SuperNumText(CalItemsValue(card))}", "确认转化");
                        break;
                    }
                case 1:
                    {
                        if (!SGqb_24.CostCoin(live, config.cost1))
                        {
                            ToChat(card, 0);
                            UIBasic.GiveErrorText("金币不足！");
                            return;
                        }
                        CardBase item = CreateCard(Sitem_33.GetRandomItem(SNtreasure_1.SummonRare(), MyTag.CardTag.good));
                        SMOitemObj_4.GiveItemObj(item, card.obj.transform.position, card.obj.transform.rotation);
                        EndChat(card);
                        break;
                    }
                case 2:
                    {
                        SGqb_24.GetCoin(live, config.coin2);
                        EndChat(card);
                        break;
                    }
            }
        }
        else if(nowChat==1)
        {
            // 计算内部物体的value，直接加钱
            SGqb_24.GetCoin(live, CalItemsValue(card));

            EndChat(card);
        }
        else
        {
            EndChat(card);
        }
    }
}