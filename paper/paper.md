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
    name: Centre Interdisciplinaire de Recherche sur le Cerveau et l'Apprentissage (CIRCA), Montreal, Canada
date: 16 April 2026
bibliography: paper.bib
---

# Summary

The Stroop color-word task is a widely used cognitive paradigm in which participants are presented with color words rendered in congruent or incongruent ink colors and must respond according to either the semantic meaning of the word or the color of its ink. Implementing the task in a research or clinical context, however, requires either costly commercial software, or substantial programming expertise, or both.
StroopApp is an open-source application for the standardized administration of Stroop tasks, requiring neither installation nor programming knowledge. The software is distributed as a single executable, providing full parametric control over sessions, real-time visual feedback through a modern interface, and automated data export.


# Statement of need

The Stroop task [@Stroop:1935] is widely used in psychology, both in research and clinical contexts [@Scarpina:2017], as well as in sport science [e.g., @Chen:2020; @Giboin:2019; @Mao:2024]. In clinical settings, the Stroop task is used to assess inhibition, a central executive function of human cognition [@MacLeod:1991; @Scarpina:2017]. In research, it is also used to measure individuals’ inhibitory capacities and self-control [e.g., @Faure:2021], but it is additionally widely employed to induce a state of ego depletion or cognitive fatigue [e.g., @Mangin:2022]. Implementing the task in a research or clinical context, however, requires either costly commercial software, substantial programming expertise, or both.

In the Stroop task, color words are presented and are either displayed in an ink color that matches the meaning of the word (e.g., the word “red” written in red ink), referred to as a “congruent trial” (or “Word test”), or in an ink color that differs from the meaning of the word (e.g., the word “red” written in blue ink), referred to as an “incongruent trial” (\hyperref[fig:stroop]{\autoref*{fig:stroop}a and 1b}). The difference in reaction time between congruent and incongruent trials reveals interference processes and therefore inhibitory control [@MacLeod:1991][^1]. However, when the Stroop task is repeated over time, either within a long session or across multiple sessions, a training effect occurs [@Dulaney:1994]. Participants gradually become more efficient at inhibiting the semantic meaning of the word and therefore respond more easily with the ink color.

To avoid this mechanism, two main methods have been implemented by researchers. In the first approach, when the word is presented in a specific color, participants are instructed not to report the ink color but instead to read the word. For example, when the word is presented in red, participants must read the word rather than report the color red [e.g., @Rozand:2014]. In the second approach, a visual cue is presented before the word, and depending on this cue, participants must either read the word or report the ink color [e.g., @Mangin:2021; @Mangin:2022]. For instance, a square or a circle may appear before the word. When participants see a square, they must read the word that follows, whereas when they see a circle, they must report the ink color. This second solution makes it possible to vary the proportion of word reading and color naming without altering the number of stimuli presented in each color (\hyperref[fig:stroop]{\autoref*{fig:stroop}c}).

[^1]: It should be noted that there is a debate in the literature regarding the baseline condition against which interference should be measured. Some authors suggest comparing incongruent Stroop trials with color name words presented in black ink to assess interference processes. This debate is beyond the scope of the present article. For further information, see [@MacLeod:1991].

