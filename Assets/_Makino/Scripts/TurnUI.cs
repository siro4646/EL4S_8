using TMPro;
using UnityEngine;

public class TurnUI : MonoBehaviour
{
    public bool is1PTurn = true;
    public TextMeshProUGUI turnText;

    void Start()
    {
        UpdateTurnUI();
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
        if (is1PTurn)
        {
            //隠す側のターン
            turnText.text = "TaxEvader";
            turnText.color = Color.red;
        }
        else
        {
            //見つける側のターン
            turnText.text = "TaxAuditor";
            turnText.color = Color.blue;
        }
    }
}
