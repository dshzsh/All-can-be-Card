using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using static Sbuff_35;

public class CFhd_9 : Cbuffbase_36
{
    public float armor;

    public GameObject armorBar;
}
public class DFhd_9 : DataBase
{
    public string armorBarName;

    public GameObject armorBar;
    public override void Init(int id)
    {
        armorBar = DataManager.LoadResource<GameObject>(id, armorBarName);
    }
}
public class SFhd_9 : Sbuffbase_36
{
    public static int armorID;
    public static void GiveArmor(CardBase card, CardBase fromLive, float armor)
    {
        if (!TryGetCobj(card, out var live)) return;
        if (live.myBuff == null) return;

        BuffInfo buffInfo = Sbuff_35.GetBuff(live, armorID);
        if(buffInfo==null)
        {
            CFhd_9 hd = CreateCard<CFhd_9>();hd.armor = armor;
            Sbuff_35.GiveBuff(fromLive, live, new MsgBeBuff(hd, Sbuff_35.BuffSpeTime.Inf));
            return;
        }
        CFhd_9 chd = buffInfo.buff as CFhd_9;
        chd.armor += armor;
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeDamage, BeDamageBefore, HandlerPriority.Before);
        AddHandle(MsgType.OnItem, OnItem);
        armorID = id;
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFhd_9 card = _card as CFhd_9;
        MsgOnItem msg = _msg as MsgOnItem;
        DFhd_9 config = basicConfig as DFhd_9;

        if (!TryGetCobj(card, out var cobj)) return;

        if (msg.op == 1)
        {
            card.armorBar = UIBasic.GiveLiveUpUI(config.armorBar, cobj);
            card.armorBar.GetComponent<ArmorBar>().Set(card);
        }
        else
        {
            if (card.armorBar != null)
                GameObject.Destroy(card.armorBar);
        }
    }
    void BeDamageBefore(CardBase _card, MsgBase _msg)
    {
        CFhd_9 card = _card as CFhd_9;
        MsgBeDamage msg = _msg as MsgBeDamage;

        float armorDam = Mathf.Min(card.armor, msg.damage);

        Shealth_4.GiveDamageText(msg, armorDam);

        if (card.armor >= msg.damage)
        {
            card.armor -= msg.damage;
            msg.valid = false;
            return;
        }

        msg.damage -= card.armor;
        card.armor = 0;
        DestroyCard(card);
    }
}