<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="FeedBackLogList.aspx.cs" Inherits="FW.WT.AdminPortal.CommonFuncManage.FeedBackLogList" %>
 <%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Scripts/jqueryeasyui/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../Scripts/jqueryeasyui/themes/icon.css" />
    <script type="text/javascript" src="../Scripts/jqueryeasyui/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="../Scripts/jqueryeasyui/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../Scripts/jqueryeasyui/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src=""></script>
    <script type="text/javascript">
        $(function () {
            $('#dg').datagrid({
                title: '一线反馈记录列表',
                //iconCls: 'icon-save',
                collapsible: false,
                fitColumns: false,
                singleSelect: false,
                remoteSort: false,
                sortName: 'EndDate',
                sortOrder: 'desc',
                nowrap: true,
                striped: true,
                method: 'get',
                loadMsg: '正在加载数据...',
                url: '../Ajax/FeedBackLogAjax.ashx?Method=GetFeedBackLog',
                frozenColumns: [[
                    { field: 'ck', checkbox: true }
			    ]],
                columns: [[
                    { field: 'FeedBackDate', title: '反馈日期', width: 100, sortable: true, formatter: function (value, row, index) {
                        if (value != null) return value.substr(0, 10);
                    }
                    },
                    { field: 'CstName', title: '客户名称', width: 100, sortable: true },
                    { field: 'TaskNo', title: '任务编号', width: 100, sortable: true },
                    { field: 'FeedBackContent', title: '反馈内容', width: 220 },
				    { field: 'CsZrr', title: '测试/需求跟进人', width: 120, sortable: true },
				    { field: 'CsYzResult', title: '测试/需求验证结果', width: 220, sortable: false },
				    { field: 'IsKfCl', title: '开发是否处理', width: 100 },
                    { field: 'KfZrr', title: '开发责任人', width: 100 },
                    { field: 'EndDate', title: '处理期限', width: 100, formatter: function (value, row, index) {
                        if (value != null) return value.substr(0, 10);
                    }
                    },
                    { field: 'KfClDate', title: '开发处理时间', width: 100, sortable: true },
                    { field: 'Wtyy', title: '问题原因', width: 220 },
			    ]],
                pagination: true,
                pageNumber: 1,
                rownumbers: true,
                toolbar:'#tb',
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

        function openDialog(mode,id) {
            $("#dialogAddFeedBack").show();
            $("#dialogAddFeedBack").attr("title", "添加反馈");
            $("#dialogAddFeedBack").dialog({
                width: 520,
                height: 600,
                draggable: true,
                resizable: false,
                modal: true,
                buttons:
                    [
                        {
                            text: '提交',
                            iconCls: 'icon-ok',
                            handler: function () {
                                saveFeedBackLog(mode,id);
                            }
                        },
				        {
				            text: '取消',
				            handler: function () {
				                $('#dialogAddFeedBack').dialog('close');
				            }
				        }
				    ]
            });
				}

		function searchFB() {
			$('#dg').datagrid('load', {
				TaskNo: $("#seaTaskNo").val()
			});
		}

        //添加
        function add() {
            $("#CstName").val("");
            $("#TaskNo").val(""),
            $("#FeedBackContent").val("");
            $('#CsZrr').combobox('select', '');
            $("#CsYzResult").val("");
            $('#IsKfCl').combobox('select', '');
            $('#KfZrr').combobox('select', '');
            $('#EndDate').datebox('setValue', '');
            $("#KfClDate").val("");
            $("#Wtyy").val("");
            $('#FeedBackDate').datebox('setValue', '');
            openDialog(1,"");
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

        function edit() {
            var rowData = $('#dg').datagrid('getSelections');
            if (rowData.length == 0) {
                $.messager.alert('提示', '请选择要编辑的项！');
            }
            else if (rowData.length > 1) {
                $.messager.alert('提示', '只能选择一项进行编辑！');
            }
            else {
                $("#CstName").val(rowData[0].CstName);
                $("#TaskNo").val(rowData[0].TaskNo),
                $("#FeedBackContent").val(rowData[0].FeedBackContent);
                $('#CsZrr').combobox('select', rowData[0].CsZrr);
                $("#CsYzResult").val(rowData[0].CsYzResult);
                $('#IsKfCl').combobox('select', rowData[0].IsKfCl);
                $('#KfZrr').combobox('select', rowData[0].KfZrr);
                if (rowData[0].EndDate != null && rowData[0].EndDate.length > 10) {
                    $('#EndDate').datebox('setValue', rowData[0].EndDate.substr(0, 10));
                }
                else {
                    $('#EndDate').datebox('setValue', "");
                }
                
                $("#KfClDate").val(rowData[0].KfClDate);
                $("#Wtyy").val(rowData[0].Wtyy);
                if (rowData[0].EndDate != null && rowData[0].EndDate.length > 10) {
                    $('#FeedBackDate').datebox('setValue', rowData[0].FeedBackDate.substr(0, 10));
                }
                else {
                    $('#FeedBackDate').datebox('setValue',"");
                }
               
                openDialog(2, rowData[0].FeedBackLogID);
            }
        }

        //保存
        function saveFeedBackLog(mode,id) {
            $.ajax({
                type: "POST",
                dataType: "json",
                //cache:true,
                url: "../Ajax/FeedBackLogAjax.ashx?Method=SaveFeedBackLog",
                data: { FeedBackDate: $("#FeedBackDate").datebox("getValue"),
                    CstName: $("#CstName").val(),
                    TaskNo: $("#TaskNo").val(),
                    FeedBackContent: $("#FeedBackContent").val(),
                    CsZrr: $("#CsZrr").combobox("getValue"),
                    CsYzResult: $("#CsYzResult").val(),
                    IsKfCl: $("#IsKfCl").combobox("getValue"),
                    KfZrr: $("#KfZrr").combobox("getValue"),
                    EndDate: $("#EndDate").datebox("getValue"),
                    KfClDate: $("#KfClDate").val(),
                    Wtyy: $("#Wtyy").val(),
                    Mode:mode,
                    ID:id
                },

                success: function (data) {
                    if (data.Flag[0].Status == 1) {
                        $("#dialogAddFeedBack").hide();
                        $("#dialogAddFeedBack").dialog("close");
                        $.messager.alert('提示', '保存成功');
                        $("#dg").datagrid('reload');
                    }
                    else {
                        $.messager.alert('错误', '保存失败!', 'error');
                    }
                },
                error: function (data) {
                    $.messager.alert('错误', '保存失败!', 'error');
                }
            });
        }

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
   
        <table id="dg" fit="true">
        </table>
    <form id="fb" method="post" action="../Ajax/FeedBackLogAjax.ashx?Method=SaveFeedBackLog">

     <div id="dialogAddFeedBack" icon="icon-add" title="添加一线反馈" style="display: none;">
            <table>
                <tr>
                    <td align="right" style="width: 80px;">
                        任务编号:
                    </td>
                    <td align="left">
                        <input id="TaskNo" name="TaskNo" maxlength="20"  style="width: 150px;" />
                    </td>
                    <td align="right" style="width: 80px;">
                        客户名称:
                    </td>
                    <td align="left">
                        <input id="CstName" name="CstName" maxlength="20" style="width: 150px;" />
                    </td>
                </tr>
                 <tr>
                    <td align="right">
                        反馈日期:
                    </td>
                    <td align="left">
                        <input id="FeedBackDate" name="FeedBackDate" class="easyui-datebox" maxlength="10" style="width: 150px;" />
                    </td>
                    <td align="right">
                        测试跟进人:
                    </td>
                    <td align="left">
                        <select id="CsZrr" class="easyui-combobox" name="CsZrr" style="width:150px;">  
                            <option value=""></option>   
                            <option value="邓莹">邓莹</option>  
                            <option value="黎芳">黎芳</option>  
                            <option value="易云">易云</option>  
                        </select>  
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        反馈内容:
                    </td>
                    <td align="left" colspan="3">
                       <textarea id="FeedBackContent" name="FeedBackContent" rows="8" style="width: 390px;"></textarea>
                    </td>
                </tr>
                    <tr>
                    <td align="right">
                        测试验证结果:
                    </td>
                    <td align="left" colspan="3">
                       <textarea id="CsYzResult" name="CsYzResult" rows="8" style="width: 390px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        开发是否处理:
                    </td>
                    <td align="left">
                          <select id="IsKfCl" class="easyui-combobox" name="IsKfCl" style="width:150px;"> 
                            <option value=""></option>   
                            <option value="是">是</option>  
                            <option value="否">否</option>  
                        </select>  
                    </td>
                    <td align="right">
                        开发责任人:
                    </td>
                    <td align="left">
                         <select id="KfZrr" class="easyui-combobox" name="KfZrr" style="width:150px;">  
                            <option value=""></option>   
                            <option value="刘玺">刘玺</option>  
                            <option value="冯威">冯威</option>  
                            <option value="周晓星">周晓星</option>  
                            <option value="付登山">付登山</option>  
                            <option value="郑云海">郑云海</option>  
                            <option value="付鹏">付鹏</option>  
                        </select>  
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        处理期限:
                    </td>
                    <td align="left">
                         <input id="EndDate"  class="easyui-datebox" name="EndDate" maxlength="10" style="width: 150px;" />
                    </td>
                    <td align="right">
                        开发处理时间:
                    </td>
                    <td align="left">
                        <input id="KfClDate" name="KfClDate" maxlength="100" style="width: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        问题原因:
                    </td>
                    <td align="left" colspan="3 ">
                        <textarea id="Wtyy" name="Wtyy" rows="8" style="width: 390px;"></textarea>
                    </td>
                </tr>
             
            </table>
    </div>
    </form>
   
   <!--工具栏-->
    <div id="tb" style="padding:5px;height:auto">  
        <div style="margin-bottom:5px">  
            <a href="#" class="easyui-linkbutton" iconCls="icon-add"  onclick="add()">新增</a>  
            <a href="#" class="easyui-linkbutton" iconCls="icon-edit"  onclick="edit()">编辑</a>   
            <a href="#" class="easyui-linkbutton" iconCls="icon-remove" onclick="del()">删除</a> 
   
            任务编号: <input id="seaTaskNo" style="width:100px" />  
            <a href="#" class="easyui-linkbutton" iconCls="icon-search" onclick="searchFB()">查询</a>  
        </div>  
        
    </div>  
</asp:Content>
