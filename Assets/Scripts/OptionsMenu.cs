using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider backwardSlider;
    public Slider forwardSlider;

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
