using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using static SfloorBase_2;
using Newtonsoft.Json;
using System;

public class CfloorBase_2 : CObj_2
{
    public GameObject metaGround;

    public Dictionary<int, List<SumInfo>> enemySumInfos = new();
    public Dictionary<FloorEnemySumPos.PosType, List<SumInfo>> sumInfos = new();
}
public class DFloor : DataBase
{
    public int fromEnv;
    public string typeStr;
    public bool canMeet;

    [JsonIgnore]
    public FloorType type;

    public static Dictionary<int, int> difficultyToEnv = new();
    public static Dictionary<int, Dictionary<FloorType, List<int>>> envToTypeToFloors = new();


    public override void Init(int id)
    {
        type = Enum.Parse<FloorType>(typeStr);
        fromEnv = DataManager.VidToPid(fromEnv, CardField.env);
        if(canMeet)
        {
            Dictionary<FloorType, List<int> > typeToFloors = envToTypeToFloors.GetValueOrDefault(fromEnv, new());
            envToTypeToFloors[fromEnv] = typeToFloors;
            List<int> list = typeToFloors.GetValueOrDefault(type, new());
            typeToFloors[type] = list;
            list.Add(id);
        }
    }
    public static int GetRandFloor(int env, FloorType type, bool useMapRand = false)
    {
        Dictionary<FloorType, List<int>> typeToFloors = envToTypeToFloors.GetValueOrDefault(env, new());
        List<int> list = typeToFloors.GetValueOrDefault(type, new());
        if (list.Count == 0) return GetRandFloor(DataManager.VidToPid(0, CardField.env), type);
        return MyRandom.RandomInList(list);
    }
}
public class SfloorBase_2: SObj_2
{
    public enum FloorType
    {
        fight,
        bossFight,
        treasureGood,
        start,
        thing,
        tresureMagic,
        wzd,
        shop,
        forge
    }
    public class FloorTypeShow
    {
        public int showID;// 显示的图标名称的name
        public Color color;
        public FloorTypeShow(int showID, Color color)
        {
            this.showID = showID;
            this.color = color;
        }
    }
    public static Dictionary<FloorType, FloorTypeShow> floorTypeShow = new();
    public static FloorTypeShow GetFloorShow(int id)
    {
        DFloor config = DataManager.GetConfig<DFloor>(id);
        if (config == null) return new FloorTypeShow(0, Color.white);
        return GetFloorShow(DataManager.GetConfig<DFloor>(id).type);
    }
    public static FloorTypeShow GetFloorShow(FloorType type)
    {
        if (!floorTypeShow.ContainsKey(type))
            return new FloorTypeShow(0, Color.white);
        return floorTypeShow[type];
    }
    public static void AddFloorShow(FloorType type, FloorTypeShow show)
    {
        floorTypeShow.Add(type, show);
    }

    public class MsgFloorStart :MsgBase
    {
        public CObj_2 live;
        public MsgFloorStart(CObj_2 player)
        {
            this.live = player;
        }
    }
    public static int mTFloorStart = MsgType.ParseMsgType(GetTypeId(typeof(CfloorBase_2)), 0);

