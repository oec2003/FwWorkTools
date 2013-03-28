<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="TypicalFuncList.aspx.cs" Inherits="FW.WT.AdminPortal.CommonFuncManage.TypicalFuncList" %>

<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Scripts/jqueryeasyui/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../Scripts/jqueryeasyui/themes/icon.css" />
    <script type="text/javascript">
        var _typicalFuncID;
        var _mode;
        $(function () {
            $('#dg').datagrid({
                title: '典型功能列表',
                //iconCls: 'icon-save',
                collapsible: false,
                fitColumns: true,
                singleSelect: false,
                remoteSort: false,
                sortName: 'CustomerName',
                sortOrder: 'desc',
                nowrap: true,
                striped: true,
                method: 'get',
                loadMsg: '正在加载数据...',
                url: '../Ajax/TypicalFuncAjax.ashx?Method=GetTypicalFunc',
                frozenColumns: [[
                    { field: 'ck', checkbox: true }
			    ]],
                columns: [[
                    { field: 'CustomerArea', title: '区域', width: 70, sortable: true },
                    { field: 'CustomerName', title: '客户名称', width: 120, sortable: true },
                    { field: 'TaskNo', title: '任务编号', width: 100, sortable: true },
                    { field: 'Title', title: '标题', width: 220, formatter: function (val, row) {
                        if (val == null) return "";
                        if (row.Url != "") {
                            return "<a href='" + row.Url + "' target='_blank' >" + val + "</a>";
                        }
                        else {
                            return val;
                        }
                    }
                    },
                    { field: 'Fzr', title: '负责人', width: 80 },
                    { field: 'Remark', title: '备注', width: 220 }
			    ]],
                pagination: true,
                pageNumber: 1,
                rownumbers: true,
                toolbar: '#tb',
                onLoadSuccess: function () {
                    //$("#roleManageGrid").datagrid('reload');
                },
                onClickRow: function (rowIndex, rowData) {

                }
            });

            $("#tb").show();
        });

        var _url = "";
        //添加
        function add() {
            _mode = "1";
            openDialog();
            $('#fm').form('clear');
            $("#ServerPort").val("1433");
            $("#UserName").val("sa");
            url = "../Ajax/TypicalFuncAjax.ashx?Method=SaveTypicalFunc&Mode=" + _mode;
        }

        function edit() {
            var rowData = $('#dg').datagrid('getSelections');
            if (rowData.length == 0) {
                $.messager.alert('提示', '请选择要编辑的项！', 'info');
            }
            else if (rowData.length > 1) {
                $.messager.alert('提示', '只能选择一项进行编辑！', 'info');
            }
            else {
                _mode = "2";
                var row = $('#dg').datagrid('getSelected');
                //$('#dlg').dialog('open').dialog('setTitle', '编辑');
                openDialog();
                $('#fm').form('load', row);
                _typicalFuncID = row.TypicalFuncID;

                url = "../Ajax/TypicalFuncAjax.ashx?Method=SaveTypicalFunc&ID=" + row.TypicalFuncID + "&Mode=" + _mode;
            }
        }

        function openDialog() {
            var title = "添加";
            if (_mode == "2") {
                title = "编辑";
            }
            $("#dlg").show();
            $("#dlg").attr("title", title);
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
                $.messager.alert('提示', '请选择要删除的项！', 'info');
            }
            else {
                $.messager.confirm('删除提示', '确认删除吗?', function (r) {
                    if (r) {
                        var ids = "";
                        for (var i = 0; i < rowData.length; i++) {
                            ids += rowData[i].TypicalFuncID + ",";
                        }
                        ids = ids.substr(0, ids.length - 1);
                        $.ajax({
                            type: "POST",
                            dataType: "text",
                            //cache:true,
                            url: "../Ajax/TypicalFuncAjax.ashx?Method=DelTypicalFunc",
                            data: { IDs: ids },
                            success: function (data) {
                                if (data == "Success") {
                                    $.messager.alert('提示', '删除成功', 'info');
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
                    if (data == "Success") {
                        $('#dlg').dialog('close');      // close the dialog  
                        $('#dg').datagrid('reload');    // reload the user data  
                        $.messager.alert('提示', '保存成功!', 'info');
                    }
                    else if (data == "Error") {
                        $.messager.alert('错误', '系统出错!', 'error');
                    }

                }
            });
        }

        //        var downRegFile = function (filePath, fileName) {
        //            strWindowPos = "height=0,width=0,left=" + (screen.width / 2 - 40) + ",top=" + (screen.height / 2 - 40);
        //            var path = "/DownLoadFile.aspx?";
        //            var params = [];
        //            params.push("FilePath=" + escape(filePath));
        //            params.push("FileName=" + escape(fileName));
        //            params.push("p_open_type=_blank");
        //            params.push("random=" + Math.random());
        //            path = path + params.join("&");

        //            $("#ifrDown").attr("src", path);
        //        }
        function searchTypicalFunc() {
            $('#dg').datagrid('load', {
                CustomerName: $("#seaCstName").val(),
                CustomerArea: $("#seaArea").val(),
                TaskNo: $("#TaskNo").val()
            });
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <iframe id="ifrDown" src="../DownLoadFile.aspx" style="display: none;"></iframe>
    <table id="dg" fit="true">
    </table>
    <div id="dlg" style="width: 520px; height: 400px; padding: 10px 20px; display: none;">
        <form id="fm" method="post" novalidate>
        <table>
            <tr>
                <td align="right">
                    标题:
                </td>
                <td align="left" colspan="3">
                    <input id="Title" name="Title" style="width: 392px" class="easyui-validatebox" required="true"
                        maxlength="100" />
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 80px;">
                    任务编号:
                </td>
                <td align="left">
                    <input id="TaskNo" name="TaskNo" maxlength="20" style="width: 150px;" />
                </td>
                <td align="right" style="width: 80px;">
                    负责人:
                </td>
                <td align="left">
                    <input name="Fzr" maxlength="20" style="width: 150px;" />
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 80px;">
                    区域:
                </td>
                <td align="left">
                    <input name="CustomerArea" maxlength="20" style="width: 150px;" />
                </td>
                <td align="right" style="width: 80px;">
                    客户名称:
                </td>
                <td align="left">
                    <input id="CustomerName" name="CustomerName" maxlength="20" style="width: 150px;"
                        class="easyui-validatebox" required="true" onblur="checkCstName(this);" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    链接地址:
                </td>
                <td align="left" colspan="3">
                    <input id="Url" name="Url" style="width: 392px" maxlength="100" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    备注:
                </td>
                <td align="left" colspan="3 ">
                    <textarea id="Remark" name="Remark" rows="10" style="width: 390px;"></textarea>
                </td>
            </tr>
        </table>
        </form>
    </div>
    <!--工具栏-->
    <div id="tb" style="padding: 5px; height: auto; display: none;">
        <div style="margin-bottom: 5px">
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" onclick="add()">新增</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-edit" onclick="edit()">编辑</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-remove" onclick="del()">删除</a>
            区域:
            <input id="seaArea" style="width: 100px" />
            客户名称:
            <input id="seaCstName" style="width: 100px" />
            任务编号:
            <input id="seaTaskNo" style="width: 100px" />
            <a href="#" class="easyui-linkbutton" iconcls="icon-search" onclick="searchTypicalFunc()">
                查询</a>
        </div>
    </div>
</asp:Content>
