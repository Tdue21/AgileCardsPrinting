@echo off
packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -returntargetcode "-filter:+[PrintIssueCards]* -[*Test]* -[PrintIssueCards]*Annotations.*" "-target:RunTests.cmd"
packages\ReportGenerator.2.5.6\tools\ReportGenerator.exe "-reports:results.xml" "-targetdir:.\coverage"