using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellsDescUI : MonoBehaviour
{
    public static SpellsDescUI instance { get; private set; }

    private TextMeshProUGUI[] textMeshPro;
    private RectTransform rectTransform;

    private void Awake()
    {
        instance = this;

        textMeshPro = new TextMeshProUGUI[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            textMeshPro[i] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }

    }

    public static SpellsDescUI ShowItemDescription(Vector2 position, Spell spell)
    {
        Transform transform = Instantiate(SpellAssets.Instance.pfSpellDescUI, position, Quaternion.identity);
        transform.SetParent(GameObject.Find("SpellManager").transform);

        SpellsDescUI spellsDescUI = transform.GetComponent<SpellsDescUI>();
        spellsDescUI.SetSpellDesc(spell);

        return spellsDescUI;
    }

    public void SetSpellDesc(Spell spell)
    {
        string spellTitle = spell.SpellName();
        string spellDescription = spell.SpellDesc();

        textMeshPro[0].SetText(spellTitle);
        textMeshPro[1].SetText(spellDescription);
    }

    public void DestroyDescBox()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}
