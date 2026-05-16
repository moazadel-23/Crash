using UnityEngine;
using UnityEngine.SceneManagement;

public enum Controls { mobile, pc }

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float doubleJumpForce = 8f;

    [Header("Detection")]
    public LayerMask groundLayer;
    public Transform groundCheck;

    private Rigidbody2D rb;
    private bool isGroundedBool = false;
    private bool canDoubleJump = false;

    [Header("References")]
    public Animator playeranim;
    public Controls controlmode;

    private float moveX;
    [HideInInspector] public bool isPaused = false;
    private bool isDead = false;

    [Header("Effects")]
    public ParticleSystem footsteps;
    public ParticleSystem ImpactEffect;
    private bool wasonGround;

    [Header("Combat")]
    public GameObject projectile;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isDead = false;
        if (controlmode == Controls.mobile && UIManager.instance != null)
            UIManager.instance.EnableMobileControls();
    }

    private void Update()
    {
        if (isPaused || isDead) return;
        isGroundedBool = IsGrounded();
        HandleInput();
        if (controlmode == Controls.pc && Input.GetButtonDown("Fire1") && Time.time >= nextFireTime) Shoot();
        SetAnimations();
        if (moveX != 0) FlipSprite(moveX);
        if (!wasonGround && isGroundedBool && ImpactEffect != null)
        {
            ImpactEffect.gameObject.SetActive(true);
            ImpactEffect.Stop();
            ImpactEffect.Play();
        }
        wasonGround = isGroundedBool;
    }

    private void HandleInput()
    {
        if (controlmode == Controls.pc) moveX = Input.GetAxisRaw("Horizontal");
        if (isGroundedBool)
        {
            canDoubleJump = true;
            if (Input.GetButtonDown("Jump")) Jump(jumpForce, "jump");
        }
        else if (canDoubleJump && Input.GetButtonDown("Jump"))
        {
            Jump(doubleJumpForce, "doubleJump");
            canDoubleJump = false;
        }
    }

    private void Jump(float force, string animationTrigger)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        if (playeranim != null) playeranim.SetTrigger(animationTrigger);
    }

    public void SetAnimations()
    {
        if (playeranim == null) return;
        bool isRunning = Mathf.Abs(moveX) > 0.1f && isGroundedBool;
        playeranim.SetBool("run", isRunning);
        if (footsteps != null)
        {
            var emission = footsteps.emission;
            emission.rateOverTime = isRunning ? 35f : 0f;
        }
    }

    private void FlipSprite(float direction)
    {
        if (direction > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (direction < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FixedUpdate()
    {
        if (!isDead) rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Shoot()
    {
        if (projectile != null && firePoint != null && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.identity);
            bullet.transform.localScale = new Vector3(direction, 1f, 1f);
            if (playeranim != null) playeranim.SetTrigger("shoot");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("KillZone") || other.CompareTag("Zombie")) && !isDead)
        {
            // لا نجعل isDead = true هنا مباشرة، بل نترك Die تقرر
            if (HealthManager.instance != null) HealthManager.instance.LoseHeart();
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return; // منع تكرار الموت
        isDead = true;

        if (playeranim != null) playeranim.SetTrigger("die");

        if (GetComponent<Collider2D>() != null) GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;

        // شرط مهم جداً: لو لسه فيه قلوب، عيد المشهد. لو مفيش، GameManager هيشغل اللوحة.
        if (HealthManager.instance != null && HealthManager.instance.currentHealth > 0)
        {
            Invoke("ReloadLevel", 0.6f);
        }
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}