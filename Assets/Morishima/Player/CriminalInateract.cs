using UnityEngine;

[RequireComponent(typeof(HideMoney))]
public class CriminalInateract : MonoBehaviour
{
    HideMoney hideMoney;
    [SerializeField] private KeyCode hideMoneyKey = KeyCode.Space;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hideMoney = GetComponent<HideMoney>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(hideMoneyKey))
        {
            hideMoney.HideMoneyNearest();
        }
    }
}
