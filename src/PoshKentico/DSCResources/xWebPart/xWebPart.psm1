Import-Module posh-kentico

function Get-NameFromPath
{
    param
    (
        [parameter(Mandatory = $true)]
        [System.String]
        $Path
    )

    $Path.Substring($Path.LastIndexOf('/') + 1)
}

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

	$webPart = Get-CMSWebPart -WebPartPath $Path

	if ($null -ne $webPart) {
		$Ensure = "Present"
	}
	else {
		$Ensure = "Absent"
	}
	
    $returnValue = @{
		Path = $Path
		Ensure = $Ensure
		DisplayName = $webPart.WebPartDisplayName
		FileName = $webPart.WebPartFileName
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

        [parameter(Mandatory = $true)]
        [System.String]
        $FileName
    )

	if ([string]::IsNullOrEmpty($Ensure)) {
		$Ensure = "Present"
	}

    $webPart = Get-CMSWebPart -WebPartPath $Path

    $name = Get-NameFromPath -Path $Path

    if ([string]::IsNullOrEmpty($DisplayName)) {
        $DisplayName = $name
    }

	if ($Ensure -eq "Present") {
		if ($null -ne $webPart) {
			$webPart.WebPartDisplayName = $DisplayName
			$webPart.WebPartFileName = $FileName

			$webPart | Set-CMSWebPart
		}
		else {
			New-CMSWebPart -Path $Path -DisplayName $DisplayName -FileName $FileName
		}
	}
	elseif ($Ensure -eq "Absent") {
		if ($null -ne $webPart) {
			$webPart | Remove-CMSWebPart -Confirm:$false
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

        [parameter(Mandatory = $true)]
        [System.String]
        $FileName
    )

    $name = Get-NameFromPath -Path $Path
    $webPart = Get-CMSWebPart -WebPartPath $Path

	if ([string]::IsNullOrEmpty($DisplayName)) {
		$DisplayName = $name
	}

	if ($null -ne $webPart) {
		$Ensure -eq "Present" -and
		$DisplayName -eq $webPart.WebPartDisplayName -and
		$FileName -eq $webPart.WebPartFileName
	}
	else {
		$Ensure -eq "Absent"
	}
}


Export-ModuleMember -Function *-TargetResource

