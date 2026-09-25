using UnityEngine;

/// <summary>Déplacement FPS et interaction à courte portée.</summary>
public class PlayerController : MonoBehaviour
{
    public float moveSpeed=4.2f, mouseSensitivity=2.2f, interactDistance=3.2f;
    Camera viewCamera; CharacterController body; MissionManager mission; float pitch;
    public void Setup(Camera cam,MissionManager manager){viewCamera=cam;mission=manager;body=GetComponent<CharacterController>();body.height=1.8f;body.radius=.35f;body.center=new Vector3(0,.9f,0);}
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){Cursor.lockState=Cursor.lockState==CursorLockMode.Locked?CursorLockMode.None:CursorLockMode.Locked;}
        if(mission==null||mission.IsFinished)return;
        if(Cursor.lockState==CursorLockMode.Locked){
            float mx=Input.GetAxis("Mouse X")*mouseSensitivity, my=Input.GetAxis("Mouse Y")*mouseSensitivity;
            transform.Rotate(0,mx,0);pitch=Mathf.Clamp(pitch-my,-80,80);viewCamera.transform.localRotation=Quaternion.Euler(pitch,0,0);
            Vector3 move=(transform.forward*Input.GetAxisRaw("Vertical")+transform.right*Input.GetAxisRaw("Horizontal")).normalized;
            body.SimpleMove(move*moveSpeed);
        }
        if(Input.GetKeyDown(KeyCode.E))Interact();
        if(Input.GetKeyDown(KeyCode.F))mission.TryNegotiate();
        if(Input.GetKeyDown(KeyCode.Q))mission.CycleTeamOrder();
    }
    void Interact(){
        var ray=new Ray(viewCamera.transform.position,viewCamera.transform.forward);
        if(Physics.Raycast(ray,out var hit,interactDistance)){
            var civilian=hit.collider.GetComponentInParent<Civilian>();
            if(civilian!=null){if(civilian.isCrisisLead)mission.TryNegotiate();else mission.Rescue(civilian);return;}
            if(Vector3.Distance(transform.position,mission.ExitPosition)<3f)mission.Evacuate();
        } else if(Vector3.Distance(transform.position,mission.ExitPosition)<3f)mission.Evacuate();
    }
}
