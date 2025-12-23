using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;
using static Scoldes_6;

public class Cbullet_10 : CObj_2
{
    public float baseSpeed;
    public Vector3 baseScale;

    public float bulletPow;

    public Ctimedes_7 timeDes;
    public Ccoldam_5 colDam;

    public GameObject move;
    public GameObject hit;
}
public class Dbullet_10 : DataBase
{
    public Scoldes_6.ColDesType bulletCol = Scoldes_6.ColDesType.Enemy;
    public float bulletTime = 0;
    public float bulletSpeed = 0;
    public string move = "move";
    public string start = "start";
    public string hit = "hit";

    [JsonIgnore]
    public GameObject moveObj;
    [JsonIgnore]
    public GameObject startObj;
    [JsonIgnore]
    public GameObject hitObj;
    public override void Init(int id)
    {
        moveObj = DataManager.LoadResource<GameObject>(id, move);
        startObj = DataManager.LoadResource<GameObject>(id, start);
        hitObj = DataManager.LoadResource<GameObject>(id, hit);
    }
}
public class MsgBullet : MsgBaseWithTag, ICloneAble
{
    public int id = 0;
    public CardBase from;
    public MsgMagicUse fromMagicUse;
    public float bulletPow = 1f;
    public Vector3 initPos = Vector3.zero;
    public float damage = 0;
    public Vector3 dir = Vector3.zero;
    public int team = 0;
    public BasicAtt speed = new BasicAtt();
    public bool isMelee = false;//是否为近战
    public Vector3 exScale = Vector3.one;
    public Quaternion exRotate = Quaternion.identity;
    public float baseScale = 1;
    public float time = -1;

    public List<CardBase> addCards = new List<CardBase>();

    public void AddCard(CardBase card)
    {
        addCards.Add(card);
    }
    public void MulPow(float pow)
    {
        bulletPow *= pow;
    }
    public void AddExScale(float add)
    {
        exScale += add * Vector3.one;
    }
    public void AddRotate(Quaternion rotate)
    {
        exRotate *= rotate;
    }
    public void AddInitShift(CardBase card)
    {
        float size = 1f;
        if(TryGetCobj(card, out var cobj))
        {
            size = cobj.obj.transform.localScale.x;
        }
        initPos += 0.5f * size * dir;
    }
    public MsgBullet(MsgBullet other)
    {
        this.from = other.from;
        this.fromMagicUse = other.fromMagicUse;
        this.bulletPow = other.bulletPow;
        this.initPos = other.initPos;
        this.damage = other.damage;
        this.dir = other.dir;
        this.team = other.team;
        this.speed = other.speed;
        this.isMelee = other.isMelee;
        this.exScale = other.exScale;
        this.exRotate = other.exRotate;
        this.baseScale = other.baseScale;
        this.time = other.time;
        this.addCards = other.addCards;
    }
    object ICloneAble.Clone()
    {
        return new MsgBullet(this);
    }
    public void AddWx(int wxTag)
    {
        CFdamType_19 buff = CreateCard<CFdamType_19>();
        buff.wxTag = wxTag;
        AddCard(buff);
    }
    public MsgBullet(MsgMagicUse useMsg, bool dirNoY = false)
    {
        from = useMsg.live;
        fromMagicUse = useMsg;
        bulletPow = useMsg.pow;
        if (TryGetCobj(useMsg.live, out var obj))
        {
            dir = (useMsg.pos - obj.obj.Center).normalized;
            if(dirNoY)
                dir = MyMath.V3zeroYNor(dir);
            initPos = obj.obj.Center;
            team = obj.team;
        }

        if (useMsg.mdata.wxTag != 0)
        {
            AddWx(useMsg.mdata.wxTag);
        }

        if (useMsg.mks != null && useMsg.mks.TryGetValue(MsgMagicUse.AddCardType.bullet, out var add))
            addCards.AddRange(add);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fatherBullet"></param>
    /// <param name="addEx">是否复制额外的效果</param>
    /// <param name="addNoAddEx">对于通过额外效果衍生，不复制衍生来源的效果（比如超级溅射不会触发任何的超级溅射）</param>
    public MsgBullet(Cbullet_10 fatherBullet, bool addEx = true, int noAddId = 0)
    {
        from = Sysw_26.GetYsw(fatherBullet);
        fromMagicUse = null;
        bulletPow = fatherBullet.bulletPow;
        dir = fatherBullet.obj.transform.forward;
        team = fatherBullet.team;
        initPos = fatherBullet.obj.transform.position;
        exScale = MyMath.Vector3Div(fatherBullet.obj.transform.localScale, fatherBullet.baseScale);
        
        if (fatherBullet.colDam != null)
            damage = fatherBullet.colDam.damage;

        if (fatherBullet.timeDes != null)
            time = fatherBullet.timeDes.timeRes;

        if(addEx)
        {
            if (fatherBullet.myBuff != null)
            {
                foreach (var buffInfo in fatherBullet.myBuff.buffInfos)
                {
                    if (buffInfo.itemFrom != Sbullet_10.fromBulletBuff)
                        continue;
                    if (buffInfo.time <= 0) // 正在移除的一些效果不被添加
                        continue;
                    if (buffInfo.buff.id == noAddId)
                        continue;
                    
                    addCards.Add(NewCopy(buffInfo.buff));
                }
            }
        }
    }
}
public class MsgBulletCreate :MsgBase
{
    public MsgBullet msg;
    public CObj_2 bullet;

