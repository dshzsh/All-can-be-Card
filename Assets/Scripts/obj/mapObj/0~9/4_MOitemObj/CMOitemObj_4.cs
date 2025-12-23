using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMOitemObj_4 : CObj_2
{
    public CardBase card;
}
public class DMOitemObj_4 : DataBase
{

}
public class MsgBeInteract : MsgBase
{
    public CardBase live;

    public MsgBeInteract(CardBase live)
    {
        this.live = live;
    }
}
public class SMOitemObj_4 : SObj_2
{
    public static CMOitemObj_4 GiveItemObj(CardBase card, Vector3 pos, Quaternion quaternion)
    {
        CMOitemObj_4 itemObj = CreateCard<CMOitemObj_4>();
        itemObj.card = card;
        AddToWorld(itemObj);
        itemObj.obj.GetComponent<ItemObj>().SetCard(card);
        itemObj.obj.transform.SetPositionAndRotation(pos, quaternion);
        return itemObj;
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BeInteract, BeInteract);
    }
    void BeInteract(CardBase _card, MsgBase _msg)
    {
        CMOitemObj_4 card = _card as CMOitemObj_4;
        MsgBeInteract msg = _msg as MsgBeInteract;

        Sbag_40.LiveGetItem(msg.live, card.card);
        DestroyCard(card);
    }
}