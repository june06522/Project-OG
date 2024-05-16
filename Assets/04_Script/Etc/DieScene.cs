using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DieScene : MonoBehaviour
{
    [SerializeField] Image fadeImage;

    private void Start()
    {
        Time.timeScale = 1f;
        fadeImage.DOFade(0f, 2f);
    }

    public void ReturnToIntro()
    {
        SceneManager.LoadScene("Intro");
    }
}
