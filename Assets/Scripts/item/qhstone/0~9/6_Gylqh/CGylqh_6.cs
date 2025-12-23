using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGylqh_6 : CGqhsbase_11
{

}
public class DGylqh_6 : DataBase
{
    public float speedUp;
    public float UpPos;
}
public class SGylqh_6 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.CreateBullet, CreateBullet);
    }
    void CreateBullet(CardBase _card, MsgBase _msg)
    {
        CGylqh_6 card = _card as CGylqh_6;
        MsgBulletCreate msg = _msg as MsgBulletCreate;
        DGylqh_6 config = basicConfig as DGylqh_6;

        if (msg.msg.fromMagicUse == null) return;
        if (msg.msg.fromMagicUse.magic != Sqhc_38.GetQhMagic(card)) return;

        msg.msg.speed.finalMul *= config.speedUp * card.pow;

        msg.msg.initPos = msg.msg.fromMagicUse.pos + Vector3.up * config.UpPos;
        msg.msg.dir = Vector3.down;
    }
}