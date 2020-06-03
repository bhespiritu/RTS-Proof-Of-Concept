using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Author: Noah Espiritu
/*
 * The game is going to be working on 3 different synchronized loops
 * - Visual Loop
 *   - This is the loop for anything that completely independent from any game logic (i.e UI, visual interpolation)
 *   - You use this loop by using the Update/FixedUpdate
 * - Game Loop
 *   - This is the loop acts as the base time system for any internal game logic. For testing's sake, it is set to 20 fps. We can discuss what to set it to later
 *   - Access this loop by using OnRoundStart/OnRoundTick
 * - Input Loop
 *   - The loop at which user input is processed
 *   - Player input isn't processed right as it comes in. It is instead accumulated over time and then all executed at once every 4 Game Loop ticks. 
 *   - Will be accessible through the AddAction method
 * */

public class RoundManager : MonoBehaviour
{
    #region Fields
    public static RoundManager INSTANCE;



    List<Player> players = new List<Player>();

    public readonly float TICK_LENGTH = .05f;
    private float _accumulatedTime = 0;

    private float _startTime;
    private float _currentTime;
    private uint _tickCount;
    private uint _inputTickCount;

    public bool isPaused = false;

    public delegate void StartEvent();
    public static event StartEvent OnRoundStart;

    public delegate void TickEvent();
    public static event TickEvent OnRoundTick;

    private ActionSystem actionSystem;

    public BuildingManager buildingManager { get; private set; }
    public UnitManager unitManager { get; private set; }

    private ulong uIDCount = 0;
    #endregion

    public RoundManager()
    {
        actionSystem = new ActionSystem(this);
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
            Debug.LogError("There already exists a RoundTimeManager in this world!");
        }
    }

    public void Update()
    {
        if (!isPaused)
        {
            _accumulatedTime += Time.deltaTime;
            while(_accumulatedTime > TICK_LENGTH)
            {
                RoundTick();
                _accumulatedTime -= TICK_LENGTH;
            }
        }
    }


    private int subTick = 0;
    private int TicksPerInputStep = 4;

    public void StartRound()
    {
        _startTime = Time.time;
        _currentTime = 0;
        _tickCount = 0;
        _inputTickCount = 0;

        OnRoundStart?.Invoke();
    }

    public void RoundTick()
    {
        if (subTick == 0)//process inputs
        {
            if (InputTick())
            {
                subTick++;
                OnRoundTick?.Invoke();
            }
        }
        else //update main game cycle
        {
            _currentTime += TICK_LENGTH;
            _tickCount++;
            OnRoundTick?.Invoke();
            
            subTick++;
            if (subTick == TicksPerInputStep)
            {
                subTick = 0;
            }
        }
    }

    public bool InputTick()
    {

        bool canMoveOn = CanDoNextStep();

        if(canMoveOn)
        {
            _inputTickCount++;
            actionSystem.performAction();
        }

        return canMoveOn;
    }

    public bool CanDoNextStep()
    {
        //TODO add in any logic that should be checked before advancing time
        //This isn't needed yet because there aren't multiple players yet.
        return true;
    }

    #region Getters and Setters
    public float startTime
    {
        get
        {
            return _startTime;
        }
    }

    public float currentTime//Time since start in terms of Game Loop ticks
    {
        get
        {
            return _currentTime;
        }
    }

    public float tickCount
    {
        get
        {
            return _tickCount;
        }
    }

    public float inputTickCount
    {
        get
        {
            return _inputTickCount;
        }
    }
    

    public float timeSinceLastTick
    {
        get
        {
            return Time.time - (startTime + currentTime);
        }
    }

    public float timeSinceStart//Time since start in terms of Display Loop ticks
    {
        get
        {
            return Time.time - startTime;
        }
    }

    public ulong requestUID => uIDCount++;//Placeholder code
    #endregion

}
