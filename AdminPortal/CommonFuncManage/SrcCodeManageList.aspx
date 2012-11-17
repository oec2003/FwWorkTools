<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="SrcCodeManageList.aspx.cs" Inherits="FW.WT.AdminPortal.CommonFuncManage.SrcCodeManageList" %>
 <%@ MasterType VirtualPath="~/Main.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        var _url = '/WT/AdminPortal/Ajax/SrcCodeManageAjax.ajax';
        var _srcCodeManageID;
        var _mode;
        $(document).ready(function () {
            var theme = getTheme();

            var getAdapter = function () {
                // debugger;
                var dataSource = "";
                $.ajax({
                    type: 'GET',
                    async: false,
                    url: _url,
                    data: { action: 'GetSrcCodeManage', rnd: Math.random() },
                    success: function (data) {
                        dataSource = data;
                    }
                });

                var source =
                {
                    datatype: "json",
                    localdata: dataSource,
                    datafields: [
                        { name: 'SrcCodeManageID' },
                        { name: 'CustomerName' },
                        { name: 'CustomerArea' },
                        { name: 'VSSAddress' },
                        { name: 'ServerName' },
                        { name: 'ServerPort' },
                        { name: 'DBName' },
                        { name: 'ApplicationName' },
                        { name: 'UserName' },
                        { name: 'UserPwd' },
                        { name: 'Remark' },
                        { name: 'ProjVersion' },
                    ],
                    pager: function (pagenum, pagesize, oldpagenum) {
                        // callback called when a page or page size is changed.
                        refreshGrid();
                    },

                    addrow: function (rowid, rowdata) {
                        // synchronize with the server - send insert command
                        //rowdata = JSON.stringify(rowdata);
                        var jsonData = rowdata.CustomerName + "|" + rowdata.CustomerArea + "|" + rowdata.ProjVersion + "|" +
                        rowdata.VSSAddress + "|" + rowdata.DBName + "|" + rowdata.ServerPort + "|" + rowdata.ServerName + "|" +
                        rowdata.ApplicationName + "|" + rowdata.UserName + "|" + rowdata.UserPwd + "|" + rowdata.Remark;
                        $.ajax({
                            type: 'POST',
                            async: false,
                            url: _url,
                            data: { action: 'AddSrcCodeManage', JsonData: jsonData, rnd: Math.random() },
                            success: function (data) {
                                //debugger;
                                if (data == "ERROR") {
                                    operatorSuccess("保存失败！", "");
                                }
                                else if (data == "OK") {
                                    operatorSuccess("保存成功！", "");
                                    refreshGrid();
                                }
                                else {
                                    operatorSuccess(data, "");
                                }
                            }
                        });
                    },
                    deleterow: function (id) {

                        $.ajax({
                            type: 'POST',
                            async: false,
                            url: _url,
                            data: { action: 'DelSrcCodeManage', SrcCodeManageID: id, rnd: Math.random() },
                            success: function (data) {

                                if (data == "ERROR") {
                                    operatorSuccess("删除失败！", "");
                                }
                                else if (data == "OK") {
                                    operatorSuccess("删除成功！", "");
                                    refreshGrid();
                                }
                                else {
                                    operatorSuccess(data, "");
                                }
                            }
                        });
                    },
                    updaterow: function (rowid, rowdata) {
                        // synchronize with the server - send update command
                        var jsonData = rowdata.CustomerName + "|" + rowdata.CustomerArea + "|" + rowdata.ProjVersion + "|" +
                        rowdata.VSSAddress + "|" + rowdata.DBName + "|" + rowdata.ServerPort + "|" + rowdata.ServerName + "|" +
                        rowdata.ApplicationName + "|" + rowdata.UserName + "|" + rowdata.UserPwd + "|" + rowdata.Remark + "|" +
                        _srcCodeManageID;
                        $.ajax({
                            type: 'POST',
                            async: false,
                            url: _url,
                            data: { action: 'UpdateSrcCodeManage', JsonData: jsonData, rnd: Math.random() },
                            success: function (data) {
                                //debugger;
                                if (data == "ERROR") {
                                    operatorSuccess("保存失败！", "");
                                }
                                else if (data == "OK") {
                                    operatorSuccess("保存成功！", "");
                                    refreshGrid();
                                }
                                else {
                                    operatorSuccess(data, "");
                                }
                            }
                        });
                    }
                };

                var dataAdapter = new $.jqx.dataAdapter(source);
                return dataAdapter;
            }
            var editrow = -1;
            var dataS = getAdapter();
            $("#jqxgrid").jqxGrid(
            {
                width: "99%",
                autoheight: true,
                source: dataS,
                showfilterrow: true,
                filterable: true,
                sortable: true,
                pageable: true,
                columnsresize: true,
                pagesize: 25,
                pagesizeoptions: ['10', '25', '50', '100'],
                horizontalscrollbarstep: 5,
                theme: theme,
                selectionmode: 'singlerow',
                columns: [
                 { text: '修改', sortable: false, filterable: false, columntype: 'button', width: 70,
                     cellsrenderer: function () { return "修改"; },
                     buttonclick: function (row) {
                         // open the popup window when the user clicks a button.
                         editrow = row;
                         var offset = $("#jqxgrid").offset();
                         $("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 60, y: parseInt(offset.top) + 60} });
                         // get the clicked row's data and initialize the input fields.
                         var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', editrow);
                         $("#txtCustomerName").val(dataRecord.CustomerName);
                         $("#txtCustomerArea").val(dataRecord.CustomerArea);
                         $("#txtProjVersion").val(dataRecord.ProjVersion);
                         $("#txtVSSAddress").val(dataRecord.VSSAddress);
                         $("#txtDBName").val(dataRecord.DBName);
                         $("#txtServerPort").val(dataRecord.ServerPort);
                         $("#txtServerName").val(dataRecord.ServerName);
                         $("#txtApplicationName").val(dataRecord.ApplicationName);
                         $("#txtUserName").val(dataRecord.UserName);
                         $("#txtUserPwd").val(dataRecord.UserPwd);
                         $("#txtRemark").val(dataRecord.Remark);
                         _mode = 2;
                         _srcCodeManageID = dataRecord.SrcCodeManageID;
                         // show the popup window.
                         $("#popupWindow").jqxWindow('show');
                     }
                 },
                 { text: '删除', sortable: false, filterable: false, columntype: 'button', width: 70,
                     cellsrenderer: function () { return "删除"; },
                     buttonclick: function (row) {
                         // open the popup window when the user clicks a button.
                         var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', row);

                         Boxy.confirm("确认删除吗？", function () { $('#jqxgrid').jqxGrid('deleterow', dataRecord.SrcCodeManageID, row); }, null);
                     }
                 },
                  { text: '复制VSS&TFS地址', sortable: false, filterable: false, width: 120, columntype: 'button',
                      cellsrenderer: function () {
                          return "复制VSS&TFS地址";
                      },
                      buttonclick: function (row) {
                          //open the popup window when the user clicks a button.
                          var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', row);
                          //window.clipboardData.setData('text', dataRecord.VSSAddress);
                          copy(dataRecord.VSSAddress);
                          // operatorSuccess("复制成功！", "");

                      }
                  },
                   { text: '下载Reg', sortable: false, filterable: false, width: 100, columntype: 'button',
                       cellsrenderer: function () { return "下载Reg"; },
                       buttonclick: function (row) {
                           //debugger;
                           var dataRecord = $("#jqxgrid").jqxGrid('getrowdata', row);
                           var jsonData = dataRecord.DBName + "|" + dataRecord.ApplicationName + "|" + dataRecord.ServerName + "|" +
                            dataRecord.UserName + "|" + dataRecord.UserPwd + "|" + dataRecord.ServerPort + "|" + dataRecord.CustomerName;
                           $.ajax({
                               type: 'POST',
                               async: false,
                               url: _url,
                               data: { action: 'RegisterAndDownLoadReg', JsonData: jsonData, rnd: Math.random() },
                               success: function (data) {
                                   //debugger;
                                   if (data == "ERROR") {
                                       operatorSuccess("注册&下载失败！", "");
                                   }
                                   else if (data == "OK") {
                                       //operatorSuccess("注册&下载成功！", "");
                                   }
                                   else {
                                       //operatorSuccess(data, "");

                                       if (data.split("|").length == 2) {
                                           var filePath = data.split("|")[0];
                                           var fileName = data.split("|")[1];

                                           downReg(filePath, fileName);
                                       }

                                   }
                               }
                           });
                       }
                   },
                  { text: 'ID', datafield: 'SrcCodeManageID', hidden: "false", columntype: 'textbox', filtertype: 'textbox', width: 10 },
                  { text: '区域', datafield: 'CustomerArea', columntype: 'textbox', filtertype: 'textbox', width: 80 },
                  { text: '客户名称', datafield: 'CustomerName', columntype: 'textbox', filtertype: 'textbox', width: 120,
                      validation: function (cell, value) {
                          if ($.trim(value) == "") {
                              return { result: false, message: "客户名称不能为空！" };
                          }
                          return true;
                      }
                  },
                  
                  { text: 'ERP版本', datafield: 'ProjVersion', columntype: 'textbox', filtertype: 'textbox', width: 80 },
                  { text: 'VSS&TFS地址', datafield: 'VSSAddress', columntype: 'textbox', filtertype: 'textbox', width: 220 },
                  { text: '数据库名', datafield: 'DBName', columntype: 'textbox', filtertype: 'textbox', width: 120 },
                  { text: '端口', datafield: 'ServerPort', columntype: 'textbox', filtertype: 'textbox', width: 50 },
                  { text: '服务器地址', datafield: 'ServerName', columntype: 'textbox', filtertype: 'textbox', width: 120 },
                  { text: '应用程序名', datafield: 'ApplicationName', columntype: 'textbox', filtertype: 'textbox', width: 120 },
                  { text: '用户名', datafield: 'UserName', columntype: 'textbox', filtertype: 'textbox', width: 80 },
                  { text: '密码', datafield: 'UserPwd', columntype: 'textbox', filtertype: 'textbox', width: 80 },
                  { text: '备注', datafield: 'Remark', columntype: 'textbox', filtertype: 'textbox'}
                ]
            });

            //隐藏列
//              $('#jqxgrid').jqxGrid('hidecolumn', 'UserName');
//              $('#jqxgrid').jqxGrid('hidecolumn', 'ApplicationName');
//              $('#jqxgrid').jqxGrid('hidecolumn', 'ServerName');
//              $('#jqxgrid').jqxGrid('hidecolumn', 'DBName');
//              $('#jqxgrid').jqxGrid('hidecolumn', 'ServerPort');
//              $('#jqxgrid').jqxGrid('hidecolumn', 'UserPwd');


            $("#btnAdd").jqxButton({ width: 70, theme: theme });
            $("#btnRefresh").jqxButton({ width: 70, theme: theme });

            $("#btnAdd").click(function () {
                clearInput();
                _mode = 1;
                var offset = $("#jqxgrid").offset();
                $("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 60, y: parseInt(offset.top) + 60} });
                $("#popupWindow").jqxWindow('show');
            });

            $("#btnRefresh").click(function (event) {
                refreshGrid();
            });

            // initialize the popup window and buttons.
            $("#popupWindow").jqxWindow({ width: 500, resizable: false, theme: theme, isModal: true, autoOpen: false, cancelButton: $("#Cancel"), modalOpacity: 0.5 });
            $("#btnCancel").jqxButton({ theme: theme });
            $("#btnSave").jqxButton({ theme: theme });
            $("#btnCancel").click(function () {
                $("#popupWindow").jqxWindow('hide');
            });
            // update the edited row when the user clicks the 'Save' button.
            $("#btnSave").click(function () {
                if ($("#txtCustomerName").val() == "") {
                    //operatorSuccess("客户名称不能为空！", "");
                    alert("客户名称不能为空!");
                    return false;
                }
                var srcCodeManageID = "";
                if (_mode == "2") {
                    srcCodeManageID = _srcCodeManageID;
                }
                $.ajax({
                    type: 'POST',
                    async: false,
                    url: _url,
                    data: { action: 'IsExistName', CustomerName: $("#txtCustomerName").val(), Mode: _mode, SrcCodeManageID: srcCodeManageID, rnd: Math.random() },
                    success: function (data) {
                        //debugger;
                        if (data == "ERROR") {
                            alert("出错啦!");
                            return false;
                        }
                        else if (data == "NO") {
                            alert("客户名称不能重复，请重新输入!");
                            return false;
                        }
                        else {
                            var row = { CustomerName: $("#txtCustomerName").val(), CustomerArea: $("#txtCustomerArea").val(),
                                VSSAddress: $("#txtVSSAddress").val(), ProjVersion: $("#txtProjVersion").val(),
                                DBName: $("#txtDBName").val(), ServerPort: $("#txtServerPort").val(),
                                ServerName: $("#txtServerName").val(), ApplicationName: $("#txtApplicationName").val(),
                                UserName: $("#txtUserName").val(), UserPwd: $("#txtUserPwd").val(),
                                Remark: $("#txtRemark").val()
                            };
                            if (_mode == 1) {
                                $('#jqxgrid').jqxGrid('addrow', null, row);
                            }
                            else if (_mode == 2) {
                                $('#jqxgrid').jqxGrid('updaterow', editrow, row);
                            }
                            $("#popupWindow").jqxWindow('hide');
                        }
                    }
                });
            });

            var refreshGrid = function () {
                $("#jqxgrid").jqxGrid({ source: getAdapter() });
            }

            var clearInput = function () {
                $("#popupWindow").find("input").each(function () {
                    if ($(this)[0].type == "text") {
                        $(this).val("");
                    }
                });
                $("#txtRemark").val("");

                $("#txtServerPort").val("1433");
                $("#txtUserName").val("sa");

            }

            var downReg = function (filePath, fileName) {
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

            var copy = function (content) {
                if (copy2Clipboard(content) != false) {
                    operatorSuccess("VSS地址已复制到剪切板！", "");
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
                        alert("您使用的浏览器不支持此复制功能，请使用Ctrl+C或鼠标右键。");
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

        });
      
    </script>
    <style type="text/css">
        .btnMenu
        {
            float: left;
            margin-left: 0px;
            margin-right: 10px;
            margin-top: 10px;
            margin-bottom: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <iframe id="ifrDown" src="../DownLoadFile.aspx"  style="display:none;"></iframe>
    <%--<div id="divTip">
        <span style="color: Red;">注：点击【注册&下载Reg】会将数据库信息添加到注册表并在C盘根目录生成注册表导出文件，文件名为客户名称。</span>
    </div>--%>
   
    <div class="btnMenu">
        <input type="button" id="btnAdd" value="添加" />

        <input type="button" id="btnRefresh" value="刷新" />
    </div>
    <div id="jqxgrid">
    </div>
    <div id="popupWindow" style="display:none;">
        <div>
            添加/修改</div>
        <div style="overflow: hidden;">
            <table>
                <tr>
                    <td align="right" style="width: 80px;">
                        客户名称:
                    </td>
                    <td align="left">
                        <input id="txtCustomerName" maxlength="20" style="width: 150px;" />
                    </td>
                    <td align="right" style="width: 80px;">
                        区域:
                    </td>
                    <td align="left">
                        <input id="txtCustomerArea" maxlength="20" style="width: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        VSS地址:
                    </td>
                    <td align="left" colspan="3">
                        <input id="txtVSSAddress" style="width: 390px" maxlength="500" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ERP版本:
                    </td>
                    <td align="left">
                        <input id="txtProjVersion" maxlength="10" style="width: 150px;" />
                    </td>
                    <td align="right">
                        端口:
                    </td>
                    <td align="left">
                        <input id="txtServerPort" value="1433" maxlength="10" style="width: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        数据库名:
                    </td>
                    <td align="left">
                        <input id="txtDBName" maxlength="100" style="width: 150px;" />
                    </td>
                    <td align="right">
                        应用程序名:
                    </td>
                    <td align="left">
                        <input id="txtApplicationName" maxlength="100" style="width: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        服务器地址:
                    </td>
                    <td align="left" colspan="3">
                        <input id="txtServerName" maxlength="50" style="width: 390px;" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        用户名:
                    </td>
                    <td align="left">
                        <input id="txtUserName" maxlength="50" style="width: 150px;" value="sa" />
                    </td>
                    <td align="right">
                        密码:
                    </td>
                    <td align="left" colspan="3">
                        <input id="txtUserPwd" maxlength="50" style="width: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        备注:
                    </td>
                    <td align="left" colspan="3 ">
                        <textarea id="txtRemark" rows="5" style="width: 390px;"></textarea>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                    </td>
                    <td style="padding-top: 10px;" align="right" colspan="3">
                        <input style="margin-right: 5px;" type="button" id="btnSave" value="保存" />
                        <input id="btnCancel" type="button" value="取消" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
