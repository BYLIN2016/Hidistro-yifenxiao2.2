<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ManageMyDebitNote.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.sales.ManageMyDebitNote" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript">
        function showRemark(objThis, remark) {
            if (remark == "") {
                $("#spanRemark").html("无备注信息");
            }
            else {
                $("#spanRemark").html(remark);
            }
            ShowMessageDialog('明细备注', 'detailremark', 'remark');

        }  
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

<!--选项卡-->
    <div class="optiongroup mainwidth">
	  <ul>
        <li class="menucurrent"><a href="#"><span>收款单</span></a></li>
        <li ><a href="ManageMyRefundNote.aspx"><span>退款单</span></a></li>
	    <li class="optionend"><a href="ManageMyReturnNote.aspx"><span>退货单</span></a></li>
      </ul>
    </div>
    <div class="dataarea mainwidth databody">
            <div class="searcharea clearfix br_search">
                <ul>
                    <li><span>订单编号：</span><span>
                        <asp:TextBox ID="txtOrderId" runat="server" CssClass="forminput" /><asp:Label ID="lblStatus"
                            runat="server" Style="display: none;"></asp:Label>
                    </span></li>
                    <li>
                        <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="查询" />
                    </li>
                </ul>
            </div>
            <!--结束-->
            <div class="functionHandleArea clearfix m_none">
                <!--分页功能-->
                <div class="pageHandleArea">
                    <ul>
                        <li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" />
                        </li>
                    </ul>
                </div>
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                    </div>
                </div>
                <!--结束-->
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span><span class="allSelect">
                            <a href="javascript:void(0)" onclick="SelectAll()">全选</a></span> <span class="reverseSelect">
                                <a href="javascript:void(0)" onclick=" ReverseSelect()">反选</a></span> <span class="delete">
                                    <Hi:ImageLinkButton ID="lkbtnDeleteCheck" runat="server" Text="删除" IsShow="true" />
                                </span></li>
                    </ul>
                </div>
            </div>
            <input type="hidden" id="hidOrderId" runat="server" />
            <!--数据列表区域-->
            <div class="datalist">
                <asp:DataList ID="dlstDebitNote" runat="server" DataKeyField="NoteId" Width="100%">
                    <HeaderTemplate>
                        <table width="0" border="0" cellspacing="0">
                            <tr class="table_title">
                                <td width="6%" class="td_right td_left">
                                    选择
                                </td>
                                <td width="10%" class="td_right td_left">
                                    订单号
                                </td>
                                <td width="10%" class="td_right td_left">
                                    会员用户名
                                </td>
                                <td width="10%" class="td_right td_left">
                                    支付金额(元)
                                </td>
                                <td width="14%" class="td_right td_left">
                                    支付网关费用(元)
                                </td>
                                <td width="10%" class="td_right td_left">
                                    支付方式
                                </td>
                                <td width="10%" class="td_right td_left">
                                    支付时间
                                </td>
                                <td width="10%" class="td_left td_right">
                                    操作员
                                </td>
                                <td width="20%" class="td_left td_right">
                                    备注
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("NoteId") %>' />
                            </td>
                            <td>
                                <%# Eval("OrderId") %>
                            </td>
                            <td>
                                <%# Eval("Username") %>
                            </td>
                            <td>
                                <Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("OrderTotal") %>' runat="server" />
                            </td>
                            <td>
                                <Hi:FormatedMoneyLabel ID="lblPayCharge" Money='<%#Eval("PayCharge") %>' runat="server" />
                            </td>
                            <td>
                                <%# Eval("PaymentType")%>
                            </td>
                             <td>
                                <%# Eval("PayDate")%>
                            </td>
                            <td>
                                <%# Eval("Operator")%>
                            </td>
                            <td>
                               <%# Eval("Remark")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:DataList>
                <div class="blank5 clearfix">
                </div>
            </div>
        <!--数据列表底部功能区域-->
        <div class="page">
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination">
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
    <div id="remark" style="display:none">
      <div class="frame-content">
      <p id="spanRemark" ></p>
      </div>
</div>
</asp:Content>
