using UnityEngine;
using System.Collections;

//Controls light traffic's lights

public class Junction : MonoBehaviour {
	//used to save light traffic's state
	public bool free = false;
	public bool waiting = false;

	public Renderer trafficLight;

	//used to know which material is which color
	public int redLightMatNum;
	public int yelloLightMatNum;
	public int greenLightMatNum;

	//used to change color for corresponding material
	private Material redLight;
	private Material yelloLight;
	private Material greenLight;
	
	void Start()
	{
		//get materials from traffic light's object
		Material[] mats = trafficLight.materials;
		redLight = mats[redLightMatNum];
		yelloLight = mats[yelloLightMatNum];
		greenLight = mats[greenLightMatNum];

		//make all color gray (disabled)
		redLight.color = Color.gray;
		yelloLight.color = Color.gray;
		greenLight.color = Color.gray;
	}

	void Update ()
	{
		//change colors depending on state
		if (free)
		{
			greenLight.color = Color.green;
			yelloLight.color = Color.gray;
		}
		else if(waiting)
		{
			yelloLight.color = Color.yellow;
			redLight.color = Color.gray;
			greenLight.color = Color.gray;
		}
		else
		{
			redLight.color = Color.red;
			yelloLight.color = Color.gray;
		}

	}
}
