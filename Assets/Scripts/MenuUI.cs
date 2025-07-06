using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public LevelPanel[] levels;
    void Start()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            float progress = PlayerPrefs.GetFloat($"{i+1}_progress", 0);
            int stars = PlayerPrefs.GetInt($"{i+1}_stars", 0);
            levels[i].progressBar.fillAmount = progress;
            levels[i].progressText.text = $"{(int)(progress * 100)}%";
            for (int j = 0; j < 3; j++)
            {
                levels[i].stars[j].color = 
                    (stars >= j + 1) ? 
                    new Color(1, 1, 1, 1) : 
                    new Color(1, 1, 1, 0.4f);
            }
        }
    }
    public void LoadLevel(int id)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(id);
    }
    [ContextMenu("Reset PlayerPrefs")]
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
