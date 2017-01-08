using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Effect {
    string _name; 
    GameObject _go;
    float _createdAt;
    uint _seed;
    List<ParticleSystem> _particles = new List<ParticleSystem>();
    bool _hasParent = false;
    Transform _parent;
    bool _hasLocation = false;
    Vector3 _loc;
    Quaternion _rot;
    float _endedAt; 
    public float EndedAt { get { return _endedAt; } }

    public Effect(string name)
    {
        _name = name;
        _createdAt = GameManager.FixedGameTime;
        _seed = EffectsManager.GetSeed();
        CreateEffect(); 
    }
    public Effect(string name, Transform parent)
    {
        _name = name;
        _parent = parent;
        _hasParent = true;
        _createdAt = GameManager.FixedGameTime;
        _seed = EffectsManager.GetSeed();
        CreateEffect();
    }
    public Effect(string name, Vector3 loc, Quaternion rot)
    {
        _createdAt = GameManager.FixedGameTime;
        _seed = EffectsManager.GetSeed();
        _name = name;
        _loc = loc;
        _rot = rot;
        _hasLocation = true;
        CreateEffect();
    }
    public Effect(string name, Transform parent, Vector3 loc, Quaternion rot)
    {
        _name = name; 
        _loc = loc;
        _rot = rot;
        _parent = parent;
        _hasLocation = true;
        _hasParent = true;
        _createdAt = GameManager.FixedGameTime;
        _seed = EffectsManager.GetSeed();
        CreateEffect(); 
    }

    void CreateEffect()
    {
        if(_hasLocation)
        {
            _go = EffectsManager.CreateEffect(_name, _loc, _rot);
        }else
        {
            _go = EffectsManager.CreateEffect(_name); 
        }
        if (_hasParent)
        {
            _go.transform.parent = _parent; 
        }
        ParticleSystem _goPs = _go.GetComponent<ParticleSystem>(); 
        if(_goPs != null)
        {
            _particles.Add(_goPs); 
        }
        _particles.AddRange(_go.GetComponentsInChildren<ParticleSystem>());
        foreach (ParticleSystem _ps in _particles)
        {
            _ps.randomSeed = _seed;
            _ps.playbackSpeed = EffectsManager.PlaySpeed; 
        }
        EffectsManager.RegisterEffect(this); 
    }

    public void Play(float _gametime)
    {
        if(_gametime < _createdAt)
        {
            DeleteSelf(); 
        }
        if(!_particles.Any((_ps) => _ps.IsAlive()))
        {
            EndEffect(); 
        }
    }
    public void ChangeSpeed(float _speed)
    {
        foreach(ParticleSystem _ps in _particles)
        {
            _ps.playbackSpeed = _speed; 
        }
    }
    public void Pause()
    {
        foreach(ParticleSystem _ps in _particles)
        {
            _ps.Pause();
        }
    }
    public void Resume()
    {
        foreach(ParticleSystem _ps in _particles)
        {
            _ps.Play(); 
        }
    }
    public void ReloadEffect(float _time)
    {
        CreateEffect(); 
        foreach(ParticleSystem _ps in _particles)
        {
            _ps.time = _time - _createdAt; 
        }
    }
    void EndEffect()
    {
        if(_go != null)
        {
            GameObject.Destroy(_go);
            _go = null; 
        }
        _particles.Clear();
        EffectsManager.UnregisterEffect(this);
        EffectsManager.EffectEnded(this); 
    }
    void DeleteSelf()
    {
        EffectsManager.UnregisterEffect(this);
        _particles.Clear(); 
        if(_go != null)
        {
            GameObject.Destroy(_go); 
        }
        EffectsManager.RewindGetSeed(); 
    }
}
