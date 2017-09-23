<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Stripe3DSecureRedirect.aspx.cs" Inherits="WebTest.Stripe3DSecureRedirect" %>
<%@ Import Namespace="TCG.PaymentGateways.Providers" %>
<%@ Import Namespace="TCG.PaymentGateways.Classes.Payments" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title></title>
</head>
<body>
    <form id="form1" >
    <div>
    <% //Response.Write(Request["tokenVal"]);
        var stripe = new gwStripe();
        stripe.LoginTest();
        try
        {

            Response.Write("GENERATED TEMPORARY TOKEN BY THE 3DSecure FORM AT WebForm1.aspx>>> " + Request["tokenVal"]);
            Sale_Details sd = new Sale_Details()
            {
                Amount = Decimal.Parse(Request["amount"]),
                ProviderToken = Request["tokenVal"],
                CurrencyCode = Request["currency"],
                CardHolderName = Request["name"],
                CardHolderLastName = Request["surname"]
            };
            Transaction_Result rssale = stripe.Sale(sd);
            Response.Write("<br/> ERROR>>> " + rssale.ErrorText);
            Response.Write("<br/> SUCCESS>>> " + rssale.ResultText);
            Response.Write("<br/> TOKEN>>> " + rssale.TransactionIndex);
        }
        catch (TCG.PaymentGateways.Providers.Stripe.StripeException e)
        {
            Console.WriteLine(">>>>>>" + e.Message);
        }

        %>



    </div>
    </form>
</body>
</html>
