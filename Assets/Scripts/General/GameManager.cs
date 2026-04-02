using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int totalProgressionScore;
    [SerializeField] private int progressScoreThreshold;

    [SerializeField] TextMeshProUGUI progressScoreText;

    [SerializeField] private Animator sceneTransitionAnim;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        sceneTransitionAnim = GameObject.Find("SceneTransitionCanvas").GetComponent<Animator>();
    }

    public void UpdateProgressScore(int scoreValue)
    {
        totalProgressionScore += scoreValue;
        UpdateUI();
        CheckProgress();
    }

    private void UpdateUI()
    {
        progressScoreText.text = "Progresso: " + totalProgressionScore.ToString() + "/100";
    }

    private void CheckProgress()
    {
        if(totalProgressionScore == progressScoreThreshold)
        {
            LoadSceneByName("EndGame");
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        sceneTransitionAnim.Play("FadeOut");
        StartCoroutine(WaitToLoadScene(sceneName));
    }

    private IEnumerator WaitToLoadScene(string name)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
        sceneTransitionAnim = null;
    }

    public void TryAgain()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.button);
        LoadSceneByName("Gameplay");
    }

    public void ExitGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.button);
        Application.Quit();
    }
}
