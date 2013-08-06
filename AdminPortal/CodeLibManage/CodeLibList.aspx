<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CodeLibList.aspx.cs" Inherits="FW.WT.AdminPortal.CodeLibManage.CodeLibList" %>

<%@ MasterType VirtualPath="~/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/CodeLib/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/CodeLib/style.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" type="text/css" href="../Scripts/jqueryeasyui/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../Scripts/jqueryeasyui/themes/icon.css" />

    <link href="../Scripts/kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/kindeditor/kindeditor-min.js" type="text/javascript"></script>
    <script src="../Scripts/kindeditor/lang/zh_CN.js" type="text/javascript"></script>

    <script type="text/javascript">

        var _url;
        var _mode;
        var _editor;

        $(function () {
            $("#tb").show();
        });

        //添加
        function add() {
            _mode = "1";
            //$('#dlg').dialog('open').dialog('setTitle', '新增');
            openDialog();

            _editor= KindEditor.create('textarea[name="CodeContent"]', {
                resizeType: 1,
                allowPreviewEmoticons: false,
                allowImageUpload: false,
                items: [
						'source', '|', 'code', '|', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
						'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
						'insertunorderedlist', '|', 'emoticons', 'image', 'link','quote']
            });

            $('#fm').form('clear');
            _url = "../Ajax/CodeLibAjax.ashx?Method=SaveCodeLib&Mode=" + _mode;
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
                                saveCodeLib();
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

				//保存
        function saveCodeLib() {
            $("#hdnContent").val(_editor.html());
            $('#fm').form('submit', {
                url: _url,
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
     <div id="dlg" style="width: 850px; height: 600px; padding: 10px 20px;display:none;">
       <form id="fm" method="post" novalidate>
        <input type="hidden" id="hdnContent" name="Content" />  
        <table>
            <tr>
                <td align="right" style="width: 80px;">
                    标题:
                </td>
                <td align="left">
                    <input id="txtTitle" name="Title" maxlength="100" style="width: 700px;" class="easyui-validatebox" required="true"/>
                </td>
            </tr>
            <tr>
                <td align="right">
                    摘要:
                </td>
                <td align="left" >
                    <textarea  id="txtSummary" name="Summary" style="width:696px;height:50px;resize:none;"
                         class="easyui-validatebox" required="true"></textarea>
                </td>
            </tr>
            <tr>
                <td align="right">
                    标签:
                </td>
                <td align="left">
                    <input id="txtTag" name="Tag" style="width: 700px;"  class="easyui-validatebox" required="true"/>
                    <br />
                    (多个关键字之间用“,”分隔，最多不超过10个)
                </td>
            </tr>
            <tr>
                <td align="right">
                    内容:
                </td>
                <td align="left">
                    <textarea name="CodeContent" style="width:700px;height:350px;"></textarea>
                </td>
            </tr>
        </table>
        </form>
    </div>

     <!--工具栏-->
    <div id="tb" style="padding: 5px; height: auto;display:none;">
        <div style="margin-bottom: 5px">
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" onclick="add()">新增</a>
        </div>
    </div>


    
        <div id="room">
             <div id="CALeft">
                <div class="subheader">
                    <div class="headQuestions">
                        所有代码
                    </div>
                    <!--    <a class="feed-icon" style="background-image:url('{ media "media/images/feed-icon-small.png" %}');" href="/questions/?type=rss" title="{% trans "subscribe to question RSS feed" }"></a>-->
                    <div class="tabsB">
                        <a href="/questions/?sort=active" class="imhere" title="按发布时间排序">最新发布</a><a href="/questions/?sort=newest"
                            title="按受欢迎程度排序">最多投票</a><a href="/questions/?sort=hottest" title="按浏览次数排序">最多浏览</a></div>
                </div>
            
            </div>
        </div>
   

</asp:Content>
