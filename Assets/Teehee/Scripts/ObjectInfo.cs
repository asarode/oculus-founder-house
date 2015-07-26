using UnityEngine;
using System.Collections;

public class ObjectInfo : MonoBehaviour {

	public enum Position
	{
		Top,
		Left,
		Right,
		Front
	};
	[SerializeField]
	private Position popDisplay;

	[SerializeField]
	private string[] words = new string[3];
	public struct OptionData
	{
		public string text;
		public bool isCorrect;

		public OptionData(string _text, bool _isCorrect)
		{
			text = _text;
			isCorrect = _isCorrect;
		}
	}
	public OptionData[] Options = new OptionData[3];

	[SerializeField]
	private bool ignore;
	
	void Start () {
		Options[0] = new OptionData(words[0], true);
		for (int i = 1; i < words.Length; i++)
		{
			Options[i] = new OptionData(words[i], false);
		}
	}
	
	void Update () {
	
	}

	public Position PopDisplay
	{
		get
		{
			return popDisplay;
		}
	}

	public bool Ignore
	{
		get
		{
			return ignore;
		}
	}
}
