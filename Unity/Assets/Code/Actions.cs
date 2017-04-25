using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic; 

public abstract class Action
{
    protected ActionType _action;
    protected float _time;
    protected bool _isExternal; 
    public ActionType Type { get { return _action; } }
    public float Time { get { return _time; } }
    public bool IsExternal { get { return _isExternal; } }
    public virtual float Value { get { return float.NaN; } }
    public virtual Vector3 Vector { get { return Vector3.zero; } }
    public virtual HistoryNode PossibleFuture { get { return null; } }
    public virtual ITargetable Target { get { return null; } }
    public virtual Task Task { get { return null; } }
    public virtual Weapon Weapon { get { return null; } }
    public virtual CombatState Combat { get { return null; } }
    public virtual IBehavior Behavior { get { return null; } }
    public virtual InvenItem Item { get { return null; } }
    public virtual int IValue { get { return int.MinValue; } }
    public virtual Vector3 SecondVec { get { return Vector3.zero; } }
	public abstract int SerialIndex{ get ; }
	public virtual Action Parse (List<string> _tokens){
		return null;
		} 
}

public class BasicAction : Action{

    public BasicAction(ActionType _actionType)
    {
        _action = _actionType;
        _time = GameManager.FixedGameTime;
    }
    public BasicAction(ActionType _actionType, bool isExternal)
    {
        _action = _actionType;
        _time = GameManager.FixedGameTime;
        _isExternal = isExternal; 
    }
    public BasicAction(ActionType _actionType, float time, bool isExternal)
    {
        _action = _actionType;
        _time = time;
        _isExternal = isExternal;
    }
    public BasicAction(ActionType _actionType, float time)
    {
        _action = _actionType;
        _time = time;
    }
	public override int SerialIndex { get { return 1; } }
	/*public override  Action Parse(List<string> _tokens){
		return new BasicAction (ActionType (int.TryParse (_tokens [1])),
			float.TryParse (_tokens [2]), Util.UnserializeBool (_tokens [2]));  
	}*/
}

public class ValueAction : Action
{
    float _value;
    public ValueAction(ActionType _actionType, float value)
    {
        _action = _actionType;
        _time = GameManager.FixedGameTime;
        _value = value; 
    }
    public ValueAction(ActionType _actionType, float value, float time)
    {
        _action = _actionType;
        _time = time;
        _value = value;
    }
    public ValueAction(ActionType _actionType, float value, bool isExternal)
    {
        _action = _actionType;
        _value = value;
        _isExternal = isExternal;
        _time = GameManager.FixedGameTime;
    }
    public ValueAction(ActionType _actionType, float value, float time, bool isExternal)
    {
        _action = _actionType;
        _value = value;
        _time = time;
        _isExternal = isExternal; 
    }
    public override float Value { get { return _value; } }
	public override int SerialIndex { get { return 2; } }/*
	public static Action Parse(List<string> _tokens){
		return new ValueAction (ActionType (int.TryParse (_tokens [1])), 
			float.TryParse(_tokens[2]),
			float.TryParse (_tokens [3]), Util.UnserializeBool (_tokens [4]));  
	*/
}
public class VectorAction : Action
{
    Vector3 _vector;

    public VectorAction(ActionType _actionType, Vector3 vector)
    {
        _action = _actionType;
        _time = GameManager.FixedGameTime;
        _vector = vector;
    }
    public VectorAction(ActionType _actionType, Vector3 vector, float time)
    {
        _action = _actionType;
        _time = time;
        _vector = vector;
    }
    public VectorAction(ActionType _actionType, Vector3 vector, bool isExternal)
    {
        _action = _actionType;
        _vector = vector;
        _isExternal = isExternal;
        _time = GameManager.FixedGameTime; 
    }
    public VectorAction(ActionType _actionType, Vector3 vector, float time, bool isExternal)
    {
        _action = _actionType;
        _vector = vector;
        _isExternal = isExternal;
        time = _time; 
    }
    public override Vector3 Vector { get { return _vector; } }
	public override int SerialIndex { get { return 3; } }/*
	public static Action Parse(List<string> _tokens){
		return new VectorAction (ActionType (int.TryParse (_tokens [1])), 
			float.TryParse(_tokens[2]),
			float.TryParse (_tokens [3]), Util.UnserializeBool (_tokens [4]));  
	}*/
}
public class FutureActions : Action
{
    HistoryNode _future;

