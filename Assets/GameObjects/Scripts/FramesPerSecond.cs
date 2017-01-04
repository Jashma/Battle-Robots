using UnityEngine;
using System.Collections;

public class FramesPerSecond : MonoBehaviour 
{
	public float updateInterval = 0.5F;
	private double lastInterval;
	private int frames = 0;
	public static float fps;

	void Start()
	{
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}

	void Update()
	{
		++frames;
		float timeNow = Time.realtimeSinceStartup;
		if (timeNow > lastInterval + updateInterval)
		{
			fps = (float)(frames / (timeNow - lastInterval));
			frames = 0;
			lastInterval = timeNow;
		}
	}
}
