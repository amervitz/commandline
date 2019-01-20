# Prerequisites before using:
# 1. add nuget.exe to folder in the system path
# 2. create the MYGET_API_KEY environment variable

$projectFile = [xml](Get-Content -Path .\src\commandline\commandline.csproj -Raw)
$propertyGroup = $projectFile.Project.PropertyGroup

nuget push ".\src\commandline\bin\Debug\$($propertyGroup.PackageId).$($propertyGroup.Version).nupkg" $env:MYGET_API_KEY -Source https://www.myget.org/F/amervitz/api/v2/package