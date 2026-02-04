
# Paths to your files
$lorePath = "$PSScriptRoot\religion_lore.json"
$statsPath = "$PSScriptRoot\religion_stats_v2.json"

# Load JSON
$lore = Get-Content $lorePath -Raw | ConvertFrom-Json
$stats = Get-Content $statsPath -Raw | ConvertFrom-Json

# Flatten into comparable objects
$loreRows = $lore.Religions | Select-Object @{n='Id';e={$_.Id}}, @{n='Name';e={$_.Name}}
$statsRows = $stats.ReligionStats | Select-Object @{n='Id';e={$_.StringId}}, @{n='Name';e={$_.Name}}

# Build hash tables for quick lookup
$loreHash = @{}
foreach ($row in $loreRows) { $loreHash[$row.Id] = $row }

$statsHash = @{}
foreach ($row in $statsRows) { $statsHash[$row.Id] = $row }

# Collect all unique keys
$allKeys = ($loreHash.Keys + $statsHash.Keys) | Sort-Object -Unique

# Perform the full outer join
$result = foreach ($key in $allKeys) {
    [PSCustomObject]@{
        LoreId   = if ($loreHash.ContainsKey($key)) { $loreHash[$key].Id } else { $null }
        LoreName = if ($loreHash.ContainsKey($key)) { $loreHash[$key].Name } else { $null }
        StatsId  = if ($statsHash.ContainsKey($key)) { $statsHash[$key].Id } else { $null }
        StatsName= if ($statsHash.ContainsKey($key)) { $statsHash[$key].Name } else { $null }
    }
}

# Output as a table
$result | Format-Table -AutoSize