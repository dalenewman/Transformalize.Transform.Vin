nuget pack Transformalize.Transform.Vin.nuspec -OutputDirectory "c:\temp\modules"
nuget pack Transformalize.Transform.Vin.Autofac.nuspec -OutputDirectory "c:\temp\modules"

nuget push "c:\temp\modules\Transformalize.Transform.Vin.0.3.3-beta.nupkg" -source https://api.nuget.org/v3/index.json
nuget push "c:\temp\modules\Transformalize.Transform.Vin.Autofac.0.3.3-beta.nupkg" -source https://api.nuget.org/v3/index.json






