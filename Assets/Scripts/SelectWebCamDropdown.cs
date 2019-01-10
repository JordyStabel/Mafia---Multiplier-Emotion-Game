using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Dropdown))]
public class SelectWebCamDropdown : MonoBehaviour 
{
	[Serializable]
	public class OnWebCamSelect : UnityEvent<bool,string> {}
	public OnWebCamSelect SendWebcamUpdate;
	public string SelectedDevice;
	Dropdown dropdown;
	
	void Start()
	{
		dropdown = GetComponent<Dropdown>();
		RefreshWebcams();
	}

	void RefreshWebcams()
	{
		dropdown.ClearOptions();
		WebCamDevice[] devices = WebCamTexture.devices;
		List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
		for (int i = 0; i < devices.Length; i++)
		{
			var option = new Dropdown.OptionData(devices[i].name);
			options.Add(option);
		}
		dropdown.AddOptions(options);
		dropdown.onValueChanged.AddListener(onSelect);
	}

	void onSelect(int selection)
	{
		SelectedDevice = WebCamTexture.devices[selection].name;
		SendWebcamUpdate.Invoke(WebCamTexture.devices[selection].isFrontFacing, SelectedDevice);
	}
}
