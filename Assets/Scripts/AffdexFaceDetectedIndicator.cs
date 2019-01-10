using System.Collections;
using System.Collections.Generic;
using Affdex;
using UnityEngine;
using UnityEngine.Events;

public class AffdexFaceDetectedIndicator : ImageResultsListener
{
	[System.Serializable]public class SetColorEvent : UnityEvent<Color> {}

	public SetColorEvent OnFaceVisibleChanged;
	public Color FaceVisible;
	public Color FaceNotVisible;

	int facesFound;

    public override void onImageResults(Dictionary<int, Face> faces)
    { }

    public override void onFaceFound(float timestamp, int faceId)
    {
		if (facesFound == 0) { OnFaceVisibleChanged.Invoke(FaceVisible); }
		facesFound++;
    }

    public override void onFaceLost(float timestamp, int faceId)
    {
		facesFound--;
		if (facesFound == 0) { OnFaceVisibleChanged.Invoke(FaceNotVisible); }
    }
}
