<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageMembers.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageMembers" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>会员列表</h1>
  <span>对店铺的会员进行管理，您可以修改会员的资料或调整会员的等级</span>
</div>
		<!--搜索-->
	
		<!--数据列表区域-->
		<div class="datalist">
			<div class="searcharea clearfix br_search">
			<ul>
				<li>
                <span>会员名：</span>
                <span><asp:TextBox ID="txtSearchText" CssClass="forminput" runat="server" /></span>
          </li>
          	<li>
                <span>会员真实姓名：</span>
                <span><asp:TextBox ID="txtRealName" CssClass="forminput" runat="server" /></span>
          </li>
				<li>
          <span>会员等级：</span>
					<abbr class="formselect">
						<Hi:MemberGradeDropDownList ID="rankList" runat="server" AllowNull="true" NullToDisplay="全部" />
				</abbr>
				</li>
                
				<li>
				    <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="搜索" />
				</li>
			</ul>
          <ul>
          <li id="clickTopDown" class="clickTopX"><strong class="fonts">导出会员信息</strong></li>
	      </ul>
          <dl id="dataArea" style="display:none;">
		  <ul>
		    <li>请选择需要导出的信息：</li>
            <li>
            <Hi:ExportFieldsCheckBoxList ID="exportFieldsCheckBoxList" runat="server"></Hi:ExportFieldsCheckBoxList>
           </li>
	      </ul>
          <ul>
		    <li style="padding-left:47px;">请选择导出格式：</li>
            <li>
           <Hi:ExportFormatRadioButtonList ID="exportFormatRadioButtonList" runat="server" />
            </li>
	      </ul>
           <ul>
		    <li style=" width:135px;"></li>
             <li><asp:Button ID="btnExport" runat="server" CssClass="submit_queding" Text="导出" /></li>
	      </ul>
         </dl>
	  </div>
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea" style="float:left;">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	          
	      </div>
		 <div class="pageNumber" style="float:right;"> 
		     <div class="pagination"><UI:Pager runat="server" ShowTotalPages="false" ID="pager" /></div>
        </div>
		  <!--结束-->
		  <div class="blank8 clearfix"></div>
		  <div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton">
              <span class="signicon"></span> <span class="allSelect"><a href="javascript:void(0);" onclick="CheckClickAll()">全选</a></span> 
              <span class="reverseSelect"><a href="javascript:void(0);" onclick="CheckReverse()">反选</a></span> 
              <span class="delete"><Hi:ImageLinkButton ID="lkbDelectCheck1" IsShow="true" Height="25px" runat="server" Text="删除" /></span>
               <span id="Span1" class="sendmsg"  runat="server"><a href="javascript:void(0);" onclick="return SendMessage()">短信群发</a></span>
              <span id="Span2" class="sendemail" runat="server"><a href="javascript:void(0);" onclick="return SendEmail()">邮件群发</a></span>
              <span id="Span3" class="sendsite" runat="server"><a href="javascript:void(0);" onclick="return SendSiteMessage()">站内信群发</a></span>
              </li>
	        </ul>
	      </div>
		  <div class="filterClass"> <span><b>会员审核状态：</b></span> <span class="formselect">
		    <Hi:ApprovedDropDownList runat="server" ID="ddlApproved"  />
		    </span> </div>
	  </div>
		    <UI:Grid ID="grdMemberList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="UserId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        <UI:CheckBoxColumn HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField HeaderText="用户名" SortExpression="UserName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
	                              <span class="Name"><asp:Literal ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>' /></span>
	                              <span style="display:none"><%# Eval("RealName") %></span>
                             </itemtemplate>
                        </asp:TemplateField>
                      <asp:TemplateField HeaderText="手机号"  HeaderStyle-CssClass="td_right td_left" ShowHeader="true">
                        <ItemTemplate><div></div><%# Eval("CellPhone")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="邮箱"  HeaderStyle-CssClass="td_right td_left" ShowHeader="true">
                        <ItemTemplate><div></div><%# Eval("Email")%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="会员等级" SortExpression="GradeName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
	                              <asp:Literal ID="lblGradeName" runat="server" Text='<%# Eval("GradeName") %>' />
                             </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="预付款余额"  ItemStyle-Width="100px" SortExpression="Balance" HeaderStyle-CssClass="td_right td_left" >                            
                            <itemtemplate>
                                <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%# Eval("Balance") %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="订单数"   SortExpression="OrderNumber" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
                                <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/sales/ManageOrder.aspx?UserName={0}",Server.UrlEncode(Eval("UserName").ToString()))) %>'
                                style="text-decoration:underline;"><asp:Label id="lblOrderNumberBandField" text='<%# Eval("OrderNumber") %>' runat="server"></asp:Label></a>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText=" 积分点数" DataField="Points" SortExpression="Points" HeaderStyle-CssClass="td_right td_left" />
                            <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom" ItemStyle-Width="200px" >
                                <ItemStyle CssClass="spanD spanN" />
                                <ItemTemplate>
                                    <span class="submit_chakan"><a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/MemberDetails.aspx?userId={0}", Eval("UserId")))%>' >查看</a> </span>
		                           <span class="submit_jiage"><a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/EditMember.aspx?userId={0}", Eval("UserId")))%>'>编辑</a></span> 
		                           <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="Delete" IsShow="true" Text="删除" CommandName="Delete" /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                    </Columns>
                </UI:Grid>               
