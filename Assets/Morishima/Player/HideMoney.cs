using UnityEngine;

public class HideMoney : MonoBehaviour
{
    private EventObject currentTarget;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("ColArea"))
            return;

        var eventObject = other.GetComponentInParent<EventObject>();
        if (eventObject == null)
            return;

        // 現在ターゲットが無い、またはより近い場合更新
        if (currentTarget == null)
        {
            currentTarget = eventObject;
        }
        else
        {
            float currentDist = Vector3.Distance(transform.position, currentTarget.transform.position);
            float newDist = Vector3.Distance(transform.position, eventObject.transform.position);

            if (newDist < currentDist)
            {
                currentTarget = eventObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("ColArea"))
            return;

        var breakable = other.GetComponentInParent<BreakableObject>();
        if (breakable == currentTarget)
        {
            currentTarget = null;
        }
    }
    // 🎯 外部から呼び出す用
    public void HideMoneyNearest()
    {
        if (currentTarget != null)
        {
            currentTarget.IsHidden = true;
        }
    }
}
