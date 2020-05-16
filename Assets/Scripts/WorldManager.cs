using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{

    public static WorldManager INSTANCE;

    List<Player> players = new List<Player>();

    private float _startTime;
    private float _currentTime;

    public delegate void StartEvent();
    public static event StartEvent OnRoundStart;

    public delegate void TickEvent();
    public static event TickEvent OnRoundTick;

    public bool isPaused = false;

    public WorldManager()
    {
        
    }

    public void Awake()
    {
        if (!INSTANCE)
        {
            INSTANCE = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("There already exists a WorldManager in this world!");
        }
    }

    public void FixedUpdate()
    {
        if (!isPaused)
        {
            _currentTime += Time.fixedDeltaTime;
            OnRoundTick?.Invoke();
        }
    }

    public void StartRound()
    {
        _startTime = 0;
        _currentTime = 0;

        OnRoundStart?.Invoke();
    }


    public float startTime
    {
        get
        {
            return _startTime;
        }
    }

    public float currentTime
    {
        get
        {
            return _currentTime;
        }
    }

}
