using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueText
{
    [SerializeField] List<string> lines;
    [SerializeField] List<Sprite> icons;

    public List<string> Lines
    {
        get { return lines; }
    }

    public List<Sprite> Icons
    {
        get { return icons; }
    }
}
