using System;
using System.Collections;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float noMatamaosCooldawn = 10f;
    [SerializeField] private float derrapeCooldawn = 1f;
    private bool noMatamaosOnCooldawn = false;
    private bool derrapeOnCooldawn = false;

    private void Update()
    {
        Debug.Log(player.SpeedPercentage);
        if (!noMatamaosOnCooldawn && player.SpeedPercentage > 0.8f)
        {
            StartCoroutine(NoMatamaos());
        }

        if (!derrapeOnCooldawn && player.Drifting)
        {
            StartCoroutine(Derrape());
        }
        
    }

    private IEnumerator NoMatamaos()
    {
        Debug.Log("No matamaos");
        noMatamaosOnCooldawn = true;
        AudioManager.Instance.PlaySFXOneShot("noMatamos");
        yield return  new WaitForSeconds(noMatamaosCooldawn);
        noMatamaosOnCooldawn = false;
    }

    private IEnumerator Derrape()
    {
        derrapeOnCooldawn = true;
        AudioManager.Instance.PlaySFXOneShot("frenada");
        yield return  new WaitForSeconds(derrapeCooldawn);
        derrapeOnCooldawn = false;
    }
}
