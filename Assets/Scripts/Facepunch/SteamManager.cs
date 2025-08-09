using UnityEngine;
using FishyFacepunch;
using TMPro;
public class SteamManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TMP_Text idText;

    void Start()
    {
        idText.text = FishyFacepunch.FishyFacepunch.CLIENT_HOST_ID.ToString();
        GUIUtility.systemCopyBuffer = idText.text;
    }

  
}
