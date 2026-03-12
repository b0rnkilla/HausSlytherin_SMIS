# Haus Slytherin – Development Guidelines

Dieses Dokument definiert die Coding- und Git-Regeln für das Projekt
**Slytherin Magical Intelligence System**.

Ziel ist es, dass der Code:

* konsistent
* wartbar
* verständlich
* konfliktarm im Git

entwickelt wird.

---

# 1 C# Naming Conventions

## Klassen

PascalCase

```
Creature
CreatureService
RiskAnalysisService
InvalidDangerLevelException
```

---

## Methoden

PascalCase

```
AddCreature()
GetAllCreatures()
GenerateRiskReport()
CalculateRisk()
```

---

## Properties

PascalCase

```
public int DangerLevel { get; set; }
public string Name { get; set; }
```

---

## Private Felder

camelCase mit `_`

```
private List<Creature> _creatures;
private readonly ICreatureRepository _repository;
```

---

## Interfaces

Prefix `I`

```
IRepository
ICreatureRepository
IRiskStrategy
IReportGenerator
```

---

## Enums

PascalCase

```
CreatureType
IncidentSeverity
House
```

Enum Werte ebenfalls PascalCase.

```
Dragon
Basilisk
Hippogriff
```

---

# 2 Code Style

## Eine Klasse pro Datei

Dateiname = Klassenname

```
Creature.cs
CreatureService.cs
```

---

## Reihenfolge innerhalb einer Klasse

```
Fields
Properties
Constructor
Public Methods
Private Methods
```

---

# 4 Git Workflow

Es wird **nicht direkt auf main gearbeitet**.

## Branch Naming

Format:
```
type/short-description
```

Beispiele:
```
feat/1.A2.Models_anlegen
feat/3.A16.RiskReport_Model_bauen
fix/Models.Creature
ref/CreatureRepository
feat/CodeGuidlines_hinzugefuegt
```

---

# 5 Commit Messages

Commit Messages sollen kurz und klar sein.

Format:
```
title

type: short description
type: short description
type: short description

detailed description text (optional)
```

Beispiele:
```
feat: IRepository erstellt
feat: ICreatureRepository erstellt
feat: CreatureRepository erstellt
fix: CreatureService Methode GetAllCreatures korrigiert
refactor: Factories in eine generische Methode zusammengefasst
docs: Projekt CodeGuidelines hinzugefügt
```

Types:
```
feat        <- neues Features, Klassen, Methoden, etc.
fix         <- Fehlerbehebung
refactor    <- Codeverbesserung ohne Funktionalitätsänderung
docs        <- Dokumentation, Kommentare, Summaries, Readme, etc.
test        <- Tests
```

---

# 6 Pull Requests

Nach dem Commit und Push und vor dem Merge in `main` muss ein Pull Request erstellt werden.

Der Pull Request muss enthalten:

* Beschreibung der Änderung
* Welche Dateien betroffen sind
* Kurze Erklärung der Implementierung

Beispiel:
```
CreatureService um Methode zum Berechnen des durchschnittlichen DangerLevels erweitert.
Verwendet LINQ zur Auswertung der Creature-Liste.
Neue Methode: GetAverageDangerLevel().
```

---

# 7 Code Review Regeln

**Eine Person reviewt den Code**, bevor er gemerged wird.

Review prüft:
* Naming korrekt
* Architektur eingehalten
* keine doppelte Logik
* Methoden nicht zu lang
* und so weiter

---

# 8 Architekturregeln

Wichtige Architekturregeln:

Models enthalten keine Business Logik.

Factories erzeugen Objekte.

Services orchestrieren Logik.

Repositories kümmern sich nur um Datenzugriff.

Strategies implementieren austauschbare Algorithmen.

---

# 9 Exceptions

Eigene Exceptions werden im Ordner `Exceptions` gespeichert.

Beispiele:
```
InvalidDangerLevelException
InvalidIncidentException
```

Exceptions werden im Service geworfen und im Program abgefangen.

---

# 10 LINQ Nutzung

LINQ soll genutzt werden für:

* Durchschnittswerte
* Filter
* Sortierung
* Statistiken

Beispiel:
```csharp
var dangerousCreatures = creatures
    .Where(c => c.DangerLevel > 7)
    .OrderByDescending(c => c.DangerLevel);
```

---

# 11 Allgemeine Regeln

* Keine Magic Numbers
* Methoden möglichst kurz halten
* Klassen sollen eine klare Verantwortung haben (SRP)
* Code muss lesbar sein

---

# 12 Ziel des Projekts

Das Ziel ist nicht nur ein funktionierendes Programm, sondern ein **sauberes Architekturprojekt**, das folgende Konzepte zeigt:

* OOP
* SOLID
* Repository Pattern
* Service Layer
* Strategy Pattern
* Factory Pattern
* Exception Handling
* LINQ