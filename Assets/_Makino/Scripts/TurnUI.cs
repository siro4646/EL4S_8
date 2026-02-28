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

    
    public void SwitchTurn()
    {
        //ターン反転
        is1PTurn = !is1PTurn;
        UpdateTurnUI();
    }

    //UI更新
    private void UpdateTurnUI()
    {
        if (is1PTurn)
        {
            turnText.text = "TaxEvader";
            turnText.color = Color.red;
        }
        else
        {
            turnText.text = "Surveyor";
            turnText.color = Color.blue;
        }
    }
}
