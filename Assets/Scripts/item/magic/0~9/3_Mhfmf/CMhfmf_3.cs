using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMhfmf_3 : Cmagicbase_17
{

}
public class DMhfmf_3 : DataBase
{
    public float manaPerSec;
}
public class SMhfmf_3 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CMhfmf_3 card = _card as CMhfmf_3;
        MsgUpdate msg = _msg as MsgUpdate;
        DMhfmf_3 config = basicConfig as DMhfmf_3;

        SendMsg(GetTop(_card), MsgType.RestoreMana, new MsgRestoreMana(msg.time * card.pow * config.manaPerSec));
        //SendMsg(GetTop(_card), MsgType.RestoreCd, new MsgRestoreCd() { value = cardAbandon.pow, all = true });
    }
}