using UnityEngine;
using System.Collections;

public class TimerScript : MonoBehaviour
{
	int seconds = 0;
	int minutes = 0;
	int hours = 0;
	string timeText = string.Empty;
	
	// Update is called once per frame
	void Update ()
	{
		if(!IsInvoking("Timer"))
		{
			Invoke ("Timer", 1);
		}
	}
	
	void OnGUI()
	{
		timeText = "Время в игре: " + hours + ":" + minutes + ":" + seconds;
		GUI.Label(new Rect(10, Screen.height - 75, 150, 25), timeText);
	}
	
	void Timer()
	{
		seconds++;
		if(seconds > 59)
		{
			seconds = 0;
			minutes++;
		}
		
		if(minutes > 59)
		{
			hours++;
			minutes = 0;
		}
		
		if(hours > 23)
		{
			hours = 0;
		}
	}
}

