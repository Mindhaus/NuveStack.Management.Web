$results = @{}
$results.Add("CLR Version", $PSVersionTable.ClrVersion)
$results.Add("PS Version", $PSVersionTable.PSVersion)
$results.Add("Machine Name", $env:MachineName)
$arch = ""
if([IntPtr]::size -eq 4)
{
 $arch =  "32 Bit"
}
else
{
 $arch = "64 Bit"
}
$results.Add("Architecture", $arch);
$results