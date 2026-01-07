# Phase C0 - Analyse Exhaustive de SharedExperimentData et son Écosystème

## ?? Objectif

Produire une analyse factuelle et exhaustive de `SharedExperimentData` et de ses dépendances graphiques avant l'extraction de la logique LiveCharts (Phase C).

**Contrainte**: Aucune modification de code, lecture seule, rapport basé uniquement sur les faits observés.

---

## 1?? Inventaire Exhaustif de SharedExperimentData

### Héritage

```csharp
public class SharedExperimentData : ModelBase
```

**Héritage**: `ModelBase` (fournit `INotifyPropertyChanged` via `OnPropertyChanged()`)

### Propriétés (16 total)

#### Propriétés de Collections - Métier

| Propriété | Type | Visibilité | Init | Catégorie | Description Factuelle |
|-----------|------|------------|------|-----------|----------------------|
| `Blocks` | `ObservableCollection<Block>` | get-only | Ctor | Métier | Collection de blocs d'expérience. Initialisée vide. |
| `ReactionPoints` | `ObservableCollection<ReactionTimePoint>` | get/set | Ctor | Métier | Points de données (trial, RT, validité). Initialisée vide. |

#### Propriétés de Collections - Graphique (LiveCharts)

| Propriété | Type | Visibilité | Init | Catégorie | Dépendance LiveCharts |
|-----------|------|------------|------|-----------|----------------------|
| `BlockSeries` | `ObservableCollection<ISeries>` | get-only | Ctor | Graphique | **OUI** - `ISeries` de LiveChartsCore |
| `ColumnSerie` | `ObservableCollection<ISeries>?` | get/set | `NewColumnSerie()` | Graphique | **OUI** - `ISeries` de LiveChartsCore |
| `Sections` | `ObservableCollection<RectangularSection>` | get-only | Ctor | Graphique | **OUI** - `RectangularSection` de LiveChartsCore |

#### Propriétés d'État Runtime

| Propriété | Type | Visibilité | Default | Catégorie | Description |
|-----------|------|------------|---------|-----------|-------------|
| `CurrentBlock` | `Block?` | get/set | null | Métier | Bloc actuellement en cours. Backing field avec PropertyChanged. |
| `CurrentTrial` | `StroopTrial?` | get/set | null | Métier | Trial actuel. **Event subscription** sur PropertyChanged du trial. |
| `IsBlockFinished` | `bool` | get/set | false | État | Flag indiquant fin de bloc. |
| `IsTaskStopped` | `bool` | get/set | false | État | Flag indiquant arrêt de tâche. |
| `IsParticipantSelectionEnabled` | `bool` | get/set | true | État | Flag permettant sélection participant. |
| `HasUnsavedExports` | `bool` | get/set | true | État | Flag indiquant exports non sauvegardés. |
| `NextAction` | `ExperimentAction` | get/set | None | État | Prochaine action après fin de bloc (enum). |

#### Propriétés Internes - Indices

| Propriété | Type | Visibilité | Default | Catégorie | Description |
|-----------|------|------------|---------|-----------|-------------|
| `currentBlockStart` | `int` | public field | 1 | Graphique | Index de départ du bloc courant (pour mapping LiveCharts). |
| `currentBlockEnd` | `int` | public field | 0 | Graphique | Index de fin du bloc courant (pour mapping LiveCharts). |

#### Champs Privés - Graphique

| Champ | Type | Visibilité | Catégorie | Description |
|-------|------|------------|-----------|-------------|
| `_palette` | `SKColor[]` | private readonly | Graphique | Palette de 4 couleurs SkiaSharp pour séries. |
| `_colorIndex` | `int` | public field | Graphique | Index cyclique dans la palette (public!). |

### Méthodes (4 total)

#### Méthodes Publiques

| Méthode | Signature | Catégorie | Description Factuelle |
|---------|-----------|-----------|----------------------|
| `NewColumnSerie()` | `void NewColumnSerie()` | Graphique | **Crée** une nouvelle `ColumnSerie<ReactionTimePoint>` avec configuration LiveCharts complète. Affecte à `ColumnSerie`. |
| `AddNewSerie(ExperimentSettings)` | `void AddNewSerie(ExperimentSettings _settings)` | Legacy/Délégation | **Délègue** vers surcharge interface via adaptateur (Phase A). |
| `AddNewSerie(IBlockConfiguration)` | `void AddNewSerie(IBlockConfiguration config)` | **MIXTE** Métier+Graphique | **Crée** un Block, l'ajoute à `Blocks`. **Crée** `LineSeries` et `RectangularSection` LiveCharts. **Incrémente** `_colorIndex`. |
| `Reset()` | `virtual void Reset()` | Mixte | **Clear** toutes collections. **Réinitialise** flags état. **Appelle** `NewColumnSerie()`. |

#### Méthodes Privées

| Méthode | Signature | Catégorie | Description |
|---------|-----------|-----------|-------------|
| `CurrentTrial_PropertyChanged` | `void CurrentTrial_PropertyChanged(object, PropertyChangedEventArgs)` | Métier | Handler d'événement. Si `TrialNumber` change, propage `PropertyChanged(CurrentTrial)`. |

