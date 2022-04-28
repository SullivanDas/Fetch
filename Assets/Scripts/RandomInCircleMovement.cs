using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInCircleMovement : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float speed;
    [SerializeField] private float minWaitTime = 0.2f;
    [SerializeField] private float maxWaitTime = 2f;
    private Vector2 prevDestination;
    private Vector2 nextDestination;
    private Vector2 center;
    private float t = 0;
    private bool isWaiting;
    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
        prevDestination = center;
        nextDestination = GetNextPos();

    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting)
        {
            transform.position = Vector2.Lerp(prevDestination, nextDestination, t);
            t += Time.deltaTime;
            if (t > 1)
            {
                t = 0;
                prevDestination = nextDestination;
                nextDestination = GetNextPos();
                StartCoroutine(WaitRandom());
                isWaiting = true;
            }
        }

    }

    private Vector2 GetNextPos()
    {
        Vector2 next = (Vector2)transform.position + (Random.insideUnitCircle * Random.Range(radius/3, radius/1.5f));
        next = next.magnitude - center.magnitude > radius ? next = center : next;
        return next;
    }

    private IEnumerator WaitRandom()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime,maxWaitTime));
        isWaiting = false;
    }
}
