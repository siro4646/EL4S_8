using UnityEngine;

[RequireComponent(typeof(HideMoney))]
public class CriminalInateract : MonoBehaviour
{
    HideMoney hideMoney;
    [SerializeField] private KeyCode hideRealMoneyKey = KeyCode.R;
    [SerializeField] private int hideRealMoneyMaxCount = 3;
    [SerializeField] private KeyCode hideDummyMoneyKey = KeyCode.T;
    [SerializeField] private int hideDummyMoneyMaxCount = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hideMoney = GetComponent<HideMoney>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(hideRealMoneyKey))
        {
            if(hideRealMoneyMaxCount <= 0)
            {
                return;
            }
            if (hideMoney.HideRealMoneyNearest())
            {
                hideRealMoneyMaxCount--;
            }
        }

        if (Input.GetKeyDown(hideDummyMoneyKey))
        {
            if (hideDummyMoneyMaxCount <= 0)
            {
                return;
            }
            if (hideMoney.HideDummyMoneyNearest())
            {
                hideDummyMoneyMaxCount--;
            }
        }
    }
}
