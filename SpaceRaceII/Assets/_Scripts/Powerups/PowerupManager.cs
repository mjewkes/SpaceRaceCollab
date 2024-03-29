using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {
	public GameObject[] powerupPrefabs;
	public List<Powerup> powerups;
	[SerializeField] float minSpawnTime = 3.0f;
	[SerializeField] float maxSpawnTime = 5.0f;
	[SerializeField] bool generating;
	float curSpawnTime;
	float time;
	
	void Start() {
		Static.Events.PowerupDestroyed += RemovePowerup;
		Static.Events.RoundStarted += RoundStarted;
		Static.Events.RoundEnded += RoundEnded;
		powerups = new List<Powerup>();
		SetNewSpawnTime();
	}
	
	void SetNewSpawnTime() {
		curSpawnTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
		time = 0.0f;
	}
	
	public void GeneratePowerup() {
		int laneIndex = (int)UnityEngine.Random.Range(0, Static.LevelData.LaneManager.numberOfLanes);
		int powerupIndex = (int)UnityEngine.Random.Range(0, powerupPrefabs.Length);
		Lane lane = Static.LevelData.LaneManager.GetLane(laneIndex);
		Vector3 placement = new Vector3(lane.predictorPlacement.position.x, lane.predictorPlacement.position.y, lane.predictorPlacement.position.z - 2f);
		//GameObject predictor = Instantiate(predictorPrefab, placement, Quaternion.identity) as GameObject;
		GameObject powerup = Instantiate(powerupPrefabs[powerupIndex], placement, Quaternion.identity) as GameObject;
        
        // hax because this is getting messed up on instantiate somehow
        //powerup.GetComponent<BoxCollider>().center = Vector3.zero;
        //powerup.GetComponent<BoxCollider>().size = new Vector3(6, 6, 2);
		
        Powerup p = powerup.GetComponent<Powerup>();
		p.Deploy(Static.LevelData.LaneManager.GetLane(laneIndex));
		powerups.Add(p);
		//predictor.GetComponent<Predictor>().SetPowerup(powerup);
		
	}
	
	void RoundStarted(Round r) {
		RemoveAllPowerups();
        generating = true;
	}
	
	void RoundEnded(Round r) {
		generating = false;
	}
	
	void RemoveAllPowerups() {
		for (int i = 0; i < powerups.Count; i++) {
            powerups[i].Destroy();
		}
		powerups.Clear();
	}
	
	void RemovePowerup(Powerup p) {
		powerups.Remove(p);
		// my power pool is not yet a stack 
        p.Destroy();
	}
	
	void Update() {
		if (generating) {
			time += Time.deltaTime;
			if (time >= curSpawnTime) {
				GeneratePowerup();
				SetNewSpawnTime();
			}
		}
	}
}

public partial class LevelData {
	public PowerupManager powerupManager;
	public PowerupManager PowerupManager {
		get { return powerupManager; }
	}
}
