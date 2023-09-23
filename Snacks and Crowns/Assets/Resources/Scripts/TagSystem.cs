using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TagSystem : MonoBehaviour
{
    [SerializeReference] List<string> tags = new List<string>();

    public List<string> GetTags()
    {
        return new List<string>(tags);
    }
    public void AddTag(string tag)
    {
        if (!tags.Contains(tag)) tags.Insert(0, tag);
    }

    public void AddTags(List<string> addTags)
    {
        foreach(string tag in addTags)
        {
            if (!tags.Contains(tag)) tags.Insert(0, tag);
        }
    }
    public void RemoveTag(string tag)
    {
        if (tags.Contains(tag)) tags.Remove(tag);
    }

    public void RemoveTags(List<string> addTags)
    {
        foreach (string tag in addTags)
        {
            if (tags.Contains(tag)) tags.Remove(tag);
        }
    }
    public bool HasTag(string tag)
    {
        return tags.Contains(tag);
    }
    public bool HasTags(List<string> tagSubset)
    {
        bool HasTags = true;

        foreach (string tag in tagSubset)
        {
            if (!tags.Contains(tag))
            {
                HasTags = false;
                break;
            }
        }
        return HasTags;
    }
    public bool SameTags(List<string> tagSet)
    {
        bool SameTags = true;

        foreach (string tag in tagSet)
        {
            if (!tags.Contains(tag))
            {
                SameTags = false;
                break;
            }
        }
        foreach (string tag in tags)
        {
            if (!tagSet.Contains(tag))
            {
                SameTags = false;
                break;
            }
        }
        return SameTags;
    }
}