![Different version of the Stroop task. Panel A represents Congruent trials. The meaning of the word matches the color of the ink, and participants have to read the word. Panel B represents Incongruent trials. The meaning of the word does not match the color of the ink, and participants have to indicate the color of the ink. Panel C represents a modified version of the incongruent Stroop task. Before the words, a visual cue, a square or a circle, is presented. When participants see a square, they have to read the following word, and when they see a circle, they have to name the color of the ink.](Figure1_Different_modalities_of_the_Stroop_task.jpeg){#fig:stroop width=100%}

It is important to note, however, that these methods introduce additional instruction as well as the executive function of task switching. Consequently, the task becomes more difficult and requires greater effort from participants [@Mangin:2026]. This latter aspect is not necessarily a limitation, since in certain protocols, such as those investigating ego depletion or cognitive fatigue, greater effort during the task leads to greater fatigue, which is precisely the effect sought by the authors [@Muller:2019]. In such cognitive fatigue protocols, performance (i.e., reaction time and/or accuracy) on the Stroop task is expected to decline as time on task increases [@Mangin:2022]. However, for performance to decrease over time, it is necessary that the task be properly learned beforehand. If it is not the case, a learning effect may occur and performance may improve, at least during the initial phase of the task. To verify that learning has been properly achieved during a familiarization phase, reaction time curves can be monitored. Reaction times are expected to decrease progressively with learning until reaching a plateau once learning is complete. At this point, it is possible to assess the experimental Stroop task to induce cognitive fatigue while controlling for the learning effects. By monitoring performance, this approach allows fatigue to be tracked throughout the task. 

Administering the Stroop task therefore requires software capable of supporting this range of experimental configurations while remaining accessible to non-specialist users. As detailed in the following section, no currently available solution simultaneously satisfies these requirements.

# State of the field

Several platforms are currently employed for Stroop task administration, varying considerably in cost, technical accessibility, and measurement precision.

E-Prime [@EPrime:2024] is a reference in the domain, acknowledged for its temporal precision, but comes with a significant licensing cost that renders it inaccessible for laboratories with more modest budgets. Proficient use additionally requires training on both the Designer interface and the E-Basic scripting language. This solution, with sufficient resources and time invested, guarantees maximal flexibility but at a maximal cost.

PsychoPy [@Peirce:2019] on the other hand, is a free, open-source platform. A complete experiment can be built using its Builder interface and Python, however, implementing a fully parameterized Stroop Task requires code-level intervention, a prerequisite that cannot be assumed across all research or clinical profiles.

SuperLab [@SuperLab:2024], combined with dedicated hardware such as LabChart [@LabChart:2024] and PowerLab [@PowerLab:2024], offers hardware-level timing precision beyond what software platforms can achieve. This comes at an investment of several thousand dollars, not accounting for the specialized training and complex setup procedures involved. This solution is therefore reserved for laboratories with dedicated psychophysiological infrastructure.

Finally, paper-based Stroop instruments remain an established clinical tool, but rely on manual response time measurement and hand-entry of results, introducing non-trivial sources of experimenter error and incurring recurring material costs across longitudinal protocols.

# Software design

The development of StroopApp is based on a central philosophy: transforming a complex process into a simple task. The code architecture follows three fundamental principles: (1) a modern software with a UX-friendly interface, (2) a simple and expeditious setup requiring no prior installation, and (3) a code-free environment. Furthermore, this commitment to open science dictated the strict selection of exclusively open-source packages for all of the software's dependencies.

To meet the requirements of research protocols, the software implements the following features:

- A simple configuration (by default), which remains highly customizable as needed via a graphical user interface (GUI).
- An experimenter interface providing detailed visual feedback of the ongoing session. This monitoring is made possible by the LiveChart package, which generates optimized, real-time performance graphs, allowing researchers to effectively monitor the learning curve during familiarization phases.
- End-to-end data management using the ClosedXML package. Results are formatted and exported through this library. To prevent any accidental data loss, the application incorporates several safeguards: confirmation and warning modals for critical actions, automatic archiving of deleted participant folders, and an automated naming convention to avoid duplicate file conflicts.

# Research impact statement

To validate the temporal precision of StroopApp [@Beghin:2026], a rigorous technical evaluation was conducted to compare its performance against standard software in the field (E-Prime, Superlab and PsychoPy). The protocol consisted of a 1000-trial session, with each word displayed for 1000 ms. An external hardware setup, utilizing an Arduino board and a photoresistor, was designed to detect a white square displayed simultaneously with the Stroop task stimulus. Upon visual detection, the arduino board introduced a strict 500 ms delay before sending a keyboard input. The results demonstrated that StroopApp provides a temporal precision for reaction time recording directly comparable to both E-Prime and PsychoPy. A consistent 40 ms additional delay was observed uniformly across all three platforms, which is attributable to the inherent hardware latency of the testing circuit.

Beyond this technical precision, distributing StroopApp as a single standalone executable guarantees robust experimental reproducibility at no cost. By eliminating dependencies on specific programming environments, this approach ensures the long-term continuity and validity of the results produced by the software.

Ultimately, StroopApp removes significant barriers in research, clinical  and educational settings. The software frees users from expensive licensing and hardware costs, as well as the need for programming expertise, while maintaining high-precision functionality. This immediate accessibility makes it an excellent resource not only for experimental use, but also for general academic teaching and the practical training of research students.  Furthermore, it offers high versatility: the software is equally suited for "standard" clinical evaluations of inhibitory control as it is for long, complex protocols designed to induce cognitive fatigue.

# AI usage disclosure

In accordance with JOSS guidelines, the authors disclose the use of generative AI tools during the development of StroopApp and the preparation of this manuscript:

- Gemini was used as a learning tool for the WPF framework, the MVVM architecture, and the XAML language.
- GitHub Copilot, through the Sonnet 3.5 and Opus 4 models, was used for code completion, code and unit test generation, refactoring and assistance with architecture planning.
- ChatGPT was used for language translation.

All AI-generated content was explicitly reviewed, verified, and validated. StroopApp is a software that followed fundamental implementation principles, through regular commits for over 6 months, more than 350 commits, 45 pull requests, and the implementation of over 340 unit tests as well as continuous integration via GitHub Actions. 

All decisions regarding the software's development direction were made during meetings between Thomas Mangin, Benjamin Pageaux, and Mylan Beghin.

# Acknowledgements

**CRediT Author Statement:**

**Mylan Beghin**: Software, Validation, Investigation, Project Administration, Funding acquisition, Writing - Original Draft, Visualization.

**Benjamin Pageaux**: Conceptualization, Supervision, Funding acquisition, Writing - Review & Editing.

**Thomas Mangin**: Conceptualization, Methodology, Formal Analysis, Investigation, Supervision, Writing - Original Draft, Writing - Review & Editing.

# References
