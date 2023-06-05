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

CREATE ASSEMBLY [System.Runtime.Serialization] FROM 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Runtime.Serialization.dll' WITH PERMISSION_SET = UNSAFE;
GO

CREATE ASSEMBLY [Smarty.Plugins.SqlServer] FROM 'C:\path\to\SmartySqlServerPlugin.dll' WITH PERMISSION_SET = EXTERNAL_ACCESS;
GO

CREATE FUNCTION SmartyUSStreetVerify
(
	@street nvarchar(max),
	@street_2 nvarchar(max),
	@secondary nvarchar(max),
    @city nvarchar(max),
	@state nvarchar(max),
	@zip_code nvarchar(max),
	@last_line nvarchar(max),
    @addressee nvarchar(max),
	@urbanization nvarchar(max),
    @max_candidates int,
	@match_strategy nvarchar(max),
    @auth_id nvarchar(max),
	@auth_token nvarchar(max),
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
EXTERNAL NAME [Smarty.Plugins.SqlServer].[SmartySqlServerPlugin.USStreetApi].[SmartyUSStreetVerify];
GO

DECLARE @AUTH_ID nvarchar(max), @AUTH_TOKEN nvarchar(max), @URL nvarchar(max)
SELECT @AUTH_ID = auth_id FROM AuthDB.dbo.Auth
SELECT @AUTH_TOKEN = auth_token FROM AuthDB.dbo.Auth

-- Only SET @URL if you are not using the default value (https://us-street.api.smartystreets.com/street-address)
-- SELECT @URL = url FROM AuthDB.dbo.Auth

CREATE TABLE verify_results
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

INSERT INTO verify_results SELECT * FROM SmartyUSStreetVerify('3214 N University Ave', '', '', 'Provo', 'UT', '84604', '', '', '', 2, '', @AUTH_ID, @AUTH_TOKEN, @URL);
INSERT INTO verify_results SELECT * FROM SmartyUSStreetVerify('3214 N University Ave', '', '', '', '', '84604', '', '', '', 2, '', @AUTH_ID, @AUTH_TOKEN, @URL);
INSERT INTO verify_results SELECT * FROM SmartyUSStreetVerify('3215 N University Ave', '', '', '', '', '', '', '', '', 2, '', @AUTH_ID, @AUTH_TOKEN, @URL);
INSERT INTO verify_results SELECT * FROM SmartyUSStreetVerify('1109 9th', '', '', 'Phoenix', 'AZ', '', '', '', '', 2, '', @AUTH_ID, @AUTH_TOKEN, @URL);

SELECT * from verify_results;

DROP FUNCTION [dbo].[SmartyUSStreetVerify]
GO
