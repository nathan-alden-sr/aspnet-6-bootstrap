#Requires -PSEdition Core

<#
.SYNOPSIS
Runs a Seq Docker container.

.PARAMETER Id
An ID that uniquely identifies the container and the volume that stores Seq data.

.PARAMETER Network
The name of the network the container will connect to.
#>
[CmdletBinding()]
param(
    [string] $Id = "company_product",
    [string] $Network = $Id
)

Set-StrictMode -Version Latest

$ErrorActionPreference = "Stop"

# Pull postgres image

[int] $option = $Host.UI.PromptForChoice(
    "Pull latest datalust/seq image?",
    $null,
    @( "&Yes", "&No" ),
    1)

if ($option -eq 0) {
    Write-Verbose "Pulling datalust/seq image"

    docker pull datalust/seq
}

Write-Host

# Create volume

[string] $volumeName = "$Id`_seq"

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

[string] $containerName = "$Id`_seq"

Write-Verbose "Running $containerName container"

docker run `
    --detach `
    --restart unless-stopped `
    --name $containerName `
    --volume "$volumeName`:/data" `
    --network $Network `
    --publish 5340:80 `
    --publish 5341:5341 `
    --env ACCEPT_EULA=Y `
    datalust/seq:latest