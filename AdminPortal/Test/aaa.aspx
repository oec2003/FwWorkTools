<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="aaa.aspx.cs" Inherits="FW.WT.AdminPortal.Test.tttt.aaa" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
<script type="text/javascript" src="tttt/src/javascripts/jquery-1.4.1.js"></script>
<script type="text/javascript" src="tttt/src/javascripts/jquery.boxy.js"></script>
<link rel="stylesheet" href="tttt/src/stylesheets/boxy.css" type="text/css" />

    <script type="text/javascript">

		 $(document).ready(function() {
			$("#btn").click(function(){
				Boxy.alert("添加成功！", null, null);
			});
           
        });
       
    
    </script>
</head>
<body>
 <input id="btn" type="button" value="show"  />
</body>
</html>