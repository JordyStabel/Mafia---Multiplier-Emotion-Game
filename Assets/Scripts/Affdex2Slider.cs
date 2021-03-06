﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Affdex;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Affdex2Slider : ImageResultsListener
{
	public Emotions Emotion;
	private Slider slider;
	public Text Label;

    // Emotion value, that can be accessed form other scripts
    public float EmotionValue { get; protected set; }

    public float emotionValue;

    private float timeLeft = 1f;

    // Creating an instance of 'Affdex2Slider' which is accessible from all classes
    public static Affdex2Slider Instance { get; protected set; }

    Detector detector;

	public void Start(){
		slider = GetComponent<Slider>();
		if (Label) Label.text = Emotion.ToString();
        detector = FindObjectOfType<Detector>();
        if (detector) 
        { 
            StartCoroutine(SetEmotionStateWhenReady());
        }
        // Setting the instance equal to this current one (with check)
        if (Instance != null)
            Debug.LogError("There shouldn't be two controllers.");
        else
            Instance = this;
    }

    public void ChangeSlider(float value)
    {
        slider.value = value;
    }

    private void Update()
    {
        // Testing
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            EmotionValue = Random.Range(0f, 1f);
            emotionValue = EmotionValue;
            //slider.value = EmotionValue;
            timeLeft = 1f;
        }
    }

    IEnumerator SetEmotionStateWhenReady()
    {
        yield return new WaitUntil(() => detector.IsRunning );
        Debug.Log(string.Format("Setting EmotionState {0} for slider.", Emotion.ToString()));
        detector.SetEmotionState(Emotion, true);
    }

    public override void onFaceFound(float timestamp, int faceId) { }
    public override void onFaceLost(float timestamp, int faceId) { }

    public override void onImageResults(Dictionary<int, Face> faces)
    {
		if (faces.Count == 0)
        {
            slider.value = 0;
        }
        else
        {
            Debug.Log("Fired");

            var emotionValues = faces[0].Emotions;
            emotionValue = emotionValues[Emotion];
            slider.value = emotionValue;
        }
    }
}
