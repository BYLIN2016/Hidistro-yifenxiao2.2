<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageDistributor.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageDistributor" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

	<!--选项卡-->

	<div class="dataarea mainwidth databody">
	  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>分销商列表</h1>
  <span>对店铺的分销商进行管理，您可以调整分销商的代理等级</span>
</div>

		<!--搜索-->
	
		
	
		<!--数据列表区域-->
	  <div class="datalist">
	  	<div class="searcharea clearfix">
			<ul>
				<li><span>分销商名称：</span><span><asp:TextBox ID="txtUserName" CssClass="forminput" runat="server"></asp:TextBox></span></li>
				<li><span>分销商姓名：</span><span><asp:TextBox ID="txtTrueName" CssClass="forminput" runat="server"></asp:TextBox></span></li>
				<li><span>分销商等级：</span><span><Hi:DistributorGradeDropDownList ID="dropGrade" runat="server" NullToDisplay="请选择分销商等级" /></span></li>
				<li><asp:Button ID="btnSearchButton" runat="server" CssClass="searchbutton" Text="查询" /></li>
			</ul>
		</div>
		<div class="blank12 clearfix"></div>
		<div class="searcharea clearfix br_search">
        <ul><li id="clickTopDown" class="clickTopX"><strong class="fonts">导出分销商信息</strong></li></ul>
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
		<!--结束-->


         <div class="functionHandleArea clearfix m_none">
	    <!--分页功能-->
	    <div class="pageHandleArea" style="float:left;">
	      <ul>
	        <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
          </ul>
        </div>
	    <div class="pageNumber"> <div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
        </div></div>
	    <!--结束-->
	    
	    	 <div class="blank8 clearfix"></div>
	  <div class="batchHandleArea">
		    <ul>
		      <li class="batchHandleButton">
              <span class="signicon"></span> <span class="allSelect"><a href="javascript:void(0);" onclick="CheckClickAll()">全选</a></span> 
              <span class="reverseSelect"><a href="javascript:void(0);" onclick="CheckReverse()">反选</a></span> 
               <span id="Span1"  class="sendmsg"  runat="server"><a href="javascript:void(0);" onclick="javascript:return SendMessage();">短信群发</a></span>
              <span id="Span2"  class="sendemail" runat="server"><a href="javascript:void(0);" onclick="return SendEmail()">邮件群发</a></span>
              <span id="Span3"  class="sendsite" runat="server"><a href="javascript:void(0);" onclick="javascript:SendSiteMessage();">站内信群发</a></span>
              </li>
	        </ul>
	      </div>
      </div>
	  <UI:Grid ID="grdDistributorList" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="UserId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
              <UI:CheckBoxColumn HeaderStyle-CssClass="td_right td_left" />
                  <asp:TemplateField HeaderText="分销商名称" SortExpression="UserName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
	                            <%#Eval("UserName")%>
	                            <Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%# Eval("Wangwang") %>' />	                              
                             </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="分销商姓名"  HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>
                            <div></div><%# Eval("RealName")%>
                           </ItemTemplate>
                   </asp:TemplateField>
                      <asp:TemplateField HeaderText="分销商邮箱"  HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>
                            <div></div><%# Eval("Email")%>
                           </ItemTemplate>
                   </asp:TemplateField>
                     <asp:TemplateField HeaderText="分销商手机号"  HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>
                            <div></div><%# Eval("CellPhone")%>
                           </ItemTemplate>
                   </asp:TemplateField>
                        <asp:TemplateField HeaderText="分销商产品数" SortExpression="ProductCount" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>	
                            <table cellpadding="0" cellspacing="0" style="border:none;">
                                <tr><td style="border:none; width:10px;"><span class="Name"><%# Eval("ProductCount")%></span> </td>
                                   <td style="border:none;"><span class="submit_xugai"><asp:HyperLink ID="lkProductSummary" NavigateUrl='<%# "../product/ProductOnSales.aspx?distributorId="+Eval("UserId")%>' runat="server">产品目录</asp:HyperLink></span></td></tr>
                            </table>                                                 
                             </itemtemplate>
                        </asp:TemplateField>
        
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="30%" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                             <span class="submit_chakan"><a id="lkView" href="javascript:void(0);" onclick="showMessage('<%# Eval("UserId")%>')">查看</a> </span>
                            <span class="submit_jiage"><asp:HyperLink ID="lkEdit" runat="server" Text="编辑" NavigateUrl='<%# "EditDistributorSettings.aspx?UserId="+Eval("UserId")%>'></asp:HyperLink> </span>
                            <span class="submit_bianji"><a href="javascript:void(0);" onclick="ShowDistributorAccountSummary('<%# Eval("UserId")%>')">概要</a></span>
                            <span class="submit_shanchu"><Hi:ImageLinkButton runat="server" ID="BtnStopCooperation" CommandName="StopCooperation" DeleteMsg="清除分销商后，分销商下的所有信息将会被删除，但分销商已付款的采购单仍可继续发货，确认要清除吗？" IsShow="true" Text="清除" /></span>
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
            </UI:Grid>
                      
