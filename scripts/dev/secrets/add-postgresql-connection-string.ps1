#Requires -PSEdition Core

<#
.SYNOPSIS
Adds a PostgreSQL connection string using "dotnet user-secrets".

.PARAMETER Project
The project whose <UserSecretsId> property will be used when storing secrets.

.PARAMETER Alias
The connection string alias.

.PARAMETER Configuration
The MSBuild configuration to use when reading the project file.

.PARAMETER DatabaseHost
The hostname of the database server.

.PARAMETER DatabasePort
The port of the database server.

.PARAMETER DatabaseUser
The authenticating user.

.PARAMETER DatabasePassword
The authenticating user's password.

.PARAMETER DatabaseName
The name of the database.

.PARAMETER NoConnectionPooling
Determines whether to disable connection pooling.
#>
[CmdletBinding()]
param(
    [string] $Project = "Api",
    [string] $Alias = "database",
    [string] $Configuration = "Debug",
    [string] $DatabaseHost = "localhost",
    [ushort] $DatabasePort = 5432,
    [string] $DatabaseUser = $Alias,
    [string] $DatabasePassword = $Alias,
    [string] $DatabaseName = $Alias,
    [int] $CommandTimeoutInSeconds = 600,
    [int] $TimeoutInSeconds = 15,
    [switch] $NoConnectionPooling
)

Set-StrictMode -Version Latest

$ErrorActionPreference = "Stop"

$projectDirectory = Join-Path $PSScriptRoot .. .. .. source $Project -Resolve
$isVerbose = $PSBoundParameters["Verbose"] -eq $true -or $VerbosePreference -eq "Continue"

$arguments = @(
    "user-secrets",
    "set",
    "ConnectionStrings:$Alias",
    "Server=`"$DatabaseHost`"; Port=$DatabasePort; User Id=`"$DatabaseUser`"; Password=`"$DatabasePassword`"; Database=`"$DatabaseName`"; CommandTimeout=$CommandTimeoutInSeconds; Timeout=$TimeoutInSeconds; Pooling=$((-not $NoConnectionPooling).ToString().ToLowerInvariant());"
    "--project", $projectDirectory,
    "--configuration", $Configuration
)

if ($isVerbose) {
    $arguments += "--verbose"
}

dotnet $arguments