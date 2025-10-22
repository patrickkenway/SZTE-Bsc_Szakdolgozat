using Unity.VisualScripting;
using UnityEngine;

public class FireRing : MonoBehaviour
{
    private Vector3 target;
    private float projectileMoveSpeed;
    public int projectileDamage = 1;
    private float stayTime = 2f; // Például 2 másodpercig marad ott
    private bool reachedTarget = false;


    void Start()
    {
        Destroy(gameObject, 10f);
    }
    private void Update()
    {
        Vector3 moveDirNormalized = (target - transform.position).normalized;
        transform.position += moveDirNormalized * projectileMoveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, 300 * Time.deltaTime); // Rotate around Z axis

        if (!reachedTarget && Vector3.Distance(transform.position, target) < 0.1f)
        {
            reachedTarget = true;
            // Optionally, you can add logic here to make the projectile stay for a while before disappearing
        }
        if (reachedTarget)
        {
            // Logic for when the projectile has reached its target, if needed
            transform.position = target; // Ensure it stays at the target position
            Destroy(gameObject, stayTime); // Destroy the projectile after reaching the target
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.TakeDamage(projectileDamage);

        }

    }
    private float damageCooldown = 1f;
    private float lastDamageTime = -1f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                enemy.TakeDamage(enemy.damage);
                lastDamageTime = Time.time;
            }
        }
    }
    public void InitializeProjectile(Vector2 target, float projectileMoveSpeed)
    {
        this.target = target;
        this.projectileMoveSpeed = projectileMoveSpeed;
        //this.stayTime = staytime; // Például 2 másodpercig marad ott
    }
}
