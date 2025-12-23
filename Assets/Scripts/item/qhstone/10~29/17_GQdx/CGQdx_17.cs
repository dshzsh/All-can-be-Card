using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQdx_17 : CGqhsbase_11
{

}
public class DGQdx_17 : DataBase
{
    public float sizeAdd;
}
public class SGQdx_17 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.CreateBullet, CreateBullet);
    }
    void CreateBullet(CardBase _card, MsgBase _msg)
    {
        CGQdx_17 card = _card as CGQdx_17;
        MsgBulletCreate msg = _msg as MsgBulletCreate;
        DGQdx_17 config = basicConfig as DGQdx_17;

        if (!Sqhc_38.IsQhMagic(card, msg.GetFromMagic())) return;

        msg.msg.AddExScale(config.sizeAdd * card.pow);
    }
}