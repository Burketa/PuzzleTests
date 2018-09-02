using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackHole : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D trigger_obj)
    {
        if (trigger_obj.tag.Equals("Player"))
        {
            Player.player.player_progress++;
            if (Player.player.player_progress < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.UnloadSceneAsync(Player.player.player_progress - 1);
                SceneManager.LoadScene(Player.player.player_progress, LoadSceneMode.Additive);

                Player.player.BreakPlayer();
                Player.player.transform.position = Vector2.zero;
                Player.player.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }
}
