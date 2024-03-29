using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour {
	int i;
	float time = 0.0f;
	Car car;
	public float startingVelocity;
    tk2dSprite sprite;
	
	void Start() {
        sprite = GetComponent<tk2dSprite>();
		i = (int)UnityEngine.Random.Range(0, 4);
        car = transform.parent.GetComponent<Car>();
	}
	
	void LateUpdate () {
		time += Time.deltaTime;
		if (time < startingVelocity / (4*car.velocity)) return;
		time = 0.0f;
		
        sprite.spriteId = i;
		i = (i + 1) % 4;
	}
}
