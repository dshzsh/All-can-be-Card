using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCzsfj_66 : Cbuffbase_36
{
    public float damageRate;
}
public class DGQCzsfj_66 : DataBase
{

}
public class SGQCzsfj_66 : Sbuffbase_36
{
    public override void Init()
    {
        AddHandle(MsgType.Collision, Collision);
    }
    void Collision(CardBase _card, MsgBase _msg)
    {
        CGQCzsfj_66 card = _card as CGQCzsfj_66;
        MsgCollision msg = _msg as MsgCollision;

        if (!TryGetCobj(card, out var cobj)) return;
        if (cobj is not Cbullet_10 cbullet) return;
        if (cbullet.colDam == null) return;

        MsgBeDamage dmsg = new MsgBeDamage(cbullet.colDam.damage * cbullet.bulletPow * card.damageRate);
        dmsg.AddTag(CreateCard<CTTzssh_3>());
        Scoldam_5.GiveBulletDamage(card, msg, dmsg, false);
    }
}