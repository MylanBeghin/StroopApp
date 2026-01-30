# Technical Validation and Timing Accuracy

## Objective

This document reports a hardware-based validation of **StroopApp** aimed at assessing the **stability and reliability** of its reaction time measurements.

The purpose of this study is to document timing behaviour using established validation methods. It is not intended to assess absolute response speed or to rank software tools.

For contextual comparison, StroopApp was evaluated alongside:
- PsychoPy (PP)
- SuperLab (SL)

---

## Validation approach

Timing accuracy was assessed using a hardware-based setup independent of software clocks.

Each stimulus was presented together with a **small white visual marker** displayed in the bottom-right corner of the screen. This marker produced a clear luminance transition, allowing unambiguous detection by a photodiode and avoiding ambiguities related to text rendering.

Presenting an additional visual marker is at least as demanding as presenting the stimulus alone. Measured precision therefore represents a **conservative estimate** of stimulus timing accuracy.

---

## Hardware setup

Stimulus onset and response registration were measured using:

- a **photodiode** detecting luminance changes,
- a programmable **Arduino** microcontroller.

Upon detecting a luminance change above a fixed threshold, the Arduino waited **500 ms** before emitting a simulated keyboard response.

The photodiode was positioned directly over the visual marker to ensure identical detection conditions across trials and software tools.

![Hardware-based validation setup](images/hardware_validation_setup.jpg)

*Hardware-based timing validation setup (photodiode and Arduino).*

---

## Display and systems

All experiments were conducted on standard LCD displays operating at a **60 Hz refresh rate**, with no adaptive or variable refresh technologies enabled.

Measurements were repeated across multiple computers and two operating systems:
- Windows 7
- Windows 11

Results were comparable across machines.

---

## Experimental protocol

The same protocol was implemented in all software tools:

- **Trials**: 1000  
- **Fixation duration**: 100 ms  
- **Maximum stimulus duration**: 1000 ms  
- **Simulated response delay**: ~500 ms after stimulus onset  

Early responses ensured that timing compensation at the trial level was exercised in all applications.

---

## Results

### Hardware offset

Mean response times across all conditions clustered around **540 ms**, indicating a constant offset of approximately **40 ms** introduced by the photodiode–Arduino circuit.

This offset was identical across software tools, operating systems, and computers, and therefore does not affect comparative interpretation.

---

### StroopApp timing behaviour

StroopApp showed:

- **Low variability** (standard deviation ≈ 8 ms),
- **Stable distributions** across operating systems,
- **No missing trials** across 1000 measurements.

Observed variability is consistent with half a refresh cycle on a 60 Hz display, as expected for visually triggered stimuli.

---

### Comparison with other tools

- **PsychoPy** showed generally shorter mean response times, with occasional outliers and sporadic missing trials.
- **SuperLab** exhibited higher mean response times and greater sensitivity to operating system differences, with improved performance under Windows 11.

---

## Response time distributions

![Response time distributions on Windows 7](images/validation_rt_distributions_win7.png)
![Response time distributions on Windows 11](images/validation_rt_distributions_win11.png)

Response time distributions across software tools under Windows 7 and Windows 11.

---

## Mean response times

![Mean response times across applications and operating systems](images/validation_rt_means.png)

Mean reaction times across software tools and operating systems.

---

## Statistical considerations

Statistical analyses were conducted to support descriptive interpretation only.  
They are not used to support inferential claims regarding performance differences between tools.

Given the constant hardware offset and limited experimental conditions, results are interpreted descriptively.

---

## Conclusion

This validation shows that StroopApp provides **stable and reliable reaction time measurements** with low variability and no missing trials under controlled conditions.

The study documents timing stability rather than absolute performance and supports the suitability of StroopApp for reaction time experiments.
