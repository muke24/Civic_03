using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public Image aboutImage;
    public Text titleText;
    public Text bodyText;

    public void CreateMsg(InfoPanelObject obj)
    {
        aboutImage.sprite = obj.aboutSprite;
        titleText.text = obj.title;
        bodyText.text = obj.description;
    }
}
