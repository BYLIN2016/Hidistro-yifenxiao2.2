<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageSites.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ManageSites"  %>
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
  <h1>连锁加盟站点列表</h1>
  <span>查看各个分销商的店铺域名</span>
</div>		
		<!--数据列表区域-->
	  <div class="datalist">
	  	<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>分销商名称：</span><span><asp:TextBox ID="txtDistributorName" runat="server" CssClass="forminput"></asp:TextBox></span></li>
				<li><span>分销商姓名：</span><span><asp:TextBox ID="txtTrueName" CssClass="forminput" runat="server"></asp:TextBox></span></li>
				<li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" /></li>
			</ul>
		</div>
		<!--结束-->


         <div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea" style="float:left;">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize ID="hrefPageSize" runat="server" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                         <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                </div>

			</div>
			<!--结束-->
		</div>
	  <UI:Grid ID="grdDistributorSites" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="UserId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="分销商名称" ItemStyle-Width="15%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litName" Text='<%#Eval("Username") %>' runat="server"></asp:Literal>
                           <Hi:WangWangConversations ID="wangwang"  runat="server" WangWangAccounts='<%# Eval("Wangwang") %>' />
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="分销商姓名" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           &nbsp;<asp:Literal ID="litRealName" Text='<%#Eval("RealName") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="域名" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDomain" Text='<%#Eval("SiteUrl") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="申请日期" ItemStyle-Width="15%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDate" Text='<%#Eval("RequestDate") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="状态" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litState" Text='<%#Eval("Disabled") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="30%" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                             <span class="submit_chakan"><a href="javascript:void(0);" onclick="ShowSiteMessage('<%#Eval("UserId") %>');">查看</a> </span>
                             <span class="submit_shanchu"><Hi:ImageLinkButton ID="btnIsOpen" runat="server"  IsShow="true"   CommandName="open"  /> </span>
                             <span class="submit_tongyi"><a href="javascript:DialogFrame('<%# "distribution/ManageTemples.aspx?UserId="+Eval("UserId")  %>','模板管理',null,null)">模板管理</a> </span>
                             <span class="submit_tongyi"><a href="javascript:void(0);" onclick="ShowManageUrl('<%#Eval("UserId") %>','<%#Eval("SiteUrl") %>')">域名管理</a> </span>
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
<!--域名管理-->

<div id="DivManageUrl" style="display:none;">
  <div class="frame-content">
    <p>在域名通过备案后，需要检查此域名解析的IP是否指向到：<strong><asp:Literal runat="server" ID="litServerIp"></asp:Literal></strong></p>
    <p><span class="frame-span frame-input90">域名</span><asp:TextBox runat="server" ID="txtFirstSiteUrl" class="forminput" /></p>
   </div>
</div>

<!--查看分销站点详情-->
<div id="DivShowSiteMessage" style="display:none;">
  <div class="frame-content">
	<table width="100%" border="0" cellspacing="0">
  <tr>
    <td class="td_right">分销商名称：<br /></td>
    <td><span class="colorE"><strong class="colorA"><span id="spanDistributorName" runat="server" /></strong></span></td>
    <td class="td_right">姓名：</td>
    <td><span id="spanRealName" /></td>
  </tr>
  <tr>
    <td class="td_right">公司名：</td>
    <td><span id="spanCompanyName" /></td>
    <td class="td_right">地区：</td>
    <td><span id="spanArea" /></td>
  </tr>
  <tr>
    <td class="td_right">QQ：</td>
    <td><span id="spanQQ" /></td>
    <td class="td_right">邮编：</td>
    <td><span id="spanPostCode" /></td>
  </tr>
  <tr>
    <td class="td_right">MSN：</td>
    <td><span id="spanMSN" /></td>
    <td class="td_right">旺旺：</td>
    <td><span id="spanWangwang" /></td>
  </tr>
  <tr>
    <td class="td_right">电子邮件：</td>
    <td><span id="spanEmail" /></td>
    <td class="td_right">手机号码：</td>
    <td><span id="spanCellPhone" /></td>
  </tr>
    <tr>
    <td class="td_right">详细地址：</td>
    <td colspan="3"><span id="spanAddress" /></td>
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
    <td class="td_right">独立域名</td>
    <td><span id="spanDomain1" /></td>
  </tr>
    </table>

  </div>
</div>


 <div style="display:none">
 <input id="hidUserId" type="hidden" runat="server" />
 <asp:Button ID="btnSave" runat="server" Text="域名管理"  CssClass="submit_DAqueding"/>
 </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

    <script language="javascript" type="text/javascript">
        function validatorForm() {
            var siteurl1 = $("#ctl00_contentHolder_txtFirstSiteUrl").val().replace(/\s/g, "");
            if (siteurl1.lenght <= 0) {
                alert('域名不允许为空！');
                return false;
            }
            return true;
        }

    function ShowManageUrl(userId, url1) {
        $("#ctl00_contentHolder_hidUserId").val(userId);
        $("#ctl00_contentHolder_txtFirstSiteUrl").val(url1);

        arrytext = null;
        setArryText("ctl00_contentHolder_hidUserId", userId);
        setArryText("ctl00_contentHolder_txtFirstSiteUrl", url1);

        DialogShow("域名管理", 'sitemanager', 'DivManageUrl', 'ctl00_contentHolder_btnSave');
    }

    function ShowSiteMessage(userId) { 
     $.ajax({
            url: "ManageSites.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                showMessage: "true",
                userId: userId
            },
            async: false,
            success: function(resultData) {
                if (resultData.Status == "1")
                 {
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

                    ShowMessageDialog("查看分销站点详情", 'viewsite', 'DivShowSiteMessage');
          
                }

                else { alert("未知错误，没有此分销商的信息");}
            }

        });
    }
</script>
</asp:Content>
