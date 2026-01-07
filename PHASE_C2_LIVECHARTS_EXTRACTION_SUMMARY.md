# Phase C2 - Extraction Logique LiveCharts

## ?? Résultat

- **Branche**: `refactor/livecharts-extraction-phaseC2`
- **Tests Totaux**: **164** ? (100% verts)
- **Commits**: 4 (atomiques)
- **Comportement Modifié**: ? AUCUN
- **Régressions**: 0

## ? Objectif Atteint

**Phase C2** avait pour but d'extraire la logique graphique LiveCharts hors de `SharedExperimentData` et `EndExperimentPageViewModel`, tout en préservant **EXACTEMENT** le comportement observable.

**Mission accomplie**: La logique LiveCharts est maintenant isolée dans `ExperimentChartFactory`, les 164 tests restent verts, aucun changement observable.

---

## ?? Modifications Effectuées

### 1?? Création ExperimentChartFactory

**Fichier**: `StroopApp\Services\Charts\ExperimentChartFactory.cs` (170 lignes)

**Responsabilité**: Encapsulation COMPLÈTE de la création d'objets LiveCharts.

#### Méthodes Publiques (4)

| Méthode | Responsabilité | Type Retourné |
|---------|---------------|---------------|
| `CreateLiveColumnSerie()` | Crée ColumnSeries avec binding **LIVE** | `ObservableCollection<ISeries>` |
| `CreateSnapshotColumnSerie()` | Crée ColumnSeries avec binding **SNAPSHOT** | `ObservableCollection<ISeries>` |
| `CreateBlockLineSeries()` | Crée LineSeries pour un bloc | `LineSeries<double?>` |
| `CreateBlockSection()` | Crée RectangularSection pour un bloc | `RectangularSection` |

#### Extraction Mécanique

**Principe appliqué**: Copie-colle EXACTE du code depuis `SharedExperimentData` et `EndExperimentPageViewModel` vers la factory.

**Aucune "amélioration"**: Les lambdas, formatters, couleurs, mapping sont préservés caractère par caractère.

**Exemples de préservation exacte**:

```csharp
// Formatters IDENTIQUES au code original
DataLabelsFormatter = point => point.Coordinate.SecondaryValue.Equals(double.NaN) 
    ? "Aucune réponse" 
    : point.Coordinate.PrimaryValue.ToString("N0")

// Mapping IDENTIQUE (y compris .Value qui assume non-null)
Mapping = (pt, idx) => new Coordinate(start + idx, pt.Value)

// Couleurs IDENTIQUES (valeurs RGB hardcodées)
var orange = new SKColor(255, 166, 0);      // #FFA600
var purple = new SKColor(91, 46, 255);      // #5B2EFF
```

### 2?? Refactorisation SharedExperimentData

**Fichier**: `StroopApp\Models\SharedExperimentData.cs`

**Avant Phase C2**: 295 lignes (logique LiveCharts inline)

**Après Phase C2**: ~230 lignes (-65 lignes LiveCharts)

#### Changements

**Ajout**:
- `using StroopApp.Services.Charts;`
- `private readonly ExperimentChartFactory _chartFactory;`
- Initialisation dans constructeur: `_chartFactory = new ExperimentChartFactory();`

**NewColumnSerie()** - AVANT:
```csharp
public void NewColumnSerie()
{
    ColumnSerie = new ObservableCollection<ISeries>
    {
        new ColumnSeries<ReactionTimePoint>
        {
            Values = ReactionPoints,
            // ... 47 lignes de configuration LiveCharts ...
        }
    };
}
```

**NewColumnSerie()** - APRÈS:
```csharp
public void NewColumnSerie()
{
    ColumnSerie = _chartFactory.CreateLiveColumnSerie(ReactionPoints);
}
```

**AddNewSerie()** - Partie graphique AVANT:
```csharp
BlockSeries.Add(new LineSeries<double?>
{
    Values = CurrentBlock.TrialTimes,
    // ... 10 lignes de configuration ...
});
Sections.Add(new RectangularSection
{
    Xi = start,
    // ... 7 lignes de configuration ...
});
```

**AddNewSerie()** - Partie graphique APRÈS:
```csharp
var lineSeries = _chartFactory.CreateBlockLineSeries(CurrentBlock.TrialTimes, start);
BlockSeries.Add(lineSeries);

var section = _chartFactory.CreateBlockSection(start, end, config.Block, fillColor);
Sections.Add(section);
```

