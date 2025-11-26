using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoldToLoadLevel : MonoBehaviour
{
    public float holdDuration = 1f;
    public Image fillCricle;

    private float holdTimer = 0;
    private bool isHolding = false;

    public static event Action OnHoldComplete;


    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            fillCricle.fillAmount = holdTimer / holdDuration;
            if (holdTimer >= holdDuration)
            {
                //load level
                OnHoldComplete.Invoke();
                ResetHold();
            }
        }
    }

    public void OnHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isHolding = true;
        }
        else if (context.canceled)
        {
            //reset holding
            ResetHold();
        }

    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0;
        fillCricle.fillAmount = 0;
    }
}
