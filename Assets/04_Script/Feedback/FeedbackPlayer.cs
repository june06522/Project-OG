using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FeedbackPlayer : MonoBehaviour
{
    private List<Feedback> _feedbackList = new List<Feedback>();

    private void Awake()
    {
        AddFeedback();
    }

    private void AddFeedback()
    {
        _feedbackList = transform.GetComponents<Feedback>().ToList<Feedback>();
    }

    public void Play(float damage)
    {
        foreach (Feedback feedback in _feedbackList)
        {
            if (gameObject.activeInHierarchy == false)
                return;

            feedback.Play(damage);
        }
    }

}
