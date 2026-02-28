using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObjectBreaker))]
[RequireComponent(typeof(HiddenSearcher))]
public class MarusaInteract : MonoBehaviour
{
    ObjectBreaker objectBreaker;
    HiddenSearcher hiddenSearcher;

    [SerializeField] private KeyCode breakObjectKey = KeyCode.B;
    [SerializeField] private KeyCode echoHiddenObjectKey = KeyCode.E;

    [SerializeField] private ArrowIndicator arrow;

    [SerializeField] private float arrowDisplayTime = 3f;

    [Header("Echo CoolTime")]
    [SerializeField] private float echoCooldown = 5f;

    private float nextEchoAvailableTime = 0f;

    Coroutine arrowCoroutine;

    void Start()
    {
        objectBreaker = GetComponent<ObjectBreaker>();
        hiddenSearcher = GetComponent<HiddenSearcher>();
        arrow.SetTarget(null);
    }

    void Update()
    {
        if (Input.GetKeyDown(breakObjectKey))
        {
            objectBreaker.BreakNearest();
        }

        if (Input.GetKeyDown(echoHiddenObjectKey))
        {
            TryUseEcho();
        }
    }

    void TryUseEcho()
    {
        // ⛔ CTチェック
        if (Time.time < nextEchoAvailableTime)
        {
            Debug.Log("Echo CT中");
            return;
        }

        nextEchoAvailableTime = Time.time + echoCooldown;

        ShowArrowTemporarily();
    }

    void ShowArrowTemporarily()
    {
        EventObject nearest = hiddenSearcher.GetNearestHiddenObject();

        if (nearest == null)
        {
            arrow.SetTarget(null);
            return;
        }

        arrow.SetTarget(nearest.transform);

        if (arrowCoroutine != null)
        {
            StopCoroutine(arrowCoroutine);
        }

        arrowCoroutine = StartCoroutine(HideArrowAfterTime());
    }

    IEnumerator HideArrowAfterTime()
    {
        yield return new WaitForSeconds(arrowDisplayTime);

        arrow.SetTarget(null);
        arrowCoroutine = null;
    }
}
