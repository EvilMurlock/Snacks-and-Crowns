using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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


    [SerializeField]
    SubmitHandler MenuButton;
    [SerializeField]
    string sceneToLoad;
    MenuCode menuCode = new MenuCode();

    class MenuCode : IRegistrable
    {
        public string sceneToLoad;
        public void ToRegister()
        {
            GoToMenu();
        }
        void GoToMenu()
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
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

        gameObject.transform.parent.parent.GetComponentInChildren<MenuManager>().SelectObject(MenuButton.gameObject);
        
        QuitButton.Register(this);

        menuCode.sceneToLoad = sceneToLoad;
        MenuButton.Register(menuCode);
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
