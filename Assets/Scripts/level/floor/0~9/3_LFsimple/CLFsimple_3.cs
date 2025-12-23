using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CLFsimple_3 : CLFfightBase_6
{

}

public class SLFsimple_3: SLFfightBase_6
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CLFsimple_3 card = _card as CLFsimple_3;
        MsgUpdate msg = _msg as MsgUpdate;

        //cardAbandon.obj.transform.position += Vector3.up * msg.time;

        if (Input.GetKeyUp(KeyCode.Z))
        {
            GiveNextCsm(card);
        }
    }
}