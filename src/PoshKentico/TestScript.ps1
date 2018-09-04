﻿# Requires -RunAsAdministrator

$VerbosePreference = "Continue"
$DebugPreference = "Continue"
Import-Module posh-kentico

Configuration KenticoTest
{
	Import-DscResource -Name xWebPartCategory
	Import-DscResource -Name xWebPart
	Import-DscResource -Name xSite
	Import-DscResource -Name xServer
	Import-DscResource -Name xSettingValue

	Node localhost
	{
		xWebPart TestWebPart
		{
			Name = "TestWebPart"
			Path = "Kentico:\Development\WebParts\TestCategory"
			Ensure = "Absent"
			FileName = "AbuseReport/AbuseReport.ascx"
		}
		
		xSite TestSite
		{
			SiteName = "Kenticotest"
			DomainName = "localhost:743"
			DisplayName = "KenticoTest"
			Status = "Running"
			Ensure = "Present"
		}

		xServer TestServer
		{
			ServerName = "Kenticotest"
			ServerSiteName = "Kenticotest"
			ServerURL = "http://dappcluster:743"
			ServerDisplayName = "KenticoTest"
			ServerAuthentication = "UserName"
			ServerEnabled = $true
			ServerUsername = "admin"
			ServerPassword = "pass"
			Ensure = "Present"
		}

		xSettingValue TestSettingValue
		{
			Key = "CMSSchedulerTasksEnabled"
			Value = "60"
			SiteName = "Kenticotest"
		}
	}
}

KenticoTest -OutputPath .\Temp

Start-DscConfiguration -Wait -Force -Path .\Temp

