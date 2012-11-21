<%@ Page Title="" ViewStateMode="Disabled" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VBSQLConvert.aspx.cs" Inherits="FW.WT.AdminPortal.CommonFuncManage.VBSQLConvert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%@ MasterType VirtualPath="~/Main.Master" %>

 <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <link rel="stylesheet" href="../Scripts/jqwidgets/styles/jqx.base.css" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery-1.8.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxcore.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxdata.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxbuttons.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxscrollbar.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxlistbox.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxdropdownlist.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxmenu.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxgrid.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxgrid.pager.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxgrid.filter.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxgrid.sort.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxgrid.selection.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxnumberinput.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxwindow.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxpanel.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxcheckbox.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxcalendar.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/jqxdatetimeinput.js"></script>
    <script type="text/javascript" src="../Scripts/jqwidgets/globalization/jquery.global.js"></script>
    <script type="text/javascript" src="../Scripts/gettheme.js"></script>
    <script type="text/javascript" src="generatedata.js"></script>
    <script type="text/javascript" src="../Scripts/json2.js"></script>
    <script type="text/javascript" src="../Scripts/boxy/jquery.boxy.js"></script>
    <link type="text/css" rel="Stylesheet" href="../Scripts/boxy/boxy.css" />
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>

    <script type="text/javascript">
        var _url = '/WT/AdminPortal/Ajax/VBSQLConvertAjax.ajax';

        $(document).ready(function () {
            var theme = getTheme();

            $("#btnClear").jqxButton({ width: 70, theme: theme });
            $("#btnVbToSql").jqxButton({ width: 100, theme: theme });
            $("#btnSqlToVb").jqxButton({ width: 100, theme: theme });
            $("#btnCopy").jqxButton({ width: 70, theme: theme });

            $("#btnClear").click(function () {
                $("#txtUp").val("");
                $("#txtDown").val("");
            });

            $("#btnSqlToVb").click(function () {
                var content = $("#txtUp").val();
                $.ajax({
                    type: 'POST',
                    async: false,
                    url: _url,
                    data: { action: 'SqlToVb', Content: content, rnd: Math.random() },
                    success: function (data) {
                        $("#txtDown").val(data);
                    }
                });
            });

            $("#btnVbToSql").click(function () {
                var content = $("#txtUp").val();
                $.ajax({
                    type: 'POST',
                    async: false,
                    url: _url,
                    data: { action: 'VbToSql', Content: content, rnd: Math.random() },
                    success: function (data) {
                        $("#txtDown").val(data);
                    }
                });
            });
        });
    </script>

     <style type="text/css">
        .divMid
        {
            text-align:center;
           
            margin-left: 0px;
            margin-right: 10px;
            margin-top: 5px;
            margin-bottom: 5px;
            width:800px;
        }
        .divUp
        {
            width:800px;
        }
        .divDown
        {
            width:800px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
<form id="form1" runat="server">
 <div class="divUp">
      <asp:TextBox Wrap="false" runat="server" ClientIDMode="Static" ID="txtUp" TextMode="MultiLine" Height="200" Width="100%"  ></asp:TextBox>
 </div>
 <div class="divMid">
      <input type="button" id="btnClear" value="清空" style="cursor:hand;"/>
      <input type="button" id="btnSqlToVb" value="SQL 转 VB" style="cursor:hand;"/>
      <input type="button" id="btnVbToSql" value="VB 转 SQL" style="cursor:hand;"/>
      <%--<input type="button" id="btnCopy" value="复制" />--%>
 </div>
 <div class="divDown">
    <asp:TextBox Wrap="false" runat="server"  ClientIDMode="Static" ID="txtDown" TextMode="MultiLine"   Height="200" Width="100%"  ></asp:TextBox>
 </div>
</form>

</asp:Content>
