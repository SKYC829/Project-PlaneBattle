using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 飞弹的数据实体类对象
/// </summary>
[Serializable]
public class RocketInfo
{
    /// <summary>
    /// 飞行速度
    /// </summary>
    public float Speed;

    /// <summary>
    /// 伤害
    /// </summary>
    public float Damage;

    /// <summary>
    /// 皮肤索引
    /// </summary>
    public int SkinIndex;

    /// <summary>
    /// 来源标识
    /// </summary>
    public GameTags FromTags;

    /// <summary>
    /// 旋转角度
    /// </summary>
    public Vector3 Rotation;

    /// <summary>
    /// 反转x轴
    /// </summary>
    public bool FlipX;

    /// <summary>
    /// 反转y轴
    /// </summary>
    public bool FlipY;

    public RocketInfo()
    {
        Speed = 1;
        Damage = 1;
        SkinIndex = 1;
        FromTags = GameTags.Enemy;
        Rotation = new Vector3(90, 0, 0);
        FlipY = true;
        FlipX = false;
    }
}
