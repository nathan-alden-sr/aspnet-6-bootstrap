<#
.SYNOPSIS
Runs a PostgreSQL Docker container.

.PARAMETER Id
An ID that uniquely identifies the container and the volume that stores PostgreSQL data.

.PARAMETER Network
The name of the network the container will connect to.

.PARAMETER DatabaseUser
The authenticating user.

.PARAMETER DatabasePassword
The authenticating user's password.
#>
[CmdletBinding()]
param(
    [string] $Id = "database",
    [string] $Network = $Id,
    [string] $DatabaseUser = $Id,
    [string] $DatabasePassword = $Id
)

Set-StrictMode -Version Latest

$ErrorActionPreference = "Stop"

# Pull postgres image

[int] $postgresVersion = 14
[int] $option = $Host.UI.PromptForChoice(
    "Pull postgres:$postgresVersion image?",
    $null,
    @( "&Yes", "&No" ),
    1)

if ($option -eq 0) {
    Write-Verbose "Pulling postgres:$postgresVersion image"

    docker pull "postgres:$postgresVersion"
}

Write-Host

# Create volume

[string] $volumeName = "$Id`_postgres"

$option = $Host.UI.PromptForChoice(
    "Create $volumeName volume?",
    $null,
    @( "&Yes", "&No" ),
    1)

if ($option -eq 0) {
    Write-Verbose "Creating $volumeName volume"

    docker volume create $volumeName
}

Write-Host

# Run container

[string] $containerName = "$Id`_postgres"

Write-Verbose "Running $containerName container"

docker run `
    --detach `
    --name $containerName `
    --volume "$volumeName`:/var/lib/postgresql/data" `
    --network $Network `
    --publish 5432:5432 `
    --env "POSTGRES_USER=`"$DatabaseUser`"" `
    --env "POSTGRES_PASSWORD=`"$DatabasePassword`"" `
    "postgres:$postgresVersion"