using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeObjective : MonoBehaviour
{
    public bool addNewObjective;
    public bool completeActiveObjective;
    public int objectiveID;
    public string objectiveTitle;

    public string objectiveDescription;
    public int objectiveReward;

    public Coroutine addingCoroutine;
    public Coroutine completingCoroutine;

    public void AddingObjectives()
    {
        if (!addNewObjective) return;

        Objective objective = new Objective();

        objective.id = objectiveID;
        objective.title = objectiveTitle;
        objective.description = objectiveDescription;
        objective.reward = objectiveReward;

        GameManager.instance.objectiveManager.AddObjective(objective);

        addingCoroutine = StartCoroutine(ObjectiveNofitication());
    }

    private IEnumerator ObjectiveNofitication()
    {
        GameManager.instance.sfxManager.PlayPaper();

        Transform transform = Instantiate(ObjectiveAssets.Instance.pfQuestNot, new Vector2((float)950, (float)293), Quaternion.identity);

        transform.SetParent(GameObject.Find("HUD").transform);

        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)950, (float)293);
        transform.localScale = new Vector3(0.827901f, 0.827901f, 0.827901f);

        transform.Find("QuestTitle").GetComponent<TextMeshProUGUI>().SetText(GameManager.instance.objectiveManager.GetObjective().title);

        yield return new WaitForSeconds(2f);

        Destroy(transform.gameObject);
    }

    private IEnumerator CompletedNofitication()
    {
        GameManager.instance.sfxManager.PlayPaper();

        Transform transform = Instantiate(ObjectiveAssets.Instance.pfQuestNot, new Vector2((float)950, (float)293), Quaternion.identity);

        transform.SetParent(GameObject.Find("HUD").transform);

        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)950, (float)293);
        transform.localScale = new Vector3(0.827901f, 0.827901f, 0.827901f);

        transform.Find("QuestTitle").GetComponent<TextMeshProUGUI>().SetText("Quest completada");

        yield return new WaitForSeconds(2f);

        Destroy(transform.gameObject);
    }

    public void FinishingObjectives()
    {
        if (!completeActiveObjective) return;

        GameManager.instance.objectiveManager.CompleteObjective();

        completingCoroutine = StartCoroutine(CompletedNofitication());
    }
}
