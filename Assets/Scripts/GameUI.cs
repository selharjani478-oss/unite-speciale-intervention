using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>Interface entière créée par code pour éviter tout asset externe.</summary>
public class GameUI:MonoBehaviour
{
    Canvas canvas; Font font; GameObject menuPanel,optionsPanel,dialoguePanel,resultPanel,hudPanel; Text hudText,messageText,dialogueText,resultText; Slider volumeSlider,sensitivitySlider; MissionManager mission;
    public bool InGame{get;private set;}
    void Awake(){canvas=GetComponent<Canvas>();font=Resources.GetBuiltinResource<Font>("Arial.ttf");}
    void Start(){BuildPanels();}
    public void ShowMenu(){if(menuPanel==null)BuildPanels();InGame=false;menuPanel.SetActive(true);optionsPanel.SetActive(false);hudPanel.SetActive(false);dialoguePanel.SetActive(false);resultPanel.SetActive(false);Cursor.lockState=CursorLockMode.None;}
    void BuildPanels(){
        menuPanel=Panel("Menu principal");Label(menuPanel.transform,"Unité spéciale : Intervention",36,0,190,700,70);
        Button(menuPanel.transform,"Jouer",0,80,230,48,()=>{if(mission==null)mission=FindObjectOfType<MissionManager>();mission.StartMission();});
        Button(menuPanel.transform,"Options",0,20,230,48,()=>{menuPanel.SetActive(false);optionsPanel.SetActive(true);});
        Button(menuPanel.transform,"Quitter",0,-40,230,48,()=>Application.Quit());
        Label(menuPanel.transform,"Protégez les civils. Écoutez. Choisissez une issue sûre.",16,0,-130,900,45);
        optionsPanel=Panel("Options");Label(optionsPanel.transform,"Options",34,0,170,500,55);
        Label(optionsPanel.transform,"Volume",18,-120,85,200,35);volumeSlider=Slider(optionsPanel.transform,40,85);volumeSlider.value=PlayerPrefs.GetFloat("volume",.8f);volumeSlider.onValueChanged.AddListener(v=>{AudioListener.volume=v;PlayerPrefs.SetFloat("volume",v);});
        Label(optionsPanel.transform,"Sensibilité souris",18,-120,20,250,35);sensitivitySlider=Slider(optionsPanel.transform,40,20);sensitivitySlider.minValue=.5f;sensitivitySlider.maxValue=5;sensitivitySlider.value=PlayerPrefs.GetFloat("sensitivity",2.2f);sensitivitySlider.onValueChanged.AddListener(v=>{PlayerPrefs.SetFloat("sensitivity",v);var p=FindObjectOfType<PlayerController>();if(p)p.mouseSensitivity=v;});
        Button(optionsPanel.transform,"Retour",0,-100,200,45,()=>{optionsPanel.SetActive(false);menuPanel.SetActive(true);});
        hudPanel=new GameObject("HUD");hudPanel.transform.SetParent(canvas.transform,false);Stretch(hudPanel.GetComponent<RectTransform>()??hudPanel.AddComponent<RectTransform>());
        hudText=Label(hudPanel.transform,"",18,0,-290,1050,75,TextAnchor.UpperLeft);hudText.rectTransform.anchorMin=new Vector2(0,1);hudText.rectTransform.anchorMax=new Vector2(0,1);hudText.rectTransform.pivot=new Vector2(0,1);hudText.rectTransform.anchoredPosition=new Vector2(24,-22);
        messageText=Label(hudPanel.transform,"",20,0,0,850,80);messageText.rectTransform.anchorMin=new Vector2(.5f,.82f);messageText.rectTransform.anchorMax=new Vector2(.5f,.82f);
        Label(hudPanel.transform,"+",28,0,0,40,40);
        dialoguePanel=Panel("Dialogue");dialogueText=Label(dialoguePanel.transform,"",22,0,80,780,100);
        Button(dialoguePanel.transform,"1  Écouter et rassurer",0,5,430,46,()=>FindObjectOfType<MissionManager>().ChooseDialogue(1));
        Button(dialoguePanel.transform,"2  Proposer une sortie sûre",0,-52,430,46,()=>FindObjectOfType<MissionManager>().ChooseDialogue(2));
        Button(dialoguePanel.transform,"3  Faire pression",0,-109,430,46,()=>FindObjectOfType<MissionManager>().ChooseDialogue(3));
        resultPanel=Panel("Bilan mission");resultText=Label(resultPanel.transform,"",24,0,50,800,220);Button(resultPanel.transform,"Recommencer",0,-120,260,50,()=>FindObjectOfType<MissionManager>().Restart());Button(resultPanel.transform,"Menu principal",0,-180,260,46,()=>ShowMenu());
        menuPanel.SetActive(true);optionsPanel.SetActive(false);hudPanel.SetActive(false);dialoguePanel.SetActive(false);resultPanel.SetActive(false);
    }
    GameObject Panel(string name){var g=new GameObject(name,typeof(RectTransform),typeof(Image));g.transform.SetParent(canvas.transform,false);var r=g.GetComponent<RectTransform>();r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=Vector2.zero;r.offsetMax=Vector2.zero;g.GetComponent<Image>().color=new Color(.035f,.055f,.07f,.94f);return g;}
    Text Label(Transform parent,string text,int size,float x,float y,float w,float h,TextAnchor anchor=TextAnchor.MiddleCenter){var g=new GameObject("Texte",typeof(RectTransform),typeof(Text));g.transform.SetParent(parent,false);var t=g.GetComponent<Text>();t.font=font;t.text=text;t.fontSize=size;t.color=Color.white;t.alignment=anchor;t.horizontalOverflow=HorizontalWrapMode.Wrap;t.verticalOverflow=VerticalWrapMode.Overflow;var r=t.rectTransform;r.sizeDelta=new Vector2(w,h);r.anchoredPosition=new Vector2(x,y);return t;}
    void Button(Transform p,string text,float x,float y,float w,float h,UnityEngine.Events.UnityAction action){var g=new GameObject("Bouton - "+text,typeof(RectTransform),typeof(Image),typeof(Button));g.transform.SetParent(p,false);var r=g.GetComponent<RectTransform>();r.sizeDelta=new Vector2(w,h);r.anchoredPosition=new Vector2(x,y);var image=g.GetComponent<Image>();image.color=new Color(.12f,.3f,.38f);var b=g.GetComponent<Button>();b.onClick.AddListener(action);var t=Label(g.transform,text,19,0,0,w-12,h-8);t.raycastTarget=false;}
    Slider Slider(Transform p,float x,float y){var g=new GameObject("Curseur",typeof(RectTransform),typeof(Slider));g.transform.SetParent(p,false);g.GetComponent<RectTransform>().sizeDelta=new Vector2(300,24);g.GetComponent<RectTransform>().anchoredPosition=new Vector2(x,y);var s=g.GetComponent<Slider>();var bg=new GameObject("Fond",typeof(RectTransform),typeof(Image));bg.transform.SetParent(g.transform,false);var br=bg.GetComponent<RectTransform>();br.anchorMin=Vector2.zero;br.anchorMax=Vector2.one;br.offsetMin=Vector2.zero;br.offsetMax=Vector2.zero;bg.GetComponent<Image>().color=Color.gray;var fill=new GameObject("Remplissage",typeof(RectTransform),typeof(Image));fill.transform.SetParent(bg.transform,false);fill.GetComponent<Image>().color=new Color(.15f,.7f,.55f);var fr=fill.GetComponent<RectTransform>();fr.anchorMin=Vector2.zero;fr.anchorMax=Vector2.one;fr.offsetMin=Vector2.zero;fr.offsetMax=Vector2.zero;s.fillRect=fr;var knob=new GameObject("Poignée",typeof(RectTransform),typeof(Image));knob.transform.SetParent(g.transform,false);knob.GetComponent<Image>().color=Color.white;s.targetGraphic=knob.GetComponent<Image>();s.handleRect=knob.GetComponent<RectTransform>();return s;}
    void Stretch(RectTransform r){r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=Vector2.zero;r.offsetMax=Vector2.zero;}
    public void StartGame(){menuPanel.SetActive(false);optionsPanel.SetActive(false);resultPanel.SetActive(false);dialoguePanel.SetActive(false);hudPanel.SetActive(true);InGame=true;Cursor.lockState=CursorLockMode.Locked;}
    public void UpdateHUD(MissionManager m){hudText.text="OBJECTIF : Escortez les civils vers la sortie verte.\nCivils à l'abri : "+m.Evacuated+" / "+m.CivilianCount+"\nWASD Déplacement | Souris Regarder | E Secourir / évacuer | F Négocier | Q Ordre équipe | Échap Curseur";}
    public void ShowMessage(string text){messageText.text=text;CancelInvoke(nameof(ClearMessage));Invoke(nameof(ClearMessage),4f);}void ClearMessage(){if(messageText)messageText.text="";}
    public void ShowDialogue(bool calmed){dialoguePanel.SetActive(true);dialogueText.text=calmed?"La discussion a commencé. Comment poursuivre ?":"Le responsable de crise semble tendu. Choisissez une approche calme.";Cursor.lockState=CursorLockMode.None;}
    public void HideDialogue(){dialoguePanel.SetActive(false);if(InGame)Cursor.lockState=CursorLockMode.Locked;}
    public void ShowResult(bool success,string message,int saved,int total,float seconds,string grade,int choices){InGame=false;hudPanel.SetActive(false);dialoguePanel.SetActive(false);resultPanel.SetActive(true);int mins=(int)(seconds/60),secs=(int)(seconds%60);resultText.text=(success?"MISSION RÉUSSIE":"MISSION TERMINÉE")+"\n\n"+message+"\nCivils à l'abri : "+saved+" / "+total+"\nTemps : "+mins.ToString("00")+":"+secs.ToString("00")+"\nChoix de dialogue : "+choices+"\nÉvaluation : "+grade;Cursor.lockState=CursorLockMode.None;}
}
