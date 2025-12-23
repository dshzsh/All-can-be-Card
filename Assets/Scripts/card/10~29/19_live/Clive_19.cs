using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static Slive_19;
using static SystemManager;

public class Clive_19 : CObj_2
{
    public float inertialSpeed = 15;
    public BasicAtt rotateSpeed = new(20);
    public Vector3 seeToDir = Vector3.zero;
    public Vector3 moveWantDir = Vector3.zero;

    public Vector3 mustMoveVelocity = Vector3.zero;
}
public class Dlive_19 : DataBase
{
    public float height = 1f;
    public float health = 1f;
    public float atk = 1f;
    public float def = 1f;
    public float speed = 1f;
    public float atkSpeed = 1f;

    public string dieAnimation;
    [JsonIgnore]
    public AnimationClip dieAni;
    public override void Init(int id)
    {
        base.Init(id);
        dieAni = DataManager.LoadResource<AnimationClip>(id, dieAnimation);
    }
}
public class Dmymagic : DataBase
{
    public class MagicWithRevise
    {
        public int id;
        public float cd = -1;
        public float cost = -1;
        public float pow = -1;
        public List<int> qhstones = new List<int>();
        public CardBase ParseMagic()
        {
            if (id == 0)
                return CreateCard(0);

            Cmagicbase_17 magic = CreateCard(DataManager.VidToPid(id, CardField.magic)) as Cmagicbase_17;
            if (cd != -1)
                magic.mdata.cd = cd;
            if (cost != -1)
                magic.mdata.cost = cost;
            if (pow != -1)
                magic.pow = pow;
            if (qhstones.Count > 0)
            {
                Cqhc_38 qhc = Sqhc_38.QhcWithSlot(qhstones.Count, qhstones.ToArray());
                AddComponent(magic, qhc);
            }
            return magic;
        }
    }
    public float manaMax;
    public List<MagicWithRevise> magicIds = new List<MagicWithRevise>();
}

