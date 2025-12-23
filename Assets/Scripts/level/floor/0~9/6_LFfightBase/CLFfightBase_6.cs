using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using static DLFfight;
using Random = UnityEngine.Random;

public class CLFfightBase_6 : CfloorBase_2
{
    public List<FightTurn> turnsToSum = new();
    public List<Clive_19> enemys = new();
    public bool fightEnd = false;
    public bool isBoss = false;

    public float time;
}
public class DLFfightBase_6 : DataBase
{
    public float warningTime;

    public static float warnTime;
    public override void Init(int id)
    {
        base.Init(id);
        warnTime = warningTime;
    }
}
public class DLFbaseWall : DataBase
{
    public string wall;

    public static GameObject wallPrefab;
    public override void Init(int id)
    {
        wallPrefab = DataManager.LoadResource<GameObject>(id, wall);
    }
}
public class DLFfight : DataBase
{
    public class FightTurn
    {
        public int index;
        public float time;
        public List<int> enemyID = new();
        public List<int> enemyCnt = new();
        public FightTurn() { }
    }
    public List<FightTurn> fightTurns;
}

public class SLFfightBase_6: SfloorBase_2
{
    public static int mTFightEnd = MsgType.ParseMsgType(CardField.floor, 6, 0);
    public static int mTFightStart = MsgType.ParseMsgType(CardField.floor, 6, 1);
    public static int mTSummonEnemy = MsgType.ParseMsgType(CardField.floor, 6, 2);
    public class MsgSummonEnemy:MsgBase
    {
        public Clive_19 enemy;
        public MsgSummonEnemy(Clive_19 enemy)
        {
            this.enemy = enemy;
        }
    }
    static int fightCnt = 0;
    public static bool InFight()
    {
        return fightCnt > 0;
    }

    public static Clive_19 GiveEnemyCsm(CfloorBase_2 card, int enemyID, int turn = 0)
    {
        SumInfo sumInfo = GetEnemyPos(card, turn);
        Clive_19 live = CreateCard(enemyID) as Clive_19;
        CMOwarnCsm_6 csm = CreateCard<CMOwarnCsm_6>();
        csm.liveToSummon = live;
        csm.warnTime = DLFfightBase_6.warnTime;
        AddToWorld(csm);
        csm.obj.transform.SetPositionAndRotation(sumInfo.pos, sumInfo.rotation);

        return live;
    }
    public virtual void GiveFightRewardBoss(Vector3 basePos)
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 3 + basePos; pos.y = pos.y / 3 + 1.5f;

