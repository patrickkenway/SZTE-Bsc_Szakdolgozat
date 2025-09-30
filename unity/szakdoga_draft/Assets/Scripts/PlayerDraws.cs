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

            if (match.name == "o")
            {
                playerShoot = GetComponent<PlayerShoot>();
                playerShoot.Shoot();
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
