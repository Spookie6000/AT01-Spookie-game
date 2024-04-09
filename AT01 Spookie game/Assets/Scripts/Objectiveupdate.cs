using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Objectiveupdate : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;

    // Start is called before the first frame update
    void Start()
    {
        EventManger.unlockDoorEvent += UpdateObjectiveText;
    }
    private void UpdateObjectiveText(int doorID)
    {
        objectiveText.text = "FIND THE DOOR";
    }

    private void OnDestroy()
    {
        EventManger.unlockDoorEvent -= UpdateObjectiveText;
    }
}