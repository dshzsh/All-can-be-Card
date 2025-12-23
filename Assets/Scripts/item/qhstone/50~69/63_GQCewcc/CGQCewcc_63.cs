using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCewcc_63 : CGqhsbase_11
{
    public float exTimeMax = 1;// 额外存储的时间，以基本的魔法的冷却上限为准
    public float magicCd = 1f;// 只是用作标识的魔法冷却
    public float exTime = 0;
}
public class DGQCewcc_63 : DataBase
{
    public string goodUI;

    public static GameObject goodUIPrefab;
    public override void Init(int id)
    {
        goodUIPrefab = DataManager.LoadResource<GameObject>(id, goodUI);
    }
}
public class SGQCewcc_63 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);// 装备和卸载会清空存储的所有冷却
        AddHandle(SLFfightBase_6.mTFightStart, FightStart);// 战斗开始也会清空冷却

        AddHandle(MsgType.RestoreCd, RestoreCd, HandlerPriority.After);
        AddHandle(MsgType.MyUseMagicAfter, MyUseMagicAfter);

        AddHandle(MsgType.MyShowGoodUI, MyShowGoodUI);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGQCewcc_63 card = _card as CGQCewcc_63;

        card.exTime = 0f;
    }
    void FightStart(CardBase _card, MsgBase _msg)
    {
        CGQCewcc_63 card = _card as CGQCewcc_63;

        card.exTime = 0f;
    }
    void MyUseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CGQCewcc_63 card = _card as CGQCewcc_63;
        MsgMagicUse msg = _msg as MsgMagicUse;

        float exTime = card.exTime;
        card.exTime = 0;
        Smagic_14.RecoverMagicCd(msg.magic, exTime, false);
    }
    void RestoreCd(CardBase _card, MsgBase _msg)
    {
        CGQCewcc_63 card = _card as CGQCewcc_63;
        MsgRestoreCd msg = _msg as MsgRestoreCd;

        CardBase magic = Sqhc_38.GetQhMagic(card);
        // Debug.Log(msg.magic + " " + msg.resTime);
        if (!msg.IsMyMagic(magic)) return;
        if (magic == null || magic is not Cmagicbase_17 cmagic) return;
        float exTimeMax = cmagic.mdata.cd * card.exTimeMax * card.pow;
        card.magicCd = cmagic.mdata.cd;

        card.exTime += msg.resTime;
        if (card.exTime > exTimeMax)
        {
            msg.resTime = card.exTime - exTimeMax;
            card.exTime = exTimeMax;
        }
        else msg.resTime = 0;
    }

    void MyShowGoodUI(CardBase _card, MsgBase _msg)
    {
        CGQCewcc_63 card = _card as CGQCewcc_63;
        MsgMyShowGoodUI msg = _msg as MsgMyShowGoodUI;

        if(msg.op==1)
        {
            GameObject obj = GameObject.Instantiate(DGQCewcc_63.goodUIPrefab);
            obj.GetComponent<MGQCewcc_63_goodui>().Set(card);
            msg.goodUI.GiveUI(card, obj);
        }
        else msg.goodUI.RemoveUI(card);
    }
}