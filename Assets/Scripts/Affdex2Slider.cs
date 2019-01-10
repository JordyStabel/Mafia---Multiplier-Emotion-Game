using System.Collections;
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

    public static float emotionValue;

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
        //if (Instance != null)
        //    Debug.LogError("There shouldn't be two controllers.");
        //else
        //    Instance = this;
    }

    private void Update()
    {
        emotionValue++;
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
            var emotionValues = faces[0].Emotions;
            emotionValue = emotionValues[Emotion];
            slider.value = emotionValue;
        }
    }
}
