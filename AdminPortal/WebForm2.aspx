<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="FW.WT.AdminPortal.WebForm2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        a.closeBtn
        {
            position: absolute;
            top: 10px;
            right: 10px;
            display: block;
            width: 60px;
            padding: 4px 0;
            text-align: center;
            background: #fff;
            border: 1px solid #85B6E2;
            color: #333;
        }
        a.closeBtn:hover
        {
            color: #fff;
            border: 1px solid #85B6E2;
            background: #85B6E2;
        }
        #floatBoxBg
        {
            display: none;
            width: 100%;
            height: 100%;
            background: #000;
            position: absolute;
            top: 0;
            left: 0;
            border: 0;
            margin: 0;
            padding: 0;
            overflow: hidden;
            filter: alpha(opacity=0);
            opacity: 0;
        }
        .floatBox
        {
            border: #666 5px solid;
            width: 300px;
            position: absolute;
            top: 50px;
            left: 40%;
            display: none;
            position: fixed;
        }
        .floatBox .title
        {
            height: 23px;
            padding: 7px 10px 0;
            background: #333;
            color: #fff;
        }
        .floatBox .title h4
        {
            float: left;
            padding: 0;
            margin: 0;
            font-size: 14px;
            line-height: 16px;
        }
        .floatBox .title span
        {
            float: right;
            cursor: pointer;
        }
        .floatBox .content
        {
            padding: 20px 15px;
            background: #fff;
        }
    </style>

    <script type="text/javascript" src="Scripts/boxy/jquery-1.4.1.js"></script>

    <script type="text/javascript" src="Scripts/boxy/jquery.boxy.js"></script>

    <link href="Scripts/boxy/boxy.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript" src="Scripts/common.js"></script>

    <script type="text/javascript">

        $(document).ready(function() {
            $("#Button1").click(function() { return confirmO(this, "您确认删除吗？") });

        });

       
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="btn" type="button" value="show" />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
    </div>
    <asp:TextBox ID="txt_UserID_Equal" runat="server"></asp:TextBox>
    <asp:TextBox ID="txt_UserName_Like" runat="server"></asp:TextBox>
    <asp:DropDownList ID="ddl_TypeID_In" runat="server">
        <asp:ListItem Value="NoQuery">请选择</asp:ListItem>
        <asp:ListItem Value="1">aaaaaa</asp:ListItem>
        <asp:ListItem Value="2">bbbbbb</asp:ListItem>
    </asp:DropDownList>
    <a href="http://www.baidu.com" class="boxy">aaa</a>
    </form>
    <div id="divP" style="width: 300px; height: 400px; border: 1px #445 solid; background-color: #E4E2DD;
        display: none;">
        <input type="text" /><br />
        <input type="text" />
    </div>
    <div id="floatBox" class="floatBox">
        <div class="title">
            <h4>
            </h4>
            <span>关闭</span></div>
        <div class="content">
        </div>
    </div>
</body>
</html>
