using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class WebCam : MonoBehaviour 
{
	RectTransform rect;
	RawImage image;
	WebCamTexture webCamTexture;

	public void Start()
	{
		SetWebCam(true, "");
	}
	public void SetWebCam (bool isFrontFacing, string deviceName) 
	{
		rect = transform as RectTransform;
		image = GetComponent<RawImage>();
		if (webCamTexture) { Destroy (webCamTexture); }
		webCamTexture = new WebCamTexture(deviceName, Mathf.RoundToInt(rect.rect.width), Mathf.RoundToInt(rect.rect.height));
		image.texture = webCamTexture;
		webCamTexture.Play();
	}


}
