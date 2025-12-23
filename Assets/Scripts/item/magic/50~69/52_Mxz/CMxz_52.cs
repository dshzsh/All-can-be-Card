using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMxz_52 : Cmagicbase_17
{

}
public class DMxz_52 : DataBase
{
    public float manaPerB;
}
public class SMxz_52 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.CreateBullet, CreateBullet, HandlerPriority.After);
    }
    void CreateBullet(CardBase _card, MsgBase _msg)
    {
        CMxz_52 card = _card as CMxz_52;
        DMxz_52 config = basicConfig as DMxz_52;

        SendMsg(GetTop(_card), MsgType.RestoreMana, 
            new MsgRestoreMana(card.pow * config.manaPerB));
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMxz_52 card = _card as CMxz_52;
        DMxz_52 config = basicConfig as DMxz_52;

        
    }
}