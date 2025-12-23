using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObj : MonoBehaviour
{
    public static int hash_moveSpeed;
    public static int hash_move;
    public static int hash_atkSpeed;
    public static int hash_direction;
    public static int layer_ghost, layer_wfzd;
    public static int layer_normal;
    
    public CObj_2 card;
    public Rigidbody rbody;
    public bool useAtk2 = false;
    public Animator animator;
    public AnimatorOverrideController overrideController;
    public int base_layer = 0;
    public Vector3 Center
    {
        get
        {
            return transform.position + card.centerOffset;
        }
    }
    private void Awake()
    {
        rbody= GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        base_layer = gameObject.layer;
        if (animator != null)
        {
            overrideController = new AnimatorOverrideController(Slive_19.aniCon);
            animator.runtimeAnimatorController = overrideController;
        }
    }
    float lockMotionTime = 0;
    float upperTime = 0;

    float mbweight = 0;
    float wVelocity = 0;
    float mbspeed = 0f;
    float sVelocity = 0;

    private void FixedUpdate()
    {
        if(!CardManager.CardValid(card)) return;

        SystemManager.SendMsg(card, MsgType.FixedUpdate, new MsgUpdate(card, Time.fixedDeltaTime));
    }
    private float time = 0;
    private void SendUpdate()
    {
        if (!CardManager.CardValid(card)) return;

        MsgUpdate msg = new MsgUpdate(card, Time.deltaTime);
        SystemManager.SendMsg(card, MsgType.Update, msg);

        if (MyTool.IntervalTime(1, ref time, msg.time))
        {
            SystemManager.SendMsg(card, MsgType.UpdateSec, new MsgUpdate(card, 1));
        }
    }
    private int collisionCnt = 0;
    public bool OnGround()
    {
        /*
        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up * 0.01f, Vector3.down, 0.1f, MyPhysics.hitMask);
        foreach(RaycastHit hit in hits)
        {
            if (hit.collider.newGameObject != newGameObject) return true;
        }*/
        return collisionCnt > 0;
        //return Mathf.Abs(rbody.GetAccumulatedForce().y) < 0.01f;
    }
    public bool InLockMotion()
    {
        return lockMotionTime > 0;
    }
    public bool InUpperMotion()
    {
        return upperTime > 0;
    }
    private void DoLayerWeight(int index, float weight)
    {
        mbweight = weight;
        //animator.SetLayerWeight(index, weight);
    }
    public void SetDirection(float dir)
    {
        if (animator == null) return;

        animator.SetFloat(hash_direction, dir);
    }

    public void SetMoveSpeed(float speed)
    {
        if (animator == null) return;

        mbspeed = speed >= 0.1f ? 1f : 0f;

        if (mbspeed != 0f)
            animator.SetFloat(hash_moveSpeed, speed);
    }
    public void SetDie(int die)
    {
        if (animator == null) return;

        animator.SetInteger("die", die);
    }
    public void SetUpper(float time)
    {
        upperTime = time;

        if (animator == null) return;

        if (time == 0)
            DoLayerWeight(1, 0);
        else
            DoLayerWeight(1, 1);
    }
    public void SetLockMotion(float time)
    {

        lockMotionTime = time;

        if (animator == null) return;

        /*
        if (time==0)
        {
            animator.applyRootMotion = false;
            return;
        }
        if(animator.applyRootMotion == false)
        {
            animator.applyRootMotion = true;
        }*/
    }
    private void Update()
    {
        lockMotionTime -= Time.deltaTime;
        upperTime -= Time.deltaTime;
        SendUpdate();
        if (animator == null) return;
        float weight = animator.GetLayerWeight(1);
        if(!MyMath.FloatEqual(weight, mbweight))
        {
            animator.SetLayerWeight(1, Mathf.SmoothDamp(weight, mbweight, ref wVelocity, 0.1f));
        }
        float speed = animator.GetFloat(hash_move);
        if (!MyMath.FloatEqual(speed,mbspeed))
        {
            animator.SetFloat(hash_move, Mathf.SmoothDamp(speed, mbspeed, ref sVelocity, 0.1f));
        }
        
        if (lockMotionTime < 0&&lockMotionTime>=-5000)
        {
            animator.applyRootMotion = false;
            lockMotionTime = -10000;
        }
        if(upperTime<0 && upperTime >= -5000)
        {
            DoLayerWeight(1, 0);
            upperTime = -10000;
        }
    }

    private void OnDestroy()
    {
        if (card != null)
            CardManager.DestroyCard(card);
    }
    private bool NeedCollision()
    {
        return true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!NeedCollision()) return;
        collisionCnt++;
        collision.gameObject.TryGetComponent<CardObj>(out var cardObj);
        CObj_2 ocard = cardObj != null ? cardObj.card : null;
        SystemManager.SendMsg(card, MsgType.Collision,
                new MsgCollision(card, ocard, collision.gameObject, collision.GetContact(0).point));
    }
    private void OnCollisionExit(Collision collision)
    {
        if (!NeedCollision()) return;
        collisionCnt--;
        collision.gameObject.TryGetComponent<CardObj>(out var cardObj);
        CObj_2 ocard = cardObj != null ? cardObj.card : null;
        SystemManager.SendMsg(card, MsgType.CollisionExit,
                new MsgCollision(card, ocard, collision.gameObject, collision.collider.ClosestPointOnBounds(transform.position)));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!NeedCollision()) return;
        other.gameObject.TryGetComponent<CardObj>(out var cardObj);
        CObj_2 ocard = cardObj != null ? cardObj.card : null;
        SystemManager.SendMsg(card, MsgType.Collision,
                new MsgCollision(card, ocard, other.gameObject, other.ClosestPointOnBounds(transform.position)));
    }
    private void OnTriggerExit(Collider other)
    {
        if (!NeedCollision()) return;
        other.gameObject.TryGetComponent<CardObj>(out var cardObj);
        CObj_2 ocard = cardObj != null ? cardObj.card : null;
        SystemManager.SendMsg(card, MsgType.CollisionExit,
                new MsgCollision(card, ocard, other.gameObject, other.ClosestPointOnBounds(transform.position)));
    }
    private int ghostCnt = 0;//当前的穿透计数，升为1以上后进入虚无状态（基本除了地板什么都不碰撞）
    private int wfzdCnt = 0;// 无法阻挡，比ghost稍微差一点，会碰撞子弹
    private void LayerUpdate()
    {
        if (ghostCnt == 0 && wfzdCnt == 0)
        {
            gameObject.layer = base_layer;
        }
        else if (ghostCnt > 0) 
        {
            gameObject.layer = layer_ghost;
        }
        else if (wfzdCnt > 0)
        {
            gameObject.layer = layer_wfzd;
        }
    }
    public void AddGhost(int add)
    {
        ghostCnt += add;
        LayerUpdate();
    }
    public void AddWfzd(int add)
    {
        wfzdCnt += add;
        LayerUpdate();
    }
    public void ToMaterial(Material tarMaterial)
    {
        foreach (Renderer targetRenderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            List<Material> newMaterials = new();
            foreach (Material material in targetRenderer.materials)
            {
                Material material1 = new Material(tarMaterial);
                if (material.HasProperty("_MainTex"))
                    material1.mainTexture = material.mainTexture;
                newMaterials.Add(material1);
            }
            targetRenderer.SetMaterials(newMaterials);
        }
    }
    public List<GameObject> AddMaterial(Material tarMaterial)
    {
        List<GameObject> list = new List<GameObject>();
        foreach (Renderer targetRenderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            List<Material> newMaterials = new();
            foreach (Material material in targetRenderer.materials)
            {
                Material material1 = new Material(tarMaterial);
                material1.mainTexture = material.mainTexture;
                newMaterials.Add(material1);
            }
            GameObject newGameObject = new GameObject("newMaterial");
            list.Add(newGameObject);
            //newGameObject.AddComponent<FollowPos>().mb = targetRenderer.newGameObject;
            if (targetRenderer is SkinnedMeshRenderer skinned)
            {
                SkinnedMeshRenderer newSkinned = newGameObject.AddComponent<SkinnedMeshRenderer>();
                newSkinned.SetMaterials(newMaterials);
                newSkinned.rootBone = skinned.rootBone;
                newSkinned.sharedMesh = skinned.sharedMesh;
                newSkinned.updateWhenOffscreen= skinned.updateWhenOffscreen;
                newSkinned.bones= skinned.bones;
            }
            else if(targetRenderer is MeshRenderer)
            {
                MeshRenderer newMr = newGameObject.AddComponent<MeshRenderer>();
                newMr.SetMaterials(newMaterials);
                MeshFilter newMf = newGameObject.AddComponent<MeshFilter>();
                newMf.mesh = targetRenderer.GetComponent<MeshFilter>().mesh;
            }
            newGameObject.transform.SetParent(targetRenderer.transform, false);
        }
        return list;
    }
}