<div class="blank12 clearfix"></div>
</div>
		<!--数据列表底部功能区域-->
  <div class="bottomBatchHandleArea clearfix">
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
						<span class="bottomSignicon"></span>
						<span class="allSelect"><a href="javascript:void(0);" onclick="CheckClickAll()">全选</a></span>
						<span class="reverseSelect"><a href="javascript:void(0);" onclick="CheckReverse()">反选</a></span>
					    <span class="delete"><Hi:ImageLinkButton ID="lkbDelectCheck" IsShow="true" Height="25px" runat="server" Text="删除" /></span>
					   <span class="sendmsg" runat="server"><a href="javascript:void(0);"  onclick="return SendMessage()">短信群发</a></span>
                        <span class="sendemail" runat="server" ><a href="javascript:void(0);" onclick="return SendEmail()">邮件群发</a></span>
                        <span class="sendsite" runat="server"><a href="javascript:void(0);" onclick="return SendSiteMessage()">站内信群发</a></span>
					</li>
				</ul>
			</div>
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination" style="width:auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
		</div>
	</div>
	
 <!--会员短信群发-->
   <div  id="div_sendmsg" style="display: none;">
         <p>短信群发<span>(您还剩余短信<font color="red"><asp:Literal ID="litsmscount" runat="server" Text="0"></asp:Literal></font>条)</span></p>
          <p> <h4>发送对象(共<font style="color:Red">0</font>个会员)</h4></p>
          <div id="send_member" style="overflow-x:hidden;overflow-y:auto;margin-bottom:20px"><ul class="menber"></ul></div>
          <p><textarea id="txtmsgcontent" runat="server" style="width:750px; height:240px;" class="forminput" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea></p>
   </div>

<!--邮件群发-->
<div id="div_email" style="display: none;">
      <div class="frame-content">
       <p><h4>发送对象(共<font style="color:Red">0</font>个会员)</h4></p>
       <div id="send_email" style="overflow-x:hidden;overflow-y:auto;margin-bottom:20px">
             <ul class="menber"></ul>
       </div>
       <p>
            <textarea id="txtemailcontent" runat="server" style="width:700px; height:240px;" class="forminput" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea>
       </p>
      </div>
</div>

<!--站内群发-->
<div id="div_site" style="display: none;">
    <div class="frame-content">
    <p><h4>发送对象(共<font style="color:Red">0</font>个会员)</h4></p>
    <div id="send_site" style="overflow-x:hidden;overflow-y:auto;margin-bottom:20px">
        <ul class="menber"></ul>
    </div>
    <p><textarea id="txtsitecontent" runat="server" style="height:240px;width:700px" class="forminput" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea></p>
    </div>
</div>

