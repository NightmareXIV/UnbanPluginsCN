# ���CN�ͻ���Ban���Ʋ��
 ��������ڽ���й��ͻ����ϣ�ȡ�����Ѿ�����ֹ�Ĳ��ʹ�á�

## ���ʹ��
- ��������Ϸ������������֮ǰ����UnbanPluginsCN.exe��
- ���������ļ�����ʾ���в�����
- �����ɹ��������еĲ���������������ˡ�

## ��Ϊ�ƻ���������
- �Ҽ����`install.ps1`, ѡ��ʹ��PowerShell���С�
- powershell���������ԱȨ��, ����,����Ϊ���ܹ����ƻ�����д��ϵͳ��
- �ƻ����񽫻���ÿ���û���½���Զ�����.

## ɾ���ƻ�����
- �Ҽ����`uninstall.ps1`, ѡ��ʹ��PowerShell���С�
- powershell���������ԱȨ��, ����,����Ϊ���ܹ����ƻ�����ɾ����

## ��������������
- Powershell�ű��޷�����
  - �Թ���ԱȨ������Powershell,ִ��`Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process -Force`

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


