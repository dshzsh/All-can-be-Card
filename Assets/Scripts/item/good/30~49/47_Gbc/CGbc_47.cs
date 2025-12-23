using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGbc_47 : Citem_33
{

}
public class DGbc_47 : DataBase
{
    public float damageUp;
}
public class SGbc_47 : Sitem_33
{
    public static int IsBackstab(CardBase from, CardBase to)
    {
        if (!TryGetCobj(from, out var fromObj)) return 0;
        if (!TryGetCobj(to, out var toObj)) return 0;

        Vector3 forward = toObj.obj.transform.forward;
        Vector3 dir = toObj.obj.Center - fromObj.obj.Center;
        float dot = Vector3.Dot(forward, dir);
        // Debug.Log(forward + ", " + dir + " " + dot);
        if (dot < 0f) return -1;
        if (dot > 0f) return 1;// 这个时候在背后攻击，是背刺
        return 0;
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.GiveDamageBefore, GiveDamageBefore);
    }
    void GiveDamageBefore(CardBase _card, MsgBase _msg)
    {
        CGbc_47 card = _card as CGbc_47;
        MsgBeDamage msg = _msg as MsgBeDamage;
        DGbc_47 config = basicConfig as DGbc_47;

        if(IsBackstab(msg.from, msg.to) == 1)
        {
            msg.damage *= 1 + config.damageUp * card.pow;
        }
    }
}