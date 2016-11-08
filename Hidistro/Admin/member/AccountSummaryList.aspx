<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AccountSummaryList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AccountSummaryList" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
  <div class="title">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1>会员预付款查询</h1>
  <span>查询客户预付款账户的明细资料</span>
</div>

		<!--数据列表区域-->
		<div class="datalist">
        		<!--搜索-->
		<div class="searcharea clearfix br_search">
			<ul>
                 <li>
                <span>会员名称：</span>
				<span>
				    <asp:TextBox ID="txtUserName" runat="server" CssClass="forminput"  />
				</span>
        </li>
        <li>
                <span>会员真实姓名：</span>
                <span><asp:TextBox ID="txtRealName" CssClass="forminput" runat="server" /></span>
          </li>
				<li>
				    <asp:Button runat="server" CssClass="searchbutton" ID="btnQuery" Text="查询" />
				</li>
			</ul>
	</div>
		
<!--结束-->
		
          <div class="functionHandleArea m_none">
		  <!--分页功能-->
		  <div class="pageHandleArea" style="float:left;">
		    <ul>
		      <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
	        </ul>
	      </div>
		  <div class="pageNumber" style="float:right;"> 
		  <div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
            </div>
          </div>
		  <!--结束-->
        </div>
		    <UI:Grid ID="grdMemberAccountList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="UserId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="会员名" SortExpression="UserName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
	                              <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
	                              <Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%# Eval("Wangwang") %>' />	                              
                             </itemtemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="会员真实姓名"  HeaderStyle-CssClass="td_right td_left">
                         <ItemTemplate>&nbsp;
                          <%# Eval("RealName")%>
                           </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="账户总额"  ItemStyle-Width="100px" SortExpression="Balance" HeaderStyle-CssClass="td_right td_left" >                            
                            <itemtemplate>
                                <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%# Eval("Balance") %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="冻结金额"  ItemStyle-Width="100px" SortExpression="RequestBalance" HeaderStyle-CssClass="td_right td_left" >                            
                            <itemtemplate>
                                <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin2" Money='<%# Eval("RequestBalance") %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="可用余额"  ItemStyle-Width="100px" SortExpression="UseableBalance" HeaderStyle-CssClass="td_right td_left" >                            
                            <itemtemplate>
                                <Hi:FormatedMoneyLabel ID="lbUseableBalanceLabel" runat="server" Money='<%# (decimal)Eval("Balance") - (decimal)Eval("RequestBalance") %>' />
                            </itemtemplate>
                        </asp:TemplateField>
                            <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="25%">
                                <ItemTemplate>		                            
			                       <span class="Name  submit_tianjia"><a href="javascript:ShowAddDiv('<%# Eval("UserId") %>', '<%# Eval("UserName") %>', '<%# Eval("Balance") %>', '<%# Eval("RequestBalance") %>');">加款</a></span>
			                       <span class="Name  submit_chakan"><asp:HyperLink runat="server" ID="lkBalanceDetails" Text="明细" NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("/member/BalanceDetails.aspx?userId={0}", Eval("UserId")))%>' /></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                          
                    </Columns>
                </UI:Grid>		 
		  <div class="blank12 clearfix"></div>
</div>
		<!--数据列表底部功能区域-->
  <div class="bottomBatchHandleArea clearfix">
		</div>
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
			</div>
		</div>
	</div>
	
	
	
	
	<div id="addAttribute" style="display:none;">
        <div class="frame-content">
            <p>给“<span id="spUserName"></span>”账户加款 </p>
            <p><span class="frame-span frame-input90">可用余额：</span> <strong class="colorA"><span id="spUseableBalance"></span></strong> 元</p>
            <p><span class="frame-span frame-input90"><em >*</em>&nbsp;加款金额：</span><asp:TextBox ID="txtReCharge" runat="server" CssClass="forminput"></asp:TextBox></p>
            <p><span class="frame-span frame-input90">备注：</span><asp:TextBox ID="txtRemark" runat="server" Width="250px" Height="90px" TextMode="MultiLine" ></asp:TextBox></p>
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

    var formtype = "";

//加款明细
    function ShowAddDiv(userId, userName, balance, freezeBalance) {

        $("#spUserName").html(userName);        
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
