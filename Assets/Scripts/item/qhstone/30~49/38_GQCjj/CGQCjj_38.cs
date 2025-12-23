using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CGQCjj_38 : Citem_33
{
    public float damPerM;
    public float distance = 0f;

    [JsonIgnore]
    public Vector3 lastPos = default;
}
public class SGQCjj_38 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.FixedUpdate, Update);
        AddHandle(MsgType.GiveDamageBefore, GiveDamageBefore);
    }
    void GiveDamageBefore(CardBase _card, MsgBase _msg)
    {
        CGQCjj_38 card = _card as CGQCjj_38;
        MsgBeDamage msg = _msg as MsgBeDamage;

        msg.damage *= 1 + card.distance * card.damPerM;
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGQCjj_38 card = _card as CGQCjj_38;
        MsgUpdate msg = _msg as MsgUpdate;

        if (!TryGetCobj(card, out var cobj)) return;

        if(card.lastPos.magnitude == default)
        {
            card.lastPos = cobj.obj.transform.position;
            return;
        }

        card.distance += Vector3.Distance(card.lastPos, cobj.obj.transform.position);
        card.lastPos = cobj.obj.transform.position;
    }
}