<div class="blank5 clearfix"></div>
	  </div>
	  <div class="bottomPageNumber clearfix">
	  <div class="pageNumber">
				<div class="pagination">
            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
        </div>
			</div>
		</div>
</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
 <!--分销商信息-->
<div id="showMessage" style="display:none">
    <div class="frame-content">
        <table>
            <tr>
                <td class="td_right">分销商名称：</td><td><em><span ID="litName" runat="server"></span></em></td>
                <td class="td_right">姓名：</td><td><span id="SpanRealName" /></td>
            </tr>
             <tr>
                <td class="td_right">公司名称：</td><td><span id="spanCompanyName" /></span></td>
                <td class="td_right">邮编：</td><td><span id="spanPostCode" /></span></td>
            </tr>
            <tr>
                <td class="td_right">QQ：</td><td><span id="spanQQ" /></td>
                <td class="td_right">旺旺：</td><td><span id="span8" /></span></td>
            </tr>
            <tr>
                <td class="td_right">MSN：</td><td><span id="span7" /></td>
                <td class="td_right">电子邮件：</td><td><span id="spanEmail" /></td>
            </tr>
            <tr>
                <td class="td_right">地区：</td><td><span id="spanArea" /></td>
                <td class="td_right"></td><td></td>
            </tr>
            <tr>
                <td class="td_right">详细地址：</td><td colspan="3"><span id="spanAddress" /></span></td>
            </tr>
             <tr>
                <td class="td_right">手机号码：</td><td><span id="spanCellPhone" /></td>
                <td class="td_right">固定电话：</td><td><span id="spanTelephone"></span></td>
            </tr>
            <tr>
                <td class="td_right">注册日期：</td><td><span id="spanRegisterDate" /></td>
                <td class="td_right">最后登录日期：</td><td><span id="spanLastLoginDate"></span></td>
            </tr>
        </table>
    </div>
</div>

 <!--分销商款账户概要-->

<div id="ShowDistributorAccountSummary" style="display:none;">
    <div class="frame-content">
        <p><h1>分销商“<span id="litUser" runat="server"/>”预付款账户概要</h1></p>
        <table style="width:350px">
            <tr><td class="td_right">预付款总额：</td><td><span id="lblAccountAmount" runat="server" /></td><td> <a id="lkbtnBalanceDetail">查看明细</a></td></tr>
            <tr><td class="td_right">可用余额：</td><td><span id="lblUseableBalance" runat="server" /></td><td><a id="lkbtnRecharge" >预付款加款</a></td></tr>
            <tr><td class="td_right">冻结金额：</td><td><span id="lblFreezeBalance" runat="server" /></td><td></td></tr>
            <tr><td class="td_right">提现金额：</td><td><span id="lblDrawRequestBalance" runat="server" /></td><td><a id="lkbtnDrawRequest">提现申请</a></td></tr>
        </table>
    </div>
</div>

   <!--短信群发-->
   <div id="div_sendmsg" style="display: none;">
       <div class="frame-content">
            <p>短信群发<span>(您还剩余短信<font color="red"><asp:Literal ID="litsmscount" runat="server" Text="0"></asp:Literal></font>条)</span></p>
            <p><h4>发送对象(共<font style="color:Red">0</font>个分销商)</h4></p>
            <ul class="menber"></ul>
            <p><textarea id="txtmsgcontent" runat="server" style="width:750px; height:240px;" class="forminput" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea></p>
       </div>
   </div>

 <!--邮件群发-->
 <div id="div_email" style="display: none;">
      <div class="frame-content">
            <h4>发送对象(共<font style="color:Red">0</font>个分销商)</h4>
            <ul class="menber"></ul>
            <p><textarea id="txtemailcontent" runat="server" style="width:700px; height:240px;" class="forminput" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea></p>
      </div>
 </div>

<!--站内群发-->
<div  id="div_site" style="display: none;">
    <div class="frame-content">
        <p><h4>发送对象(共<font style="color:Red">0</font>个分销商)</h4></p>
        <ul class="menber"></ul>
        <p><textarea id="txtsitecontent" runat="server" style="width:700px; height:240px;" class="forminput" value="输入发送内容……" onfocus="javascript:addfocus(this);" onblur="javascript:addblur(this);"></textarea></p>
    </div>
</div>

