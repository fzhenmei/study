REM This batch file updates the ..\Assemblies and template directories
REM with the latest copies of the core ..\Assemblies
copy ..\WestwindWebToolkitWeb\bin\Westwind.Web.dll ..\Assemblies
copy ..\WestwindWebToolkitWeb\bin\Westwind.Web.xml ..\Assemblies
copy ..\WestwindWebToolkitWeb\bin\Westwind.Utilities.dll ..\Assemblies
copy ..\WestwindWebToolkitWeb\bin\Westwind.Utilities.xml ..\Assemblies
copy ..\WestwindWebToolkitWeb\bin\Westwind.BusinessFramework.dll ..\Assemblies
copy ..\WestwindWebToolkitWeb\bin\Westwind.BusinessFramework.xml ..\Assemblies
copy ..\Westwind.GlobalizationWeb\bin\Westwind.Globalization.dll ..\Assemblies
copy ..\Westwind.GlobalizationWeb\bin\Westwind.Globalization.xml ..\Assemblies

copy ..\WestwindWebToolkitWeb\bin\Westwind.Web.dll ..\WestWindWebToolkitTemplate\bin
copy ..\WestwindWebToolkitWeb\bin\Westwind.Web.xml ..\WestWindWebToolkitTemplate\bin
copy ..\WestwindWebToolkitWeb\bin\Westwind.Utilities.dll ..\WestWindWebToolkitTemplate\bin
copy ..\WestwindWebToolkitWeb\bin\Westwind.Utilities.xml ..\WestWindWebToolkitTemplate\bin
copy ..\Westwind.GlobalizationWeb\bin\Westwind.Globalization.dll ..\WestWindWebToolkitTemplate\bin
copy ..\Westwind.GlobalizationWeb\bin\Westwind.Globalization.xml ..\WestWindWebToolkitTemplate\bin
copy ..\WestwindWebToolkitWeb\bin\Westwind.BusinessFramework.dll ..\WestWindWebToolkitTemplate\bin
copy ..\WestwindWebToolkitWeb\bin\Westwind.BusinessFramework.xml ..\WestWindWebToolkitTemplate\bin

pause