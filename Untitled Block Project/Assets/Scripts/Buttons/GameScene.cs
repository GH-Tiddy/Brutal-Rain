using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour {

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Text level;

    public void ClickPause() {
        Time.timeScale = 0;
        level.text = (PlayerPrefs.GetInt("level") == 0) ? "TUTORIAL" : "LEVEL " + PlayerPrefs.GetInt("level");
        pauseMenu.SetActive(true);
    }

    public void ClickPlay() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ClickLevels() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelsScene");
    }

    public void ClickQuit() {
        Application.Quit();
    }

}