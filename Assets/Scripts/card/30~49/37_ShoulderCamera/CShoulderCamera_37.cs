using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Cinemachine;
using Newtonsoft.Json;

public class CShoulderCamera_37 : CardBase
{
    // 失去的damp： (0.3,0.25,0.3)
    public Vector3 lookPos;
    public CinemachineVirtualCamera camera;
    public GameObject follow;
    public float speedX = 4f;
    public float speedY = 2f;
    public float rotationX = 0f;
    public float rotationY = 0f;
    public float minY = -60f;
    public float maxY = 70f;

    public GameObject centerUI;

    [JsonIgnore]
    public int Priority
    {
        get { return camera.Priority; }
        set { if (camera.Priority != value) camera.Priority = value; }
    }
}
public class DShoulderCamera_37: DataBase
{
    public string centerUI;

    public GameObject centerUIObj;
    public override void Init(int id)
    {
        centerUIObj = DataManager.LoadResource<GameObject>(id, centerUI);
    }
}
public class MsgRotateView : MsgBase
{
    public float rotationX = 0f;
    public float rotationY = 0f;
    public MsgRotateView(float rotationX, float rotationY)
    {
        this.rotationX = rotationX;
        this.rotationY = rotationY;
    }
}
public class SShoulderCamera_37: SystemBase
{
    public static void RefreshView(CardBase card)
    {
        SendMsg(GetTop(card), SShoulderCamera_37.mTRefreshView, new MsgBase());
    }
    public static int mTRotateView = MsgType.ParseMsgType(GetTypeId(typeof(CShoulderCamera_37)), 0);
    public static int mTRefreshView = MsgType.ParseMsgType(GetTypeId(typeof(CShoulderCamera_37)), 1);
    public override void Init()
    {
        AddHandle(MsgType.FixedUpdate, UpdateFollow);
        AddHandle(MsgType.Update, Update);
        AddHandle(mTRefreshView, RefreshView);
        AddHandle(MsgType.GetLookPos, GetLookPos);
        AddHandle(MsgType.OnItem, OnCamera);
        AddHandle(MsgType.OnShowUI, OnShowUI);
        AddHandle(mTRotateView, RotateView);
    }
    void OnShowUI(CardBase _card, MsgBase _msg)
    {
        CShoulderCamera_37 card = _card as CShoulderCamera_37;
        MsgOnShowUI msg = _msg as MsgOnShowUI;
        DShoulderCamera_37 config = basicConfig as DShoulderCamera_37;

        if (msg.op == 1)
        {
            card.centerUI = UIBasic.GiveUI(config.centerUIObj);
        }
        else
        {
            if (card.centerUI != null)
                GameObject.Destroy(card.centerUI);
        }
    }
    void OnCamera(CardBase _card, MsgBase _msg)
    {
        CShoulderCamera_37 card = _card as CShoulderCamera_37;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op==1)
        {
            card.camera = CreateObj(id).GetComponent<CinemachineVirtualCamera>();
            card.camera.Priority = 0;
            card.follow = new GameObject("shoulder_follow");
            card.camera.Follow = card.follow.transform;
        }
        else
        {
            if (card.camera != null)
                GameObject.Destroy(card.camera.gameObject);
            if (card.follow != null)
                GameObject.Destroy(card.follow.gameObject);
        }
    }
    void GetLookPos(CardBase _card, MsgBase _msg)
    {
        CShoulderCamera_37 card = _card as CShoulderCamera_37;
        MsgGetLookPos msg = _msg as MsgGetLookPos;
        if (card.camera.Follow == null) return;

        msg.lookPos = card.lookPos;
        msg.lookDir = card.follow.transform.forward;
    }
    void RotateView(CardBase _card, MsgBase _msg)
    {
        CShoulderCamera_37 card = _card as CShoulderCamera_37;
        MsgRotateView msg = _msg as MsgRotateView;

        //更新虚拟物体旋转信息
        float x = msg.rotationX;
        float y = msg.rotationY;

        card.rotationX += x * card.speedX;
        card.rotationY += y * card.speedY;
        card.rotationX = ClampAngle(card.rotationX, float.MinValue, float.MaxValue);
        card.rotationY = ClampAngle(card.rotationY, card.minY, card.maxY);
        Update(card, null);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CShoulderCamera_37 card = _card as CShoulderCamera_37;

        //更新lookPos
        Vector3 screenPot = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenPot);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100, MyPhysics.ObsLayMask))
        {
            card.lookPos = raycastHit.point;
        }
        else card.lookPos = card.follow.transform.forward * 100 + card.follow.transform.position;
    }
    void UpdateFollow(CardBase _card, MsgBase _msg)
    {
        CShoulderCamera_37 card = _card as CShoulderCamera_37;
        // MsgUpdate msg = _msg as MsgUpdate;
        //更新虚拟物体的位置和旋转
        if (!TryGetCobj(card, out var cObj)) return;
        card.follow.transform.position = cObj.obj.Center;
        card.follow.transform.rotation = Quaternion.Euler(-card.rotationY, card.rotationX, 0);
        
    }
    void RefreshView(CardBase _card, MsgBase _msg)
    {
        CShoulderCamera_37 card = _card as CShoulderCamera_37;
        // MsgUpdate msg = _msg as MsgUpdate;

        if (!TryGetCobj(card, out var cObj)) return;
        card.follow.transform.forward = cObj.obj.transform.forward;
        Vector3 eularRotation = card.follow.transform.rotation.eulerAngles;
        // card.rotationY = -eularRotation.x; //y方向的视野不用更新
        card.rotationX = eularRotation.y;
        UpdateFollow(card, _msg);
        CinemachineVirtualCamera virtualCamera = card.camera;
        virtualCamera.UpdateCameraState(Vector3.up, Time.deltaTime);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    public static Vector3 GetLookPos(CardBase card)
    {
        if (!TryGetCobj(card, out var cObj)) return Vector3.zero;
        MsgGetLookPos msgl = new MsgGetLookPos();
        msgl.lookPos = cObj.obj.Center + cObj.obj.transform.forward;
        SendMsg(cObj, MsgType.GetLookPos, msgl);
        return msgl.lookPos;
    }
}