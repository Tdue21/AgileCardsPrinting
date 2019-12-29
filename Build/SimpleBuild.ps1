function Invoke-InstallTool {
    Param(
     [string]$nugetId,
     [string]$version
	)
    & .\tools\nuget.exe install $nugetId -version $version -outputdirectory tools -directdownload -excludeversion
}
$nuget           = "tools\nuget.exe"  
$vswhere         = "tools\vswhere\tools\vswhere.exe"  
$openCover       = "tools\OpenCover\tools\OpenCover.Console.exe"
$reportGenerator = "tools\ReportGenerator\tools\netcoreapp3.0\ReportGenerator.exe" 
$nunitConsole    = "tools\NUnit.ConsoleRunner\tools\nunit3-console.exe" 
$codecov         = "tools\Codecov\tools\codecov.exe"

$coverTarget     = "..\Source\AgileCardsPrinting.Tests\bin\Debug\AgileCardsPrinting.Tests.dll"
$token           = "6b363d7c-f86e-4ad2-b806-75c3bf5420e4"

"=================================================="
"$(Get-Date -f o) Downloading tools"
Invoke-InstallTool vswhere 2.8.4
Invoke-InstallTool OpenCover 4.6.519
Invoke-InstallTool ReportGenerator 4.4.0
Invoke-InstallTool NUnit.ConsoleRunner 3.10.0
Invoke-InstallTool Codecov 1.9.0

"=================================================="
"$(Get-Date -f o) Restoring nuget packages."

& $nuget restore ..\Source\AgileCardsPrinting.sln

"=================================================="
"$(Get-Date -f o) Starting MSBuild."
$msbuild = & $vswhere -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MsBuild.exe

if(Test-Path -Path "env:APPVEYOR")
{
    & $msbuild "..\Source\AgileCardsPrinting.sln" /nologo /v:n /ds /p:UseSharedCompilation=false `
             /logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll"
}
else 
{
    & $msbuild "..\Source\AgileCardsPrinting.sln" /nologo /v:n /ds /p:UseSharedCompilation=false
}

"$(Get-Date -f o) MSBuild finished."


if($LASTEXITCODE -eq 0) {

    "=================================================="
    "$(Get-Date -f o) Starting OpenCover."
    & $openCover -register:user -returntargetcode "-output:CoverageResult.xml" "-filter:+[AgileCardsPrinting]* -[*Test]* -[AgileCardsPrinting]*Annotations.*" -target:$nunitConsole -targetargs:$coverTarget

    "=================================================="
    "$(Get-Date -f o) Starting ReportGenerator."
    & $reportGenerator "-reports:CoverageResult.xml" "-targetdir:.\coverage" "-reporttypes:HtmlSummary"
    
    "=================================================="
    "$(Get-Date -f o) Uploading result to Codecov.io"
    & $codecov -f .\CoverageResult.xml -t $token
}

"$(Get-Date -f o) All done."

