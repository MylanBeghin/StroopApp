# Usage Guide

This guide walks you through the basic workflow of conducting a Stroop experiment with StroopApp.

## Workflow Overview

```
1. Select Interface Language (optional)
   ?
2. Configure Export Folder
   ?
3. Create/Select Participant
   ?
4. Create/Select Experiment Profile
   ?
5. Configure Key Mappings
   ?
6. Launch Experiment
   ?
7. Participant Window Setup
   ?
8. Participant Completes Task
   ?
9. Export Results
```

## Step-by-Step Guide

### 1. Select Interface Language (Optional)

Before configuring your experiment:

1. Click the **Display** menu at the top of the window
2. Select **Languages**
3. Choose **English** or **Français**
4. The interface updates immediately
5. Your choice is saved and will be used for future sessions

**Note**: The selected language affects:
- All UI text and menus
- Column headers in exported Excel files
- Auto-generated participant instructions

### 2. Configure Export Folder

On the configuration page:

1. Locate the **Export Folder** section
2. Click **Browse**
3. Select a folder where results will be saved
4. The path is saved automatically

**Recommended**: Create a dedicated folder like `C:\StroopApp_Data\`

### 3. Participant Management

#### Create a New Participant

1. In the **Participant** section, click **Create**
2. Fill in the participant information:
   - **ID** (required): Unique identifier (e.g., "P001")
   - Age, Height, Weight, Sex, Gender (optional)
3. Click **Save**

#### Select an Existing Participant

1. Use the search box to filter participants
2. Click on a participant to select them
3. Click **Edit** to modify or **Delete** to remove

**Note**: Deleting a participant archives their data in `[ExportFolder]/Archived/[ParticipantID]/`. Data is never permanently deleted.

### 4. Experiment Profile Configuration

#### Create a New Profile

1. In the **Experiment Profile** section, click **Create**
2. Enter a profile name (e.g., "Standard Stroop")
3. Configure task parameters:

**Calculation Mode:**
- **Based on trial count**: Specify number of trials (e.g., 100)
- **Based on task duration**: Specify duration (e.g., 5 minutes)

**Congruence Percentage:**
- 0% = All incongruent trials
- 50% = Half congruent, half incongruent
- 100% = All congruent trials

**Visual Cue (Optional):**
- ? **Add visual cue**: Enable task-switching paradigm
- **Visual cue duration**: How long the cue is displayed (ms)
- **Switch percentage**: How often the cue changes (0-100%)
  - 100% = Alternates every trial (~N-1 switches for N trials)
  - 50% = Changes approximately every 2 trials
  - 0% = Blocked design (all same cue, then switches once)

**Timing Parameters:**
- **Fixation duration**: Cross display time (default: 100 ms)
- **Trial duration**: Word display time (default: 400 ms)
- **Averaging interval**: Group size for statistics (default: 5)

4. Click **Save**

### 5. Configure Key Mappings

Map keyboard keys to color responses:

1. In the **Key Mappings** section, click on a color field
2. Press the desired key on your keyboard
3. Repeat for all four colors (Red, Blue, Green, Yellow)

**Recommended mappings:**
- Red: `D` (right hand)
- Blue: `F` (right hand)
- Green: `J` (left hand)
- Yellow: `K` (left hand)

### 6. Launch the Experiment

1. Verify all configurations are correct
2. Click **Start Experiment**
3. **Two windows open simultaneously:**
   - **Experimenter Window** (current window): Shows dashboard, progress, controls
   - **Participant Window** (new window): Shows task stimuli (black fullscreen)

### 7. Participant Window Setup

**The participant window opens as a fullscreen black window** with no visible title bar (for distraction-free presentation).

**To move it to a second screen:**

**Method 1: Drag the invisible title bar**
1. **Move your mouse to the very top edge** of the black participant window
2. **Click and hold** (even though you don't see a title bar, it exists)
3. **Drag** the window to your second monitor
4. **Drag to the top** of the screen to snap it to fullscreen

**Method 2: Windows keyboard shortcut**
1. Click on the participant window to ensure it has focus
2. Press `Win + Shift + Right Arrow` (or Left) to move between screens

**Important - Setting Keyboard Focus:**
- **Click anywhere on the participant window** before starting
- This ensures keyboard input (Space bar, color responses) is captured
- Without focus, the instructions won't advance

**Auto-Generated Instructions:**

The participant window displays **3 instruction pages** automatically:
- Instructions adapt to your experiment configuration (with/without visual cues)
- Available in **English or French** (based on language selection from Step 1)
- Participant presses **Space bar** to advance through pages
- **No need to manually explain the task** — the app provides standardized instructions

### 8. During the Experiment

**Task flow for each trial:**
1. Fixation cross appears (100 ms default)
2. (If enabled) Visual cue surrounds the cross (square or circle)
3. Colored word appears
4. Participant presses the corresponding color key
5. (Optional) Feedback appears briefly

**Experimenter Dashboard shows:**
- Real-time reaction times (live graph)
- Progress bar with trials completed / total
- Current block statistics (accuracy, mean RT)
- Live averages (grouped by averaging interval)

**To interrupt the experiment:**
- Click **Stop Task** button in experimenter window
- Partial data will be preserved
- You can export incomplete blocks

### 9. Export Results

After the experiment completes:

1. The **Export Window** appears automatically
2. Verify the export folder path is correct
3. Click **Export**
4. Wait for confirmation message
5. Click **Open Results Folder** to view the Excel file

**Export file location:**
```
[ExportFolder]/Results/[ParticipantID]/[Date]/
  ??? [ParticipantID]_[Timestamp].xlsx
