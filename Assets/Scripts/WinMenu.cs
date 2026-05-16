using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    // زرار New Game (يعيد ليفل 1)
    public void RestartLevel1()
    {
        Time.timeScale = 1f; // مهم جداً نرجع الوقت عشان اللعبة متفضلش واقفة
        PlayerPrefs.DeleteAll(); // نمسح أي حفظ قديم
        SceneManager.LoadScene(1); // يفتح ليفل 1
    }

    // زرار Continue (يدخل ليفل 2)
    public void GoToLevel2()
    {
        Time.timeScale = 1f; // نرجع الوقت
        SceneManager.LoadScene(2); // يفتح ليفل 2
    }
}