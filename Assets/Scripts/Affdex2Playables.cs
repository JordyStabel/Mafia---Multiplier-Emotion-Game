using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using Affdex;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Affdex2Playables : ImageResultsListener 
{

	public bool ControlMask = true;
	public Graphic FaceFoundUI;
	public AnimationClip NeutralAnimation;
	[Range(0.001f,1f)] public float NeutralWeight = 0.001f;
	public AffdexPlayable[] AnimationMapping;
	Animator animator;
	PlayableGraph graph;
	AnimationMixerPlayable mixerPlayable;
 	Dictionary<Emotions, float> emotionValues;

	
	
  	//Dictionary<Expressions, float> expressionValues;
	public override void onFaceFound(float timestamp, int faceId)
	{
		if (FaceFoundUI) FaceFoundUI.color = Color.green;
		Debug.Log("Face detected");
	}

	public override void onFaceLost(float timestamp, int faceId)
	{
		if (FaceFoundUI) FaceFoundUI.color = Color.red;
		Debug.Log("Face lost");
	}

	public override void onImageResults(Dictionary<int, Face> faces)
	{
		if (faces.Count == 0) return;

		emotionValues = faces[0].Emotions;
		//expressionValues = faces[0].Expressions;
	}
	IEnumerator Start()
	{
		animator = GetComponent<Animator>();
		yield return new WaitForSeconds(0.1f);
		graph = CreatePlayableGraph();		
	}

	void OnDestroy()
	{
		graph.Destroy();
	}

	public bool AttemptLayerMix;
    PlayableGraph CreatePlayableGraph()
	{
		// Create a Mixer.
		PlayableGraph g = animator.playableGraph;
		//var playableOutput = AnimationPlayableOutput.Create(g, "AnimationOutput", animator);

		mixerPlayable =	AnimationMixerPlayable.Create(g, AnimationMapping.Length + 1);

		// Create a 'neutral' playable with low weight
		var neutral = AnimationClipPlayable.Create(g, NeutralAnimation);
		g.Connect(neutral, 0, mixerPlayable, 0);
		// Create a playable for every AffdexPlayable and connect inputs to the Mixer
		for (int i = 0; i < AnimationMapping.Length; i++)
		{
			var playable = AnimationClipPlayable.Create(g, AnimationMapping[i].CubismExpression);
			Debug.Log("Connection: " + i + 1);
			g.Connect(playable, 0, mixerPlayable, i + 1);
		}

		if (AttemptLayerMix){
			AttachMixerToLayerMixer(mixerPlayable, g);
		}
		else{
			AttachMixerToAnimationPlayableOutput(mixerPlayable, g);
		}
		g.Play();
		GraphVisualizerClient.Show(g, "Affdex");
		rawWeights = new float[AnimationMapping.Length];
		return g;
	}

	void AttachMixerToAnimationPlayableOutput(AnimationMixerPlayable mixer, PlayableGraph g)
	{
		var playableOutput = g.GetOutput(0);
		playableOutput.SetSourcePlayable(mixerPlayable);
	}

	void AttachMixerToLayerMixer(AnimationMixerPlayable mixer, PlayableGraph g)
	{
		var playableOutput = g.GetOutput(0);
		var animatorMixer = playableOutput.GetSourcePlayable();
		if (!animatorMixer.IsValid()) { Debug.LogError("AnimatorMixerPlayable not found.");}
		var layerMixer = animatorMixer.GetInput(0);
		layerMixer.SetInputCount(1);
		layerMixer.ConnectInput(0, mixer, 0);
		//layerMixer.AddInput(mixer, 0);
	}

	float[] rawWeights;
	float totalWeights;
	void LateUpdate()
	{
		if (emotionValues == null || rawWeights == null) { return; }
		totalWeights = NeutralWeight;
		for (int i = 0; i < AnimationMapping.Length; i++)
		{
			rawWeights[i] = emotionValues[AnimationMapping[i].Emotion];
			totalWeights += rawWeights[i];
		}
		float normalizedNeutral = NeutralWeight / totalWeights;
		mixerPlayable.SetInputWeight(0, normalizedNeutral);
		for (int i = 0; i < AnimationMapping.Length; i++)
		{
			float normalizedWeight = rawWeights[i] / totalWeights;
			mixerPlayable.SetInputWeight(i + 1, normalizedWeight);
		}
	}

	void OnGUI()
	{
		if (rawWeights == null) { return; }
		GUI.color = Color.green;
		GUILayout.Label("Neutral Mixin: " + NeutralWeight / totalWeights);
		for (int i = 0; i < rawWeights.Length; i++)
		{
			GUILayout.Label(AnimationMapping[i].Emotion + ": " + Mathf.Max((rawWeights[i] / totalWeights), 0.001f)); // Cut off at very low values for readability
		}
		GUI.color = GUI.contentColor;
	}

	[System.Serializable]
	public struct AffdexPlayable
	{
		public Emotions Emotion;
		public AnimationClip CubismExpression;

	}

}
