using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameEndMenu : MonoBehaviour, IRegistrable
{
    Color winColor = Color.green;
    Color loseColor = Color.red;
    [SerializeField]
    Image background;
    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    SubmitHandler QuitButton;

    const string winText = "You WON!!!";
    const string loseText = "You LOST!!!";
    public void Inicialize(bool victory)
    {
        if (victory)
        {
            background.color = winColor;
            text.text = winText;
        }
        else
        {
            background.color = loseColor;
            text.text = loseText;
        }

        gameObject.transform.parent.parent.GetComponentInChildren<MenuManager>().SelectObject(QuitButton.gameObject);
        QuitButton.Register(this);
    }

    public void ToRegister()
    {
        Quit();
    }
    void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
