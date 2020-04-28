using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour,ISendInfo
{
    private RocketInfo m_Data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        VerifyMove();
    }

    public void SetInfo(string vFrom)
    {
        //设置实体类对象，用作数据的传递
        m_Data = JsonUtility.FromJson<RocketInfo>(vFrom);
        //启动协同根据实体类对象初始化物体
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images\\Sequences");
        SpriteRenderer renderer = transform.GetComponent<SpriteRenderer>();
        renderer.sprite = sprites[m_Data.SkinIndex];
        renderer.flipX = m_Data.FlipX;
        renderer.flipY = m_Data.FlipY;
        yield return renderer;
        transform.rotation = Quaternion.Euler(m_Data.Rotation);
    }

    protected virtual void VerifyMove()
    {
        if(m_Data != null)
        {
            float realSpeed = m_Data.Speed * Time.deltaTime;
            Vector3 forward = CalculationForward();
            transform.Translate(forward * realSpeed,Space.World);
        }
    }

    /// <summary>
    /// 根据旋转角度判断朝向
    /// </summary>
    /// <returns></returns>
    protected virtual Vector3 CalculationForward()
    {
        //因为视角只能沿Y轴从上向下俯视，而物体的Sprite是2D的，为了显示出来x肯定是旋转了-90，z变动体现不出来，所以最需要判断的是y轴
        float y = Mathf.Abs(m_Data.Rotation.y); //得到y轴的绝对值
        //因为是手动进行旋转，所以基本上可以确定绝对是0，90，180，360这几个值，不带小数
        if(y == 90)
        {
            //在设计器上观察得出，y旋转了90或-90,向着“前方”的坐标轴都是标着红色的x轴
            return transform.right;
        }
        else
        {
            //在设计器上观察得出，y轴旋转了0,180或360，向着“前方”的坐标轴都是蓝色的z轴，但是在Persp坐标对应的好像是标着绿色的y轴
            return transform.up;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                VerifyHit_Player(other);
                break;
            case "Enemy":
                VerifyHit_Enemy(other);
                break;
            //case "Rocket":
            default:
                VerifyHit_Rocket(other);
                break;
        }
    }

    private void VerifyHit_Rocket(Collider other)
    {
        //if(m_Data.FromTags == GameTags.Player && other.tag == "Enemy")
        //{
        //    //玩家可以击破敌人的子弹
        //    Destroy(other.gameObject);
        //}
        Destroy(other.gameObject);
        DestroySelf();
    }

    private void VerifyHit_Player(Collider other)
    {
        if (m_Data.FromTags != GameTags.Player)
        {
            DestroySelf();
            //敌人对玩家一击必杀
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }

    private void VerifyHit_Enemy(Collider other)
    {
        if(m_Data.FromTags == GameTags.Player)
        {
            DestroySelf();
            other.gameObject.transform.parent.gameObject.SendMessage("OnPlayerHit", m_Data);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