    public FutureActions(ActionType type, HistoryNode future)
    {
        _action = type;
        _future = future;
        _time = GameManager.FixedGameTime;
    }
    public FutureActions(ActionType type, HistoryNode future, float time)
    {
        _action = type; 
        _future = future;
        _time = time; 
    }

    public FutureActions(ActionType type, HistoryNode future, bool isExternal)
    {
        _isExternal = isExternal; 
        _action = type;
        _future = future;
        _time = GameManager.FixedGameTime;
    }
    public FutureActions(ActionType type, HistoryNode future, float time, bool isExternal)
    {
        _isExternal = isExternal; 
        _action = type;
        _future = future;
        _time = time;
    }
	public override HistoryNode PossibleFuture { get { return _future; } }
	public override int SerialIndex { get { return 4; } }
}

public class TargetedAction : Action
{
    ITargetable _target;
	Vector3 _vec;

	public TargetedAction(ActionType type, ITargetable target, Vector3 vec)
    {
        _action = type;
        _target = target;
        _time = GameManager.FixedGameTime; 
		_vec = vec; 
    }
	public TargetedAction(ActionType type, ITargetable target, Vector3 vec, bool external)
    {
		_vec = vec; 
        _action = type;
        _target = target;
        _isExternal = external;
        _time = GameManager.FixedGameTime; 
    }

    public override ITargetable Target { get { return _target; } }
	public override Vector3 Vector { get { return _vec; } }
	public override int SerialIndex { get { return 5; } }
}

public class TaskAction : Action
{
    Task _task;

    public TaskAction(ActionType type, Task task)
    {
        _action = type;
        _task = task;
        _time = GameManager.FixedGameTime;
    }
    public TaskAction(ActionType type, Task task, float time)
    {
        _action = type;
        _task = task;
        _time = time;
    }
    public TaskAction(ActionType type, Task task, bool external)
    {
        _action = type;
        _task = task;
        _isExternal = external;
        _time = GameManager.FixedGameTime;
    }
	public override Task Task { get { return _task; } }
	public override int SerialIndex { get { return 6; } }
}

public class WeaponAction : Action
{
    Weapon _weapon; 

    public WeaponAction(ActionType type, Weapon weapon)
    {
        _action = type;
        _weapon = weapon; 
        _time = GameManager.FixedGameTime;
    }
    public WeaponAction(ActionType type, Weapon weapon, bool external)
    {
        _action = type;
        _weapon = weapon;
        _isExternal = external;
        _time = GameManager.FixedGameTime;
    }

	public override Weapon Weapon { get { return _weapon; } }
	public override int SerialIndex { get { return 7; } }
}


public class CombatAction : Action
{
    CombatState _combat;

    public CombatAction(ActionType type, CombatState combat)
    {
        _action= type;
        _combat = combat;
        _time = GameManager.FixedGameTime;
    }
    public CombatAction(ActionType type, CombatState combat, bool external)
    {
        _action = type;
        _combat = combat; 
        _isExternal = external;
        _time = GameManager.FixedGameTime;
    }

	public override CombatState Combat { get { return _combat; } }
	public override int SerialIndex { get { return 8; } }
}

public class BehaviorAction : Action
{

    IBehavior _behavior;

    public BehaviorAction(ActionType type, IBehavior behavior)
    {
        _action = type;
        _behavior = behavior; 
        _time = GameManager.FixedGameTime;
    }
    public BehaviorAction(ActionType type, IBehavior behavior, bool external)
    {
        _action = type;
        _behavior = behavior; 
        _isExternal = external;
        _time = GameManager.FixedGameTime;
    }

