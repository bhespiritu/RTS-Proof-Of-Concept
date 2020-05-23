using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingExample : MonoBehaviour
{

    void OnEnable()
    {
        RoundTimeManager.OnRoundStart += OnStart;
        RoundTimeManager.OnRoundTick += OnTick;
    }


    void OnDisable()
    {
        RoundTimeManager.OnRoundStart -= OnStart;
        RoundTimeManager.OnRoundTick -= OnTick;
    }

    public void OnStart()
    {
        Debug.Log("I AM ALIVE");
    }

    public void OnTick()
    {
        gameObject.transform.position = Vector3.up * (Mathf.Sin(RoundTimeManager.INSTANCE.currentTime) + 2);
    }

}
