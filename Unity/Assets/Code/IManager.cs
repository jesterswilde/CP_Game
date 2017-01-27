using System.Collections;
using System.Collections.Generic;

public interface IManager {
    void UpdateManager(float _time, float _deltaTime);
    void FixedUpdateManager(float _time, float _deltaTime);
    void StartManager(); 
}
