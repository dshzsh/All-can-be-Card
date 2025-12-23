using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQbtbx_70 : CGqhsbase_11
{

}
public class DGQbtbx_70 : DataBase
{
    public float cdRecover;
}
public class SGQbtbx_70 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.CreateBullet, CreateBullet, HandlerPriority.After);
    }
    void CreateBullet(CardBase _card, MsgBase _msg)
    {
        CGQbtbx_70 card = _card as CGQbtbx_70;
        MsgBulletCreate msg = _msg as MsgBulletCreate;
        DGQbtbx_70 config = basicConfig as DGQbtbx_70;

        Smagic_14.RecoverMagicCd(Sqhc_38.GetQhMagic(card), config.cdRecover * card.pow, false);
    }
}