<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BrainTreeTest.aspx.cs" Inherits="WebTest.BrainTreeTest" %>
<%@ Import Namespace="TCG.PaymentGateways.Providers" %>
<%@ Import Namespace="TCG.PaymentGateways.Classes.Payments" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%
        var brainTree = new gwBrainTree();
        brainTree.LoginTest();
        try
        {
            // CALL SALE WITH null TOKEN
            Sale_Details sd = new Sale_Details()
            {
                Amount = (decimal)20.43,
                ProviderToken = null,
                CurrencyCode = "USD",
                CardNumber = "378282246310005",
                CardCCV = "2222",
                CardExpiryMonth = 11,
                CardExpiryYear = 2020,
                CardHolderName = "Amir",
                CardHolderLastName = "Saleh"
            };
            Transaction_Result rssale = brainTree.Sale(sd);
            Response.Write("ERROR>>> " + rssale.ErrorText);
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
