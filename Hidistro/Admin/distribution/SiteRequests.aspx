<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SiteRequests.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SiteRequests" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

	<!--选项卡-->

	<div class="dataarea mainwidth databody">
	<div class="title">
  <em><img src="../images/02.gif" width="32" height="32" /></em>
  <h1><table><tr><td>连锁加盟店开通申请</td><td><a class="help" href="http://video.92hidc.com/video/V2.1/ktfxz.html" title="查看帮助" target="_blank"></a></td></tr></table></h1>
  <span>所有分销店铺域名的申请列表</span>
</div>	
		<!--数据列表区域-->
	  <div class="datalist">
	  		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>分销商名称：</span><span><asp:TextBox ID="txtDistributorName" runat="server" CssClass="forminput"></asp:TextBox></span></li>
				<li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton"></asp:Button></li>
			</ul>
		</div>
		<!--结束-->

        <input id="hidRequestId" runat="server" type="hidden" />
         <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                </div>
			</div>
			<!--结束-->
		</div>
	  <UI:Grid ID="grdDistributorDomainRequests" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="RequestId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="分销商名称" ItemStyle-Width="13%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litName" Text='<%#Eval("Username") %>' runat="server"></asp:Literal>
                           <Hi:WangWangConversations ID="wangwang"  runat="server" WangWangAccounts='<%# Eval("Wangwang") %>' />
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="独立域名" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDomain" Text='<%#Eval("FirstSiteUrl")%>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="电子邮件" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litEmail" Text='<%#Eval("Email") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="申请日期" ItemStyle-Width="17%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDate" Text='<%#Eval("RequestTime") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="30%" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                             <span class="submit_chakan"><a href="javascript:void(0);" onclick="showMessage('<%# Eval("RequestId") %>')">查看</a> </span>
                             <span class="submit_quanxuan"><a href="javascript:void(0);" onclick="acceptSiteRequest('<%#Eval("RequestId") %>','<%#Eval("FirstSiteUrl") %>','<%#Eval("Username") %>')">接受</a> </span>
                            <span class="submit_shanchu"> <a href="javascript:void(0);" onclick="refuseSiteRequest('<%#Eval("RequestId") %>','<%#Eval("Username") %>')">拒绝</a></span>
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
            </UI:Grid>
      
      <div class="blank5 clearfix"></div>
	  </div>
	  <!--数据列表底部功能区域-->
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


<!--接受分销站点开通申请-->

<div id="DivAcceptSiteRequest" style="display:none; ">
    <div class="frame-content">
        <p><em>先核实分销商提交绑定的域名已解析到本站所在服务器IP,且域名必须是可访<br />问的，再联系空间商将分销商提交绑定的独立域名绑定到本站点IIS。</em></p>
        <p><span>分销商名称：</span><strong><span id="spanUserName" runat="server" /></strong></p>
        <p><span>分销商域名：</span><strong><samp class="colorE fonts"><span id="domainName1" runat="server"/></samp></strong></p>
    </div>
</div>



<!--查看申请信息-->
<div id="showMessage" style="display:none;">
  <div class="frame-content">
	<table>
  <tr>
    <td class="td_right">分销商名称：<br /></td>
    <td><span class="colorE"><strong class="colorA"><span id="spanDistributorName" runat="server" /></strong></span></td>
    <td class="td_right">公司名：</td>
    <td><span id="spanCompanyName" /><br /></td>
  </tr>
  <tr>
    <td class="td_right">姓名：</td>
    <td><span id="spanRealName" /></td>
    <td class="td_right">地区：<br /></td>
    <td><span id="spanArea" /></td>
  </tr>
  <tr>
    <td class="td_right">电子邮件：</td>
    <td><span id="spanEmail" /></td>
    <td class="td_right">邮编：</td>
    <td><span id="spanPostCode" /></td>
  </tr>
  <tr>
    <td class="td_right">详细地址：</td>
    <td colspan="3"><span id="spanAddress" /></td>
    </tr>
  <tr>
    <td class="td_right">QQ：</td>
    <td><span id="spanQQ" /></td>
    <td class="td_right">旺旺：</td>
    <td><span id="spanWangwang" /></td>
  </tr>
  <tr>
    <td class="td_right">MSN：</td>
    <td><span id="spanMSN" /></td>
    <td class="td_right">手机号码：</td>
    <td><span id="spanCellPhone" /></td>
  </tr>
  <tr>
    <td class="td_right">注册日期：</td>
    <td><span id="spanRegisterDate" /></td>
    <td class="td_right">固定电话：</td>
    <td><span id="spanTelephone" /></td>
  </tr>
  <tr>
    <td class="td_right">最后登录日期：</td>
    <td><span id="spanLastLoginDate" /> </td>
    <td class="td_right">独立域名：</td>
    <td><span id="spanDomain1" /></td>
  </tr>
    </table>

  </div>
