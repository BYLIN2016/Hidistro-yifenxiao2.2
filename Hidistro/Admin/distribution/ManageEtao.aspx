<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ManageEtao.aspx.cs" Inherits="Hidistro.UI.Web.Admin.distribution.ManageEtao" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">

<div class="optiongroup mainwidth">
		<ul>
            <li class="menucurrent"><a href="ManageEtao.aspx"  ><span >通一淘站点管理</span></a></li>
            <li class="optionend"><a href="EtaoRequests.aspx"><span>通一淘站点审核</span></a></li>
		</ul>
	</div>
	<div class="dataarea mainwidth">

			<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>分销商名称：</span><span><asp:TextBox ID="txtDistributorName" runat="server" CssClass="forminput"></asp:TextBox></span></li>
				<li><span>分销商姓名：</span><span><asp:TextBox ID="txtTrueName" CssClass="forminput" runat="server"></asp:TextBox></span></li>
				<li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton" 
                        onclick="btnSearch_Click" /></li>
			</ul>
		</div>
		<!--结束-->

      
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
		
		<!--数据列表区域-->
	  <div class="datalist">
	  
	<UI:Grid ID="grdDistributorSites" runat="server" AutoGenerateColumns="false" 
              ShowHeader="true" DataKeyNames="UserId" GridLines="None" Width="100%" 
              HeaderStyle-CssClass="table_title" 
              onrowcommand="grdDistributorSites_RowCommand" 
              onrowdatabound="grdDistributorSites_RowDataBound">
              <Columns>
                  <asp:TemplateField HeaderText="分销商名称" ItemStyle-Width="13%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litName" Text='<%#Eval("Username") %>' runat="server"></asp:Literal>
                           <Hi:WangWangConversations ID="wangwang"  runat="server" WangWangAccounts='<%# Eval("Wangwang") %>' />
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="分销商姓名" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           &nbsp;<asp:Literal ID="litRealName" Text='<%#Eval("RealName") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="域名" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDomain" Text='<%#Eval("SiteUrl") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  
                  <asp:TemplateField HeaderText="申请日期" ItemStyle-Width="17%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDate" Text='<%#Eval("EtaoApplyTime") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="状态" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litState" Text='<%#Eval("isopenetao") %>'  runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="40%" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                             <span class="submit_chakan"><a href="javascript:void(0);" onclick="ShowSiteMessage('<%#Eval("UserId") %>');">查看</a> </span>
                                      <span class='<%# (bool)Eval("isopenEtao")?"submit_quanxuan":"submit_shanchu" %>'><Hi:ImageLinkButton ID="btnIsOpen" runat="server"  IsShow="true"   CommandName="open"  /> </span>
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
<!--查看分销站点详情-->
<div id="DivShowSiteMessage" style="display:none;">
  <div class="frame-content">
	<table>
  <tr>
    <td class="td_right">分销商名称：<br /></td>
    <td><strong class="colorA"><span id="spanDistributorName" runat="server" /></strong></span></td>
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
    <td><span id="spanAddress" /></td>
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
    <td class="td_right">独立域名</td>
    <td><span id="spanDomain1" /></td>
  </tr>
    </table>
  </div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script language="javascript" type="text/javascript">
    function ShowSiteMessage(userId) { 
     $.ajax({
            url: "ManageEtao.aspx",
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

                    ShowMessageDialog("查看分销站点详情", "sitemessage", "DivShowSiteMessage");

                }

                else { alert("未知错误，没有此分销商的信息");}
            }

        });
    }
</script>
</asp:Content>
