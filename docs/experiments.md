# Advanced Experiment Configuration

This guide provides detailed information about configuring complex Stroop experiments in StroopApp.

## Configuration Reference

### Complete Parameter List

| Category | Parameter | Type | Range | Default | Description |
|----------|-----------|------|-------|---------|-------------|
| **Task Structure** | Calculation Mode | Enum | WordCount / TaskDuration | WordCount | How trials are calculated |
| | Trial Count | Integer | 1-10000 | 100 | Number of trials (WordCount mode) |
| | Task Duration | TimeSpan | 1s - 24h | 5 min | Total duration (TaskDuration mode) |
| **Conflict** | Congruence % | Integer | 0-100 | 50 | Percentage of congruent trials |
| **Switching** | Is Visual Cue Enabled | Boolean | true/false | false | Enable task-switching paradigm |
| | Visual Cue Duration | Integer | 0-2000 ms | 200 | Cue display time |
| | Switch % | Integer | 0-100 | 50 | Percentage of cue switches |
| **Timing** | Fixation Duration | Integer | 50-1000 ms | 100 | Cross display time |
| | Trial Duration | Integer | 100-5000 ms | 400 | Word display time |
| | Max Response Time | Integer | 500-10000 ms | 2000 | Response timeout |
| **Analysis** | Averaging Interval | Integer | 1-1000 | 10 | Grouping for statistics |

## Scientific Paradigms

### Paradigm 1: Classic Stroop Interference

**Research Question**: How much does word reading interfere with color naming?

**Configuration**:
```
Congruence: 0% (all incongruent)
Trial Count: 100
Visual Cues: Disabled
Task: Always name the color
```

**Prediction**: Slower RTs than neutral baseline (e.g., colored rectangles)

**Reference**: Original Stroop (1935)

---

### Paradigm 2: Stroop Facilitation

**Research Question**: Does congruence speed up responses?

**Configuration**:
```
Congruence: 100% (all congruent)
Trial Count: 100
Visual Cues: Disabled
```

**Prediction**: Faster RTs than incongruent or neutral conditions

---

### Paradigm 3: Task-Switching with Visual Cues (Mangin 2021)

**Research Question**: How do visual cues modulate task-level vs. response-level conflict?

**Configuration**:
```
Congruence: 25% (mostly incongruent)
Trial Count: 200
Visual Cues: Enabled
Visual Cue Duration: 200 ms
Switch %: 50% (moderate switching)
Fixation: 100 ms
```

**Measures**:
- Response conflict: Incongruent - Congruent RT
- Task conflict: Switch - Non-switch RT
- Interaction: Conflict × Switch

**Reference**: Mangin et al. (2021) - *Psychophysiology*, 58(12), e13936

---

### Paradigm 4: Practice Effects on Task-Switching (Mangin 2022)

**Research Question**: How does extended practice reduce switch costs?

**Design**: Multiple blocks with same configuration

**Configuration per block**:
```
Congruence: 50%
Trial Count: 300 per block
Blocks: 6-10
Visual Cues: Enabled
Switch %: 100% (alternating)
```

**Analysis**: Track switch cost reduction across blocks

**Reference**: Mangin et al. (2022) - *Frontiers in Psychology*, 13, 998393

---

### Paradigm 5: High-Conflict, High-Switch

**Research Question**: Maximum cognitive load condition

**Configuration**:
```
Congruence: 0% (all incongruent)
Trial Count: 500
Visual Cues: Enabled
Switch %: 100% (alternating)
Visual Cue Duration: 150 ms (brief)
```

**Warning**: Very demanding for participants; ensure breaks

---

## Parameter Interactions

### Congruence × Visual Cues

| Congruence | No Cues | With Cues (Square = Read) | With Cues (Circle = Color) |
|------------|---------|----------------------------|---------------------------|
| 0% Incongruent | Classic Stroop | Read-incongruent | Color-incongruent |
| 50% Mixed | Balanced | Task-switch + conflict | Task-switch + conflict |
| 100% Congruent | Facilitation | Read-congruent | Color-congruent |

### Switch % × Trial Count

**Low switch % (0-25%)**: 
- Blocked design
- Easier for participants
- Detects sustained task-set effects

**High switch % (75-100%)**:
- Mixed design
- More cognitively demanding
- Detects trial-by-trial switching

## Validation Rules

StroopApp enforces these constraints:

### Trial Duration Validation

**Rule**: When using **Task Duration** mode, trial duration must divide task duration evenly.

**Valid**:
- Task: 5 min (300,000 ms) ÷ Trial: 400 ms = 750 trials ?

**Invalid**:
- Task: 5 min (300,000 ms) ÷ Trial: 350 ms = 857.14 trials ?

**Solution**: Adjust trial duration to a divisor of task duration.

### Averaging Interval Validation

**Rule**: Averaging interval must divide total trial count evenly.

**Valid**:
- Trials: 100 ÷ Interval: 10 = 10 groups ?

**Invalid**:
- Trials: 100 ÷ Interval: 15 = 6.67 groups ?

**Solution**: Choose divisors like 5, 10, 20, 25, 50

## Profile Templates

### Recommended Starter Profiles

**Profile: "Quick Test (10 trials)"**
```yaml
Purpose: Test setup and key mappings
Trials: 10
Congruence: 50%
Visual Cues: Disabled
```

**Profile: "Standard Stroop (100 trials)"**
```yaml
Purpose: Standard interference measurement
Trials: 100
Congruence: 0%
Visual Cues: Disabled
Trial Duration: 400 ms
```

**Profile: "Task-Switching Short (50 trials)"**
```yaml
Purpose: Quick task-switching test
Trials: 50
Congruence: 50%
Visual Cues: Enabled
Visual Cue Duration: 200 ms
Switch %: 50%
```

**Profile: "Extended Session (5 min)"**
```yaml
Purpose: Endurance testing
Mode: Task Duration
Duration: 5 minutes
Trial Duration: 400 ms
Congruence: 25%
Visual Cues: Enabled
Switch %: 75%
```

## Pilot Testing Recommendations

Before collecting real data:

1. **Test with yourself** (Profile: "Quick Test")
   - Verify key mappings work
   - Check export folder
   - Confirm Excel file generates

2. **Test with a colleague** (Profile: "Standard Stroop")
   - Ensure instructions are clear
   - Verify real-time graphs update
   - Check data quality

3. **Run a full-length pilot** (Your actual configuration)
   - Test for participant fatigue
   - Verify timing is appropriate
   - Check statistical power with sample size

## Tips for Specific Research Goals

### Goal: Measure pure interference
? Use **0% congruence, no visual cues**

### Goal: Study facilitation
? Use **100% congruence** and compare to neutral baseline

### Goal: Investigate task-switching
? Enable **visual cues with varying switch %**

### Goal: Practice effects
? Use **multiple blocks** with identical configurations

### Goal: Individual differences
? Use **balanced design (50% congruence)** for within-subject variability

## Next Steps

- [Data Output Format](data-output.md) - Understand your exported data
- [Usage Guide](usage.md) - Return to basic workflow
