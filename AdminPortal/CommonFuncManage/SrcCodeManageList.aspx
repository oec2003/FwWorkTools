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
        });

        var _url ="" ;
        //添加
        function add() {
            $('#dlg').dialog('open').dialog('setTitle', '新增');
            $('#fm').form('clear');
            url = "../Ajax/SrcCodeManageAjax.ashx?Method=SaveSrcCodeManage"; 
        }

        function edit() {
            var rowData = $('#dg').datagrid('getSelections');
            if (rowData.length == 0) {
                $.messager.alert('提示', '请选择要编辑的项！');
            }
            else if (rowData.length > 1) {
                $.messager.alert('提示', '只能选择一项进行编辑！');
            }
            else {
                var row = $('#dg').datagrid('getSelected'); 
                $('#dlg').dialog('open').dialog('setTitle', '编辑');
                $('#fm').form('load', row);
                url = "../Ajax/SrcCodeManageAjax.ashx?Method=SaveSrcCodeManage&id=" + row.SrcCodeManageID;
            }
        }

        function del() {
            var rowData = $('#dg').datagrid('getSelections');
            if (rowData.length == 0) {
                $.messager.alert('提示', '请选择要删除的项！');
            }
            else {
                $.messager.confirm('删除提示', '确认删除吗?', function (r) {
                    if (r) {
                        var ids = "";
                        for (var i = 0; i < rowData.length; i++) {
                            ids += rowData[i].FeedBackLogID + ",";
                        }
                        ids = ids.substr(0, ids.length - 1);
                        $.ajax({
                            type: "POST",
                            dataType: "json",
                            //cache:true,
                            url: "../Ajax/FeedBackLogAjax.ashx?Method=DelFeedBackLog",
                            data: { IDs: ids },
                            success: function (data) {
                                if (data.Flag[0].Status == 1) {
                                    $.messager.alert('提示', '删除成功');
                                    $("#dg").datagrid('reload');
                                }
                                else {
                                    $.messager.alert('错误', '删除失败!', 'error');
                                }
                            },
                            error: function (data) {
                                debugger;
                                $.messager.alert('错误', '删除失败!', 'error');
                            }
                        });
                    }
                });
            }
        }

        //保存
        function saveSrc(mode, id) {
            $('#fm').form('submit', {
                url: url,
                onSubmit: function () {
                    return $(this).form('validate');
                },
                success: function (result) {
                    debugger;
                    var result = eval('(' + result + ')');
                    if (result.errorMsg) {
                        $.messager.show({
                            title: 'Error',
                            msg: result.errorMsg
                        });
                    } else {
                        $('#dlg').dialog('close');      // close the dialog  
                        $('#dg').datagrid('reload');    // reload the user data  
                    }
                }
            });  
        }

        function downReg() {

        }

        function copyVss() {

        }

        function searchSrcCode() {

        }



       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <iframe id="ifrDown" src="../DownLoadFile.aspx" style="display: none;"></iframe>
    <table id="dg" fit="true">
    </table>
    <div id="dlg" class="easyui-dialog" style="width: 550px; height: 400px; padding: 10px 20px;"
        closed="true" buttons="#dlg-buttons">
       <form id="fm" method="post" novalidate>  
        <table>
            <tr>
                <td align="right" style="width: 80px;">
                    客户名称:
                </td>
                <td align="left">
                    <input name="CustomerName" maxlength="20" style="width: 150px;" class="easyui-validatebox" required="true" />
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
                    <input id="VSSAddress" name="VSSAddress" style="width: 390px" maxlength="500" />
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
                    <input id="ServerName" name="ServerName" maxlength="50" style="width: 390px;" />
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
            <%--  <tr>
                    <td align="right">
                    </td>
                    <td style="padding-top: 10px;" align="right" colspan="3">
                        <input style="margin-right: 5px;" type="button" id="btnSave" value="保存" />
                        <input id="btnCancel" type="button" value="取消" />
                    </td>
                </tr>--%>
        </table>
        </form>
    </div>
    <div id="dlg-buttons">
        <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-ok" onclick="saveSrc()">
            保存</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-cancel"
                onclick="javascript:$('#dlg').dialog('close')">取消</a>
    </div>
    <!--工具栏-->
    <div id="tb" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" onclick="add()">新增</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-edit" onclick="edit()">编辑</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-cut" onclick="copyVss()">复制地址</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-save" onclick="downReg()">下载REG</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-remove" onclick="del()">删除</a>
            任务编号:
            <input id="seaTaskNo" style="width: 100px" />
            <a href="#" class="easyui-linkbutton" iconcls="icon-search" onclick="searchSrcCode()">
                查询</a>
        </div>
    </div>
</asp:Content>
