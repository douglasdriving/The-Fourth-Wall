using UnityEngine;

public class Machine : MonoBehaviour
{
  [SerializeField] Chrystal2InsertEvents chrystal2InsertEvents;
  int chrystalsInserted = 0;

  public void AddChrystal()
  {
    chrystalsInserted++;
    if (chrystalsInserted == 2)
    {
      chrystal2InsertEvents.StartEvents();
    }
  }
}