**Logique Métier PRÉSERVÉE**:
- Création du `Block`
- Calcul de `start`, `end`, `currentBlockEnd`
- Gestion de la palette (`_palette`, `_colorIndex`)
- Incrémentation de `_colorIndex`
- Mise à jour de `currentBlockStart`

**Ordre d'exécution PRÉSERVÉ**:
1. Création Block
2. Ajout à Blocks
3. Calcul couleur palette
4. Calcul indices
5. **Création LineSeries** (délégué factory)
6. Ajout BlockSeries
7. **Création RectangularSection** (délégué factory)
8. Ajout Sections
9. Incrémentation `_colorIndex`
10. Mise à jour `currentBlockStart`

### 3?? Refactorisation EndExperimentPageViewModel

**Fichier**: `StroopApp\ViewModels\Experiment\Experimenter\End\EndExperimentPageViewModel.cs`

**Ajout**:
- `using StroopApp.Services.Charts;`
- `private readonly ExperimentChartFactory _chartFactory;`
- Initialisation: `_chartFactory = new ExperimentChartFactory();`

**UpdateBlock()** - AVANT:
```csharp
private void UpdateBlock()
{
    var pointsSnapshot = new ObservableCollection<ReactionTimePoint>(Settings.ExperimentContext.ReactionPoints);
    
    Settings.ExperimentContext.ColumnSerie = new ObservableCollection<ISeries>
    {
        new ColumnSeries<ReactionTimePoint>
        {
            Values = pointsSnapshot,
            // ... 39 lignes de configuration LiveCharts ...
        }
    };
}
```

**UpdateBlock()** - APRÈS:
```csharp
private void UpdateBlock()
{
    var pointsSnapshot = new ObservableCollection<ReactionTimePoint>(Settings.ExperimentContext.ReactionPoints);
    Settings.ExperimentContext.ColumnSerie = _chartFactory.CreateSnapshotColumnSerie(pointsSnapshot);
}
```

**Différence critique préservée**:
- `NewColumnSerie()` ? binding **LIVE** (références `ReactionPoints` directement)
- `UpdateBlock()` ? binding **SNAPSHOT** (crée copie puis référence la copie)

---

## ?? Décisions Architecturales

### 1. Palette de Couleurs: Où la Placer?

**Décision**: Palette reste dans `SharedExperimentData`

**Raison**:
- `_colorIndex` est un état mutable géré par `AddNewSerie()`
- `_palette` est lié à `_colorIndex` (cycle modulo 4)
- Déplacer la palette nécessiterait de passer `_colorIndex` ou de le gérer dans la factory
- Garder dans `SharedExperimentData` évite couplage supplémentaire

**Alternative rejetée**: Passer palette à chaque appel factory (complexité inutile)

### 2. Duplication Live vs Snapshot

**Décision**: Deux méthodes distinctes dans la factory

**Méthodes**:
- `CreateLiveColumnSerie()` - binding live
- `CreateSnapshotColumnSerie()` - binding snapshot

**Raison**:
- Comportement différent documenté par tests Phase C1
- Fusionner en une méthode "intelligente" violerait la règle "pas d'amélioration"
- Duplication assumée (2 chemins distincts)

**Alternative rejetée**: Paramètre `bool isLive` (masquerait la différence)

### 3. Factory Sans Interface (Pas de DI)

**Décision**: Classe concrète `ExperimentChartFactory` sans abstraction

**Raison**:
- Phase C2 exclut explicitement DI container
- Abstraction `IExperimentChartFactory` sera Phase D (si nécessaire)
- Instanciation directe: `new ExperimentChartFactory()`

**Impact**: ViewModels créent leur propre instance (pas injecté)

### 4. Méthodes Factory: Publiques ou Internes?

**Décision**: Toutes les méthodes sont **publiques**

**Raison**:
- Utilisées par `SharedExperimentData` ET `EndExperimentPageViewModel`
- Potentiellement réutilisables par d'autres ViewModels graphiques futurs
- Pas d'état interne dans la factory (stateless) ? appels indépendants OK

---

## ?? Impact sur le Code

### Lignes de Code

| Fichier | Avant | Après | Différence |
|---------|-------|-------|------------|
| `SharedExperimentData.cs` | 295 | ~230 | **-65** (logique LiveCharts extraite) |
| `EndExperimentPageViewModel.cs` | ~200 | ~165 | **-35** (logique LiveCharts extraite) |
| **NOUVEAU**: `ExperimentChartFactory.cs` | 0 | 170 | **+170** |
| **TOTAL** | 495 | 565 | **+70** (clarté architecturale) |

