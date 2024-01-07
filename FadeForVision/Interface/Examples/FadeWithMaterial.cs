using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FadeWithMaterial : MonoBehaviour, IFadeObject
{
    [SerializeField] private float _fadeDuration = 0.3f;
    [SerializeField] private float _transitionDuration = 0.3f;
    [SerializeField] private MeshRenderer _material;

    [SerializeField] private float _originalFadeValue = 1f;
    [SerializeField] private float _targetFade = 0.1f;

    private float _fadeLeft = 0;
    private Task _task;

    public void Awake() => _task = Task.FromResult(0);

    public void Fade() {
        _fadeLeft = _fadeDuration;

        if (_task.IsCompleted) {
            _task = FadeMaterial();
        }
    }

    private async Task FadeMaterial() {   
        await TransitionMaterial(_targetFade, _transitionDuration);

        while (_fadeLeft > 0) {
            _fadeLeft -= Time.deltaTime;
            await Task.Yield();
        }

        await TransitionMaterial(_originalFadeValue, _transitionDuration);
    }

    private async Task TransitionMaterial(float target, float time) {
        Color color = _material.material.color;
        float iniValue = color.a;

        for (float t = 0; t < time; t += Time.deltaTime) {
            float nTime = t/time;
            float final = Mathf.Lerp(iniValue, target, nTime);

            color.a = final;
            _material.material.color = color;
            await Task.Yield();
        }
        color.a = target;
        _material.material.color = color;
    }
}
