using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class LoadSceneOnPress : MonoBehaviour, ISubmitHandler
{
    [SerializeField]
    string sceneToLoad;
    public void OnSubmit(BaseEventData eventData)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
