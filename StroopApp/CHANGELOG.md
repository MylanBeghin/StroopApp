# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),  
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.0.0] – 2026-01-30

### Added
- First stable public release of **StroopApp**.
- Stroop task execution with visual cues and keyboard responses.
- Participant and experiment profile management.
- Real-time reaction time visualization.
- Multi-language user interface (English / French).
- Export of experiment results to Excel (`.xlsx`).
- GitHub Actions CI with Release build and executable artifact generation.

### Fixed
- Removal of obsolete domain and infrastructure code:
  - Deprecated `Result` model and related unit tests.
  - Unused user notification abstraction (`IUserNotifier`, `DefaultNotifier`).
- Improved error reporting with clearer and localized messages (EN / FR).
- Cleanup of unit tests to follow modern xUnit assertions.
- Reduced build noise by addressing non-actionable test warnings.

### Changed
- Improved internal code organization (enum extraction, clearer responsibilities).
- Updated resource files to better reflect current application behavior.
- Minor internal refactors with no functional impact.

### Notes
- The application is considered stable and suitable for research and educational use.
- Some dependencies (e.g. charting components) rely on release-candidate versions; this is documented and considered acceptable for v1.
- Nullable reference warnings and large-scale refactors are intentionally deferred to post-v1 iterations.

---

## [Unreleased]

### Planned Features

#### Serial Port Communication for MRI/EEG Synchronization
**Status**: Prototype UX created, awaiting needs analysis

**Background**:  
Initial development explored integration with MRI/EEG equipment via serial port triggers. The goal was to synchronize stimulus presentation (fixation cross, visual cues, colored words) with external recording devices.

**Challenges encountered**:
- No access to MRI equipment for real-world testing and validation
- Unclear requirements for trigger timing and configuration
- Complex parameter space requiring domain expertise:
  - When to send triggers? (pre-stimulus, post-stimulus, on-response?)
  - Configurable delays (e.g., 50ms before stimulus onset)
  - Event types to transmit (trial start, stimulus type, response, block boundaries)

**Status for v1.0.0**:  
Not included due to insufficient needs analysis and lack of testing infrastructure.

**Next steps**:
- Conduct needs analysis with researchers using MRI/EEG in Stroop studies
- Define essential configuration parameters
- Develop testable implementation with simulated serial port
- Validate with real MRI/EEG equipment before inclusion

**Questions to resolve with research community**:
- What trigger timing is standard in your field?
- Which stimulus events require external synchronization?
- What configuration flexibility is needed?

---

#### Multi-Screen Window Management
- Choose which screen to open participant/experimenter windows
- Remember window positions between sessions  
- Easier window positioning (currently requires dragging invisible title bar)

#### Customizable Instructions
- Create custom instruction pages
- Add/remove instruction screens
- Per-profile instruction templates

#### Export Menu UX Redesign
- Clearer export/quit application workflow
- Simplified end-of-experiment actions

#### Additional Experiment Parameters
- Support for more than 4 colors
- Customizable color palette
- Extended trial types

#### CSV Export Format
- Universal compatibility for data analysis tools

#### Documentation
- Expanded troubleshooting guides
- Video tutorials
- Community-contributed experiment templates

---

### Technical Improvements
- Enhanced error recovery mechanisms
- Optimized memory usage for very large datasets (10,000+ trials)
- Windows Defender compatibility improvements
