<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="FW.WT.AdminPortal.WebForm1" Theme="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="Scripts/boxy/jquery-1.4.1.js"></script>

    <script type="text/javascript" src="Scripts/boxy/jquery.boxy.js"></script>

    <link href="Scripts/boxy/boxy.css" rel="Stylesheet" type="text/css" />
    <link href="Styles/style.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript" src="Scripts/common.js"></script>
 <style type="text/css">
        div
        {
            margin: 3px;
            width: 40px;
            height: 40px;
            position: absolute;
            left: 0px;
            top: 30px;
            background: green;
            display: none;
        }
        div.newcolor
        {
            background: blue;
        }
        span
        {
            color: red;
        }
    </style>
    <script type="text/javascript">
        $("#show").click(function() {
            var n = $("div").queue("px");
            $("span").text("Queque length is:" + n.length);
        });
        function runIt() {
            $("div").show("slow");
            $("div").animate({ left: '+=200' }, 2000);
            $("div").slideToggle(1000);
            $("div").slideToggle("fast");
            $("div").animate({ left: '-=200' }, 1500);
            $("div").hide("slow");
            $("div").show(1200);
            $("div").slideUp("normal", runIt);
        }
        runIt();
      
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <button id="show" >
        Show Length of Queue</button>
    <span></span>
    <div>
    </div>
    </form>
</body>
</html>
