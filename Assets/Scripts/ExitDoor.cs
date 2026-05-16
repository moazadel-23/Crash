using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    // اسحب Canvas (1) من الـ Hierarchy للخانة دي في الانسبكتور
    public GameObject winMenuCanvas;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. أظهر القائمة فوراً
            winMenuCanvas.SetActive(true);

            // 2. وقف الزمن في اللعبة
            Time.timeScale = 0f;

            // 3. سيف إن ليفل 2 هو اللي عليه الدور
            PlayerPrefs.SetInt("SavedLevelIndex", 2);
        }
    }
}