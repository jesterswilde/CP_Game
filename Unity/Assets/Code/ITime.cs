using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITime {

	List<Action> UseAction(Action _action, float _time); 
	void ReverseAction(Action _action, float _time); 
	List<Action> Act(float _deltaTime); 
	void RewindAct(float _deltaTime); 
	List<Action> PlayVisuals(float _time); 
	void RewindVisuals(float _time); 
}
