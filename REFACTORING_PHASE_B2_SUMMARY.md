# Phase B2 - Élimination de la Double Source de Vérité

## ?? Résultat

- **Branche**: `refactor/experiment-settings-phaseB2`
- **Tests**: 140/140 ? (100% de réussite)
- **Commits**: 1 (atomique)
- **Comportement**: Aucun changement observable
- **Champs legacy supprimés**: 6

## ? Objectif Atteint

### Problème Initial

**Phase B** avait introduit une **double source de vérité**:
- Les contextes (`_configuration`, `_participantContext`, `_runState`) contenaient l'état
- Les champs legacy (`block`, `_participant`, `_currentProfile`, etc.) étaient synchronisés mais **jamais utilisés**

```csharp
// Avant B2: Double écriture inutile
public ExperimentProfile CurrentProfile
{
    get => _configuration.Profile;      // Lecture depuis contexte
    set
    {
        _configuration.Profile = value;  // Écriture dans contexte
        _currentProfile = value;          // ? Synchronisation inutile
        OnPropertyChanged();
    }
}
```

### Solution Phase B2

**Suppression complète des champs legacy** car ils n'étaient jamais lus:

```csharp
// Après B2: Source unique de vérité
public ExperimentProfile CurrentProfile
{
    get => _configuration.Profile;      // Lecture depuis contexte
    set
    {
        _configuration.Profile = value;  // Écriture dans contexte
        OnPropertyChanged();             // ? Notification uniquement
    }
}
```

## ?? Audit des Champs Legacy

### Champs Supprimés (6)

| Champ Legacy | Type | Usage Observé | Justification Suppression |
|--------------|------|---------------|---------------------------|
| `block` | `int` | Synchronisé uniquement | Jamais lu, getter lit `_runState.Block` |
| `_participant` | `Participant` | Synchronisé uniquement | Jamais lu, getter lit `_participantContext.Participant` |
| `_currentProfile` | `ExperimentProfile` | Synchronisé uniquement | Jamais lu, getter lit `_configuration.Profile` |
| `_keyMappings` | `KeyMappings` | Synchronisé uniquement | Jamais lu, getter lit `_configuration.KeyMappings` |
| `_experimentContext` | `SharedExperimentData` | Synchronisé uniquement | Jamais lu, getter lit `_runState.ExperimentContext` |
| `_exportFolderPath` | `string` | Synchronisé uniquement | Jamais lu, getter lit `_configuration.ExportFolderPath` |

### Analyse de l'Impact

**Aucun code externe affecté** car:
- Les propriétés publiques sont inchangées
- Les getters lisaient déjà depuis les contextes
- Les setters écrivaient déjà dans les contextes
- Les champs legacy n'étaient que des "fantômes" synchronisés

## ?? Changements Effectués

### ExperimentSettings.cs

**Avant B2**:
```csharp
public class ExperimentSettings : ModelBase
{
    private readonly ExperimentConfiguration _configuration;
    private readonly ParticipantContext _participantContext;
    private readonly ExperimentRunState _runState;

    // ? Champs legacy inutiles (jamais lus)
    private int block;
    private Participant _participant;
    private ExperimentProfile _currentProfile;
    private KeyMappings _keyMappings;
    private SharedExperimentData _experimentContext;
    private string _exportFolderPath;

    public ExperimentSettings()
    {
        _configuration = new ExperimentConfiguration();
        _participantContext = new ParticipantContext();
        _runState = new ExperimentRunState();

        // ? Synchronisation inutile dans constructeur
        _currentProfile = _configuration.Profile;
        _keyMappings = _configuration.KeyMappings;
        _experimentContext = _runState.ExperimentContext;
        _exportFolderPath = _configuration.ExportFolderPath;
        block = _runState.Block;
        _participant = _participantContext.Participant;
    }
}
```

**Après B2**:
```csharp
public class ExperimentSettings : ModelBase
{
    // ? Contextes: seule source de vérité
    private readonly ExperimentConfiguration _configuration;
    private readonly ParticipantContext _participantContext;
    private readonly ExperimentRunState _runState;

    public ExperimentSettings()
    {
        // ? Initialisation propre sans duplication
        _configuration = new ExperimentConfiguration();
        _participantContext = new ParticipantContext();
        _runState = new ExperimentRunState();
    }
}
```

**Lignes supprimées**: 24  
**Lignes ajoutées**: 6  
**Net**: -18 lignes de code mort

## ?? Bénéfices

### 1. Source Unique de Vérité
- **Avant**: État dupliqué entre contextes et champs legacy
- **Après**: Contextes sont la seule source d'état
- **Avantage**: Élimination de bugs potentiels de désynchronisation

### 2. Clarté du Code
- **Avant**: Confusion sur quelle variable contient la vérité
- **Après**: Délégation explicite vers contextes
- **Avantage**: Compréhension immédiate du flux de données

### 3. Maintenabilité
- **Avant**: Synchronisation à maintenir dans chaque setter + constructeur
- **Après**: Écriture directe dans les contextes
- **Avantage**: Moins de code à maintenir, moins d'erreurs possibles

