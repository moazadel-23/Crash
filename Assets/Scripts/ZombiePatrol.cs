using UnityEngine;

public class ZombiePatrol : MonoBehaviour
{
    [Header("إعدادات الحركة")]
    public float speed = 2f;         // سرعة مشي الزومبي
    public float range = 3f;         // المسافة اللي هيمشيها (يمين وشمال نقطة البداية)

    private Vector3 startPosition;   // النقطة اللي الزومبي بدأ منها
    private int direction = 1;       // 1 يعني رايح يمين، -1 يعني رايح شمال
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // حفظ مكان الزومبي أول ما اللعبة تبدأ عشان يفضل يتحرك حواليه
        startPosition = transform.position;

        // الوصول لمكون الصورة عشان نقدر نلفها
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 1. حساب المسافة بين مكان الزومبي الحالي ونقطة بدايته
        float distanceFromStart = transform.position.x - startPosition.x;

        // 2. لو الزومبي عدى المسافة المسموحة (الـ range) يمين أو شمال
        if (Mathf.Abs(distanceFromStart) >= range)
        {
            // اعكس الاتجاه (لو 1 يبقى -1، ولو -1 يبقى 1)
            direction *= -1;

            // استدعاء وظيفة لف الصورة
            FlipZombie();
        }

        // 3. كود الحركة الفعلي
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    void FlipZombie()
    {
        // بناءً على صورة الزومبي اللي باصة شمال في الأصل:

        if (direction == 1) // لو ماشي يمين
        {
            // فعل الـ FlipX عشان نخليه يبص يمين
            spriteRenderer.flipX = true;
        }
        else // لو ماشي شمال
        {
            // الغي الـ FlipX يرجع يبص شمال (أصله)
            spriteRenderer.flipX = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (HealthManager.instance != null)
            {
                HealthManager.instance.LoseHeart(); // دي اللي هتنقص الرقم في الـ Inspector
                Debug.Log("الزومبي خبط اللاعب!");
            }
        }
    }
}