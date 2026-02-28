using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float turnDuration = 60.0f;
    private float timeRemaining;

    public TextMeshProUGUI timerText;
    public TurnUI turnManager;

    void Start()
    {
        timeRemaining = turnDuration;
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        }
        else
        {
            OnTimeUp();
        }
    }

    void OnTimeUp()
    {
        turnManager.SwitchTurn();
        timeRemaining = turnDuration;
    }
}
