<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFCodBar.aspx.cs" Inherits="NetLab.QR.WFCodBar" %>

<%@ Register assembly="IDAutomation.AztecServerControl" namespace="IDAutomation.AztecServerControl" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <cc1:AztecBarcode ID="AztecBarcode1" runat="server" />
    </div>
    </form>
</body>
</html>
