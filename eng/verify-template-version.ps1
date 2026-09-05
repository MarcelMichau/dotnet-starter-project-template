[CmdletBinding()]
param(
    [string]$ProjectPath = (Join-Path $PSScriptRoot "..\DotNetStarterProjectTemplate.Package.csproj"),
    [string]$ReadmePath = (Join-Path $PSScriptRoot "..\README.md")
)

$ErrorActionPreference = "Stop"

[xml]$project = Get-Content -Raw -LiteralPath $ProjectPath
$packageVersion = [string]$project.Project.PropertyGroup.PackageVersion

if ([string]::IsNullOrWhiteSpace($packageVersion)) {
    throw "PackageVersion was not found in '$ProjectPath'."
}

$readme = Get-Content -Raw -LiteralPath $ReadmePath
$installCommandPattern = '(?m)^dotnet new install MarcelMichau\.Templates\.DotNetStarterProject@(?<version>[^\s`]+)\s*$'
$installCommand = [regex]::Match($readme, $installCommandPattern)

if (-not $installCommand.Success) {
    throw "The versioned template install command was not found in '$ReadmePath'."
}

$readmeVersion = $installCommand.Groups['version'].Value

if ($packageVersion -ne $readmeVersion) {
    throw "Template versions do not match: PackageVersion is '$packageVersion', but README.md uses '$readmeVersion'."
}

Write-Host "Template package version is synchronized at $packageVersion."
