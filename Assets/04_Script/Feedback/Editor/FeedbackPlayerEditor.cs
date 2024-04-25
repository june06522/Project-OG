using NUnit.Framework;
using SpriteShadersUltimate;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FeedbackPlayer))]
[CanEditMultipleObjects]
public class FeedbackPlayerEditor : Editor
{
    FeedbackPlayer _feedbackPlayer;

    GUIStyle labelStyle;

    private bool _addFeedbackListToggle;

    private void OnEnable()
    {
        _feedbackPlayer = (FeedbackPlayer)target;
        _addFeedbackListToggle = false;
    }

    public override void OnInspectorGUI()
    {
        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.richText = true;
        base.OnInspectorGUI();

        if (_feedbackPlayer == null)
            return;

        if (AddFeedbackToggleButton())
        {

            _addFeedbackListToggle = !_addFeedbackListToggle;

        }

        if (_addFeedbackListToggle)
        {

            EditorGUILayout.Space(10);
            AddFeedbackButton<HitBlinkFeedback>("HitBlink");
            AddFeedbackButton<HitShakeFeedback>("HitShake");
            AddFeedbackButton<DamageTextFeedback>("DamageText");
            AddFeedbackButton<HitStopFeedback>("StopTime");
            AddFeedbackButton<PlaySoundFeedback>("PlaySound");
            AddFeedbackButton<SpawnParticleFeedback>("SpawnParticle");
            AddFeedbackButton<CameraShakeFeedback>("CameraShake");
            AddFeedbackButton<OverdamageShockwaveFeedback>("OverdamageShockwave");
            AddFeedbackButton<ChromaticFeedback>("Chromatic");

        }

    }

    private bool AddFeedbackToggleButton()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.Space(10);
        bool value = GUILayout.Button(_addFeedbackListToggle ? "Off" : "Add Feedback");
        EditorGUILayout.Space(10);

        EditorGUILayout.EndHorizontal();

        return value;
    }

    private void AddFeedbackButton<T>(string text) where T : Feedback
    {
        if (_feedbackPlayer.transform.GetComponent<T>() != null)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.Space(20);
        
        if (GUILayout.Button(text, GUILayout.Width(200)))
        {
            _feedbackPlayer.transform.AddComponent<T>();
            EditorUtility.SetDirty(target);
        }
        EditorGUILayout.EndHorizontal();
    }

}
