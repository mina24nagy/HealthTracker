<#
.SYNOPSIS
    Backs up the local HealthTracker PostgreSQL database (docker compose service "postgres").

.DESCRIPTION
    Runs pg_dump inside the container (custom format, compressed) and copies the file out.
    Dumping inside the container avoids PowerShell re-encoding binary output.
    Backups go to ./backups by default, which is git-ignored: they contain real health data.

.EXAMPLE
    ./scripts/db-backup.ps1
    ./scripts/db-backup.ps1 -OutputDirectory D:\Backups\HealthTracker
#>
[CmdletBinding()]
param(
    [string] $OutputDirectory = (Join-Path $PSScriptRoot '..\backups')
)

$ErrorActionPreference = 'Stop'
Push-Location (Join-Path $PSScriptRoot '..')
try {
    New-Item -ItemType Directory -Force -Path $OutputDirectory | Out-Null

    $name = 'healthtracker-{0:yyyyMMdd-HHmmss}.dump' -f (Get-Date)
    $containerPath = "/tmp/$name"
    $target = Join-Path (Resolve-Path $OutputDirectory) $name

    docker compose exec -T postgres sh -c "pg_dump -U `"`$POSTGRES_USER`" -d `"`$POSTGRES_DB`" --format=custom --file=$containerPath"
    if ($LASTEXITCODE -ne 0) { throw 'pg_dump failed. Is the stack running? (docker compose up -d postgres)' }

    docker compose cp "postgres:$containerPath" $target
    if ($LASTEXITCODE -ne 0) { throw 'Copying the dump out of the container failed.' }

    docker compose exec -T postgres rm -f $containerPath | Out-Null

    Write-Host "Backup written to $target"
}
finally {
    Pop-Location
}
