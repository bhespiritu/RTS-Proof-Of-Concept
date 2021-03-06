﻿// Author: Kevin Mulliss (kam8ef@virginia.edu)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public ulong uID = ulong.MaxValue;

    [Header("General Attributes")]
    public string name;
    public int health;
    public int healthregen;
    public static float moveSpeed;
    public static float turnSpeed;
    public int killCount;
    public int veterancy;
    public static float visionRadius;
    public static float radarRadius;
    public static float sonarRadius;
    public int armor;
    // Heat produced, negative if it takes heat from surroundings\
    [Header("Heat Attributes")]
    public int heatProd;
    public int currentHeat;
    public int heatMin;
    public int heatMax;
    public int massCost;
    public int energyCost;
    public int massCostPerTick;
    public int energyCostPerTick;
    public int massProd;
    public int energyProd;

    // Weapon information, need this for each weapon
    [Header("Weapon Attributes")]
    public float projectileDmg;
    public float projectileSpd;
    public int projectileHeat;
    public float tolerance;
    public float reloadTime;
    public float weaponRange;
    public int selfHeat;

    //Author Noah Espiritu
    [HideInInspector]
    public UnitGroup associatedPathfindingGroup;

    // Start is called before the first frame update
    void Start()
    {
        //REMOVE LATER
        if (uID == ulong.MaxValue) UnitManager.INSTANCE.AddUnit(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //I'm going to start adding things I think we need. Feel free to correct 
    //@Author Roger Clanton
    void takeDamage(int dmg){
        health -= dmg/armor;
        if (health <= 0){
            unitDeath();
        }

    }

    void heatChange(int heat)
    {
        currentHeat += heat;
        if (currentHeat < heatMin)
        {
            unitDeath();
        }
        if (currentHeat > heatMax)
        {
            unitDeath();
        }
    }

    void regenerateHealth(int healthregen)
    {
        health += healthregen;
    }

    //Destroys the unit. Notifies unit manager, notifies killer for veterancy purposes
    void unitDeath()
    {
        UnitManager.INSTANCE.unitLookup.Remove(uID);
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        unitDeath();
    }

    public int getMCPT()
    {
        return massCostPerTick;
    }

    public int getECPT()
    {
        return energyCostPerTick;
    }

    public int getMCTot()
    {
        return massCost;
    }

    public int getECTot()
    {
        return energyCost;
    }
}
