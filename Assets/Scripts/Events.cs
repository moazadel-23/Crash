//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class Events : MonoBehaviour
//{
//    // زرار New Game (بيبدأ من الصفر)
//    public void NewGame()
//    {
//        PlayerPrefs.DeleteAll(); // بيمسح أي ليفل أو عملات متسيفة قبل كدة
//        SceneManager.LoadScene(1); // بيفتح ليفيل 1
//    }

//    // زرار Continue (بيكمل من آخر ليفل)
//    public void Continue()
//    {
//        // بيشوف إحنا سيفنا رقم كام في الباب، لو ملقاش بيبدأ من ليفيل 1
//        int savedLevel = PlayerPrefs.GetInt("SavedLevelIndex", 1);
//        SceneManager.LoadScene(savedLevel);
//    }

//    // زرار الخروج (لو حابب تسيبه)
//    public void Quit()
//    {
//        Application.Quit();
//        Debug.Log("Game Quit");
//    }
//}