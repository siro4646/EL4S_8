using UnityEngine;

public class ObjectBreaker : MonoBehaviour
{
    private BreakableObject currentTarget;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("ColArea"))
            return;

        var breakable = other.GetComponentInParent<BreakableObject>();
        if (breakable == null)
            return;

        // 現在ターゲットが無い、またはより近い場合更新
        if (currentTarget == null)
        {
            currentTarget = breakable;
        }
        else
        {
            float currentDist = Vector3.Distance(transform.position, currentTarget.transform.position);
            float newDist = Vector3.Distance(transform.position, breakable.transform.position);

            if (newDist < currentDist)
            {
                currentTarget = breakable;
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
    public void BreakNearest()
    {
        if (currentTarget != null)
        {
            currentTarget.Break(
                currentTarget.transform.position,
                Vector3.left
            );
        }
    }
}
