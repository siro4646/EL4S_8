using UnityEngine;

[RequireComponent(typeof(HideMoney))]
[RequireComponent(typeof(ObjectBreaker))]
public class PlayerInteract : MonoBehaviour
{
    HideMoney hideMoney;
    ObjectBreaker objectBreaker;
    [SerializeField] private KeyCode hideMoneyKey = KeyCode.Space;
    [SerializeField] private KeyCode breakObjectKey = KeyCode.B;
    void Start()
    {
        hideMoney = GetComponent<HideMoney>();
        objectBreaker = GetComponent<ObjectBreaker>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(hideMoneyKey))
        {
            hideMoney.HideMoneyNearest();
        }
        if (Input.GetKeyDown(breakObjectKey))
        {
            objectBreaker.BreakNearest();
        }

    }
}
