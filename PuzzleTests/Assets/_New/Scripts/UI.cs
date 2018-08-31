using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text remaining_revives;
    public Button button_revive;

    public void ChangeRemainingRevives(int qtd)
    {
        remaining_revives.text = qtd.ToString();
    }
}