    public CardBase GetFromMagic()
    {
        return msg?.fromMagicUse?.magic;
    }
}
public class Sbullet_10 : SObj_2
{
    public static int fromBulletBuff;
    public static void GiveBullet(int id, MsgBullet msgbu, float lateTime)
    {
        if (lateTime <= MyTool.SmallFloat)
        {
            GiveBullet(id, msgbu);
            return;
        }
        World.world.RunCoroutine(DelayedFunction(id, msgbu, lateTime));
    }
    static IEnumerator DelayedFunction(int id, MsgBullet msgbu, float lateTime)
    {
        yield return new WaitForSeconds(lateTime);
        GiveBullet(id, msgbu);
    }
    
    public static void DieBullet(CardBase bullet)
    {
        SendMsg(bullet, MsgType.TrueDie, new MsgDie());
    }
    public static void GiveBullet(MsgBullet bmsg)
    {
        GiveBullet(bmsg.id, bmsg);
    }
    public static void GiveBullet(int id, MsgBullet msgbu)
    {
        msgbu.id = id;
        CObj_2 bullet = CreateCard(id) as CObj_2;
        AddToWorld(bullet);

        GiveBullet(bullet, msgbu);
    }
    /// <summary>
    /// 要求传入的bullet已经被AddToWorld
    /// </summary>
    /// <param name="bullet"></param>
    /// <param name="msgbu"></param>
    public static void GiveBullet(CObj_2 bullet, MsgBullet msgbu)
    {
        int id = bullet.id;

        if (bullet is Cbullet_10 _cbullet) // 配置的速度转到msg里去
            msgbu.speed.value = _cbullet.baseSpeed;

        SendMsg(msgbu.from, MsgType.CreateBullet, new MsgBulletCreate { bullet = bullet, msg = msgbu });

        Sbullet_10.Set(bullet, msgbu);

        // 产生发射特效
        if (bullet is Cbullet_10 cbullet)
        {
            Dbullet_10 bdata = DataManager.GetConfig<Dbullet_10>(id);
            if (bdata.moveObj != null)
                cbullet.move = GameObject.Instantiate(bdata.moveObj, bullet.obj.transform);

            if (bdata.hitObj != null)
            {
                cbullet.hit = GameObject.Instantiate(bdata.hitObj);
                cbullet.hit.gameObject.SetActive(false);
            }
        }

        SendMsg(bullet, MsgType.BulletStart, new MsgBulletCreate { bullet = bullet, msg = msgbu });

        if (bullet is Cbullet_10 ccbullet)
        {
            if (CardValid(ccbullet))
            {
                Dbullet_10 bdata = DataManager.GetConfig<Dbullet_10>(id);
                if (bdata.startObj != null)
                {
                    GameObject startTx = GameObject.Instantiate(bdata.startObj, bullet.obj.transform.position, bullet.obj.transform.rotation);
                    startTx.transform.localScale = MyTool.Vector3Multiply(startTx.transform.localScale, bullet.obj.transform.localScale);
                }
            }
        }
    }
    public static void Set(CObj_2 card, MsgBullet msg)
    {
        {
            card.obj.transform.position = msg.initPos;
            //Debug.Log(msg.dir);
            card.obj.transform.rotation = Quaternion.LookRotation(msg.dir, Vector3.up);
        }

        card.obj.transform.localScale = MyTool.Vector3Multiply(msg.exScale, card.obj.transform.localScale) * msg.baseScale;
        card.obj.transform.localRotation *= msg.exRotate;

        //Debug.Log(cardAbandon.obj.transform.rotation);
        if (card.obj.rbody != null)
            card.obj.rbody.velocity = msg.dir * msg.speed.GetValue();

        card.team = msg.team;
        if (card is Cbullet_10 cbullet)
        {
            cbullet.bulletPow = msg.bulletPow;
        }

        if (msg.damage != 0)
        {
            Ccoldam_5 ccoldam = CreateCard<Ccoldam_5>(); ccoldam.damage = msg.damage;
            if (card is Cbullet_10 ccbullet)
            {
                ccbullet.colDam = ccoldam;
                ActiveComponent(card, ccoldam);
            }
            else AddComponent(card, ccoldam);
        }
        if (msg.time > 0)
        {
            if (card is Cbullet_10 ccbullet)
            {
                ccbullet.timeDes.SetTime(msg.time);
            }
        }
        
        if(msg.from!=null)
        {
            Cysw_26 cysw = CreateCard<Cysw_26>();cysw.from = msg.from;
            AddComponent(card, cysw);
        }
        if(msg.addCards.Count > 0)
        {
            foreach (CardBase addcard in msg.addCards)
            {
                Sbuff_35.GiveBuff(null, card, new MsgBeBuff() { buff = NewCopy(addcard), itemFrom = fromBulletBuff, time = Sbuff_35.BuffSpeTime.Inf });
            }
        }
    }
    public static float GetBulletPow(CardBase card)
    {
        if (TryGetCobj(card, out var cobj) && cobj is Cbullet_10 cbullet)
            return cbullet.bulletPow;
        return 1f;
    }
    public static float GetBulletSize(CardBase card)
    {
        if (!TryGetCobj(card, out var cobj))
            return 1;
        float baseSize = cobj.obj.transform.localScale.x;
        if (cobj.obj.TryGetComponent<BoxCollider>(out var comB))
            return baseSize * comB.size.x;
        if (cobj.obj.TryGetComponent<SphereCollider>(out var comS))
            return baseSize * comS.radius * 2;
        return baseSize;
    }
    
    
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        Cbullet_10 card = _card as Cbullet_10;
        Dbullet_10 config = DataManager.GetConfig<Dbullet_10>(id);
        card.baseSpeed = config.bulletSpeed;

