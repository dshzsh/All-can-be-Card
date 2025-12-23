using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using System;
using Cinemachine;
using Unity.VisualScripting;

public class Ccamera_9 : CardBase
{
    public int priority = 0;
    public CinemachineFreeLook camera;
    public Vector3 lookPos;
}
public class MsgRotateControl :MsgBase
{
    public bool interrupt = true;
    public Vector3 seeTo = default;
}
public class MsgGetLookPos : MsgBase
{
    public Vector3 lookPos = Vector3.zero;
    public Vector3 lookDir = Vector3.forward;//移动用，昭示前方的方向
}

public class Scamera_9 : SystemBase
{
    public enum Mode
    {
        first,
        third
    }
    public override void Init()
    {
        AddHandle(MsgType.Update, Update);
        AddHandle(MsgType.GetLookPos, GetLookDir);
        AddHandle(MsgType.OnItem, OnCamera);
    }
    void OnCamera(CardBase _card, MsgBase _msg)
    {
        Ccamera_9 card = _card as Ccamera_9;
        MsgOnItem msg = _msg as MsgOnItem;

        if(msg.op==1)
        {

            card.camera = CreateObj(id).GetComponent<CinemachineFreeLook>();
            card.camera.Priority = card.priority;
        }
        else
        {
            if (card.camera != null)
                GameObject.Destroy(card.camera.gameObject);
        }
    }
    void GetLookDir(CardBase _card, MsgBase _msg)
    {
        Ccamera_9 card = _card as Ccamera_9;
        MsgGetLookPos msg = _msg as MsgGetLookPos;
        if (card.camera.LookAt == null) return;

        msg.lookDir = (card.camera.LookAt.position - card.camera.transform.position).normalized;

        msg.lookPos = card.lookPos;
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        Ccamera_9 card = _card as Ccamera_9;
        if (card.camera.Follow == null)
        {
            if (TryGetCobj(card, out var cobj))
            {
                Transform head = cobj.obj.animator.GetBoneTransform(HumanBodyBones.Head);
                if (head != null) card.camera.LookAt = head;
                else card.camera.LookAt = cobj.obj.transform;

                card.camera.Follow = cobj.obj.transform;
            }
            else card.camera.Priority = -1000;
        }
        if (card.camera.Priority != card.priority)
            card.camera.Priority = card.priority;

        //var x = Input.GetAxis("Mouse X");
        float x = 0.1f;
        var y = Input.GetAxis("Mouse Y");
        // 相机移动
        card.camera.m_XAxis.m_InputAxisValue = -x;
        card.camera.m_YAxis.m_InputAxisValue = -y;

        //更新lookPos
        Vector3 screenPot = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenPot);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 100))
        {
            card.lookPos = raycastHit.point;
        }
        else card.lookPos = (card.camera.LookAt.position - card.camera.transform.position).normalized * 100 + card.camera.LookAt.position;
    }
}