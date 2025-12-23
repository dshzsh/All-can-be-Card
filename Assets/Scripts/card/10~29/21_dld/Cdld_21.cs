using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using System;
public class Cdld_21 : CardBase
{
    public int choose = 0;
    public float ljTime = 0;//连击计数器
    public float ljTimeMax = 1f;//连击计数器
    public List<CardBase> magics = new List<CardBase>();
}
public class Ddld_21 : DataBase
{
    public List<int> magics = new List<int>();
}
public class Sdld_21 : SystemBase
{
    public static void AddDld(CardBase mainMagic, params int[] magics)
    {
        Cdld_21 dld = CreateCard<Cdld_21>();
        dld.magics.Add(mainMagic);
        foreach (int id in magics)
        {
            dld.magics.Add(CreateCard(id));
        }
        AddComponent(mainMagic, dld);
    }
    public static CardBase CreateDldMagic(params int[] magics)
    {
        Cdld_21 dld = CreateCard<Cdld_21>();
        foreach(int id in magics)
        {
            dld.magics.Add(CreateCard(id));
        }
        CardBase magicMain = dld.magics[0];
        AddComponent(magicMain, dld);
        return magicMain;
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.MyMagicEnd, MagicEnd);
        AddHandle(MsgType.MyMagicBegin, MagicBegin, -10000);
    }
    private void ReplaceTo(Cdld_21 card, int choose)
    {
        if (card.choose == choose) return;
        //Debug.Log(cardAbandon.choose + " change to " + choose);
        card.choose = choose;// 切换到下一个动作

        if (card.parent == null) return;

        int key = -1;
        if (card.parent.parent != null)
        {
            if (card.parent.parent is Cmagic_14 mymagic)
            {
                key = mymagic.magics.FindIndex(value => value == card.parent);
            }
        }
        //Debug.Log(key);
        CardBase root = GetTop(card);
        SendMsg(root, MsgType.MagicOn, new MsgMagicOn() { key = key, op = -1 });

        ReplaceCard(card.parent, card.magics[card.choose]);

        SendMsg(root, MsgType.MagicOn, new MsgMagicOn() { key = key, op = 1, magic = card.parent });
    }
    void MagicBegin(CardBase _card, MsgBase _msg)
    {
        Cdld_21 card = _card as Cdld_21;
        MsgMagicUse msg = _msg as MsgMagicUse;

        if (card.parent != null && msg.magic != card.parent) return;

        card.ljTime = float.MaxValue;
    }
    void MagicEnd(CardBase _card, MsgBase _msg)
    {
        Cdld_21 card = _card as Cdld_21;
        MsgMagicUse msg = _msg as MsgMagicUse;

        if (card.parent != null && msg.magic != card.parent) return;
        if (card.magics.Count == 0) return;

        ReplaceTo(card, (card.choose + 1) % card.magics.Count);// 切换到下一个动作

        card.ljTime = card.ljTimeMax;
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Cdld_21 card = _card as Cdld_21;
        MsgUpdate msg = _msg as MsgUpdate;

        card.ljTime -= msg.time;
        if (card.ljTime < 0)
        {
            ReplaceTo(card, 0);
        }

    }
}