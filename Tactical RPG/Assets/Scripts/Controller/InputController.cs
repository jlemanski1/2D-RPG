using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic input handler for multi-input gameplay
/// </summary>
public class InputController : MonoBehaviour {

    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");

    // Move Event
    public static event EventHandler<InfoEventArgs<Point>> moveEvent;

    // Fire Event
    public static event EventHandler<InfoEventArgs<int>> fireEvent;

    // Fire buttons
    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };



    private void Update() {
        // Update Input Repeaters
        int x = _hor.Update();
        int y = _ver.Update();

        // Player is moving
        if (x != 0 || y != 0) {
            if (moveEvent != null) {
                // Tie in Event with Unity's update loop
                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
            }
        }

        // Check each fire button
        for (int i = 0; i < 3; i++) {
            if (Input.GetButtonUp(_buttons[i])) {
                if (fireEvent != null)
                    // Fire Event
                    fireEvent(this, new InfoEventArgs<int>(i));
            }
        }
    }

}

/// <summary>
/// Allows for repeated pressing of the inputs for smooth movement while also allowing 
/// for single warping to tile type movement
/// 
/// Only used by InputController so it's gonna just stay in the same file
/// </summary>
class Repeater {

    const float threshold = 0.5f;       // Amount to wait between initial & repeated press
    const float rate = 0.25f;           // Rate at which the input will repeat

    float _next;    // Target point in time to repeat, intially 0
    bool _hold;     // Indicates whether button is held
    string _axis;   // Axis being pressed


    public Repeater(string axisName) {
        _axis = axisName;
    }

    public int Update() {
        int result = 0;
        int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));

        // Button is being held
        if (value != 0) {
            // Sufficient time has passed
            if (Time.time > _next) {
                result = value;
                _next = Time.time + (_hold ? rate : threshold);
                _hold = true;
            }
        }
        // Button was just pressed
        else {
            _hold = false;
            _next = 0;
        }
        return result;
    }


}
