using UnityEngine;
using System.Collections;

public class OptionSelector : MonoBehaviour {

	[SerializeField]
	private GameObject[] Options;
	private GameObject Popover;
	[SerializeField]
	private GameObject Notification;
	[SerializeField]
	private Camera CameraFacing;
	// Use this for initialization
	void Start () {
		Popover = GameObject.FindGameObjectWithTag("Popover");
	}
	
	// Update is called once per frame
	void Update () {
		Notification.transform.position = new Vector3(Popover.transform.position.x, Popover.transform.position.y + 4f, Popover.transform.position.z)
										  + CameraFacing.transform.rotation * Vector3.forward * -2f;;
		Notification.transform.LookAt (CameraFacing.transform.position);
		Notification.transform.Rotate (0.0f, 180.0f, 0.0f);
	}

	public void SelectOption(int selection)
	{
		for (int i = 0; i < Options.Length; i++)
		{
			if (i == selection)
			{
				Options[selection].GetComponent<TextMesh>().color = new Color(0.08f, 0.09f, 0.48f);
			}
			else
			{
				Options[i].GetComponent<TextMesh>().color = new Color(0f, 0f, 0f);
			}
		}
	}

	public void RemoveSelector()
	{
		for (int i = 0; i < Options.Length; i++)
		{
			Options[i].GetComponent<TextMesh>().color = new Color(0f, 0f, 0f);
		}
	}

	public bool EnterGuess(ObjectInfo obj, int selected)
	{
		ObjectInfo.OptionData selection = obj.Options[selected];
			
		if (selection.isCorrect)
		{
			Options[selected].GetComponent<TextMesh>().color = new Color(0f, 1f, 0f);
			obj.GetComponent<AudioSource>().Play();
		}
		else
		{
			Options[selected].GetComponent<TextMesh>().color = new Color(1f, 0f, 0f);
		}

		return selection.isCorrect;
	}
}
