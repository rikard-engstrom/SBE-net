# Requires Nuget packages OpenCover, ReportGenerator and NUnit Console Runner to be included in the project.
# Build project before executing script.

Remove-Item CodeCoverageReport -Recurse -ErrorAction Ignore

$openCover = Get-ChildItem packages -Name -recurse OpenCover.Console.exe
$reportGenerator = Get-ChildItem packages -Name -recurse ReportGenerator.exe
$nunit = Get-ChildItem packages -Name -recurse nunit3-console.exe

$testbins="SBE._Nunit.Tests\bin\Debug\SBE._NUnit.Tests.dll"
$filter="+[*]* -[Microsoft.*]*"

&"packages\$openCover" -register:user -target:"packages\$nunit" -targetargs:$testbins -filter:$filter -showunvisited
&"packages\$reportGenerator" -reports:results.xml -targetdir:CodeCoverageReport -reporttypes:Html