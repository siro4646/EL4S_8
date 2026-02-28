using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    [SerializeField] private Transform arrowTransform;

    public void SetTarget(Transform target)
    {
        if (target == null)
        {
            arrowTransform.gameObject.SetActive(false);
            return;
        }

        arrowTransform.gameObject.SetActive(true);

        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // 水平だけ向ける場合

        arrowTransform.rotation = Quaternion.LookRotation(direction);
    }
}
