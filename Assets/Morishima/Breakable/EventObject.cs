using UnityEngine;

public class EventObject : MonoBehaviour
{
    bool _isHidden = false;
    public bool IsHidden
    {
        get => _isHidden;
        set
        {
            _isHidden = value;
        }
    }
    BreakableObject breakableObject;

    public GameObject okaneObj;
    public void Hide()
    {
        IsHidden = true;
    }

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

    void HandleBreak(BreakableObject obj)
    {
        if (obj == breakableObject)
        {
            Debug.Log("破壊イベント受信: " + obj.name);
            if(IsHidden)
            {
                Instantiate(okaneObj, transform.position, Quaternion.identity);
            }
        }
    }
}
