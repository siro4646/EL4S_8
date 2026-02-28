using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Image itemImage;      // アイテムのメイン画像
    public Image selectFrame;    // 裏側に置いた選択中テクスチャ
    public int currentCount;     // このアイテムの残り数

    // 1. 選択状態を切り替える (外部から呼ぶ)
    public void SetSelect(bool isSelected)
    {
        selectFrame.enabled = isSelected; // 選択中なら表示、そうでないなら非表示
    }

    // 2. 表示を更新する (個数が変わるたびに呼ぶ)
    public void RefreshDisplay(int count)
    {
        currentCount = count;

        if (currentCount <= 0)
        {
            // 3. 0個なら灰色にする
            itemImage.color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
        }
        else
        {
            // 4. 1個以上なら元の色（白）に戻す
            itemImage.color = Color.white;
        }
    }

}