<div style="display:none">
    <input type="hidden" id="hdenablemsg" runat="server" value="0" />
    <input type="hidden" id="hdenableemail" runat="server" value="0" />
    <asp:Button ID="btnsitecontent" runat="server" Text="站内群发" CssClass="submit_DAqueding"/>
    <asp:Button ID="btnSendEmail" runat="server" Text="邮件群发" CssClass="submit_DAqueding"/>
    <asp:Button ID="btnSendMessage" runat="server" Text="短信群发" CssClass="submit_DAqueding"/>
    <input type="button" name="button" id="Submit1" value="分销商账户概要" onclick="CloseDiv('ShowDistributorAccountSummary')" class="submit_DAqueding"/>
</div>

	  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function showMessage(id) {
        $.ajax({
            url: "ManageDistributor.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                showMessage: "true",
                id: id
            },
            async: false,
            success: function (resultData) {
                if (resultData.Status == "1") {

                    $("#ctl00_contentHolder_litName").html(resultData.UserName);
                    $("#SpanRealName").html(resultData.RealName);
                    $("#spanCompanyName").html(resultData.CompanyName);
                    $("#spanEmail").html(resultData.Email);
                    $("#spanArea").html(resultData.Area);
                    $("#spanAddress").html(resultData.Address);
                    $("#spanQQ").html(resultData.QQ);
                    $("#spanPostCode").html(resultData.PostCode);
                    $("#spanMsn").html(resultData.MSN);
                    $("#spanWangwang").html(resultData.Wangwang);
                    $("#spanCellPhone").html(resultData.CellPhone);
                    $("#spanTelephone").html(resultData.Telephone);
                    $("#spanRegisterDate").html(resultData.RegisterDate);
                    $("#spanLastLoginDate").html(resultData.LastLoginDate);

                    ShowMessageDialog("查看分销商信息", "viewdidstributor", "showMessage");

                }

                else {
                    alert("未知错误，没有此分销商的信息");
                }
            }

        });
    }   
   

    function ShowDistributorAccountSummary(userId) {
        $.ajax({
            url: "ManageDistributor.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                showDistributorAccountSummary: "true",
                id: userId
            },
            async: false,
            success: function(resultData) {
            if (resultData.Status == "1") {
                    $("#ctl00_contentHolder_litUser").html(resultData.UserName);
                    $("#ctl00_contentHolder_lblAccountAmount").html(resultData.AccountAmount);
                    $("#ctl00_contentHolder_lblUseableBalance").html(resultData.UseableBalance);
                    $("#ctl00_contentHolder_lblFreezeBalance").html(resultData.FreezeBalance);
                    $("#ctl00_contentHolder_lblDrawRequestBalance").html(resultData.DrawRequestBalance);
                    document.getElementById("lkbtnRecharge").setAttribute("href", "javascript:DialogFrame('distribution/DistributorReCharge.aspx?userId=" + userId + "','查看预付款加款',720,0)");
                    document.getElementById("lkbtnBalanceDetail").setAttribute("href", "javascript:DialogFrame('distribution/DistributorBalanceDetails.aspx?userId=" + userId + "','查看账户明细',null,null)");
                    document.getElementById("lkbtnDrawRequest").setAttribute("href", "javascript:DialogFrame('distribution/DistributorBalanceDrawRequest.aspx?userId=" + userId + "','查看提现申请',null,null)");

                    ShowMessageDialog("查看分销商概要", "viewaccount", "ShowDistributorAccountSummary");
                    
               }

                else {
                    alert("未知错误，没有此分销商的信息");
                }
            }

        });
    }
    
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
        var realname=$(this).parent("td").parent("tr").find("td:eq(1)").text().replace(/\s/g,"");
        //var cellphone=$(this).parent("td").parent("tr").find("td:eq(2)").text().replace(/\s/g,"").replace(/\n[\s| ]*\r/g,"").replace(/(^\s*)|(\s*$)/g,"");
        var cellphone=$(this).parent("td").parent("tr").find("td:eq(4)").text().replace(/\s/g,"").replace("　","");
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
        var realname=$(this).parent("td").parent("tr").find("td:eq(1)").text().replace(/\s/g,"");
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
}


//站内群发
function SendSiteMessage(){
    var v_str=0;
    var regname=/^[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*/;
    var html_str="";
    $("#div_site .menber").html('');
    $("#div_site h4 font").text('0');
    $(".datalist input[type='checkbox']:checked").each(function(rowIndex, rowItem){
        var username=$(this).parent("td").parent("tr").find("td:eq(1)").text().replace(/\s/g,"");//会员名
        var realname=$(this).parent("td").parent("tr").find("td:eq(2)").text().replace(/\s/g,"");//真实姓名
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
