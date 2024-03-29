#### SMARTY DISCLAIMER: Subject to the terms of the associated license agreement, this software is freely available for your use. This software is FREE, AS IN PUPPIES, and is a gift. Enjoy your new responsibility. This means that while we may consider enhancement requests, we may or may not choose to entertain requests at our sole and absolute discretion.

# Smarty SQL Server Plugin

### Table of Contents
1. [Requirements](#requirements)
2. [Setup](#setup)
3. [Supported Functions](#supported-functions)
4. [Auth Setup](#auth-setup)
5. [Examples](#examples)
6. [Building Library From Source](#building-library-from-scratch)
7. [Contact](#contact)

## Requirements

Compatible with SQL Server 2012, 2014, 2016, 2017, 2019, and 2022 for Windows.

## Setup
   
Download `SmartySqlServerPlugin-merged.dll` from the [releases](https://github.com/smarty/smarty-sql-server-plugin/releases/latest/) page. 

Set necessary configuration settings with the following commands:
```sql
sp_configure 'show advanced options', 1;
GO

RECONFIGURE;
GO

sp_configure 'clr enabled', 1;
GO

RECONFIGURE;
GO

-- comment the following line out if using SQL Server 2012, 2014, or 2016
sp_configure 'clr strict security', 0;
GO

RECONFIGURE;
GO
```

Set up the database:

```sql
USE [master];
GO 

ALTER DATABASE [TestDatabase] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO 

DROP DATABASE [TestDatabase];
GO

CREATE DATABASE [TestDatabase];
GO

USE [TestDatabase];
GO

ALTER DATABASE [TestDatabase] SET TRUSTWORTHY ON
GO
```

Create the assembly for dependency System Runtime Serialization:

```sql
CREATE ASSEMBLY [System.Runtime.Serialization] FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Runtime.Serialization.dll' WITH PERMISSION_SET = UNSAFE;
GO
```

Create the assembly for the Smarty plugin:

```sql
CREATE ASSEMBLY [SmartySqlServerPlugin] FROM "C:\Path\to\plugin\SmartySqlServerPlugin-merged.dll" WITH PERMISSION_SET = EXTERNAL_ACCESS
```

Create any of the supported functions (see [below](#functions)) that you would like to use:

```sql
CREATE FUNCTION SmartyUSStreetVerifyFreeform 
(
    @freeform nvarchar(max),
    @max_candidates int,
    @match_strategy nvarchar(max),
    @authid nvarchar(max),
    @authtoken nvarchar(max),
    @url nvarchar(max)
)

RETURNS TABLE
(
    [status] int NULL,
    candidate int NULL,
    input_id nvarchar(1024) NULL,
    input_index int NULL,
    candidate_index int NULL,
    addressee nvarchar(1024) NULL,
    delivery_line_1 nvarchar(1024) NULL,
    delivery_line_2 nvarchar(1024) NULL,
    lastline nvarchar(1024) NULL,
    delivery_point_barcodeout nvarchar(1024) NULL,
    components_urbanization nvarchar(1024) NULL,
    components_primary_number nvarchar(1024) NULL,
    components_street_name nvarchar(1024) NULL,
    components_street_predirection nvarchar(1024) NULL,
    components_street_postdirection nvarchar(1024) NULL,
    components_street_suffix nvarchar(1024) NULL,
    components_secondary_number nvarchar(1024) NULL,
    components_secondary_designator nvarchar(1024) NULL,
    components_extra_secondary_number nvarchar(1024) NULL,
    components_extra_secondary_designator nvarchar(1024) NULL,
    components_pmb_designator nvarchar(1024) NULL,
    components_pmb_number nvarchar(1024) NULL,
    components_city_name nvarchar(1024) NULL,
    components_default_city_name nvarchar(1024) NULL,
    [components_state] nvarchar(1024) NULL,
    components_zip_code nvarchar(1024) NULL,
    components_plus4_code nvarchar(1024) NULL,
    components_delivery_point nvarchar(1024) NULL,
    components_delivery_point_check_digit nvarchar(1024) NULL,
    metadata_record_type nvarchar(1024) NULL,
    metadata_zip_type nvarchar(1024) NULL,
    metadata_county_fips nvarchar(1024) NULL,
    metadata_ecounty_name nvarchar(1024) NULL,
    metadata_carrier_route nvarchar(1024) NULL,
    metadata_congressional_district nvarchar(1024) NULL,
    metadata_building_default_indicator nvarchar(1024) NULL,
    metadata_rdi nvarchar(1024) NULL,
    metadata_elot_sequence nvarchar(1024) NULL,
    metadata_elot_sort nvarchar(1024) NULL,
    metadata_latitude float NULL,
    metadata_longitude float NULL,
    [metadata_precision] nvarchar(1024) NULL,
    metadata_time_zone nvarchar(1024) NULL,
    metadata_utc_offset float NULL,
    metadata_obeys_dst BIT NULL,
    analysis_dpv_match_code nvarchar(1024) NULL,
    analysis_dpv_footnotes nvarchar(1024) NULL,
    analysis_cmra nvarchar(1024) NULL,
    analysis_vacant nvarchar(1024) NULL,
    analysis_active nvarchar(1024) NULL,
    analysis_is_ews_match BIT NULL,
    analysis_footnotes nvarchar(1024) NULL,
    analysis_lacs_link_code nvarchar(1024) NULL,
    analysis_lacs_link_indicator nvarchar(1024) NULL,
    analysis_is_suite_link_match BIT NULL)
EXTERNAL NAME [SmartySqlServerPlugin].[SmartySqlServerPlugin.USStreetApi].[SmartyUsStreetVerifyFreeform]
```

Place your auth token and auth ID in a secure SQL table. See [Auth Setup](#auth-setup) for an example SQL query.

. . . and add the variables to SQL:
```sql
DECLARE @AUTH_ID nvarchar(max), @AUTH_TOKEN nvarchar(max), @URL nvarchar(max)
SELECT @AUTH_ID = auth_id FROM AuthDB.dbo.Auth
SELECT @AUTH_TOKEN = auth_token FROM AuthDB.dbo.Auth

-- Only SET @URL if you are not using the default value (https://us-street.api.smartystreets.com/street-address)
-- SELECT @URL = url FROM AuthDB.dbo.Auth
```

The Smarty functions are now ready to use! :
```sql
INSERT INTO freeform_results SELECT * FROM verifyFreeform('3214 N University Ave Provo UT 84604', 2, '', @AUTH_ID, @AUTH_TOKEN, @URL);
```
## Supported functions

The list of currently supported functions is as follows:

`SmartyUSStreetVerifyFreeform` - Verifies an entire address in one field

```sql
CREATE FUNCTION SmartyUSStreetVerifyFreeform 
(
	@freeform nvarchar(max),
    @max_candidates int,
    @match_strategy nvarchar(max),
	@authid nvarchar(max),
	@authtoken nvarchar(max),
	@url nvarchar(max)
)

RETURNS TABLE
(
    [status] int NULL,
    candidate int NULL,
    input_id nvarchar(1024) NULL,
    input_index int NULL,
    candidate_index int NULL,
    addressee nvarchar(1024) NULL,
    delivery_line_1 nvarchar(1024) NULL,
    delivery_line_2 nvarchar(1024) NULL,
    lastline nvarchar(1024) NULL,
    delivery_point_barcodeout nvarchar(1024) NULL,
    components_urbanization nvarchar(1024) NULL,
    components_primary_number nvarchar(1024) NULL,
    components_street_name nvarchar(1024) NULL,
    components_street_predirection nvarchar(1024) NULL,
    components_street_postdirection nvarchar(1024) NULL,
    components_street_suffix nvarchar(1024) NULL,
    components_secondary_number nvarchar(1024) NULL,
    components_secondary_designator nvarchar(1024) NULL,
    components_extra_secondary_number nvarchar(1024) NULL,
    components_extra_secondary_designator nvarchar(1024) NULL,
    components_pmb_designator nvarchar(1024) NULL,
    components_pmb_number nvarchar(1024) NULL,
    components_city_name nvarchar(1024) NULL,
    components_default_city_name nvarchar(1024) NULL,
    [components_state] nvarchar(1024) NULL,
    components_zip_code nvarchar(1024) NULL,
    components_plus4_code nvarchar(1024) NULL,
    components_delivery_point nvarchar(1024) NULL,
    components_delivery_point_check_digit nvarchar(1024) NULL,
    metadata_record_type nvarchar(1024) NULL,
    metadata_zip_type nvarchar(1024) NULL,
    metadata_county_fips nvarchar(1024) NULL,
    metadata_ecounty_name nvarchar(1024) NULL,
    metadata_carrier_route nvarchar(1024) NULL,
    metadata_congressional_district nvarchar(1024) NULL,
    metadata_building_default_indicator nvarchar(1024) NULL,
    metadata_rdi nvarchar(1024) NULL,
    metadata_elot_sequence nvarchar(1024) NULL,
    metadata_elot_sort nvarchar(1024) NULL,
    metadata_latitude float NULL,
    metadata_longitude float NULL,
    [metadata_precision] nvarchar(1024) NULL,
    metadata_time_zone nvarchar(1024) NULL,
    metadata_utc_offset float NULL,
    metadata_obeys_dst BIT NULL,
    analysis_dpv_match_code nvarchar(1024) NULL,
    analysis_dpv_footnotes nvarchar(1024) NULL,
    analysis_cmra nvarchar(1024) NULL,
    analysis_vacant nvarchar(1024) NULL,
    analysis_active nvarchar(1024) NULL,
    analysis_is_ews_match BIT NULL,
    analysis_footnotes nvarchar(1024) NULL,
    analysis_lacs_link_code nvarchar(1024) NULL,
    analysis_lacs_link_indicator nvarchar(1024) NULL,
    analysis_is_suite_link_match BIT NULL)
 EXTERNAL NAME [SmartySqlServerPlugin].[SmartySqlServerPlugin.USStreetApi].[SmartyUsStreetVerifyFreeform]
```

`SmartyUSStreetVerify` - Verifies an address with separated fields

```sql
CREATE FUNCTION SmartyUSStreetVerify 
(
	@street nvarchar(max),
	@street2 nvarchar(max),
	@secondary nvarchar(max),
    @city nvarchar(max),
	@state nvarchar(max),
	@zipcode nvarchar(max),
	@lastline nvarchar(max),
    @addressee nvarchar(max),
	@urbanization nvarchar(max),
    @max_candidates int,
	@match_strategy nvarchar(max),
    @authid nvarchar(max),
	@authtoken nvarchar(max),
	@url nvarchar(max)
)

RETURNS TABLE
(
    [status] int NULL,
    candidate int NULL,
    input_id nvarchar(1024) NULL,
    input_index int NULL,
    candidate_index int NULL,
    addressee nvarchar(1024) NULL,
    delivery_line_1 nvarchar(1024) NULL,
    delivery_line_2 nvarchar(1024) NULL,
    lastline nvarchar(1024) NULL,
    delivery_point_barcodeout nvarchar(1024) NULL,
    components_urbanization nvarchar(1024) NULL,
    components_primary_number nvarchar(1024) NULL,
    components_street_name nvarchar(1024) NULL,
    components_street_predirection nvarchar(1024) NULL,
    components_street_postdirection nvarchar(1024) NULL,
    components_street_suffix nvarchar(1024) NULL,
    components_secondary_number nvarchar(1024) NULL,
    components_secondary_designator nvarchar(1024) NULL,
    components_extra_secondary_number nvarchar(1024) NULL,
    components_extra_secondary_designator nvarchar(1024) NULL,
    components_pmb_designator nvarchar(1024) NULL,
    components_pmb_number nvarchar(1024) NULL,
    components_city_name nvarchar(1024) NULL,
    components_default_city_name nvarchar(1024) NULL,
    [components_state] nvarchar(1024) NULL,
    components_zip_code nvarchar(1024) NULL,
    components_plus4_code nvarchar(1024) NULL,
    components_delivery_point nvarchar(1024) NULL,
    components_delivery_point_check_digit nvarchar(1024) NULL,
    metadata_record_type nvarchar(1024) NULL,
    metadata_zip_type nvarchar(1024) NULL,
    metadata_county_fips nvarchar(1024) NULL,
    metadata_ecounty_name nvarchar(1024) NULL,
    metadata_carrier_route nvarchar(1024) NULL,
    metadata_congressional_district nvarchar(1024) NULL,
    metadata_building_default_indicator nvarchar(1024) NULL,
    metadata_rdi nvarchar(1024) NULL,
    metadata_elot_sequence nvarchar(1024) NULL,
    metadata_elot_sort nvarchar(1024) NULL,
    metadata_latitude float NULL,
    metadata_longitude float NULL,
    [metadata_precision] nvarchar(1024) NULL,
    metadata_time_zone nvarchar(1024) NULL,
    metadata_utc_offset float NULL,
    metadata_obeys_dst BIT NULL,
    analysis_dpv_match_code nvarchar(1024) NULL,
    analysis_dpv_footnotes nvarchar(1024) NULL,
    analysis_cmra nvarchar(1024) NULL,
    analysis_vacant nvarchar(1024) NULL,
    analysis_active nvarchar(1024) NULL,
    analysis_is_ews_match BIT NULL,
    analysis_footnotes nvarchar(1024) NULL,
    analysis_lacs_link_code nvarchar(1024) NULL,
    analysis_lacs_link_indicator nvarchar(1024) NULL,
    analysis_is_suite_link_match BIT NULL)
 EXTERNAL NAME [SmartySqlServerPlugin].[SmartySqlServerPlugin.USStreetApi].[SmartyUSStreetVerify];
```

## Auth Setup

```sql
USE [master];
GO 

ALTER DATABASE [AuthDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO 

DROP DATABASE [AuthDB];
GO

CREATE DATABASE [AuthDB];
GO

USE [AuthDB];
GO

ALTER DATABASE [AuthDB] SET TRUSTWORTHY ON
GO

-- Only create url if you are not using the default value (https://us-street.api.smartystreets.com/street-address)
CREATE TABLE Auth (
	auth_id nvarchar(max),
	auth_token nvarchar(max),
  	-- url nvarchar(max)
	)

INSERT INTO Auth VALUES('auth_id', 'auth_token' /*, ''*/)

SELECT * FROM Auth
```

## Examples

See examples in `/examples`

## Building the library

Rebuilding the .dll can be accomplished by the following steps:

NOTE: .NET Framework projects can only be built on Windows

### With Visual Studio or Similar (ex. Rider)

1. Install Visual Studio.
2. Build the .sln file: `SmartySqlServerPlugin.sln`

### Without Visual Studio

1. Clone this repository

```
git clone git@github.com:smarty/smarty-sql-server-plugin.git
```

2. Install [.NET Framework SDK v4.8](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks) from Microsoft.
3. Install [Build Tools for Visual Studio](https://visualstudio.microsoft.com/downloads/) to install MSBuild.
4. Install [Nuget](https://www.nuget.org/downloads) package manager.
5. Navigate `src` in cloned repository
6. Run `C:\path\to\nuget.exe restore` to install necessary dependencies.
7. Build the solution with MSBuild. The merged output file will appear in `/smarty-sql-server-plugin/src/plugin/bin/Release/`
```
&'C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe' -property:Configuration=Release
```

## Contact

For questions contact [Smarty Support](https://www.smarty.com/contact/support)

[Apache 2.0 License](LICENSE.md)
