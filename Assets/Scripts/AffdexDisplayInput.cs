using System.Collections;
using System.Collections.Generic;
using Affdex;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class AffdexDisplayInput : ImageResultsListener 
{
	RawImage image;
	public bool DrawFeatures;
	public HotSpotsIndicator HotSpots;

	Texture _affdexInput;
	Texture affdexInput
	{
		get 
		{
			if (_affdexInput == null)
			{
				Detector d = GameObject.FindObjectOfType<Detector>();
				IDetectorInput input = d.gameObject.GetComponent<IDetectorInput>();
				_affdexInput = input.Texture;
			}
			return _affdexInput;
		}
	}

	void Start () 
	{
		if (!AffdexUnityUtils.ValidPlatform()) return;
		image = GetComponent<RawImage>();
	}

	void Update()
	{
		image.texture = affdexInput;
	}

	public override void onImageResults(Dictionary<int, Face> faces)
	{
		if (!DrawFeatures) return;

		List<Vector2> normalizedHotSpots = new List<Vector2>();
		var width = affdexInput.width;
		var height = affdexInput.height;
		
		foreach (var face in faces)
		{
			var featuresPoints = face.Value.FeaturePoints;
			for (int i = 0; i < featuresPoints.Length; i++)
			{
				var fp = featuresPoints[i];
				var norm = new Vector2(fp.x / width, fp.y / height);
				normalizedHotSpots.Add(norm);
			}
		}
		HotSpots.SetHotSpots(normalizedHotSpots);
	}

	public override void onFaceFound(float timestamp, int faceId)
	{
	}

	public override void onFaceLost(float timestamp, int faceId)
	{
	}
}
