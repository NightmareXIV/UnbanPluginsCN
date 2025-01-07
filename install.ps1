[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
# Check if running as administrator
if (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator"))  
{  
    $arguments = "& '" + $myinvocation.mycommand.definition + "'"
    Start-Process powershell -Verb runAs -ArgumentList $arguments
    exit
}
# Get current user
$currentUser = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name
# Set execution policy
Set-ExecutionPolicy Bypass -Scope Process -Force

# Set working directory to script location
$workDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$exePath = "`"" + (Join-Path $workDir "UnbanPluginsCN.exe") + "`""

# Create task action
$Action = New-ScheduledTaskAction -Execute $exePath -WorkingDirectory $workDir

# Create logon trigger for all users
$Trigger = New-ScheduledTaskTrigger -AtLogOn -RandomDelay (New-TimeSpan -Minutes 1)

# Set run level to highest
$Principal = New-ScheduledTaskPrincipal -UserId $currentUser `
    -LogonType ServiceAccount `
    -RunLevel Highest

# Configure task settings
$Settings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries `
    -DontStopIfGoingOnBatteries `
    -ExecutionTimeLimit (New-TimeSpan -Hours 72) `
    -MultipleInstances IgnoreNew `
    -Priority 7
    
# Register scheduled task and capture result
try {
    $result = Register-ScheduledTask -TaskName "UnbanCNplugins-service" `
        -TaskPath "\" `
        -Description "Remove CN plugin ban list" `
        -Action $Action `
        -Trigger $Trigger `
        -Principal $Principal `
        -Settings $Settings `
        -Force

    if ($result) {
        Write-Host "Scheduled task created successfully!" -ForegroundColor Green
        Write-Host "Task information:"
        Write-Host "- Name: $($result.TaskName)"
        Write-Host "- State: $($result.State)"
        Write-Host "- Next Run Time: $($result.NextRunTime)"
    }
} catch {
    Write-Host "Error creating scheduled task:" -ForegroundColor Red
    Write-Host $_.Exception.Message
}

# Pause to allow user to view results
Write-Host "`nPress any key to continue..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")