# Summary

The Stroop effect, first described by John Ridley Stroop in 1935, is one of the most widely studied phenomena in cognitive psychology. It demonstrates the interference that occurs when the name of a color (e.g., "RED") is printed in a different ink color (e.g., blue), causing slower reaction times and more errors when participants must name the ink color rather than read the word. This paradigm has been extensively used to investigate attentional control, executive functions, and cognitive flexibility in both healthy populations and clinical groups.

StroopApp is an open-source Windows desktop application designed to facilitate the administration of Stroop tasks in experimental and clinical settings. Built using C# and Windows Presentation Foundation (WPF), the application provides a user-friendly, code-free interface for configuring experiments, managing participants, executing tasks with visual cues, and exporting results. Key features include real-time reaction time visualization, multi-language support (English and French), and comprehensive data export to Excel format.

# Statement of Need

## Existing Tools and Their Limitations

Several software solutions exist for administering cognitive tasks, including Stroop paradigms:

- **E-Prime** (Psychology Software Tools): A powerful commercial platform widely used in research laboratories. However, it requires programming knowledge, has significant licensing costs, and is Windows-only with complex installation requirements.

- **PsychoPy** [@peirce2019psychopy]: An open-source alternative offering Python-based experiment creation. While highly flexible, PsychoPy has a steep learning curve for researchers without programming experience and requires Python environment configuration.

- **OpenSesame**: Another open-source option with a graphical interface, but still requires scripting for complex paradigms and has limited real-time visualization capabilities.

## The Need for StroopApp

StroopApp addresses a specific gap in the available tools:

1. **Code-free operation**: Researchers and clinicians without programming skills can design and run experiments entirely through a graphical interface.

2. **Minimal technical requirements**: A single self-contained executable requiring no additional runtime installation, dependencies, or administrator privileges.

3. **Visual cue paradigm**: Native implementation of task-switching paradigms using geometric visual cues (square and circle), based on the methodology described in Mangin et al. [@mangin2021stroop; @mangin2022differential].

4. **Real-time monitoring**: Live visualization of reaction times and accuracy, enabling experimenters to monitor participant engagement during data collection.

5. **Clinical applicability**: A modern, intuitive interface suitable for use in clinical settings where ease of use and standardized instructions are essential.

## Technical Validation

To ensure timing accuracy critical for reaction time research, StroopApp was developed with high-precision timing using the .NET `Stopwatch` class. The application has been tested across multiple Windows versions (7, 10, 11) and hardware configurations to verify consistent performance.

# Features

## Complete Experiment Configuration

StroopApp provides comprehensive control over experiment parameters through an intuitive graphical interface:

### Trial Structure
- **Calculation mode**: Define experiments by trial count (e.g., 100 trials) or total duration (e.g., 5 minutes)
- **Congruence percentage**: Control the proportion of congruent vs. incongruent trials (0-100%)
- **Timing parameters**: Configure fixation duration (default: 100 ms), trial duration (default: 400 ms), and response timeout

### Visual Cue Task-Switching
- **Enable/disable visual cues**: Toggle the task-switching paradigm
- **Cue types**: Square (read the word) and Circle (name the ink color)
- **Switch percentage**: Control the frequency of cue alternation (0-100%)
- **Cue duration**: Configurable visual cue display time

### Participant Management
- Create and manage participant profiles with demographic information
- Automatic organization of results by participant and date
- Archive system preserving deleted participant data

## Data Export and Management

### Export Capabilities
- **Excel format**: Results exported to `.xlsx` files using the ClosedXML library
- **Comprehensive data**: Trial-level data including reaction times, accuracy, congruence, and visual cue type
- **Multi-language headers**: Column headers in English or French based on application settings

### Export Organization
- Automatic folder structure: `[ExportFolder]/Results/[ParticipantID]/[Date]/`
- Timestamped files prevent overwriting
- Cumulative export option for multi-block experiments

### Data Archiving
- Participant deletion moves data to an `Archived` folder rather than permanent deletion
- Duplicate handling for recreated participant IDs

## Visual Features

### Real-Time Graphs
StroopApp provides live visualization during experiments:

- **Global summary graph**: Displays reaction times across all completed blocks, enabling cross-session comparison
- **Current block graph**: Shows real-time data for the ongoing block
- **Running averages**: Configurable averaging intervals (e.g., every 10 trials) displayed alongside raw data
- **Accuracy statistics**: Live accuracy percentages updated after each trial

### User-Friendly, Code-Free Interface

A core design principle of StroopApp is accessibility for non-technical users:

- **Modern UI**: Built with ModernWPF for a contemporary Windows 11-style appearance
- **MVVM architecture**: Clean separation of concerns enabling future extensibility
- **Intuitive workflow**: Step-by-step configuration guiding users from setup to export
- **Auto-generated instructions**: Standardized participant instructions that adapt to the configured paradigm and selected language

The interface design was informed by principles of cognitive ergonomics, aiming to reduce cognitive load on experimenters while ensuring a distraction-free experience for participants.

# Implementation

StroopApp is implemented in C# targeting .NET 8 with Windows Presentation Foundation (WPF). The application follows the Model-View-ViewModel (MVVM) architectural pattern with dependency injection for service management.

## Technical Stack

- **Framework**: .NET 8, Windows Presentation Foundation (WPF)
- **UI Framework**: ModernWPF for contemporary styling
- **MVVM Infrastructure**: CommunityToolkit.Mvvm
- **Data Visualization**: LiveChartsCore (LiveCharts2)
- **Excel Export**: ClosedXML
- **Build System**: MSBuild with GitHub Actions CI/CD

## Quality Assurance

- **Unit Testing**: 158 unit tests covering core business logic using xUnit
- **Continuous Integration**: Automated build and test pipeline via GitHub Actions
- **Code Organization**: Clear separation between Models, ViewModels, Views, and Services

## System Requirements

- **Operating System**: Windows 7 or later (tested on Windows 7, 10, and 11)
- **RAM**: 512 MB minimum, 1 GB recommended
- **Disk Space**: 200 MB for the self-contained executable
- **Display**: 1280x720 or higher resolution

# Acknowledgements

# References
