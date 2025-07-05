using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour
{
    public Movement player;
    public Transform finish;
    public float totalDistance = 0f;
    public float xStart = 0f;

    public Image progressBar;
    public TMP_Text progressText;
    float currentMax;
    string scoreKey;

    public GameObject pauseButton;
    public GameObject menuPanel;
    public GameObject finishPanel;

    public Star[] stars;
    public Image[] starImages;

    Scene scene;
    bool isWin = false;
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        scoreKey = $"{scene.name}_score";
        xStart = player.transform.position.x;
        totalDistance = finish.position.x - player.transform.position.x;
        currentMax = PlayerPrefs.GetFloat(scoreKey, 0);
    }
    void Update()
    {
        if (isWin) 
        {
            return;
        }
        if (player.transform.position.x >= finish.position.x)
        {
            SaveScore();
            finishPanel.SetActive(true);

            for(int i = 0; i < 3; i++)
            {
                starImages[i].color = stars[i].gameObject.activeSelf ? new Color(1, 1, 1, 0.2f) : new Color(1, 1, 1, 1);
            }
            Time.timeScale = 0;
            isWin = true;
        }
    }
    public void SaveScore()
    {
        if ( (player.transform.position.x - xStart) / totalDistance > currentMax)
        {
            currentMax =  (player.transform.position.x - xStart) / totalDistance;
            progressBar.fillAmount = currentMax;
            progressText.text = $"{(int)(currentMax * 100)}%";
            PlayerPrefs.SetFloat(scoreKey, currentMax);
        }
    }

    public void OpenMenu(bool isOpened)
    {
        Time.timeScale = isOpened? 0 : 1;
        if(isOpened)
        {
            SaveScore();
        }
        pauseButton.SetActive(!isOpened);
        menuPanel.SetActive(isOpened);
        finishPanel.SetActive(false);
    }

    public void Restart()
    {
        player.Restart(); 
        OpenMenu(false);
    }
    public void Leave()
    {
        SceneManager.LoadScene(0);
    }
}
