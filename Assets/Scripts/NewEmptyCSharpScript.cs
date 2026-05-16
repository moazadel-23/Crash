using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    public GameObject choicePanel; // اسحب الـ Panel هنا في الانسبكتور

    // دالة الزرار الأول
    public void OnClickStartGame()
    {
        choicePanel.SetActive(true); // يظهر المنيو التانية
    }

    // دالة زرار "ابدأ من الأول"
    public void OnClickNewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1); // يفتح ليفيل 1
    }

    // دالة زرار "كمل"
    public void OnClickContinue()
    {
        int level = PlayerPrefs.GetInt("SavedLevelIndex", 1);
        SceneManager.LoadScene(level);
    }
}