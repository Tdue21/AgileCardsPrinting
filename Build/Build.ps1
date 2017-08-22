Import-Module -Name .\Invoke-MsBuild.psm1

$openCover      ="..\Source\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe"
$reportGenerator="..\Source\packages\ReportGenerator.2.5.6\tools\ReportGenerator.exe" 
$nunitConsole   ="..\Source\packages\NUnit.ConsoleRunner.3.6.1\tools\nunit3-console.exe" 
$coverTarget    ="..\Source\AgileCardsPrinting.Tests\bin\Debug\AgileCardsPrinting.Tests.dll"

"$(Get-Date -f o) Restoring nuget packages."

& .\nuget.exe restore ..\Source\AgileCardsPrinting.sln

"$(Get-Date -f o) Starting MSBuild."

$result = Invoke-MsBuild -Path ..\Source\AgileCardsPrinting.sln -ShowBuildOutputInCurrentWindow -BuildLogDirectoryPath . -KeepBuildLogOnSuccessfulBuilds -BypassVisualStudioDeveloperCommandPrompt

"$(Get-Date -f o) MSBuild finished."

if($result.BuildSucceeded -eq $true) {

    "$(Get-Date -f o) Starting OpenCover."
    & $openCover -register:user -returntargetcode "-filter:+[AgileCardsPrinting]* -[*Test]* -[AgileCardsPrinting]*Annotations.*" -target:$nunitConsole -targetargs:$coverTarget
    
    "$(Get-Date -f o) Starting ReportGenerator."
    & $reportGenerator "-reports:results.xml" "-targetdir:.\coverage" "-reporttypes:HtmlSummary"

}

"$(Get-Date -f o) All done."

