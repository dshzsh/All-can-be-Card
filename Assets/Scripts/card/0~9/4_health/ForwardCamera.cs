using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // 获取相机的位置
        Vector3 cameraPosition = Camera.main.transform.position;

        // 让物体朝向相机
        transform.LookAt(cameraPosition);
    }
}
