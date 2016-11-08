<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true"
    CodeBehind="ManageMyManualPurchaseOrder.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ManageMyManualPurchaseOrder" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="optiongroup mainwidth">
        <ul>
            <li id="anchors0">
                <asp:HyperLink ID="hlinkAllOrder" runat="server"><span>���вɹ���</span></asp:HyperLink></li>
            <li id="anchors1">
                <asp:HyperLink ID="hlinkNotPay" runat="server"><span>�ȴ�����</span></asp:HyperLink></li>
            <li id="anchors2">
                <asp:HyperLink ID="hlinkYetPay" runat="server"><span>�ȴ�����</span></asp:HyperLink></li>
            <li id="anchors3">
                <asp:HyperLink ID="hlinkSendGoods" runat="server"><span>�ѷ���</span></asp:HyperLink></li>
            <li id="anchors5">
                <asp:HyperLink ID="hlinkFinish" runat="server"><span>�ɹ��ɹ���</span></asp:HyperLink></li>
            <li id="anchors4">
                <asp:HyperLink ID="hlinkClose" runat="server"><span>�ѹر�</span></asp:HyperLink></li>
            <li id="anchors99">
                <asp:HyperLink ID="hlinkHistory" runat="server"><span>��ʷ�ɹ���</span></asp:HyperLink></li>
        </ul>
    </div>
    <!--ѡ�-->
    <div class="dataarea mainwidth">
        <!--����-->
        <div class="searcharea clearfix">
            <ul>
                <li><span>ѡ��ʱ��Σ�</span><span>
                    <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="forminput" />
                </span><span class="Pg_1010">��</span> <span>
                    <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="forminput" />
                </span></li>
                <li><span>�ջ������ƣ�</span><span>
                    <asp:TextBox runat="server" ID="txtShipTo" CssClass="forminput" />
                </span></li>
            </ul>
            <div class="blank12 clearfix">
            </div>
            <ul>
                <li><span>�ɹ�����ţ�</span><span>
                    <asp:TextBox ID="txtPurchaseOrderId" runat="server" CssClass="forminput"></asp:TextBox>
                    <asp:Label ID="lblStatus" runat="server" Style="display: none;"></asp:Label>
                </span></li>
                <li>
                    <asp:Button ID="btnSearchButton" runat="server" class="searchbutton" Text="��ѯ" />
                </li>
            </ul>
        </div>
        <!--����-->
        <div class="functionHandleArea clearfix">
            <!--��ҳ����-->
            <div class="pageHandleArea">
                <ul>
                    <li class="paginalNum"><span>ÿҳ��ʾ������</span><UI:PageSize runat="server" ID="hrefPageSize" />
                    </li>
                </ul>
            </div>
            <div class="pageNumber">
                <div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                </div>
            </div>
            <!--����-->
            <div class="blank8 clearfix">
            </div>
            <div class="batchHandleArea">
                <ul>
                    <li class="batchHandleButton">
                        <%-- <span class="signicon"></span><span class="allSelect">
                        <a href="javascript:void(0)" onclick="SelectAll()">ȫѡ</a></span> <span class="reverseSelect">
                            <a href="javascript:void(0)" onclick=" ReverseSelect()">��ѡ</a></span>
                            <Hi:ImageLinkButton
                                ID="btnBatchPayMoney" runat="server" Text="��������" DeleteMsg="����ǰѡ�н����ɸѡ��δ����Ĳɹ���������������Ƿ������"
                                IsShow="true" />--%></li>
                </ul>
            </div>
        </div>
        <input type="hidden" id="hidPurchaseOrderId" runat="server" />
        <!--�����б�����-->
        <div class="datalist">
            <div>
                <asp:DataList ID="dlstPurchaseOrders" runat="server" DataKeyField="PurchaseOrderId"
                    Width="100%">
                    <HeaderTemplate>
                        <table width=" 0" border="0" cellspacing="0">
                            <tr class="table_title">
                                <td width="18%" class="td_right td_left">
                                    �ɹ������
                                </td>
                                <td width="10%" class="td_right td_left">
                                    �ջ���
                                </td>
                                <td width="20%" class="td_right td_left">
                                    �ɹ���ʵ�տ�(Ԫ)
                                </td>
                                <td width="16%" class="td_right td_left">
                                    �ύʱ��
                                </td>
                                <td class="td_right td_left">
                                    �ɹ�״̬
                                </td>                                
                                <td class="td_right td_left">
                                    ����
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>&nbsp;
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("PurchaseOrderId") %>'>
                                <%#Eval("PurchaseOrderId") %>
                                <%# String.IsNullOrEmpty(Eval("ShipOrderNumber").ToString()) ? "" : "<br>��������ţ�" + Eval("ShipOrderNumber")%>
                                <asp:Literal runat="server" ID="litTbOrderDetailLink" />
                            </td>
                            <td>
                                <%#Eval("ShipTo") %>&nbsp;
                            </td>
                            <td>&nbsp;
                                <Hi:FormatedMoneyLabel ID="lblPurchaseTotal" Money='<%#Eval("PurchaseTotal") %>' runat="server" />
                                <span><asp:Literal ID="litPayment" runat="server"></asp:Literal></span>
                            </td>
                            <td>&nbsp;
                                <Hi:FormatedTimeLabel ID="lblPurchaseDate" Time='<%#Eval("PurchaseDate") %>' ShopTime="true"
                                    runat="server"></Hi:FormatedTimeLabel>
                            </td>
                            <td width="21%">
                                <table border="0" style="border: none;" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="border: none;">&nbsp;
                                            <span class="colorE">
                                                <Hi:PuchaseStatusLabel runat="server" ID="lblPurchaseStatus" PuchaseStatusCode='<%# Eval("PurchaseStatus") %>' Font-Bold="true" /></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border: none;">&nbsp;
                                            <span class="Name float">
                                                <a target="_blank" href='<%# "ManualPurchaseOrderDetails.aspx?purchaseOrderId="+Eval("PurchaseOrderId") %>'>����</a>
                                            </span> 
                                            <span>
                                               <Hi:DistributorChangePurchaseOrderItemsHyperLink ID="lkbtnUpdatePurchaseOrder" PurchaseStatusCode='<%# Eval("PurchaseStatus") %>'
                                                            PurchaseOrderId='<%# Eval("PurchaseOrderId") %>' Target="_blank" Text="�޸�" runat="server" />
                                            </span>                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>   
                            <td width="20%">
                                <span class="submit_faihuo">
                                    <asp:HyperLink ID="lkbtnPay" runat="server" NavigateUrl='<%# "ChoosePayment.aspx?PurchaseOrderId="+ Eval("PurchaseOrderId")+"&PayMode="+Eval("PaymentTypeId") %>'
                                        Target="_blank" Text="����" Visible="false" /></span> <span class="submit_tongyi">
                                            <div runat="server" visible="false" id="lkbtnClosePurchaseOrder">
                                                <a href="javascript:void(0);" onclick="ShowCloseDiv('<%#Eval("PurchaseOrderId") %>')"
                                                    id="btnClose">ȡ���ɹ�</a></div>
                                        </span>&nbsp;
                                <Hi:ImageLinkButton ID="lkbtnConfirmPurchaseOrder" IsShow="true" runat="server" Text="ȷ�ϲɹ���"
                                    CommandArgument='<%# Eval("PurchaseOrderId") %>' CommandName="FINISH_TRADE" DeleteMsg="ȷ��Ҫ��ɸòɹ�����"
                                    Visible="false" ForeColor="Red" />
                                <a href="javascript:void(0)" onclick="return ApplyForPurchaseRefund(this.title)"
                                    runat="server" id="lkbtnApplyForPurchaseRefund" visible="false" title='<%# Eval("PurchaseOrderId") %>'>
                                    �����˿�</a><br />
                                <a href="javascript:void(0)" onclick="return ApplyForPurchaseReturn(this.title)"
                                    runat="server" id="lkbtnApplyForPurchaseReturn" visible="false" title='<%# Eval("PurchaseOrderId")%> '>
                                    �����˻�</a> <a href="javascript:void(0)" onclick="return ApplyForPurchaseReplace(this.title)"
                                        runat="server" id="lkbtnApplyForPurchaseReplace" visible="false" title='<%# Eval("PurchaseOrderId")%> '>
                                        ���뻻��</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:DataList>
            </div>
        </div>
        <div class="blank5 clearfix">
        </div>
        <!--�����б�ײ���������-->
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="databottom">
    </div>
    <!--�رղɹ���-->
    <div class="Pop_up" id="ClosePurchaseOrder" style="display: none;">
        <h1>
            �رղɹ���
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform fonts colorA borbac">
            <strong>ȡ���ɹ���?��ѡ��ȡ���ɹ��������ɣ�</strong></div>
        <div class="mianform ">
            <ul>
                <li><span class="formitemtitle Pw_160">ȡ���òɹ��������ɣ�</span>
                    <abbr class="formselect">
                        <Hi:DistributorClosePurchaseOrderReasonDropDownList runat="server" ID="ddlCloseReason" />
                    </abbr>
                </li>
            </ul>
            <ul class="up Pa_160">
                <asp:Button ID="btnClosePurchaseOrder" runat="server" CssClass="submit_DAqueding"
                    OnClientClick="return ValidationCloseReason()" Text="ȷ ��" />
            </ul>
        </div>
    </div>
    <div class="Pop_up" id="refund_div" style="display: none">
        <h1>
            �����˿�
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform">
            <ul>
                <li>1.����Ҫѡ��������ν��˿�ظ��������ѡ��Ԥ�����˵�Ԥ�����ѡ������ת�ˣ�����д�����˺ŵ������Ϣ��</li>
                <li><span class="formitemtitle Pw_160">ѡ���˿�;����<em>*</em></span> <span>
                    <abbr class="formselect">
                        <asp:DropDownList ID="dropRefundType" runat="server">
                            <asp:ListItem Value="1">�˵�Ԥ���</asp:ListItem>
                            <asp:ListItem Value="2">����ת��</asp:ListItem>
                        </asp:DropDownList>
                    </abbr>
                </span></li>
                <li><span class="formitemtitle Pw_160">����ԭ���Լ��˿��ʺţ�<em>*</em><br />
                    ������ת�ˣ�����д���������˺������Ϣ��</span> <span>
                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" CssClass="forminput"
                            Width="300px" Height="70px"></asp:TextBox></span> </li>
                <li style="text-align: center;">
                    <asp:Button ID="btnOk" runat="server" Text="ȷ ��" OnClientClick="return validatorForm()" CssClass="submit_DAqueding" /></li>
            </ul>
        </div>
    </div>
    <div class="Pop_up" id="return_div" style="display: none">
        <h1>
            �����˻�
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform">
            <ul>
                <li>1.�����˻�ǰ��ͷ�ȷ���˻����Ž���Ʒ�ĳ�����ذ��������ź�������˾��д�ڱ�ע���档<br />
                    2������Ҫѡ��������ν��˿�ظ��������ѡ��Ԥ�����˵�Ԥ�����ѡ������ת�ˣ�����д�����˺ŵ������Ϣ</li>
                <li><span class="formitemtitle Pw_160">ѡ���˿�;����<em>*</em></span> <span>
                    <abbr class="formselect">
                        <asp:DropDownList ID="dropReturnRefundType" runat="server">
                            <asp:ListItem Value="1">�˵�Ԥ���</asp:ListItem>
                            <asp:ListItem Value="2">����ת��</asp:ListItem>
                        </asp:DropDownList>
                    </abbr>
                </span></li>
                <li><span class="formitemtitle Pw_160">����ԭ��������˾���������ţ��˿��˺ŵȣ�<em>*</em><br />
                    ������ת�ˣ�����д���������˺������Ϣ��</span> <span>
                        <asp:TextBox ID="txtReturnRemark" runat="server" TextMode="MultiLine" CssClass="forminput"
                            Width="300px" Height="70px"></asp:TextBox></span> </li>
                <li style="text-align: center;">
                    <asp:Button ID="btnReturn" runat="server" Text="ȷ ��" OnClientClick="return validatorForm()" CssClass="submit_DAqueding" /></li>
            </ul>
        </div>
    </div>
    <div class="Pop_up" id="replace_div" style="display: none">
        <h1>
            ���뻻��
        </h1>
        <div class="img_datala">
            <img src="../images/icon_dalata.gif" width="38" height="20" /></div>
        <div class="mianform">
            <ul>
                <li>1.���ڻ���ǰ��ϸ�Ķ�����˵������ͷ�ȷ�ϻ������Ž���Ʒ�ĳ�����ذ��������ź�������˾��д�ڱ�ע����</li>
                <li><span class="formitemtitle Pw_160">������ע��<em>*</em></span> <span>
                    <asp:TextBox ID="txtReplaceRemark" runat="server" TextMode="MultiLine" CssClass="forminput"
                        Width="300px" Height="70px"></asp:TextBox></span> </li>
                <li style="text-align: center;">
                    <asp:Button ID="btnReplace" runat="server" Text="ȷ ��" OnClientClick="return validatorForm()" CssClass="submit_DAqueding" /></li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
    <script type="text/javascript">

        function showOrderState() {
            var status;
            if (navigator.appName.indexOf("Explorer") > -1) {

                status = document.getElementById("ctl00_contentHolder_lblStatus").innerText;

            } else {

                status = document.getElementById("ctl00_contentHolder_lblStatus").textContent;

            }
            if (status != "0") {
                document.getElementById("anchors0").className = 'optionstar';
            }
            if (status != "99") {
                document.getElementById("anchors99").className = 'optionend';
            }
            document.getElementById("anchors" + status).className = 'menucurrent';
        }

        $(document).ready(function () { showOrderState(); });

        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
            if (reason == "��ѡ��ȡ��������") {
                alert("��ѡ��ȡ��������");
                return false;
            }

            return true;
        }
        var type = "";
        function validatorForm() {
            if (type == "refund") {
                if ($("#ctl00_contentHolder_txtRemark").val().replace(/\s/g, "") == "") {
                    alert("����������ԭ���Լ��˿��ʺ�");
                    return false;
                }
            }
            else if (type == "return") {
                if ($("#ctl00_contentHolder_txtReturnRemark").val().replace(/\s/g, "") == "") {
                    alert("����������ԭ��������˾���������ţ��˿��˺ŵ�");
                    return false;
                }
            }
            else if (type == "replace") {
                if ($("#ctl00_contentHolder_txtReplaceRemark").val().replace(/\s/g, "") == "") {
                    alert("�����뻻����ע");
                    return false;
                }
            }
            else if (type == "pay") {
                if ($("#ctl00_contentHolder_dropPayType").val() == "") {
                    alert("��ѡ��֧����ʽ");
                    return false;
                }
            }
            return true;
        }
        function ApplyForPurchaseRefund(purchaseOrderId) {
            type = "refund";
            $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
            DivWindowOpen(600, 400, "refund_div");
            return false;
        }
        function ApplyForPurchaseReturn(purchaseOrderId) {
            type = "return";
            $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
            DivWindowOpen(600, 400, "return_div");
            return false;
        }
        function ApplyForPurchaseReplace(purchaseOrderId) {
            type = "replace";
            $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
            DivWindowOpen(600, 400, "replace_div");
            return false;
        }
        function ShowCloseDiv(purchaseOrderId) {

            $("#ctl00_contentHolder_hidPurchaseOrderId").val(purchaseOrderId);
            DivWindowOpen(550, 200, 'ClosePurchaseOrder');
        }

     
                 
    </script>
</asp:Content>
