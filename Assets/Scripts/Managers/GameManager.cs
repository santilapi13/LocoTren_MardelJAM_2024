using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [SerializeField] private TuristicPoint[] destinations;
    [SerializeField] private TuristicPoint currentDestination;
    [SerializeField] private float minDestinationDistance = 10f;

    [SerializeField] private float minTimeTarget = 10f;
    [SerializeField] private float maxTimeTarget = 30f;
    
    [SerializeField] private float initialPoints;
    [SerializeField] private Player player;
    private float globalTime;
    private float points;
    private float timeTarget;

    [Header("UI Elements")]
    [SerializeField] private Image arrow;
    [SerializeField] private Image speedMeter;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI pointsText;

    public float PlayerPercentage => player.SpeedPercentage;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        points = initialPoints;

        foreach (var destination in destinations) destination.gameObject.SetActive(false);
        globalTime = 0;
        GenerateNewDestination();
    }

    private void Update() {
        PointArrow();
        Fillbar();
        Text();
        
        if (timeTarget <= 0) return;

        globalTime += Time.deltaTime;
        timeTarget -= Time.deltaTime;
        if (timeTarget <= 0)
            GameOver();
    }

    private void GenerateNewDestination() {
        if (currentDestination) currentDestination.gameObject.SetActive(false);

        int destinationIndex;
        float distance;

        do {
            destinationIndex = Random.Range(0, destinations.Length);
            distance = Vector2.Distance(destinations[destinationIndex].transform.position, player.transform.position);
        } while (distance < minDestinationDistance);

        currentDestination = destinations[destinationIndex];

        // Mapear la distancia al rango de tiempo [minTime, maxTime]
        timeTarget = Mathf.Lerp(minTimeTarget, maxTimeTarget, distance / 100f);

        var variation = Random.Range(-0.1f, 0.1f) * timeTarget;
        timeTarget += variation;

        currentDestination.gameObject.SetActive(true);
        StartCoroutine(WaitingArrive(timeTarget * 100));
    }

    private IEnumerator WaitingArrive(float pointsToArrive) {
        yield return new WaitUntil(() => currentDestination.IsDestination);
        EarnPoints(pointsToArrive);
        GenerateNewDestination();
    }

    private void GameOver() {
        PointsTraker.Instance.points = points;
        PointsTraker.Instance.time = globalTime;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    

    public void EarnPoints(float points) {
        this.points += points;
        AudioManager.Instance.PlaySFXOneShot("dinero");
    }

    public void LoosePoints(float points)
    {
        this.points -= points;
    }

    public void Crash(float slowTime, float slowAmount, float pointsPenalty) {
        LoosePoints(pointsPenalty);
        player.Slow(slowAmount);
    }

    private void PointArrow() {
        if(!currentDestination) return;
        Vector2 playerPosition = player.transform.position; 
        Vector2 destinationPosition = currentDestination.transform.position;
        var direction = (destinationPosition - playerPosition).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //var arrowOffset = 270f;

        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Fillbar()
    {
        speedMeter.fillAmount =  player.SpeedPercentage / 1;
    }

    private void Text()
    {
        int seconds = Mathf.FloorToInt(timeTarget); // Parte entera (segundos)
        int milliseconds = Mathf.FloorToInt((timeTarget % 1) * 100); // Parte decimal como milisegundos
        timer.text = $"{seconds}' {milliseconds}''";
        pointsText.text = "$ " + points;
    }
}