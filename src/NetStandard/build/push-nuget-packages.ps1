param (
  [Parameter(Mandatory=$true)]
  [string]
  [switch]
  $FilePath,
  [Parameter(Mandatory=$false)]
  [string]
  $Arguments,
  [Parameter(Mandatory=$false)]
  [bool]
  $ContinueOnError = $false
)

$files = Get-ChildItem $FilePath -R

if ($files.length -eq 0)
{
  Write-Host "No packages found"
} else
{
  foreach ($file in $files)
  {
    Try
    {
      Invoke-Expression "& nuget push $($file.FullName) $Arguments"
    } Catch
    {
      if ($ContinueOnError)
      {
        Write-Host $_.Exception.Message
      } else
      {
        throw $_.Exception;
      }
    }
  }
}
