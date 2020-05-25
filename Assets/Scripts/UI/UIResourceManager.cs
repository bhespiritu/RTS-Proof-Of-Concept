using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceManager : MonoBehaviour
{
    public Slider energySlider;
    public Player player;

    private void Start()
    {
        RoundTimeManager.OnRoundTick += onTick;
    }
    void onTick()
    {
        energySlider.maxValue = player.getMaxEnergy();
        energySlider.SetValueWithoutNotify(player.getEnergy());
    }
}
