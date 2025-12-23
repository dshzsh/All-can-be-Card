using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Csword_23 : CardBase
{
    public GameObject sword;
}

public class Ssword_23: SystemBase
{
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Csword_23 card = _card as Csword_23;
        MsgUpdate msg = _msg as MsgUpdate;

        if(TryGetCobj(card, out var cobj))
        {
            Transform rhand = cobj.obj.animator.GetBoneTransform(HumanBodyBones.RightHand);
            card.sword.transform.SetParent(rhand, false);
            //cardAbandon.sword.transform.localPosition = new Vector3(0.126000002f, 0.0280000009f, -0.361999989f);
            //cardAbandon.sword.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
    }
}