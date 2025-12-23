using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGAluck_13 : CGAattbase_1
{

}
public class SGAluck_13 : SGAattbase_1
{
    public static int mTLuckedOdds = MsgType.ParseMsgType(CardField.att, 13, 0);
    public class MsgLuckedOdds: MsgBase
    {
        public float baseOdds;
        public float luckedOdds;
    }
    public static float LuckedOdds(CardBase card, float odds)
    {
        float luckedOdds = odds * (Shealth_4.GetAttf(card, BasicAttID.luck) + 1);
        MsgLuckedOdds msg = new MsgLuckedOdds() { baseOdds = odds, luckedOdds = luckedOdds };
        SendMsg(GetTop(card), mTLuckedOdds, msg);
        return msg.luckedOdds;
    }
}