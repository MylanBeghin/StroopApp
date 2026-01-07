# Phase B de Refactorisation - Résumé

## ?? Résultats

- **Branche**: `refactor/experiment-settings-phaseB`
- **Tests**: 140/140 ? (100% de réussite)
- **Commits**: 4 (atomiques)
- **Comportement**: Aucun changement observable
- **API Publique**: Inchangée (rétrocompatibilité totale)

## ? Objectifs Atteints

### Transformation de ExperimentSettings

**Avant Phase B**: God object monolithique avec 6 responsabilités mélangées

**Après Phase B**: Agrégateur délégant à 3 contextes spécialisés

### 1?? Contextes Créés

#### ExperimentConfiguration
**Fichier**: `StroopApp\Models\ExperimentConfiguration.cs`

**Responsabilité**: Configuration statique de l'expérience

**Propriétés**:
- `ExperimentProfile Profile`
- `KeyMappings KeyMappings`
- `string ExportFolderPath`

**Caractéristiques**:
- POCO sans logique
- Initialisé avec valeurs par défaut dans le constructeur

#### ParticipantContext
**Fichier**: `StroopApp\Models\ParticipantContext.cs`

**Responsabilité**: Identité du participant

**Propriétés**:
- `Participant Participant`

**Caractéristiques**:
- POCO ultra-simple
- Participant initialisé à null

#### ExperimentRunState
**Fichier**: `StroopApp\Models\ExperimentRunState.cs`

**Responsabilité**: État d'exécution runtime

**Propriétés**:
- `int Block`
- `SharedExperimentData ExperimentContext`

**Caractéristiques**:
- POCO sans logique
- Block initialisé à 1
- ExperimentContext créé dans constructeur

### 2?? Refactorisation de ExperimentSettings

**Fichier**: `StroopApp\Models\ExperimentSettings.cs`

#### Structure Interne (Nouvelle)

```csharp
public class ExperimentSettings : ModelBase
{
    // Internal context objects (composition)
    private readonly ExperimentConfiguration _configuration;
    private readonly ParticipantContext _participantContext;
    private readonly ExperimentRunState _runState;

    // Legacy fields for backward compatibility
    private int block;
    private Participant _participant;
    private ExperimentProfile _currentProfile;
    private KeyMappings _keyMappings;
    private SharedExperimentData _experimentContext;
    private string _exportFolderPath;
}
```

#### Délégation des Propriétés

Chaque propriété publique délègue maintenant vers le contexte approprié:

| Propriété Publique | Délègue vers | Contexte |
|-------------------|--------------|----------|
| `Block` | `_runState.Block` | ExperimentRunState |
| `Participant` | `_participantContext.Participant` | ParticipantContext |
| `CurrentProfile` | `_configuration.Profile` | ExperimentConfiguration |
| `KeyMappings` | `_configuration.KeyMappings` | ExperimentConfiguration |
| `ExperimentContext` | `_runState.ExperimentContext` | ExperimentRunState |
| `ExportFolderPath` | `_configuration.ExportFolderPath` | ExperimentConfiguration |

#### Préservation de la Compatibilité

**Champs legacy conservés** pour synchronisation:
- Évite toute cassure de code existant
- PropertyChanged continue de fonctionner exactement comme avant
- Chaque setter met à jour à la fois le contexte ET le champ legacy

**Exemple de délégation**:
```csharp
public ExperimentProfile CurrentProfile
{
    get => _configuration.Profile;
    set
    {
        _configuration.Profile = value;
        _currentProfile = value; // Keep legacy field in sync
        OnPropertyChanged();
    }
}
```

#### Méthode Reset()

**Préservation exacte**:
- Ordre d'exécution identique aux tests de caractérisation
- Effets de bord préservés
- PropertyChanged avec string.Empty maintenu

```csharp
public void Reset()
{
    // Preserve exact order of execution from characterization tests
    ExperimentContext.Reset();
    Block = 1;
    ExperimentContext.IsBlockFinished = true;
    ExperimentContext.IsParticipantSelectionEnabled = true;
    ExperimentContext.HasUnsavedExports = true;
    OnPropertyChanged(string.Empty);
}
```

## ?? Impact de la Refactorisation

### Séparation des Responsabilités

**Avant**:
```
ExperimentSettings (God Object)
??? Block
??? Participant
??? CurrentProfile
??? KeyMappings
??? ExperimentContext
??? ExportFolderPath
```

