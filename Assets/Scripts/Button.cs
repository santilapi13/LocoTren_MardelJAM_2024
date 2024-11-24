using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public string hoverSFX = "hover_botones"; // Sonido al hacer hover
    public string clickSFX = "click_botones"; // Sonido al hacer click
    public Color normalColor = Color.white; // Color normal del botón
    public Color hoverColor = Color.gray; // Color al hacer hover
    public Color clickColor = Color.black; // Color al presionar

    private Button button; // Referencia al botón
    private Image buttonImage; // Imagen del botón para cambiar el color
    private Text buttonText; // Referencia al texto del botón

    private void Awake()
    {
        // Obtener el componente de texto
        buttonText = GetComponentInChildren<Text>();

        if (buttonText != null)
        {
            buttonText.color = normalColor; // Inicializar con el color normal
        }
        else
        {
            Debug.LogWarning("No se encontró un componente Text en los hijos del botón.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Cambiar color y reproducir SFX al hacer hover
        if (buttonImage != null)
        {
            buttonText.color = hoverColor;
        }
        AudioManager.Instance.PlaySFXOneShot(hoverSFX);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restaurar color al salir del hover
        if (buttonImage != null)
        {
            buttonText.color = normalColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Cambiar color y reproducir SFX al hacer click
        if (buttonImage != null)
        {
            buttonText.color = clickColor;
            Invoke(nameof(ResetColor), 0.1f); // Retornar al color normal después de un breve tiempo
        }
        AudioManager.Instance.PlaySFXOneShot(clickSFX);
    }

    private void ResetColor()
    {
        if (buttonImage != null)
        {
            buttonText.color = normalColor;
        }
    }
}
