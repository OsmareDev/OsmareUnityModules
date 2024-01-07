using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FadeWithShader : MonoBehaviour, IFadeObject
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
            _task = FadeShader();
        }
    }

    private async Task FadeShader() {
        await TransitionShader(_targetFade, _transitionDuration);

        while (_fadeLeft > 0) {
            _fadeLeft -= Time.deltaTime;
            await Task.Yield();
        }

        await TransitionShader(_originalFadeValue, _transitionDuration);
    }

    private async Task TransitionShader(float target, float time) {
        float iniValue = _material.material.GetFloat("_Transparency");

        for (float t = 0; t < time; t += Time.deltaTime) {
            float nTime = t/time;
            float final = Mathf.Lerp(iniValue, target, nTime);

            _material.material.SetFloat("_Transparency", final);
            await Task.Yield();
        }
        _material.material.SetFloat("_Transparency", target);
    }
}