    public static SumInfo ConvertSumInfo(CfloorBase_2 card, SumInfo sumInfo)
    {
        SumInfo ans = new SumInfo();
        ans.pos = card.obj.transform.TransformPoint(sumInfo.pos);
        ans.rotation = sumInfo.rotation * card.obj.transform.rotation;
        return ans;
    }
    protected static SumInfo GetEnemyPos(CfloorBase_2 card, int turn = 0)
    {
        SumInfo sumInfo = MyRandom.RandomInList(card.enemySumInfos[turn]);
        SumInfo ans = new SumInfo();
        ans.pos = card.obj.transform.TransformPoint(sumInfo.pos);
        ans.rotation = sumInfo.rotation * card.obj.transform.rotation;
        return ans;
    }
    protected static SumInfo GetPlayerPos(CfloorBase_2 card)
    {
        SumInfo sumInfo = MyRandom.RandomInList(card.sumInfos[FloorEnemySumPos.PosType.player]);
        SumInfo ans = new SumInfo();
        ans.pos = card.obj.transform.TransformPoint(sumInfo.pos);
        ans.rotation = sumInfo.rotation * card.obj.transform.rotation;
        return ans;
    }
    protected static SumInfo GetEndPos(CfloorBase_2 card)
    {
        SumInfo sumInfo = MyRandom.RandomInList(card.sumInfos[FloorEnemySumPos.PosType.end]);
        SumInfo ans = new SumInfo();
        ans.pos = card.obj.transform.TransformPoint(sumInfo.pos);
        ans.rotation = sumInfo.rotation * card.obj.transform.rotation;
        return ans;
    }
    public static void GiveNextCsm(CfloorBase_2 card)
    {
        SumInfo sumInfo = GetEndPos(card);
        SCmap_45.GiveNextFloorCsm(sumInfo.pos, sumInfo.rotation);
    }
    public class SumInfo
    {
        public Vector3 pos;
        public Quaternion rotation;
        public FloorEnemySumPos.PosType type;

        public void UseTo(Transform transform)
        {
            transform.position = pos;
            transform.rotation = rotation;
        }
    }
    public override void Init()
    {
        base.Init();
        AddHandle(mTFloorStart, FloorStartTpPlayer);
        AddHandle(MsgType.OnItem, ParsePosAndMat, HandlerPriority.Before);
    }
    void FloorStartTpPlayer(CardBase _card, MsgBase _msg)
    {
        CfloorBase_2 card = _card as CfloorBase_2;
        MsgFloorStart msg = _msg as MsgFloorStart;

        SumInfo sumInfo = GetPlayerPos(card);
        msg.live.obj.transform.SetPositionAndRotation(sumInfo.pos, sumInfo.rotation);
        SShoulderCamera_37.RefreshView(msg.live);
    }
    void ParsePosAndMat(CardBase _card, MsgBase _msg)
    {
        CfloorBase_2 card = _card as CfloorBase_2;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op==1)
        {
            // 处理预定义的位置
            foreach(var posObj in card.obj.gameObject.GetComponentsInChildren<FloorEnemySumPos>())
            {
                SumInfo sumInfo = new SumInfo()
                {
                    pos = card.obj.transform.InverseTransformPoint(posObj.transform.position),
                    rotation = Quaternion.Inverse(card.obj.transform.rotation) * posObj.transform.rotation,
                    type = posObj.type
                };
                if(posObj.type==FloorEnemySumPos.PosType.enemy)
                {
                    List<SumInfo> sumInfos = card.enemySumInfos.GetValueOrDefault(posObj.turn, new List<SumInfo>());
                    sumInfos.Add(sumInfo);
                    card.enemySumInfos[posObj.turn] = sumInfos;
                }
                else
                {
                    List<SumInfo> sumInfos = card.sumInfos.GetValueOrDefault(posObj.type, new List<SumInfo>());
                    sumInfos.Add(sumInfo);
                    card.sumInfos[posObj.type] = sumInfos;
                }

                GameObject.Destroy(posObj.gameObject);
            }

            // 处理预定义的材质，根据当前环境决定
            DLEenv denv = SCmap_45.GetEnvMat();
            foreach (var wallObj in card.obj.gameObject.GetComponentsInChildren<MLEwall>())
            {
                if (wallObj.isWall)
                    wallObj.GetComponent<MeshRenderer>().material = denv.wallMat;
                else if(wallObj.isGround)
                    wallObj.GetComponent<MeshRenderer>().material = denv.groundMat;

                if (wallObj.isMetaGround)
                    card.metaGround = wallObj.gameObject;
            }

            SCmap_45.nowFloor = card;
        }
    }
    public override Color GetColor(CardBase _card)
    {
        return SfloorBase_2.GetFloorShow(_card.id).color;
    }
}