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

## Data Quality Checks

### Detecting Outliers

**Criteria for exclusion** (suggested):
- RT < 200 ms (anticipatory responses)
- Accuracy < 70% (participant not engaged)

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

## Next Steps

- [Return to Usage Guide](usage.md)
- [Advanced Configuration](experiments.md)
