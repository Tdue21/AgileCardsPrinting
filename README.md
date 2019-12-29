[![Build Status](https://duenet.visualstudio.com/AgileCardPrint/_apis/build/status/Tdue21.AgileCardsPrinting?branchName=master)](https://duenet.visualstudio.com/AgileCardPrint/_build/latest?definitionId=2&branchName=master)
[![Build status](https://ci.appveyor.com/api/projects/status/1h11leogfgrj17a6?svg=true)](https://ci.appveyor.com/project/tdue21/agilecardsprinting)


# README

PrintIssueCards is a simple application for creating issue cards for a scrum or kanban like board. 
The application fetches Jira issues from either a filter, a list of issue keys or a JQL expression.

## Dependencies & Requirements

* [Atlassian.SDK](https://bitbucket.org/farmas/atlassian.net-sdk/wiki/Home) - Excellent library for connecting to the REST api of a Jira instance.
* [DevExpress.MVVM.Free](https://github.com/DevExpress/DevExpress.Mvvm.Free) - Free and Open-Source edition of the excellent DevExpress MVVM Framework. 
* [Microsoft ReportViewer](https://www.microsoft.com/en-us/download/details.aspx?id=6610) - Although technically a Winforms component, the ReportViewer is easy to use even in WPF and has great designer support. 
* [Unity Container](https://github.com/unitycontainer/unity) - Easy to use and popular Dependency Injection Container.
* [Json.NET](http://www.newtonsoft.com/json) - The best .NET library on the marked for reading and writing JSON. 

## License
The source code of the application is licensed under the [MIT License](LICENSE).