### Constructeur

```csharp
public SharedExperimentData()
{
    Blocks = new ObservableCollection<Block>();
    BlockSeries = new ObservableCollection<ISeries>();
    Sections = new ObservableCollection<RectangularSection>();
    ReactionPoints = new ObservableCollection<ReactionTimePoint>();
    NewColumnSerie();  // Appel méthode graphique
    currentBlockStart = 1;
}
```

**Observations**:
- Initialise 5 collections
- **Appelle immédiatement** `NewColumnSerie()` (logique graphique dans constructeur)
- Initialise `currentBlockStart` à 1

---

## 2?? Dépendances Graphiques LiveCharts

### Types LiveCharts Utilisés

#### Dans SharedExperimentData.cs

| Type LiveCharts | Namespace | Utilisé Dans | Ligne(s) |
|-----------------|-----------|--------------|----------|
| `ISeries` | `LiveChartsCore` | `BlockSeries`, `ColumnSerie` | Propriétés, constructeur |
| `ColumnSeries<T>` | `LiveChartsCore.SkiaSharpView` | `NewColumnSerie()` | Ligne 182-220 |
| `LineSeries<T>` | `LiveChartsCore.SkiaSharpView` | `AddNewSerie()` | Ligne 252-261 |
| `RectangularSection` | `LiveChartsCore.SkiaSharpView` | `Sections`, `AddNewSerie()` | Propriétés, ligne 262-268 |
| `SolidColorPaint` | `LiveChartsCore.SkiaSharpView.Painting` | Multiples (strokes, fills, labels) | Lignes 185-220, 252-268 |
| `Coordinate` | `LiveChartsCore.Kernel` | Mapping lambda | Lignes 200, 259 |
| `ChartPoint<T>.OnPointCreated` | `LiveChartsCore.Kernel.Events` | Configuration colonne | Ligne 201-220 |

#### Dans les SKColors

| Type SkiaSharp | Utilisé Pour |
|----------------|--------------|
| `SKColor` | Palette de couleurs (`_palette`), couleurs custom (orange, purple) |
| `SKColors` | Constantes (Black, CornflowerBlue, OrangeRed, MediumSeaGreen, Goldenrod) |
| `.WithAlpha()` | Création de couleur semi-transparente pour fill |

### Instanciation des Objets LiveCharts

#### NewColumnSerie() - Ligne 178-220