<div style="display:none"> 
<input type="hidden" id="hdenablemsg" runat="server" value="0" />
<input type="hidden" id="hdenableemail" runat="server" value="0" />
<asp:Button ID="btnsitecontent" runat="server" Text="发送站内信" CssClass="submit_DAqueding" />
<asp:Button ID="btnSendEmail" runat="server" Text="邮件群发" CssClass="submit_DAqueding" />
<asp:Button ID="btnSendMessage" runat="server" Text="短信群发" CssClass="submit_DAqueding" />
</div>

	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">	
<script type="text/javascript" language="javascript">
 var formtype = "";
//jquery控制上下显示
$(document).ready(function(){ 
  var status=1;
$("#clickTopDown").click(function(){
   $("#dataArea").toggle(500, changeClass)
 })

  changeClass=function()
  {
	if(status==1)
	{
	  $("#clickTopDown").removeClass("clickTopX"); 
	  $("#clickTopDown").addClass("clickTopS");
	  status=0;	
	}
	else
	{
	  $("#clickTopDown").removeClass("clickTopS"); 
	  $("#clickTopDown").addClass("clickTopX"); 
	  status=1;		
	}	
  }
});
    


//样式控制
function showcss(divobj,rownumber){
    if (rownumber > 12) {
        $("#" + divobj).css("height",100);
    }
}

    //短信群发
function SendMessage(){
    if($("#ctl00_contentHolder_hdenablemsg").val().replace(/\s/g,"")=="0")
    {
        alert("您还未进行短信配置，无法发送短信");
        return false;
    }
    var v_str=0;
    var regphone=/^1[3458]\d{9}$/;
    var html_str="";
    $("#div_sendmsg .menber").html('');
    $("#div_sendmsg h4 font").text('0');
    $(".datalist input[type='checkbox']:checked").each(function(rowIndex, rowItem){
        var realname=$(this).parent("td").parent("tr").find("td:eq(1) span:eq(0)").text().replace(/\s/g,"");
        //var cellphone=$(this).parent("td").parent("tr").find("td:eq(2)").text().replace(/\s/g,"").replace(/\n[\s| ]*\r/g,"").replace(/(^\s*)|(\s*$)/g,"");
        var cellphone=$(this).parent("td").parent("tr").find("td:eq(2)").text().replace(/\s/g,"").replace("　","");
        if(cellphone!=""&&cellphone!="undefined"&&regphone.test(cellphone)==true){
            html_str=html_str+"<li>"+realname+"("+cellphone+")</li>";
            v_str++;
        }else{
            $(this).attr("checked",false);
        }
    });
    if(html_str==""){
        alert("请先选择要发送的对象或检查手机号格式是否正确！");
        return false;
    }
    $("#div_sendmsg .menber").html(html_str);
    $("#div_sendmsg h4 font").text(v_str);
    arrytext = null;
    formtype = "sendmsg";
    DialogShow("会员短信群发", 'sendmsg', 'div_sendmsg', 'ctl00_contentHolder_btnSendMessage');
    art.dialog.list['sendmsg'].size('50%', '50%');
    showcss("send_member", v_str);
}

//邮件群发
 function SendEmail(){
    if($("#ctl00_contentHolder_hdenableemail").val().replace(/\s/g,"")=="0")
    {
        alert("您还未进行邮件配置，无法发送邮件");
        return false;
    }
    var v_str=0;
    var regemail=/^([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,4}){1,2})/;
    var html_str="";
    $("#div_email .menber").html('');
    $("#div_email h4 font").text('0');
    $(".datalist input[type='checkbox']:checked").each(function(rowIndex, rowItem){
        var realname=$(this).parent("td").parent("tr").find("td:eq(1) span:eq(0)").text().replace(/\s/g,"");
        var email=$(this).parent("td").parent("tr").find("td:eq(3)").text().replace(/\s/g,"").replace("　","");
        if(email!=""&&email!="undefined"&&regemail.test(email)){
            html_str=html_str+"<li>"+realname+"("+email+")</li>";
            v_str++;
        }else{
            $(this).attr("checked",false);
        }
    });
    if(html_str==""){
        alert("请先选择要发送的对象或检查邮箱格式是否正确！");
        return false;
    }
    $("#div_email .menber").html(html_str);
    $("#div_email h4 font").text(v_str);
    arrytext = null;
    formtype = "sendemail";
    DialogShow("站内邮件群发", 'sendemail', 'div_email', 'ctl00_contentHolder_btnSendEmail');
    showcss("send_email",v_str);
}


