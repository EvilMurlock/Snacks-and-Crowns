using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IRegistrable{
    public void ToRegister();
}
public class SubmitHandler : MonoBehaviour, ISubmitHandler
{
    protected UnityEvent buttonEvent = new UnityEvent();
    // Start is called before the first frame update
    public void OnSubmit(BaseEventData eventData)
    {
        buttonEvent.Invoke();
    }
    public void Register(IRegistrable r)
    {
        buttonEvent.AddListener(r.ToRegister);
    }
}
