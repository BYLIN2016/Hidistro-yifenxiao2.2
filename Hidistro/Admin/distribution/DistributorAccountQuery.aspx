<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true"  CodeBehind="DistributorAccountQuery.aspx.cs" Inherits="Hidistro.UI.Web.Admin.DistributorAccountQuery" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<!--选项卡-->

	<div class="dataarea mainwidth databody">
	  <div class="title">
      <em><img src="../images/04.gif" width="32" height="32" /></em>
      <h1>分销商预付款</h1>
      <span>分销商预付款明细</span>
      </div>

		<!--数据列表区域-->
	  <div class="datalist">
      		<!--搜索-->
		<div class="searcharea clearfix">
			<ul>
				<li><span>分销商名称：</span><span><asp:TextBox ID="txtUserName" runat="server" CssClass="forminput"   /></span></li>
				<li><span>分销商姓名：</span><span><asp:TextBox ID="txtRealName" CssClass="forminput" runat="server"></asp:TextBox></span></li>
				<li><asp:Button runat="server" CssClass="searchbutton" ID="btnQuery" Text="查询" /></li>
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
		      
      <UI:Grid ID="grdDistributorAccountList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="UserId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        
                        <asp:TemplateField HeaderText="分销商名称" SortExpression="UserName" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="25%">                            
                            <itemtemplate>
	                              <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
	                              <Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%# Eval("Wangwang") %>' />	                              
                             </itemtemplate>
                        </asp:TemplateField>
                              <asp:TemplateField HeaderText="分销商姓名"  HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>&nbsp;
                          <%# Eval("RealName")%>
                           </ItemTemplate>
                   </asp:TemplateField>
                        <asp:TemplateField HeaderText="账户总额"  ItemStyle-Width="100px" SortExpression="Balance" HeaderStyle-CssClass="td_right td_left" >                            
                            <itemtemplate>
                                <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%# Eval("Balance") %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="冻结金额"  ItemStyle-Width="100px" SortExpression="FreezeBalance" HeaderStyle-CssClass="td_right td_left" >                            
                            <itemtemplate>
                                <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" Money='<%# Eval("RequestBalance") %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="可用余额"  ItemStyle-Width="100px" SortExpression="UseableBalance" HeaderStyle-CssClass="td_right td_left" >                            
                            <itemtemplate>
                                <Hi:FormatedMoneyLabel ID="lbUseableBalanceLabel" Money='<%# (decimal)Eval("Balance") - (decimal)Eval("RequestBalance") %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="25%">
                                <ItemTemplate>		                            
			                       <span class="Name  submit_tianjia"><a href="javascript:ShowAddMoney('<%# Eval("UserId") %>', '<%# Eval("UserName") %>', '<%# Eval("Balance") %>', '<%# Eval("RequestBalance") %>')">加款</a><%--<asp:HyperLink runat="server" ID="lkbReCharge" Text="加款" NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("/distribution/DistributorReCharge.aspx?userId={0}", Eval("UserId")))%>' />--%></span>
			                       <span class="Name  submit_chakan"><asp:HyperLink runat="server" ID="lkBalanceDetails" Text="明细" NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("/distribution/DistributorBalanceDetails.aspx?userId={0}", Eval("UserId")))%>' /> </span>
			                       <span class="Name  submit_bianji"><a href="javascript:void(0);" onclick="ShowDistributorAccountSummary('<%# Eval("UserId") %>','<%# Eval("UserName") %>','<%# Eval("Balance") %>','<%# Eval("RequestBalance") %>')">概要</a></span> 
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
    <!--分销商款账户加款-->
	<div id="addAttribute" style="display:none;">
        <div class="frame-content">
            <p>给“<span id="litUserNames"></span>”分销商加款 </p>
            <p><span class="frame-span frame-input90">可用余额：</span> <strong class="colorA"><span id="spUseableBalance"></span></strong> 元</p>
            <p><span class="frame-span frame-input90">加款金额：<em >*</em></span><asp:TextBox ID="txtReCharge" runat="server" CssClass="forminput"></asp:TextBox></p>
            <p><span class="frame-span frame-input90">备注：</span><asp:TextBox ID="txtRemark" runat="server" Width="250px" Height="90px" TextMode="MultiLine" ></asp:TextBox></p>
        </div>
    </div>

