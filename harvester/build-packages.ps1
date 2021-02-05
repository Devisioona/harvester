param(
    [Parameter(Mandatory=$true, HelpMessage = "Version tag to use, for example v0.5.2")]
    [string]$versionId
)

Write-Host "Building version $versionId"

$runtimes = @("linux-x64", "linux-arm64", "win10-x64", "win10-x86", "win10-arm64")

Write-Host "Clean."
$runtimes | % { if ( Test-Path ../dist/$_ ) { Remove-Item -Recurse ../dist/$_ } }

Write-Host "Build."
$runtimes | % { dotnet publish -v q -c Release -r $_ -o "../dist/$_" }

Write-Host "Finalize packages."
$runtimes | % { Remove-Item ../dist/$_/appsettings.local.json }
$runtimes | % { Remove-Item ../dist/$_/*.pdb }
$runtimes | % { Copy-Item ../harvester.txt ../dist/$_/ }

Write-Host "Pack (win10)."
$runtimes | ? { $_ -match "^win10" } | % { Compress-Archive -Force -Path ../dist/$_/* -DestinationPath ../dist/Harvester-$versionId-$_.zip }

Write-Host "Done."
