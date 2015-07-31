# Escc.Umbraco.HeatmapAnalytics

This project allows you to manage within Umbraco the URLs that a heatmap analytics service like Crazy Egg will be applied to.

Create a content node using the 'Heatmap analytics' document type, add the URLs where you want to enable heatmap analytics, then request the data from the following URL:

	https://hostname:port/umbraco/api/HeatmapAnalytics/GetHeatmapAnalyticsUrls

## Using NuGet to include these templates in Umbraco

This project references the [Umbraco CMS NuGet package](https://www.nuget.org/packages/UmbracoCms) and is a working Umbraco installation, but this is just for testing the templates in a development environment. In production this project is published as a NuGet package to our private feed, and consumed by our main Umbraco application. We use [NuBuild](https://github.com/bspell1/NuBuild) to make creating the NuGet package really easy, and [reference our private feed using a nuget.config file](http://blog.davidebbo.com/2014/01/the-right-way-to-restore-nuget-packages.html).

When you install the package into another Umbraco project it copies in the views and references the DLL output. These are excluded from the consuming project's git respository by its `.gitignore` file. 

However, when the consuming project is deployed NuGet restore ensures the DLL reference still works, but it doesn't restore the views because [package restore doesn't restore content files](http://jeffhandley.com/archive/2013/12/09/nuget-package-restore-misconceptions.aspx). To work around this, the NuGet package includes a `.targets` file. NuGet automatically includes this as an MSBuild step in the `.csproj` file of the consuming project, which allows the `.targets` file to copy the views into the project at build time.

## Development setup steps

1. Install [NuBuild](https://github.com/bspell1/NuBuild)
2. From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS.
3. Build the solution
4. Grant modify permissions to the application pool account on the web root folder and all children
7. In `~\web.config` set the `UmbracoConfigurationStatus` and `umbracoDbDSN`, or run the Umbraco installer.
8. At a command line, run the following two commands to add the document types to Umbraco (substitute the hostname and port where you set up this project):

		curl --insecure -X POST -d "" https://hostname:port/umbraco/api/HeatmapAnalytics/CreateUmbracoSupportingTypes?token=
		curl --insecure -X POST -d "" https://hostname:port/umbraco/api/HeatmapAnalytics/CreateUmbracoDocumentTypes?token=