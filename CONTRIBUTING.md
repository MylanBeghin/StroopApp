# ?? Guide de Contribution - StroopApp

Merci de vouloir contribuer à StroopApp ! Ce document décrit les conventions et bonnes pratiques à suivre.

---

## ?? Conventions de Commit

### Format
```
<type>(<scope>): <description courte>

<corps optionnel>

<footer optionnel>
```

### Types disponibles

| Type | Description | Emoji |
|------|-------------|-------|
| `feat` | Nouvelle fonctionnalité | ? |
| `fix` | Correction de bug | ?? |
| `refactor` | Refactoring (pas de changement fonctionnel) | ?? |
| `test` | Ajout/modification de tests | ? |
| `docs` | Documentation | ?? |
| `perf` | Amélioration de performance | ? |
| `style` | Formatage, conventions de code | ?? |
| `chore` | Maintenance (deps, configs) | ?? |
| `ci` | CI/CD | ?? |

### Exemples

? **Bon :**
```
fix(async): convert async void to async Task with error handling

CRITICAL FIX - Prevents silent exceptions in production.

Changes:
- ViewModelBase: ShowErrorDialogAsync now returns Task
- All ViewModels updated to async Task pattern
- Added try/catch blocks for error logging

BREAKING CHANGE: ShowErrorDialogAsync signature changed.
Callers must now await this method.
```

? **Bon :**
```
feat(export): add multilingual support for export headers

- Export headers now use LanguageService
- Support for French and English
- Backward compatible with existing exports
```

? **Mauvais :**
```
Fixed some bugs
```

? **Mauvais :**
```
The SerialPort management feature has been fully removed and all the code related to it
```

### Règles

- ? **Titre** : max 72 caractères, impératif ("Add", pas "Added" ou "Adding")
- ? **Scope** : facultatif mais recommandé (ex: `async`, `export`, `ui`, `i18n`)
- ? **Corps** : explique le QUOI et POURQUOI, pas le COMMENT
- ? **Footer** : référence les issues (`Closes #123`) et breaking changes

---

## ?? Conventions de Branches

### Format
```
<type>/<description-kebab-case>
```

### Exemples
```
fix/async-void-corrections
feat/serial-port-support
refactor/export-cleanup
test/stroop-viewmodel-coverage
docs/contributing-guide
```

---

## ?? Pull Requests

### Titre
```
<emoji> <Type>: <Description claire>

Exemples :
?? Fix: Convert async void to async Task
? Feature: Add serial port communication
?? Refactor: Simplify export architecture
```

### Description

Utilise le template fourni dans `.github/PULL_REQUEST_TEMPLATE.md`.

Points clés :
- ?? **Objectif** : Résume le problème/feature en 1-2 phrases
- ?? **Changements** : Liste les modifications principales
- ?? **Tests** : Décris comment tu as testé
- ?? **Breaking Changes** : Si applicable, détaille l'impact
- ? **Checklist** : Coche toutes les cases applicables

---

## ? Checklist Avant de Soumettre

### Code
- [ ] Mon code suit les conventions C# et WPF du projet
- [ ] J'ai effectué une auto-review
- [ ] J'ai ajouté des commentaires dans les zones complexes
- [ ] Pas de code commenté inutile
- [ ] Pas de `Console.WriteLine` ou logs de debug

### Tests
- [ ] Tous les tests unitaires passent (`dotnet test`)
- [ ] J'ai ajouté des tests pour mes changements
- [ ] J'ai testé manuellement dans l'application
- [ ] Aucune régression détectée

### Build
- [ ] `dotnet build` réussit sans erreurs
- [ ] Aucun nouveau warning introduit
- [ ] Les dépendances NuGet sont à jour si modifiées

### Documentation
- [ ] README mis à jour si nécessaire
- [ ] XML comments ajoutés pour les méthodes publiques
- [ ] CHANGELOG.md mis à jour (si applicable)

---

## ?? Tester Localement

```bash
# Cloner le repo
git clone https://github.com/MylanBeghin/StroopApp.git
cd StroopApp

# Restaurer les dépendances
dotnet restore

# Build
dotnet build

# Lancer les tests
dotnet test

# Lancer l'application
dotnet run --project StroopApp/StroopApp.csproj
```

---

## ?? Ressources

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [WPF Best Practices](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)

---

## ?? Besoin d'Aide ?

- ?? Ouvre une [Issue](https://github.com/MylanBeghin/StroopApp/issues)
- ?? Demande dans les [Discussions](https://github.com/MylanBeghin/StroopApp/discussions)

---

Merci pour ta contribution ! ??
