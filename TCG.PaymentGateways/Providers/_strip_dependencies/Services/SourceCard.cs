﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TCG.PaymentGateways.Providers.Stripe
{
    public class SourceCard : INestedOptions
    {
        [JsonProperty("source[object]")]
        internal string Object => "card";

        [JsonProperty("source[number]")]
        public string Number { get; set; }

        [JsonProperty("source[exp_month]")]
        public int ExpirationMonth { get; set; }

        [JsonProperty("source[exp_year]")]
        public int ExpirationYear { get; set; }

        [JsonProperty("source[cvc]")]
        public string Cvc { get; set; }

        [JsonProperty("source[name]")]
        public string Name { get; set; }

        [JsonProperty("source[address_line1]")]
        public string AddressLine1 { get; set; }

        [JsonProperty("source[address_line2]")]
        public string AddressLine2 { get; set; }

        [JsonProperty("source[address_city]")]
        public string AddressCity { get; set; }

        [JsonProperty("source[address_zip]")]
        public string AddressZip { get; set; }

        [JsonProperty("source[address_state]")]
        public string AddressState { get; set; }

        [JsonProperty("source[address_country]")]
        public string AddressCountry { get; set; }

        [JsonProperty("source[description]")]
        public string Description { get; set; }

        [JsonProperty("source[metadata]")]
        public Dictionary<string, string> Metadata { get; set; }

        [JsonProperty("source[capture]")]
        public bool? Capture { get; set; }

        [JsonProperty("source[statement_descriptor]")]
        public string StatementDescriptor { get; set; }

        [JsonProperty("source[destination]")]
        public string Destination { get; set; }

        // application_fee

        // shipping
    }
}