# Data Output Format

This guide explains the structure of exported data files from StroopApp.

## Export File Location

Results are saved to:
```
[ExportFolder]/Results/[ParticipantID]/[Date]/
    [ParticipantID]_[Timestamp].xlsx
```

**Example**:
```
C:\StroopData\Results\P001\2026-01-28\
    P001_2026-01-28_14-35-22.xlsx
```

## Excel File Structure

Each Excel file contains **one sheet** with the following columns.

**Note**: Column headers are displayed in the language selected in the application (English or French). The examples below show English headers.

| Column | Type | Description | Example |
|--------|------|-------------|---------|
| **ParticipantId** | Integer | Participant identifier | `1` |
| **StroopType** | String | Experiment profile name | `"Standard Stroop"` |
| **Block** | Integer | Block number (1-indexed) | `1` |
| **TrialNumber** | Integer | Trial number within block | `42` |
| **ExpectedResponse** | String | Correct answer | `"Red"` |
| **GivenResponse** | String | Participant's actual response | `"Blue"` |
| **IsCorrect** | Boolean | Was response correct? | `FALSE` |
| **ReactionTime** | Integer | Response time in milliseconds | `687` |
| **Congruence** | String | Trial type | `"Incongruent"` |
| **VisualCueType** | String | Cue shown (if enabled) | `"Circle"` or `"Square"` or `null` |

## Column Details

### ParticipantId
- Numeric identifier from participant creation
- Used for organizing data by participant

### StroopType
- Name of the experiment profile used
- Useful when running multiple configurations

### Block
- Block number (starts at 1)
- Increment for each new experiment run with same participant

### TrialNumber
- Trial number within the current block
- Resets to 1 for each new block

### ExpectedResponse
- The correct color name for this trial
- Based on task rules (cue type, congruence)

**Examples**:
- Square cue + word "RED" = Expected: `"Red"` (read word)
- Circle cue + word "RED" in blue = Expected: `"Blue"` (name color)

### GivenResponse
- The key the participant actually pressed
- Mapped to color name (e.g., "D" key = "Red")
- Empty if no response within timeout

### IsCorrect
- `TRUE`: Expected = Given
- `FALSE`: Expected != Given or timeout

### ReactionTime
- Time in milliseconds from word appearance to key press
- `null` if no response (timeout)
- Measured with high precision (Stopwatch)

### Congruence
- `"Congruent"`: Word meaning matches ink color
- `"Incongruent"`: Word meaning differs from ink color

**Example**:
- Word "RED" in red ink = Congruent
- Word "RED" in blue ink = Incongruent

### VisualCueType
- `"Square"`: Read-word task
- `"Circle"`: Name-color task
- `null` or empty: No visual cue used

## Data Analysis Examples

### Basic Analysis in Excel

#### Calculate Mean RT by Condition

**Incongruent trials only:**
```excel
=AVERAGEIF(I:I, "Incongruent", H:H)
```

**Congruent trials only:**
```excel
=AVERAGEIF(I:I, "Congruent", H:H)
```

**Stroop interference:**
```excel
=AVERAGEIF(I:I, "Incongruent", H:H) - AVERAGEIF(I:I, "Congruent", H:H)
```

#### Calculate Accuracy

```excel
=COUNTIF(G:G, TRUE) / COUNTA(G:G) * 100
```

### Analysis in R

```r
# Load data
library(readxl)
data <- read_excel("StroopApp_P001_1_20250127_143522.xlsx")

# Calculate mean RTs
aggregate(ReactionTime ~ Congruence, data, mean)

# Stroop effect (incongruent - congruent)
incongruent_rt <- mean(data$ReactionTime[data$Congruence == "Incongruent"])
congruent_rt <- mean(data$ReactionTime[data$Congruence == "Congruent"])
stroop_effect <- incongruent_rt - congruent_rt

# Switch cost (if visual cues used)
# Filter out first trial of each cue block, then:
# switch_cost = RT(switch trials) - RT(non-switch trials)
```

### Analysis in Python (pandas)

