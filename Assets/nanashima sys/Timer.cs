using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    float startTime;

    private enum GameState
    {
        PlayerPhase,
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
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime >= 60f)
        {
            if (state == GameState.PlayerPhase)
                ChangeState(GameState.MarusaPhase);
            else if (state == GameState.MarusaPhase)
                ChangeState(GameState.End);

            startTime = Time.time;
        }
    }

    void ChangeState(GameState newState)
    {
        state = newState;

        switch (state)
        {
            case GameState.PlayerPhase:
                currentObject = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
                break;

            case GameState.MarusaPhase:
                Destroy(currentObject);
                currentObject = Instantiate(marusaPrefab, spawnPoint.position, Quaternion.identity);
                break;

            case GameState.End:
                SceneManager.LoadScene("EndScene");
                break;
        }
    }
}
