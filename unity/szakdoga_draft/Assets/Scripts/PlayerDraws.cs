using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerDraws : MonoBehaviour
{
    [SerializeField] private DrawingRecognition drawingRecognition; // reference to the DrawingRecognition script
    public Character match; // For storing the match
    private PlayerShoot playerShoot; // Reference to PlayerShoot script
    public static event Action StarDrawn;


    private void CheckDrawRecControls()
    {

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            match = drawingRecognition.GetMatch(); // Stores match & prints name to debug console 

            switch (match.name)
            {
                case "circle":
                    playerShoot = GetComponent<PlayerShoot>();
                    playerShoot.Shoot();
                    break;
                case "star":
                    if (GameController.progressAmount >= 100) { StarDrawn.Invoke(); } else { Debug.Log("„You do not have enough point to progress!” "); }
                    break;
                case "triangle":
                    Debug.Log("Triangle drawn!");
                    break;
            }
            drawingRecognition.ClearDrawing();
        }

    }
    // Update is called once per frame
    void Update()
    {
        CheckDrawRecControls();
    }

}
