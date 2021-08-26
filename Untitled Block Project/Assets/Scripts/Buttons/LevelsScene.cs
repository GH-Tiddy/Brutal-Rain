using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsScene : MonoBehaviour {

    public Sprite circleSprite;
    public Sprite lockSprite;
    public Button[] buttonsList = new Button[6];

    private void Start() {
        for (int i = 0; i < buttonsList.Length; i++) {
            if (i < PlayerPrefs.GetInt("bestLevel")) {
                buttonsList[i].image.sprite = circleSprite;
                buttonsList[i].transform.GetChild(0).gameObject.SetActive(true);
            } else {
                buttonsList[i].image.sprite = lockSprite;
                buttonsList[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void ClickLevel(int lvl) {
        if (PlayerPrefs.GetInt("bestLevel") >= lvl) {
            PlayerPrefs.SetInt("level", lvl);
            SceneManager.LoadScene("GameScene");
        }
    }

    public void ClickHome() {
        SceneManager.LoadScene("StartScene");
    }

}
