using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMOcsm_1 : CObj_2
{
    // 传送目标的vid
    public int targetID;
    // 传送对应地图的index
    public int index;
    // 传送门颜色
    public Color color;

    public float time;
}
public class DMOcsm_1 : DataBase
{
    // 保护时间，这个时候进传送门无效
    public float openTime;
}
public class SMOcsm_1 : SObj_2
{
    
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
    }
    public void OnItem(CardBase _card, MsgBase _msg)
    {
        CMOcsm_1 card = _card as CMOcsm_1;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op==1)
        {
            card.obj.gameObject.GetComponent<MMOcsm_1>().
                SetColor(card.color, DataManager.GetName(SfloorBase_2.GetFloorShow(card.targetID).showID));
        }
    }
    public void Update(CardBase _card, MsgBase _msg)
    {
        CMOcsm_1 card = _card as CMOcsm_1;
        MsgUpdate msg = _msg as MsgUpdate;

        card.time += msg.time;
    }
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CMOcsm_1 card = _card as CMOcsm_1;
        MsgCollision msg = _msg as MsgCollision;
        DMOcsm_1 config = basicConfig as DMOcsm_1;

        if (msg.other != GetMainPlayer()) return;
        if (config.openTime > card.time) return;

        Vector3 pos = Vector3.zero;Quaternion quaternion = Quaternion.identity;
        if (CardValid(SCmap_45.nowFloor))
        {
            pos = SCmap_45.nowFloor.obj.transform.position;
            quaternion = SCmap_45.nowFloor.obj.transform.rotation;
        }
        DestroyCard(SCmap_45.nowFloor);

        CfloorBase_2 cfloor = CreateCard(card.targetID) as CfloorBase_2;
        AddToWorld(cfloor);
        cfloor.obj.transform.SetParent(null);
        cfloor.obj.transform.SetPositionAndRotation(pos, quaternion);
        SCmap_45.ToNewFloor(cfloor, card.index);

        DestroyCard(card);
    }
}