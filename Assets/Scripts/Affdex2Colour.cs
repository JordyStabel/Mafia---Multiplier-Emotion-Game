using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Affdex;
using UnityEngine.Events;
using System;

public class Affdex2Colour : ImageResultsListener 
{
	[System.Serializable]
	public class SetColorEvent : UnityEvent<Color> {}
	public bool ShowDebug;
	public EmotionMapping[] Emotions;
	public SetColorEvent OnSetColor;
	public SetColorEvent OnFaceFound;
	public Color FaceFoundColor = Color.green;
	public Color FaceLostColor = Color.red;


	void Start()
	{
		var detector = FindObjectOfType<Affdex.Detector>();
		for	(int i = 0; i < Emotions.Length; i++)
		{
			detector.SetEmotionState(Emotions[i].AffDex, true);
		}

	}
    public override void onFaceFound(float timestamp, int faceId)
    {
		OnFaceFound.Invoke(FaceFoundColor);
    }

    public override void onFaceLost(float timestamp, int faceId)
    {
		OnFaceFound.Invoke(FaceLostColor);
    }

	public void OnGUI()
	{
		if (!ShowDebug) return;
		GUI.color = Color.white;
		for (int i = 0; i < Emotions.Length; i++)
		{
			GUILayout.Label(Emotions[i].AffDex + ": " + Emotions[i].Value);
		}
	}

	Color[] colors;
    public override void onImageResults(Dictionary<int, Face> faces)
    {
		if (faces.Count == 0) return;
		var emotionValues = faces[0].Emotions;
    	//var expressionValues = faces[0].Expressions;

		colors = new Color[Emotions.Length];
		for (int i = 0; i < Emotions.Length; i++)
		{
			Emotions[i].Value = emotionValues[Emotions[i].AffDex];
			colors[i] = mulColorByAffdexValue(Emotions[i].Color, Emotions[i].Value); // Multiply color by [0,1] Affdex
		}
		//colors[Emotions.Length] = Color.white;
		Color setcolor = MixColorsAdditive(colors);
		OnSetColor.Invoke(setcolor);
    }
	[ContextMenu("Test")]
	public void Test()
	{
		Color a = mulColorByAffdexValue(Color.red, 100);
		Debug.Log(a);
		Color b = mulColorByAffdexValue(Color.green, 100);
		Debug.Log(b);
		MixColorsAdditive(new Color[] {a,b});
	}
	private Color mulColorByAffdexValue(Color c, float f)
	{
		return c * (f * 0.01f);
	}
	private static Color MixColorsByAverage(Color[] inputs)
	{
		// Incorrect testimplementation: take average value of RGB inputs
		float r = 0, g = 0, b = 0;
		foreach (Color i in inputs)
		{
			r += i.r;
			g += i.g;
			b += i.b;
		}
		float mValue = 1f / inputs.Length;
		r *= mValue;
		g *= mValue;
		b *= mValue;
		Debug.Log(String.Format("{0},{1},{2}",r,g,b));
		return new Color(r,g,b);

		// Correct implementation would be:
		// Convert to Additive color space
		// Add together
		// Convert back to RGB values.
	}
	///<Summary>Adds colors together and caps at white.</Summary>
	private static Color MixColorsAdditive(Color[] inputs)
	{
		float r = 0, g = 0, b = 0;
		foreach (Color i in inputs)
		{
			r += i.r;
			g += i.g;
			b += i.b;
		}
		r = Mathf.Min(r, 1f);
		g = Mathf.Min(g, 1f);
		b = Mathf.Min(b, 1f);
		return new Color(r,g,b);
	}

	public class ColorMapping<T> where T : struct
	{
		public T AffDex;
		public Color Color; 
		[NonSerialized]public float Value = 0f;
	}

	[System.Serializable]
	public class EmotionMapping : ColorMapping<Emotions> { }
	[System.Serializable]
	public class ExpressionMapping : ColorMapping<Expressions> { }
}

#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer(typeof(Affdex2Colour.EmotionMapping))]
public class ColorMappingDrawer : UnityEditor.PropertyDrawer
{
	public override void OnGUI(Rect position, UnityEditor.SerializedProperty p, GUIContent label)
	{
		UnityEditor.EditorGUI.BeginProperty(position, label, p);
		Rect[] controls = new Rect[2];
		controls[0] = new Rect(position.x, position.y, position.width / 2, position.height);
		controls[1] = new Rect(controls[0].xMax, position.y, position.width / 2, position.height);
		p.Next(true);
		UnityEditor.EditorGUI.PropertyField(controls[0], p);
		p.Next(true);
		//p.colorValue = UnityEditor.EditorGUI.ColorField(controls[1], p.colorValue);
		UnityEditor.EditorGUI.PropertyField(controls[1], p);
		UnityEditor.EditorGUI.EndProperty();
	}
}

#endif
