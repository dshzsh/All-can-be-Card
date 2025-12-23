using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CMxljr_39 : Cmagicbase_17
{
    public float resetTime = 0f;
    public float totalDamage = 0f;
    public float damageTime = -1f;

    public float healTime = 0f;
    public TextMeshProUGUI tmp;
}
public class DMxljr_39 : DataBase
{
    public string upText;

    public GameObject upTextPrefab;
    public override void Init(int id)
    {
        base.Init(id);
        upTextPrefab = DataManager.LoadResource<GameObject>(id, upText);
    }
}
public class SMxljr_39 : Smagicbase_17
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        AddComponent(_card, CreateCard<CFwfyd_15>());
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeDamageAfter, BeDamageAfter);
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.BeBuff, BeBuff);
        AddHandle(MsgType.Die, Die, HandlerPriority.Low);
        AddHandle(MsgType.OnItem, OnItem);
    }
    private static void Reset(CMxljr_39 card)
    {
        card.resetTime = 0;//重置计时器
        card.totalDamage = 0;//总伤
        card.damageTime = -1;//伤害计时器，值为[攻击时间+1](开始标记)
    }
    private static void Show(CMxljr_39 card)
    {
        if (card.tmp == null) return;
        TextMeshProUGUI tmp = card.tmp;
        tmp.text = "总伤:" + MyTool.SuperNumText(card.totalDamage) + "\n" +
            "dps:" + MyTool.SuperNumText(card.totalDamage / Mathf.Max(0.1f, card.damageTime));
    }
    void BeDamageAfter(CardBase _card, MsgBase _msg)
    {
        CMxljr_39 card = _card as CMxljr_39;
        MsgBeDamage msg = _msg as MsgBeDamage;

        card.resetTime = 0;
        card.totalDamage += msg.damage;

        if (card.damageTime < 0f) card.damageTime = 0f;//标记开始dps计时

        Show(card);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CMxljr_39 card = _card as CMxljr_39;
        MsgUpdate msg = _msg as MsgUpdate;

        card.resetTime += msg.time;
        if (card.resetTime >= 6f)
            Reset(card);
        if (card.damageTime >= 0f) card.damageTime += msg.time;

        // 血量低后回满血
        if (MyTool.IntervalTime(0.5f, ref card.healTime, msg.time))
        {
            Show(card);

            CardBase live = GetTop(card);
            if (Shealth_4.GetNowHealth(live) < 0)
            {
                Shealth_4.GiveHeal(live, live, new MsgBeHeal(Shealth_4.GetAttf(live, BasicAttID.healthMax) * 10));
            }
        }
    }
    void BeBuff(CardBase _card, MsgBase _msg)
    {
        CMxljr_39 card = _card as CMxljr_39;
        MsgBeBuff msg = _msg as MsgBeBuff;

        string text = "受到" + msg.time + "s的" + DataManager.GetName(msg.buff.id) + " " + msg.buff.id + "";
        if (!TryGetCobj(card, out var cobj)) return;
        UIBasic.GiveText(text, cobj.obj.Center + (Vector3)Random.insideUnitSphere * 0.5f,
            Color.blue, 0.6f, Mathf.Clamp(msg.time, 1, 30));
    }
    void Die(CardBase _card, MsgBase _msg)
    {
        CMxljr_39 card = _card as CMxljr_39;
        MsgDie msg = _msg as MsgDie;

        msg.valid = false;
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CMxljr_39 card = _card as CMxljr_39;
        MsgOnItem msg = _msg as MsgOnItem;
        DMxljr_39 config = basicConfig as DMxljr_39;

        if (!TryGetCobj(card, out var cobj)) return;

        if(msg.op == 1)
        {
            card.tmp = UIBasic.GiveLiveUpUI(config.upTextPrefab, cobj).GetComponent<TextMeshProUGUI>();
            Show(card);
        }
        else
        {
            if (card.tmp != null)
                GameObject.Destroy(card.tmp.gameObject);
        }
    }
}