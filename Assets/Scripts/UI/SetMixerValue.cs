using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetMixerValue : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private string _parameterName = "Mixer Paramater Name";

    private void OnEnable()
    {
        // get mixer value
        float value = 0f;
        _mixer.GetFloat(_parameterName, out value);

        // set slider value from mixer
        if (TryGetComponent(out Slider slider))
        {
            slider.SetValueWithoutNotify(value);
        }
    }

    public void SetValue(float value)
    {
        _mixer.SetFloat(_parameterName, value);
    }
}
