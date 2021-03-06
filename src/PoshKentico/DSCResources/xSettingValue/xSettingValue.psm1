
# DSC uses the Get-TargetResource function to fetch the status of the resource instance specified in the parameters for the target machine
function Get-TargetResource
{
	[CmdletBinding()]
    [OutputType([System.Collections.Hashtable])]
    param
    (
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$Key,

		[parameter(Mandatory = $true)]
        [string]$Value,

        [Parameter(Mandatory = $true)]
        [string]$SiteName
    )

	# Add all setting properties to the hash table
	$getTargetResourceResult = @{
									Key = $Key;
									Value = $Value;
									SiteName = $SiteName;
									WebConfigDefaultValue = $WebConfigDefaultValue;
								}
	
    $getTargetResourceResult;
}

# The Set-TargetResource function is used to create, delete or configure a setting on the target machine.
function Set-TargetResource
{
    [CmdletBinding(SupportsShouldProcess=$true)]
    param
    (
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$Key,

		[parameter(Mandatory = $true)]
        [string]$Value,

        [Parameter(Mandatory = $true)]
        [string]$SiteName
    )

    $oldVal = Get-CMSSettingValue -SiteName $SiteName -Key $Key

	# if existing value not equal to the value, set the value, otherwise do nothing
	if ($oldVal -ne $Value)
    {
		Write-Verbose -Message "Updating the setting $($SettingName)"
        Set-CMSSettingValue -SiteName $SiteName -Key $Key -Value $Value
    }
}

function Test-TargetResource
{
	[CmdletBinding()]
	[OutputType([System.Boolean])]
	 param
    (
        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$Key,

		[parameter(Mandatory = $true)]
        [string]$Value,

        [Parameter(Mandatory = $true)]
        [string]$SiteName
    )

	Write-Verbose "Use this cmdlet to deliver information about command processing."

	Write-Debug "Use this cmdlet to write debug information while troubleshooting."

	$oldVal = Get-CMSSettingValue -SiteName $SiteName -Key $Key

    return $oldVal -eq $Value
}

Export-ModuleMember -Function *-TargetResource