using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Affdex;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Affdex2Audio : ImageResultsListener 
{
	public Emotions Emotion;
	private AudioSource _audio;
	public float PlaySoundWhenOverValue;

    Detector detector;

	public void Start(){
		_audio = GetComponent<AudioSource>();
        detector = FindObjectOfType<Detector>();
        if (detector) 
        { 
            StartCoroutine(SetEmotionStateWhenReady());
        }
	}

    IEnumerator SetEmotionStateWhenReady()
    {
        yield return new WaitUntil(() => detector.IsRunning );
        Debug.Log(string.Format("Setting EmotionState {0} for Audio.", Emotion.ToString()));
        detector.SetEmotionState(Emotion, true);
    }

    public override void onFaceFound(float timestamp, int faceId) { }
    public override void onFaceLost(float timestamp, int faceId) { }

    public override void onImageResults(Dictionary<int, Face> faces)
    {
		if (faces.Count == 0)
        {
        }
        else
        {
            var emotionValues = faces[0].Emotions;
	        if (emotionValues[Emotion] > PlaySoundWhenOverValue)
	        {
		        if (!_audio.isPlaying) _audio.PlayOneShot(_audio.clip);
	        };
        }
    }
}