```

**Example:**
```
C:\StroopData\Results\P001\2026-01-28\
  ??? P001_2026-01-28_14-35-22.xlsx
```

## Common Workflows

### Workflow A: Single Session, Multiple Participants

1. Configure export folder **once**
2. Create experiment profile **once**
3. For each participant:
   - Create participant
   - Select profile
   - Launch experiment
   - Export results
   - Click **New Experiment** to reset for next participant

### Workflow B: Repeated Measures (Same Participant, Multiple Sessions)

1. Create participant **once**
2. Create profile **once**
3. For each session (e.g., Day 1, Day 2, Day 3):
   - Select existing participant
   - Select profile
   - Launch experiment
   - Export results (automatically organized by date in separate folders)

### Workflow C: Multiple Conditions (Different Profiles)

1. Create multiple profiles with different configurations:
   - "Profile A - Incongruent Only"
   - "Profile B - 50-50 Mix"
   - "Profile C - With Visual Cues"
2. For each participant:
   - Run experiment with Profile A ? Export
   - Click **Continue with same participant**
   - Select Profile B ? Run ? Export
   - Select Profile C ? Run ? Export
   - Result: 3 separate Excel files (one per block/profile)

## Tips & Best Practices

### Before Data Collection
- ? **Test the setup** with yourself or a colleague (use a "Test" profile with 10 trials)
- ? **Verify key mappings** with the participant before starting
- ? **Ensure a quiet, distraction-free environment**
- ? **Check that the export folder is accessible and has write permissions**
- ? **Position participant window** on the correct screen before starting
- ? **Test keyboard focus** by pressing Space on the instruction page

### During Data Collection
- ? **Monitor the experimenter dashboard** for anomalies (e.g., all incorrect responses)
- ? **Keep room lighting consistent** (affects color perception)
- ? **Minimize interruptions** (close other applications, silence phones)
- ? **Don't interact with the experimenter window** during the task (can distract participant)

### After Data Collection
- ? **Export immediately** after each session (don't close the app without exporting)
- ? **Verify the Excel file opens correctly** in Excel/LibreOffice
- ? **Backup data regularly** to external drive or cloud storage
- ? **Archive completed studies** to prevent accidental overwrite

## Troubleshooting

### Participant window not responding to keyboard

**Symptom**: Pressing Space or color keys does nothing

**Solution**:
1. Click on the participant window to set keyboard focus
2. Ensure no other window is overlapping
3. Check that the correct window has focus (border may flash)

### Can't find the participant window

**Symptom**: Window opened but not visible

**Solutions**:
1. Check if it opened on another monitor
2. Press `Alt + Tab` to cycle through windows
3. Use `Win + Shift + Left/Right Arrow` to move it

### Instructions won't advance

**Symptom**: Space bar doesn't move to next instruction page

**Solution**:
1. Ensure participant window has keyboard focus (click on it)
2. Check that Space key is working (test in Notepad)
3. Restart the experiment if issue persists

### Export fails

**Symptom**: Export button doesn't work or shows error

**Causes & Solutions**:
1. **Export folder not configured**: Go back and select a folder
2. **Folder doesn't exist**: Verify the path is valid
3. **No write permissions**: Choose a folder you own (e.g., Documents)
4. **Disk full**: Check available disk space

## Next Steps

- [Experiment Configuration Details](experiments.md)
- [Understanding Data Output](data-output.md)
