using UnityEngine;
using System.Collections;
using System;

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
    public override HistoryNode PossibleFuture { get { return _future; } }
}

public class TargetedAction : Action
{
    ITargetable _target;

    public TargetedAction(ActionType type, ITargetable target)
    {
        _action = type;
        _target = target;
        _time = GameManager.FixedGameTime; 
    }
    public TargetedAction(ActionType type, ITargetable target, bool external)
    {
        _action = type;
        _target = target;
        _isExternal = external;
        _time = GameManager.FixedGameTime; 
    }

    public override ITargetable Target { get { return _target; } }
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

    public IBehavior Behavior { get { return _behavior; } }
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
    PickUp,
    PutDown,
    PullTrigger,
    ReleaseTrigger,
    Clear,
    Activate,
    Deactivate,
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
    SpliceFuture,
    ClearFuture, 
    Null, 
    SawPlayer,
    Alert,
    UnAlert
}

