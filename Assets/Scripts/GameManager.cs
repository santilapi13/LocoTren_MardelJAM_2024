using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    
    [SerializeField] private Transform[] destinations;
    [SerializeField] private Transform currentDestination;
    [SerializeField] private float minDestinationDistance = 10f;
    
    [SerializeField] private float initialPoints;
    [SerializeField] private Player player;
    private float points;
    private float globalTime;
    private float timeTarget;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        points = initialPoints;
        globalTime = 0;
        GenerateNewDestination();
    }

    private void Update() {
        globalTime += Time.deltaTime;
        timeTarget -= Time.deltaTime;

        if (timeTarget <= 0)
            GameOver();
    }

    private void GenerateNewDestination() {
        int destinationIndex;
        float distance;

        do {
            destinationIndex = Random.Range(0, destinations.Length);
            distance = Vector2.Distance(destinations[destinationIndex].position, player.transform.position);
        } while (distance < minDestinationDistance);
        
        currentDestination = destinations[destinationIndex];
        
        float minTime = 10f;
        float maxTime = 60f; 

        // Mapear la distancia al rango de tiempo [minTime, maxTime]
        timeTarget = Mathf.Lerp(minTime, maxTime, distance / 100f);
        
        float variation = Random.Range(-0.1f, 0.1f) * timeTarget;
        timeTarget += variation;

        Debug.Log($"Nuevo destino generado: {currentDestination.name}, Distancia: {distance}, Tiempo objetivo: {timeTarget}");
    }
    
    private void GameOver() {
        // TODO: Implementar Game Over
        Application.Quit();
    }

    public void EarnPoints(float points) {
        this.points += points;
    }

    public void LoosePoints(float points) {
        this.points -= points;
    }
}