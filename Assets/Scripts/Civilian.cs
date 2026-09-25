using UnityEngine;

/// <summary>État simple d'un civil pouvant être escorté.</summary>
public class Civilian : MonoBehaviour
{
    public string displayName="Civil";
    public bool isCrisisLead, rescued, evacuated;
    public void SetRescued(){if(isCrisisLead||evacuated)return;rescued=true;}
    public void SetEvacuated(){if(rescued&&!isCrisisLead){evacuated=true;gameObject.SetActive(false);}}
}
