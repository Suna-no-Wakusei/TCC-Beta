using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;

public class SpellBookUI : MonoBehaviour
{
    public TextMeshProUGUI xpPoints;

    void Update()
    {
        xpPoints.SetText(GameManager.instance.xpPoints.ToString());

        foreach (Transform transform in this.transform)
        {
            if(transform.GetComponent<Button_UI>() != null)
            {
                transform.GetComponent<Button_UI>().ClickFunc = () =>
                {
                    GameManager.instance.spellBook.AddSpell(transform.GetComponent<LearnSpell>().spell);
                };
            }
        }
    }
}
