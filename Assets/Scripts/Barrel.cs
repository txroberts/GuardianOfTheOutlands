using UnityEngine;

public class Barrel : MonoBehaviour {

	public bool targeted = false, pickedUp = false;

	public void setTargeted (bool newValue){
		targeted = newValue;
	}

	public bool getTargeted (){
		return targeted;
	}

	public void setPickedUp (bool newValue){
		pickedUp = newValue;
	}

	public bool getPickedUp(){
		return pickedUp;
	}
}