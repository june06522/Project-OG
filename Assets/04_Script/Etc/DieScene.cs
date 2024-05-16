using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DieScene : MonoBehaviour
{
    [SerializeField] Image fadeImage;

    private void Start()
    {
        fadeImage.DOFade(0f, 1f);
    }

    public void ReturnToIntro()
    {
        SceneManager.LoadScene("Intro");
    }
}
