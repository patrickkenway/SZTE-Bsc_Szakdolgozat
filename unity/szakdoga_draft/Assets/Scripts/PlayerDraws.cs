using UnityEngine;

public class PlayerDraws : MonoBehaviour
{
    [SerializeField] private DrawingRecognition drawingRecognition; // reference to the DrawingRecognition script
    public Character match; // For storing the match
    [SerializeField] private PlayerShoot playerShoot; // Reference to PlayerShoot script

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
                case "square":
                    Debug.Log("Square drawn!");
                    break;
                case "triangle":
                    Debug.Log("Triangle drawn!");
                    break;
                default:
                    Debug.Log("No match found.");
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
