Param($path,$version)

$apikey = "API-NLITTOD95WSOKIT7FLWVSKTGIW"
$octopath = "http://deploy.antix.local"

if ($version -Eq $null) {
    $version = Read-Host "Enter Version Number"
}
if($path -Eq $null){
    $path = Split-Path -parent $PSCommandPath 
    $path = "$path\..\.."
}

Write-Output "begin deploy version $version from $path"

set-alias nuget $path\src\.nuget\NuGet.exe
set-alias octo $path\src\.deploy\Octo.exe

function deploy{
	param($project)

	Get-ChildItem -Path "$path\src\$project\obj\octopacked" -Filter "*.nupkg" -Recurse | ForEach-Object { 

		Write-Output "pushing $_.fullname"

		nuget push $_.fullname -ApiKey $apikey -Source $octopath/nuget/packages

		octo create-release --project=$project --version=$version --packageversion=$version --server=$octopath/api --apikey $apikey
		octo deploy-release --project=$project --version=$version --deployto=Tim --server=$octopath/api --apikey $apikey
	}
}

deploy "Antix.Nuget.Server"