</div>

<!--拒绝分销站点开通申请-->
<div id="DivRefuseSiteRequest" style="display:none;">
  <div class="frame-content">
      <p><em>拒绝域名绑定的申请后,分销站点将无法开通.</em></p>
      <p><span class="frame-span frame-input90">分销商名称:</span><strong><span id="spanUserNameForRefuse" runat="server" /></strong></p>
      <p><span class="frame-span frame-input90">拒绝原因<em>*</em></span><asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="6"  Columns="40"></asp:TextBox></p>
       <b>拒绝原因不能为空，长度限制在300个字符以内</b>
  </div>
</div>

<div style="display:none">
    <asp:Button ID="btnSave" runat="server"  Text="接受站点申请"  CssClass="submit_DAqueding" />
    <asp:Button ID="btnRefuse" runat="server" Text="拒绝站点申请"  CssClass="submit_DAqueding"></asp:Button>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    var formtype = "";

    function validatorForm() {
        switch (formtype) {
            case "accept":
                if ($("#ctl00_contentHolder_hidRequestId").val().replace(/\s/g, "") == "") {
                    alert("接受站点的分销商编号不存在");
                    return false;
                }
                break;
            case "refuse":
                var distriutornumber = $("#ctl00_contentHolder_hidRequestId").val().replace(/\s/g, "");

                if (distriutornumber.lenght <= 0) {
                    alert('申请站点不存在！');
                    return false;
                }
                var refusestr = $("#ctl00_contentHolder_txtReason").val().replace(/\s/g, "");
                if (refusestr.length <= 0 || refusestr.length > 300) {
                    alert("拒绝原因不能为空，长度限制在300个字符以内");
                    return false;
                }
                setArryText("ctl00_contentHolder_hidRequestId", distriutornumber);
                setArryText("ctl00_contentHolder_txtReason", refusestr);
                break;
        };
     
        return true;
    }


    function InitValidators() {

        initValid(new InputValidator('ctl00_contentHolder_txtReason', 1, 300, false, null, '拒绝原因不能为空，长度限制在300个字符以内'));
    }
    $(document).ready(function() { InitValidators(); });
   

    //拒绝站点审核
    function refuseSiteRequest(requestId, userName) {    
        $("#ctl00_contentHolder_hidRequestId").val(requestId);
        $("#ctl00_contentHolder_spanUserNameForRefuse").html(userName);

        formtype = "refuse";
        arrytext = null;
        DialogShow("拒绝分销站点开通申请", 'refusesite', 'DivRefuseSiteRequest', 'ctl00_contentHolder_btnRefuse');
    }

    //接受站点审核
    function acceptSiteRequest(requestId, domainName1, userName) {
        $("#ctl00_contentHolder_hidRequestId").val(requestId);
        $("#ctl00_contentHolder_spanUserName").html(userName);
        $("#ctl00_contentHolder_domainName1").html(domainName1);

        formtype = "accept";
        setArryText("ctl00_contentHolder_hidRequestId", requestId);
        DialogShow("接受分销站点开通申请", 'acceptesite', 'DivAcceptSiteRequest', 'ctl00_contentHolder_btnSave');
    }

    function showMessage(requestId) {
        $.ajax({
            url: "SiteRequests.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                showMessage: "true",
                requestId: requestId
            },
            async: false,
            success: function (resultData) {
                if (resultData.Status == "1") {

                    $("#ctl00_contentHolder_spanDistributorName").html(resultData.UserName);
                    $("#spanRealName").html(resultData.RealName);
                    $("#spanCompanyName").html(resultData.CompanyName);
                    $("#spanEmail").html(resultData.Email);
                    $("#spanArea").html(resultData.Area);
                    $("#spanAddress").html(resultData.Address);
                    $("#spanQQ").html(resultData.QQ);
                    $("#spanPostCode").html(resultData.PostCode);
                    $("#spanMSN").html(resultData.MSN);
                    $("#spanWangwang").html(resultData.Wangwang);
                    $("#spanCellPhone").html(resultData.CellPhone);
                    $("#spanTelephone").html(resultData.Telephone);
                    $("#spanRegisterDate").html(resultData.RegisterDate);
                    $("#spanLastLoginDate").html(resultData.LastLoginDate);
                    $("#spanDomain1").html(resultData.Domain1);

                    ShowMessageDialog("查看分销站点开通申请详情", "distributorsiterequest", "showMessage");
                }

                else {
                    alert("未知错误，没有此分销商的信息");
                }
            }

        });
    }
</script>
</asp:Content>
