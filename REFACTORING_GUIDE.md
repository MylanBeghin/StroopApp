# Tests de Caractérisation - Guide de Refactorisation

## ?? État Actuel

? **Phase 1 COMPLÉTÉE** - 140 tests de caractérisation couvrant les zones critiques

## ?? Zones Sécurisées par les Tests

### ? Complètement Couvert
- `ExperimentSettings.Reset()` - 13 tests
- `SharedExperimentData.AddNewSerie()` - 24 tests
- `TrialGenerationService.GenerateTrials()` - 24 tests
- `ConfigurationPageViewModel.LaunchExperimentAsync()` - 12 tests

### ?? Partiellement Couvert (tests existants)
- `ExperimentProfile` - Tests de base
- `Participant` - Tests de base
- `SharedExperimentData` - Tests Reset() et propriétés
- `Block` - Tests de base
- `Result` - Tests de base
- `ExportationService` - Tests export Excel

### ? Non Couvert (Zone Rouge - NE PAS REFACTORISER)
- `StroopViewModel.StartTrials()` - Logique async complexe
- `ExperimentDashBoardPageViewModel` - Arrêt tâche
- `EndExperimentPageViewModel` - Workflow fin d'expérience
- `WindowManager` - Singleton window
- Navigation workflows complets
- `ProfileService` / `ParticipantService` - Persistance

## ?? Workflow de Refactorisation Sécurisée

### Étape 1: Choisir une Cible
Commencer par les zones **? Complètement Couvert**

### Étape 2: Vérifier les Tests
```bash
dotnet test --filter "FullyQualifiedName~<NomDeLaZone>"
```

### Étape 3: Refactoriser
Faire les changements en suivant le cycle **Rouge-Vert-Refactor**

### Étape 4: Valider
```bash
dotnet test
```
Tous les tests doivent rester verts ?

### Étape 5: Commit Atomique
```bash
git add .
git commit -m "refactor: <description>"
```

## ?? Plan de Refactorisation Recommandé

### Phase A: Extraction d'Interfaces (Sécurisé ?)
1. **ITrialConfiguration** - Extraire de `ExperimentSettings`
   ```csharp
   public interface ITrialConfiguration
   {
       int Block { get; }
       string ParticipantId { get; }
       int WordCount { get; }
       int CongruencePercent { get; }
       bool IsAmorce { get; }
       int DominantPercent { get; }
   }
   ```

2. **IBlockConfiguration** - Extraire de `ExperimentSettings`
   ```csharp
   public interface IBlockConfiguration
   {
       int Block { get; }
       string ProfileName { get; }
       int WordCount { get; }
       bool IsAmorce { get; }
   }
   ```

3. Modifier `TrialGenerationService.GenerateTrials()` pour accepter `ITrialConfiguration`
4. Modifier `SharedExperimentData.AddNewSerie()` pour accepter `IBlockConfiguration`

**Validation**: Les tests de caractérisation doivent tous rester verts

### Phase B: Décomposition ExperimentSettings (Sécurisé ?)
1. **ExperimentConfiguration** - Config statique
   ```csharp
   public class ExperimentConfiguration
   {
       public ExperimentProfile Profile { get; set; }
       public KeyMappings KeyMappings { get; set; }
       public string ExportFolderPath { get; set; }
   }
   ```

2. **ExperimentRunState** - État runtime
   ```csharp
   public class ExperimentRunState
   {
       public int CurrentBlock { get; set; }
       public SharedExperimentData Context { get; set; }
   }
   ```

3. **ParticipantContext** - Identité
   ```csharp
   public class ParticipantContext
   {
       public Participant Participant { get; set; }
   }
   ```

4. Adapter `ExperimentSettings` pour agréger ces objets
5. Propager les changements aux consommateurs

**Validation**: Tests de Reset() doivent passer

### Phase C: Extraction Logique Graphique (Risque Moyen ??)
1. Créer `ExperimentChartService`
2. Déplacer `NewColumnSerie()`, `AddNewSerie()` logique graphique
3. `SharedExperimentData` ne contient que DTOs
4. Injecter service dans ViewModels graphiques

**Attention**: Nécessite tests supplémentaires pour les ViewModels graphiques

### Phase D: Machine d'État Explicite (Risque Élevé ?)
**ARRÊT** - Zone non couverte par tests

Nécessite Phase 2 de tests de caractérisation d'abord:
- Tester `StroopViewModel` états
- Tester `ExperimentDashBoardPageViewModel` transitions
- Tester `EndExperimentPageViewModel` actions

## ??? Règles de Sécurité

### ? AUTORISÉ
- Extraire interfaces
- Renommer variables locales
- Extraire méthodes privées
- Ajouter tests supplémentaires
- Documenter comportements

### ?? AVEC PRÉCAUTION
- Modifier signatures publiques (vérifier tous les usages)
- Déplacer code entre classes (vérifier dépendances)
- Changer types de retour (adapter appelants)

### ? INTERDIT
- Refactoriser zones non testées
- Changer comportement observable
- Supprimer tests existants
- Merge sans validation CI

## ?? En Cas de Doute

### Test Échoue Après Refactoring
1. **Annuler** le refactoring: `git checkout -- .`
2. Relire le test: que documente-t-il?
3. Le comportement observé doit-il changer?
   - **Non** ? Corriger le refactoring
   - **Oui** ? Adapter le test ET documenter la raison

### Comportement Inattendu
1. Écrire un nouveau test qui capture le comportement actuel
2. Valider que le test passe
3. Refactoriser
4. Le test doit rester vert

### Couverture Insuffisante
1. Arrêter le refactoring
2. Écrire tests de caractérisation manquants
3. Valider qu'ils passent tous
4. Reprendre le refactoring

## ?? Références

- **Livre**: *Working Effectively with Legacy Code* - Michael Feathers
- **Pattern**: Characterization Tests (Golden Master)
- **Branche**: `test/characterization-phase1`
- **Documentation**: `CHARACTERIZATION_TESTS_PHASE1_SUMMARY.md`

---

**Dernière mise à jour**: Phase 1 complétée  
**Prochain jalon**: Phase 2 - Tests StroopViewModel & Workflows
