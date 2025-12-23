using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMOdzt_5 : CObj_2
{
    public CGbox_26 box;

    public bool used = false;
}
public class DMOdzt_5 : DataBase
{
    public string forgeUI;
    public float powAdd;
    public int qhcAdd;

    public static GameObject forgeUIPrefab;

    public override void Init(int id)
    {
        forgeUIPrefab = DataManager.LoadResource<GameObject>(id, forgeUI);
    }
}
public class MsgContainerAllItem: MsgBase
{
    public CardBase container;
    public List<CardBase> items = new List<CardBase>();
    public MsgContainerAllItem(CardBase container)
    {
        this.container = container;
    }

    public static List<CardBase> GetAllItem(CardBase container)
    {
        MsgContainerAllItem msg = new MsgContainerAllItem(container);
        SendMsg(container, MsgType.SelfContainerAllItem, msg);
        SendMsg(container, MsgType.MyContainerAllItem, msg);
        return msg.items;
    }
}
public class SMOdzt_5 : SObj_2
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeInteract, BeInteract);
        AddHandle(MsgType.MyInteractItem, MyInteractItem);

        AddHandle(MsgType.SelfContainerJudge, SelfContainerJudge);
    }
    public override void Create(CardBase _card)
    {
        CMOdzt_5 card = _card as CMOdzt_5;
        card.box = SGbox_26.BoxWithSlot(1);
        ActiveComponent(card, card.box);
    }
    void MyInteractItem(CardBase _card, MsgBase _msg)
    {
        CMOdzt_5 card = _card as CMOdzt_5;
        MsgInteractItem msg = _msg as MsgInteractItem;
        DMOdzt_5 config = basicConfig as DMOdzt_5;

        if (card.used) return;

        msg.used = card.used;

        CardBase magic = card.box.items[0];
        if (magic.id == 0)
        {
            UIBasic.GiveErrorText("没有放上魔法");
            return;
        }

        card.used = true;
        msg.used = card.used;

        // 强化槽位增加
        if (TryGetComponent<Cqhc_38>(magic, out var com))
        {
            for (int i = 0; i < config.qhcAdd; i++)
                com.qhstones.Add(ContainedCnull(com));
        }
        else
            AddComponent(magic, Sqhc_38.QhcWithSlot(config.qhcAdd));

        // 威力强化
        if (!(magic is Citem_33 cmagic)) return;
        cmagic.pow = new BasicAtt(config.powAdd).UseAttTo(cmagic.pow, 1);
    }
    void SelfContainerJudge(CardBase _card, MsgBase _msg)
    {
        CMOdzt_5 card = _card as CMOdzt_5;
        MsgContainerJudge msg = _msg as MsgContainerJudge;

        if (msg.gmsg.op == 1)
        {
            if (!Sitem_33.IsMagic(msg.gmsg.item))
            {
                msg.ok = false;
                msg.AddMsg("只能锻造魔法");
                return;
            }
        }

    }

    public void BeInteract(CardBase _card, MsgBase _msg)
    {
        CMOdzt_5 card = _card as CMOdzt_5;
        MsgBeInteract msg = _msg as MsgBeInteract;
        DMOdzt_5 config = basicConfig as DMOdzt_5;

        UIBasic.GiveUI(DMOdzt_5.forgeUIPrefab).GetComponent<MInteractBoxUI>()
            .SetWithUse(msg.live, card,
            $"选择强化一个技能\n{new AttAndRevise(BasicAttID.pow, new BasicAtt(config.powAdd))} 强化槽位+1",
            card.used, "已锻造", "锻造");
    }
}