using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public Text[] Textes;

    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<UIManager>();
            return instance;
        }
    }

    private void Start()
    {
        for (int i = 0; i < Textes.Length; i++)
        {
            Textes[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameRunning)
        {
            Textes[(int)Spawner.Instance.GetTeamWithMostTickets()].gameObject.SetActive(true);
        }
    }
}
