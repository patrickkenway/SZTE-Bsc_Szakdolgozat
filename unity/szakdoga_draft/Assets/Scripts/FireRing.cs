using UnityEngine;

public class FireRing : MonoBehaviour
{
    private Vector3 target;
    private float projectileMoveSpeed;
    public int projectileDamage = 1;

    private bool reachedTarget = false;
    private float stayTime = 500f; // Mennyi ideig maradjon ott
    private float stayTimer;

    private void Update()
    {
        if (!reachedTarget)
        {
            Vector3 moveDirNormalized = (target - transform.position).normalized;
            transform.position += moveDirNormalized * projectileMoveSpeed * Time.deltaTime;

            // Ha elérte a célt (kis távolságon belül)
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                reachedTarget = true;
                stayTimer = stayTime;
            }
        }
        else
        {
            // Ott marad egy ideig
            stayTimer -= Time.deltaTime;
            if (stayTimer <= 0f)
            {
                //Destroy(gameObject);
            }
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
    public void InitializeProjectile(Vector3 target, float projectileMoveSpeed)
    {
        this.target = target;
        this.projectileMoveSpeed = projectileMoveSpeed;

    }
}
