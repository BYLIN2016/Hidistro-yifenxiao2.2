<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectImage.aspx.cs" Inherits="Hidistro.UI.Web.DialogTemplates.SelectImage" %>
<%@ Import Namespace="Hidistro.Core" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="swfupload/swfupload.js"></script>
	<script type="text/javascript" src="js/handlers.js"></script>
    <script type="text/javascript" src="../Utility/jquery-1.6.4.min.js"></script>
    <script type="text/javascript" src="../Utility/jquery.artDialog.js"></script>
    <script type="text/javascript" src="js/Hidistro_Dialog.js"></script>
    <link href="../Utility/skins/blue.css" rel="stylesheet" type="text/css" />
    <link href="css/design.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="lunbo">
        <div class="lunbo_body">
          <div class="lunbo_info">请选择要上传的路径：
          <select id="slsbannerposition" name="slsbannerposition" runat="server" onchange="ChangeValue(this.value)">
          <option value="advertimg" selected="selected">广告位图片</option>
          <option value="titleimg">标题导航图片</option>
          </select>
         <span>(请使用<b id="imagesize" runat="server">30*100</b>的图片，大小不超过500k)</span><span>
         <span id="swfu_container">
		        <span>
				    <span id="spanButtonPlaceholder"></span>
		        </span>
		        <span id="divFileProgressContainer"></span>
	        </span></span></div>
          <div class="lunbo_list" id="thumbnails" style="overflow:hidden">
            <ul><asp:Repeater ID="rp_img" runat="server">
            <ItemTemplate>
                 <li><a title="点击添加图片"><img src="<%# ShowImage(Eval("Name").ToString()) %>" width="60px" height="60px" border="0" alt="点击添加图片" ><i></i></a></li>
            </ItemTemplate>
            </asp:Repeater></ul>
          </div>
          <div class="select_pager"><span>共<span id="sptotal"><%=sum %></span> 张图片，当前第 <span id="sp_current"><%=pageindex %></span>/<%=pagetotal %>页</span><span><a id="sp_pre" onclick="javascript:SearchImage.Prevpage(<%=pageindex %>,<%=pagetotal %>);">上一页</a></span><span><a  id="sp_next" onclick="javascript:SearchImage.Nextpage(<%=pageindex %>,<%=pagetotal %>);">下一页</a></span></div>
        </div>
    </div>
    </form>
	<script type="text/javascript">                                                         
	var queueErrorArray;
	var swfu;
	window.onload = function () {
	    swfu = new SWFUpload({
	        // Backend Settings
	        upload_url: "SelectImage.aspx",
	        use_query_string: true,
	        post_params: {
	            iscallback: "true",
	            size: $("#imagesize").text(),
	            type: $("#slsbannerposition").val(),
	            pageindex: 1
	        },

	        // File Upload Settings
	        file_size_limit: "501",
	        file_types: "*.jpg;*.gif;*.png;*.jpeg",
	        file_types_description: "JPG Images",
	        file_upload_limit: "0",    // Zero means unlimited

	        // Event Handler Settings - these functions as defined in Handlers.js
	        // The handlers are not part of SWFUpload but are part of my website and control how
	        // my website reacts to the SWFUpload events.
	        file_queue_error_handler: fileQueueError,
	        file_dialog_complete_handler: fileDialogComplete,
	        upload_progress_handler: uploadProgress,
	        upload_error_handler: uploadError,
	        upload_success_handler: uploadSuccess,
	        upload_complete_handler: uploadComplete,

	        // Button settings
	        button_image_url: "/DialogTemplates/images/swfupload_uploadBtn.png",
	        button_placeholder_id: "spanButtonPlaceholder",
	        button_width: 63,
	        button_height: 22,

	        default_preview: "/DialogTemplates/images/07.png",

	        // Flash Settings
	        flash_url: "/DialogTemplates/swfupload/swfupload.swf", // Relative to this file
	        custom_settings: {
	            upload_target: "divFileProgressContainer"
	        }
	    });

        if(<%=pageindex %><=1){
            $("#sp_pre").addClass("disable");
        }
        if(<%=pageindex %>>=<%=pagetotal %>){
            $("#sp_next").addClass("disable");
        }

        SearchImage.LoadIinit();

	}
	</script>
</body>
</html>
