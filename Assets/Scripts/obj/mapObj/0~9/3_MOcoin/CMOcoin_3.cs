using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMOcoin_3 : CObj_2
{
    public float cnt;
}
public class DMOcoin_3 : DataBase
{

}
public class SMOcoin_3 : SObj_2
{
    public static CMOcoin_3 GiveCoin(Vector3 pos, float cnt)
    {
        return GiveCoin(pos, Random.rotation, cnt);
    }
    public static CMOcoin_3 GiveCoin(Vector3 pos, Quaternion rot, float cnt)
    {
        CMOcoin_3 coin = CreateCard<CMOcoin_3>();
        coin.cnt = cnt;
        AddToWorld(coin);
        coin.obj.transform.SetPositionAndRotation(pos, rot);
        float size = Mathf.Clamp(cnt / 5, 0.1f, 10f);
        coin.obj.transform.localScale *= size;
        return coin;
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, Collision, HandlerPriority.After);
        AddHandle(MsgType.BeInteract, BeInteract);
    }
    public void BeInteract(CardBase _card, MsgBase _msg)
    {
        CMOcoin_3 card = _card as CMOcoin_3;
        MsgBeInteract msg = _msg as MsgBeInteract;        

        SGqb_24.GetCoin(msg.live, card.cnt);
        DestroyCard(card);
    }
    public void Collision(CardBase _card, MsgBase _msg)
    {
        CMOcoin_3 card = _card as CMOcoin_3;
        MsgCollision msg = _msg as MsgCollision;

        if (msg.other == null) return;
        if (msg.other != GetMainPlayer()) return;

        SGqb_24.GetCoin(msg.other, card.cnt);
        DestroyCard(card);
    }
}