<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="FW.WT.AdminPortal.UserControls.Menu" %>
<asp:Menu ID="Menu1" runat="server" BackColor="#B5C7DE" 
    DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.8em" 
    ForeColor="#284E98" Orientation="Horizontal" StaticSubMenuIndent="10px" 
    DataSourceID="SiteMapDataSource1">
    <StaticSelectedStyle BackColor="#507CD1" />
    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
    <DynamicHoverStyle BackColor="#284E98" ForeColor="White" />
    <DynamicMenuStyle BackColor="#B5C7DE" />
    <DynamicSelectedStyle BackColor="#507CD1" />
    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
    <StaticHoverStyle BackColor="#284E98" ForeColor="White" />
</asp:Menu>
<asp:SiteMapPath ID="SiteMapPath1" runat="server">
</asp:SiteMapPath>

<asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" />


