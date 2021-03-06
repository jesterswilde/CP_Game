﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class PlayerInput {

    public static bool[] GetAbsKeyboardState()
    {
        return new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.D),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftShift),
            Input.GetKey(KeyCode.E)
        };
    }
    public static void CheckSpeedButtons()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameManager.SwitchCharacter();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameManager.ShowCtrlMenu(true); 
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            GameManager.ShowCtrlMenu(false); 
        }
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift) && GameManager.ActiveCharacter == null 
            || Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift) && GameManager.ActiveCharacter != null 
            && GameManager.ActiveCharacter.CanRewind)
        {
            GameManager.SetSpeed(GameSettings.RewindSpeed);
        }
        if (Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.LeftShift)
            || Input.GetMouseButton(0) && Input.GetKeyUp(KeyCode.LeftShift))
        {
            GameManager.SetSpeed(GameSettings.ForwardSpeed);
        }
        if (Input.GetMouseButtonDown(1) && GameManager.ActiveCharacter != null 
            && Input.GetKey(KeyCode.LeftShift) && GameManager.ActiveCharacter.CanDeleteFuture)
        {
            GameManager.ActiveCharacter.DeleteFuture();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.GameSpeed != 0)
            {
                GameManager.SetSpeed(0);
            }
            else
            {
                GameManager.SetSpeed(GameSettings.ForwardSpeed);
            }
        }
    }
    public static Action ActionButtons(Character _activeCharacter)
    {
        if(_activeCharacter != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                return _activeCharacter.CreateTargetAction(); 
            }
        }
        return null; 
    }

    public static Action GetActionInputs()
    {
        if (GameManager.ActiveCharacter.WillRotate())
        {
            return new ValueAction(ActionType.Rotation, GameManager.ActiveCharacter.RotationDifference());
        }
        return null; 
    }
}
