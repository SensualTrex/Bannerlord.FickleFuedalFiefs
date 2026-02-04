# ============================
#  DiskCleanupEx.psm1
#  Full COM + Non-COM Estimator
# ============================

# --- COM Interface Definition ---
Add-Type -Namespace DiskCleanup -Name NativeMethods -MemberDefinition @"
[System.Runtime.InteropServices.ComImport]
[System.Runtime.InteropServices.Guid("8FCE5227-04DA-11d1-A004-00805F8ABE06")]
public class EmptyVolumeCacheCall {}

[System.Runtime.InteropServices.ComImport]
[System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
[System.Runtime.InteropServices.Guid("8FCE5224-04DA-11d1-A004-00805F8ABE06")]
public interface IEmptyVolumeCache
{
    void Initialize(
        [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
        string pszVolume,
        out System.IntPtr ppwszDisplayName,
        out System.IntPtr ppwszDescription,
        out uint pdwFlags);

    void GetSpaceUsed(out ulong pdwSpaceUsed, System.IntPtr hWnd, uint dwFlags);

    void Purge(ulong dwSpaceToFree, System.IntPtr hWnd, out ulong pdwSpaceFreed);

    void ShowProperties(System.IntPtr hWnd);
}
"@

# --- Enumerate ALL handlers ---
function Get-DiskCleanupHandlers {
    $results = @()

    $paths = @(
        "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches",
        "HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Explorer\VolumeCaches"
    )

    foreach ($path in $paths) {
        if (Test-Path $path) {
            foreach ($key in Get-ChildItem $path) {
                $props = Get-ItemProperty $key.PSPath

                $type =
                    if ($props.CLSID) { "COM" }
                    elseif ($props.Folder -or $props.FileList) { "FolderBased" }
                    else { "Unknown" }

                $results += [PSCustomObject]@{
                    Name        = $key.PSChildName
                    Type        = $type
                    CLSID       = $props.CLSID
                    Dll         = $props.Dll
                    Folder      = $props.Folder
                    FileList    = $props.FileList
                    Flags       = $props.Flags
                    Priority    = $props.Priority
                    RegPath     = $key.PSPath
                }
            }
        }
    }

    return $results
}

# --- Folder size estimator ---
function Get-FolderEstimate {
    param([string[]]$Folders)

    $total = 0

    foreach ($folder in $Folders) {
        $expanded = $folder -replace "\?\:", (Get-PSDrive -PSProvider FileSystem | Select-Object -First 1).Root

        if (Test-Path $expanded) {
            $total += (Get-ChildItem $expanded -Recurse -Force -ErrorAction SilentlyContinue |
                       Measure-Object Length -Sum).Sum
        }
    }

    return $total
}

# --- FileList estimator ---
function Get-FileListEstimate {
    param(
        [string[]]$Folders,
        [string[]]$Patterns
    )

    $total = 0

    foreach ($folder in $Folders) {
        $expanded = $folder -replace "\?\:", (Get-PSDrive -PSProvider FileSystem | Select-Object -First 1).Root

        if (Test-Path $expanded) {
            foreach ($pattern in $Patterns) {
                $total += (Get-ChildItem $expanded -Recurse -Force -Filter $pattern -ErrorAction SilentlyContinue |
                           Measure-Object -ErrorAction SilentlyContinue Length -Sum).Sum
            }
        }
    }

    return $total
}

# --- COM handler estimator ---
function Get-ComEstimate {
    param([string]$CLSID)

    try {
        $type = [Type]::GetTypeFromCLSID($CLSID)
        $obj  = [Activator]::CreateInstance($type)

        $displayPtr = [IntPtr]::Zero
        $descPtr    = [IntPtr]::Zero
        $flags      = 0

        $obj.Initialize("C:\", [ref]$displayPtr, [ref]$descPtr, [ref]$flags)

        $space = 0
        $obj.GetSpaceUsed([ref]$space, [IntPtr]::Zero, 0)

        return $space
    }
    catch {
        return 0
    }
}

# --- Unified Estimator ---
function Get-DiskCleanupEstimates {
    $handlers = Get-DiskCleanupHandlers
    $results = @()

    foreach ($h in $handlers) {
        $bytes = 0

        switch ($h.Type) {
            "COM" {
                $bytes = Get-ComEstimate -CLSID $h.CLSID
            }

            "FolderBased" {
                $folders = $h.Folder -split "\|"
                $patterns = $h.FileList -split "\|"

                if ($h.FileList) {
                    $bytes = Get-FileListEstimate -Folders $folders -Patterns $patterns
                }
                elseif ($h.Folder) {
                    $bytes = Get-FolderEstimate -Folders $folders
                }
            }

            default {
                $bytes = 0
            }
        }

        $results += [PSCustomObject]@{
            Name        = $h.Name
            Type        = $h.Type
            EstimatedMB = [math]::Round($bytes / 1MB, 2)
            Bytes       = $bytes
        }
    }

    return $results | Sort-Object EstimatedMB -Descending
}

$results=Get-DiskCleanupEstimates
echo $results