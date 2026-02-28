using TMPro;
using UnityEngine;

public class TurnUI : MonoBehaviour
{
    public bool is1PTurn = true;
    public TextMeshProUGUI turnText;
    public SysTimer sysTimer;

    void Start()
    {
        UpdateTurnUI();
        sysTimer = GameObject.FindAnyObjectByType<SysTimer>();
    }

    //ターン反転
    public void SwitchTurn()
    {
        is1PTurn = !is1PTurn;
        UpdateTurnUI();
    }

    //UI更新
    private void UpdateTurnUI()
    {
        if (sysTimer.GetNowGamestate() == SysTimer.GameState.PlayerPhase)
        {
            //隠す側のターン
            turnText.text = "TaxEvader";
            turnText.color = Color.red;
        }
        else if (sysTimer.GetNowGamestate() == SysTimer.GameState.MarusaPhase)
        {
            //見つける側のターン
            turnText.text = "TaxAuditor";
            turnText.color = Color.blue;
        }
        else
        {
            //見つける側のターン
            turnText.text = "???";
            turnText.color = Color.white;
        }
    }

    public bool IsAuditorTurn()
    {
        return !is1PTurn;
    }
}
