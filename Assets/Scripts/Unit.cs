using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public int health;

    public static float moveSpeed;
    public static float turnSpeed;
    public static float energyConsumption;
    public static float visionRadius;
    public static float radarRadius;
    public int killCount;

    //Weapon information need this for each weapon
    public float dps;
    public float projectileDmg;
    public float reloadTime;
    public float WeaponRange;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //I'm going to start adding things I think we need. Feel free to correct 
    //@Author Roger Clanton
    void takeDamage(int dmg){
        health -= dmg;
        if (health <= 0){
            unitDeath();
        }
    }

    //Destroys the unit. Notifies unit manager
    void unitDeath(){

    }


}