### 4. Performance (Mineure)
- **Avant**: 6 affectations inutiles dans constructeur + synchronisation dans setters
- **Après**: Aucune opération redondante
- **Avantage**: Légère amélioration (négligeable mais conceptuellement propre)

## ?? Audit des Contextes

Vérification que les contextes respectent les contraintes POCO:

### ExperimentConfiguration ?
```csharp
public class ExperimentConfiguration
{
    public ExperimentProfile Profile { get; set; }        // Propriété auto
    public KeyMappings KeyMappings { get; set; }          // Propriété auto
    public string ExportFolderPath { get; set; }          // Propriété auto
    
    // Pas de INotifyPropertyChanged ?
    // Pas de logique métier ?
    // Pas d'événements ?
}
```

### ParticipantContext ?
```csharp
public class ParticipantContext
{
    public Participant Participant { get; set; }          // Propriété auto
    
    // Pas de INotifyPropertyChanged ?
    // Pas de logique métier ?
    // Pas d'événements ?
}
```

### ExperimentRunState ?
```csharp
public class ExperimentRunState
{
    public int Block { get; set; }                        // Propriété auto
    public SharedExperimentData ExperimentContext { get; set; }  // Propriété auto
    
    // Pas de INotifyPropertyChanged ?
    // Pas de logique métier ?
    // Pas d'événements ?
}
```

**Constat**: Tous les contextes sont déjà des POCO purs avec propriétés automatiques. Aucune refactorisation nécessaire.

## ?? Métriques

| Métrique | Phase B | Phase B2 | Évolution |
|----------|---------|----------|-----------|
| Champs privés dans ExperimentSettings | 9 | 3 | -6 (champs legacy) |
| Lignes dans constructeur | 12 | 4 | -8 lignes |
| Synchronisations redondantes | 12 (6 setter + 6 ctor) | 0 | -12 opérations |
| Source de vérité | Double | Unique | ? Simplifié |
| Tests passants | 140 | 140 | ? Stable |

## ?? Historique Git

```bash
git --no-pager log --oneline -n 5
```

```
0cb437e (HEAD -> refactor/experiment-settings-phaseB2) refactor: remove legacy fields - contexts are now single source of truth
d2d3653 (refactor/experiment-settings-phaseB) docs: add Phase B refactoring summary
f4a40fb refactor: delegate ExperimentSettings properties to internal context objects
0114f06 feat: add ExperimentRunState context
4ada423 feat: add ParticipantContext
```

## ? Validation

### Tests de Caractérisation
```
Récapitulatif du test : total : 140; échec : 0; réussi : 140; ignoré : 0
```

**Résultat**: ? 100% de réussite

### API Publique
- ? Aucune signature modifiée
- ? Aucune propriété renommée
- ? Aucun comportement observable changé

### Comportement Observable
- ? PropertyChanged fonctionne identiquement
- ? Reset() préserve l'ordre exact d'exécution
- ? Initialisation produit le même état

## ?? Impact pour les Phases Futures

### Phase C - Extraction Logique Graphique
**Facilité par B2**: Les contextes purs facilitent l'extraction de services graphiques sans risque de dépendances cachées vers des champs legacy.

### Phase D - Dependency Injection
**Facilité par B2**: Les contextes peuvent maintenant être injectés directement sans crainte de désynchronisation avec des champs fantômes.

### Phase E - Migration Complète
**Facilité par B2**: Le code client peut maintenant migrer vers les interfaces (Phase A) sans se soucier de champs legacy qui n'existent plus.

## ?? Leçons Apprises

### Pourquoi les Champs Legacy Étaient-ils Là?

**Intention initiale (Phase B)**: Préserver la compatibilité binaire totale et éviter tout risque.

**Réalité observée**: Les champs n'étaient jamais lus, seulement écrits. La synchronisation était une **sécurité inutile**.

### Audit Systématique

L'audit a révélé que:
1. Tous les **getters** lisaient depuis les contextes
2. Tous les **setters** écrivaient dans les contextes
3. Les champs legacy étaient **write-only** (jamais lus)
4. La suppression était donc **sans risque**

### Tests de Caractérisation = Filet de Sécurité

Sans les 140 tests, cette suppression aurait été **risquée**. Avec eux, elle est **triviale et prouvée**.

## ?? Notes Techniques

### PropertyChanged Reste sur ExperimentSettings

`OnPropertyChanged()` continue d'être appelé sur `ExperimentSettings` (qui hérite de `ModelBase`). Les contextes restent des **POCO purs** sans événements.

**Raison**: Préserver le contrat existant et éviter la propagation d'événements entre couches.

### Readonly sur les Contextes

Les contextes restent `readonly` et créés une seule fois dans le constructeur. Cela garantit la stabilité de la composition.

---

**Status**: ? PHASE B2 COMPLÉTÉE  
**Sécurité**: Garantie par 140 tests de caractérisation  
**Prêt pour**: Phase C - Extraction logique graphique  
**Risque**: Zéro (tests verts, API inchangée, comportement préservé)  
**Impact utilisateur**: Aucun (changements internes uniquement)
