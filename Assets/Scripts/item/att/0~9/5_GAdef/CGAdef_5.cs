using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGAdef_5 : CGAattbase_1
{
    public float Reduce
    {
        get
        {
            return 1 - SGAdef_5.DefDamageReduction(bvalue.GetValue());
        }
    }
}
public class SGAdef_5 : SGAattbase_1
{
    public static float DefDamageReduction(float def)
    {
        if (def > 1)
            return 1 / (def);
        return 1 - (def - 1);
    }
    public override void Init()
    {
        AddHandle(MsgType.BeDamageBefore, BeDamageBefore);
    }
    void BeDamageBefore(CardBase _card, MsgBase _msg)
    {
        CGAdef_5 card = _card as CGAdef_5;
        MsgBeDamage msg = _msg as MsgBeDamage;

        if (msg.HaveTag(STTzssh_3.tid)) return;// 对于真实伤害，不应用防御力

        msg.damage *= DefDamageReduction(card.bvalue.GetValue());
    }
}