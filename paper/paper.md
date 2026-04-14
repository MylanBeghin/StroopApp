---
title: 'StroopApp: an open-source Stroop application for researchers and clinicians'
tags:
  - Stroop task
  - cognitive assessment
  - cognitive fatigue
  - inhibitory control
  - open-source
  - C#
authors:
  - given-names: Mylan
    surname: Beghin
    orcid: 0009-0009-8290-6263
    affiliation: "1, 2"
  - given-names: Benjamin
    surname: Pageaux
    orcid: 0000-0001-9302-5183
    affiliation: "1, 2, 3"
  - given-names: Thomas
    surname: Mangin
    orcid: 0000-0002-3393-7946
    affiliation: "1, 2"
affiliations:
  - index: 1
    name: École de kinésiologie et des sciences de l'activité physique (EKSAP), Faculté de médecine, Université de Montréal, Montreal, Canada
  - index: 2
    name: Centre de recherche de l'Institut universitaire de gériatrie de Montréal (CRIUGM), Montreal, Canada
  - index: 3
    name: Centre interdisciplinaire de recherche sur le cerveau (CIRCA), Montreal, Canada
date: 14 April 2026
bibliography: paper.bib
---

# Summary

The Stroop color-word task is a widely used cognitive paradigm in which participants are presented with color words rendered in congruent or incongruent ink colors and must respond according to either the semantic meaning of the word or the color of its ink. Implementing the task in a research or clinical context, however, requires either costly commercial software, substantial programming expertise, or both.

StroopApp is an open-source application for the standardized administration of Stroop tasks, requiring neither installation nor programming knowledge. The software is distributed as a single executable, providing full parametric control over sessions, real-time visual feedback through a modern interface, and automated data export.

# Statement of need

The Stroop task [@Stroop1935] is widely used in psychology, both in research and clinical contexts [@Scarpina2017], as well as in sport science [e.g., @Chen2020; @Giboin2019; @Mao2024]. In clinical settings, the Stroop task is used to assess inhibition, a central executive function of human cognition [@MacLeod1991; @Scarpina2017]. In research, it is also used to measure individuals' inhibitory capacities and self-control [e.g., @Faure2021], but it is additionally widely employed to induce a state of ego depletion or cognitive fatigue [e.g., @Mangin2022]. Implementing the task in a research or clinical context, however, requires either costly commercial software, substantial programming expertise, or both.

In the Stroop task, color words are presented and are either displayed in an ink color that matches the meaning of the word (e.g., the word "red" written in red ink), referred to as a "congruent trial" (or "Word test"), or in an ink color that differs from the meaning of the word (e.g., the word "red" written in blue ink), referred to as an "incongruent trial" (\autoref{fig:stroop}A and \autoref{fig:stroop}B). The difference in reaction time between congruent and incongruent trials reveals interference processes and therefore inhibitory control [@MacLeod1991][^1]. However, when the Stroop task is repeated over time, either within a long session or across multiple sessions, a training effect occurs [@Dulaney1994]. Participants gradually become more efficient at inhibiting the semantic meaning of the word and therefore respond more easily with the ink color.

[^1]: It should be noted that there is a debate in the literature regarding the baseline condition against which interference should be measured. Some authors suggest comparing incongruent Stroop trials with color name words presented in black ink to assess interference processes. This debate is beyond the scope of the present article. For further information, see @MacLeod1991.

To avoid this mechanism, two main methods have been implemented by researchers. In the first approach, when the word is presented in a specific color, participants are instructed not to report the ink color but instead to read the word. In the second approach, a visual cue is presented before the word, and depending on this cue, participants must either read the word or report the ink color [e.g., @Mangin2021; @Mangin2022]. For instance, a square or a circle may appear before the word: when participants see a square, they must read the word that follows, whereas when they see a circle, they must report the ink color. This second solution makes it possible to vary the proportion of word reading and color naming without altering the number of stimuli presented in each color (\autoref{fig:stroop}C).

It is important to note, however, that these methods introduce additional instruction as well as the executive function of task switching. Consequently, the task becomes more difficult and requires greater effort from participants [@Mangin2026]. This is not necessarily a limitation: in certain protocols investigating ego depletion or cognitive fatigue, greater effort leads to greater fatigue, which is precisely the effect sought [@Muller2019]. In such protocols, performance (i.e., reaction time and/or accuracy) on the Stroop task is expected to decline as time on task increases [@Mangin2022]. However, for performance to decrease over time, it is necessary that the task be properly learned beforehand. To verify that learning has been properly achieved during a familiarization phase, reaction time curves can be monitored. Reaction times are expected to decrease progressively until reaching a plateau once learning is complete, at which point the experimental Stroop task can be administered to induce cognitive fatigue while controlling for learning effects.

Administering the Stroop task therefore requires software capable of supporting this range of experimental configurations while remaining accessible to non-specialist users. As detailed in the following section, no currently available solution simultaneously satisfies these requirements.

![Different versions of the Stroop task. Panel A represents congruent trials: the meaning of the word matches the color of the ink, and participants have to read the word. Panel B represents incongruent trials: the meaning of the word does not match the color of the ink, and participants have to indicate the color of the ink. Panel C represents a modified version of the incongruent Stroop task: before the words, a visual cue (a square or a circle) is presented. When participants see a square, they have to read the following word; when they see a circle, they have to name the color of the ink.\label{fig:stroop}](figure1.png)

