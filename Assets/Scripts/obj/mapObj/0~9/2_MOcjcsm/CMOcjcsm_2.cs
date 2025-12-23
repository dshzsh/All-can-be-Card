using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMOcjcsm_2 : CObj_2
{
    // 传送目标的vid
    public int targetEnvID;
    // 传送门颜色
    public Color color;

    public float time;
}
public class DMOcjcsm_2 : DataBase
{
    // 保护时间，这个时候进传送门无效
    public float openTime;
}
public class SMOcjcsm_2 : SObj_2
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
        CMOcjcsm_2 card = _card as CMOcjcsm_2;
        MsgOnItem msg = _msg as MsgOnItem;

        if (msg.op == 1)
        {
            card.obj.gameObject.GetComponent<MMOcsm_1>().
                SetColor(card.color, DataManager.GetName(card.targetEnvID));
        }
    }
    public void Update(CardBase _card, MsgBase _msg)
    {
        CMOcjcsm_2 card = _card as CMOcjcsm_2;
        MsgUpdate msg = _msg as MsgUpdate;

        card.time += msg.time;
    }
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CMOcjcsm_2 card = _card as CMOcjcsm_2;
        MsgCollision msg = _msg as MsgCollision;
        DMOcjcsm_2 config = basicConfig as DMOcjcsm_2;

        if (msg.other != GetMainPlayer()) return;
        if (config.openTime > card.time) return;

        SCmap_45.mainMap.nowDifficulty++;
        SCmap_45.CreateNewMap(card.targetEnvID);

        Vector3 pos = Vector3.zero; Quaternion quaternion = Quaternion.identity;
        if (CardValid(SCmap_45.nowFloor))
        {
            pos = SCmap_45.nowFloor.obj.transform.position;
            quaternion = SCmap_45.nowFloor.obj.transform.rotation;
        }
        DestroyCard(SCmap_45.nowFloor);

        CfloorBase_2 cfloor = CreateCard(SCmap_45.mainMap.NowNode.id) as CfloorBase_2;
        AddToWorld(cfloor);
        cfloor.obj.transform.SetParent(null);
        cfloor.obj.transform.SetPositionAndRotation(pos, quaternion);
        SCmap_45.mainMap.nowHeight = -1;
        SCmap_45.ToNewFloor(cfloor, 0);

        DestroyCard(card);
    }
}