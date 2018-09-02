using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    //Esse script esta sendo usado para todo UI, não só para o canvas !
    public GameObject menu;
    public Text remaining_revives, text_obj;
    public string dead_text, pause_text;
    public Button button_revive, button_continue;

    public void ChangeRemainingRevives(int qtd)
    {
        remaining_revives.text = qtd.ToString();
    }

    public void ShowDieMenu()
    {
        text_obj.text = dead_text;
        ChangeRemainingRevives(Player.player.remaining_revives);                     //Atualiza a quantidade de revives restantes no UI
        if (Player.player.remaining_revives <= 0)
            button_revive.interactable = false;
        else
            button_revive.interactable = true;
        menu.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        text_obj.text = pause_text;
        ChangeRemainingRevives(Player.player.remaining_revives);

        button_continue.gameObject.SetActive(true);
        menu.SetActive(true);
    }

    public void ReviveClicked()
    {
        menu.SetActive(false);
        Camera.main.GetComponent<AdManager>().ShowRewardedAd();             //Mostra um anuncio para reviver
    }

    public void TryAgainClicked()
    {

        Time.timeScale = 1;
        menu.SetActive(false);
        button_continue.gameObject.SetActive(false);
        SceneManager.UnloadSceneAsync(Player.player.player_progress);
        SceneManager.LoadScene(Player.player.player_progress, LoadSceneMode.Additive);
    }

    public void ContinueClicked()
    {
        menu.SetActive(false);

        Time.timeScale = 1;
        button_continue.gameObject.SetActive(false);
        text_obj.text = dead_text;
    }

    public void PauseClicked()
    {
        ShowPauseMenu();
        Time.timeScale = 0;
    }
}