**Après**:
```
ExperimentSettings (Aggregator)
??? _configuration (ExperimentConfiguration)
?   ??? Profile
?   ??? KeyMappings
?   ??? ExportFolderPath
??? _participantContext (ParticipantContext)
?   ??? Participant
??? _runState (ExperimentRunState)
    ??? Block
    ??? ExperimentContext
```

### Bénéfices Immédiats

1. **Clarté architecturale**: Les responsabilités sont explicites
2. **Testabilité future**: Chaque contexte peut être testé indépendamment
3. **Maintenance facilitée**: Changements isolés par contexte
4. **Évolutivité**: Nouveaux contextes peuvent être ajoutés facilement
5. **API préservée**: Code client non affecté

### Limitations Conservées (Volontaires)

- **Champs legacy**: Conservés pour compatibilité binaire totale
- **PropertyChanged**: Reste sur ExperimentSettings (pas déplacé vers contextes)
- **Reset()**: Logique non extraite (sera Phase C si nécessaire)

## ?? Points Clés

### ? Respecté

- **API publique inchangée**: Toutes les propriétés publiques identiques
- **Comportement identique**: 140 tests de caractérisation passent
- **Commits atomiques**: 4 commits (3 créations + 1 refactoring)
- **Tests verts à chaque étape**: Validation continue
- **Pas de logique dans les contextes**: POCO purs

### ?? Choix de Design

#### Pourquoi des champs legacy?

Pour garantir une compatibilité totale sans risque de régression. Les champs peuvent être supprimés dans une phase ultérieure si nécessaire.

#### Pourquoi readonly sur les contextes?

Les contextes sont créés une fois dans le constructeur et ne doivent jamais être remplacés. Cela garantit la stabilité de la composition.

#### Pourquoi pas d'INotifyPropertyChanged sur les contextes?

Les contextes sont des DTOs internes. ExperimentSettings reste la façade avec notification. Cela évite la complexité et préserve le contrat existant.

## ?? Historique Git

```bash
f4a40fb refactor: delegate ExperimentSettings properties to internal context objects
0114f06 feat: add ExperimentRunState context
4ada423 feat: add ParticipantContext
e8b6325 feat: add ExperimentConfiguration context
```

## ?? Prochaines Étapes (Phase C)

### Extraction de la Logique Graphique

1. Créer `ExperimentChartService`
2. Extraire `NewColumnSerie()` de `SharedExperimentData`
3. Extraire logique graphique de `AddNewSerie()`
4. `SharedExperimentData` devient DTO pur
5. Injecter service dans ViewModels graphiques

### Phase D (DI Réelle)

1. Introduire conteneur IoC (Microsoft.Extensions.DependencyInjection)
2. Enregistrer services et contextes
3. Composer dans `App.OnStartup()`
4. Supprimer composition manuelle
5. Supprimer champs legacy si approprié

### Phase E (Suppression Progressive)

1. Migrer ViewModels vers interfaces (Phase A)
2. Remplacer usages directs de `ExperimentSettings`
3. Supprimer champs legacy une fois code client migré
4. Potentiellement faire de `ExperimentSettings` un simple facade/builder

## ?? Métriques

| Métrique | Avant | Après |
|----------|-------|-------|
| Responsabilités directes | 6 | 0 (délégation) |
| Contextes internes | 0 | 3 |
| Champs privés | 6 | 12 (6 legacy + 6 nouveaux) |
| API publique | 6 props + Reset() | Inchangée |
| Tests passants | 140 | 140 ? |
| Lignes de code | ~70 | ~120 (+50 pour clarté) |

## ?? Notes Techniques

### Synchronisation Legacy

La synchronisation bidirectionnelle entre contextes et champs legacy est temporaire. Elle garantit:
- Aucune cassure de références existantes
- Comportement PropertyChanged identique
- Migration future possible sans Big Bang

### Ordre d'Initialisation

Le constructeur initialise dans l'ordre:
1. Contextes internes (nouvelles instances)
2. Champs legacy (copie depuis contextes)

Cela garantit la cohérence dès la construction.

---

**Status**: ? PHASE B COMPLÉTÉE  
**Sécurité**: Garantie par 140 tests de caractérisation  
**Prêt pour**: Phase C - Extraction logique graphique  
**Risque de régression**: Minimal (API préservée, tests verts)
