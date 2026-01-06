# ?? Protocole Git & GitHub - StroopApp

## ?? Ton Protocole Personnalisé

Ce document combine **ton style actuel** avec les **bonnes pratiques professionnelles**.

---

## 1?? Structure des Commits

### Format Standard
```
<type>(<scope>): <description courte et claire>

<corps - explique le QUOI et POURQUOI>

<footer - issues, breaking changes>
```

### Exemple Concret (ton commit actuel)
```
fix(async): convert async void to async Task with error handling

CRITICAL FIX - Prevents silent exceptions and random crashes in production.

Changes:
- ViewModelBase: ShowErrorDialogAsync/ShowConfirmationDialogAsync now return Task
- All ViewModels: converted async void methods to async Task pattern  
- Added try/catch blocks with Debug.WriteLine for error logging
- Fixed typo: QuitWihtoutExport -> QuitWithoutExport

Testing:
- Updated TestableConfigurationPageViewModel to use override
- Converted unit tests to async Task pattern
- All 67 tests passing ?

BREAKING CHANGE: ShowErrorDialogAsync and ShowConfirmationDialogAsync
signatures changed. Callers must now await these methods.
```

### Types Autorisés

| Type | Quand l'utiliser | Emoji |
|------|------------------|-------|
| `feat` | Nouvelle fonctionnalité | ? |
| `fix` | Correction de bug | ?? |
| `refactor` | Refactoring (pas de changement fonctionnel) | ?? |
| `test` | Ajout/modif de tests | ? |
| `docs` | Documentation | ?? |
| `perf` | Amélioration perf | ? |
| `style` | Formatage, style | ?? |
| `chore` | Maintenance | ?? |

### Règles d'Or
- ? **Titre** : max 72 caractères, impératif
- ? **Corps** : facultatif mais recommandé pour changements complexes
- ? **Footer** : `Closes #XX` pour référencer une issue

---

## 2?? Nommage des Branches

### Format
```
<type>/<description-kebab-case>
```

### Exemples
```
fix/async-void-corrections        ? (ton style - parfait!)
feat/multilingual-export          ?
refactor/export-service-cleanup   ?
test/stroop-viewmodel-tests       ?
```

**Ton style actuel est excellent, continue comme ça !** ??

---

## 3?? Pull Requests

### Titre
```
<emoji> <Type>: <Description>

Exemples:
?? Fix: Convert async void to async Task with error handling
? Feature: Add multilingual export support
?? Refactor: Simplify ExportationService architecture
```

### Structure de Description

```markdown
## ?? Objectif
<!-- Résume le problème/feature en 1-2 phrases -->

## ?? Changements
<!-- Liste des modifs principales -->

## ?? Tests
<!-- Comment tu as testé -->

## ?? Breaking Changes
<!-- Si applicable -->

## ?? Notes
<!-- Infos pour reviewers -->

---
Closes #XX
```

---

## 4?? Workflow Complet

### Créer une Feature/Fix

```bash
# 1. Créer une branche depuis master
git checkout master
git pull origin master
git checkout -b fix/ma-correction

# 2. Faire tes modifications
# ... code code code ...

# 3. Tester
dotnet test

# 4. Commit avec message formaté
git add .
git commit -m "fix(scope): description courte"

# 5. Push
git push -u origin fix/ma-correction

# 6. Créer la PR sur GitHub avec le template
```

### Modifier un Commit Déjà Poussé

```bash
# Si tu veux améliorer ton dernier commit
git reset --soft HEAD~1
git commit -F .git/COMMIT_MSG
git push --force-with-lease origin ma-branche
```

---

## 5?? Exemples Avant/Après

### Commit Message

**? Avant (ton ancien style):**
```
The SerialPort management feature has been fully removed:
```

**? Après (nouveau protocole):**
```
refactor!: remove SerialPort management feature

BREAKING CHANGE: Serial port communication completely removed.
All references deleted from codebase.

Closes #46
```

---

**? Avant:**
```
Add dynamic localization to export and ViewModels
```

**? Après:**
```
feat(i18n): add dynamic localization to export and ViewModels

- Export headers now use localized strings
- ViewModels support language-aware content
- Language service integration across export flow

Resolves #45
```

---

## 6?? Checklist Rapide

### Avant Chaque Commit
- [ ] Le code compile sans warnings
- [ ] Tous les tests passent (67/67)
- [ ] Message de commit suit le format
- [ ] Auto-review effectuée

### Avant Chaque PR
- [ ] Branche à jour avec master
- [ ] Titre avec emoji approprié
- [ ] Description complète avec template
- [ ] Tests passants
- [ ] Screenshots si UI modifiée

---

## ?? Résumé Ultra-Rapide

### Commit
```
type(scope): courte description

corps optionnel

Closes #XX
```

### Branche
```
type/description-kebab
```

### PR
```
?? Type: Description

## Objectif
...

## Changements
...

## Tests
...
```

---

## ?? Fichiers de Référence

- `CONTRIBUTING.md` - Guide complet pour contribuer
- `.github/PULL_REQUEST_TEMPLATE.md` - Template de PR
- `PR_DESCRIPTION.md` - Exemple de description pour ta PR actuelle

---

**C'est tout ! Tu es prêt à faire des commits et PRs pro ! ??**
