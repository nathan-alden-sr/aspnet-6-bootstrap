#Requires -PSEdition Core

<#
.SYNOPSIS
Adds a PostgreSQL connection string using "dotnet user-secrets".

.PARAMETER Projects
The projects whose <UserSecretsId> properties will be used when storing secrets.

.PARAMETER Environment
The DOTNET_ENVIRONMENT to use as a prefix for the connection string name.

.PARAMETER Name
The connection string name.

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
    [ValidateSet("Api", "ScheduledTasks")]
    [string[]] $Projects = @("Api", "ScheduledTasks"),
    [Parameter(Mandatory)]
    [ValidateSet("DeveloperDocker", "DeveloperVisualStudio")]
    [string] $Environment,
    [string] $Name,
    [string] $Configuration = "Debug",
    [string] $DatabaseHost,
    [ushort] $DatabasePort = 5432,
    [string] $DatabaseUser = "company_product",
    [string] $DatabasePassword = "company_product",
    [string] $DatabaseName = $Name,
    [int] $CommandTimeoutInSeconds = 600,
    [int] $TimeoutInSeconds = 15,
    [switch] $NoConnectionPooling
)

Set-StrictMode -Version Latest

$ErrorActionPreference = "Stop"

foreach ($project in $Projects) {
    [string] $projectDirectory = Join-Path $PSScriptRoot .. .. .. source $project -Resolve
    [bool] $isVerbose = $PSBoundParameters["Verbose"] -eq $true -or $VerbosePreference -eq "Continue"

    [string[]] $arguments = @(
        "user-secrets",
        "set",
        "ConnectionStrings:$Environment-$Name",
        "Server=`"$DatabaseHost`"; Port=$DatabasePort; User Id=`"$DatabaseUser`"; Password=`"$DatabasePassword`"; Database=`"$DatabaseName`"; CommandTimeout=$CommandTimeoutInSeconds; Timeout=$TimeoutInSeconds; Pooling=$((-not $NoConnectionPooling).ToString().ToLowerInvariant());"
        "--project", $projectDirectory,
        "--configuration", $Configuration
    )

    if ($isVerbose) {
        $arguments += "--verbose"
    }

    Write-Verbose "Setting connection string secret for $project project"

    dotnet $arguments
}
