using UnityEngine;

public class PlayerDraws : MonoBehaviour
{
    [SerializeField] private DrawingRecognition drawingRecognition; // reference to the DrawingRecognition script
    public Character match; // For storing the match
    private void CheckDrawRecControls()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            match = drawingRecognition.GetMatch(); // Stores match & prints name to debug console 
            drawingRecognition.ClearDrawing();
        }

    }


    // Update is called once per frame
    void Update()
    {
        CheckDrawRecControls();
    }   
}
