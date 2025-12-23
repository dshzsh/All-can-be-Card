using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCgh_22 : Citem_33
{
    public Color color;
    public float intensity;
}
public class DGQCgh_22 : DataBase
{

}
public class SGQCgh_22 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem, HandlerPriority.After);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CGQCgh_22 card = _card as CGQCgh_22;
        MsgOnItem msg = _msg as MsgOnItem;
        DGQCgh_22 config = basicConfig as DGQCgh_22;

        if(msg.op==1)
        {
            Light light = card._onTx.GetComponent<Light>();
            light.color = card.color;
            light.intensity = card.intensity * Sbullet_10.GetBulletPow(card);
        }
    }
}