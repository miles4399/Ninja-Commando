using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SetGammaValue : MonoBehaviour
{
    [SerializeField] private VolumeProfile _profile;


    private void OnEnable()
    {
        // attempt to get LiftGammaGain component on volume profile
        if (_profile.TryGet(out LiftGammaGain liftGammaGain))
        {
            // set slider value from current gamma value
            if(TryGetComponent(out Slider slider))
            {
                // without notify prevents the slider from firing value changed events
                slider.SetValueWithoutNotify(liftGammaGain.gamma.value.w);
            }
        }
        else
        {
            Debug.LogWarning("Volume profile is missing LiftGammaGain component", _profile);
        }
    }

    public void SetValue(float value)
    {
        // attempt to get LiftGammaGain component on volume profile
        if(_profile.TryGet(out LiftGammaGain liftGammaGain))
        {
            // set all colors + intensity to the same value to prevent changing scene color
            liftGammaGain.gamma.value = Vector4.one * value;
        }
        else
        {
            Debug.LogWarning("Volume profile is missing LiftGammaGain component", _profile);
        }
    }
}
