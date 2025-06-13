using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Wood : MonoBehaviour
{
    private float _baseSpeed;
    private RotationType _rotationType;
    private float _time;
    private float _direction = 1f;
    private float _directionTimer;
    private float _directionInterval;
    private AnimationCurve _curve;

    public void InitFromLevel(LevelData level)
    {
        _baseSpeed = level.targetRotationSpeed;
        _rotationType = level.rotationType;
        _directionInterval = level.directionChangeInterval;
        _curve = level.rotationCurve;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        float currentSpeed = _baseSpeed;

        switch (_rotationType)
        {
            case RotationType.Constant:
                break;

            case RotationType.PingPong:
                currentSpeed = _baseSpeed * Mathf.Sin(_time);
                break;

            case RotationType.Random:
                currentSpeed = _baseSpeed * Mathf.PerlinNoise(_time, 0f);
                break;
            
            case RotationType.DirectionChange:
                _directionTimer += Time.deltaTime;
                if (_directionTimer >= _directionInterval)
                {
                    _direction *= -1f;
                    _directionTimer = 0f;
                }
                currentSpeed *= _direction;
                break;
            
            case RotationType.EaseInOut:
                float curveValue = Mathf.Sin(_time);
                currentSpeed *= curveValue;
                break;
            
            case RotationType.CurveDriven:
                float normalizedTime = _time % 1f; // repeat every 1s
                currentSpeed *= _curve.Evaluate(normalizedTime);
                break;
            
        }

        var shouldRotate = GameManager.Instance.IsGameOver(); //stop rotating once won
        if(!shouldRotate) 
            transform.Rotate(Vector3.forward * (currentSpeed * Time.deltaTime));
    }
}
