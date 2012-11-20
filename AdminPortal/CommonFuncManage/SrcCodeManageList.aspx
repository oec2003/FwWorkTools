<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" ViewStateMode="Disabled"
    CodeBehind="SrcCodeManageList.aspx.cs" Inherits="FW.WT.AdminPortal.CommonFuncManage.SrcCodeManageList" %>

<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Scripts/jqueryeasyui/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../Scripts/jqueryeasyui/themes/icon.css" />

    <script type="text/javascript">
        var _srcCodeManageID;
        var _mode;
        $(function () {
            $('#dg').datagrid({
                title: '一线反馈记录列表',
                //iconCls: 'icon-save',
                collapsible: false,
                fitColumns: false,
                singleSelect: false,
                remoteSort: false,
                sortName: 'CustomerName',
                sortOrder: 'desc',
                nowrap: true,
                striped: true,
                method: 'get',
                loadMsg: '正在加载数据...',
                url: '../Ajax/SrcCodeManageAjax.ashx?Method=GetSrcCodeManage',
                frozenColumns: [[
                    { field: 'ck', checkbox: true }
			    ]],
                columns: [[
                    { field: 'CustomerArea', title: '区域', width: 100, sortable: true },
                    { field: 'CustomerName', title: '客户名称', width: 100, sortable: true },
                    { field: 'ProjVersion', title: 'ERP版本', width: 60, sortable: true },
                    { field: 'VSSAddress', title: 'VSS&TFS地址', width: 220 },
				    { field: 'DBName', title: '数据库名', width: 120 },
				    { field: 'ServerPort', title: '端口', width: 60 },
				    { field: 'ServerName', title: '服务器地址', width: 100 },
                    { field: 'ApplicationName', title: '应用程序名', width: 100 },
                    { field: 'UserName', title: '用户名', width: 100 },
                    { field: 'UserPwd', title: '密码', width: 100 },
                    { field: 'Remark', title: '备注', width: 220 },
			    ]],
                pagination: true,
                pageNumber: 1,
                rownumbers: true,
                toolbar: '#tb',
                onLoadSuccess: function () {
                    //$("#roleManageGrid").datagrid('reload');
                },
                onClickRow: function (rowIndex, rowData) {
                    var id = rowData.FeedBackLogID;
                    //alert(id);
                    // loadTree(roleId);
                    // RoleId = roleId;
                }
            });

            $("#tb").show();
        });

        var _url ="" ;
        //添加
        function add() {
            _mode = "1";
            //$('#dlg').dialog('open').dialog('setTitle', '新增');
            openDialog();
            $('#fm').form('clear');
           
            url = "../Ajax/SrcCodeManageAjax.ashx?Method=SaveSrcCodeManage&Mode="+_mode; 
        }

        function edit() {
            var rowData = $('#dg').datagrid('getSelections');
            if (rowData.length == 0) {
                $.messager.alert('提示', '请选择要编辑的项！','info');
            }
            else if (rowData.length > 1) {
                $.messager.alert('提示', '只能选择一项进行编辑！','info');
            }
            else {
                _mode = "2";
                var row = $('#dg').datagrid('getSelected');
                //$('#dlg').dialog('open').dialog('setTitle', '编辑');
                openDialog();
                $('#fm').form('load', row);
                _srcCodeManageID = row.SrcCodeManageID;
                
                url = "../Ajax/SrcCodeManageAjax.ashx?Method=SaveSrcCodeManage&ID=" + row.SrcCodeManageID+"&Mode="+_mode;
            }
        }

        function openDialog() {
            $("#dlg").show();
            $("#dlg").attr("title", _mode == "1" ? "添加" : "编辑");
            $("#dlg").dialog({
                draggable: true,
                resizable: false,
                modal: true,
                buttons:
                    [
                        {
                            text: '保存',
                            iconCls: 'icon-ok',
                            handler: function () {
                                saveSrc();
                            }
                        },
				        {
				            text: '取消',
				            iconCls: 'icon-no',
				            handler: function () {
				                $('#dlg').dialog('close');
				            }
				        }
				    ]
            });
        }

        function del() {
            var rowData = $('#dg').datagrid('getSelections');
            if (rowData.length == 0) {
                $.messager.alert('提示', '请选择要删除的项！','info');
            }
            else {
                $.messager.confirm('删除提示', '确认删除吗?', function (r) {
                    if (r) {
                        var ids = "";
                        for (var i = 0; i < rowData.length; i++) {
                            ids += rowData[i].SrcCodeManageID + ",";
                        }
                        ids = ids.substr(0, ids.length - 1);
                        $.ajax({
                            type: "POST",
                            dataType: "text",
                            //cache:true,
                            url: "../Ajax/SrcCodeManageAjax.ashx?Method=DelSrcCodeManage",
                            data: { IDs: ids },
                            success: function (data) {
                                if (data == "Success") {
                                    $.messager.alert('提示', '删除成功');
                                    $("#dg").datagrid('reload');
                                }
                                else {
                                    $.messager.alert('错误', '删除失败!', 'error');
                                }
                            },
                            error: function (data) {
                                $.messager.alert('错误', '删除失败!', 'error');
                            }
                        });
                    }
                });
            }
        }

        //保存
        function saveSrc() {
            $('#fm').form('submit', {
                url: url,
                onSubmit: function () {
                    return $(this).form('validate');
                },
                success: function (data) {
                    if (data =="Success") {
                        $('#dlg').dialog('close');      // close the dialog  
                        $('#dg').datagrid('reload');    // reload the user data  
                        $.messager.alert('提示', '保存成功!', 'info');
                    }
                    else if (data=="Error")
                    {
                        $.messager.alert('错误', '系统出错!', 'error');
                    }
                   
                }
            });  
        }

        function checkCstName() {
            if ($("#CustomerName").val().trim() == "") return;
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "../Ajax/SrcCodeManageAjax.ashx?Method=IsExistName",
                data: { id: _srcCodeManageID, Mode: _mode, CstName: $("#CustomerName").val() },
                success: function (data) {
                    if (data.Flag[0].Status == 1) {
                        $.messager.alert('提示', '客户名称已经存在!', 'info');
                    }
                },
                error: function (data) {
                    $.messager.alert('错误', '系统出错!', 'error');
                }
            });
        }

        function downReg() {
            var rowData = $('#dg').datagrid('getSelections');
            if (rowData.length == 0) {
                $.messager.alert('提示', '请选择要下载的项！', 'info');
            }
            else if (rowData.length > 1) {
                $.messager.alert('提示', '只能选择一项进行下载！', 'info');
            }
            else {
                $.ajax({
                    type: 'POST',
                    url: "../Ajax/SrcCodeManageAjax.ashx?Method=RegisterAndDownLoadReg",
                    data: { dbName: rowData[0].ServerName, appName: rowData[0].ApplicationName,
                        serverName: rowData[0].ServerName, userName: rowData[0].UserName,
                        saPassword: rowData[0].UserPwd, serverProt: rowData[0].ServerPort,
                        customerName: rowData[0].CustomerName, rnd: Math.random()
                    },
                    success: function (data) {
                        if (data != "Error") {
                            if (data.split("|").length == 2) {
                                var filePath = data.split("|")[0];
                                var fileName = data.split("|")[1];
                                //debugger;
                                downRegFile(filePath, fileName);
                            }
                        }
                        else{
                            $.messager.alert('错误', '系统出错!', 'error');
                        }
                    }
                });
            }
        }
        var downRegFile = function (filePath, fileName) {
            strWindowPos = "height=0,width=0,left=" + (screen.width / 2 - 40) + ",top=" + (screen.height / 2 - 40);
            var path = "/DownLoadFile.aspx?";
            var params = [];
            params.push("FilePath=" + escape(filePath));
            params.push("FileName=" + escape(fileName));
            params.push("p_open_type=_blank");
            params.push("random=" + Math.random());
            path = path + params.join("&");

            $("#ifrDown").attr("src", path);
        }

        function copyVss() {
            var rowData = $('#dg').datagrid('getSelections');
            if (rowData.length == 0) {
                $.messager.alert('提示', '请选择要复制的项！', 'info');
            }
            else if (rowData.length > 1) {
                $.messager.alert('提示', '只能选择一项进行复制！', 'info');
            }
            else {
                copy(rowData[0].VSSAddress);
            }
           
        }

        function searchSrcCode() {
            $('#dg').datagrid('load', {
                CustomerName: $("#seaCstName").val(),
                CustomerArea: $("#seaArea").val()
            });
        }

        var copy = function (content) {
            if (copy2Clipboard(content) != false) {
                $.messager.alert('提示', 'VSS&TFS地址已复制到剪切板！','info');
            }
        }

        var copy2Clipboard = function (txt) {
            if (window.clipboardData) {
                window.clipboardData.clearData();
                window.clipboardData.setData("Text", txt);
            } else if (navigator.userAgent.indexOf("Opera") != -1) {
                window.location = txt;
            } else if (window.netscape) {
                try {
                    netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
                }
                catch (e) {
                    $.messager.alert('提示', '您使用的浏览器不支持此复制功能，请使用Ctrl+C或鼠标右键。','info');
                    return false;
                }
                var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
                if (!clip) return;
                var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
                if (!trans) return;
                trans.addDataFlavor('text/unicode');
                var str = new Object();
                var len = new Object();
                var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
                var copytext = txt; str.data = copytext;
                trans.setTransferData("text/unicode", str, copytext.length * 2);
                var clipid = Components.interfaces.nsIClipboard;
                if (!clip) return false;
                clip.setData(trans, null, clipid.kGlobalClipboard);
                return true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <iframe id="ifrDown" src="../DownLoadFile.aspx" style="display: none;"></iframe>
    <table id="dg" fit="true">
    </table>
    <div id="dlg" style="width: 530px; height: 400px; padding: 10px 20px;display:none;">
       <form id="fm" method="post" novalidate>  
        <table>
            <tr>
                <td align="right" style="width: 80px;">
                    客户名称:
                </td>
                <td align="left">
                    <input id="CustomerName" name="CustomerName" maxlength="20" style="width: 150px;" class="easyui-validatebox" required="true" onblur="checkCstName();" />
                </td>
                <td align="right" style="width: 80px;">
                    区域:
                </td>
                <td align="left">
                    <input name="CustomerArea" maxlength="20" style="width: 150px;" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    VSS地址:
                </td>
                <td align="left" colspan="3">
                    <input id="VSSAddress" name="VSSAddress" style="width: 392px" maxlength="500" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    ERP版本:
                </td>
                <td align="left">
                    <input id="ProjVersion" name="ProjVersion" maxlength="10" style="width: 150px;" />
                </td>
                <td align="right">
                    端口:
                </td>
                <td align="left">
                    <input id="ServerPort" name="ServerPort" value="1433" maxlength="10" style="width: 150px;" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    数据库名:
                </td>
                <td align="left">
                    <input id="DBName" name="DBName" maxlength="100" style="width: 150px;" />
                </td>
                <td align="right">
                    应用程序名:
                </td>
                <td align="left">
                    <input id="ApplicationName" name="ApplicationName" maxlength="100" style="width: 150px;" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    服务器地址:
                </td>
                <td align="left" colspan="3">
                    <input id="ServerName" name="ServerName" maxlength="50" style="width: 392px;" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    用户名:
                </td>
                <td align="left">
                    <input id="UserName" name="UserName" maxlength="50" style="width: 150px;" value="sa" />
                </td>
                <td align="right">
                    密码:
                </td>
                <td align="left" colspan="3">
                    <input id="UserPwd" name="UserPwd" maxlength="50" style="width: 150px;" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    备注:
                </td>
                <td align="left" colspan="3 ">
                    <textarea id="Remark" name="Remark" rows="8" style="width: 390px;"></textarea>
                </td>
            </tr>
        </table>
        </form>
    </div>
  
    <!--工具栏-->
    <div id="tb" style="padding: 5px; height: auto;display:none;">
        <div style="margin-bottom: 5px">
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" onclick="add()">新增</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-edit" onclick="edit()">编辑</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-cut" onclick="copyVss()">复制地址</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-save" onclick="downReg()">下载REG</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-remove" onclick="del()">删除</a>
            区域:
            <input id="seaArea" style="width: 100px" />
            客户名称:
            <input id="seaCstName" style="width: 100px" />
            <a href="#" class="easyui-linkbutton" iconcls="icon-search" onclick="searchSrcCode()">
                查询</a>
        </div>
    </div>
</asp:Content>
