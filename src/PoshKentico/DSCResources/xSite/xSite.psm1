
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
	$site = Get-CMSSite -SiteName $SiteName -Exact

    $getTargetResourceResult = $null;
    
    if ($site -ne $null)
    {
		# $Site is not null, Add all Website properties to the hash table
		$getTargetResourceResult = @{
										Ensure = "Present";
										SiteName = $site.SiteName;
										DisplayName = $site.DisplayName;
										DomainName = $site.DomainName;
										Status = $site.Status;
									}
	}
	else
	{
		# $Site is null, Add all Website properties to the hash table
		$getTargetResourceResult = @{
										Ensure = "Absent";
										SiteName = $null;
										DisplayName = $null;
										DomainName = $null;
										Status = $null;
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
	$site = Get-CMSSite -SiteName $SiteName -Exact

	if ($Ensure -eq "Present")
    {
        if ($site -eq $null)
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
        if ($site -ne $null)
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

	$site = Get-CMSSite -SiteName $SiteName -Exact

    if ($Ensure -eq "Present")
    {
        return $null -ne $site
    }
    else
    {
        return $null -eq $site
    }
}

Export-ModuleMember -Function *-TargetResource