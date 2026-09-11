using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class VoiceColorChanger : MonoBehaviour
{
    private KeywordRecognizer recognizer;
    private Renderer rend;
    private Material materialCubo;

    private Dictionary<string, Color> colores = new Dictionary<string, Color>()
    {
        { "rojo", Color.red },
        { "verde", Color.green },
        { "azul", Color.blue },
        { "amarillo", Color.yellow }
    };

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (rend == null)
        {
            Debug.LogError("No se encontró Renderer.");
            return;
        }

        // Obtiene una instancia del material asignado al Cube
        materialCubo = rend.material;

        // Prueba visual inicial
        CambiarColor(Color.red);

        recognizer = new KeywordRecognizer(
            colores.Keys.ToArray(),
            ConfidenceLevel.Low
        );

        recognizer.OnPhraseRecognized += ReconocerColor;
        recognizer.Start();

        Debug.Log("Reconocimiento de voz iniciado.");
    }

    void ReconocerColor(PhraseRecognizedEventArgs args)
    {
        Debug.Log("VOZ RECONOCIDA: " + args.text);

        if (colores.TryGetValue(args.text, out Color nuevoColor))
        {
            CambiarColor(nuevoColor);
            Debug.Log("COLOR CAMBIADO A: " + args.text);
        }
    }

    void CambiarColor(Color nuevoColor)
    {
        if (materialCubo.HasProperty("_BaseColor"))
        {
            materialCubo.SetColor("_BaseColor", nuevoColor);
        }
        else
        {
            materialCubo.color = nuevoColor;
        }
    }

    void OnDestroy()
    {
        if (recognizer != null)
        {
            recognizer.OnPhraseRecognized -= ReconocerColor;

            if (recognizer.IsRunning)
                recognizer.Stop();

            recognizer.Dispose();
        }
    }
}