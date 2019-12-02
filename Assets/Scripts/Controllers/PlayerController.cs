﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : AbstractController
{

    // Events

    public void OnPause()
    {
        gameManager.Pause();
    }

    public void OnVerticalAttack()
    {
        State.OnVerticalAttack();
    }

    public void OnDashAttack()
    {
        State.OnDashAttack();
    }

    public void OnMovement(InputValue value)
    {
        State.OnMovement(value.Get<float>());
    }

    public void OnBackDash()
    {
        State.OnBackDash();
    }

    public void OnDeviceLostEvent()
    {
        // TODO
    }
    
    public void OnDeviceRegainedEvent()
    {
        // TODO
    }
}
