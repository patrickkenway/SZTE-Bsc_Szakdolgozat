using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{
    public GameObject fireRingPrefab;
    public float fireRingSpeed = 1.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();

        } //Left click
    }

    public void Shoot()
    {

        //Get mouse pos
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        //direction from us to mouse
        //Vector3 shootDirection = (mousePosition - transform.position).normalized;

        FireRing fireRing = Instantiate(fireRingPrefab, transform.position, Quaternion.identity).GetComponent<FireRing>();

        fireRing.InitializeProjectile(mousePosition, fireRingSpeed);
        fireRing.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground"; // Set the sorting layer to "Foreground" azért hogy ne menjen be a sprite a falak mögé
        fireRing.GetComponent<SpriteRenderer>().sortingOrder = 6; // Magas szam legyen hogy mindig elöl legyen



    }
}
