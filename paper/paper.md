---
title: 'StroopApp: A Modern WPF Application for Administering Stroop Tasks in Experimental Research'
tags:
  - C#
  - WPF
  - experimental psychology
  - Stroop task
  - cognitive neuroscience
  - visual cues
authors:
  - name: Mylan Beghin
    orcid: 0000-0000-0000-0000
    affiliation: 1
affiliations:
  - name: Independent Researcher, Belgium
    index: 1
date: 27 January 2025
bibliography: paper.bib
---

# Summary

StroopApp is an open-source Windows Presentation Foundation (WPF) application designed to facilitate the administration of Stroop tasks in experimental psychology research. The software implements the visual cue paradigm described by @Mangin2021 and @Mangin2022, enabling researchers to configure task-switching experiments with precise control over timing, congruence ratios, and visual cue presentation.

The application provides a user-friendly interface for configuring experiment parameters, managing participant data, executing tasks, and exporting results in Excel format. Real-time visualization of reaction times allows experimenters to monitor performance during data collection.

# Statement of Need

Stroop tasks are fundamental tools in cognitive psychology for studying attentional control, executive functions, and task-switching abilities. While commercial software exists (e.g., E-Prime, PsychoPy), there is a gap for lightweight, open-source, Windows-native applications that combine ease of use with the flexibility required for modern Stroop paradigms, particularly those involving visual cues for task-switching.

StroopApp addresses this need by providing:

1. **Configurable visual cue paradigms**: Implementation of the methodology from @Mangin2021 and @Mangin2022, allowing researchers to manipulate task-switching demands through geometric cues (round vs. square shapes).

2. **No-code configuration**: Researchers without programming skills can define experiment parameters (congruence percentage, switch percentage, trial durations) through a graphical interface.

3. **Minimal installation requirements**: Self-contained executable requiring no additional runtime installation or dependencies.

4. **Multi-language support**: Interface available in English and French, facilitating international research collaboration.

# Key Features

- **Task generation**: Automatic generation of trial sequences based on congruence and switch percentage parameters
- **Real-time monitoring**: Live visualization of reaction times and accuracy using LiveCharts2
- **Data management**: Participant and profile management with persistent storage
- **Export capabilities**: Results exported to Excel (.xlsx) format with comprehensive trial-level data
- **Customizable key mappings**: Flexible color-response key assignments
- **Block-based design**: Support for multi-block experiments with interruption/resume capability

# Implementation

StroopApp is implemented in C# using .NET 8 and Windows Presentation Foundation (WPF). The architecture follows the Model-View-ViewModel (MVVM) pattern with dependency injection for service management. The application uses:

- **ClosedXML** for Excel export generation
- **LiveChartsCore** for real-time data visualization
- **ModernWPF** for contemporary user interface styling
- **CommunityToolkit.Mvvm** for MVVM infrastructure

The codebase includes 158 unit tests covering core business logic, with continuous integration via GitHub Actions.

# Research Applications

StroopApp has been designed to support research similar to @Mangin2021 and @Mangin2022, which demonstrated:

- Task-level conflict in visual cue Stroop paradigms
- Differential effects of practice on task-switching components
- Event-related potential correlates of Stroop conflict

The software's configurability makes it suitable for investigating various cognitive phenomena including response conflict, task conflict, cognitive flexibility, and executive control mechanisms.

# Acknowledgements

This software implements experimental paradigms developed by Thomas Mangin and colleagues. Special thanks to the open-source community for the libraries that made this project possible.

# References