//站内群发
function SendSiteMessage(){
    var v_str=0;
    var regname=/^[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*/;
    var html_str="";
    $("#div_site .menber").html('');
    $("#div_site h4 font").text('0');
    $(".datalist input[type='checkbox']:checked").each(function(rowIndex, rowItem){
        var username=$(this).parent("td").parent("tr").find("td:eq(1) span:eq(0)").text().replace(/\s/g,"");//会员名
        var realname=$(this).parent("td").parent("tr").find("td:eq(1) span:eq(1)").text().replace(/\s/g,"");//真实姓名
        if(username!=""&&username!="undefined"){
            if(realname!=""){
                username+="("+realname+")";
            }
            html_str=html_str+"<li>"+username+"</li>";
            v_str++;
        }else{
            $(this).attr("checked",false);
        }
    });
    if(html_str==""){
        alert("请先选择要发送的对象！");
        return false;
    }
    $("#div_site .menber").html(html_str);
    $("#div_site h4 font").text(v_str);
    arrytext = null;
    formtype = "sendzhannei";
    DialogShow("站内信群发", 'sendzhannei', 'div_site', 'ctl00_contentHolder_btnsitecontent');
    showcss("send_site", v_str);
}

function addfocus(obj){
    if(obj.value.replace(/\s/g,"")=="输入发送内容……"){
        obj.value="";
    }
}

function addblur(obj){
    if(obj.value.replace(/\s/g,"")==""){
        obj.value="输入发送内容……";
    }
}

//检验群发信息条件
function CheckSendMessage(){   
    var sendcount=$("#div_sendmsg h4 font").text();//获取发送对象数量
    var smscount=$("#div_sendmsg h1 font").text();//获取剩余短信条数
    if(parseInt(sendcount)>parseInt(smscount)){
        alert("您剩余短信条数不足，请先充值");
        return false;
    }
    if($("#ctl00_contentHolder_txtmsgcontent").val().replace(/\s/g,"")==""||$("#ctl00_contentHolder_txtmsgcontent").val().replace(/\s/g,"")=="输入发送内容……"){
        alert("请先输入要发送的信息内容！");
        return false;
    }
    setArryText("ctl00_contentHolder_txtmsgcontent", $("#ctl00_contentHolder_txtmsgcontent").val().replace(/\s/g, ""));
    return true;

}

//验证群发邮件条件
function CheckSendEmail(){
    if($("#ctl00_contentHolder_txtemailcontent").val().replace(/\s/g,"")==""||$("#ctl00_contentHolder_txtemailcontent").val().replace(/\s/g,"")=="输入发送内容……"){
        alert("请先输入要发送的信息内容！");
        return false;
    }
    setArryText("ctl00_contentHolder_txtemailcontent", $("#ctl00_contentHolder_txtemailcontent").val().replace(/\s/g, ""));
    return true;
}

//验证站内群发
function CheckSendSite(){
    if($("#ctl00_contentHolder_txtsitecontent").val().replace(/\s/g,"")==""||$("#ctl00_contentHolder_txtsitecontent").val().replace(/\s/g,"")=="输入发送内容……"){
        alert("请先输入要发送的信息内容！");
        return false;
    }
    setArryText("ctl00_contentHolder_txtsitecontent", $("#ctl00_contentHolder_txtsitecontent").val().replace(/\s/g, ""));
    return true;
}

//验证
function validatorForm() {
    switch (formtype) {
        case "sendzhannei":
            return CheckSendSite();
            break;
        case "sendemail":
            return CheckSendEmail();
            break;
        case "sendmsg":
            return CheckSendMessage();
            break;
    };
    return true;
}
</script>
</asp:Content>  