# State of the field

Several platforms are currently employed for Stroop task administration, varying considerably in cost, technical accessibility, and measurement precision.

E-Prime (Psychology Software Tools) is a reference in the domain, acknowledged for its temporal precision, but comes with a significant licensing cost that renders it inaccessible for laboratories with more modest budgets. Proficient use additionally requires training on both the Designer interface and the E-Basic scripting language.

PsychoPy is a free, open-source platform. A complete experiment can be built using its Builder interface and Python; however, implementing a fully parameterized Stroop task requires code-level intervention, a prerequisite that cannot be assumed across all research or clinical profiles.

SuperLab, combined with dedicated hardware such as LabChart and PowerLab (ADInstruments), offers hardware-level timing precision beyond what software platforms can achieve. This comes at an investment of several thousand dollars, not accounting for the specialized training and complex setup procedures involved. This solution is therefore reserved for laboratories with dedicated psychophysiological infrastructure.

Finally, paper-based Stroop instruments remain an established clinical tool, but rely on manual response time measurement and hand-entry of results, introducing non-trivial sources of experimenter error and incurring recurring material costs across longitudinal protocols.

StroopApp addresses the gap left by these alternatives: it is free, requires no installation or programming knowledge, and supports the full range of experimental configurations described above — including real-time performance monitoring and automated data export — within a single standalone executable.

# Software design

The development of StroopApp is based on a central philosophy: transforming a complex process into a simple task. The code architecture follows three fundamental principles: (1) a modern software with a UX-friendly interface, (2) a simple and expeditious setup requiring no prior installation, and (3) a code-free environment. Furthermore, this commitment to open science dictated the strict selection of exclusively open-source packages for all of the software's dependencies.

To meet the requirements of research protocols, the software implements the following features. A simple configuration (by default) remains highly customizable via a graphical user interface (GUI). An experimenter interface provides detailed visual feedback of the ongoing session: this monitoring is made possible by the LiveChart package, which generates optimized, real-time performance graphs, allowing researchers to effectively monitor the learning curve during familiarization phases. End-to-end data management is handled using the ClosedXML package: results are formatted and exported through this library, and several safeguards prevent accidental data loss, including confirmation and warning modals for critical actions, automatic archiving of deleted participant folders, and an automated naming convention to avoid duplicate file conflicts.

# Research impact statement

To validate the temporal precision of StroopApp, a rigorous technical evaluation was conducted comparing its performance against standard software in the field (E-Prime, SuperLab, and PsychoPy). The protocol consisted of a 1000-trial session, with each word displayed for 1000 ms. An external hardware setup, utilizing an Arduino board and a photoresistor, was designed to detect a white square displayed simultaneously with the Stroop task stimulus. Upon visual detection, the Arduino board introduced a strict 500 ms delay before sending a keyboard input. The results demonstrated that StroopApp provides temporal precision for reaction time recording directly comparable to both E-Prime and PsychoPy. A consistent 40 ms additional delay was observed uniformly across all three platforms, attributable to the inherent hardware latency of the testing circuit.

Beyond this technical precision, distributing StroopApp as a single standalone executable guarantees robust experimental reproducibility at no cost. By eliminating dependencies on specific programming environments, this approach ensures the long-term continuity and validity of results produced by the software.

StroopApp removes significant barriers in research, clinical, and educational settings. The software frees users from expensive licensing and hardware costs, as well as the need for programming expertise, while maintaining high-precision functionality. This immediate accessibility makes it an excellent resource not only for experimental use, but also for general academic teaching and the practical training of research students. Furthermore, the software is equally suited for standard clinical evaluations of inhibitory control and for long, complex protocols designed to induce cognitive fatigue.

# AI usage disclosure

In accordance with JOSS guidelines, the authors disclose the use of generative AI tools during the development of StroopApp and the preparation of this manuscript. Gemini was used as a learning tool for the WPF framework, the MVVM architecture, and the XAML language. GitHub Copilot (Sonnet 3.5 and Opus 4 models) was used for code completion, code and unit test generation, refactoring, and assistance with architecture planning. ChatGPT was used for language translation.

All AI-generated content was explicitly reviewed, verified, and validated. StroopApp followed fundamental implementation principles across more than 350 commits over six months, 45 pull requests, and the implementation of over 340 unit tests, with continuous integration via GitHub Actions. All decisions regarding the software's development direction were made during meetings between Thomas Mangin, Benjamin Pageaux, and Mylan Beghin.

# Acknowledgements

The authors thank all participants who took part in the validation experiments.

**CRediT Author Statement:**

**Benjamin Pageaux**: Conceptualization, Supervision, Funding acquisition, Writing – Review & Editing. 
**Thomas Mangin**: Conceptualization, Methodology, Formal Analysis, Investigation, Supervision, Writing – Original Draft, Writing – Review & Editing. 
**Mylan Beghin**: Software, Validation, Investigation, Project Administration, Funding acquisition, Writing – Original Draft, Visualization.

# References