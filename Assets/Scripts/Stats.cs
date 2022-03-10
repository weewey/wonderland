using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
  public GameObject StatsPanel;
  public GameObject StatsButton;

  public void OpenPanel()
  {
    // print("Open panel");
    if (StatsPanel != null) {
      StatsPanel.SetActive(true);
      if (StatsButton != null) {
        StatsButton.SetActive(false);
      }
    }
  }

  public void ClosePanel()
  {
    // print("Close panel");
    if (StatsPanel != null) {
      StatsPanel.SetActive(false);
      if (StatsButton != null) {
        StatsButton.SetActive(true);
      }
    }
  }
}
