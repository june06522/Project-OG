using UnityEngine;

public interface IHitAble
{
    public FeedbackPlayer feedbackPlayer { get; set; }

    /// <returns>������ false��ȯ</returns>
    public bool Hit(float damage)
    {
        feedbackPlayer.Play(damage + Random.Range(0.25f, 1.75f));
        return true;
    }
}
