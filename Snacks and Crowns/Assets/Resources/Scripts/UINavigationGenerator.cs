using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Custom UI navigation generator, but is UNUSED because it was not generating desired connections
/// </summary>
public class UINavigationGenerator : MonoBehaviour
{
    List<Selectable> selectables = new List<Selectable>();
    void Start()
    {
        GenerateNavigation();
    }
    void UpdateSelectables()
    {
        selectables = new List<Selectable>();
        Selectable[] ss = gameObject.GetComponentsInChildren<Selectable>();
        foreach (Selectable s in ss)
        {
            selectables.Add(s);
        }
    }
    void GenerateNavigation()
    {
        UpdateSelectables();
        foreach(Selectable sel in selectables)
        {
            Navigation myNavigation = new Navigation();
            myNavigation.mode = Navigation.Mode.Explicit;
            myNavigation.selectOnUp = ClosestInAngle(sel, -45, 45);
            myNavigation.selectOnRight = ClosestInAngle(sel, 45, 135);
            myNavigation.selectOnDown = ClosestInAngle(sel, 135, 225);
            myNavigation.selectOnLeft = ClosestInAngle(sel, 225, 335);
            sel.navigation = myNavigation;
        }
    }

    // UNITY DOES IT NOT BASED ON ANGLES, BUT axis based, candidates are all objects more in that direction than us,
    // but it seems that distance is weight by the angle, and how far awway from striaght it is
    Selectable ClosestInAngle(Selectable me, float minAngle, float maxAngle)
    {
        
        List<Selectable> candidates = new List<Selectable>();
        foreach (Selectable other in selectables)
        {
            float angle = Vector2.SignedAngle((Vector2)me.transform.position - (Vector2)other.transform.position, Vector2.down);
            if (angle < 0)
                angle += 360;
            if (other == me)
                continue;
            if (minAngle > 0 && angle > maxAngle)
                continue;
            if (minAngle > 0 && angle <= minAngle)
                continue;
            if (minAngle < 0 && angle <= (minAngle+360) && angle > maxAngle)
                continue;

            candidates.Add(other);
        }

        if (candidates.Count == 0)
            return null;
        Selectable closest = candidates[0];
        float minDistance = Vector3.Distance(me.transform.position, closest.transform.position);
        foreach (Selectable candidate in candidates)
        {
            float distance = Vector3.Distance(me.transform.position, candidate.transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                closest = candidate;
            }
        }
        return closest;
    }
}
