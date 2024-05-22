using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;
using UnityEngine.UI;

public class Objectiveupdate : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image imageCooldown;

    //variables for cooldowntimer
    private bool iscooldown = false;
    private float cooldownTimer = 0;
    private

  
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
