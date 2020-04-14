using UnityEngine;
using UnityEngine.UI;

public class SetupUIManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform ScrollviewContent;

    private void Start()
    {
        var pois = PointOfInterestManager.Instance.GetPOIsList();

        for (int i = 0; i < pois.Count; i++)
        {
            InstantiateButton(pois[i]);
        }
    }

    void InstantiateButton(string name)
    {
        var newbutton = Instantiate(buttonPrefab);

        newbutton.transform.SetParent(ScrollviewContent);
        // button text
        newbutton.GetComponentInChildren<Text>().text = name;

        // Setup button's properties
        var button = newbutton.GetComponent<Button>();
        button.onClick.AddListener(
            new UnityEngine.Events.UnityAction(
                () => PointOfInterestManager.Instance.TakeCameraToPoi(name)));
    }
}
