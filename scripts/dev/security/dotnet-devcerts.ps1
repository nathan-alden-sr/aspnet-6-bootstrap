#Requires -PSEdition Core

<#
.SYNOPSIS
.NET HTTPS development certificate management.

.PARAMETER Action
The action to take with .NET HTTPS development certificates.
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidateSet("Trust", "Clean", "Check")]
    [string] $Action
)

Set-StrictMode -Version Latest

$ErrorActionPreference = "Stop"

[bool] $isVerbose = $PSBoundParameters["Verbose"] -eq $true -or $VerbosePreference -eq "Continue"

$arguments = @(
    "dev-certs",
    "https",
    "--quiet"
)

switch ($Action) {
    "Trust" {
        $arguments += "--trust"
    }
    "Clean" {
        $arguments += "--clean"
    }
    "Check" {
        $arguments += "--check"
    }
}

if ($isVerbose) {
    $arguments += "--verbose"
}

& dotnet $arguments