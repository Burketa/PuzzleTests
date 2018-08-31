using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{

    public void ShowSimpleAd()
    {
        //Time.timeScale = 0;
        if (Advertisement.IsReady("video"))
        {
            Advertisement.Show("video", new ShowOptions() { resultCallback = HandleResult});
        }
	}

    public void ShowRewardedAd()
    {

        //Time.timeScale = 0;
        if (Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleResult });
        }
    }

    private void HandleResult(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Finished:
                Debug.Log("Viu o anuncio todo");
                //Time.timeScale = 1;
                break;
            case ShowResult.Skipped:
                Debug.Log("Clicou em skip");

                //Time.timeScale = 1;
                break;
            case ShowResult.Failed:
                Debug.Log("Falhou");

                //Time.timeScale = 1;
                break;
        }
    }
}
