using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Fire : MonoBehaviour
{
    public Light2D light;
    public float turnOnOffLightDuration = 2f;

    private bool turnOn;
    private bool turnOff;
    private float timer;

    private void Update()
    {
        if (turnOn && timer < turnOnOffLightDuration)
        {
            timer = Mathf.Min(timer + Time.deltaTime, turnOnOffLightDuration);
            light.intensity = timer / turnOnOffLightDuration;
        }
        else
        {
            turnOn = false;
        }

        if (turnOff && timer > 0)
        {
            timer = Mathf.Max(timer - Time.deltaTime, 0);
            light.intensity = timer / turnOnOffLightDuration;
        }
        else
        {
            turnOff = false;
        }
    }

    public void TurnOnLight()
    {
        turnOff = false;
        turnOn = true;
    }

    public void TurnOffLight()
    {
        turnOn = false;
        turnOff = true;
    }
}
