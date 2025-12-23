using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFdamType_19 : Cbuffbase_36
{
    public int wxTag;
}
public class DFdamType_19 : DataBase
{

}
public class SFdamType_19 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        // 使用子弹的造成伤害之前的接口添加属性tag
        AddHandle(MsgType.GiveDamageBefore, GiveDamageBefore, HandlerPriority.Highest);
    }
    void GiveDamageBefore(CardBase _card, MsgBase _msg)
    {
        CFdamType_19 card = _card as CFdamType_19;
        MsgBeDamage msg = _msg as MsgBeDamage;

        CTTdamType_2 tag = CreateCard<CTTdamType_2>();
        tag.wxTag = card.wxTag;
        msg.AddTag(tag);
    }
}