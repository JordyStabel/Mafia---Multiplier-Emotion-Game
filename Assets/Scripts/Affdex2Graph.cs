using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Affdex;
using UnityEngine.UI;

public class Affdex2Graph : ImageResultsListener 
{
	public Emotions Emotion;
	private LineRenderer graphLine;
    public LineRenderer frame;
	public Text Label;

    public Color NormalColor;
    public Color HighColor;
    Detector detector;

	public void Start(){
		graphLine = GetComponent<LineRenderer>();
		if (Label) Label.text = Emotion.ToString();
        initGraph();
        detector = FindObjectOfType<Detector>();
        if (detector) 
        { 
            StartCoroutine(SetEmotionStateWhenReady());
        }
	}

    IEnumerator SetEmotionStateWhenReady()
    {
        yield return new WaitUntil(() => detector.IsRunning );
        Debug.Log(string.Format("Setting EmotionState {0} for graphLine.", Emotion.ToString()));
        detector.SetEmotionState(Emotion, true);
    }

    public override void onFaceFound(float timestamp, int faceId) { }
    public override void onFaceLost(float timestamp, int faceId) { }

    public override void onImageResults(Dictionary<int, Face> faces)
    {
		if (faces.Count == 0)
        {
            addDataPoint(0f);
        }
        else
        {
            var emotionValues = faces[0].Emotions;
            addDataPoint(emotionValues[Emotion]);
        }

    }
    float[] dataPoints;
    public int maxDataPoints;
    
    void initGraph() 
    { 
        dataPoints = new float[maxDataPoints];
        lineVectors = new Vector3[maxDataPoints];
        graphLine.positionCount = maxDataPoints;
    }
    void addDataPoint(float newDataPoint)
    {
        for (int i = 1; i < maxDataPoints; i++)
        {
            dataPoints[i - 1] = dataPoints[i];
        }
        dataPoints[maxDataPoints - 1] = newDataPoint / 100; // normalize to 0-1
    }
    Vector3[] lineVectors;
    void Update()
    {
        for (int i = 0; i < dataPoints.Length; i++)
        {
            lineVectors[i] = floatsToGraphVector((float)i / dataPoints.Length, dataPoints[i]);
        }
        if (dataPoints[dataPoints.Length -1] > 0.7f){
            graphLine.material.color = HighColor;
            frame.material.color = HighColor;
        }
        else {
            graphLine.material.color = NormalColor;
            frame.material.color = NormalColor;
        }
        graphLine.SetPositions(lineVectors);
    }

    public Rect GraphRect = new Rect(-5,-5,10,10);

    Vector3 floatsToGraphVector(float normInputX, float normInputY)
    {
        float x = Mathf.Lerp(GraphRect.xMin, GraphRect.xMax, normInputX);
        float y = Mathf.Lerp(GraphRect.yMin, GraphRect.yMax, normInputY);
        return new Vector3(x,y,0);
    }

}