#
# .SYNOPSIS
# Get pending tasks from todo files.
#
# .DESCRIPTION
# Loads all todo-YYMMDD.txt files, parses them using the
# Kkj.Tasks library, and displays pending tasks sorted by
# date and priority. Use -Today to see only the latest
# snapshot, or -Days to look back a specific number of days.
#
# .EXAMPLE
# .\Get-Task.ps1
# Lists all pending tasks across all todo files.
#
# .EXAMPLE
# .\Get-Task.ps1 -Today
# Lists only tasks from today's file.
#
# .EXAMPLE
# .\Get-Task.ps1 -Days 7
# Lists tasks from the last 7 days.
#

[CmdletBinding(
    DefaultParametersetName = 'Convenience'
)]
param(

    #
    # Get todays tasks.
    #
    [Parameter(
        ParameterSetName = 'Convenience'
    )]
    [switch] $Today,

    #
    # How many days old.
    #
    [Parameter(
        ParameterSetName = 'Days'
    )]
    [int] $Days,

    #
    # Path to the folder containing todo files.
    #
    [string] $Path = (
        $env:OneDrive | Join-Path -ChildPath 'todo'
    ),

    #
    # Encoding of the todo files.
    #
    [System.Text.Encoding] $Encoding =
    [System.Text.Encoding]::GetEncoding(1252)

)

$netVersion = "net$([System.Environment]::Version.Major).0"
Add-Type -Path (
    Join-Path $PSScriptRoot "Kkj.Tasks\bin\Release\$netVersion\Kkj.Tasks.dll"
)
$parser = New-Object -TypeName Kkj.Tasks.TaskParser
$store = New-Object -TypeName Kkj.Tasks.MemoryTaskStore
$factory = New-Object `
    -TypeName Kkj.Tasks.TaskFactory `
    -ArgumentList $parser, $store
$taskFilePattern = '^todo-(\d{2})(\d{2})(\d{2}).txt$'
$taskFiles = Get-ChildItem -Path $Path -Filter todo-*.txt
$taskFiles |
Where-Object -FilterScript { $_.Name -match $taskFilePattern } |
ForEach-Object -Process {
    $date = New-Object `
        -TypeName System.DateTime `
        -ArgumentList "20$($Matches[1])", $Matches[2], $Matches[3]
    $_ | Get-Content -Encoding $Encoding | ForEach-Object {
        $factory.Create($date, $_) | Out-Null
    }
}
$store.Tasks |
Where-Object -FilterScript { $_.Status -ne 'Done' } |
Where-Object -FilterScript {
    if ($Today) {
        $_.Date -ge [System.DateTime]::Today
    }
    elseif ($Days -gt 0) {
        $_.Date -ge [System.DateTime]::Today.AddDays(-$Days)
    }
    else {
        $true
    }
} |
Sort-Object -Property Date, Priority -Descending |
Select-Object `
    -Property `
    Priority, `
    Status, `
@{ Name = 'Date'; Expression = { '{0:d}' -f $_.Date } }, `
    Name
