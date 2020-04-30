using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public readonly static object locker = new object();
    public Transform Canvas_Main,Canvas_GameOver;

    private Text Text_Score,Text_Best;
    private SpawnEnemy SpawnPoint;
    protected int Score,BestScore;
    protected Player Player;
    private void Awake()
    {
        if(Instance == null)
        {
            lock (locker)
            {
                if(Instance == null)
                {
                    Instance = this;
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Init()
    {
        if (SceneManager.GetActiveScene().name != "GameTitle")
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); //找到玩家脚本
            Text_Score = Canvas_Main.transform.Find("Panel").transform.Find("txt_score").GetComponent<Text>(); //找到得分的标签的脚本
            Text_Score.text = "0"; //初始化得分
            Text_Best = Canvas_Main.transform.Find("Panel").transform.Find("txt_best").GetComponent<Text>();
            Text_Best.text = "0";
            SpawnPoint = GameObject.Find("SpawnPoint_Enemy").GetComponent<SpawnEnemy>();
        }
        Button btn_Restart = null;
        Button btn_Exit = null;
        if(Canvas_GameOver != null)
        {
            btn_Exit = Canvas_GameOver.transform.Find("Panel").transform.Find("Button_Exit").GetComponent<Button>();
            btn_Restart = Canvas_GameOver.transform.Find("Panel").transform.Find("Button_Restart").GetComponent<Button>(); //找到重新开始按钮的脚本
            Canvas_GameOver.gameObject.SetActive(false); //默认不显示游戏失败界面
        }
        else
        {
            btn_Exit = Canvas_Main.transform.Find("Panel").transform.Find("Button_Exit").GetComponent<Button>();
        }
        if (btn_Exit != null)
        {
            btn_Exit.onClick.AddListener(delegate ()
            {
                if(SceneManager.GetActiveScene().name == "GameTitle")
                {
                    Application.Quit();
                }
                else
                {
                    SceneManager.LoadScene("GameTitle");
                }
            });
        }
        if (btn_Restart != null)
        {
            btn_Restart.onClick.AddListener(delegate () //给按钮的点击事件添加新事件
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); //重新开始当前场景
            });
        }
        if (SceneManager.GetActiveScene().name == "GameTitle")
        {
            Button btn_Start = Canvas_Main.transform.Find("Panel").transform.Find("Button_Start").GetComponent<Button>();
            if (btn_Start != null)
            {
                btn_Start.onClick.AddListener(delegate ()
                {
                    SceneManager.LoadScene("Level_0");
                });
            }
        }
    }

    public void AddScore(int num)
    {
        Score += num;
        if(BestScore <= Score)
        {
            BestScore = Score;
        }
        Text_Score.text = Score.ToString();
        Text_Best.text = BestScore.ToString();
    }

    public void GameOver()
    {
        SpawnPoint.IsStop = true;
        Canvas_Main.gameObject.SetActive(false);
        if (Canvas_GameOver != null)
        {
            Canvas_GameOver.gameObject.SetActive(true);
        }
    }
}