            SMOcoin_3.GiveCoin(pos, Random.rotation, Random.Range(6, 9 + 1));
        }
        // 给一个魔法箱子，且至少为2级
        CNmagicTreasure_2 treaMagic = CreateCard<CNmagicTreasure_2>();
        treaMagic.rare = Mathf.Max(2, SNtreasure_1.SummonRare());
        AddToWorld(treaMagic);
        treaMagic.obj.transform.position = basePos + Vector3.up;

        // 给一个增加魔法槽的boss道具
        CNtreasure_1 trea = CreateCard<CNtreasure_1>(); trea.rare = 4;
        trea.tags.Clear(); trea.tags.Add(MyTag.CardTag.PbossGood);
        AddToWorld(trea);
        trea.obj.transform.position = basePos + new Vector3(-2, 1, 0);
    }
    public virtual void GiveFightReward(Vector3 basePos)
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 pos = Random.insideUnitSphere * 3 + basePos; pos.y = pos.y / 3 + 1.5f;

            SMOcoin_3.GiveCoin(pos, Random.rotation, Random.Range(3, 7 + 1));
        }
        // 给一个魔法箱子
        CNmagicTreasure_2 treaMagic = CreateCard<CNmagicTreasure_2>();
        AddToWorld(treaMagic);
        treaMagic.obj.transform.position = basePos + Vector3.up;

        // 给一个道具箱子
        CNtreasure_1 trea = CreateCard<CNtreasure_1>(); trea.rare = 4;
        AddToWorld(trea);
        trea.obj.transform.position = basePos + new Vector3(-2, 1, 0);
    }
    public void FightEnd(CLFfightBase_6 card)
    {
        fightCnt--;
        GiveNextCsm(card);

        if (card.isBoss)
            GiveFightRewardBoss(card.obj.transform.position);
        else
            GiveFightReward(card.obj.transform.position);

        SendMsgToPlayer(mTFightEnd, new MsgBase());
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CLFfightBase_6 card = _card as CLFfightBase_6;
        DFloor dFloor = DataManager.GetConfig<DFloor>(id);

        if (dFloor.type == FloorType.bossFight)
            card.isBoss = true;
    }
    public override void Init()
    {
        base.Init();
        AddHandle(mTFloorStart, FloorStart);
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.OnItem, ParseWall, HandlerPriority.After);
    }
    void GenerateWall(CLFfightBase_6 card, Material mat, int index, Vector3 position, Vector3 scale)
    {
        // 创建墙体
        GameObject wall = GameObject.Instantiate(DLFbaseWall.wallPrefab);
        wall.transform.SetParent(card.obj.transform);

        // 设置位置和缩放
        wall.transform.localPosition = position;
        wall.transform.localScale = scale;

        // 设置材质
        if (wall.TryGetComponent<Renderer>(out var wallRenderer))
        {
            wallRenderer.material = mat;
        }

        // 可选：为墙体命名以便区分
        wall.name = "Wall_" + index;
    }
    void ParseWall(CardBase _card, MsgBase _msg)
    {
        CLFfightBase_6 card = _card as CLFfightBase_6;
        MsgOnItem msg = _msg as MsgOnItem;

        if (msg.op != 1) return;

        float sizeX = card.metaGround.transform.localScale.x;
        float sizeZ = card.metaGround.transform.localScale.z;
        float high = 2f;

        Material wallMat = SCmap_45.GetEnvMat().wallMat;

        // 生成四面包围的墙体
        GenerateWall(card, wallMat, 0, new Vector3(sizeX / 2 - 0.5f, high / 2, 0), new Vector3(1, high, sizeZ)); // 前墙
        GenerateWall(card, wallMat, 1, new Vector3(-sizeX / 2 + 0.5f, high / 2, 0), new Vector3(1, high, sizeZ)); // 后墙
        GenerateWall(card, wallMat, 2, new Vector3(0, high / 2, sizeZ / 2 - 0.5f), new Vector3(sizeX, high, 1)); // 右墙
        GenerateWall(card, wallMat, 3, new Vector3(0, high / 2, -sizeZ / 2 + 0.5f), new Vector3(sizeX, high, 1)); // 左墙
    }
    void FloorStart(CardBase _card, MsgBase _msg)
    {
        CLFfightBase_6 card = _card as CLFfightBase_6;
        DLFfight config = DataManager.GetConfig<DLFfight>(id);
        foreach(FightTurn configTurn in config.fightTurns)
        {
            FightTurn fightTurn = new FightTurn();
            fightTurn.index = configTurn.index;
            fightTurn.time = configTurn.time;
            for (int i = 0; i < configTurn.enemyID.Count; i++)
                fightTurn.enemyID.Add(DataManager.VidToPid(configTurn.enemyID[i], CardField.enemy));
            for (int i = 0; i < configTurn.enemyCnt.Count; i++)
                fightTurn.enemyCnt.Add(configTurn.enemyCnt[i]);
            card.turnsToSum.Add(fightTurn);
        }
        card.fightEnd = false;
        fightCnt++;
        SendMsgToPlayer(mTFightStart, new MsgBase());
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CLFfightBase_6 card = _card as CLFfightBase_6;
        MsgUpdate msg = _msg as MsgUpdate;

        // 已经战斗结束，不再继续操作
        if (card.fightEnd) return;

        card.time += msg.time;

        if(card.turnsToSum.Count > 0) // 还有没有生成的怪，不会结束战斗
        {
            if (card.time >= card.turnsToSum[0].time)
            {
                FightTurn fightTurn = card.turnsToSum[0];
                for(int i = 0; i < fightTurn.enemyID.Count;i++)
                {
                    for (int j = 0; j < fightTurn.enemyCnt[i]; j++)
                        card.enemys.Add(GiveEnemyCsm(card, fightTurn.enemyID[i], fightTurn.index));
                }
                card.turnsToSum.RemoveAt(0);
            }
        }
        else // 所有的怪已经生成，只需要看有没有存活的怪
        {
            for(int i=0;i<card.enemys.Count;i++)
            {
                if (!CardValid(card.enemys[i]))
                {
                    card.enemys.RemoveAt(i);
                    i--;
                }
            }
            if(card.enemys.Count==0)
            {
                FightEnd(card);
                card.fightEnd = true;
            }
        }
    }
}