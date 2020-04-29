using PlaneBattle.Assets.Scripts;
using PlaneBattle.Assets.Scripts.Models.SystemBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public float SpawnInterval = 30f;
    private float m_SpawnInterval;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_SpawnInterval -= Time.deltaTime;
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        if (m_SpawnInterval <= 0)
        {
            m_SpawnInterval = SpawnInterval;
            //EnemyType EnemyIndex = GenerateEnemy();
            GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs\\Enemy");
            if (enemyPrefab != null)
            {
                //enemyPrefab.SendMessage("SetInfo", EnemyIndex);
                enemyPrefab = Instantiate<GameObject>(enemyPrefab, transform.position, transform.rotation);
                yield return enemyPrefab;
            }
        }
    }

    private EnemyType GenerateEnemy()
    {
        //初始化奖池
        List<EnemyType> allEnemyType = new List<EnemyType>()
        {
            EnemyType.Normal,
            EnemyType.Rare,
            EnemyType.Epic,
            EnemyType.Legend,
            EnemyType.Boss
        };
        //初始化权重
        List<double> weights = new List<double>()
        {
            10,//最简单的敌人生成率最大
            6,//第二简单的敌人生成率略低
            2,//困难敌人很少生成
            0.05,//小Boss基本不生成
            0.001,//大Boss堪比SSR
        };
        LotteryManager lottery = new LotteryManager(1); //每次只生成一个敌人
        EnemyType[] results = lottery.ControlLottery<EnemyType>(new System.Random(), allEnemyType, weights);
        return results[0];
    }
}
