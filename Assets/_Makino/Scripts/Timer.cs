using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float turnDuration = 10.0f;
    private float timeRemaining;
    private bool isTimerRunning = false; // 1. タイマーが動いているか

    public TextMeshProUGUI timerText;
    public GameObject changeTurnPanel; // 2. 「Enterを押して交代」を表示するUIパネル
    public TurnUI turnUI;

    public SysTimer timer;

    void Start()
    {
        timeRemaining = turnDuration;
        isTimerRunning = true;
        timer = GameObject.FindAnyObjectByType<SysTimer>();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timerText.text = "Time:" + Mathf.CeilToInt(timeRemaining).ToString();
                timerText.text = "Time:" + (60 - timer.GetTime());
            }
            else
            {
                OnTimeUp();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartNextTurn();
            }
        }
    }

    void OnTimeUp()
    {
        isTimerRunning = false;
        timeRemaining = 0;
        timerText.text = "Time:0";

        // 1. もし現在が「見つける側（TaxAuditor）」のターンなら終了
        if (turnUI.IsAuditorTurn())
        {
            EndGame();
        }
        else
        {
            // 2. 隠す側の後なら通常通り交代パネルを表示
            changeTurnPanel.SetActive(true);
        }
    }

    // ボタンから呼び出すか、Enterキーで実行される関数
    public void StartNextTurn()
    {
        // 6. 役割や表示の切り替えを実行
        turnUI.SwitchTurn();

        // 7. 表示のリセット
        changeTurnPanel.SetActive(false);
        timeRemaining = turnDuration;
        isTimerRunning = true; // タイマー再開
    }

    void EndGame()
    {
        Debug.Log("終了");
        //リザルトシーン
        //SceneManager.LoadScene("ResultScene");
    }
}
