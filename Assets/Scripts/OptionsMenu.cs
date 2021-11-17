using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider backwardSlider = null;
    [SerializeField] private Slider forwardSlider = null;

    private void Start()
    {
        backwardSlider.value = MovementOptions.minimalBackwardDistance;
        forwardSlider.value = MovementOptions.minimalForwardDistance;
    }

    public void updateDistances()
    {
        MovementOptions.minimalBackwardDistance = backwardSlider.value;
        MovementOptions.minimalForwardDistance = forwardSlider.value;
    }
}
