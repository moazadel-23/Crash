using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Final Hearts Solution")]
    public Image[] finalHearts;

    [Header("Player Settings")]
    [SerializeField] private PlayerController playerController;
    private Vector3 playerPosition;

    [Header("UI & Scoring")]
    [SerializeField] private TMP_Text coinText;
    private int coinCount = 0;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Timer Settings")]
    public TMP_Text timerText;
    public float timeRemaining = 30f;
    private float currentTime;
    private bool isTimerRunning = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // لو عايز الـ GameManager يكمل بين المشاهد فعل السطر اللي تحت
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 1. تصفير الوقت عند بداية المشهد (سواء أول مرة أو بعد الموت)
        currentTime = timeRemaining;
        isTimerRunning = true;

        // 2. ربط الـ UI تلقائياً عشان ميبقاش "Missing" بعد الموت
        AutoLinkUI();

        if (playerController != null)
            playerPosition = playerController.transform.position;

        UpdateGUI();
    }

    void Update()
    {
        // كود العداد
        if (isTimerRunning)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                UpdateTimerDisplay(currentTime);
            }
            else
            {
                currentTime = 0;
                isTimerRunning = false;
                TimerFinished();
            }
        }

        // سطر حماية: لو النص اختفى لأي سبب (زي Reload المشهد) دوره عليه تاني
        if (timerText == null || coinText == null) AutoLinkUI();
    }

    // دالة بتبحث عن النصوص في المشهد بالاسم (تأكد من الأسماء في Unity)
    public void AutoLinkUI()
    {
        if (timerText == null)
        {
            GameObject t = GameObject.Find("Time1"); // لازم يكون اسم النص في الـ Hierarchy هو Time1
            if (t != null) timerText = t.GetComponent<TMP_Text>();
        }

        if (coinText == null)
        {
            GameObject c = GameObject.Find("CoinText"); // لازم يكون اسم نص الفلوس هو CoinText
            if (c != null) coinText = c.GetComponent<TMP_Text>();
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        if (timeToDisplay < 0) timeToDisplay = 0;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerFinished()
    {
        if (HealthManager.instance != null)
        {
            HealthManager.instance.LoseHeart();
            // لو لسه فيه قلوب، الـ Start اللي فوق هترست الوقت تلقائياً عند إعادة المشهد
        }
    }

    public void IncrementCoinCount()
    {
        coinCount++;
        UpdateGUI();
    }

    private void UpdateGUI()
    {
        if (coinText != null) coinText.text = coinCount.ToString();
    }

    public void UpdateHeartsVisual(int health)
    {
        if (finalHearts == null) return;
        for (int i = 0; i < finalHearts.Length; i++)
        {
            if (finalHearts[i] != null)
                finalHearts[i].gameObject.SetActive(i < health);
        }
    }

    public void GameOver()
    {
        if (gameOverPanel != null && !gameOverPanel.activeSelf)
        {
            StartCoroutine(GameOverSequence());
        }
    }

    private IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(0.5f);
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void IncrementGemCount()
    {
        // الكود اللي إنت عايزه هنا
        Debug.Log("Gem Collected!");
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}