param(
    [string]$InputPath = "$PSScriptRoot\religion_stats_v2.json",
    [string]$OutputPath = "$PSScriptRoot\scrubbed.json"
)

# Keywords that tend to trigger content filters
$sensitivePatterns = @(
    "violence",
    "harm",
    "sacrifice",
    "flay",
    "cannibal",
    "execution",
    "blood",
    "torture",
    "slavery",
    "kill",
    "death",
    "warrior"
)

function Scrub-PSObject {
    param([object]$obj)

    # If it's a PSCustomObject, rebuild it property-by-property
    if ($obj -is [pscustomobject]) {
        $new = [pscustomobject]@{}

        foreach ($prop in $obj | Get-Member -MemberType NoteProperty) {
            $name = $prop.Name
            $lower = $name.ToLower()

            # Skip sensitive keys entirely
            if ($sensitivePatterns | Where-Object { $lower -like "*$_*" }) {
                continue
            }

            # Recursively scrub nested values
            $value = $obj.$name
            $new | Add-Member -NotePropertyName $name -NotePropertyValue (Scrub-PSObject $value)
        }

        return $new
    }

    # If it's an array, scrub each element
    if ($obj -is [System.Collections.IEnumerable] -and $obj -isnot [string]) {
        $newArray = @()
        foreach ($item in $obj) {
            $newArray += Scrub-PSObject $item
        }
        return $newArray
    }

    # Primitive values pass through unchanged
    return $obj
}

# Load JSON
$json = Get-Content $InputPath -Raw | ConvertFrom-Json 

# Scrub it
$scrubbed = Scrub-PSObject $json

# Save output
$scrubbed | ConvertTo-Json -Depth 100 | Set-Content $OutputPath

Write-Host "Scrubbed JSON saved to $OutputPath"