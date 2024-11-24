using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float noMatamaosCooldawn = 10f;
    [SerializeField] private float derrapeCooldawn = 1f;
    private bool noMatamaosOnCooldawn = false;
    private bool derrapeOnCooldawn = false;

    private void Update()
    {
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
        noMatamaosOnCooldawn = true;
        var rand = Random.Range(1, 6);
        AudioManager.Instance.PlaySFXOneShot("nosMatamos" + rand);
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
