namespace SmartySqlServerPlugin
{
    using System.Collections;
    using System.Net;
    using Microsoft.SqlServer.Server;
    using Interop;

    public static class USStreetApi
    {
        [SqlFunction(FillRowMethodName = "ParseFreeformResult")]
        public static IEnumerable SmartyUSStreetVerifyFreeform(string freeform, int maxCandidates, string matchStrategy, string id, string token, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                url = "https://us-street.api.smartystreets.com/street-address";
            }
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            return new USStreetRequest(freeform, "", "",
                "", "", "", "",
                "", "", maxCandidates, matchStrategy,
                id, token, url);
        }

        [SqlFunction(FillRowMethodName = "ParseResult")]
        public static IEnumerable SmartyUSStreetVerify(
            string street, string street2, string secondary,
            string city, string state, string zipcode, string lastline,
            string addressee, string urbanization,
            int maxCandidates, string matchStrategy,
            string id, string token, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                url = "https://us-street.api.smartystreets.com/street-address";
            }
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            return new USStreetRequest(
                street, street2, secondary,
                city, state, zipcode, lastline,
                addressee, urbanization, maxCandidates, matchStrategy,
                id, token, url);
        }

        private static void ParseFreeformResult(object source,
            out int status,
            out int? index,
            out string inputID,
            out int inputIndex,
            out int candidateIndex,
            out string addressee,
            out string deliveryLine1,
            out string deliveryLine2,
            out string lastLine,
            out string deliveryPointBarcodeout,
            out string urbanization,
            out string primaryNumber,
            out string streetName,
            out string streetPredirection,
            out string streetPostdirection,
            out string streetSuffix,
            out string secondaryNumber,
            out string secondaryDesignator,
            out string extraSecondaryNumber,
            out string extraSecondaryDesignator,
            out string pmbDesignator,
            out string pmbNumber,
            out string cityName,
            out string defaultCityName,
            out string state,
            out string zipCode,
            out string plus4Code,
            out string deliveryPoint,
            out string deliveryPointCheckDigit,
            out string recordType,
            out string zipType,
            out string countyFips,
            out string countyName,
            out string carrierRoute,
            out string congressionalDistrict,
            out string buildingDefaultIndicator,
            out string rdi,
            out string elotSequence,
            out string elotSort,
            out double? latitude,
            out double? longitude,
            out string precision,
            out string timeZone,
            out double? utcOffset,
            out bool? obeysDst,
            out string dpvMatchCode,
            out string dpvFootnotes,
            out string cmra,
            out string vacant,
            out string active,
            out bool? isEwsMatch,
            out string footnotes,
            out string lacsLinkCode,
            out string lacsLinkIndicator,
            out bool? isSuiteLinkMatch)
        {
            var item = (USStreetResponse)source;
            status = item.Status;
            index = item.Index;

            var candidate = item.Candidate;
            inputID = candidate.InputId;
            inputIndex = candidate.InputIndex;
            candidateIndex = candidate.CandidateIndex;
            addressee = candidate.Addressee;
            deliveryLine1 = candidate.DeliveryLine1;
            deliveryLine2 = candidate.DeliveryLine2;
            lastLine = candidate.LastLine;
            deliveryPointBarcodeout = candidate.DeliveryPointBarcode;

            var components = candidate.Components;
            if (components == null)
            {
                urbanization = primaryNumber = streetName = streetPredirection =
                    streetPostdirection = streetSuffix = secondaryNumber = secondaryDesignator =
                        extraSecondaryNumber = extraSecondaryDesignator = pmbDesignator = pmbNumber =
                            cityName = defaultCityName = state = zipCode = plus4Code = deliveryPoint =
                                deliveryPointCheckDigit = null;
            }
            else
            {
                urbanization = components.Urbanization;
                primaryNumber = components.PrimaryNumber;
                streetName = components.StreetName;
                streetPredirection = components.StreetPredirection;
                streetPostdirection = components.StreetPostdirection;
                streetSuffix = components.StreetSuffix;
                secondaryNumber = components.SecondaryNumber;
                secondaryDesignator = components.SecondaryDesignator;
                extraSecondaryNumber = components.ExtraSecondaryNumber;
                extraSecondaryDesignator = components.ExtraSecondaryDesignator;
                pmbDesignator = components.PmbDesignator;
                pmbNumber = components.PmbNumber;
                cityName = components.CityName;
                defaultCityName = components.DefaultCityName;
                state = components.State;
                zipCode = components.ZipCode;
                plus4Code = components.Plus4Code;
                deliveryPoint = components.DeliveryPoint;
                deliveryPointCheckDigit = components.DeliveryPointCheckDigit;
            }


            var metadata = candidate.Metadata;
            if (metadata == null)
            {
                recordType = zipType = countyFips = countyName = carrierRoute =
                    congressionalDistrict = buildingDefaultIndicator = rdi = elotSequence =
                        elotSort = precision = timeZone = null;
                latitude = longitude = utcOffset = null;
                obeysDst = null;
            }
            else
            {
                recordType = metadata.RecordType;
                zipType = metadata.ZipType;
                countyFips = metadata.CountyFips;
                countyName = metadata.CountyName;
                carrierRoute = metadata.CarrierRoute;
                congressionalDistrict = metadata.CongressionalDistrict;
                buildingDefaultIndicator = metadata.BuildingDefaultIndicator;
                rdi = metadata.Rdi;
                elotSequence = metadata.ElotSequence;
                elotSort = metadata.ElotSort;
                latitude = metadata.Latitude;
                longitude = metadata.Longitude;
                precision = metadata.Precision;
                timeZone = metadata.TimeZone;
                utcOffset = metadata.UtcOffset;
                obeysDst = metadata.ObeysDst;
            }

            var analysis = candidate.Analysis;
            if (analysis == null)
            {
                dpvMatchCode = dpvFootnotes = cmra = vacant = active =
                    footnotes = lacsLinkCode = lacsLinkIndicator = null;
                isEwsMatch = isSuiteLinkMatch = null; //analysis.IsEwsMatch;// 
            }
            else
            {
                dpvMatchCode = analysis.DpvMatchCode;
                dpvFootnotes = analysis.DpvFootnotes;
                cmra = analysis.Cmra;
                vacant = analysis.Vacant;
                active = analysis.Active;
                isEwsMatch = metadata.IsEwsMatch; //analysis.IsEwsMatch;
                footnotes = analysis.Footnotes;
                lacsLinkCode = analysis.LacsLinkCode;
                lacsLinkIndicator = analysis.LacsLinkIndicator;
                isSuiteLinkMatch = analysis.IsSuiteLinkMatch;
            }
        }

        private static void ParseResult(object source,
            out int status,
            out int? index,
            out string inputID,
            out int inputIndex,
            out int candidateIndex,
            out string addressee,
            out string deliveryLine1,
            out string deliveryLine2,
            out string lastLine,
            out string deliveryPointBarcodeout,
            out string urbanization,
            out string primaryNumber,
            out string streetName,
            out string streetPredirection,
            out string streetPostdirection,
            out string streetSuffix,
            out string secondaryNumber,
            out string secondaryDesignator,
            out string extraSecondaryNumber,
            out string extraSecondaryDesignator,
            out string pmbDesignator,
            out string pmbNumber,
            out string cityName,
            out string defaultCityName,
            out string state,
            out string zipCode,
            out string plus4Code,
            out string deliveryPoint,
            out string deliveryPointCheckDigit,
            out string recordType,
            out string zipType,
            out string countyFips,
            out string countyName,
            out string carrierRoute,
            out string congressionalDistrict,
            out string buildingDefaultIndicator,
            out string rdi,
            out string elotSequence,
            out string elotSort,
            out double? latitude,
            out double? longitude,
            out string precision,
            out string timeZone,
            out double? utcOffset,
            out bool? obeysDst,
            out string dpvMatchCode,
            out string dpvFootnotes,
            out string cmra,
            out string vacant,
            out string active,
            out bool? isEwsMatch,
            out string footnotes,
            out string lacsLinkCode,
            out string lacsLinkIndicator,
            out bool? isSuiteLinkMatch)
        {
            var item = (USStreetResponse)source;
            status = item.Status;
            index = item.Index;

            var candidate = item.Candidate;
            inputID = candidate.InputId;
            inputIndex = candidate.InputIndex;
            candidateIndex = candidate.CandidateIndex;
            addressee = candidate.Addressee;
            deliveryLine1 = candidate.DeliveryLine1;
            deliveryLine2 = candidate.DeliveryLine2;
            lastLine = candidate.LastLine;
            deliveryPointBarcodeout = candidate.DeliveryPointBarcode;

            var components = candidate.Components;
            if (components == null)
            {
                urbanization = primaryNumber = streetName = streetPredirection =
                    streetPostdirection = streetSuffix = secondaryNumber = secondaryDesignator =
                        extraSecondaryNumber = extraSecondaryDesignator = pmbDesignator = pmbNumber =
                            cityName = defaultCityName = state = zipCode = plus4Code = deliveryPoint =
                                deliveryPointCheckDigit = null;
            }
            else
            {
                urbanization = components.Urbanization;
                primaryNumber = components.PrimaryNumber;
                streetName = components.StreetName;
                streetPredirection = components.StreetPredirection;
                streetPostdirection = components.StreetPostdirection;
                streetSuffix = components.StreetSuffix;
                secondaryNumber = components.SecondaryNumber;
                secondaryDesignator = components.SecondaryDesignator;
                extraSecondaryNumber = components.ExtraSecondaryNumber;
                extraSecondaryDesignator = components.ExtraSecondaryDesignator;
                pmbDesignator = components.PmbDesignator;
                pmbNumber = components.PmbNumber;
                cityName = components.CityName;
                defaultCityName = components.DefaultCityName;
                state = components.State;
                zipCode = components.ZipCode;
                plus4Code = components.Plus4Code;
                deliveryPoint = components.DeliveryPoint;
                deliveryPointCheckDigit = components.DeliveryPointCheckDigit;
            }


            var metadata = candidate.Metadata;
            if (metadata == null)
            {
                recordType = zipType = countyFips = countyName = carrierRoute =
                    congressionalDistrict = buildingDefaultIndicator = rdi = elotSequence =
                        elotSort = precision = timeZone = null;
                latitude = longitude = utcOffset = null;
                obeysDst = null;
            }
            else
            {
                recordType = metadata.RecordType;
                zipType = metadata.ZipType;
                countyFips = metadata.CountyFips;
                countyName = metadata.CountyName;
                carrierRoute = metadata.CarrierRoute;
                congressionalDistrict = metadata.CongressionalDistrict;
                buildingDefaultIndicator = metadata.BuildingDefaultIndicator;
                rdi = metadata.Rdi;
                elotSequence = metadata.ElotSequence;
                elotSort = metadata.ElotSort;
                latitude = metadata.Latitude;
                longitude = metadata.Longitude;
                precision = metadata.Precision;
                timeZone = metadata.TimeZone;
                utcOffset = metadata.UtcOffset;
                obeysDst = metadata.ObeysDst;
            }

            var analysis = candidate.Analysis;
            if (analysis == null)
            {
                dpvMatchCode = dpvFootnotes = cmra = vacant = active =
                    footnotes = lacsLinkCode = lacsLinkIndicator = null;
                isEwsMatch = isSuiteLinkMatch = null; //analysis.IsEwsMatch;// 
            }
            else
            {
                dpvMatchCode = analysis.DpvMatchCode;
                dpvFootnotes = analysis.DpvFootnotes;
                cmra = analysis.Cmra;
                vacant = analysis.Vacant;
                active = analysis.Active;
                isEwsMatch = metadata.IsEwsMatch; //analysis.IsEwsMatch;
                footnotes = analysis.Footnotes;
                lacsLinkCode = analysis.LacsLinkCode;
                lacsLinkIndicator = analysis.LacsLinkIndicator;
                isSuiteLinkMatch = analysis.IsSuiteLinkMatch;
            }
        }
        /*
        [SqlFunction(DataAccess = DataAccessKind.Read, FillRowMethodName = "ParseFreeformResult")]
        public static IEnumerable VerifyTable(string table, string authid, string authtoken, string url)
        {
            List<EnumerableResponse<Candidate>> results = new List<EnumerableResponse<Candidate>>();

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();

                using (SqlCommand verifyAddresses = new SqlCommand(
                           "SELECT * FROM " + table, connection))
                {
                    using (SqlDataReader addressReader = verifyAddresses.ExecuteReader())
                    {
                        while (addressReader.Read())
                        {
                            string address = addressReader.GetString(0);
                            EnumerableResponse<Candidate> enumerator =
                                (EnumerableResponse<Candidate>)VerifyFreeform(address, authid, authtoken, url)
                                    .GetEnumerator();

                            while (enumerator.MoveNext())
                            {
                                results.Add(enumerator);
                            }
                        }
                    }
                }
            }

            return results;
        }*/
    }
}