using TMPro;
using UnityEngine;

public class QuantityUI : MonoBehaviour
{
    public int currentCount { get; private set; }

    [SerializeField] private int initialCount = 5;
    [SerializeField] private TextMeshProUGUI countText;

    void Start()
    {
        currentCount = initialCount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (countText != null)
        {
            countText.text = currentCount.ToString() + " / 5";
        }
    }

    public void ChangeCount(int amount)
    {
        currentCount += amount;

        if (currentCount < 0) currentCount = 0;

        UpdateUI();
    }
}
