using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>Construit une scène complète à partir de primitives Unity.</summary>
public class GameBootstrap : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
        BuildWorld();
        BuildUI();
        var mission = new GameObject("GestionnaireMission").AddComponent<MissionManager>();
        var player = new GameObject("Joueur");
        player.transform.position = new Vector3(0, 1.1f, -8);
        var cameraObject = new GameObject("CameraJoueur");
        cameraObject.transform.SetParent(player.transform);
        cameraObject.transform.localPosition = new Vector3(0, .55f, 0);
        var camera = cameraObject.AddComponent<Camera>();
        cameraObject.AddComponent<AudioListener>();
        player.AddComponent<CharacterController>();
        var controller = player.AddComponent<PlayerController>();
        controller.Setup(camera, mission);
        mission.Setup(controller, FindObjectOfType<GameUI>());
        CreateTeam();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void BuildWorld()
    {
        RenderSettings.ambientLight = new Color(.58f, .58f, .62f);
        RenderSettings.fog = false;
        var light = new GameObject("Eclairage").AddComponent<Light>();
        light.type = LightType.Directional; light.intensity = 1.15f;
        light.transform.rotation = Quaternion.Euler(48, -30, 0);
        CreateBox("Sol", new Vector3(0,-.15f,0), new Vector3(18,.3f,22), new Color(.34f,.37f,.39f));
        // Enceinte avec entrée large au sud et passages lisibles.
        CreateBox("Mur nord", new Vector3(0,1.5f,10), new Vector3(18,3, .35f), new Color(.73f,.72f,.66f));
        CreateBox("Mur ouest", new Vector3(-9,1.5f,0), new Vector3(.35f,3,20), new Color(.73f,.72f,.66f));
        CreateBox("Mur est", new Vector3(9,1.5f,0), new Vector3(.35f,3,20), new Color(.73f,.72f,.66f));
        CreateBox("Mur sud gauche", new Vector3(-6,1.5f,-10), new Vector3(6,3,.35f), new Color(.73f,.72f,.66f));
        CreateBox("Mur sud droite", new Vector3(6,1.5f,-10), new Vector3(6,3,.35f), new Color(.73f,.72f,.66f));
        // Comptoirs et cloisons basses : navigation sans blocage complet.
        CreateBox("Comptoir accueil", new Vector3(-4, .65f, 1), new Vector3(4,.9f,.65f), new Color(.38f,.23f,.13f));
        CreateBox("Comptoir caisse", new Vector3(4,.65f,2), new Vector3(4,.9f,.65f), new Color(.38f,.23f,.13f));
        CreateBox("Cloison", new Vector3(0,1.25f,5), new Vector3(.2f,2.5f,5), new Color(.77f,.77f,.72f));
        CreateBox("Bureau", new Vector3(-5,.55f,6), new Vector3(3,1,.8f), new Color(.42f,.28f,.18f));
        CreateBox("Zone de sortie", new Vector3(0,.03f,-8.8f), new Vector3(3,.08f,1.3f), new Color(.12f,.8f,.33f));
        CreateText3D("SORTIE SECURISEE", new Vector3(0,.25f,-8.5f), .28f, Color.white);
        CreateText3D("BANQUE", new Vector3(0,2.6f,9.7f), .7f, new Color(.15f,.18f,.22f));
        // Repères visuels lumineux.
        var lamp = new GameObject("Lumiere hall").AddComponent<Light>();
        lamp.type = LightType.Point; lamp.range=16; lamp.intensity=1.2f; lamp.color=new Color(1f,.86f,.68f); lamp.transform.position=new Vector3(0,2.7f,0);
        var exitLight = new GameObject("Lumiere sortie").AddComponent<Light>();
        exitLight.type=LightType.Point; exitLight.range=6; exitLight.intensity=2; exitLight.color=Color.green; exitLight.transform.position=new Vector3(0,2,-8);
        MakeCivilian("Civil - Lina", new Vector3(-5,0,3), "Lina");
        MakeCivilian("Civil - Marc", new Vector3(5,0,5), "Marc");
        MakeCivilian("Responsable de crise", new Vector3(2,0,7), "Responsable de crise").GetComponent<Civilian>().isCrisisLead=true;
        MakeCivilian("Civil - Sam", new Vector3(-2,0,-2), "Sam");
    }

    Civilian MakeCivilian(string objectName, Vector3 position, string display)
    {
        var root = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        root.name=objectName; root.transform.position=position; root.transform.localScale=new Vector3(.65f, .9f, .65f);
        var c=root.AddComponent<Civilian>(); c.displayName=display;
        var renderer=root.GetComponent<Renderer>();
        renderer.material=NewMaterial(objectName.Contains("Responsable")?new Color(.65f,.27f,.2f):new Color(.2f,.48f,.78f));
        CreateText3D(display,position+Vector3.up*2.1f,.22f,Color.white).transform.SetParent(root.transform);
        return c;
    }

    void CreateTeam()
    {
        for(int i=0;i<2;i++){
            var ally=GameObject.CreatePrimitive(PrimitiveType.Capsule); ally.name="Equipier "+(i+1);
            ally.transform.position=new Vector3(i==0?-2:2,0,-6); ally.transform.localScale=new Vector3(.65f,.9f,.65f);
            ally.GetComponent<Renderer>().material=NewMaterial(new Color(.16f,.35f,.27f));
            var ai=ally.AddComponent<TeamMate>(); ai.memberIndex=i;
            CreateText3D("EQUIPE",ally.transform.position+Vector3.up*2,.18f,new Color(.75f,1,.8f)).transform.SetParent(ally.transform);
        }
    }

    void BuildUI()
    {
        var canvasObject=new GameObject("Interface",typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
        var canvas=canvasObject.GetComponent<Canvas>(); canvas.renderMode=RenderMode.ScreenSpaceOverlay;
        canvasObject.GetComponent<CanvasScaler>().uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObject.GetComponent<CanvasScaler>().referenceResolution=new Vector2(1280,720);
        if(FindObjectOfType<EventSystem>()==null)new GameObject("EventSystem",typeof(EventSystem),typeof(StandaloneInputModule));
        canvasObject.AddComponent<GameUI>();
    }

    GameObject CreateBox(string n,Vector3 p,Vector3 s,Color c){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.position=p;g.transform.localScale=s;g.GetComponent<Renderer>().material=NewMaterial(c);return g;}
    Material NewMaterial(Color c){var shader=Shader.Find("Universal Render Pipeline/Lit")??Shader.Find("Standard");var m=new Material(shader);m.color=c;return m;}
    GameObject CreateText3D(string text,Vector3 pos,float size,Color color){var g=new GameObject("Panneau - "+text);g.transform.position=pos;var t=g.AddComponent<TextMesh>();t.text=text;t.fontSize=48;t.characterSize=size;t.anchor=TextAnchor.MiddleCenter;t.alignment=TextAlignment.Center;t.color=color;return g;}
}