	public override IBehavior Behavior { get { return _behavior; } }

	public override int SerialIndex { get { return 9; } }
}

public class ItemAction : Action
{
    InvenItem _item;

    public ItemAction(ActionType type, InvenItem item)
    {
        _action = type;
        _item = item; 
        _time = GameManager.FixedGameTime;
    }
    public ItemAction(ActionType type, InvenItem item, bool external)
    {
        _action = type;
        _item = item;
        _isExternal = external;
        _time = GameManager.FixedGameTime;
    }

	public override InvenItem Item { get { return _item; } }
	public override int SerialIndex { get { return 10; } }
}

public class AnimAction : Action
{
    int _int;
    float _value; 

    public AnimAction(ActionType type, int integer, float value)
    {
        _action = type;
        _int = integer;
        _value = value; 
        _time = GameManager.FixedGameTime;
    }
    public AnimAction(ActionType type, int integer, float value, bool external)
    {
        _action = type;
        _int = integer;
        _value = value; 
        _isExternal = external;
        _time = GameManager.FixedGameTime;
    }
    public override int IValue { get { return _int; } }
	public override float Value { get { return _value; } }
	public override int SerialIndex { get { return 11; } }
}


public class DirTargetAction : Action
{
    Vector3 _dir;
    Vector3 _target; 
    public DirTargetAction(ActionType type, Vector3 direction, Vector3 target)
    {
        _action = type;
        _dir = direction;
        _target = target; 
        _time = GameManager.FixedGameTime;
    }
    public DirTargetAction(ActionType type, Vector3 direction, Vector3 target, float time)
    {
        _action = type;
        _dir = direction;
        _target = target;
        _time = time;
    }
    public DirTargetAction(ActionType type, Vector3 direction, Vector3 target, bool external)
    {
        _action = type;
        _dir = direction;
        _target = target; 
        _isExternal = external;
        _time = GameManager.FixedGameTime;
    }
    public DirTargetAction(ActionType type, Vector3 direction, Vector3 target, float time, bool external)
    {
        _action = type;
        _dir = direction;
        _target = target;
        _isExternal = external;
        _time = time;
    }
    public override Vector3 Vector { get { return _dir; } }
	public override Vector3 SecondVec { get { return _target; } }
	public override int SerialIndex { get { return 12; } }
}

public class ValueTargetAction : Action
{
    float _value;
    Vector3 _target;
    public ValueTargetAction(ActionType type, float value, Vector3 target)
    {
        _action = type;
        _value = value;
        _target = target;
        _time = GameManager.FixedGameTime;
    }
    public ValueTargetAction(ActionType type, float value, Vector3 target, float time)
    {
        _action = type;
        _value = value;
        _target = target;
        _time = time;
    }
    public ValueTargetAction(ActionType type, float value, Vector3 target, bool external)
    {
        _action = type;
        _value = value; 
        _target = target;
        _isExternal = external;
        _time = GameManager.FixedGameTime;
    }
    public ValueTargetAction(ActionType type, float value, Vector3 target, float time, bool external)
    {
        _action = type;
        _value = value; 
        _target = target;
        _isExternal = external;
        _time = time;
    }
    public override float Value { get { return _value; } }
	public override Vector3 SecondVec { get { return _target; } }
	public override int SerialIndex { get { return 13; } }
}

public class ValueIntAction : Action
{
	float _value;
	int _index;
	public ValueIntAction(ActionType type, float value, int index)
	{
		_action = type;
		_value = value;
		_index = index; 
		_time = GameManager.FixedGameTime;
	}
	public ValueIntAction(ActionType type, float value, int index, float time)
	{
		_action = type;
		_value = value;
		_index = index; 
		_time = time;
	}
	public ValueIntAction(ActionType type, float value, int index, bool external)
	{
		_action = type;
		_value = value; 
		_index = index; 
		_isExternal = external;
		_time = GameManager.FixedGameTime;
	}
	public ValueIntAction(ActionType type, float value, int index, float time, bool external)
	{
		_action = type;
		_value = value; 
		_index = index; 
		_isExternal = external;
		_time = time;
	}
	public override float Value { get { return _value; } }
	public override int IValue { get { return _index; } }
	public override int SerialIndex { get { return 14; } }
}

