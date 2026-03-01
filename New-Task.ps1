#
# .SYNOPSIS
# Create a new task list from the most recent one.
#
# .DESCRIPTION
# Finds the most recent todo-YYMMDD.txt file, copies its
# content to a new file named with today's date, and
# returns the path. If today's file already exists, returns
# that instead.
#
# .EXAMPLE
# .\New-Task.ps1
# Creates today's task list and opens it in the default
# editor.
#

[CmdletBinding()]
param(

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

$taskFilePattern = '^todo-(\d{2})(\d{2})(\d{2}).txt$'
$taskFiles = Get-ChildItem -Path $Path -Filter todo-*.txt
$lastTasks = $taskFiles |
Where-Object -FilterScript { $_.Name -match $taskFilePattern } |
ForEach-Object -Process {
    New-Object -TypeName PSObject -Property @{
        Date = New-Object `
            -TypeName System.DateTime `
            -ArgumentList "20$($Matches[1])", $Matches[2], $Matches[3]
        File = $_
    }
} |
Sort-Object -Property Date -Descending |
Select-Object -First 1

if ($lastTasks) {
    $today = [datetime]::Today
    if ($lastTasks.Date -eq $today) {
        Invoke-Item $lastTasks.File.FullName
    }
    else {
        $newTasks = $Path |
        Join-Path -ChildPath ('todo-{0}.txt' -f $today.ToString('yyMMdd'))
        Get-Content -Path $lastTasks.File.FullName -Encoding $Encoding |
        Add-Content -Path $newTasks -Encoding $Encoding
        Invoke-Item $newTasks
    }
}
else {
    throw 'No previous task file found.'
}
