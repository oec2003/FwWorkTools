<%@ Page Title="" ViewStateMode="Disabled" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VBSQLConvert.aspx.cs" Inherits="FW.WT.AdminPortal.CommonFuncManage.VBSQLConvert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%@ MasterType VirtualPath="~/Main.Master" %>

 <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <link rel="stylesheet" type="text/css" href="../Scripts/jqueryeasyui/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../Scripts/jqueryeasyui/themes/icon.css" />
    <script type="text/javascript" src="../Scripts/jqueryeasyui/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../Scripts/jqueryeasyui/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../Scripts/jqueryeasyui/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#btnClear").click(function () {
                $("#txtUp").val("");
                $("#txtDown").val("");
            });

            $("#btnSqlToVb").click(function () {
                var content = $("#txtUp").val();
                $.ajax({
                    type: 'POST',
                    async: false,
                    url: '../Ajax/VBSQLConvertAjax.ashx?Method=SqlToVb',
                    data: { Content: content, rnd: Math.random() },
                    success: function (data) {
                        if (data == "Error") {
                            $.messager.alert('错误', '删除成功', 'error');
                        }
                        else {
                            $("#txtDown").val(data);
                        }
                    }
                });
            });

            $("#btnVbToSql").click(function () {
                var content = $("#txtUp").val();
                $.ajax({
                    type: 'POST',
                    async: false,
                    url: '../Ajax/VBSQLConvertAjax.ashx?Method=VbToSql',
                    data: { Content: content, rnd: Math.random() },
                    success: function (data) {
                        if (data == "Error") {
                            $.messager.alert('错误', '删除成功', 'error');
                        }
                        else {
                            $("#txtDown").val(data);
                        }
                        
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
    <a href="#" id="btnClear" class="easyui-linkbutton" >清空</a>
    <a href="#" id="btnSqlToVb" class="easyui-linkbutton" >SQL 转 VB</a>
    <a href="#" id="btnVbToSql" class="easyui-linkbutton" >VB 转 SQL</a>
 </div>
 <div class="divDown">
    <asp:TextBox Wrap="false" runat="server"  ClientIDMode="Static" ID="txtDown" TextMode="MultiLine"   Height="200" Width="100%"  ></asp:TextBox>
 </div>
</form>

</asp:Content>
