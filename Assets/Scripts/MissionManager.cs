using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>Gère les objectifs, les issues, les choix et l'évaluation.</summary>
public class MissionManager : MonoBehaviour
{
    PlayerController player; GameUI ui; List<Civilian> civilians=new List<Civilian>(); float elapsed; int negotiationScore=0, choices=0; bool leadCalmed;
    public bool IsFinished{get;private set;} public Vector3 ExitPosition=>new Vector3(0,0,-8.8f);
    public int Rescued=>civilians.Count(c=>c.rescued&&!c.isCrisisLead);
    public int Evacuated=>civilians.Count(c=>c.evacuated);
    public int CivilianCount=>civilians.Count(c=>!c.isCrisisLead);
    public void Setup(PlayerController p,GameUI interfaceUI){player=p;ui=interfaceUI;civilians=FindObjectsOfType<Civilian>().ToList();ui.ShowMenu();}
    void Update(){if(IsFinished||ui==null||!ui.InGame)return;elapsed+=Time.deltaTime;if(elapsed>=600)Finish(false,"Le temps est écoulé. La mission prend fin.");ui.UpdateHUD(this);}
    public void StartMission(){elapsed=0;IsFinished=false;ui.StartGame();}
    public void Rescue(Civilian c){if(IsFinished||c==null||c.isCrisisLead||c.rescued)return;c.SetRescued();ui.ShowMessage(c.displayName+" est avec vous. Rejoignez la sortie verte.");}
    public void Evacuate(){if(IsFinished)return;var carried=civilians.Where(c=>c.rescued&&!c.evacuated).ToList();if(carried.Count==0){ui.ShowMessage("Approchez-vous d'un civil et pressez E pour l'accompagner.");return;}foreach(var c in carried)c.SetEvacuated();ui.ShowMessage("Civils mis à l'abri : "+Evacuated+" / "+CivilianCount);if(Evacuated>=CivilianCount)Finish(true,"Tous les civils sont en sécurité.");}
    public void TryNegotiate(){if(IsFinished)return;var lead=civilians.FirstOrDefault(c=>c.isCrisisLead);if(lead==null)return;if(Vector3.Distance(player.transform.position,lead.transform.position)>4f){ui.ShowMessage("Approchez du responsable de crise pour parler.");return;}ui.ShowDialogue(leadCalmed);}
    public void ChooseDialogue(int choice){choices++;if(choice==1||choice==2){negotiationScore+=choice==1?3:2;leadCalmed=true;ui.HideDialogue();ui.ShowMessage(choice==1?"Le dialogue apaise la situation. Continuez l'évacuation.":"Votre proposition ouvre une voie de sortie sûre.");}else{negotiationScore-=1;ui.HideDialogue();ui.ShowMessage("La tension augmente. Prenez le temps d'écouter et protégez les civils.");}}
    public void CycleTeamOrder(){TeamMate.order=(TeamMate.order+1)%3;var names=new[]{"Suivre","Rester en position","Accompagner un civil"};var civ=civilians.Where(c=>!c.isCrisisLead&&!c.evacuated).OrderBy(c=>Vector3.Distance(player.transform.position,c.transform.position)).FirstOrDefault();foreach(var tm in FindObjectsOfType<TeamMate>())tm.SetTarget(civ);ui.ShowMessage("Ordre équipe : "+names[TeamMate.order]+(TeamMate.order==2&&civ!=null?" — "+civ.displayName:""));}
    void Finish(bool success,string message){if(IsFinished)return;IsFinished=true;Cursor.lockState=CursorLockMode.None;float score=Evacuated*30+negotiationScore*5-Mathf.Max(0,elapsed-180)/15f;string grade=success?(score>=85?"Intervention exemplaire":score>=55?"Bonne intervention":"Intervention maîtrisée"):"Mission interrompue";ui.ShowResult(success,message,Evacuated,CivilianCount,elapsed,grade,choices);}
    public void Restart(){UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);}
}
