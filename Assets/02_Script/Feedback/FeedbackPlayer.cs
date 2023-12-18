using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum FeedbackFlag
{

    None = 0,
    HitBlink =      1,
    HitShake =      4,
    HitStop =       8,
    DamageText =    16

}

public class FeedbackPlayer : MonoBehaviour
{

    [SerializeField] private FeedbackFlag enableFeedback;
    [SerializeField] private DamageText prefab;

    private Dictionary<FeedbackFlag, Feedback> feedbackContainer = new();

    private void Awake()
    {
        
        feedbackContainer.Add(FeedbackFlag.HitBlink, new HitBlinkFeedback(this));
        feedbackContainer.Add(FeedbackFlag.HitShake, new HitShakeFeedback(this));
        feedbackContainer.Add(FeedbackFlag.HitStop, new HitStopFeedback(this));
        feedbackContainer.Add(FeedbackFlag.DamageText, new DamageTextFeedback(this));

    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            Play(10);

        }

        if (Input.GetKeyDown(KeyCode.Return))
        {

            Instantiate(prefab).Set(Random.Range(10f, 30f));

        }

    }

    public void Play(float damage)
    {

        foreach(var feedback in feedbackContainer)
        {

            if (enableFeedback.HasFlag(feedback.Key))
            {

                feedback.Value.Play(damage);

            }

        }

    }

}
