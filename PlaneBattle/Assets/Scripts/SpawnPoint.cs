using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private static InternalGameObjectInfo m_SpawnObj;
    public float SpawnInterval;
    private float m_SpawnInterval;
    // Start is called before the first frame update
    void Start()
    {
        m_SpawnObj = new InternalGameObjectInfo();
    }

    // Update is called once per frame
    void Update()
    {
        m_SpawnInterval -= Time.deltaTime;
        Spawn();
        VerifyMove();
    }

    private void Spawn()
    {
        if(tag == "Player")
        {
            SpawnPlayer();
        }else if(tag == "Enemy")
        {
            if (m_SpawnInterval <= 0)
            {
                m_SpawnInterval = SpawnInterval;   
                SpawnEnemy();
            }
        }
    }

    /// <summary>
    /// 生成玩家
    /// </summary>
    public void SpawnPlayer()
    {
        lock (m_SpawnObj)
        {
            if(m_SpawnObj.SpawnObject == null)
            {
                //没有生成过，生成一个新的
                GameObject temp = Resources.Load<GameObject>("Prefabs\\PlayerPlane");
                if(temp != null)
                {
                    temp = Instantiate<GameObject>(temp, transform.position, transform.rotation);
                    m_SpawnObj.SpawnObject = temp;
                    m_SpawnObj.IsInstance = true;
                }
            }else if(m_SpawnObj.SpawnObject != null)
            {
                GameObject temp = GameObject.FindGameObjectWithTag("Player");
                if(temp == null)
                {
                    m_SpawnObj.SpawnObject = null;
                    SpawnPlayer();
                }
            }
        }
    }

    /// <summary>
    /// 生成敌人
    /// </summary>
    public void SpawnEnemy()
    {
        GameObject temp = Resources.Load<GameObject>("Prefabs\\Enemy");
        if (temp != null)
        {
            temp = Instantiate<GameObject>(temp, transform.position, temp.transform.rotation);
            m_SpawnObj.SpawnObject = temp;
        }
    }

    public void VerifyMove()
    {
        float horizontalValue = Mathf.Sin(Time.time) * Time.deltaTime; //模拟移动让敌人在水平位置上的任意位置生成
        transform.Translate(new Vector3(horizontalValue, 0, 0));
    }

    private class InternalGameObjectInfo
    {
        public GameObject SpawnObject;

        public bool IsInstance;
    }
}
