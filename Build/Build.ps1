$vswhere        ="..\Source\packages\vswhere.2.7.1\tools\vswhere.exe"
$openCover      ="..\Source\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe"
$reportGenerator="..\Source\packages\ReportGenerator.2.5.10\tools\ReportGenerator.exe" 
$nunitConsole   ="..\Source\packages\NUnit.ConsoleRunner.3.7.0\tools\nunit3-console.exe" 
$codecov        ="..\Source\packages\Codecov.1.0.3\tools\codecov.exe"
$coverTarget    ="..\Source\AgileCardsPrinting.Tests\bin\Debug\AgileCardsPrinting.Tests.dll"
$token          ="6b363d7c-f86e-4ad2-b806-75c3bf5420e4"

"$(Get-Date -f o) Restoring nuget packages."

& .\nuget.exe restore ..\Source\AgileCardsPrinting.sln

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

    "$(Get-Date -f o) Starting OpenCover."
    & $openCover -register:user -returntargetcode "-filter:+[AgileCardsPrinting]* -[*Test]* -[AgileCardsPrinting]*Annotations.*" -target:$nunitConsole -targetargs:$coverTarget

    "$(Get-Date -f o) Starting ReportGenerator."
    & $reportGenerator "-reports:results.xml" "-targetdir:.\coverage" "-reporttypes:HtmlSummary"
    
    "$(Get-Date -f o) Uploading result to Codecov.io"
    & $codecov -f .\results.xml -t $token
}

"$(Get-Date -f o) All done."

