using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    public float chaseSpeed = 2f;
    public float jumpForce = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;

    public int damage = 1;

    public int maxHealth = 3;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color ogColor;

    //Loot table
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        ogColor = spriteRenderer.color;
    }

    void Update()
    {
        //is grounded
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        //player direction
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        //player above detection
        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 5f, 1 << player.gameObject.layer);

        if (isGrounded)
        {
            //chase player
            rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);

            //jump if theres gap ahead &&no ground infront
            //else if theres player above and platform above

            //if ground
            RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
            //if gap
            RaycastHit2D gapAhead = Physics2D.Raycast(transform.position+new Vector3(direction,0,0),Vector2.down, 2f, groundLayer);
            //if platform
            RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 5f, groundLayer);

            if(!groundInFront.collider && !gapAhead.collider)
            {
                shouldJump = true;
            }
            else if(isPlayerAbove && platformAbove.collider)
            {
                shouldJump = true;
            }
        }
    }
    private void FixedUpdate()
    {
        if (isGrounded && shouldJump)
        {
            shouldJump = false;
            Vector2 direction = (player.position - transform.position).normalized;

            Vector2 jumpDirection = direction * jumpForce;
            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        StartCoroutine(FlashWhite());
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashWhite()
    {
        ogColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = ogColor;
    }

    void Die()
    {
        //Go around lootable
        //Spawn item
        foreach(LootItem lootItem in lootTable)
        {
            if(Random.Range(0f,100f) <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemPrefab);
                break;
            }
            
        }

        Destroy(gameObject);
    }

    void InstantiateLoot(GameObject loot)
    {
        if(loot)
        {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);

            droppedLoot.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
