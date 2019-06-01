using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public Text[] Textes;

    public Slider[] TicketsSlider;
    public TMPro.TextMeshProUGUI[] TicketsCount;

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

        Spawner.Instance.SetupTicketUpdateAction(UpdateTeamTickets);
        TicketsSlider[0].maxValue = GameManager.Instance.ticketsPerTeam;
        TicketsSlider[1].maxValue = GameManager.Instance.ticketsPerTeam;
        TicketsSlider[0].value = GameManager.Instance.ticketsPerTeam;
        TicketsSlider[1].value = GameManager.Instance.ticketsPerTeam;
        TicketsCount[0].text = GameManager.Instance.ticketsPerTeam.ToString();
        TicketsCount[1].text = GameManager.Instance.ticketsPerTeam.ToString();

    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameRunning)
        {
            Textes[(int)Spawner.Instance.GetTeamWithMostTickets()].gameObject.SetActive(true);
        }
    }

    void UpdateTeamTickets(Team team, int amount)
    {
        TicketsSlider[(int)team].value = amount;
        TicketsCount[(int)team].text = amount.ToString();
    }
}
