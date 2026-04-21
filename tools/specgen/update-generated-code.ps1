[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0, HelpMessage = "Release tag to use for moby api/client (example: docker-v29.1.5)")]
    [ValidateNotNullOrEmpty()]
    [string]$ReleaseTag
)

$ErrorActionPreference = 'Stop'

$scriptDir = $PSScriptRoot
$modelsDir = Resolve-Path (Join-Path $scriptDir '..\..\src\Docker.DotNet\Models')
$specgenExe = Join-Path $scriptDir 'specgen.exe'

$version = $ReleaseTag -replace 'docker-', ''
$propsPath = Join-Path $scriptDir '..\..\Directory.Build.props'

Push-Location $scriptDir

try {
    Write-Host "Updating moby api package to tag '$ReleaseTag'"
    go get "github.com/moby/moby/api@$ReleaseTag"

    Write-Host "Updating moby client package to tag '$ReleaseTag'"
    go get "github.com/moby/moby/client@$ReleaseTag"

    Write-Host "Updating props DockerVersion with '$version'"
    $propsContent = Get-Content $propsPath -Raw
    $propsContent = $propsContent -replace '(?<=<DockerVersion>).*?(?=</DockerVersion>)', $version
    Set-Content $propsPath $propsContent -NoNewline

    Write-Host 'Building specgen'
    go build

    Write-Host "Deleting existing generated model classes in '$modelsDir'"
    Get-ChildItem -Path $modelsDir -Filter '*.Generated.cs' -File | Remove-Item -Force

    Write-Host 'Regenerating model classes'
    & $specgenExe $modelsDir
}
finally {
    if (Test-Path -Path $specgenExe) {
        Remove-Item -Path $specgenExe -Force
    }

    Pop-Location
}