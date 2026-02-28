using TMPro;
using UnityEngine;

public class QuantityUI : MonoBehaviour
{
    //隠す物の現在数と最大数
    [Header("Item A Settings")]
    public string nameA = "大";
    public int countA = 1;
    public int maxA = 1;
    public TextMeshProUGUI textA;

    [Header("Item B Settings")]
    public string nameB = "中";
    public int countB = 1;
    public int maxB = 1;
    public TextMeshProUGUI textB;

    [Header("Item C Settings")]
    public string nameC = "小";
    public int countC = 1;
    public int maxC = 1;
    public TextMeshProUGUI textC;

    public ItemSlotUI[] slots; // 3つのスロットを登録
    private int selectedIndex = 0;

    void Start()
    {
        SelectSlot(0);

        UpdateAllUI();
    }

    void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
    }

    //外部からID（0, 1, 2）と増減量を指定して呼び出す
    public void ChangeCount(int itemID, int amount)
    {
        if (itemID == 0) //大
        {
            countA = Mathf.Clamp(countA + amount, 0, maxA);
        }
        else if (itemID == 1) //中
        {
            countB = Mathf.Clamp(countB + amount, 0, maxB);
        }
        else if (itemID == 2) //小
        {
            countC = Mathf.Clamp(countC + amount, 0, maxC);
        }

        //UIのグレーアウト判定を呼ぶ
        int current = (itemID == 0) ? countA : (itemID == 1) ? countB : countC;
        slots[itemID].RefreshDisplay(current);

        UpdateAllUI();
    }

    //全リセット(一応)
    public void ResetAll()
    {
        countA = maxA;
        countB = maxB;
        countC = maxC;
        UpdateAllUI();
    }

    //UI表示の一括更新
    private void UpdateAllUI()
    {
        if (textA != null) textA.text = ": " + countA + " / " + maxA;
        if (textB != null) textB.text = ": " + countB + " / " + maxB;
        if (textC != null) textC.text = ": " + countC + " / " + maxC;
    }

    public void SelectSlot(int index)
    {
        selectedIndex = index;
        for (int i = 0; i < slots.Length; i++)
        {
            // 5. 選択中スロットだけ枠を表示
            slots[i].SetSelect(i == selectedIndex);
        }
    }
}
