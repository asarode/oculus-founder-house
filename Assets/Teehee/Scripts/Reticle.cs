using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {
	public Camera CameraFacing;
	private Vector3 originalScale;
	private Color originalColor;
	private GameObject Popover;
	private bool popoverIsShown;
	private float popoverHeight;
	private bool listenForInput;
	[SerializeField]
	private GameObject Notification;
	private int selected;
	private OptionSelector Selector;
	private GameObject currentHit;
	private ObjectInfo objectInfo;
	
	// Use this for initialization
	void Start () {
		originalScale = transform.localScale;
		Popover = GameObject.FindGameObjectWithTag("Popover");
		Selector = Popover.GetComponent<OptionSelector>();
		popoverHeight = Popover.GetComponent<Renderer>().bounds.size.y;
		listenForInput = false;
		popoverIsShown = false;
		selected = -1;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		float distance;
		if (Physics.Raycast (new Ray (CameraFacing.transform.position, CameraFacing.transform.rotation * Vector3.forward), out hit)) {
			distance = hit.distance;
			Debug.Log (hit.transform.gameObject.name);
			objectInfo = hit.transform.gameObject.GetComponent<ObjectInfo>();
			if (currentHit == null || currentHit.GetInstanceID() != hit.transform.gameObject.GetInstanceID())
			{
				ShowPopover(objectInfo, hit);
			}
			Popover.transform.position = PopoverPosition(objectInfo.PopDisplay, hit);
			Popover.transform.LookAt (CameraFacing.transform.position);
			Popover.transform.Rotate (0.0f, 180.0f, 0.0f);
		} else {
			distance = CameraFacing.farClipPlane * 0.95f;
			if (popoverIsShown)
			{
				HidePopover();
			}
		}

		transform.position = CameraFacing.transform.position +
			CameraFacing.transform.rotation * Vector3.forward * distance;
		transform.LookAt (CameraFacing.transform.position);
		transform.Rotate (0.0f, 180.0f, 0.0f);
		if (distance < 10.0f) {
			distance *= 1 + 5*Mathf.Exp (-distance);
		}
		transform.localScale = originalScale * distance;

		if (Input.GetKeyUp("space") && listenForInput)
		{
			selected = (selected + 1) % 3;
			Selector.SelectOption(selected);
		}
		if (Input.GetKeyUp("return") && listenForInput)
		{
			Selector.EnterGuess(objectInfo, selected);
		}
	}

	void ShowPopover (ObjectInfo info, RaycastHit hit)
	{
		Popover.SetActive(true);
		ObjectInfo.OptionData[] wordOptions = info.Options;
		Shuffle (wordOptions);
		Popover.transform.FindChild("Option 1").GetComponent<TextMesh>().text = wordOptions[0].text;
		Popover.transform.FindChild("Option 2").GetComponent<TextMesh>().text = wordOptions[1].text;
		Popover.transform.FindChild("Option 3").GetComponent<TextMesh>().text = wordOptions[2].text;

		listenForInput = true;
		popoverIsShown = true;
		currentHit = hit.transform.gameObject;
	}

	void HidePopover ()
	{
		Popover.SetActive(false);
		Selector.RemoveSelector();
		Notification.SetActive(false);
		listenForInput = false;
		popoverIsShown = false;
		currentHit = null;
		selected = -1;
	}

	Vector3 PopoverPosition (ObjectInfo.Position pos, RaycastHit hit)
	{
		switch (pos)
		{
			case ObjectInfo.Position.Top:
				return TopPosition(hit);
			case ObjectInfo.Position.Left:
				return LeftPosition(hit);
			case ObjectInfo.Position.Right:
				return RightPosition(hit);
			case ObjectInfo.Position.Front:
				return FrontPosition(hit);
			default:
				return new Vector3(1f, 1f, 1f);
		}
	}

	Vector3 TopPosition (RaycastHit hit)
	{
		return new Vector3(hit.transform.position.x, hit.transform.GetComponent<Renderer>().bounds.size.y + popoverHeight * 1.2f, hit.transform.position.z);
	}

	Vector3 LeftPosition (RaycastHit hit)
	{
		return new Vector3(1f, 1f, 1f);
	}

	Vector3 RightPosition (RaycastHit hit)
	{
		return new Vector3(1f, 1f, 1f);
	}

	Vector3 FrontPosition (RaycastHit hit)
	{
		return hit.transform.position + (Vector3.up * popoverHeight * 1f) - (CameraFacing.transform.rotation * Vector3.forward * 1.2f);
	}

	void Shuffle(ObjectInfo.OptionData[] a)
	{
		// Loops through array
		for (int i = a.Length-1; i > 0; i--)
		{
			// Randomize a number between 0 and i (so that the range decreases each time)
			int rnd = Random.Range(0,i);

			ObjectInfo.OptionData temp = a[i];
			
			// Swap the new and old values
			a[i] = a[rnd];
			a[rnd] = temp;
		}
	}
}
