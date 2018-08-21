
# DSC uses the Get-TargetResource function to fetch the status of the resource instance specified in the parameters for the target machine
function Get-TargetResource
{
	[CmdletBinding()]
    [OutputType([System.Collections.Hashtable])]
    param
    (
		[ValidateSet("Present", "Absent")]
        [string]$Ensure = "Present",

        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerName,

		[parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerSiteID,

        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerDisplayName,

        [Parameter(Mandatory)]
        [bool]$ServerEnabled = "True",

		[ValidateSet("UserName", "X509")]
        [string]$ServerAuthentication = "UserName",

		[Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerURL,

		[Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerUsername,

		[Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerPassword
    )

	<# Insert logic that uses the mandatory parameter values to get the server and assign it to a variable called $Server #>
    <# Set $ensureResult to "Present" if the requested server exists and to "Absent" otherwise #>
	$Server = Get-CMSServer -SiteID $ServerSiteID -ServerName $ServerName -Exact
	if ($Server -ne $null) {
		$Ensure = "Present"
	}
	else {
		$Ensure = "Absent"
	}

    $getTargetResourceResult = $null;
    
    if ($Ensure)
    {
		# $Server is not null, Add all server properties to the hash table
		$getTargetResourceResult = @{
										Ensure = "Present";
										ServerName = $Server.ServerName;
										ServerSiteID = $Server.ServerSiteID;
										ServerDisplayName = $Server.ServerDisplayName;
										ServerEnabled = $Server.ServerEnabled;
										ServerURL = $Server.ServerURL;
										ServerAuthentication = $Server.ServerAuthentication;
										ServerUsername = $Server.ServerUsername;
										ServerPassword = $Server.ServerPassword;
									}
	}
	else
	{
		# $Server is null, Add all server properties to the hash table
		$getTargetResourceResult = @{
										Ensure = "Absent";
										SServerName = $Server.ServerName;
										ServerSiteID = $Server.ServerSiteID;
										ServerDisplayName = $Server.ServerDisplayName;
										ServerEnabled = $Server.ServerEnabled;
										ServerURL = $Server.ServerURL;
										ServerAuthentication = $Server.ServerAuthentication;
										ServerUsername = $Server.ServerUsername;
										ServerPassword = $Server.ServerPassword;
									}
	}

    $getTargetResourceResult;
}

# The Set-TargetResource function is used to create, delete or configure a server on the target machine.
function Set-TargetResource
{
    [CmdletBinding(SupportsShouldProcess=$true)]
    param
    (
        [ValidateSet("Present", "Absent")]
        [string]$Ensure,

        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerName,

		[parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerSiteID,

        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerDisplayName,

        [Parameter(Mandatory)]
        [bool]$ServerEnabled,

		[ValidateSet("UserName", "X509")]
        [string]$ServerAuthentication,

		[Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerURL,

		[Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerUsername,

		[Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerPassword
    )

    <# If Ensure is set to "Present" and the server specified in the mandatory input parameters does not exist, then create it using the specified parameter values #>
    <# Else, if Ensure is set to "Present" and the server does exist, then update its properties to match the values provided in the non-mandatory parameter values #>
    <# Else, if Ensure is set to "Absent" and the server does not exist, then do nothing #>
    <# Else, if Ensure is set to "Absent" and the server does exist, then delete the server #>
	$serverExists = Get-CMSServer -SiteID $ServerSiteID -ServerName $ServerName -Exact

	if ($Ensure -eq "Present")
    {
        if(-not $serverExists)
        {
			Write-Verbose -Message "Creating the server $($ServerName)"
            New-CMSServer -SiteID $ServerSiteID -DisplayName $ServerDisplayName -ServerName $ServerName -URL $ServerURL -Authentication $ServerAuthentication -Enabled $ServerEnabled -UserName $ServerUsername -Password $ServerPassword
        }
		else
		{
			Write-Verbose -Message "Updating the server $($ServerName)"
			Set-CMSServer -ServerName $ServerName -SiteID $ServerSiteID -DisplayName $ServerDisplayName -URL $ServerURL -ServerName $ServerName -Authentication $ServerAuthentication -Enabled $ServerEnabled -UserName $ServerUsername -Password $ServerPassword
		}
    }
    else
    {
        if ($serverExists)
        {
            Write-Verbose -Message "Deleting the server $($ServerName)"
            Remove-CMSServer -SiteID $ServerSiteID -ServerName $ServerName -Exact
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
        [string]$Ensure,

        [parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerName,

		[parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerSiteID,

        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerDisplayName,

        [Parameter(Mandatory)]
        [bool]$ServerEnabled,

		[ValidateSet("UserName", "X509")]
        [string]$ServerAuthentication,

		[Parameter(Mandatory = $true)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerURL,

		[Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerUsername,

		[Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$ServerPassword
    )

	Write-Verbose "Use this cmdlet to deliver information about command processing."

	Write-Debug "Use this cmdlet to write debug information while troubleshooting."

	$serverExists = Get-CMSServer -SiteID $ServerSiteID -ServerName $ServerName -Exact

    if ($Ensure -eq "Present")
    {
        if(-not $serverExists)
        {
			return $false
        }
		else
		{
			return $true
		}
    }
    else
    {
        if(-not $serverExists)
        {
			return $true
        }
		else
		{
			return $false
		}
    }
}

Export-ModuleMember -Function *-TargetResource