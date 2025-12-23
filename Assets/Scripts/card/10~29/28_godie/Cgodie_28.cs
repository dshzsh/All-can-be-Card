using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Cgodie_28 : CardBase
{
    public bool startDie;
    public float trueDieTimeRes;
    public AnimationClip dieAni;
}
public class Dgodie_28 : DataBase
{
    
}
public class Sgodie_28 : SystemBase
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Die, Die, -10000);
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Cgodie_28 card = _card as Cgodie_28;
        MsgUpdate msg = _msg as MsgUpdate;

        if(card.startDie)
        {
            card.trueDieTimeRes -= msg.time;

            if (card.trueDieTimeRes< 0)
            {
                SendMsg(GetTop(card), MsgType.TrueDie, new MsgDie());
            }
        }
    }
    void Die(CardBase _card, MsgBase _msg)
    {
        Cgodie_28 card = _card as Cgodie_28;

        // 清除其他所有卡牌，清除碰撞箱，然后再进入动作
        // 死亡后的判定放在自己的子节点，不然会被清除
        if (TryGetCobj(card, out var cobj))
        {
            int ii = 0;
            
            if(ii == 0) { Debug.LogError("死掉没有直接放在物体下"); }
            cobj.obj.gameObject.GetComponent<Collider>().enabled = false;
            cobj.obj.gameObject.GetComponent<Rigidbody>().useGravity = false;
            //Debug.Log(cardAbandon.dieAni.length);
            SObj_2.PlayAtkAnim(cobj, card.dieAni);
            card.startDie = true;
            card.trueDieTimeRes = card.dieAni.length + 1f;
            cobj.obj.SetDie(1);
            if (cobj is Clive_19 clive)
                Slive_19.lives.Remove(clive);
        }

    }
}