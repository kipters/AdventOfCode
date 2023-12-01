param (
  [Parameter(Mandatory = $true)][int]$year
)

$folder = "Year$year"
$path = [System.IO.Path]::Combine("src", $folder)
$csprojPath = [System.IO.Path]::Combine($path, "$folder.csproj")

$csproj =
@"
<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <ProjectReference Include="..\Utilities\Utilities.csproj" />
  </ItemGroup>
</Project>

"@

$testTemplate =
@"
namespace $folder;

public class Day{0}
{
    //[Theory]
    //[FileLines("data_sample.txt", 0)]
    //[FileLines("data.txt", 0)]
    public void Part1(IEnumerable<string> data, int result)
    {
        throw new NotImplementedException();
    }

    //[Theory]
    //[FileLines("data_sample.txt", 0)]
    //[FileLines("data.txt", 0)]
    public void Part2(IEnumerable<string> data, int result)
    {
        throw new NotImplementedException();
    }
}

"@


if (Test-Path -Path $path) {
  Write-Output "Folder already exists"
  Remove-Item -Path $path -Force -Recurse
  # exit
}

New-Item -ItemType Directory -Path $path
New-Item -ItemType File -Path $csprojPath -Value $csproj
New-Item -ItemType File -Path ([System.IO.Path]::Combine($path, "GlobalUsings.cs")) -Value "global using Xunit;`n"

for ($i = 1; $i -lt 26; $i++) {
  $dayPath = [System.IO.Path]::Combine($path, "Day$i")
  New-Item -ItemType Directory -Path $dayPath

  New-Item -ItemType File -Path ([System.IO.Path]::Combine($dayPath, "data.txt"))
  New-Item -ItemType File -Path ([System.IO.Path]::Combine($dayPath, "data_sample.txt"))

  $text = $testTemplate.Replace("{0}", $i)
  New-Item -ItemType File -Path ([System.IO.Path]::Combine($dayPath, "Day$i.cs")) -Value $text
}

dotnet sln add $csprojPath
