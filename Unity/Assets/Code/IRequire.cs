using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRequire {

	bool AllowActivation(Character _character); 
	List<Action> ActivationConsequences (); 
}
