using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject completedText;

    public void Start() {
        animator.SetInteger("completed", PlayerPrefs.GetInt("completed"));
        if (PlayerPrefs.GetInt("completed") == 1) {
            PlayerPrefs.SetInt("completed", 2);
            PlayerPrefs.SetInt("level", 15);
        }
    }

    public void ClickStart() {
        SceneManager.LoadScene("GameScene");
    }

    public void ClickLevels() {
        SceneManager.LoadScene("LevelsScene");
    }

    public void ClickQuit() {
        Application.Quit();
    }

}
