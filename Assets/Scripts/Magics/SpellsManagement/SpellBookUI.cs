using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;

public class SpellBookUI : MonoBehaviour
{
    public TextMeshProUGUI xpPoints;
    public TextMeshProUGUI proficiency;

    void Update()
    {
        xpPoints.SetText(GameManager.instance.xpPoints.ToString());
        proficiency.SetText(GameManager.instance.magicProficiency.ToString());

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