public class VectorIntAction : Action
{
	Vector3 _vector;
	int _index;
	public VectorIntAction(ActionType type, Vector3 vector, int index)
	{
		_action = type;
		_vector = vector; 
		_index = index; 
		_time = GameManager.FixedGameTime;
	}
	public VectorIntAction(ActionType type, Vector3 vector, int index, float time)
	{
		_action = type;
		_vector = vector; 
		_index = index; 
		_time = time;
	}
	public VectorIntAction(ActionType type, Vector3 vector, int index, bool external)
	{
		_action = type;
		_vector = vector; 
		_index = index; 
		_isExternal = external;
		_time = GameManager.FixedGameTime;
	}
	public VectorIntAction(ActionType type, Vector3 vector, int index, float time, bool external)
	{
		_action = type;
		_vector = vector; 
		_index = index; 
		_isExternal = external;
		_time = time;
	}
	public override Vector3 Vector { get { return _vector; } }
	public override int IValue { get { return _index; } }
	public override int SerialIndex { get { return 15; } }
}

public class TaskIntAction : Action
{
	Task _task; 
	int _index;
	public TaskIntAction(ActionType type, Task task, int index)
	{
		_action = type;
		_task = task;
		_index = index; 
		_time = GameManager.FixedGameTime;
	}
	public TaskIntAction(ActionType type, Task task, int index, float time)
	{
		_action = type;
		_task = task; 
		_index = index; 
		_time = time;
	}
	public TaskIntAction(ActionType type, Task task, int index, bool external)
	{
		_action = type;
		_task = task; 
		_index = index; 
		_isExternal = external;
		_time = GameManager.FixedGameTime;
	}
	public TaskIntAction(ActionType type, Task task, int index, float time, bool external)
	{
		_action = type;
		_task = task; 
		_index = index; 
		_isExternal = external;
		_time = time;
	}
	public override Task Task { get { return _task; } }
	public override int IValue { get { return _index; } }
	public override int SerialIndex { get { return 16; } }
}

public enum ActionType
{
    PressForward,
    ReleaseForward,
    PressRight,
    ReleaseRight,
    PressLeft,
    ReleaseLeft,
    PressBack,
    ReleaseBack,
    Rotation,
	Instantiated,
    ForwardBlocked,
    ForwardUnblocked,
    BackwardBlocked,
    BackwardUnblocked,
    LeftBlocked,
    LeftUnblocked,
    RightBlocked,
    RightUnblocked,
    IsGrounded,
    IsFalling,
    StandingOnSurface,
    LeavingSurface,
	LockTrans,
    PickUp,
    PutDown,
    PullTrigger,
    ReleaseTrigger,
    Clear,
    Activate,
    Deactivate,
	ThresholdReached,
    SetInterruption,
    UnsetInterruption,
    UnsetTaskInterruption,
    ReloadInterruptedTask,
    Interrupt,
    Resume,
    ResumeBranch,
    UnResumeTask,
    Fire, 
    TakeDamage,
    ShotBy,
    Die,
    AnimEnded, 
    Extract,
    SetTask,
    UnsetTask,
    BSeekAndShoot,
    BSeekAndShootUnset,
	AnimRunning,
	AnimStopRunning,
    AIWait,
    AIWaitUnset,
    AIMoveForward,
    AIMoveForwardUnset,
    AIRotate,
    AIRotateUnset,
    AIRotateDir,
    AIRotateDirUnset,
    AIMoveTo, 
    AIMoveToUnset,
    SawTarget,
    DidntSeeTarget,
	Untarget,
    SpliceFuture,
    ClearFuture, 
    Null, 
    SawPlayer,
    Alert,
    UnAlert
}

