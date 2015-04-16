using UnityEngine;

public class Barrel : MonoBehaviour {

	bool targeted = false, pickedUp = false;
	public int pointsValue = 1000;

	public bool Targeted {
		get {
			return targeted;
		}
		set {
			targeted = value;
		}
	}

	public bool PickedUp {
		get {
			return pickedUp;
		}
		set {
			pickedUp = value;
		}
	}

	/*public void setTargeted (bool newValue){
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
	}*/
}