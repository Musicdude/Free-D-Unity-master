using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiController : MonoBehaviour {
	[SerializeField] List<Transform> PathPoints;
	[SerializeField] private NotesManager noteman;
	[SerializeField] public int state;
	public Vector3 EvasionDirection;
	private Vector3 TargetPos;
	public float WaitReturnPeriod = 1f;
	private float NextTimeReturn;
	private List<NoteAI> activeAI = new List<NoteAI>();
	private NoteAI ChaseObject;
	public GameObject player;
	//States:
	/*0-DGo Offscreen
	 *1-Go to a random node
	 *2-Grab note 
	 *3-Avoid Planet 
	 *4-Moving to a position 
	 *5-Chaseing Note
	 */ 
	void Start () {
		state =2;
	}

	void Update () {
		if(player==null){
			player = GameObject.Find("Player");
		}
		switch(state){
		case 0:
			TargetPos=PathPoints[0].position;
			MoveToPosition();
			if(Time.time >= NextTimeReturn){
				state=GenerateRandomNumState(0,2);
				Debug.Log("called");
				if(state == 0){
					NextTimeReturn = Time.time+WaitReturnPeriod;
				}
			}
			break;
		case 1:
			int random =GenerateRandomNumState(1,PathPoints.Count);
				TargetPos=PathPoints[random].position;
				state = 4;
			break;
		case 2:
			for(int i =0; i<noteman.mList.Count; i++){
				if(noteman.mList[i].IsEnabled == true){
					activeAI.Add(noteman.mList[i]);
				}
			}
			for(int z =0; z<activeAI.Count; z++){
				float distance = Vector3.Distance(activeAI[z].gameObject.transform.position,player.transform.position);
				Debug.Log(distance);
				if(distance <500 && distance> 200){
					ChaseObject = activeAI[z];			
				}
			
			}
			activeAI.Clear();
			if(ChaseObject!=null){
				state = 5;
			}
			else{
				state = 2;
			}
			break;
		case 4:
			if(this.transform.position != TargetPos){
				MoveToPosition();	
			}
			else{
				state=GenerateRandomNumState(0,3);
				if(state==0){
					NextTimeReturn = Time.time+WaitReturnPeriod;
				}
			}
			break;
		case 5:
			this.transform.position = Vector3.MoveTowards(this.transform.position, ChaseObject.gameObject.transform.position,Time.deltaTime*NotesManager.Instance.NoteSpeed + 1.5f);
			if(this.transform.position == ChaseObject.gameObject.transform.position){
				state = 1;
				ChaseObject.IsEnabled = false;
			}
			break;
		case 3:
			this.transform.position = Vector3.MoveTowards(this.transform.position,this.transform.position+(50*EvasionDirection),Time.deltaTime*100);
			break;
		}
	}

	void MoveToPosition(){
		this.transform.position = Vector3.MoveTowards(this.transform.position, TargetPos,Time.deltaTime*50);
	}

	int GenerateRandomNumState(int min, int max){
		return Random.Range(min,max);
	}
}
