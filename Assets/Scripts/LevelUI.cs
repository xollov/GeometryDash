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
    string progressKey;

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
        progressKey = $"{scene.buildIndex}_progress";
        xStart = player.transform.position.x;
        totalDistance = finish.position.x - player.transform.position.x;
        currentMax = PlayerPrefs.GetFloat(progressKey, 0);
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

            int starCounter = 0;
            for(int i = 0; i < 3; i++)
            {
                if (!stars[i].gameObject.activeSelf)
                {
                    starCounter++;
                    starImages[i].color =  new Color(1, 1, 1, 1);
                }
                else 
                {
                    starImages[i].color = new Color(1, 1, 1, 0.2f);
                }
            }
            int maxStars = PlayerPrefs.GetInt($"{scene.buildIndex}_stars", 0);
            PlayerPrefs.SetInt($"{scene.buildIndex}_stars", Mathf.Max(maxStars, starCounter));
            Time.timeScale = 0;
            isWin = true;
        }
    }
    public void SaveScore()
    {
        print($"Current max: {currentMax}\n current distance: {(player.transform.position.x - xStart) / totalDistance}");
        if ( (player.transform.position.x - xStart) / totalDistance > currentMax)
        {
            currentMax =  (player.transform.position.x - xStart) / totalDistance;
            progressBar.fillAmount = currentMax;
            progressText.text = $"{(int)(currentMax * 100)}%";
            if (currentMax > 1) 
            {
                currentMax = 1;
            }
            PlayerPrefs.SetFloat(progressKey, currentMax);
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
