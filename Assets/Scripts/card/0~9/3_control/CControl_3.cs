using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public interface IEscClose
{
    void Close();
}
public class CControl_3 : CardBase
{
    public int cursorLockedCnt = 0;
    public float interactDis = 3f;
}
public class MsgMoveControl : MsgBase
{
    public bool islocal = false;
    public Vector3 dir;
}
public static class ConKeys
{
    public static KeyCode OpenBag = KeyCode.B;
    public static KeyCode[] UseMagics = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Q, KeyCode.E, KeyCode.Space };
    public static KeyCode InteractKey = KeyCode.F;
    public static KeyCode ShowMap = KeyCode.M;
}
public class SControl_3 : SystemBase
{
    private static List<IEscClose> escCloses = new List<IEscClose>();
    public static void AddEscClose(IEscClose escClose)
    {
        escCloses.Add(escClose);
    }
    public static void RemoveEscClose(IEscClose escClose)
    {
        escCloses.Remove(escClose);
    }
    static int lockMouseCnt = 0;
    public static void LockMouse(int op)
    {
        lockMouseCnt += op;

        // 锁定鼠标逻辑，默认隐藏鼠标，在不控制的时候显示鼠标
        if (lockMouseCnt == 1)
        {
            Cursor.lockState = CursorLockMode.Locked;  // 锁定鼠标
            Cursor.visible = false;  // 隐藏鼠标
        }
        else if (lockMouseCnt == 0)
        {
            Cursor.lockState = CursorLockMode.None;  // 解除锁定
            Cursor.visible = true;  // 显示鼠标
        }
    }
    
    static int noControl = 0;
    public static void AddNoControl(int op)
    {
        noControl += op;

        // 这个时候时停最合适了
        Stime_41.PauseTime(op);

        // 越[不操作]，越不[锁定鼠标]
        LockMouse(-op);
    }
    public static bool InControl()
    {
        return noControl <= 0;
    }
    public override void Init()
    {
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.Update, Update);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CControl_3 card = _card as CControl_3;
        MsgOnItem msg = _msg as MsgOnItem;

        LockMouse(msg.op);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CControl_3 card = _card as CControl_3;
        MsgUpdate msg = _msg as MsgUpdate;

        OpenBagControl(msg.cobj, card);
        ShowMap(card);

        if (InControl())
        {
            MoveControl(msg.cobj, card);
            InteractControl(card);
            RotateView(msg.cobj, card);
        }
        EscClose();
    }
    private void EscClose()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && escCloses.Count>0)
        {
            escCloses[escCloses.Count - 1].Close();
        }
    }
    private void OpenBagControl(CardBase cobj, CControl_3 card)
    {
        CardBase top = cobj;
        if (Input.GetKeyUp(ConKeys.OpenBag))
        {
            SendMsg(top, Sbag_40.mTOpenBag, new MsgOpenBag());
        }        
    }
    private void MoveControl(CardBase cobj, CControl_3 card)
    {
        Vector3 cdir = Vector3.zero;
        CardBase top = cobj;
        if (Input.GetKey(KeyCode.A)) cdir += Vector3.left;
        if (Input.GetKey(KeyCode.D)) cdir += Vector3.right;
        if (Input.GetKey(KeyCode.W)) cdir += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) cdir += Vector3.back;
        if (Input.GetKeyDown(KeyCode.Space))
            SendMsg(top, MsgType.Jump, new MsgBase());

        SendMsg(top, MsgType.MoveControl, new MsgMoveControl { dir = cdir.normalized, islocal = true });
    }
    private void InteractControl(CControl_3 card)
    {
        if (!Input.GetKeyUp(ConKeys.InteractKey)) return;
        if (!TryGetCobj(card, out var cobj)) return;

        Vector3 screenPot = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenPot);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 10, MyPhysics.hitMask))
        {
            //Debug.Log(raycastHit.collider.gameObject);
            //Debug.Log(Vector3.Distance(cobj.obj.Center, raycastHit.point));
            if (Vector3.Distance(cobj.obj.Center, raycastHit.point) > card.interactDis) return;
            GameObject obj = raycastHit.collider.gameObject;
            if(obj.TryGetComponent<CardObj>(out var other))
            {
                SendMsg(other.card, MsgType.BeInteract, new MsgBeInteract(cobj));
            }
        }
    }
    private void RotateView(CardBase cobj, CControl_3 card)
    {
        //更新虚拟物体旋转信息
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        SendMsg(cobj, SShoulderCamera_37.mTRotateView, new MsgRotateView(x, y));
    }
    private void ShowMap(CControl_3 card)
    {
        if (Input.GetKeyDown(ConKeys.ShowMap))
        {
            if (MCmap_45_UI.instance != null) // 再次使用按键关闭地图而不是再开一个
                MCmap_45_UI.instance.Close();
            else
            {
                UIBasic.GiveUI(DCmap_45.mapUIPrefab).GetComponent<MCmap_45_UI>().Set(SCmap_45.mainMap);
            }
        }
    }
}