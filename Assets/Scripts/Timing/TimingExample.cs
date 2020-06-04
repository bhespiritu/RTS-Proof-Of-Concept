using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingExample : MonoBehaviour
{


    void OnEnable()
    {
        RoundManager.OnRoundStart += OnStart;
        RoundManager.OnRoundTick += OnTick;
    }


    void OnDisable()
    {
        RoundManager.OnRoundStart -= OnStart;
        RoundManager.OnRoundTick -= OnTick;
    }

    public void OnStart()
    {
        Debug.Log("I AM ALIVE");
    }

    public void OnTick()
    {
        gameObject.transform.position = Vector3.up * (Mathf.Sin(RoundManager.INSTANCE.currentTime) + 2);
    }

}
