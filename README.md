# Tasks

A plain-text task management system built on a simple idea: **a task list is a
snapshot**. Each list captures what's on your mind right now. The previous list
carries forward as a starting point — not as obligation, but as context. Tasks
that are done get checked off. Tasks that aren't relevant anymore get removed.
Tasks that linger are not failures — they are reminders of things you _could_ do
when the moment is right.

The system is designed around a few core principles:

- **Snapshots** — one list per session, a picture of current intentions
- **Carry-forward** — the previous list is the starting point for the next one
- **External memory** — the list holds more than you plan to do; it's a pool of
  possibilities to draw inspiration from
- **History is preserved** — completed and uncompleted tasks stay visible over
  time, forming a narrative
- **Low friction above all** — adding a task should be as easy as writing a
  sentence

## The format

Tasks are stored in plain text files named by date (`todo-YYMMDD.txt`). Each
line is one task:

```
[!?] [status] task name { tags }
```

**Priority** (optional): `!` = high, `?` = low, blank = normal. **Status**:
`[ ]` = pending, `[X]` = done. **Tags** (optional): comma-separated in braces.

Examples:

```
! [ ] Fix the leaking faucet { home }
  [X] Write unit tests for parser { dev, project }
? [ ] Look into that new framework { dev }
  [ ] Buy groceries
```

A single regex parses it all:

```
^\s*([!?])?\s*\[([ X])\]\s*([^{]+)(?:{\s*([^}]+)})?$
```

### Sections

Files can have section headers to visually separate tasks into zones. The parser
ignores them — they are purely for the human reader:

```
Today

 [!] Pay bills                        { home, finance }
  [ ] Install new router              { home, IT }

Backlog

  [ ] Read Liars and Outliers         { books }
  [ ] Learn Python                    { dev }
```

**Today** is what you might tackle now. **Backlog** rides along so it's not
forgotten. The boundary is fluid — tasks move between sections as priorities
shift.

## The C# library (Kkj.Tasks)

A .NET library that parses the format into a structured model:

| Class             | Purpose                                                                |
| ----------------- | ---------------------------------------------------------------------- |
| `TaskParser`      | Parses a single line into a `ParserResult`                             |
| `TaskFactory`     | Creates `Task` and `TaskVersion` objects from parser output            |
| `Task`            | A named task with a collection of `TaskVersion` entries                |
| `TaskVersion`     | A snapshot of a task at a point in time (date, status, priority, tags) |
| `MemoryTaskStore` | Stores tasks keyed by name, versions sorted chronologically            |
| `Tag`             | A category label with references back to its tasks                     |

The key design choice: **a task is its history**. `Task` doesn't store status
directly — it delegates to `Versions.Last()`. The `MemoryTaskStore` uses
`IDictionary<string, SortedDictionary<DateTime, TaskVersion>>`, making every
task a chronological sequence of states across snapshots.

### Build and test

```
dotnet build -c Release
dotnet test
```

Targets .NET 9 and .NET 10. Tests use MSTest and NSubstitute.

## PowerShell scripts

Two PowerShell scripts for the command-line workflow:

**`Get-Task.ps1`** loads all todo files via the C# library and outputs pending
tasks as objects. Supports `-Today`, `-Days`, `-Path`, and `-Encoding`
parameters.

```powershell
.\Get-Task.ps1                # all pending tasks
.\Get-Task.ps1 -Today         # today's snapshot only
.\Get-Task.ps1 -Days 7        # last 7 days
```

**`New-Task.ps1`** finds the most recent todo file, copies it to a new file
named with today's date, and opens it in the default editor. This is the
carry-forward workflow — _start from where you left off_.

Both scripts default to `$env:OneDrive\todo` and Windows-1252 encoding,
configurable via `-Path` and `-Encoding`.

## License

This project is not currently licensed. It is published as a historical artifact
and reference for anyone interested in lightweight, text-based task management.