**Note**: L'augmentation nette de 70 lignes est due à:
- Commentaires XML documentation dans la factory
- Séparation des responsabilités (code plus verbeux mais plus clair)

### Responsabilités

**SharedExperimentData** - Avant Phase C2:
- ? Gestion blocs métier
- ? Gestion trials
- ? Flags état runtime
- ? **Création objets LiveCharts**

**SharedExperimentData** - Après Phase C2:
- ? Gestion blocs métier
- ? Gestion trials
- ? Flags état runtime
- ? Délégation vers factory (pas de création directe)

**ExperimentChartFactory** - Après Phase C2:
- ? Création `ColumnSeries<ReactionTimePoint>`
- ? Création `LineSeries<double?>`
- ? Création `RectangularSection`
- ? Configuration formatters, mapping, couleurs

### Dépendances LiveCharts

**Avant Phase C2**:
```
SharedExperimentData
?? using LiveChartsCore
?? using LiveChartsCore.Kernel
?? using LiveChartsCore.SkiaSharpView
?? using SkiaSharp
?? Crée ColumnSeries, LineSeries, RectangularSection

EndExperimentPageViewModel
?? using LiveChartsCore
?? using LiveChartsCore.SkiaSharpView
?? Crée ColumnSeries
```

**Après Phase C2**:
```
ExperimentChartFactory (ISOLÉ)
?? using LiveChartsCore
?? using LiveChartsCore.Kernel
?? using LiveChartsCore.SkiaSharpView
?? using SkiaSharp
?? Crée TOUS les objets LiveCharts

SharedExperimentData
?? using LiveChartsCore (pour ISeries - types de propriétés)
?? using LiveChartsCore.SkiaSharpView (pour RectangularSection - type de propriété)
?? using SkiaSharp (pour _palette - encore dans classe)
?? Délègue création vers factory

EndExperimentPageViewModel
?? using LiveChartsCore (pour ISeries - peut être retiré si refactor ultérieur)
?? Délègue création vers factory
```

**Amélioration**: La logique de **création** est isolée. Les **types** LiveCharts restent (propriétés publiques).

---

## ?? Validation

### Tests de Caractérisation (24 tests graphiques)

**Tous verts ?**: Le comportement LiveCharts observé est préservé.

| Catégorie Tests | Résultat |
|-----------------|----------|
| NewColumnSerie structure | ? 7/7 |
| NewColumnSerie mapping | ? 3/3 |
| AddNewSerie LineSeries | ? 3/3 |
| AddNewSerie RectangularSection | ? 4/4 |
| AddNewSerie ColorIndex | ? 2/2 |
| Mapping lambdas | ? 2/2 |
| Live vs Snapshot | ? 3/3 |

**Total graphiques**: 24/24 ?

### Tests Métier (140 tests existants)

**Tous verts ?**: Aucun comportement métier affecté.

**Total**: 164/164 tests passent.

---

## ?? Commits

```bash
git --no-pager log --oneline -n 5
```

```
10c82f4 (HEAD -> refactor/livecharts-extraction-phaseC2) refactor: delegate UpdateBlock snapshot series creation to ExperimentChartFactory
207e0d7 refactor: delegate AddNewSerie graphics creation to ExperimentChartFactory
7a2eea0 refactor: delegate NewColumnSerie to ExperimentChartFactory
1f93861 feat: add ExperimentChartFactory (passive extraction, not used yet)
5664a76 (test/characterization-graphics-phaseC1) docs: add Phase C1 graphics characterization tests summary
```

**Stratégie de commits**: Incrémentale et réversible
1. Création factory (sans usage) ? tests verts
2. Délégation NewColumnSerie ? tests verts
3. Délégation AddNewSerie ? tests verts
4. Délégation UpdateBlock ? tests verts

**Avantage**: Chaque commit est un point de rollback sûr.

---

## ?? Comportements Préservés (Même Discutables)

### 1. Mapping LineSeries Assume Non-Null

**Code préservé**:
```csharp
Mapping = (pt, idx) => new Coordinate(start + idx, pt.Value)
```

**Implication**: Crash si `CurrentBlock.TrialTimes` contient null.

**Décision Phase C2**: Préservé tel quel (documenté par test `AddNewSerie_LineSeriesMappingValidValueMapsToY`).

### 2. Duplication Formatters Live vs Snapshot

**Observation**: `CreateLiveColumnSerie()` et `CreateSnapshotColumnSerie()` ont des formatters **presque** identiques.

