
# DSC uses the Get-TargetResource function to fetch the status of the resource instance specified in the parameters for the target machine
function Get-TargetResource
{
	[CmdletBinding()]
    [OutputType([System.Collections.Hashtable])]
    param
    (
		[ValidateSet("Present", "Absent")]
		[string]$Ensure = "Present",

		[Parameter(Mandatory = $true)]
		[ValidateNotNullOrEmpty()]
		[string]$SiteName,

		[Parameter(Mandatory)]
		[ValidateNotNullOrEmpty()]
		[string]$DomainName,

		[ValidateSet("Running", "Stopped")]
		[string]$Status = "Running",

		[Parameter(Mandatory)]
		[ValidateNotNullOrEmpty()]
		[string]$DisplayName
    )

	<# Insert logic that uses the mandatory parameter values to get the site and assign it to a variable called $Site #>
    <# Set $ensureResult to "Present" if the requested site exists and to "Absent" otherwise #>
	$Site = Get-CMSSite -SiteName $SiteName -Exact
	if ($Site -ne $null) {
		$Ensure = "Present"
	}
	else {
		$Ensure = "Absent"
	}

    $getTargetResourceResult = $null;
    
    if ($Ensure)
    {
		# $Site is not null, Add all Website properties to the hash table
		$getTargetResourceResult = @{
										Ensure = "Present";
										SiteName = $Site.SiteName;
										DisplayName = $Site.DisplayName;
										DomainName = $Site.DomainName;
										Status = $Site.status;
									}
	}
	else
	{
		# $Site is null, Add all Website properties to the hash table
		$getTargetResourceResult = @{
										Ensure = "Absent";
										SiteName = $Site.SiteName;
										DisplayName = $Site.DisplayName;
										DomainName = $Site.DomainName;
										Status = $Site.status;
									}
	}

    $getTargetResourceResult;
}

# The Set-TargetResource function is used to create, delete or configure a site on the target machine.
function Set-TargetResource
{
    [CmdletBinding(SupportsShouldProcess=$true)]
    param
    (
        [ValidateSet("Present", "Absent")]
        [string]$Ensure = "Present",

        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$SiteName,

        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$DomainName,

        [ValidateSet("Running", "Stopped")]
        [string]$Status = "Running",

		[Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$DisplayName
    )

    <# If Ensure is set to "Present" and the site specified in the mandatory input parameters does not exist, then create it using the specified parameter values #>
    <# Else, if Ensure is set to "Present" and the site does exist, then update its properties to match the values provided in the non-mandatory parameter values #>
    <# Else, if Ensure is set to "Absent" and the site does not exist, then do nothing #>
    <# Else, if Ensure is set to "Absent" and the site does exist, then delete the site #>
	$siteExists = Get-CMSSite -SiteName $SiteName -Exact

	if ($Ensure -eq "Present")
    {
        if(-not $siteExists)
        {
			Write-Verbose -Message "Creating the site $($SiteName)"
            New-CMSSite -DisplayName $DisplayName -SiteName $SiteName -Status $Status -DomainName $DomainName
        }
		else
		{
			Write-Verbose -Message "Updating the site $($SiteName)"
			Set-CMSSite -DisplayName $DisplayName -SiteName $SiteName -Status $Status -DomainName $DomainName	
		}
    }
    else
    {
        if ($siteExists)
        {
            Write-Verbose -Message "Deleting the site $($SiteName)"
            Remove-CMSSite -SiteName $SiteName -Exact
        }
    }
}

function Test-TargetResource
{
	[CmdletBinding()]
	[OutputType([System.Boolean])]
	 param
    (
        [ValidateSet("Present", "Absent")]
        [string]$Ensure = "Present",

        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$SiteName,

        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$DomainName,

        [ValidateSet("Running", "Stopped")]
        [string]$Status = "Running",

		[Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$DisplayName
    )

	Write-Verbose "Use this cmdlet to deliver information about command processing."

	Write-Debug "Use this cmdlet to write debug information while troubleshooting."

	$present = Get-CMSSite -SiteName $SiteName -Exact

    if ($Ensure -eq "Present")
    {
        return $present
    }
    else
    {
        return -not $present
    }
}

Export-ModuleMember -Function *-TargetResource