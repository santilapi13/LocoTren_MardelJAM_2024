using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class set_values_end : MonoBehaviour
{
    public TMP_Text duracion_text;
    public TMP_Text puntuacion_text;
    private float duracion;
    private float puntuacion;

    // Start is called before the first frame update
    void Start()
    {
        duracion = PointsTraker.Instance.time;
        puntuacion = PointsTraker.Instance.points;
        assign_values();
    }

    private void assign_values()
    {
        duracion_text.text = duracion.ToString();
        puntuacion_text.text = puntuacion.ToString();
    }
}
