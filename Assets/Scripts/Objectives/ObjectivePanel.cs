using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectivePanel : MonoBehaviour
{
    private Objective objective;
    private ObjectiveManager objectiveManager;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI rewardText;

    public void SetObjective(ObjectiveManager thatObjectiveManager)
    {
        objectiveManager = thatObjectiveManager;

        InventoryManager.Instance.OnPause += RefreshObjective;
    }

    private void DisableObject()
    {
        GameManager.instance.objectiveUI.gameObject.SetActive(false);
    }

    private void EnableObject()
    {
        GameManager.instance.objectiveUI.gameObject.SetActive(true);

        objective = GameManager.instance.objectiveManager.GetObjective();

        titleText.SetText(objective.title);
        descriptionText.SetText(objective.description);

        rewardText.SetText(objective.reward.ToString());

        if (objective.reward == 0)
        {
            GameManager.instance.objectiveUI.rewardText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            GameManager.instance.objectiveUI.rewardText.transform.parent.gameObject.SetActive(true);
        }
        
    }

    private void RefreshObjective()
    {
        if(GameManager.instance.objectiveManager.GetObjective() != null)
        {
            EnableObject();
        }
        else
        {
            DisableObject();
        }
    }
}
