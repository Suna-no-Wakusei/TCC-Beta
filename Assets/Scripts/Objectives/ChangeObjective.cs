using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeObjective : MonoBehaviour
{
    [SerializeField] private bool addNewObjective;
    [SerializeField] private bool completeActiveObjective;
    public string objectiveTitle;

    public string objectiveDescription;
    public int objectiveReward;

    public void HandleUpdate()
    {
        if(addNewObjective)
            AddingObjectives();
        else if (completeActiveObjective)
        {
            if(GameManager.instance.objectiveManager.GetObjective() != null)
                FinishingObjectives();
        }
    }

    public void AddingObjectives()
    {
        Objective objective = new Objective();

        objective.title = objectiveTitle;
        objective.description = objectiveDescription;
        objective.reward = objectiveReward;

        GameManager.instance.objectiveManager.AddObjective(objective);

        StartCoroutine(ObjectiveNofitication());
    }

    private IEnumerator ObjectiveNofitication()
    {
        Transform transform = Instantiate(ObjectiveAssets.Instance.pfQuestNot, new Vector2((float)950, (float)293), Quaternion.identity);

        transform.SetParent(GameObject.Find("HUD").transform);

        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)950, (float)293);

        transform.Find("QuestTitle").GetComponent<TextMeshProUGUI>().SetText(GameManager.instance.objectiveManager.GetObjective().title);

        yield return new WaitForSeconds(2f);

        Destroy(transform.gameObject);
    }

    private IEnumerator CompletedNofitication()
    {
        Transform transform = Instantiate(ObjectiveAssets.Instance.pfQuestNot, new Vector2((float)950, (float)293), Quaternion.identity);

        transform.SetParent(GameObject.Find("HUD").transform);

        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)950, (float)293);

        transform.Find("QuestTitle").GetComponent<TextMeshProUGUI>().SetText("Quest completada");

        yield return new WaitForSeconds(2f);

        Destroy(transform.gameObject);
    }

    public void FinishingObjectives()
    {
        GameManager.instance.objectiveManager.CompleteObjective();

        StartCoroutine(CompletedNofitication());
    }
}
