using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed;
    public readonly float AttackInterval = 2.5f;
    public float Health;

    private float m_AttackInterval;
    private RocketInfo m_EnemyRocketData;

    protected bool IsActivited;
    protected Renderer m_Renderer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());
        ChangeScale(new Vector3(0.1f, 0.1f, 0.1f));
        InitRocketData();
    }

    // Update is called once per frame
    void Update()
    {
        m_AttackInterval -= Time.deltaTime;
        VerifyAttack();
        VerifyMove();
        VerifyDestory();
    }

    /// <summary>
    /// 初始化敌人模型
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Init()
    {
        GameObject enemyModel = Resources.Load<GameObject>("Models\\SpaceShips\\Enemy_01");
        if(enemyModel != null)
        {
            enemyModel = Instantiate<GameObject>(enemyModel, transform);
            enemyModel.AddComponent<BoxCollider>().isTrigger = true;
            enemyModel.tag = tag;
            StartCoroutine(InitTexture(enemyModel));
            yield return enemyModel;
        }
        m_Renderer = transform.GetComponent<Renderer>();
    }

    /// <summary>
    /// 初始化材质
    /// </summary>
    /// <param name="enemyModel"></param>
    /// <returns></returns>
    protected virtual IEnumerator InitTexture(GameObject enemyModel)
    {
        //设置材质
        MeshRenderer renderer = enemyModel.GetComponentInChildren<MeshRenderer>();
        //加载材质
        Texture2D diffuseTexture = Resources.Load<Texture2D>("Images\\Textures\\SpaceShips\\Enemy_01_diffuse"); //主材质
        Texture2D normalTexture = Resources.Load<Texture2D>("Images\\Textures\\SpaceShips\\Enemy_01_normal"); //我也不知道这算是什么图
        Texture2D emissionTexture = Resources.Load<Texture2D>("Images\\Textures\\SpaceShips\\Enemy_01_emission"); //反射材质
        Texture2D specularTexture = Resources.Load<Texture2D>("Images\\Textures\\SpaceShips\\Enemy_01_specular"); //光谱图
        Material material = new Material(Shader.Find("Standard"));
        yield return material;
        material.SetTexture("_MainTex", diffuseTexture); //设置主材质
        material.EnableKeyword("_NORMALMAP"); //启用NormalMap
        material.SetTexture("_DetailNormalMap", normalTexture); //设置Normal材质
        material.EnableKeyword("_EMISSION"); //启用反射材质
        material.SetTexture("_EmissionMap", emissionTexture); //设置反射材质
        material.SetColor("_EmissionColor", Color.white); //设置反射材质的颜色
        material.EnableKeyword("_SPECGLOSSMAP"); //启用光谱图
        material.SetTexture("_MetallicGlossMap", specularTexture); //设置光谱图
        renderer.material = material;
    }

    /// <summary>
    /// 修改大小
    /// </summary>
    /// <param name="vTo"></param>
    public void ChangeScale(Vector3 vTo)
    {
        transform.localScale = vTo; //修改大小
    }

    /// <summary>
    /// 初始化飞弹数据
    /// </summary>
    protected virtual void InitRocketData()
    {
        if(m_EnemyRocketData == null)
        {
            m_EnemyRocketData = new RocketInfo
            {
                SkinIndex = 2,
                Damage = 4,
                Speed = -4,
                FromTags = GameTags.Enemy,
                Rotation = new Vector3(90, -90, 0),
                FlipX = true
            };
        }
    }

    /// <summary>
    /// 模拟攻击
    /// </summary>
    protected virtual void VerifyAttack()
    {
        if(m_AttackInterval <= 0)
        {
            m_AttackInterval = AttackInterval;
            CreateRocket();
        }
    }

    /// <summary>
    /// 创建飞弹
    /// </summary>
    /// <returns></returns>
    protected virtual GameObject CreateRocket()
    {
        GameObject rocketModel = Resources.Load<GameObject>("Prefabs\\Rocket");
        if(rocketModel != null)
        {
            rocketModel = Instantiate<GameObject>(rocketModel, transform.position, transform.rotation);
        }
        rocketModel.SendMessage("SetInfo", JsonUtility.ToJson(m_EnemyRocketData));
        return rocketModel;
    }

    /// <summary>
    /// 模拟移动
    /// </summary>
    protected virtual void VerifyMove()
    {
        float horizontalValue = Mathf.Sin(Time.time) * Time.deltaTime; //计算Speed和Time.time的Sin值，让水平坐标在1~-1中移动
        transform.Translate(new Vector3(horizontalValue, 0, Speed * Time.deltaTime));
    }

    private void OnBecameVisible()
    {
        IsActivited = true;
    }

    protected virtual void VerifyDestory()
    {
        if(IsActivited && !m_Renderer.isVisible)
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnPlayerHit(RocketInfo vInfo)
    {
        Health -= vInfo.Damage;
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
