using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region Variables
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private GameObject hitterPrefab;
    [SerializeField] private GameObject shrinkerPrefab;
    [SerializeField] private GameObject freezerPrefab;
    [SerializeField] private GameObject exploderPrefab;
    [SerializeField] private GameObject hitterStreamPrefab;
    [SerializeField] private GameObject leftHitterStreamPrefab;
    [SerializeField] private GameObject rightHitterStreamPrefab;
    [SerializeField] private GameObject rapidHitterPrefab;
    [SerializeField] private GameObject growerPrefab;
    [SerializeField] private GameObject shielderPrefab;
    [SerializeField] private Button retryButton;
    [SerializeField] private GameObject trigger;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator tutorialAnimator;
    [SerializeField] private GameObject tutorialScreen;
    private ArrayList prefabs = new ArrayList();
    private short loopNumber = 1;
    private bool firstFreezer = true;
    #endregion

    #region Setup

    private void Start() {
        animator.SetInteger("level", PlayerPrefs.GetInt("level"));
        PlayLevel(PlayerPrefs.GetInt("level"));
    }

    public IEnumerator Spawner(GameObject prefab, float sec) {
        short n = loopNumber;
        while (n == loopNumber) {
            GameObject a = Instantiate(prefab) as GameObject;
            prefabs.Add(a);
            yield return new WaitForSeconds(sec);
        }
    }

    public IEnumerator SingleSpawner(GameObject prefab) {
        while(!animator.GetBool("hitGround")) {
            yield return new WaitForSeconds(1);
        }
        GameObject a = Instantiate(prefab) as GameObject;
        prefabs.Add(a);
    }

    IEnumerator Ending() {
        float[,] you = {
            { -5, -1, 0, 4, 5, 10 },
            { -5, -2, 1, 3, 6, 10 },
            { -5.5f, -4.5f, -2, 1, 3, 6 },
            { -6, -4, -1, 0, 3, 6 }
        };

        float[,] win = {
            { -5.5f, -3.5f, -1, 1, 3.25f, 5.25f, 10},
            { -5.67f, -4.5f, -3.33f, -1, 1, 2.5f, 3.25f},
            { -5.84f, -3.16f, -1, 1, 1.75f, 3.25f, 5.25f},
            { -6,  -3, -1, 1, 3.25f, 5.25f, 10}
        };

        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 6; j++) {
                if (you[i, j] == 10) continue;
                GameObject a = Instantiate(hitterPrefab) as GameObject;
                a.transform.position = new Vector2(you[i, j], 5);
            }
            yield return new WaitForSeconds(2);
        }
        yield return new WaitForSeconds(4);
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 7; j++) {
                if (win[i, j] == 10) continue;
                GameObject a = Instantiate(hitterPrefab) as GameObject;
                a.transform.position = new Vector2(win[i, j], 5);
            }
            yield return new WaitForSeconds(2f);
        }
        yield return new WaitForSeconds(5);
        animator.SetBool("finished", true);
    }

    public IEnumerator Tutorial() {
        tutorialScreen.SetActive(true);
        tutorialAnimator.SetInteger("stage", 1);
        while (player.position.y > -3) yield return new WaitForSeconds(0.5f);
        tutorialAnimator.SetInteger("stage", 2);
        while (true) {
            if (Input.touchCount > 0) {
                if (Input.GetTouch(0).position.x < Screen.width / 2) {
                    break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        tutorialAnimator.SetInteger("stage", 3);
        while (player.position.y < -3) {
            yield return new WaitForSeconds(0.1f);
        }
        tutorialAnimator.SetInteger("stage", 4);
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("opengoal", true);
        while (PlayerPrefs.GetInt("level") == 0) yield return new WaitForSeconds(0.1f);
        tutorialAnimator.SetInteger("stage", 5);
        tutorialScreen.SetActive(false);
    }

    public bool GetFirstFreezer() {
        return firstFreezer;
    }

    public void SetFreezer(Vector2 pos) {
        firstFreezer = true;
        StartCoroutine(Spawner(exploderPrefab, 5));
        FindObjectOfType<ExploderAI>().HomingBullet(pos);
    }

    public short GetLoopNumber() {
        return loopNumber;
    }
    #endregion

    #region LevelEnded
    public void LevelComplete() {
        DestroyEnemies();
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        animator.SetBool("hitGround", false);
        animator.SetInteger("level", PlayerPrefs.GetInt("level"));
        if (PlayerPrefs.GetInt("bestLevel") < PlayerPrefs.GetInt("level")) PlayerPrefs.SetInt("bestLevel", PlayerPrefs.GetInt("level"));
        PlayerPrefs.Save();
        PlayLevel(PlayerPrefs.GetInt("level"));
    }

    public void LevelFailed() {
        DestroyEnemies();
        retryButton.gameObject.SetActive(true);
        trigger.SetActive(false);
    }

    public void TryAgain() {
        retryButton.gameObject.SetActive(false);
        animator.SetBool("tryAgain", true);
        animator.SetBool("hitGround", false);
        PlayLevel(PlayerPrefs.GetInt("level"));
        FindObjectOfType<PlayerMovement>().SetSize(1f);
        trigger.SetActive(true);
        FindObjectOfType<PlayerMovement>().Start();
    }

    public void DestroyEnemies() {
        loopNumber++;
        foreach (GameObject a in prefabs) Destroy(a);
        FindObjectOfType<PlayerMovement>().ResetReverse();
    }
    #endregion

    #region Levels
    public void PlayLevel(int level) {
        switch (level) {
            case 0:
                StartCoroutine(Tutorial());
                break;
            case 1:
                StartCoroutine(Spawner(hitterPrefab, 1.5f));
                break;
            case 2:
                StartCoroutine(Spawner(hitterPrefab, 0.7f));
                break;
            case 3:
                StartCoroutine(Spawner(hitterPrefab, 1));
                StartCoroutine(Spawner(hitterStreamPrefab, 1.2f));
                break;
            case 4:
                StartCoroutine(Spawner(hitterPrefab, 0.8f));
                StartCoroutine(Spawner(shrinkerPrefab, 4));
                break;
            case 5:
                StartCoroutine(Spawner(hitterPrefab, 0.8f));
                StartCoroutine(Spawner(rapidHitterPrefab, 1.5f));
                StartCoroutine(Spawner(shrinkerPrefab, 4));
                break;
            case 6:
                StartCoroutine(Spawner(hitterPrefab, 1f));
                StartCoroutine(Spawner(shrinkerPrefab, 4));
                StartCoroutine(SingleSpawner(growerPrefab));
                break;
            case 7:
                StartCoroutine(Spawner(hitterPrefab, 1));
                StartCoroutine(Spawner(hitterStreamPrefab, 0.7f));
                StartCoroutine(Spawner(shielderPrefab, 5));
                break;
            case 8:
                StartCoroutine(Spawner(hitterPrefab, 1));
                StartCoroutine(Spawner(leftHitterStreamPrefab, 0.67f));
                StartCoroutine(Spawner(rightHitterStreamPrefab, 0.67f));
                StartCoroutine(Spawner(shielderPrefab, 4));
                break;
            case 9:
                StartCoroutine(Spawner(hitterPrefab, 1));
                StartCoroutine(Spawner(leftHitterStreamPrefab, 0.5f));
                StartCoroutine(Spawner(rightHitterStreamPrefab, 0.5f));
                StartCoroutine(Spawner(shielderPrefab, 4));
                StartCoroutine(Spawner(shrinkerPrefab, 4));
                break;
            case 10:
                StartCoroutine(Spawner(hitterPrefab, 1.2f));
                StartCoroutine(Spawner(rapidHitterPrefab, 2.5f));
                StartCoroutine(Spawner(freezerPrefab, 4));
                break;
            case 11:
                StartCoroutine(Spawner(hitterPrefab, 1f));
                StartCoroutine(Spawner(rapidHitterPrefab, 2f));
                StartCoroutine(Spawner(shrinkerPrefab, 4));
                StartCoroutine(Spawner(freezerPrefab, 4));
                break;
            case 12:
                StartCoroutine(Spawner(hitterPrefab, 1f));
                StartCoroutine(Spawner(rapidHitterPrefab, 1.8f));
                StartCoroutine(Spawner(freezerPrefab, 4));
                break;
            case 13:
                firstFreezer = false;
                StartCoroutine(Spawner(hitterPrefab, 1.5f));
                StartCoroutine(Spawner(rapidHitterPrefab, 1f));
                StartCoroutine(Spawner(freezerPrefab, 4));
                break;
            case 14:
                StartCoroutine(Spawner(hitterPrefab, 1.3f));
                StartCoroutine(Spawner(rapidHitterPrefab, 2.5f));
                StartCoroutine(Spawner(hitterStreamPrefab, 0.5f));
                StartCoroutine(Spawner(freezerPrefab, 6));
                StartCoroutine(Spawner(shrinkerPrefab, 5));
                StartCoroutine(Spawner(shielderPrefab, 5));
                break;
            case 15:
                StartCoroutine(Spawner(hitterPrefab, 1.3f));
                StartCoroutine(Spawner(rapidHitterPrefab, 2));
                StartCoroutine(Spawner(hitterStreamPrefab, 0.5f));
                StartCoroutine(Spawner(freezerPrefab, 6));
                StartCoroutine(Spawner(shrinkerPrefab, 1.8f));
                StartCoroutine(Spawner(shielderPrefab, 6));
                StartCoroutine(Spawner(exploderPrefab, 5));
                StartCoroutine(SingleSpawner(growerPrefab));
                break;
            case 16:
                StartCoroutine(Ending());
                break;
        }
    }
    #endregion

}