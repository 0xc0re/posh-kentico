Import-Module posh-kentico

function Get-TargetResource
{
    [CmdletBinding()]
    [OutputType([System.Collections.Hashtable])]
    param
    (
        [parameter(Mandatory = $true)]
        [System.String]
        $Name,

        [parameter(Mandatory = $true)]
        [System.String]
        $Path,

        [parameter(Mandatory = $true)]
        [System.String]
        $FileName
    )

	# Ignore $FileName.  We want it to be required when setting the value.

	$fullPath = "$Path\$Name"

	if (Test-Path $fullPath) {
		$Ensure = "Present"
	}
	else {
		$Ensure = "Absent"
	}
	
	$properties = Get-ItemProperty -Path $fullPath
    $returnValue = @{
		Name = $Name
		Path = $Path
		Ensure = $Ensure
		DisplayName = $properties.DisplayName
		FileName = $properties.FileName
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
        $Name,

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

	$fullPath = "$Path\$Name"

	if ([string]::IsNullOrEmpty($Ensure)) {
		$Ensure = "Present"
	}

	if ([string]::IsNullOrEmpty($DisplayName)) {
		$DisplayName = $Name
	}

	if ($Ensure -eq "Present") {
		if (Test-Path $fullPath) {
			Set-ItemProperty -Path $Path -Name DisplayName -Value $DisplayName
			Set-ItemProperty -Path $Path -Name FileName -Value $FileName
		}
		else {
			New-Item -ItemType WebPart -Path $Path -Name $Name -DisplayName $DisplayName -FileName $FileName
		}
	}
	elseif ($Ensure -eq "Absent") {
		if (Test-Path $fullPath) {
			Remove-Item $fullPath -Recurse
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
        $Name,

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

	$fullPath = "$Path\$Name"

	if ([string]::IsNullOrEmpty($DisplayName)) {
		$DisplayName = $Name
	}

	if (Test-Path $fullPath) {
		$properties = Get-ItemProperty -Path $fullPath

		$Ensure -eq "Present" -and
		$DisplayName -eq $properties.DisplayName -and
		$FileName -eq $properties.FileName
	}
	else {
		$Ensure -eq "Absent"
	}
}


Export-ModuleMember -Function *-TargetResource

