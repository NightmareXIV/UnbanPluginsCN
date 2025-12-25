# This program is not relevant anymore. Instead, use Atmo's version of XivLauncher and Dalamud: https://github.com/AtmoOmen/FFXIVQuickLauncher

# 解除CN客户端Ban限制插件
 本插件用于解除中国客户端上，取消对已经被禁止的插件使用。

## 如何使用
- 在启动游戏或者启动卫月之前运行UnbanPluginsCN.exe。
- 按照配置文件的提示进行操作。
- 启动成功后你所有的插件都能正常加载了。

## 作为计划任务运行
- 右键点击`install.ps1`, 选择使用PowerShell运行。
- powershell会申请管理员权限, 放心,这是为了能够将计划任务写入系统。
- 计划任务将会在每次用户登陆后自动运行.

## 删除计划任务
- 右键点击`uninstall.ps1`, 选择使用PowerShell运行。
- powershell会申请管理员权限, 放心,这是为了能够将计划任务删除。

## 可能遇到的问题
- Powershell脚本无法运行
  - 以管理员权限运行Powershell,执行`Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process -Force`

# UnbanPluginsCN
A program that is designed to unrestrict banned plugins usage on Chinese Dalamud client.

## How to use:
- Run UnbanPluginsCN.exe before starting the game
- Follow the instructions in the config file
- All your plugins should be loadable now

## Running as a Scheduled Task
- Right click on `install.ps1`, choose to run with PowerShell
- PowerShell will ask for admin permissions, this is to allow the script to create a scheduled task
- The scheduled task will run every time you log in

## Removing the Scheduled Task
- Right click on `uninstall.ps1`, choose to run with PowerShell

## Possible issues:
- PowerShell script won't run
  - Run PowerShell as an administrator and execute `Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process -Force`


