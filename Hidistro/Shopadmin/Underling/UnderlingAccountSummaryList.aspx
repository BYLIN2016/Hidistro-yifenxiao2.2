<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="UnderlingAccountSummaryList.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.UnderlingAccountSummaryList" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
  <div class="toptitle">
  <em><img src="../images/04.gif" width="32" height="32" /></em>
  <h1 class="title_height">会员账户查询</h1>
</div>
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
                <span>真实姓名：</span>
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
		    <UI:Grid ID="grdUnderlingAccountList" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="UserId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
                    <Columns>
                        
                        <asp:TemplateField HeaderText="会员名" SortExpression="UserName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>
	                              <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
	                              <Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%# Eval("Wangwang") %>' />	                              
                             </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="真实姓名" SortExpression="RealName" HeaderStyle-CssClass="td_right td_left">                            
                            <itemtemplate>&nbsp;
	                              <%# Eval("RealName")%>
                             </itemtemplate>
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
			                       <span class="Name  submit_tianjia"><a href="#"  onclick="ShowAddDiv('<%# Eval("UserId") %>', '<%# Eval("UserName") %>', '<%# Eval("Balance") %>', '<%# Eval("RequestBalance") %>');">加款 </a></span>
			                       <span class="Name  submit_chakan"><asp:HyperLink runat="server" ID="lkBalanceDetails" Text="明细" NavigateUrl='<%# "UnderlingBalanceDetails.aspx?userId="+ Eval("UserId")%>' /></span>
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
	
	
	
	
	<div class="Pop_up" id="addAttribute" style=" display:none;">
      <h1>给“<span id="spUserName"></span>”账户加款 </h1>
        <div class="img_datala"><img src="../images/icon_dalata.gif" width="38" height="20" /></div>
      <div class="mianform">
        <ul>
                  <li> <span class="formitemtitle Pw_100">可用余额：</span>
                    <strong class="colorA"><span id="spUseableBalance"></span></strong>
                  </li>
                  <li> <span class="formitemtitle Pw_100">加款金额：<em >*</em></span>
                    <asp:TextBox ID="txtReCharge" runat="server" CssClass="forminput"></asp:TextBox>
                  </li>
                   <li> <span class="formitemtitle Pw_100">备注：</span>
                    <asp:TextBox ID="txtRemark" runat="server" Width="250px" Height="90px" TextMode="MultiLine" ></asp:TextBox>
                  </li>
            </ul>
            <ul class="up Pa_100">
              <asp:Button ID="btnAddBalance" runat="server" Text="添加"  OnClientClick="return PageIsValid();" CssClass="submit_DAqueding" />
            </ul>
      </div>
    </div>
    
    <input type="hidden" id="currentUserId" runat="server" />
    <input type="hidden" id="curentBalance" runat="server" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

<script type="text/javascript" language="javascript">
    function ShowAddDiv(userId, userName, balance, freezeBalance) {
      
        $("#spUserName").html(userName);        
        $("#spUseableBalance").html(Number(balance) - Number(freezeBalance));        
        $("#ctl00_contentHolder_currentUserId").val(userId);
        $("#ctl00_contentHolder_curentBalance").val(balance)

        DivWindowOpen(450, 320, 'addAttribute');
    }   

    
</script>
</asp:Content>
