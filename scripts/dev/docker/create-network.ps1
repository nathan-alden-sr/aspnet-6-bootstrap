<#
.SYNOPSIS
Creates a Docker network.

.PARAMETER Name
The name of the network to create.
#>
[CmdletBinding()]
param(
    [string] $Name = "database"
)

Set-StrictMode -Version Latest

$ErrorActionPreference = "Stop"

Write-Verbose "Creating $Name network"

docker network create $Name