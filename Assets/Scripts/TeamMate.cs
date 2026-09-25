using UnityEngine;

/// <summary>Comportement volontairement simple pour les équipiers alliés.</summary>
public class TeamMate : MonoBehaviour
{
    public int memberIndex;
    public static int order; // 0 suivre, 1 rester, 2 accompagner
    Transform player; Civilian target;
    void Start(){var p=GameObject.Find("Joueur");if(p)player=p.transform;}
    public void SetTarget(Civilian c){target=c;}
    void Update(){
        if(player==null||order==1)return;
        Vector3 destination=order==2&&target!=null?target.transform.position:player.position+player.right*(memberIndex==0?-1.5f:1.5f)-player.forward*1.7f;
        destination.y=transform.position.y;
        if(Vector3.Distance(transform.position,destination)>.8f)transform.position=Vector3.MoveTowards(transform.position,destination,2.2f*Time.deltaTime);
        Vector3 look=destination-transform.position;if(look.sqrMagnitude>.01f)transform.forward=Vector3.Lerp(transform.forward,look.normalized,Time.deltaTime*4);
    }
}