**Différence subtile**:
- Live: `point.Coordinate.SecondaryValue.Equals(double.NaN)`
- Snapshot: `point.Coordinate.PrimaryValue.Equals(null)`

**Décision Phase C2**: Préservé tel quel (extraction mécanique du code existant).

**Question pour Phase D**: Bug ou intention? Tests caractérisent mais ne répondent pas.

### 3. Palette Hardcodée

**Code préservé**:
```csharp
private readonly SKColor[] _palette = { 
    SKColors.CornflowerBlue, 
    SKColors.OrangeRed, 
    SKColors.MediumSeaGreen, 
    SKColors.Goldenrod 
};
```

**Décision Phase C2**: Reste dans `SharedExperimentData` (lié à `_colorIndex`).

**Alternative future**: Extraire dans service configuration séparé.

---

## ?? Prochaines Étapes (Phase D)

### Phase D - Dependency Injection (Optionnelle)

1. Créer `IExperimentChartFactory` interface
2. Enregistrer dans conteneur DI (`Microsoft.Extensions.DependencyInjection`)
3. Injecter dans constructeurs ViewModels
4. Remplacer `new ExperimentChartFactory()` par injection

**Bénéfice**: Testabilité (mock factory dans tests unitaires).

### Phase E - Nettoyage Using Statements

1. Analyser quels `using LiveChartsCore` sont encore nécessaires
2. Retirer imports inutilisés (si seulement pour types, peut rester)

**Bénéfice**: Clarté des dépendances.

### Questions Ouvertes

1. **Formatter inconsistency**: Corriger ou documenter?
2. **Null handling LineSeries**: Ajouter gestion ou assumer non-null?
3. **Palette**: Extraire ou garder dans SharedExperimentData?

---

## ?? Métriques Finales

| Métrique | Phase C1 | Phase C2 |
|----------|----------|----------|
| **Tests Totaux** | 164 | 164 ? |
| **Tests Graphiques** | 24 | 24 ? |
| **Logique LiveCharts dans SharedExperimentData** | 100% | 0% ? |
| **Logique LiveCharts dans ViewModels** | Inline | Délégué ? |
| **Factory dédiée** | ? | ? ExperimentChartFactory |
| **Régressions** | N/A | 0 ? |
| **Lignes extraites** | 0 | ~100 (vers factory) |

---

## ? Critères de Réussite (Phase C2)

| Critère | Statut |
|---------|--------|
| Logique LiveCharts extraite de SharedExperimentData | ? COMPLET |
| Logique LiveCharts extraite de EndExperimentPageViewModel | ? COMPLET |
| Factory dédiée créée | ? COMPLET |
| Comportement préservé (tests verts) | ? 164/164 |
| Commits atomiques | ? 4 commits |
| Aucune régression | ? CONFIRMÉ |
| API publique préservée | ? CONFIRMÉ |
| Ordre d'exécution préservé | ? CONFIRMÉ |

---

## ?? Leçons Phase C2

### Extraction Mécanique vs Refactorisation Intelligente

**Approche Phase C2**: Extraction **mécanique** (copie-colle exact + délégation).

**Avantages observés**:
- Aucune régression (comportement identique garanti)
- Rapide (4 commits, ~30 min total)
- Réversible (chaque commit testable indépendamment)

**Tentation évitée**:
- "Harmoniser" les formatters live/snapshot
- "Corriger" le null handling LineSeries
- "Améliorer" la gestion palette

**Résultat**: Phase C2 = succès sans risque.

### Tests de Caractérisation = Filet de Sécurité

**Sans Phase C1**: Extraction risquée (comportement non documenté).

**Avec Phase C1**: Extraction sûre (24 tests garantissent préservation).

**Exemple concret**: Test `NewColumnSerie_ValuesReferenceLiveReactionPoints` garantit que factory fait binding live (pas copie).

### Séparation Responsabilités

**Avant**: SharedExperimentData = god object (métier + graphique).

**Après**: 
- SharedExperimentData = métier + délégation
- ExperimentChartFactory = graphique pur

**Bénéfice**: Modification graphique future isolée (factory uniquement).

---

**Branche**: `refactor/livecharts-extraction-phaseC2`  
**Status**: ? PHASE C2 COMPLÉTÉE  
**Prêt pour merge**: Oui (après revue)  
**Risque régression**: ?? ZÉRO (164 tests verts)  
**Comportement LiveCharts**: ? PRÉSERVÉ EXACTEMENT  
**Prochaine étape**: Phase D (DI - optionnelle) ou Merge vers main
