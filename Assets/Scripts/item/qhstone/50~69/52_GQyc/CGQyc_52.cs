using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQyc_52 : CGqhsbase_11
{

}
public class DGQyc_52 : DataBase
{
    public BasicAtt powAdd;
    public float timeDelay;
    public string magicCircle;
    public static GameObject magicCirclePrefab;

    public override void Init(int id)
    {
        base.Init(id);
        magicCirclePrefab = DataManager.LoadResource<GameObject>(id, magicCircle);
    }
}
public class SGQyc_52 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQyc_52 card = _card as CGQyc_52;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQyc_52 config = basicConfig as DGQyc_52;

        CGQCyc_53 buff = CreateCard<CGQCyc_53>();
        buff.powAdd = config.powAdd.WithPow(card.pow);
        buff.timeDelay = config.timeDelay;
        buff.color = SGRwx_4.GetItemLightColor(msg.magic);
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
}