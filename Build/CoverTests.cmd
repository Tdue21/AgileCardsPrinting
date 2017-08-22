@echo off
cd ..\Source

del /s /f /q coverage



set cover=packages\OpenCover.4.6.519\tools\OpenCover.Console.exe 
set report=packages\ReportGenerator.2.5.6\tools\ReportGenerator.exe 
rem set console=nunit3-console.exe 
set console=packages\NUnit.ConsoleRunner.3.6.1\tools\nunit3-console.exe 
set target=PrintIssueCards.Tests\bin\Debug\PrintIssueCards.Tests.dll

%cover% -register:user -returntargetcode "-filter:+[PrintIssueCards]* -[*Test]* -[PrintIssueCards]*Annotations.*" "-target:%console%" "-targetargs:%target%"
%report% "-reports:results.xml" "-targetdir:.\coverage" "-reporttypes:HtmlSummary"