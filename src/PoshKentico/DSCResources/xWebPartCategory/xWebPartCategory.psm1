Import-Module posh-kentico

function Get-TargetResource
{
    [CmdletBinding()]
    [OutputType([System.Collections.Hashtable])]
    param
    (
        [parameter(Mandatory = $true)]
        [System.String]
        $Path
    )

    $webPartCategory = Get-CMSWebPartCategory -Path $Path -Exact
	if ($webPartCategory -ne $null) {
		$Ensure = "Present"
	}
	else {
		$Ensure = "Absent"
	}
	
    $returnValue = @{
		Path = $Path
		Ensure = $Ensure
		DisplayName = $webPartCategory.CategoryDisplayName
		ImagePath = $webPartCategory.CategoryImagePath
    }

    $returnValue
}


function Set-TargetResource
{
    [CmdletBinding()]
    param
    (
        [parameter(Mandatory = $true)]
        [System.String]
        $Path,
		
        [ValidateSet("Present","Absent")]
        [System.String]
        $Ensure,

        [System.String]
        $DisplayName,

        [System.String]
        $ImagePath
    )

	if ([string]::IsNullOrEmpty($Ensure)) {
		$Ensure = "Present"
	}

    $webPartCategory = Get-CMSWebPartCategory -Path $Path -Exact

	if ($Ensure -eq "Present") {
		if ($webPartCategory -ne $null) {
            $webPartCategory.CategoryDisplayName = $DisplayName

			if (-not [string]::IsNullOrEmpty($ImagePath)) {
                $webPartCategory.CategoryImagePath = $ImagePath
            }
            
            $webPartCategory | Set-CMSWebPartCategory
		}
		else {
            New-CMSWebPartCategory -Path $Path -DisplayName $DisplayName -ImagePath $ImagePath
		}
	}
	elseif ($Ensure -eq "Absent") {
		if ($webPartCategory -ne $null) {
            $webPartCategory | Remove-CMSWebPartCategory -Confirm:$false
		}
	}
}


function Test-TargetResource
{
    [CmdletBinding()]
    [OutputType([System.Boolean])]
    param
    (
        [parameter(Mandatory = $true)]
        [System.String]
        $Path,
		
        [ValidateSet("Present","Absent")]
        [System.String]
        $Ensure,

        [System.String]
        $DisplayName,

        [System.String]
        $ImagePath
    )

	if ([string]::IsNullOrEmpty($DisplayName)) {
		$DisplayName = $Path.Substring($Path.LastIndexOf('/') + 1)
	}

    $webPartCategory = Get-CMSWebPartCategory -Path $Path -Exact
	if ($webPartCategory -ne $null) {
		$Ensure -eq "Present" -and
		$DisplayName -eq $webPartCategory.CategoryDisplayName -and
        $ImagePath -eq $webPartCategory.CategoryImagePath
	}
	else {
		$Ensure -eq "Absent"
	}
}


Export-ModuleMember -Function *-TargetResource

