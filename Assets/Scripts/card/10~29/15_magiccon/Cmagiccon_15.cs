using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Cmagiccon_15 : CardBase
{
    public bool[] lastPress = new bool[ConKeys.UseMagics.Length];
    public float[] pressTime = new float[ConKeys.UseMagics.Length];
}
public class MsgMagicPress :MsgBase
{
    public bool isStart = false;
    public bool isEnd = false;
    public int key = 0;
    public CardBase magic;//由Cmagic给予
    public bool isNowUse = false;
    public MsgMagicUse nowUse;
    public float time;
    public Vector3 pos;
}
public class Smagiccon_15 : SystemBase
{
    public static LayerMask groundLayer = LayerMask.GetMask("ground");
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Cmagiccon_15 card = _card as Cmagiccon_15;
        MsgUpdate msg = _msg as MsgUpdate;

        if(!SControl_3.InControl()) { return; }

        Vector3 pos = SShoulderCamera_37.GetLookPos(card);

        for (int i = 0; i < ConKeys.UseMagics.Length; i++)
        {
            if (Input.GetKeyDown(ConKeys.UseMagics[i]))//单点释放魔法
            {
                SendMsg(msg.cobj, MsgType.MagicCon, new MsgMagicCon { cobj = msg.cobj, key = i, pos = pos });
            }

            if(Input.GetKey(ConKeys.UseMagics[i])) // 蓄力释放逻辑
            {
                bool isStart = !card.lastPress[i];
                card.lastPress[i] = true;
                if (isStart)
                {
                    card.pressTime[i] = 0;
                }
                else card.pressTime[i] += msg.time;

                SendMsg(msg.cobj, MsgType.MagicPress, new MsgMagicPress { key = i, isStart = isStart, time = card.pressTime[i], pos = pos });
                
            }
            else 
            {
                if (card.lastPress[i])
                    SendMsg(msg.cobj, MsgType.MagicPress, new MsgMagicPress { key = i, isEnd = true, time = card.pressTime[i], pos = pos });
                card.lastPress[i] = false;
            }
            
        }
        
    }
    
}