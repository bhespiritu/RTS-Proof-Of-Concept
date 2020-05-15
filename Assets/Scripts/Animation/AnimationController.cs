using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Vector3 lastPosition;

    public float cycleProgress = 0;

    public float cycleLength = 25;

    public Animation testAnimation;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y = 0;
        Vector3 diff = currentPosition - lastPosition;

        cycleProgress += diff.sqrMagnitude;
        cycleProgress %= cycleLength;

        Vector3 newPosition = transform.position;
        newPosition.y = 1 + 0.5f*Mathf.Sin(2 * Mathf.PI * (cycleProgress / cycleLength));
        transform.position = newPosition;
        

        lastPosition = transform.position;
        lastPosition.y = 0;
    }
}
