using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public readonly float AttackInterval = 1.5f;
    public float Health;

    private float m_AttackInterval;
    private RocketInfo m_PlayerRocketData;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());
    }

    // Update is called once per frame
    void Update()
    {
        //控制攻击间隔
        m_AttackInterval -= Time.deltaTime;
        //检测移动
        VerifyMove();
        //检测攻击
        VerifyAttack();
    }

    private IEnumerator Init()
    {
        GameObject playerModel = Resources.Load<GameObject>("Models\\SpaceShips\\SciFi_Fighter_MK");
        if(playerModel != null)
        {
            playerModel = Instantiate<GameObject>(playerModel, transform);
            yield return playerModel;
        }
        playerModel.transform.parent = transform;
        playerModel.AddComponent<BoxCollider>().isTrigger = true;
        playerModel.tag = tag;
        m_PlayerRocketData = new RocketInfo()
        {
            SkinIndex = 0,
            Damage = 10,
            Speed = 4,
            FromTags = GameTags.Player,
            Rotation = new Vector3(90,-90,0)
        };
    }

    private void VerifyMove()
    {
        float horizontalValue = Input.GetAxis("Horizontal");
        float verticalValue = Input.GetAxis("Vertical");

        if(horizontalValue != 0 || verticalValue != 0)
        {
            //这里在设计器内用ISO的视角模式更为明显，从上到下俯视是y，而物体的移动前后是z，左右是x
            transform.Translate(new Vector3(horizontalValue*Speed*Time.deltaTime, 0, verticalValue*Speed*Time.deltaTime));
        }
    }

    private void VerifyAttack()
    {
        if (Input.GetKey(KeyCode.Space) && m_AttackInterval <=0)
        {
            m_AttackInterval = AttackInterval;
            GameObject rocket = CreateRocket();
        }
    }

    private GameObject CreateRocket()
    {
        GameObject rocketModel = Resources.Load<GameObject>("Prefabs\\Rocket");
        if(rocketModel != null)
        {
            rocketModel = Instantiate<GameObject>(rocketModel,transform.position,transform.rotation);
        }
        rocketModel.SendMessage("SetInfo", JsonUtility.ToJson(m_PlayerRocketData));
        return rocketModel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            //碰到敌人自己瞬间暴毙
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.GameOver();
        //transform.GetComponent<Animation>().Play("An_Explode");
    }
}
