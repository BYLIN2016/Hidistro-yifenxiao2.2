<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="DistributorRequests.aspx.cs" Inherits="Hidistro.UI.Web.Admin.DistributorRequests" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentHolder" Runat="Server">
<!--选项卡-->
	<!--选项卡-->

	<div class="dataarea mainwidth databody">
			  <div class="title">
  <em><img src="../images/01.gif" width="32" height="32" /></em>
  <h1><table><tr><td>待审核分销商</td><td><a class="help" href="http://video.92hidc.com/video/V2.1/cwfxs.html" title="查看帮助" target="_blank"></a></td></tr></table></h1>
  <span>所有待审核的分销商列表</span>
        </div>
		<!--搜索-->

			<!--结束-->
		
		
		<!--数据列表区域-->
	  <div class="datalist">
	  		<div class="searcharea clearfix">
			<ul>
				<li><span>分销商名称：</span><span><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput" /></span></li>
				<li><span>分销商姓名：</span><span><asp:TextBox ID="txtRealName" runat="server" CssClass="forminput" /></span></li>
				<li><asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="searchbutton"/></li>
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
			</div>

	   <UI:Grid ID="grdDistributorRequests" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="UserId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="分销商名称" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litName" Text='<%#Eval("Username") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                       <asp:TemplateField HeaderText="分销商姓名"  HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>&nbsp;
                          <%# Eval("RealName")%>
                           </ItemTemplate>
                   </asp:TemplateField>
                  <asp:TemplateField HeaderText="电子邮件" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>&nbsp;
                           <asp:Literal ID="litDiscountRate" Text='<%#Eval("Email") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="申请日期" ItemStyle-Width="28%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <asp:Literal ID="litDescription" Text='<%#Eval("CreateDate") %>' runat="server"></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="22%" HeaderStyle-CssClass="td_left td_right_fff">
                        <ItemTemplate>
                             <span class="submit_chakan"><a href="javascript:void(0);" onclick="showMessage('<%#Eval("UserId") %>');">查看</a></span>
                              <span class="submit_quanxuan"><a href="javascript:DialogFrame('<%# "distribution/AcceptDistributorRequest.aspx?userId="+Eval("UserId") %>','审核分销商',900,560)">接受</a> </span>
                              <span class="submit_shanchu"><a href="javascript:void(0);" onclick="ShowRefuseDistrbutorDiv('<%#Eval("UserId") %>','<%#Eval("Username") %>');">拒绝</a></span>
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
<!--拒绝分销商的申请-->

<div id="RefuseDistrbutor" style="display:none;">
    <div class="frame-content">
       <p>拒绝&quot;<span id="litUserName" runat="server" />&quot;分销商的申请</p>
       <p><em>拒绝分销商申请后,该分销商提交的申请信息将被全部删除!</em></p>
    </div>
</div>

<!--分销商信息-->
<div id="showMessage" style="display:none;">
  <div class="frame-content">
	<table>
        <tr>
            <td class="td_right">分销商名称：</td><td colspan="3"><strong class="colorE"><span ID="litName" runat="server"></span></strong></td>
        </tr>
        <tr>
            <td class="td_right">姓名：</td>
            <td><span id="SpanRealName" /></td>
            <td class="td_right">公司名：</td>
            <td><span id="spanCompanyName" /><br /></td>
        </tr>
        <tr>
            <td class="td_right">电子邮件：</td>
            <td><span id="spanEmail" /></td>
            <td class="td_right">地区：<br /></td>
            <td><span id="spanArea" /></td>
        </tr>
        <tr>
            <td class="td_right">详细地址：</td>
            <td colspan="3"><span id="spanAddress" /></td>
        </tr>
        <tr>
            <td class="td_right">QQ：</td>
            <td><span id="spanQQ" /></td>
            <td class="td_right">邮编：</td>
            <td><span id="spanPostCode" /></td>
        </tr>
        <tr>
            <td class="td_right">MSN：</td>
            <td><span id="spanMsn" /></td>
            <td class="td_right">旺旺：</td>
            <td><span id="spanWangwang" /></td>
        </tr>
        <tr>
            <td class="td_right">手机号码：</td>
            <td><span id="spanCellPhone" /></td>
            <td class="td_right">固定电话：</td>
            <td><span id="spanTelephone" /></td>
        </tr>
        <tr>
            <td class="td_right">注册日期：</td>
            <td><span id="spanRegisterDate" /></td>
            <td class="td_right">最后登录日期：</td>
            <td><span id="spanLastLoginDate" /> </td>
        </tr>
    </table>

  </div>
</div>

<div style="display:none">
<input runat="server" type="hidden" id="txtUserId" />
    <asp:Button ID="btnRefuseDistrbutor" Text="拒绝" CssClass="submit_DAqueding" runat="server"/>
</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" Runat="Server">
<script language="javascript" type="text/javascript">
    function ShowRefuseDistrbutorDiv(id, keywords) {
        $("#ctl00_contentHolder_litUserName").html(keywords);
        $("#ctl00_contentHolder_txtUserId").val(id)

        arrytext = null;
        setArryText("ctl00_contentHolder_txtUserId", id);
        DialogShow("拒绝分销商", 'sitemanager', 'RefuseDistrbutor', 'ctl00_contentHolder_btnRefuseDistrbutor');
    }

    function validatorForm() {
        if (!confirm("您确定拒绝该分销商？")) {
            return false;
        }
        var distriutornumber = $("#ctl00_contentHolder_txtUserId").val().replace(/\s/g, "");
        if (distriutornumber.lenght <= 0) {
            alert('分销商不存在！');
            return false;
        }
        return true;
    }

    function showMessage(id) {
        $.ajax({
            url: "DistributorRequests.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {
                isCallback: "true",
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

                    ShowMessageDialog("查看分销商信息", "viewdistributor", "showMessage");
                }

                else {
                    alert("未知错误，没有此分销商的申请信息");
                }
            }

        });
    }  
</script>
</asp:Content>