```python
import pandas as pd

# Load data
df = pd.read_excel("StroopApp_P001_1_20250127_143522.xlsx")

# Mean RT by congruence
df.groupby('Congruence')['ReactionTime'].mean()

# Accuracy
accuracy = (df['IsCorrect'].sum() / len(df)) * 100

# Filter correct responses only
correct_df = df[df['IsCorrect'] == True]

# Stroop effect on correct trials
stroop_effect = (
    correct_df[correct_df['Congruence'] == 'Incongruent']['ReactionTime'].mean() -
    correct_df[correct_df['Congruence'] == 'Congruent']['ReactionTime'].mean()
)

print(f"Stroop Effect: {stroop_effect:.0f} ms")
```

## Data Quality Checks

### Detecting Outliers

**Criteria for exclusion** (suggested):
- RT < 200 ms (anticipatory responses)
- RT > 3 standard deviations from mean
- Accuracy < 70% (participant not engaged)

**Excel formula** (flag outliers):
```excel
=IF(OR(H2<200, H2>AVERAGE($H:$H)+3*STDEV($H:$H)), "OUTLIER", "OK")
```

### Missing Data

Check for trials with no response:
```excel
=COUNTBLANK(F:F)
```

## Multi-Block Analysis

When analyzing multiple blocks for the same participant:

### Combine Blocks in R

```r
files <- list.files(path = "C:/StroopData/Results/P001/2025-01-27/",
                   pattern = "*.xlsx",
                   full.names = TRUE)

# Read all files
data_list <- lapply(files, read_excel)

# Combine
all_data <- do.call(rbind, data_list)

# Analyze across blocks
aggregate(ReactionTime ~ Block + Congruence, all_data, mean)
```

## Expected Data Patterns

### Typical RT Patterns

| Condition | Expected Mean RT | Typical Range |
|-----------|------------------|---------------|
| Congruent | 450-550 ms | Baseline |
| Incongruent | 600-750 ms | +150-200 ms |
| Neutral (if tested) | 500-600 ms | Between |

### Switch Costs

| Metric | Typical Value |
|--------|---------------|
| Switch cost (RT switch - no switch) | 50-150 ms |
| Switch cost reduction with practice | 10-30 ms per block |

### Accuracy

| Condition | Expected Accuracy |
|-----------|-------------------|
| Congruent | 95-99% |
| Incongruent | 85-95% |
| Overall | >90% (for valid data) |

## Data Archiving

### Participant Deletion

When you delete a participant in StroopApp:
- Their data is **moved** to `[ExportFolder]/Archived/[ParticipantID]/`
- Original files are preserved
- Not permanently deleted

### Backup Recommendations

1. **Daily**: Backup active `Results/` folder
2. **Weekly**: Backup entire export directory
3. **Post-study**: Archive to cloud (OneDrive, Google Drive, institutional storage)

## Frequently Asked Questions

### Q: Can I edit the Excel file after export?

**A**: Yes, but create a copy first. StroopApp doesn't re-read exported files, so your edits won't affect the app.

### Q: How do I combine data from multiple participants?

**A**: Use R, Python, or Excel Power Query to merge files. Ensure `ParticipantId` column distinguishes participants.

### Q: What if a participant presses the wrong key accidentally?

**A**: The trial is recorded as incorrect (`IsCorrect = FALSE`). This is valid data showing human error. Only exclude if accuracy is systematically low (<70%).

### Q: Can I re-export data?

**A**: Each export creates a new timestamped file. Previous exports are never overwritten.

## Statistical Analysis Recommendations

### Recommended Exclusions

1. **Practice trials**: Exclude first 10-20 trials per block
2. **Timeout trials**: RT = 0 or missing response
3. **Anticipatory**: RT < 200 ms
4. **Outliers**: RT > 3 SD from participant mean
5. **Low accuracy**: Participants with <70-80% overall accuracy

### Recommended Measures

**Primary:**
- Mean RT (correct trials only)
- Accuracy (% correct)
- Stroop effect (Incongruent RT - Congruent RT)

**Secondary (if visual cues used):**
- Switch cost (Switch RT - Non-switch RT)
- Task conflict x Response conflict interaction
- Switch cost reduction across blocks (practice effects)

## Next Steps

- [Return to Usage Guide](usage.md)
- [Advanced Configuration](experiments.md)
