using UnityEngine;
using System.Collections;

public class Predictor : MonoBehaviour {
	Transform target;
	float targetStart;
	float start;
	float width=11f;
	int Severity;
	Renderer r;
	void LateUpdate(){
		 //Start of predictor
		var distance = target.position.x-start;
		var ratio = distance/(targetStart-start);
		var xPos = MathfEx.PInterp(start+width,start,1-distance/targetStart,5);
		transform.position = new Vector3(xPos,transform.position.y,transform.position.z);
		if (distance<0){
			r.active=true;
			Destroy(gameObject);
		}
	}
	
	public void SetPowerup(GameObject g){
		target=g.transform;
		r=target.GetComponentInChildren<Renderer>();
		r.active=false;
		targetStart=target.position.x;
		start=transform.position.x;
		LateUpdate();
	}
}

static class MathfEx{
	public static float PInterp (float start, float finish, float t, int n){
		float a = Mathf.Pow(t,n);
		return start + (a*(finish-start));
	}
}