public class Slive_19: SObj_2
{
    public static RuntimeAnimatorController aniCon;
    public static List<Clive_19> lives = new List<Clive_19>();
    public enum FindLiveMode
    {
        friend,
        enemy,
        any
    }
    public static class Team
    {
        public const int friend = 0;
        public const int enemy = 1;
        public const int other = 2;
    }
    public static bool TeamSatisfy(int teamA, int teamB, FindLiveMode mode)
    {
        switch(mode)
        {
            case FindLiveMode.enemy:
                return teamA != teamB;
            case FindLiveMode.friend:
                return teamA == teamB;
            case FindLiveMode.any:
                return true;
        }
        return false;
    }
    public static List<Clive_19> GetLives(CObj_2 live, FindLiveMode mode = FindLiveMode.enemy, float findLiveMaxDis = 100)
    {
        List<Clive_19> anses = new();
        foreach (Clive_19 olive in Slive_19.lives)
        {
            //Debug.Log(olive + " " + olive.team);
            if (!CardValid(olive)) continue;
            if (!TeamSatisfy(olive.team, live.team, mode)) continue;
            if (Slive_19.GetDistance(olive, live) > findLiveMaxDis) continue;
            
            anses.Add(olive);
        }
        return anses;
    }
    public static Clive_19 FindLive(Vector3 pos, int team, FindLiveMode mode = FindLiveMode.enemy, float findLiveMaxDis = 100)
    {
        Clive_19 mb = null;
        foreach (Clive_19 olive in Slive_19.lives)
        {
            //Debug.Log(olive + " " + olive.team);
            if (!CardValid(olive)) continue;
            if (!TeamSatisfy(olive.team, team, mode)) continue;
            if (mb == null) { mb = olive; continue; }
            if (Slive_19.GetDistance(mb, pos) > Slive_19.GetDistance(olive, pos))
            {
                mb = olive;
            }
        }
        if (mb == null) return null;
        if (Slive_19.GetDistance(mb, pos) > findLiveMaxDis) return null;
        return mb;
    }
    public static Clive_19 FindLive(CObj_2 live, FindLiveMode mode=FindLiveMode.enemy, float findLiveMaxDis=100)
    {
        Clive_19 mb = null;
        foreach (Clive_19 olive in Slive_19.lives)
        {
            //Debug.Log(olive + " " + olive.team);
            if (!CardValid(olive)) continue;
            if (!TeamSatisfy(olive.team, live.team, mode)) continue;
            if (mb == null) { mb = olive; continue; }
            if (Slive_19.GetDistance(mb, live) > Slive_19.GetDistance(olive, live))
            {
                mb = olive;
            }
        }
        if (mb == null) return null;
        if (Slive_19.GetDistance(mb, live) > findLiveMaxDis) return null;
        return mb;
    }
    public static float GetDistance(CObj_2 a, CObj_2 b)
    {
        return Vector3.Distance(a.obj.transform.position, b.obj.transform.position);
    }
    public static float GetDistance(CObj_2 a, Vector3 pos)
    {
        return Vector3.Distance(a.obj.transform.position, pos);
    }

    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnLive);
        AddHandle(MsgType.MoveControl, SetDir);
        AddHandle(MsgType.RotateControl, SetRotate);
        AddHandle(MsgType.Update, UpdateRotation);
        AddHandle(MsgType.FixedUpdate, UpdatePosition);
        AddHandle(MsgType.Update, UpdatePositionShow);
        aniCon = DataManager.LoadResource<RuntimeAnimatorController>(19, "basicAnimator");
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        Clive_19 card = _card as Clive_19;
        
        Dlive_19 configBase = DataManager.GetConfig<Dlive_19>(id);
        Dmymagic configMagic = DataManager.GetConfig<Dmymagic>(id);

        card.height = configBase.height;
        card.centerOffset = Vector3.up * configBase.height * 0.55f;

        Chealth_4 health = CreateCard<Chealth_4>(); 
        health.nowHealth = health.GetAtt(BasicAttID.healthMax).value = configBase.health;
        health.GetAtt(BasicAttID.atk).value = configBase.atk;
        health.GetAtt(BasicAttID.speed).value = configBase.speed;
        health.GetAtt(BasicAttID.def).value = configBase.def;
        BasicAtt batkSpeed = health.GetAtt(BasicAttID.atkSpeed);
        batkSpeed.value = 1; batkSpeed.finalMul = configBase.atkSpeed;
        health.GetAtt(BasicAttID.cdSpeed).value = 1f;
        AddComponent(card, health);
        card.myHealth = health;

        if (configMagic == null) return;

        Cmagic_14 mymagic = CreateCard<Cmagic_14>();
        card.myMagic = mymagic;
        health.GetAtt(BasicAttID.manaMax).value = health.nowMana = configMagic.manaMax;
        AddComponent(card, mymagic);
        for (int i = 0; i < configMagic.magicIds.Count; i++)
        {
            CardBase magic = configMagic.magicIds[i].ParseMagic();
            if (magic.id != 0)
                AddComponent(magic, CreateCard<CGQwfxx_15>());
            Smagic_14.ChangeHoldMax(mymagic, 1);
            SendMsg(mymagic, MsgType.SelfContainerGet,
                new MsgGetItem { pos = i, item = magic, op = 1 });
        }
    }
    void SetRotate(CardBase _card, MsgBase _msg)
    {
        MsgRotateControl msg = (MsgRotateControl)_msg;
        Clive_19 card = _card as Clive_19;

        if (!msg.interrupt && (card.obj.InLockMotion() || card.obj.InUpperMotion())) return;

        if (msg.seeTo != default)
        {
            Vector3 dir = msg.seeTo - card.obj.transform.position; dir.y = 0;
            card.seeToDir = dir.normalized;
        }

    }
    void SetDir(CardBase _card, MsgBase _msg)
    {
        MsgMoveControl msg = (MsgMoveControl)_msg;
        Clive_19 card = _card as Clive_19;

        //if (cardAbandon.obj.InLockMotion()) return;

        msg.dir.y = 0;
        msg.dir = msg.dir.normalized;

        if (msg.islocal)//根据当前视角，需要对dir作旋转
        {
            MsgGetLookPos msgl = new MsgGetLookPos();
            SendMsg(card, MsgType.GetLookPos, msgl);
            Vector3 lookDir = msgl.lookDir;
            lookDir.y = 0; lookDir = lookDir.normalized;//去除y方向的向量
            //Debug.Log(lookDir);
            msg.dir = Quaternion.LookRotation(lookDir, Vector3.up) * msg.dir;
        }

        //cardAbandon.obj.rbody.AddForce(msg.dir * cardAbandon.speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        card.moveWantDir = msg.dir;
        //cardAbandon.obj.rbody.velocity = msg.dir*4 + new Vector3(0, cardAbandon.obj.rbody.velocity.y, 0);        
    }
    public void UpdateRotation(CardBase _card, MsgBase _msg)
    {
        MsgUpdate msg = (MsgUpdate)_msg;
        Clive_19 card = _card as Clive_19;

        if (card.seeToDir.magnitude >= 0.01f && Vector3.Distance(card.seeToDir, card.obj.transform.forward) >= 0.01f)
        {
            //Debug.Log("forward:" + cardAbandon.obj.transform.forward + " seeTo:" + cardAbandon.seeToDir);
            float rotateSpeed = card.rotateSpeed.GetValue();
            if (rotateSpeed < 0) rotateSpeed = 0;
            card.obj.transform.forward = Vector3.RotateTowards(card.obj.transform.forward, card.seeToDir, msg.time * rotateSpeed, 0);
        }
        else card.seeToDir = Vector3.zero;//置为空

    }
    public void UpdatePositionShow(CardBase _card, MsgBase _msg)
    {
        MsgUpdate msg = (MsgUpdate)_msg;
        Clive_19 card = _card as Clive_19;

        Vector3 dir = card.moveWantDir;
        if (card.obj.InLockMotion()) dir = Vector3.zero;

        if (dir.magnitude <= 0.01f)
        {
            card.obj.SetMoveSpeed(0);
            return;
        }

        if (dir.magnitude >= 0.01f)//转动向目标方向
        {
            SetRotate(card, new MsgRotateControl() { seeTo = card.moveWantDir + card.obj.transform.position, interrupt = false });
        }

        //动画做出反应
        if (card.obj.animator != null && !card.obj.InLockMotion())
        {
            //更正移动的speed
            card.obj.SetMoveSpeed(Shealth_4.GetAttf(card, BasicAttID.speed));
            //更正移动的direction
            //0~1对应F-R-B-L-F
            Vector3 moveDir = dir;
            Vector3 seeDir = card.obj.transform.forward;

            Vector2 v1XZ = new Vector2(moveDir.x, moveDir.z);
            Vector2 v2XZ = new Vector2(seeDir.x, seeDir.z);

            // 计算 v1 和 v2 之间的角度
            float angle = Vector2.Angle(v1XZ, v2XZ);

            // 判断旋转方向（顺时针或逆时针）
            float cross = v1XZ.x * v2XZ.y - v1XZ.y * v2XZ.x;

            // 如果 cross > 0，表示逆时针；如果 cross < 0，表示顺时针
            if (cross < 0)
            {
                angle = 360 - angle; // 顺时针旋转
            }
            card.obj.SetDirection(angle / 360);
        }

        //cardAbandon.moveWantDir = Vector3.zero;
    }
    public void UpdatePosition(CardBase _card, MsgBase _msg)
    {
        MsgUpdate msg = (MsgUpdate)_msg;
        Clive_19 card = _card as Clive_19;

        if (card.obj.transform.position.y <= -60)
        {
            card.obj.transform.position = Vector3.up * 60;
        }

        if (card.obj.rbody == null) return;
        
        Vector3 dir = card.moveWantDir;
        if (card.obj.InLockMotion()) dir = Vector3.zero;// 在施法动作时想要静止

        float speed = Shealth_4.GetAttf(card, BasicAttID.speed);
        if (speed < 0) speed = 0;

        //如果速度的偏移过大，不能马上设置成0
        Vector3 xzVel = card.obj.rbody.velocity; xzVel.y = 0;
        const float minSpeedAsInertial = 1;
        xzVel = Vector3.MoveTowards(xzVel, dir * speed, 
            Mathf.Max(minSpeedAsInertial, speed) * card.inertialSpeed * msg.time);

        Vector3 movedVelocity = new Vector3(0, card.obj.rbody.velocity.y, 0) + xzVel;
        const float mustMoveInertialSpeed = 20;
        card.obj.rbody.velocity = Vector3.MoveTowards(movedVelocity, card.mustMoveVelocity,
            card.mustMoveVelocity.magnitude * mustMoveInertialSpeed * msg.time);
    }
    void OnLive(CardBase _card, MsgBase _msg)
    {
        Clive_19 card = _card as Clive_19;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op==1)
        {
            lives.Add(card);
        }
        else
        {
            lives.Remove(card);
        }
    }
}