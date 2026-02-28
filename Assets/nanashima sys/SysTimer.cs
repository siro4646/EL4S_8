using UnityEngine;
using UnityEngine.SceneManagement;

public class SysTimer : MonoBehaviour
{
    float startTime;

    private enum GameState
    {
        PlayerPhase,
        WaitForInput,
        MarusaPhase,
        End
    }

    private GameState state;

    public GameObject playerPrefab;
    public GameObject marusaPrefab;
    public Transform spawnPoint;

    GameObject currentObject;

    void Start()
    {
        ChangeState(GameState.PlayerPhase);
    }

    void Update()
    {
        switch (state)
        {
            case GameState.PlayerPhase:
                if (Time.time - startTime >= 60f)
                {
                    ChangeState(GameState.WaitForInput);
                }
                break;

            case GameState.WaitForInput:

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ChangeState(GameState.MarusaPhase);
                }
                break;

            case GameState.MarusaPhase:
                if (Time.time - startTime >= 60f)
                {
                    ChangeState(GameState.End);
                }
                break;
        }
    }

    void ChangeState(GameState newState)
    {
        state = newState;
        startTime = Time.time;

        switch (state)
        {
            case GameState.PlayerPhase:
                currentObject = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
                break;

            case GameState.WaitForInput:
                Destroy(currentObject);
                // プレイヤーは残したまま待つ
                break;

            case GameState.MarusaPhase:
               
                currentObject = Instantiate(marusaPrefab, spawnPoint.position, Quaternion.identity);
                break;

            case GameState.End:
                SceneManager.LoadScene("GameoverScene");
                break;
        }
    }
    GameState GetNowGamestate()
    {
        return state;
    }
    int GetTime()
    {
        return (int)Time.time;
    }
}
