using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField]
    private Image aboutImage;
	[SerializeField]
	private Text titleText;
	[SerializeField]
	private Text bodyText;
    [SerializeField]
    private GameObject infoPanel;

	public Sprite vehicleIcon;
	public Sprite lightingIcon;

	public GameObject defaul;
	public GameObject vehicle;

	[SerializeField]
	private VehicleSettings vehicleSettings;

	public static InfoPanel instance { get; private set; }
	private void Awake()
	{
		// If there is an instance, and it's not me, delete myself.

		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	public void CreateMsg(Message msg)
    {
        infoPanel.SetActive(true);

		aboutImage.sprite = msg.aboutImage;
		titleText.text = msg.title;
		bodyText.text = msg.description;

		if (msg.title == "Vehicle")
		{
			vehicle.SetActive(true);
			defaul.SetActive(false);
			vehicleSettings.OnOpen();
		}
		else
		{
			vehicle.SetActive(false);
			defaul.SetActive(true);
		}
    }

	public void Close()
	{
		if (titleText.text == "Lighting" || titleText.text == "Vehicle")
		{
			ButtonPanel.instance.Home();

			if (titleText.text == "Vehicle")
			{
				vehicleSettings.ApplyAll();
			}
		}

		infoPanel.SetActive(false);
	}
}

public struct Message
{
	public Sprite aboutImage;
	public string title;
	public string description;

	public Message(Sprite aboutImage, string title, string description)
	{
		this.aboutImage = aboutImage;
		this.title = title;
		this.description = description;
	}
}
