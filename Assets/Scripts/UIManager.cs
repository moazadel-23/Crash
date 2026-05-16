using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Mobile Controls")]
    public GameObject mobileControls;

    [Header("Fade Settings")]
    public bool fadeToBlack;
    public bool fadeFromBlack;
    public Image blackScreen;
    public float fadeSpeed = 2f;

    [Header("Health UI")]
    // اسحب صور القلوب الثلاثة هنا في الـ Inspector بالترتيب
    public Image[] hearts;

    private void Awake()
    {
        // تحديث الـ instance في كل مرة يحمل فيها المشهد لضمان التواصل مع الـ HealthManager
        instance = this;
    }

    private void Start()
    {
        if (blackScreen != null)
        {
            // نضمن أن الشاشة السوداء تبدأ شفافة تماماً
            blackScreen.color = new Color(0, 0, 0, 0);
        }

        // تحديث حالة القلوب فور بداية المشهد بناءً على بيانات الـ HealthManager
        if (HealthManager.instance != null)
        {
            UpdateHearts(HealthManager.instance.currentHealth);
        }

        // تفعيل تحكم الموبايل تلقائياً إذا لزم الأمر
        EnableMobileControls();
    }

    private void Update()
    {
        HandleFadeEffect();
    }

    // --- وظيفة تحديث القلوب (تظهر وتختفي حسب الصحة) ---
    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                // إذا كان رقم القلب (i) أقل من الصحة الحالية يظهر، وإلا يختفي
                hearts[i].gameObject.SetActive(i < currentHealth);
            }
        }
    }

    // --- وظائف التلاشي (Fade System) ---
    private void HandleFadeEffect()
    {
        if (blackScreen == null) return;

        if (fadeToBlack)
        {
            Fade(1f);
            if (blackScreen.color.a >= 1f) fadeToBlack = false;
        }
        else if (fadeFromBlack)
        {
            Fade(0f);
            if (blackScreen.color.a <= 0f) fadeFromBlack = false;
        }
    }

    private void Fade(float targetAlpha)
    {
        Color color = blackScreen.color;
        float newAlpha = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
        blackScreen.color = new Color(color.r, color.g, color.b, newAlpha);
    }

    // --- وظائف التحكم في الموبايل ---
    public void EnableMobileControls()
    {
        if (mobileControls != null) mobileControls.SetActive(true);
    }

    public void DisableMobileControls()
    {
        if (mobileControls != null) mobileControls.SetActive(false);
    }
}