# Test-Analytics : An Implementation of Google Test-Analytics via Asp.Net Core.

I could have compiled the GTA but it seems the [Google Test-Analytics Source Code](https://code.google.com/archive/p/test-analytics/source) 
could not be built properly for some referenced java components are no longer available since the decay of code.google.com. Maybe, really I dont know. Besides, the [app website](https://test-analytics.appspot.com) could not be accessed from here, at least here China.

So I decided to write my own Test-Analytics. There are some features already implemented.

* ACC Add Delete
* View risk table 
* Download risk table as .xls

Due to the lack of Authorization, features below might come later.

* Project Sharing( not support )
* Make project public( not support )
* locate features( English only )

## Building
Firstly, you need to add some nuget package reference.
```
$dotnet add package BuildBundlerMinifier
$dotnet add package Serilog
$dotnet add package Serilog.Console
$dotnet add package Serilog.File
```
Modify the listening IP.
```
#launchSettings.json
...
  "applicationUrl":"https://your-ip:port1;your-ip:port2",
...
```

## Web APIs
 
```
[base route]
/project

[goto index page]
/project/index

[learn more page]
/project/learn-more

[save & delete stuffs] {
  [save project]
  /project/save-project

  [delete porject]
  /project/delete-project

  [save project attributes components capabilities]
  /project/save-project-attrs
  /project/save-project-comps
  /project/save-project-capas
}

[download xls risk table]
/project/make-risktable-xls     #which will make a xls file on remote server.
/project/download-risktable-xls #download xls file.
```
 
Good Luck!
