using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyParticleSystemControl : MonoBehaviour
{
    public ParticleSystem ps;       // 粒子系统
    public float endLifeTime = 2f;     // 最后的一个粒子的消散时间
    public float emitDuration = 1f; // 整圈发射用时（秒）

    public bool loop = true;
    public float totalTime = 3f;

    private float time = 0f;

    void Start()
    {
        Emmit();
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time > totalTime)
        {
            if (!loop)
                Destroy(gameObject);
            else
            {
                time -= totalTime;
                Emmit();
            }
        }
    }
    public virtual void Emmit()
    {
        StartCoroutine(EmitCircleSequential());
        StartCoroutine(EmitCrossSequential());
    }
    public IEnumerator EmitCrossSequential(int crossParticleCount = 20, float radius = 1f)
    {
        if (ps == null) yield break;

        float delay = emitDuration / crossParticleCount; // 每个粒子发射间隔

        // 竖向
        for (int i = 0; i < crossParticleCount / 2; i++)
        {
            // 映射到-1~1
            float shift = (i - (crossParticleCount / 2 / 2.0f)) / (crossParticleCount / 2 / 2.0f);
            Vector3 pos = shift * radius * Vector3.up;

            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.position = pos;

            // 保证统一同时消散：加上emitDuration
            emitParams.startLifetime = endLifeTime + delay * (crossParticleCount - i - 1);

            ps.Emit(emitParams, 1);

            yield return new WaitForSeconds(delay); // 等待再发射下一个
        }

        // 横向
        for (int i = crossParticleCount / 2; i < crossParticleCount; i++)
        {
            float shift = (i - crossParticleCount / 2 - (crossParticleCount / 2 / 2.0f)) / (crossParticleCount / 2 / 2.0f);
            Vector3 pos = shift * radius * Vector3.left;

            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.position = pos;

            // 保证统一同时消散：加上emitDuration
            emitParams.startLifetime = endLifeTime + delay * (crossParticleCount - i - 1);

            ps.Emit(emitParams, 1);

            yield return new WaitForSeconds(delay); // 等待再发射下一个
        }
    }

    public IEnumerator EmitCircleSequential(int cicleParticleCount = 20, float radius = 1f)
    {
        if (ps == null) yield break;

        float delay = emitDuration / cicleParticleCount; // 每个粒子发射间隔

        for (int i = 0; i < cicleParticleCount; i++)
        {
            float angle = (Mathf.PI * 2f / cicleParticleCount) * i; // 顺时针均分角度
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.position = pos;

            // 保证统一同时消散：加上emitDuration
            emitParams.startLifetime = endLifeTime + delay * (cicleParticleCount - i - 1);

            ps.Emit(emitParams, 1);

            yield return new WaitForSeconds(delay); // 等待再发射下一个
        }
    }
}
public class MMOwarnCsm_6_csm : MyParticleSystemControl
{
    public int circleCount = 20;
    public int crossCount = 10;

    public override void Emmit()
    {
        StartCoroutine(EmitCircleSequential(circleCount));
        StartCoroutine(EmitCrossSequential(crossCount));
    }
}
