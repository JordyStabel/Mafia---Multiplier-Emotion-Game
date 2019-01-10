using System.Collections;
using System.Collections.Generic;
using Affdex;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Affdex2Animator : ImageResultsListener 
{
	public Graphic FaceFoundUI;
  public string Param_JoyAnger;
	private Animator animator;
  [SerializeField] private float joy;
  [SerializeField] private float anger;


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

      faces[0].Emotions.TryGetValue(Emotions.Joy, out joy);
      faces[0].Emotions.TryGetValue(Emotions.Anger, out anger);

      float valueJoy = joy / (joy + anger);
      float v = Mathf.Lerp(0, 1, valueJoy);
      
      animator.SetFloat(Param_JoyAnger, v);
    }

    void Awake()
	{
		animator = GetComponent<Animator>();
	}

}
