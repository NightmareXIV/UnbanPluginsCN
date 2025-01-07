# 检查管理员权限
if (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator"))  
{  
    $arguments = "& '" + $myinvocation.mycommand.definition + "'"
    Start-Process powershell -Verb runAs -ArgumentList $arguments
    exit
}

# 设置执行策略
Set-ExecutionPolicy Bypass -Scope Process -Force

# 删除计划任务并捕获结果
try {
    $result = Unregister-ScheduledTask -TaskName "UnbanCNplugins-service" -Confirm:$false
    Write-Host "Uninstall Successed" -ForegroundColor Green
} catch {
    Write-Host "Error::" -ForegroundColor Red
    Write-Host $_.Exception.Message
}

# 暂停让用户查看结果
Write-Host "`nPress any key to continue..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")