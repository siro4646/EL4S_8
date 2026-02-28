using System.Collections.Generic;
using UnityEngine;

public class HiddenSearcher : MonoBehaviour
{
    [SerializeField] private float searchRadius = 5f;
    [SerializeField] private LayerMask searchLayer;

    public void EmitSonar()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            searchRadius,
            searchLayer
        );

        foreach (var hit in hits)
        {
            var eventObj = hit.GetComponentInParent<EventObject>();
            if (eventObj == null)
                continue;

            if (eventObj.CurrentHiddenType != EventObject.HiddenType.None)
            {
                Debug.Log("ソナー検出: " + eventObj.name);

                // 🔥 検出された側に通知
                eventObj.OnSonarDetected();
            }
        }
    }

    public List<EventObject> GetNearbyHiddenObjects()
    {
        List<EventObject> result = new List<EventObject>();

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            searchRadius,
            searchLayer
        );

        foreach (var hit in hits)
        {
            var eventObj = hit.GetComponentInParent<EventObject>();
            if (eventObj == null)
                continue;

            // Real または Dummy だけ取得
            if (eventObj.CurrentHiddenType == EventObject.HiddenType.Real ||
                eventObj.CurrentHiddenType == EventObject.HiddenType.Dummy)
            {
                if (!result.Contains(eventObj))
                    result.Add(eventObj);
            }
        }

        return result;
    }


    public EventObject GetNearestHiddenObject()
    {
        EventObject nearest = null;
        float minDist = float.MaxValue;

        foreach (var obj in GetNearbyHiddenObjects())
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = obj;
            }
        }

        return nearest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
