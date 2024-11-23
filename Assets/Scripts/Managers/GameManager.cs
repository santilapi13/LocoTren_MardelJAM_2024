using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TuristicPoint[] destinations;
    [SerializeField] private TuristicPoint currentDestination;
    [SerializeField] private float minDestinationDistance = 10f;


    [SerializeField] private Image arrow;


    [SerializeField] private float initialPoints;
    [SerializeField] private Player player;
    private float points;
    private float globalTime;
    private float timeTarget;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        points = initialPoints;

        foreach (var destination in destinations)
        {
            destination.gameObject.SetActive(false);
        }

        globalTime = 0;
        GenerateNewDestination();
    }

    private void Update()
    {
        PointArrow();
        if (timeTarget <= 0) return;

        globalTime += Time.deltaTime;
        timeTarget -= Time.deltaTime;
        Debug.Log("$ Tiempo restante: " + timeTarget + " $Puntos: " + points);
        /*if (timeTarget <= 0)
            GameOver();*/
    }

    private void GenerateNewDestination()
    {

        if (currentDestination)
        {
            currentDestination.gameObject.SetActive(false);
        }

        int destinationIndex;
        float distance;

        do
        {
            destinationIndex = Random.Range(0, destinations.Length);
            distance = Vector2.Distance(destinations[destinationIndex].transform.position, player.transform.position);
        } while (distance < minDestinationDistance);

        currentDestination = destinations[destinationIndex];

        float minTime = 10f;
        float maxTime = 60f;

        // Mapear la distancia al rango de tiempo [minTime, maxTime]
        timeTarget = Mathf.Lerp(minTime, maxTime, distance / 100f);

        float variation = Random.Range(-0.1f, 0.1f) * timeTarget;
        timeTarget += variation;

        currentDestination.gameObject.SetActive(true);
        StartCoroutine(WaitingArrive(timeTarget));
        Debug.Log(
            $"Nuevo destino generado: {currentDestination.name}, Distancia: {distance}, Tiempo objetivo: {timeTarget}");
    }

    private IEnumerator WaitingArrive(float pointsToArrive)
    {
        yield return new WaitUntil(() => currentDestination.IsDestination);
        EarnPoints(pointsToArrive);
        GenerateNewDestination();
    }

    private void GameOver()
    {
        // TODO: Implementar Game Over
        Application.Quit();
    }

    public void EarnPoints(float points)
    {
        this.points += points;
    }

    public void LoosePoints(float points)
    {
        this.points -= points;
    }

    private void PointArrow()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 destinationPosition = currentDestination.transform.position;


        Vector2 direction = (destinationPosition - playerPosition).normalized;


        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float arrowOffset = 180f;
        
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle + arrowOffset);
    }
}