        card.baseScale = DataManager.GetConfig<DObj_2>(id).gameObject.transform.localScale;

        Cdiedes_27 cdiedes_27 = CreateCard<Cdiedes_27>();
        AddComponent(card, cdiedes_27);
        if (config.bulletCol != ColDesType.None)
        {
            Ccoldes_6 ccoldes_6 = CreateCard<Ccoldes_6>(); ccoldes_6.type = config.bulletCol;
            AddComponent(card, ccoldes_6);
        }

        {
            Ctimedes_7 ctimedes = CreateCard<Ctimedes_7>(); ctimedes.timeRes = config.bulletTime;
            card.timeDes = ctimedes;

            ActiveComponent(card, ctimedes);
        }
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Collision, CollisionHit, HandlerPriority.After);
        AddHandle(MsgType.OnItem, OnItem);
        fromBulletBuff = id;
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        Cbullet_10 card = _card as Cbullet_10;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op == 1)
        {
            
        }
        else
        {
            if (card.move != null)//让粒子特效完全消散后再消毁粒子
            {
                card.move.transform.SetParent(null);

                foreach (ParticleSystem particleSystem in card.move.gameObject.GetComponentsInChildren<ParticleSystem>())
                {
                    particleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                    //GameObject.Destroy(cardAbandon.move.gameObject, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
                }
            }
            if (card.hit != null)
            {
                GameObject.Destroy(card.hit.gameObject);
            }
        }
    }
    public void CollisionHit(CardBase _card, MsgBase _msg)
    {
        Cbullet_10 card = _card as Cbullet_10;
        MsgCollision msg = _msg as MsgCollision;

        if (msg.hit && card.hit != null)
        {
            GameObject.Instantiate(card.hit, msg.hitPos, card.obj.transform.rotation)
                .SetActive(true);
        }
    }
}