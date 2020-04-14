using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestManager : Singleton<PointOfInterestManager>
{
    public Camera cam;
    public List<PointOfInterest> POIs;
    public Transform player;

    private void Awake()
    {
        POIs = FindObjectsOfType<PointOfInterest>().ToList();
        //POIs = GetComponentsInChildren<PointOfInterest>().ToList();
    }

    public List<string> GetPOIsList()
    {
        return POIs.Select(x => x.gameObject.name).ToList();
    }

    public void TakeCameraToPoi(string poiname)
    {
        PointOfInterest poi = POIs.Where(x => x.name == poiname).First();

        poi.Begin();

        cam.transform.SetParent(poi.transform);
        cam.transform.localPosition = Vector3.zero;
        cam.transform.localRotation = Quaternion.identity;
    }
}
