using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackboardBonus : MonoBehaviour
{
    [SerializeField] private Material backboardMaterial;
    [SerializeField] private float interval = 20f;
    
    private Dictionary<Color, int> backboardColors;
    private Color? currentColor = null;
    
    public bool wasHit;

    private void Start()
    {
        backboardMaterial = GetComponent<MeshRenderer>().material;
        backboardColors = new Dictionary<Color, int>
        {
            { new Color(1f, 1f, 0f), 8 },
            { new Color(0f, 0f, 1f), 6 },
            { new Color(0f, 1f, 0f), 4 }
        };

        StartCoroutine(EmissionPulseRoutine());
    }

    IEnumerator EmissionPulseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            UIFeedback.Instance.ShowBackboardBonus();
            EnableEmission();
            
            wasHit = false;

            while (!wasHit)
                yield return null;

            DisableEmission();
            wasHit = false;
        }
    }

    private void EnableEmission()
    {
        if (backboardMaterial != null && backboardColors.Count > 0)
        {
            List<Color> keys = new List<Color>(backboardColors.Keys);
            int index = Random.Range(0, keys.Count);
            Color chosenColor = keys[index];

            Color emissionColor = chosenColor * 2f;
            backboardMaterial.EnableKeyword("_EMISSION");
            backboardMaterial.SetColor("_EmissionColor", emissionColor);

            currentColor = chosenColor;
        }
    }

    private void DisableEmission()
    {
        if (backboardMaterial != null)
        {
            backboardMaterial.SetColor("_EmissionColor", Color.black);
            backboardMaterial.DisableKeyword("_EMISSION");
        }

        currentColor = null;
    }

    public int GetBonusValue()
    {
        if (currentColor.HasValue && backboardColors.ContainsKey(currentColor.Value))
            return backboardColors[currentColor.Value];
        return 0;
    }
}
