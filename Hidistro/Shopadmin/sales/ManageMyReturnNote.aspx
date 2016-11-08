<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ManageMyReturnNote.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.sales.ManageMyReturnNote" %>
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
    function showRemark(objThis, remark, adminRemark) {
        if (remark == "" && adminRemark == "") {
            $("#spanRemark").html("无备注信息");
        }
        else {
            $("#spanRemark").html("会员退货详情：" + remark + "<br/>" + "管理员备注：" + adminRemark);
        }
        ShowMessageDialog('明细备注', 'detailremark', 'remark');

    }  

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
 <!--选项卡-->
    <div class="optiongroup mainwidth">
	  <ul>
        <li class="optionstar"><a href="ManageMyDebitNote.aspx" class="optionnext"><span>收款单</span></a></li>
        <li><a href="ManageMyRefundNote.aspx" class="optionnext"><span>退款单</span></a></li>
	    <li class="menucurrent"><a href="#" ><span  class="optioncenter">退货单</span></a></li>
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
                <asp:DataList ID="dlstReturnNote" runat="server" DataKeyField="ReturnsId" Width="100%">
                    <HeaderTemplate>
                        <table width="0" border="0" cellspacing="0" style="TABLE-LAYOUT: fixed">
                            <tr class="table_title">
                                <td width="5%" class="td_right td_left">
                                    选择
                                </td>
                                <td width="6%" class="td_right td_left">
                                    退货单号
                                </td>
                                <td width="8%" class="td_right td_left">
                                    订单号
                                </td>
                                <td width="5%" class="td_right td_left">
                                    会员
                                </td>
                                 <td width="20%" class="td_right td_left">
                                    退货备注
                                </td>
                                <td width="8%" class="td_right td_left">
                                    退款金额(元)
                                </td>
                                <td width="6%" class="td_right td_left">
                                    退货时间
                                </td>
                                <td width="10%" class="td_left td_right">
                                    管理员备注
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ReturnsId") %>' />
                            </td>
                            <td>
                                <%# Eval("ReturnsId")%>
                            </td>
                            <td>
                                <%#Eval("OrderId") %>
                            </td>
                            <td>
                                <%# Eval("Username")%>
                            </td>
                            <td style="word-WRAP:break-word">
                                <%# Eval("Comments")%>
                            </td> 
                            <td>
                                <Hi:FormatedMoneyLabel ID="lblOrderTotal" Money='<%#Eval("RefundMoney") %>' runat="server" />
                            </td>                           
                            <td>
                                <%# Eval("HandleTime")%>
                            </td>
                            <%--<td>
                                <%# Eval("Operator")%>
                            </td>--%>
                            <td>
                                <%# Eval("AdminRemark")%>
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
