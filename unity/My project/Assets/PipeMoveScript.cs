using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed = 7;
    public float deadZone = -60;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;
        if (transform.position.x < deadZone) {
            Debug.Log("pipe deleted");
            Destroy(gameObject);
        }
    }
}