<!--分销商款账户概要-->
<div id="ShowDistributorAccountSummary" style="display:none;">
    <div class="frame-content">
        <table>
            <tr>
                <td class="td_right">预付款总额:</td><td><span id="lblAccountAmount" runat="server" /></td>
                <td> <a id="lkbtnBalanceDetail">查看明细</a></td>
                <td></td>
            </tr>
            <tr>
                <td class="td_right">可用余额:</td><td><strong class="colorA fonts"><span id="lblUseableBalance" runat="server" /></strong></td>
                <td><a id="lkbtnRecharge" >预付款加款</a></td>
                <td></td>
            </tr>
            <tr>
                <td class="td_right"><span class="frame-content frame-input90">冻结金额:</span></td>
                <td><span id="lblFreezeBalance" runat="server" /></td>
                <td></td>
            </tr>
            <tr>
                <td class="td_right"><span class="frame-span frame-input90">提现金额：</span></td>
                <td><span id="lblDrawRequestBalance" runat="server" /></td>
                <td><a id="lkbtnDrawRequest" >提现申请</a></td>
            </tr>
        </table>
    </div>
</div>


    <div style="display:none">
        <input type="hidden" id="currentUserId" runat="server" />
    <input type="hidden" id="curentBalance" runat="server" />
    <asp:Button ID="btnAddBalance" runat="server" Text="添加"   CssClass="submit_DAqueding" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function ShowDistributorAccountSummary(userId, userName, accountAmount, freezeBalance) {

        if (freezeBalance == "") {
            $("#ctl00_contentHolder_lblFreezeBalance").html("0.00");
            $("#ctl00_contentHolder_lblDrawRequestBalance").html("0.00");
            $("#ctl00_contentHolder_lblUseableBalance").html(accountAmount);
        }
        else {
            var useableBalance = parseFloat(accountAmount) - parseFloat(freezeBalance);
            $("#ctl00_contentHolder_lblFreezeBalance").html(freezeBalance);
            $("#ctl00_contentHolder_lblDrawRequestBalance").html(freezeBalance);
            $("#ctl00_contentHolder_lblUseableBalance").html(useableBalance);
        }
        
        $("#ctl00_contentHolder_litUser").html(userName);
        $("#ctl00_contentHolder_lblAccountAmount").html(accountAmount);
        
        
        document.getElementById("lkbtnRecharge").setAttribute("href", "DistributorReCharge.aspx?userId=" + userId);
        document.getElementById("lkbtnBalanceDetail").setAttribute("href", "DistributorBalanceDetails.aspx?userId=" + userId);
        document.getElementById("lkbtnDrawRequest").setAttribute("href", "DistributorBalanceDrawRequest.aspx?userId=" + userId);


        ShowMessageDialog("查看分销商概要", "viewdistributor", "ShowDistributorAccountSummary");

    }


    //加款
    function ShowAddMoney(userId, userName, balance, freezeBalance) {
        $("#litUserNames").html(userName);
        $("#spUseableBalance").html(Number(balance) - Number(freezeBalance));
        $("#ctl00_contentHolder_currentUserId").val(userId);
        $("#ctl00_contentHolder_curentBalance").val(balance);
        arrytempstr = null;
        formtype = "addmoney";
        DialogShow("加款操作", "addmoney", "addAttribute", "ctl00_contentHolder_btnAddBalance");
    }

    function validatorForm() {
        switch (formtype) {
            case "addmoney":
                var moneystr = $("#ctl00_contentHolder_txtReCharge").val().replace(/\s/g, "");
                if (moneystr == "" || moneystr == null || moneystr == "undefined") {
                    alert("加款金额不允许为空！");
                    return false;
                }
                setArryText("ctl00_contentHolder_txtReCharge", moneystr);
                setArryText("ctl00_contentHolder_txtRemark", $("#ctl00_contentHolder_txtRemark").val());
                break;
        };
        return true;
    }     
      
</script>
</asp:Content>
