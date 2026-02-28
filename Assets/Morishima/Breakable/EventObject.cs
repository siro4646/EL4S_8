using UnityEngine;

public class EventObject : MonoBehaviour
{
    public enum HiddenType
    {
        None,
        Real,
        Dummy
    }

    [SerializeField] private HiddenType hiddenType = HiddenType.None;

    public HiddenType CurrentHiddenType => hiddenType;

    BreakableObject breakableObject;

    [Header("Objects")]
    public GameObject realObject;
    public GameObject dummyObject;

    void OnEnable()
    {
        breakableObject = GetComponent<BreakableObject>();
        if (breakableObject != null)
        {
            breakableObject.OnBreakTriggered += HandleBreak;
        }
    }

    void OnDisable()
    {
        if (breakableObject != null)
        {
            breakableObject.OnBreakTriggered -= HandleBreak;
        }
    }

    // 🎯 隠す対象を指定
    public bool Hide(HiddenType type)
    {
        if (hiddenType != HiddenType.None)
        {
            return false; // すでに隠れている場合は無視
        }
        hiddenType = type;
        return true;
    }

    void HandleBreak(BreakableObject obj)
    {
        if (obj != breakableObject)
            return;

        Debug.Log("破壊イベント受信: " + obj.name);

        switch (hiddenType)
        {
            case HiddenType.Real:
                Instantiate(realObject, transform.position, Quaternion.identity);
                break;

            case HiddenType.Dummy:
                Instantiate(dummyObject, transform.position, Quaternion.identity);
                break;

            case HiddenType.None:
            default:
                break;
        }

        hiddenType = HiddenType.None; // リセット
    }

    public void OnSonarDetected()
    {
        // 例：光らせる
        Debug.Log("ソナーに反応: " + name);

        // エフェクトやアウトラインを有効化など
    }
}
