using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Unity.VisualScripting.FullSerializer;

public class CGltwj_28 : Citem_33
{
    public int cnt;
}
public class DGltwj_28 : DataBase
{
    public float damage;
    public int useCnt;
}
public class SGltwj_28 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic);
        AddHandle(MsgType.UseMagicAfter, UseMagicAfter);
        AddHandle(MsgType.GiveDamageAfter, GiveDamageAfter);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGltwj_28 card = _card as CGltwj_28;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGltwj_28 config = basicConfig as DGltwj_28;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.initPos = msg.pos;
        bmsg.damage = msg.pow * config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        Sbullet_10.GiveBullet(GetTypeId(typeof(CBlj_2)), bmsg);
    }
    void UseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CGltwj_28 card = _card as CGltwj_28;
        MsgMagicUse msg = _msg as MsgMagicUse;

        if (!msg.isConUse) return;

        card.cnt++;
    }
    void GiveDamageAfter(CardBase _card, MsgBase _msg)
    {
        CGltwj_28 card = _card as CGltwj_28;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGltwj_28 config = basicConfig as DGltwj_28;

        if (card.cnt < config.useCnt) return;
        card.cnt = 0;

        Vector3 pos = Vector3.zero;
        if (TryGetCobj(msg.to, out var cobj)) pos = cobj.obj.transform.position;

        Smagic_14.UseMagicWithBA(new MsgMagicUse(cobj, card, pos));
    }
}