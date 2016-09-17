using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour 
{
	public static int CurrentScore;	
	private Text myText;
	
	void Start()
	{
		myText = GetComponent<Text>();
		Reset();
	}
	
	public void Score(int points)
	{
		CurrentScore += points;
		myText.text = CurrentScore.ToString();
	}
	
	public static void Reset()
	{
		CurrentScore = 0;		
	}
}
