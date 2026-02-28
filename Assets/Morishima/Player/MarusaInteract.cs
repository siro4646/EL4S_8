using UnityEngine;

[RequireComponent(typeof(ObjectBreaker))]

public class MarusaInteract : MonoBehaviour
{
    ObjectBreaker objectBreaker;
    [SerializeField] private KeyCode breakObjectKey = KeyCode.B;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

            objectBreaker = GetComponent<ObjectBreaker>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(breakObjectKey))
        {
            objectBreaker.BreakNearest();
        }
    }
}