**Crée**:
- `ObservableCollection<ISeries>` contenant 1 série
- `ColumnSeries<ReactionTimePoint>` configurée avec:
  - `Values = ReactionPoints` (binding à la collection métier)
  - `DataLabelsPosition`, `DataLabelsSize`, `DataLabelsPaint`
  - `DataLabelsFormatter` (lambda: `double.NaN` ? "Aucune réponse", sinon `N0`)
  - `XToolTipLabelFormatter` (lambda: formatte "Essai n°X")
  - `YToolTipLabelFormatter` (lambda: formatte temps réaction ou "Pas de réponse")
  - `Mapping` (lambda: `TrialNumber-1` ? X, `ReactionTime ?? double.NaN` ? Y)
  - `.OnPointCreated()` avec lambda:
    - Si `IsValidResponse == true` ? couleur purple (#5B2EFF)
    - Si `IsValidResponse == false` ? couleur orange (#FFA600)
    - Applique `Fill` et `Stroke` via `SolidColorPaint`

**Effet de bord**: Assigne directement à `ColumnSerie`

#### AddNewSerie(IBlockConfiguration) - Ligne 237-271

**Crée (métier)**:
- `CurrentBlock = new Block(config)`
- Ajoute à `Blocks`

**Crée (graphique)**:
- `LineSeries<double?>` avec:
  - `Values = CurrentBlock.TrialTimes`
  - `Stroke`, `Fill`, `LineSmoothness`, `GeometrySize`, `GeometryStroke`, `GeometryFill`
  - `Mapping` (lambda: mappage index vers coordonnées)
- `RectangularSection` avec:
  - `Xi`, `Xj` (range de trials)
  - `Fill` (couleur de palette avec alpha)
  - `Label` (e.g. "Bloc n°2")
  - `LabelSize`, `LabelPaint`

**Effets de bord**:
- Ajoute `LineSeries` à `BlockSeries`
- Ajoute `RectangularSection` à `Sections`
- Incrémente `_colorIndex`
- Met à jour `currentBlockEnd`
- Met à jour `currentBlockStart` (pour bloc suivant)

### ViewModels Consommateurs

#### ColumnGraphViewModel

**Fichier**: `StroopApp\ViewModels\Experiment\Experimenter\ColumnGraphViewModel.cs`

**Utilise**:
- `settings.ExperimentContext.ColumnSerie` ? bind à propriété graphique
- `settings.ExperimentContext.ReactionPoints` ? event `CollectionChanged` pour ajuster axes
- Crée `Axis[]` XAxes et YAxes (LiveCharts)
- `TooltipFormatter` (custom formatting)

**Dépendance directe**: Lecture de `ColumnSerie` créée par `NewColumnSerie()`

#### GlobalGraphViewModel

**Fichier**: `StroopApp\ViewModels\Experiment\Experimenter\GlobalGraphViewModel.cs`

**Utilise**:
- `settings.ExperimentContext.BlockSeries` ? bind ISeries
- `settings.ExperimentContext.Sections` ? bind RectangularSection
- `settings.ExperimentContext.Blocks` ? calcul totalTrials et maxRT
- Crée `Axis[]` XAxes et YAxes

**Dépendance directe**: Lecture de `BlockSeries` et `Sections` créées par `AddNewSerie()`

#### EndExperimentPageViewModel

**Fichier**: `StroopApp\ViewModels\Experiment\Experimenter\End\EndExperimentPageViewModel.cs`

**Utilise**:
- `Settings.ExperimentContext.Blocks` (bind)
- `Settings.ExperimentContext.ReactionPoints` (lecture + snapshot)
- `Settings.ExperimentContext.ColumnSerie` (réaffectation complète!)
- `Settings.ExperimentContext.NewColumnSerie()` (appel méthode)

**Observation critique**: `UpdateBlock()` **recrée** complètement `ColumnSerie` avec un snapshot de `ReactionPoints`. **Duplication de logique** de `NewColumnSerie()`.

#### LiveReactionTimeViewModel

**Fichier**: `StroopApp\ViewModels\Experiment\Experimenter\LiveReactionTimeViewModel.cs`

**Utilise**:
- `_settings.ExperimentContext.ReactionPoints` (bind + event CollectionChanged)
- Ne touche **pas** aux objets LiveCharts directement
- Calcule des moyennes par groupe et les expose via `GroupAverages`

**Observation**: ViewModel métier pur, pas de dépendance LiveCharts.

---

## 3?? Flux d'Appel Réels

### Flux 1: Initialisation de l'Expérience

```
ExperimentSettings constructor
  ??> ExperimentRunState constructor
      ??> SharedExperimentData constructor
          ??> Blocks = new ObservableCollection<Block>()
          ??> BlockSeries = new ObservableCollection<ISeries>()
          ??> Sections = new ObservableCollection<RectangularSection>()
          ??> ReactionPoints = new ObservableCollection<ReactionTimePoint>()
          ??> NewColumnSerie()  ? GRAPHIQUE
          ?   ??> Crée ColumnSeries<ReactionTimePoint> avec formatters
          ??> currentBlockStart = 1
```

**Observation**: La logique graphique est **déclenchée dès la construction** de SharedExperimentData.

### Flux 2: Ajout d'un Nouveau Bloc

```
ConfigurationPageViewModel.LaunchExperimentAsync()
  ??> settings.ExperimentContext.IsTaskStopped = false
  ??> settings.ExperimentContext.IsBlockFinished = false
  ??> settings.ExperimentContext.NewColumnSerie()  ? GRAPHIQUE
  ??> settings.ExperimentContext.AddNewSerie(settings)
      ??> Adaptateur ? AddNewSerie(IBlockConfiguration)
      ?   ??> CurrentBlock = new Block(config)  ? MÉTIER
      ?   ??> Blocks.Add(CurrentBlock)  ? MÉTIER
      ?   ??> Calcul start/end trials
      ?   ??> BlockSeries.Add(new LineSeries<double?> {...})  ? GRAPHIQUE
      ?   ??> Sections.Add(new RectangularSection {...})  ? GRAPHIQUE
      ?   ??> _colorIndex++
      ?   ??> currentBlockStart = end + 1
      ??> trials = trialGenService.GenerateTrials(settings)
      ??> foreach trial in trials
          ??> CurrentBlock.TrialRecords.Add(trial)
```

**Observation**: `AddNewSerie()` **mélange** logique métier (création Block) et logique graphique (LineSeries, RectangularSection).

### Flux 3: Reset de l'Expérience

```
ExperimentSettings.Reset()
  ??> ExperimentContext.Reset()
      ??> Blocks.Clear()  ? MÉTIER
      ??> BlockSeries.Clear()  ? GRAPHIQUE
      ??> Sections.Clear()  ? GRAPHIQUE
      ??> ReactionPoints.Clear()  ? MÉTIER
      ??> IsTaskStopped = false
      ??> _colorIndex = 0
      ??> currentBlockStart = 1
      ??> currentBlockEnd = 0
      ??> CurrentBlock = null
      ??> IsBlockFinished = false
      ??> HasUnsavedExports = true
      ??> NextAction = ExperimentAction.None
      ??> NewColumnSerie()  ? GRAPHIQUE
```

**Ordre documenté par tests**:
1. `ExperimentContext.Reset()` appelé en premier
2. `Block = 1` ensuite
3. Flags état positionnés
4. `PropertyChanged(string.Empty)` à la fin

### Flux 4: Affichage Graphique (Binding)

```
EndExperimentPage.xaml.cs constructor
  ??> GlobalGraphView(settings)
  ?   ??> GlobalGraphViewModel(settings)
  ?       ??> Series = settings.ExperimentContext.BlockSeries  ? BIND
  ?       ??> Sections = settings.ExperimentContext.Sections  ? BIND
  ?       ??> Calcul axes depuis Blocks (métier)
  ??> LiveReactionTimeView(settings)
      ??> LiveReactionTimeViewModel(settings)
          ??> ReactionPoints = settings.ExperimentContext.ReactionPoints  ? BIND
              ??> Event CollectionChanged
```

**Observation**: Les ViewModels graphiques **lisent directement** les collections LiveCharts de `SharedExperimentData`.

### Flux 5: Mise à Jour UpdateBlock (EndExperimentPageViewModel)

```
EndExperimentPageViewModel.UpdateBlock()
  ??> pointsSnapshot = new ObservableCollection<ReactionTimePoint>(ReactionPoints)
  ??> Settings.ExperimentContext.ColumnSerie = new ObservableCollection<ISeries>
      ??> new ColumnSeries<ReactionTimePoint>
          ??> Values = pointsSnapshot (copie!)
          ??> DataLabelsFormatter, XToolTipLabelFormatter, YToolTipLabelFormatter
          ??> Mapping
          ??> OnPointCreated (couleurs purple/orange)
```

**Observation critique**: Cette méthode **duplique** la logique de `NewColumnSerie()` mais avec un **snapshot** au lieu du binding live.

---

## 4?? Invariants Implicites Critiques

### Invariants d'Ordre d'Exécution

#### 1. Ordre dans Reset()

**Documenté par**: `ExperimentSettingsTests.Reset_ExecutionOrder_ResetsContextBeforeSettingBlock`

```
ExperimentContext.Reset()  ? Doit être appelé EN PREMIER
Block = 1
ExperimentContext.IsBlockFinished = true
ExperimentContext.IsParticipantSelectionEnabled = true
ExperimentContext.HasUnsavedExports = true
OnPropertyChanged(string.Empty)  ? Doit être appelé EN DERNIER
```

**Raison**: Les tests de caractérisation garantissent cet ordre exact.

#### 2. Ordre dans AddNewSerie()

**Observé dans code**:

```
CurrentBlock = new Block(config);  ? Création AVANT ajout
Blocks.Add(CurrentBlock);          ? Ajout à collection
// Calculs start/end
BlockSeries.Add(...)               ? Série graphique ajoutée
Sections.Add(...)                  ? Section graphique ajoutée
_colorIndex++                      ? Incrémentation
currentBlockStart = end + 1        ? Mise à jour index EN DERNIER
```

**Documenté par**: `SharedExperimentDataAddNewSerieTests.AddNewSerie_CalledTwice_UpdatesCurrentBlockStartForSecondBlock`

**Invariant**: `currentBlockStart` pointe vers le **bloc SUIVANT** après `AddNewSerie()`.

### Invariants d'État

#### 1. CurrentTrial Event Handling

**Code**:
```csharp
set
{
    if (_currentTrial != null)
        _currentTrial.PropertyChanged -= CurrentTrial_PropertyChanged;  // Désabonnement

    _currentTrial = value;
    OnPropertyChanged(nameof(CurrentTrial));

    if (_currentTrial != null)
    {
        _currentTrial.PropertyChanged -= CurrentTrial_PropertyChanged;  // Double désabonnement!
        _currentTrial.PropertyChanged += CurrentTrial_PropertyChanged;  // Abonnement
    }
}
```

**Invariant**: Seul le **dernier** CurrentTrial a son event handler actif. Les anciens trials sont désabonnés.

**Documenté par**: `SharedExperimentDataTests.CurrentTrial_SetTwice_OnlyLastEventHandlerAttached`

#### 2. Palette Cyclique

**Code**: `var color = _palette[_colorIndex % _palette.Length];`

**Invariant**: `_colorIndex` cycle indéfiniment sur 4 couleurs (modulo 4).

**Documenté par**: `SharedExperimentDataAddNewSerieTests.AddNewSerie_MultipleBlocks_CyclesColorIndex`

#### 3. Collections Non Null Après Constructeur

**Invariant**: Après construction, toutes les collections (`Blocks`, `BlockSeries`, `Sections`, `ReactionPoints`, `ColumnSerie`) sont **non-null** (vides mais initialisées).

**Documenté par**: `SharedExperimentDataTests.Constructor_InitializesCollectionsAndDefaults`

### Hypothèses sur l'État

#### 1. CurrentBlock Non Modifié Après Ajout

**Hypothèse**: Une fois `CurrentBlock` créé et ajouté à `Blocks`, il n'est **pas remplacé** pendant l'exécution du bloc. Seuls ses `TrialRecords` sont modifiés.

**Observation**: `AddNewSerie()` **écrase** `CurrentBlock` à chaque appel.

**Documenté par**: `SharedExperimentDataAddNewSerieTests.AddNewSerie_OverwritesCurrentBlock`

#### 2. ReactionPoints Ajoutés Séquentiellement

**Hypothèse**: Les `ReactionTimePoint` sont ajoutés dans l'ordre des trials (1, 2, 3, ...).

**Non documenté par tests**, mais observé dans la logique de `Mapping` qui utilise `TrialNumber-1` comme index X.

#### 3. NewColumnSerie() Appelé Avant Binding

**Hypothèse**: `NewColumnSerie()` doit être appelé **avant** que `ColumnSerie` soit bindée à un graphique.

**Observé**: Constructeur appelle `NewColumnSerie()`, donc satisfait pour graphiques initiaux.

**Problème**: `UpdateBlock()` réaffecte `ColumnSerie` directement, potentiellement après binding.

---

## 5?? Couverture de Tests

### Tests Existants (SharedExperimentDataTests.cs)

| Test | Cible | Catégorie | Coverage |
|------|-------|-----------|----------|
| `Constructor_InitializesCollectionsAndDefaults` | Constructeur | Métier | ? Collections initialisées |
| `CurrentBlock_SetValue_RaisesPropertyChangedAndUpdatesValue` | Setter | Métier | ? PropertyChanged |
| `CurrentTrial_SetValue_RaisesPropertyChangedAndSubscribesEvent` | Setter | Métier | ? Event subscription |
| `CurrentTrial_SetTwice_OnlyLastEventHandlerAttached` | Setter | Métier | ? Désabonnement précédent |
| `IsBlockFinished_SetTrueAndFalse_UpdatesValue` | Setter | État | ? Flag état |
| `IsParticipantSelectionEnabled_DefaultTrue_AndCanSet` | Setter | État | ? Flag état |
| `NextAction_SetValue_UpdatesValue` | Setter | État | ? Enum état |
| `Reset_ClearsAllCollectionsAndResetsState` | Reset() | Métier+Graphique | ? Clear collections, ? Flags, ? NewColumnSerie appelé |

### Tests Existants (SharedExperimentDataAddNewSerieTests.cs)

**Total**: 24 tests

**Catégories couvertes**:
- ? Création de Block
- ? Ajout à collections
- ? Calcul indices (currentBlockStart, currentBlockEnd)
- ? Incrémentation _colorIndex
- ? Propagation propriétés profil au Block
- ? VisualCue selon IsAmorce
- ? Ranges sections graphiques
- ? Collections vides après création Block
- ? Remplacement CurrentBlock
- ? Non-modification ExperimentSettings

**Observations**:
- Tests se concentrent sur **comportement observable** (état des propriétés)
- **Pas de tests** sur les objets LiveCharts créés (LineSeries, RectangularSection)
- **Pas de tests** sur les lambdas (Mapping, formatters)

### Ce Qui N'Est PAS Couvert

#### 1. Logique LiveCharts

| Aspect Non Testé | Raison | Risque |
|------------------|--------|--------|
| `NewColumnSerie()` contenu exact | Création objet graphique | ?? MOYEN - Formatters, lambdas non vérifiés |
| `AddNewSerie()` LineSeries config | Création objet graphique | ?? MOYEN - Stroke, Fill, Mapping non vérifiés |
| `AddNewSerie()` RectangularSection config | Création objet graphique | ?? MOYEN - Xi, Xj, Label non vérifiés |
| Lambdas `DataLabelsFormatter` | Logique inline | ?? BAS - Format d'affichage, pas métier |
| Lambdas `Mapping` | Logique inline | ?? ÉLEVÉ - Transforme données métier ? coordonnées graphiques |
| `.OnPointCreated()` callback | Logique inline | ?? MOYEN - Coloration selon validité |

#### 2. ViewModels Graphiques

| ViewModel | Couverture Actuelle | Risque |
|-----------|---------------------|--------|
| `ColumnGraphViewModel` | ? AUCUN TEST | ?? ÉLEVÉ - Binding direct à ColumnSerie |
| `GlobalGraphViewModel` | ? AUCUN TEST | ?? ÉLEVÉ - Binding direct à BlockSeries/Sections |
| `LiveReactionTimeViewModel` | ? AUCUN TEST | ?? MOYEN - Logique métier (groupes) |
| `EndExperimentPageViewModel` | ? AUCUN TEST | ?? CRITIQUE - Duplication logique UpdateBlock() |

#### 3. Intégration Graphique

| Scénario | Couverture | Risque |
|----------|------------|--------|
| Binding WPF ? LiveCharts | ? NON TESTÉ | ?? MOYEN - Assume que binding fonctionne |
| CollectionChanged propagation | ? NON TESTÉ | ?? MOYEN - Mise à jour UI |
| Performance avec grandes collections | ? NON TESTÉ | ?? BAS - Problème utilisateur, pas métier |

### Ce Qui Doit Être Testé AVANT Phase C

#### Tests Critiques à Ajouter

1. **Test de NewColumnSerie() - Structure**
   ```
   GIVEN SharedExperimentData initialisé
   WHEN NewColumnSerie() est appelé
   THEN ColumnSerie contient 1 ISeries
   AND ISeries est de type ColumnSeries<ReactionTimePoint>
   AND Values pointe vers ReactionPoints
   ```

2. **Test de AddNewSerie() - LineSeries**
   ```
   GIVEN config avec WordCount = 10
   WHEN AddNewSerie(config) est appelé
   THEN BlockSeries contient 1 nouveau ISeries
   AND ISeries est de type LineSeries<double?>
   AND Values pointe vers CurrentBlock.TrialTimes
   ```

3. **Test de AddNewSerie() - RectangularSection**
   ```
   GIVEN currentBlockStart = 1, WordCount = 10
   WHEN AddNewSerie(config) est appelé
   THEN Sections contient 1 nouvelle section
   AND Xi = 1, Xj = 10
   AND Label = "Bloc n°{Block}"
   ```

4. **Test de Mapping Lambda - Coordonnées**
   ```
   GIVEN ReactionPoint avec TrialNumber=5, ReactionTime=500
   WHEN Mapping lambda est appliqué
   THEN Coordinate X = 4 (TrialNumber-1)
   AND Coordinate Y = 500
   ```

5. **Test de Duplication UpdateBlock()**
   ```
   GIVEN ColumnSerie initialisé par NewColumnSerie()
   WHEN UpdateBlock() est appelé
   THEN nouvelle ColumnSerie est créée (pas même instance)
   AND Values pointe vers SNAPSHOT de ReactionPoints (pas live)
   ```

#### Tests Recommandés (Non Bloquants)

6. Test de formatters (DataLabelsFormatter, TooltipFormatter)
7. Test de `.OnPointCreated()` callback (couleurs)
8. Test de `CurrentTrial_PropertyChanged` handler
9. Tests d'intégration ViewModels graphiques

---

## 6?? Problèmes Architecturaux Identifiés

### 1. Mélange Métier/Présentation

**Problème**: `SharedExperimentData` contient à la fois:
- Données métier (`Blocks`, `ReactionPoints`, `CurrentBlock`, `CurrentTrial`)
- Objets graphiques (`BlockSeries`, `ColumnSerie`, `Sections`)
- Logique de création graphique (`NewColumnSerie()`, partie de `AddNewSerie()`)

**Impact**: Violation de SRP (Single Responsibility Principle). Impossible de tester la logique métier sans LiveCharts.

**Documenté par**: Imports `using LiveChartsCore.*` dans un modèle de données.

### 2. Duplication de Logique

**Problème**: `NewColumnSerie()` et `EndExperimentPageViewModel.UpdateBlock()` contiennent la **même logique** de création de `ColumnSeries<ReactionTimePoint>`.

**Différences**:
- `NewColumnSerie()`: Bind `Values` directement à `ReactionPoints` (live)
- `UpdateBlock()`: Crée un snapshot `new ObservableCollection<>(ReactionPoints)` (figé)

**Impact**: Maintenance difficile, risque de désynchronisation, logique métier éparpillée.

### 3. Champs Publics

**Problème**: `currentBlockStart`, `currentBlockEnd`, `_colorIndex` sont des **champs publics** (pas de propriétés).

**Impact**:
- Pas de `PropertyChanged` (mais probablement pas nécessaire ici)
- Accès direct possible depuis n'importe où
- Violation d'encapsulation

**Observé**: `_colorIndex` est même préfixé `_` mais déclaré `public`.

### 4. Responsabilités de AddNewSerie()

**Problème**: `AddNewSerie()` fait 6 choses:
1. Crée un Block (métier)
2. Ajoute à Blocks (métier)
3. Crée une LineSeries (graphique)
4. Ajoute à BlockSeries (graphique)
5. Crée une RectangularSection (graphique)
6. Ajoute à Sections (graphique)
7. Met à jour indices (graphique)
8. Incrémente palette (graphique)

**Impact**: Impossible de créer un Block sans créer ses représentations graphiques.

### 5. Logique Graphique dans Constructeur

**Problème**: Le constructeur appelle `NewColumnSerie()` immédiatement.

**Impact**:
- Même si on n'affiche jamais de graphique, les objets LiveCharts sont créés
- Couplage fort dès l'instanciation
- Tests difficiles (nécessite LiveCharts même pour tester métier)

### 6. État Global Modifiable (_colorIndex, currentBlockStart)

**Problème**: `_colorIndex` et `currentBlockStart` sont des **compteurs globaux** modifiés par `AddNewSerie()`.

**Impact**:
- État implicite
- Difficile de prédire la couleur d'un bloc sans connaître l'historique
- `currentBlockStart` pointe toujours vers le **futur** (bloc suivant)

**Documenté par**: Tests de caractérisation `AddNewSerie_SetsCurrentBlockStartCorrectly` ? valeur = 11 après ajout du premier bloc (pas 1).

---

## 7?? Dépendances Externes

### Packages NuGet

| Package | Namespace | Utilisé Pour |
|---------|-----------|--------------|
| `LiveChartsCore` | `LiveChartsCore` | ISeries, Axis, Coordinate |
| `LiveChartsCore.SkiaSharpView` | `LiveChartsCore.SkiaSharpView` | ColumnSeries, LineSeries, RectangularSection |
| `LiveChartsCore.SkiaSharpView.Painting` | (même) | SolidColorPaint |
| `SkiaSharp` | `SkiaSharp` | SKColor, SKColors |

### Modèles de Données Dépendants

| Modèle | Utilisé Dans | Rôle |
|--------|--------------|------|
| `Block` | `Blocks`, `CurrentBlock` | Représente un bloc d'expérience |
| `ReactionTimePoint` | `ReactionPoints`, `ColumnSerie` | DTO pour graphique colonne |
| `StroopTrial` | `CurrentTrial`, `Block.TrialRecords` | Métier - trial individuel |

### Services Consommateurs

| Service/ViewModel | Dépend De | Type Dépendance |
|-------------------|-----------|-----------------|
| `ColumnGraphViewModel` | `ColumnSerie`, `ReactionPoints` | Binding direct |
| `GlobalGraphViewModel` | `BlockSeries`, `Sections`, `Blocks` | Binding direct + calculs |
| `LiveReactionTimeViewModel` | `ReactionPoints` | Event CollectionChanged |
| `EndExperimentPageViewModel` | Toutes propriétés + méthodes | Lecture + appels méthodes |

---

## 8?? Graphe de Dépendances

### Dépendances Entrantes (Qui Utilise SharedExperimentData)

```
ExperimentSettings
  ??> ExperimentRunState.ExperimentContext : SharedExperimentData

ConfigurationPageViewModel
  ??> settings.ExperimentContext.AddNewSerie()
  ??> settings.ExperimentContext.NewColumnSerie()
  ??> settings.ExperimentContext.IsBlockFinished = false
  ??> settings.ExperimentContext.IsTaskStopped = false

ColumnGraphViewModel
  ??> settings.ExperimentContext.ColumnSerie (bind)
  ??> settings.ExperimentContext.ReactionPoints (event)

GlobalGraphViewModel
  ??> settings.ExperimentContext.BlockSeries (bind)
  ??> settings.ExperimentContext.Sections (bind)
  ??> settings.ExperimentContext.Blocks (calculs)

LiveReactionTimeViewModel
  ??> settings.ExperimentContext.ReactionPoints (bind + event)

EndExperimentPageViewModel
  ??> settings.ExperimentContext.Blocks (bind)
  ??> settings.ExperimentContext.ReactionPoints (lecture + snapshot)
  ??> settings.ExperimentContext.ColumnSerie (réaffectation)
  ??> settings.ExperimentContext.NewColumnSerie() (appel)
```

### Dépendances Sortantes (Ce Dont Dépend SharedExperimentData)

```
SharedExperimentData
  ??> ModelBase (héritage INotifyPropertyChanged)
  ??> ObservableCollection<T> (collections)
  ??> Block (création, stockage)
  ??> ReactionTimePoint (stockage)
  ??> StroopTrial (référence CurrentTrial)
  ??> LiveChartsCore.ISeries (collections graphiques)
  ??> LiveChartsCore.SkiaSharpView.ColumnSeries<T> (création)
  ??> LiveChartsCore.SkiaSharpView.LineSeries<T> (création)
  ??> LiveChartsCore.SkiaSharpView.RectangularSection (création)
  ??> LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint (création)
  ??> LiveChartsCore.Kernel.Coordinate (mapping)
  ??> SkiaSharp.SKColor (palette)
  ??> SkiaSharp.SKColors (constantes)
  ??> IBlockConfiguration (interface Phase A)
  ??> ExperimentSettingsBlockConfigurationAdapter (adaptateur Phase A)
```

---

## 9?? Métriques de Complexité

### Lignes de Code

| Catégorie | Lignes | Pourcentage |
|-----------|--------|-------------|
| Métier (propriétés, constructeur de base) | ~80 | 27% |
| Graphique (NewColumnSerie, partie AddNewSerie) | ~120 | 41% |
| État/Flags (getters/setters état) | ~60 | 20% |
| Event handling (CurrentTrial) | ~15 | 5% |
| Reset() | ~20 | 7% |
| **TOTAL** | **~295** | **100%** |

### Cyclomatic Complexity

| Méthode | Branches | Complexity | Catégorie |
|---------|----------|------------|-----------|
| `NewColumnSerie()` | 5 (lambdas) | Moyenne | Graphique |
| `AddNewSerie(IBlockConfiguration)` | 2 | Faible | Mixte |
| `Reset()` | 0 | Triviale | Mixte |
| `CurrentTrial` setter | 4 | Moyenne | Métier |

### Nombre de Responsabilités

**Responsabilités Identifiées**: 8

1. Stocker blocs métier (`Blocks`, `CurrentBlock`)
2. Stocker trials en cours (`CurrentTrial`)
3. Stocker points de données (`ReactionPoints`)
4. Gérer états runtime (flags)
5. **Créer séries graphiques colonnes** (`NewColumnSerie()`)
6. **Créer séries graphiques lignes** (`AddNewSerie()` partie)
7. **Créer sections graphiques** (`AddNewSerie()` partie)
8. **Gérer palette de couleurs** (`_colorIndex`, `_palette`)

**Violation SRP**: OUI - Responsabilités 5-8 devraient être dans un service graphique séparé.

---

## ?? Recommandations pour Phase C

### Avant de Commencer Phase C

#### 1. Créer Tests Manquants (Bloquant)

**Priorité CRITIQUE**:
- [ ] Test `NewColumnSerie()` - vérifier structure ISeries créée
- [ ] Test `AddNewSerie()` - vérifier LineSeries ajoutée à BlockSeries
- [ ] Test `AddNewSerie()` - vérifier RectangularSection ajoutée à Sections
- [ ] Test duplication `UpdateBlock()` vs `NewColumnSerie()`

**Priorité HAUTE**:
- [ ] Test lambda `Mapping` - vérifier transformation TrialNumber ? Coordinate.X
- [ ] Test lambda `Mapping` - vérifier transformation ReactionTime ? Coordinate.Y (avec null)

**Justification**: Ces tests garantissent que l'extraction n'altère pas la logique graphique observable.

#### 2. Documenter Contrats Implicites

**Action**: Créer un document `LIVECHARTS_CONTRACTS.md` listant:
- Format exact des lambdas `Mapping`
- Format exact des formatters (DataLabels, Tooltip)
- Couleurs exactes (purple #5B2EFF, orange #FFA600)
- Ordre de création (LineSeries avant RectangularSection)

**Justification**: Ces contrats seront les specs du futur `ExperimentChartService`.

### Stratégie d'Extraction Recommandée

#### Option A: Service Graphique Pur (Recommandée)

**Créer**: `IExperimentChartService` avec:
- `CreateColumnSeries(ObservableCollection<ReactionTimePoint>)` ? `ISeries`
- `CreateBlockLineSeries(ObservableCollection<double?>, int start)` ? `ISeries`
- `CreateBlockSection(int start, int end, int blockNumber, int colorIndex)` ? `RectangularSection`
- `GetNextColor(int index)` ? `SKColor`

**Avantages**:
- SharedExperimentData devient POCO pur
- Logique graphique testable indépendamment
- Réutilisable pour d'autres graphiques
- Dépendances LiveCharts isolées

**Inconvénients**:
- Nécessite migration ViewModels
- Plus de code initial

#### Option B: Extraction Partielle (Plus Safe)

**Créer**: `ExperimentChartHelper` statique avec:
- `CreateColumnSeries(...)` ? `ISeries`
- Laisser `AddNewSerie()` appeler le helper

**Avantages**:
- Migration progressive
- Tests plus faciles à écrire
- Moins de changements ViewModels

**Inconvénients**:
- SharedExperimentData reste dépendant LiveCharts
- Classe statique (testabilité réduite)

### Zones de Risque Identifiées

| Zone | Risque | Mitigation |
|------|--------|------------|
| `UpdateBlock()` duplication | ?? ÉLEVÉ | Tests avant/après extraction |
| Lambdas `Mapping` | ?? MOYEN | Tests unitaires spécifiques |
| Order d'appel `AddNewSerie()` | ?? MOYEN | Tests de caractérisation existants OK |
| Binding WPF ? nouvelles propriétés | ?? MOYEN | Tests manuels UI |
| Performance grandes collections | ?? BAS | Profiling si nécessaire |

---

## ?? Résumé Exécutif

### Constats Factuels

1. **SharedExperimentData = 41% logique graphique** (120 lignes sur 295)
2. **8 responsabilités distinctes** ? Violation SRP confirmée
3. **Duplication logique** entre `NewColumnSerie()` et `UpdateBlock()`
4. **Champs publics** (`_colorIndex`, `currentBlockStart`) ? Encapsulation violée
5. **Dépendance LiveCharts dès la construction** ? Couplage fort

### Couverture Tests Actuelle

- ? **Métier**: 100% couvert (24 tests AddNewSerie + 8 tests Reset/Propriétés)
- ? **Graphique**: 0% couvert (aucun test sur objets LiveCharts créés)
- ? **ViewModels**: 0% couvert (aucun test sur consommateurs graphiques)

### Prérequis Phase C

**Bloquants**:
1. Créer 6 tests critiques (structure ISeries, LineSeries, Section, Mapping)
2. Documenter contrats graphiques (couleurs, formats, ordre)

**Recommandés**:
3. Tests ViewModels graphiques (intégration)
4. Tests duplication UpdateBlock()

### Architecture Cible Phase C

```
SharedExperimentData (POCO pur - métier uniquement)
  ??> Blocks
  ??> ReactionPoints
  ??> CurrentBlock
  ??> CurrentTrial
  ??> Flags état

IExperimentChartService (nouveau)
  ??> CreateColumnSeries(ReactionPoints) ? ISeries
  ??> CreateBlockLineSeries(TrialTimes, start) ? ISeries
  ??> CreateBlockSection(start, end, block, colorIndex) ? RectangularSection
  ??> GetNextColor(index) ? SKColor

ViewModels Graphiques
  ??> Injectent IExperimentChartService
      ??> Utilisent SharedExperimentData (données) + Service (visualisation)
```

### Estimation Effort Phase C

| Tâche | Complexité | Durée Estimée |
|-------|------------|---------------|
| Créer tests bloquants | Moyenne | 2-3h |
| Créer IExperimentChartService | Faible | 1h |
| Implémenter service | Moyenne | 3-4h |
| Refactor SharedExperimentData | Faible | 1h |
| Migrer ViewModels | Moyenne | 2-3h |
| Tests intégration | Moyenne | 2h |
| Validation complète (140 tests) | Triviale | 30min |
| **TOTAL** | | **12-15h** |

---

**Rapport généré**: Phase C0 - Analyse Pre-Extraction  
**Status**: ? COMPLET  
**Prochaine étape**: Créer tests bloquants ? Lancer Phase C  
**Risque global Phase C**: ?? MOYEN (avec tests bloquants) / ?? ÉLEVÉ (sans tests)
