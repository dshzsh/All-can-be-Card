using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CMcc_10 : Cmagicbase_17
{

}
public class DMcc_10 : DataBase
{
    public float distance;
    public float moveTime;
}
public class SMcc_10 : Smagicbase_17
{
    public static Vector3 GetSprintDir(CardBase card, Vector3 magicPos)
    {
        if (!TryGetCobj(card, out var cobj)) return Vector3.zero;

        Vector3 dir = MyMath.V3zeroYNor(magicPos - cobj.obj.Center);
        if (cobj is Clive_19 clive)
        {
            dir = clive.moveWantDir; dir.y = 0; dir.Normalize();
        }
        if (dir.magnitude <= MyMath.SmallFloat)
            dir = MyMath.V3zeroYNor(magicPos - cobj.obj.Center);
        return dir;
    }
    public static void GiveSprint(CardBase card, float distance, float moveTime, Vector3 dir, bool penetrate = false)
    {
        CardBase live = GetTop(card);
        CFmustMove_4 buff = CreateCard<CFmustMove_4>();
        buff.mustMoveVelocity = distance / moveTime * MyMath.V3zeroYNor(dir);
        Sbuff_35.GiveBuff(live, live, new MsgBeBuff(buff, moveTime));
        if(penetrate)
        {
            Sbuff_35.GiveBuff(live, live, new MsgBeBuff(CreateCard<CFwfzd_17>(), moveTime));
        }
    }
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMcc_10 card = _card as CMcc_10;
        DMcc_10 config = basicConfig as DMcc_10;

        Vector3 dir = GetSprintDir(card, msg.pos);

        GiveSprint(card, config.distance * msg.pow, config.moveTime, dir);

        CFcyxx_5 cy = CreateCard<CFcyxx_5>();
        Sbuff_35.GiveBuff(msg.live, msg.live, new MsgBeBuff(cy, config.moveTime));
    }
}