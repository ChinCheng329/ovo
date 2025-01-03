using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public string 玩家Tag = "Player";  // 玩家对象的Tag
    public Vector3 偏移量 = new Vector3(0, 5, -10); // 相机相对玩家的位置偏移
    public float 跟隨速度 = 5f; // 跟随速度

    private Transform 玩家;

    void Start()
    {
        // 根据玩家的 Tag 查找玩家对象
        玩家 = GameObject.FindGameObjectWithTag(玩家Tag).transform;
    }

    void LateUpdate()
    {
        if (玩家 != null)
        {
            // 计算相机目标位置
            Vector3 目標位置 = 玩家.position + 偏移量;

            // 平滑过渡到目标位置
            transform.position = Vector3.Lerp(transform.position, 目標位置, 跟隨速度 * Time.deltaTime);

            // 可选：让相机始终朝向玩家
            transform.LookAt(玩家);
        }
    }
}