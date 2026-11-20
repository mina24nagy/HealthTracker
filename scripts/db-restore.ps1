<#
.SYNOPSIS
    Restores a backup made by db-backup.ps1 into the local HealthTracker database.

.DESCRIPTION
    DESTRUCTIVE: existing objects are dropped and replaced by the backup's contents.
    Asks for confirmation unless -Confirm:$false is passed.

.EXAMPLE
    ./scripts/db-restore.ps1 -Path .\backups\healthtracker-20261120-210000.dump
#>
[CmdletBinding(SupportsShouldProcess, ConfirmImpact = 'High')]
param(
    [Parameter(Mandatory)]
    [ValidateScript({ Test-Path $_ -PathType Leaf })]
    [string] $Path
)

$ErrorActionPreference = 'Stop'
$file = Resolve-Path $Path
Push-Location (Join-Path $PSScriptRoot '..')
try {
    if (-not $PSCmdlet.ShouldProcess('local HealthTracker database', "Replace contents with $file")) {
        return
    }

    $containerPath = '/tmp/restore.dump'
    docker compose cp $file "postgres:$containerPath"
    if ($LASTEXITCODE -ne 0) { throw 'Copying the dump into the container failed. Is the stack running?' }

    docker compose exec -T postgres sh -c "pg_restore -U `"`$POSTGRES_USER`" -d `"`$POSTGRES_DB`" --clean --if-exists --no-owner $containerPath"
    $restoreExit = $LASTEXITCODE
    docker compose exec -T postgres rm -f $containerPath | Out-Null
    if ($restoreExit -ne 0) { throw "pg_restore failed (exit code $restoreExit)." }

    Write-Host "Restored $file"
}
finally {
    Pop-Location
}
