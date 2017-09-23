<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StripeTest.aspx.cs" Inherits="WebTest.StripeTest" %>
<%@ Import Namespace="TCG.PaymentGateways.Providers" %>
<%@ Import Namespace="TCG.PaymentGateways.Classes.Payments" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   
    
</head>
<body>
     
    <div>
        <%var stripe = new gwStripe();
            stripe.LoginTest();


            //3D-Secure


            #region
            Response.Write(stripe.Lookup3dSecure(new Lookup3dSecure_Details()
            {
                isOnceoff = true,
                SaleData = new Sale_Details()
                {
                    Amount = (decimal)2.44,
                    CardHolderName = "amir",
                    CardHolderLastName = "saleh",
                    CurrencyCode = "usd"
                },
                isPayment = true,
                ExternalIdentifier1 = "/Stripe3DSecureRedirect.aspx"
            }).ThreeDSecurePost);

            #endregion


            //ONCE-OFF PAYEMENT TEST


            #region
            //try
            //{
            //    // CALL SALE WITH null TOKEN
            //    Sale_Details sd = new Sale_Details()
            //    {
            //        Amount = (decimal)21.43,
            //        ProviderToken = null,
            //        CurrencyCode = "USD",
            //        CardNumber = "4000000000000077",
            //        CardCCV = "222",
            //        CardExpiryMonth = 11,
            //        CardExpiryYear = 2020,
            //        CardHolderName = "Amir",
            //        CardHolderLastName = "Saleh"
            //    };
            //    Transaction_Result rssale = stripe.Sale(sd);
            //    Response.Write("ERROR>>> " + rssale.ErrorText);
            //    Response.Write("<br/> SUCCESS>>> " + rssale.ResultText);
            //    Response.Write("<br/> TOKEN>>> " + rssale.TransactionIndex);

            //}
            //catch (TCG.PaymentGateways.Providers.Stripe.StripeException e)
            //{
            //    Console.WriteLine(">>>>>>" + e.Message);
            //}
            #endregion


            //AUTH THEN CAPTURE TEST


            #region
            //try
            //{
            //    Sale_Details sd = new Sale_Details()
            //    {
            //        Amount = (decimal)2.43,
            //        CurrencyCode = "USD",
            //        CustomerLastName = "Saleh",
            //        CustomerFirstName = "Amir",
            //        CardNumber = "4000000000000077",
            //        CardCCV = "222",
            //        CardExpiryMonth = 10,
            //        CardExpiryYear = 2020
            //    };
            //    //CREATE AUTHENTICATION
            //    Transaction_Result rs = stripe.Auth(sd);
            //    Response.Write("Auth Token >>> " + rs.ProviderToken);
            //    //CALLS THE CHARGE RIGHT AFTER AUTH
            //    AuthCapture_Details acd = new AuthCapture_Details()
            //    {
            //        TransactionIndex = rs.ProviderToken
            //    };
            //    var capt = stripe.Capture(acd);
            //    Response.Write("</br>ERROR >>> " + capt.ErrorText);
            //    Response.Write("</br>Success >>> " + capt.ResultText);
            //    Response.Write("</br>TRANSACTION TOKEN >>> " + capt.TransactionIndex);
            //}
            //catch (TCG.PaymentGateways.Providers.Stripe.StripeException e)
            //{
            //    Console.WriteLine(">>>>>>" + e.Message);
            //}


            #endregion


            //AUTH AND CAPTURE TEST


            #region


            //AUTH TEST


            #region
            //try
            //{
            //    Sale_Details sd = new Sale_Details()
            //    {
            //        Amount = (decimal)2.43,
            //        CurrencyCode = "USD",
            //        CustomerLastName = "Saleh",
            //        CustomerFirstName = "Amir",
            //        CardNumber = "4000000000000077",
            //        CardCCV = "222",
            //        CardExpiryMonth = 10,
            //        CardExpiryYear = 2020
            //    };
            //    Transaction_Result rs = stripe.Auth(sd);
            //    Response.Write("ERROR>>> " + rs.ErrorText);
            //    Response.Write("<br/> SUCCESS>>> " + rs.ApprovalCode);
            //    Response.Write("<br/> AUTH TOKEN>>> COPY PASTE THIS AUTHENTICATION TOKEN IN THE CAPTURE ID >> " + rs.ProviderToken);

            //}
            //catch (TCG.PaymentGateways.Providers.Stripe.StripeException e)
            //{
            //    Console.WriteLine(">>>>>>" + e.Message);
            //}
            #endregion


            //CAPTURE TEST


            #region
            //try
            //{
            //    Transaction_Result rs = stripe.Capture(new AuthCapture_Details()
            //    {
            //        TransactionIndex = "ch_1B1ZpVDai4eHx4p2AETkbw6v"
            //    });
            //    Response.Write("ERROR>>> " + rs.ErrorText);
            //    Response.Write("<br/> SUCCESS>>> " + rs.ApprovalCode);
            //    Response.Write("<br/> CHARGE TOKEN>>> " + rs.ProviderToken);
            //    Response.Write("<br/> TRANSACTION TOKEN>>> " + rs.TransactionIndex);
            //    //System.Console.WriteLine(rssale.ToString());
            //}
            //catch (TCG.PaymentGateways.Providers.Stripe.StripeException e)
            //{
            //    Console.WriteLine(">>>>>>" + e.Message);
            //}
            #endregion

            #endregion


            //REUSABLE CARDS


            #region


            //try
            //{
            //    //////********************TO CREATE A NEW REUSABLE CUSTOMER UNCOMMENT THIS BLOCK AND COMMENT SALE (next block in below)

            //    //StorePaymentMethod_Details details = new StorePaymentMethod_Details()
            //    //{
            //    //    CardNumber = "4000000000000077",
            //    //    CardCVV = "222",
            //    //    CardExpiryMonth = 10,
            //    //    CardExpiryYear = 2020,
            //    //    CardHolderName = "AmirS",
            //    //    CardHolderSurname = "REUSABLE Surname"
            //    //};
            //    //StorePaymentMethod_Result rs = stripe.StorePaymentMethod(details);
            //    //Response.Write("ERROR>>> " + rs.ErrorMessage);
            //    //Response.Write("<br/> CUSTOMER TOKEN>>> " + rs.CardToken);


            //    //////*******************CALL SALE
            //    //SAMPLE CUSTOMER NO
            //    //cus_BGSlQ6AC7Qb9vY
            //    //cus_BONoTS2IpFgIeB
            //    //cus_BONspVPFOMQpkQ

            //    Sale_Details sd = new Sale_Details()
            //    {
            //        Amount = (decimal)15.43,
            //        ProviderToken = "cus_BGSlQ6AC7Qb9vY",  //PASTE THE CUSTOMER TOKEN HERE 
            //        CurrencyCode = "USD"
            //    };
            //    Transaction_Result rssale = stripe.Sale(sd);
            //    Response.Write("ERROR>>> " + rssale.ErrorText);
            //    Response.Write("<br/> SUCCESS>>> " + rssale.ResultText);
            //    Response.Write("<br/> TOKEN>>> " + rssale.TransactionIndex);

            //}
            //catch (TCG.PaymentGateways.Providers.Stripe.StripeException e)
            //{
            //    Console.WriteLine(">>>>>>" + e.Message);
            //}

            #endregion


        %>


    </div>

</body>
    
</html>
