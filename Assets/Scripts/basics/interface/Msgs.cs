using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//负数象征不需要进入cobj的队列维护中去
public static class MsgType
{
    public const int SelfMsg = 1000000;
    public static int None = 0;
    public static int Test=1;

    public static int Update=2;
    public static int FixedUpdate=3;
    //public static int LateUpdate=4;

    public static int MoveControl=5;
    public static int Jump=6;
    public static int RotateControl=7;
    public static int GetLookPos=8;
    public static int Collision=9;

    public static int BeBuff=10;
    public static int GiveBuff=11;

    public static int Die=12;//尝试进入死亡状态
    public static int TrueDie=13;//真正的死亡
    public static int BeDamage=14;
    public static int GiveDamage=15;
    public static int BeHeal=16;
    public static int GiveHeal=17;

    public static int MagicCon=18;
    public static int MagicOn=19;//尝试卸下或装备魔法
    public static int OnItem=-20;//自身被提携，不进入优化
    public static int BeginUseMagic=21;//试图释放魔法
    public static int MyMagicBegin=-22;//魔法准备阶段
    public static int MagicBegin = 22;//判断消耗的阶段
    public static int MyUseMagicBefore=-23;
    public static int UseMagicBefore = 23;
    public static int MyUseMagic=-24;//魔法释放阶段
    public static int UseMagicAfter = 25;
    public static int MyUseMagicAfter=-25;
    public static int MyMagicEnd=-26;//魔法结束阶段
    public static int RestoreMana=27;//恢复法力
    public static int RestoreCd=28;//恢复冷却

    public static int OnStone=29;

    public static int CreateBullet=30;
    public static int BulletStart=31;

    public static int OnShowUI = 32;
    public static int ParseDescribe = 33;
    public static int SelfParseDescribe = 33 + SelfMsg;
    public static int MagicPress = 34;
    public static int MagicInterrupt = 35;
    public static int MyMagicInterrupt = -35;
    public static int MyContainerGet = 36;
    public static int SelfContainerGet = 36 + SelfMsg;
    public static int SelfContainerJudge = 37 + SelfMsg;//判断物品是否能装备
    public static int MyContainerJudge = -37;
    public static int BeInteract = 38;
    public static int BeDamageBefore = 39;
    public static int GiveDamageBefore = 40;
    public static int BeDamageAfter = 41;
    public static int GiveDamageAfter = 42;
    public static int CostMana = 43;
    public static int CollisionExit = 44;
    public static int MyInteractItem = -45;
    public static int InteractItem = 45;
    public static int MyContainerAllItem = 46;
    public static int SelfContainerAllItem = 46 + SelfMsg;//判断物品是否能装备
    public static int KillLive = 47;
    public static int UpdateSec = 48;// 每秒更新一次的update
    public static int MyShowGoodUI = -49;
    public static int CostHealth = 50;

    public static int ParseMsgType(int pid, int msgId)
    {
        return pid * 100 + msgId;
    }
    public static int ParseMsgType(CardField field, int vid, int msgId)
    {
        return DataManager.VidToPid(vid, field) * 100 + msgId;
    }
    public static bool IsSelfMsg(int pid)
    {
        return pid >= SelfMsg;
    }
    public static bool IsAddToObjMsg(int pid)
    {
        return pid < SelfMsg && pid > 0 && pid != OnItem;
    }
}
public class MsgSendMsg:MsgBase
{
    public MsgBase msg;
    public CardBase to;
    public MsgSendMsg(MsgBase msg, CardBase to)
    {
        this.msg = msg;
        this.to = to;
    }
}
public class MsgCollision :MsgBase
{
    // 由碰撞方向被碰撞方发送碰撞消息
    public CObj_2 cobj;
    public CObj_2 other;
    public GameObject obj;
    public Rigidbody robj;
    public Vector3 hitPos;
    public bool hit = false;
    public MsgCollision() { }
    public MsgCollision(CObj_2 cobj, CObj_2 other, GameObject obj, Vector3 hitPos, Rigidbody robj = null)
    {
        this.cobj = cobj;
        this.other = other;
        this.obj = obj;
        this.hitPos = hitPos;
        if(robj == null)
            robj = obj.GetComponent<Rigidbody>();
        this.robj = robj;
    }
    public bool JudgeCollision(CardBase card, Slive_19.FindLiveMode findLiveMode)
    {
        if (other == null) return false;

        if (CardManager.TryGetCobj(card, out var obj1) && CardManager.TryGetCobj(other, out var obj2))
        {
            if (Slive_19.TeamSatisfy(obj1.team, obj2.team, findLiveMode)) return true;
        }
        return false;
    }
}
public class MsgTrigger : MsgBase
{
    public Collider other;
    public MsgTrigger(Collider other)
    {
        this.other = other;
    }
}
public class MsgUpdate : MsgBase
{
    public CObj_2 cobj;
    public float time;
    
    public MsgUpdate(CObj_2 cobj, float time)
    {
        this.cobj = cobj;
        this.time = time;
    }
}
public class MsgOnItemOther : MsgBase
{
    public MsgOnItem msg;
    public MsgOnItemOther(MsgOnItem msg)
    {
        this.msg = msg;
    }
}
public class MsgParseDescribe : MsgBase
{
    public CardBase card;
    public string text;
    public MsgParseDescribe() { }
    public MsgParseDescribe(CardBase card, string text)
    {
        this.card = card;
        this.text = text;
    }

    public void InsertField(string title, string color, string content)
    {
        this.text = Sitem_33.InsertField(text, title, color, content);
    }
    public void InsertEnd(string content)
    {
        this.text = Sitem_33.InsertEnd(text, content);
    }
    public void InsertFront(string content)
    {
        this.text = Sitem_33.InsertFront(text, content);
    }
}