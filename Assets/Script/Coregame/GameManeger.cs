using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManeger : MonoBehaviour
{
    public static GameManeger instance; 

    [Header("Game Status")]
    public int currentScore = 0;
    public int maxScore = 5;
    public bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isGameOver) return;

        if (PanicSystem.instance.currentHeartRate >= 180f)
        {
            GameOver("Panic Attack! (Heart Rate Too High)");
        }
    }

    public void CompleteObjective(string objectiveName)
    {
        if (isGameOver) return;

        currentScore++;
        Debug.Log("สำเร็จ: " + objectiveName + " (" + currentScore + "/" + maxScore + ")");

        if (currentScore >= maxScore)
        {
            GameWin();
        }
    }

    void GameWin()
    {
        isGameOver = true;
        Debug.Log("YOU CALMED DOWN! (WIN)");
        // TODO: เดี๋ยวเราค่อยมาทำหน้าจอ Win ตรงนี้
        // SceneManager.LoadScene("WinScene"); 
    }

    void GameOver(string reason)
    {
        isGameOver = true;
        Debug.Log("GAME OVER: " + reason);
        // TODO: เดี๋ยวเราค่อยมาทำหน้าจอ Game Over ตรงนี้
    }
}